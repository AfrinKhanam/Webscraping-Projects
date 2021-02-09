using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using IndianBank_ChatBOT.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IndianBank_ChatBOT.Controllers
{
    public class StaticFilesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppSettings _appSettings;

        private static bool _isStaticFilesScrapingInProgress = false;

        public StaticFilesController(AppDbContext dbContext, IOptions<AppSettings> appsettings)
        {
            _dbContext = dbContext;
            _appSettings = appsettings.Value;
        }

        #region Public Methods

        [Authorize]
        public IActionResult GetPageConfigById(int id)
        {
            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    var staticPage = _dbContext.StaticPages.FirstOrDefault(s => s.Id == id);
                    if (staticPage != null)
                    {
                        return Ok(staticPage);
                    }
                }
                return NotFound($"Static Page with the id {id} not found!");
            }
            return BadRequest("Invalid Input");
        }

        [Authorize]
        public IActionResult GetAllStaticFileInfo()
        {
            var pageConfigarations = _dbContext.StaticPages.ToList();
            var configObjects = new List<object>();
            foreach (var item in pageConfigarations)
            {
                configObjects.Add(JsonConvert.DeserializeObject(item.PageConfig));
            }
            return Ok(configObjects);
        }

        [AllowAnonymous]
        public IActionResult GetAllStaticFileInfoAsText()
        {
            var staticPages = _dbContext.StaticPages.ToList();
            var sb = new StringBuilder();
            sb.Append("{");

            for (int i = 0; i < staticPages.Count; i++)
            {
                var trimedFirstCharString = staticPages[i].PageConfig;
                if (!string.IsNullOrEmpty(trimedFirstCharString))
                {
                    trimedFirstCharString = trimedFirstCharString.Substring(1);

                    string trimmedLastCharString = trimedFirstCharString.Remove(trimedFirstCharString.Length - 1, 1);

                    sb.Append(trimmedLastCharString);
                    if (i != staticPages.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.Append("}");
            var dataString = sb.ToString();
            return Ok(dataString);
        }

        [Authorize]
        public ActionResult Index()
        {
            var staticPages = _dbContext.StaticPages.Include(l => l.WebPageLanguage).OrderByDescending(f => f.CreatedOn).ToList();
            StaticPageViewModel vm = new StaticPageViewModel
            {
                StaticPages = staticPages,
                IsStaticFilesScrapingInProgress = _isStaticFilesScrapingInProgress
            };

            return View(vm);
        }

        [HttpGet]
        public ContentResult GetStaticFileContent(int staticFileId)
        {
            var staticFile = _dbContext.StaticPages.FirstOrDefault(file => file.Id == staticFileId);

            if (staticFile != null)
            {
                var htmlSanitizer = new HtmlSanitizer();
                var sanitizedHtml = htmlSanitizer.Sanitize(ToBase64Decode(staticFile.FileData));

                if (!string.IsNullOrWhiteSpace(staticFile.FileData))
                    return Content(sanitizedHtml, "text/html");
            }
            var result = new ContentResult
            {
                StatusCode = 404,
                Content = "Page not found!"
            };

            return result;
        }

        [Authorize]
        [HttpPost]
        public ActionResult DownLoadStaticFile(int staticFileId)
        {
            var staticFile = _dbContext.StaticPages.FirstOrDefault(file => file.Id == staticFileId);
            if (staticFile != null && !string.IsNullOrWhiteSpace(staticFile.FileData))
            {
                var bytes = Convert.FromBase64String(staticFile.FileData);

                return File(bytes, "text/html", staticFile.FileName);
            }
            return NotFound($"File with the Id {staticFileId} is not found");
        }


        [Authorize]
        [HttpPost]
        public IActionResult FileUpload([FromForm] IFormFile file, [FromQuery] int languageId)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    return BadRequest("Please select a file to upload");
                }
                else
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(file.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = string.Concat(fileName, fileExtension);

                    if (fileExtension != ".html")
                    {
                        return BadRequest("Invalid File type! Please upload a Html File.");
                    }

                    var hasFileWithSameName = _dbContext.StaticPages.Any(file => file.FileName.ToLower() == fileName.ToLower());

                    if (hasFileWithSameName)
                    {
                        return BadRequest("file already exist with the same name. Please Use different file");
                    }

                    var staticPage = new StaticPage
                    {
                        Id = 0,
                        PageConfig = "",
                        FileName = fileName,
                        CreatedOn = DateTime.Now,
                        FileType = fileExtension,
                        ScrapeStatus = ScrapeStatus.YetToScrape,
                        ErrorMessage = string.Empty,
                        IsActive = true,
                        LanguageId = languageId
                    };

                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        staticPage.FileData = Convert.ToBase64String(target.ToArray());
                    }

                    var contentResult = Content(ToBase64Decode(staticPage.FileData), "text/html");

                    var isSafeFile = IsFileHasDangerousContent(contentResult.Content);

                    if (isSafeFile)
                    {
                        try
                        {
                            var htmlSanitizer = new HtmlSanitizer();
                            var sanitizedHtml = htmlSanitizer.Sanitize(contentResult.Content);

                            staticPage.FileData = EncodeToBase64(sanitizedHtml);

                            _dbContext.StaticPages.Add(staticPage);
                            _dbContext.SaveChanges();

                            var appBaseUrl = _appSettings.ChatBotBackEndUIEndPoint;

                            var staticFile = _dbContext.StaticPages.FirstOrDefault(s => s.Id == staticPage.Id);
                            var staticFileContentUrl = appBaseUrl + "/" + "StaticFiles" + "/" + nameof(GetStaticFileContent) + "?staticFileId=" + staticPage.Id + "&languageId=" + staticPage.LanguageId;
                            staticFile.PageUrl = staticFileContentUrl;
                            _dbContext.StaticPages.Update(staticFile);
                            _dbContext.SaveChanges();

                            return Ok("File uploaded successfully ");
                        }
                        catch
                        {
                            return BadRequest("Failed to process your file. Please try again later.");
                        }
                    }
                    else
                    {
                        return BadRequest("Suspicious file content found! Please upload a plain HTML");
                    }
                }
            }
            return BadRequest("Invalid input!");
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdatePageConfigById([FromBody] PageConfigViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Id != 0)
                {
                    var staticPage = _dbContext.StaticPages.FirstOrDefault(s => s.Id == vm.Id);
                    if (staticPage != null)
                    {
                        staticPage.PageConfig = vm.PageConfig;
                        _dbContext.StaticPages.Update(staticPage);
                        _dbContext.SaveChanges();
                        return Ok();
                    }
                }
                return NotFound($"Static Page with the id {vm.Id } not found!");
            }
            return BadRequest("Invalid Input");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteStaticFileById(int id)
        {
            if (ModelState.IsValid)
            {
                var staticPage = _dbContext.StaticPages.FirstOrDefault(f => f.Id == id);
                if (staticPage != null)
                {
                    _dbContext.StaticPages.Remove(staticPage);
                    _dbContext.SaveChanges();
                    return Ok();
                }
                return NotFound($"File with the Id {id} is not found");
            }
            return BadRequest("Invalid Input");
        }

        [AllowAnonymous]
        [HttpPut]
        public IActionResult UpdateStatus(StaticPage vm)
        {
            var staticFile = _dbContext.StaticPages.FirstOrDefault(s => s.Id == vm.Id);
            if (staticFile != null)
            {
                staticFile.ErrorMessage = vm.ErrorMessage;
                staticFile.ScrapeStatus = vm.ScrapeStatus;
                staticFile.LastScrapedOn = DateTime.Now;
                _dbContext.StaticPages.Update(staticFile);
                _dbContext.SaveChanges();
                return Ok();
            }
            return NotFound($"Static Page with the Id {vm.Id} is not found");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RescrapeAllStaticPages()
        {
            string rescrapeAllStaticPagesEndPoint = _appSettings.RescrapeAllStaticPagesEndPoint;

            if (!string.IsNullOrEmpty(rescrapeAllStaticPagesEndPoint))
            {
                try
                {
                    ResetStaticPageScrapeStatus();
                    _isStaticFilesScrapingInProgress = true;

                    using var client = new HttpClient
                    {
                        BaseAddress = new Uri(rescrapeAllStaticPagesEndPoint)
                    };

                    var response = await client.GetAsync("");
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(responseContent);
                    }
                    else
                    {
                        return BadRequest($"Failed to Start the Static Page Web Scraping. Error : {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    _isStaticFilesScrapingInProgress = false;
                    return BadRequest($"Failed to Start the Static Page Web Scraping. Error : {ex.Message} ");
                }
            }
            return BadRequest("Static Page Web Scrape Url not found. Please check the configuration");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult OnScrapingCompleted()
        {
            _isStaticFilesScrapingInProgress = false;
            return Ok(_isStaticFilesScrapingInProgress);
        }

        [Authorize]
        [HttpGet]
        public IActionResult IsStaticPagesWebScrapingInProgress()
        {
            return Ok(_isStaticFilesScrapingInProgress);
        }

        #endregion

        #region Private Methods
        private string ToBase64Decode(string base64EncodedText)
        {
            if (string.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private bool IsFileHasDangerousContent(string fileContent)
        {
            string[] suspiciousContents = _appSettings.StaticFileSuspiciousContentsCSV.Split(',');

            foreach (string x in suspiciousContents)
            {
                if (fileContent.Contains(x))
                {
                    return false;
                }
            }
            return true;
        }

        private string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private void ResetStaticPageScrapeStatus()
        {
            var staticPages = _dbContext.StaticPages.ToList();

            foreach (var webPage in staticPages)
            {
                webPage.ScrapeStatus = ScrapeStatus.YetToScrape;
                webPage.ErrorMessage = string.Empty;
            }

            _dbContext.StaticPages.UpdateRange(staticPages);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
