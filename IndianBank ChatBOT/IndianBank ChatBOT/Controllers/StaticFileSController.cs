using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
    public class StaticFilesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StaticFilesController(AppDbContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

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

        public IActionResult GetAllStaticFileInfoAsText()
        {
            var pageConfigarations = _dbContext.StaticPages.ToList();
            var sb = new StringBuilder();
            sb.Append("{");

            for (int i = 0; i < pageConfigarations.Count; i++)
            {
                var trimedFirstCharString = pageConfigarations[i].PageConfig;
                trimedFirstCharString = trimedFirstCharString.Substring(1);

                string trimmedLastCharString = trimedFirstCharString.Remove(trimedFirstCharString.Length - 1, 1);

                sb.Append(trimmedLastCharString);
                if (i != pageConfigarations.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
            var dataString = sb.ToString();
            return Ok(dataString);
        }

        public ActionResult Index()
        {
            var files = _dbContext.StaticPages.ToList();
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    return BadRequest("Please select a file to upload");
                }
                else
                {
                    var uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "WebScrapingStaticFiles");

                    var fileToAdd = Path.Combine(uploadFolderPath, file.FileName);

                    if (System.IO.File.Exists(fileToAdd))
                    {
                        return BadRequest("file already exist with the same name. Use different file");
                    }

                    var filePath = Path.Combine(uploadFolderPath, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var staticPage = new StaticPage
                    {
                        EncodedPageUrl = System.Net.WebUtility.UrlEncode(GetStaticPageUrl(file.FileName)),
                        Id = 0,
                        PageConfig = "",
                        FileName = file.FileName,
                        PageUrl = GetStaticPageUrl(file.FileName)
                    };

                    _dbContext.StaticPages.Add(staticPage);
                    _dbContext.SaveChanges();

                    return Ok("File uploaded successfully ");
                }
            }
            return BadRequest("Invalid Input");
        }

        [HttpGet]
        public IActionResult UpdatePageConfigById(int id, string pageConfig)
        {
            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    var staticPage = _dbContext.StaticPages.FirstOrDefault(s => s.Id == id);
                    if (staticPage != null)
                    {
                        staticPage.PageConfig = pageConfig;
                        _dbContext.StaticPages.Update(staticPage);
                        _dbContext.SaveChanges();
                        return Ok();
                    }
                }
                return NotFound($"Static Page with the id {id} not found!");
            }
            return BadRequest("Invalid Input");
        }

        private string GetStaticPageUrl(string fileName)
        {
            var appBaseUrl = AppHttpContext.AppBaseUrl;
            var urlPath = Path.Combine(appBaseUrl, "WebScrapingStaticFiles");
            var staticFilePath = Path.Combine(urlPath, fileName);
            return staticFilePath;
        }

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

                    string projectRootPath = _hostingEnvironment.WebRootPath;
                    var folderPath = Path.Combine(projectRootPath, "WebScrapingStaticFiles");
                    var fileToDelete = Path.Combine(folderPath, staticPage.FileName);
                    if (System.IO.File.Exists(fileToDelete))
                    {
                        System.IO.File.Delete(fileToDelete);
                        return Ok();
                    }
                }
                return NotFound($"File with the Id {id} is not found");
            }
            return BadRequest("Invalid Input");
        }
    }
}
