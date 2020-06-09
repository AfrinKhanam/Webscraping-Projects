﻿using AdaptiveExpressions;
using Antlr4.Runtime.Tree;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
    public class StaticFilesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public StaticFilesController(AppDbContext dbContext, IOptions<AppSettings> appsettings)
        {
            _dbContext = dbContext;
            _appSettings = appsettings.Value;
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
            var pageConfigarations = _dbContext.StaticPages.Where(s => s.ScrapeStatus == ScrapeStatus.YetToScrape).ToList();
            var configObjects = new List<object>();
            foreach (var item in pageConfigarations)
            {
                configObjects.Add(JsonConvert.DeserializeObject(item.PageConfig));
            }
            return Ok(configObjects);
        }

        public IActionResult GetAllStaticFileInfoAsText()
        {
            var staticPages = _dbContext.StaticPages.Where(s => s.ScrapeStatus == ScrapeStatus.YetToScrape).ToList();
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

        public ActionResult Index()
        {
            var files = _dbContext.StaticPages.OrderByDescending(f => f.CreatedOn).ToList();
            return View(files);
        }

        private string ToBase64Decode(string base64EncodedText)
        {
            if (string.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }


        [HttpGet]
        public ContentResult GetStaticFileContent(int staticFileId)
        {
            var staticFile = _dbContext.StaticPages.FirstOrDefault(file => file.Id == staticFileId);

            if (staticFile != null && !string.IsNullOrWhiteSpace(staticFile.FileData))
            {
                return Content(ToBase64Decode(staticFile.FileData), "text/html");
            }

            return null;
        }

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

        [HttpPost]
        public IActionResult FileUpload(IFormFile file)
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
                        ScrapeStatus = ScrapeStatus.YetToScrape
                    };

                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        staticPage.FileData = Convert.ToBase64String(target.ToArray());
                    }

                    _dbContext.StaticPages.Add(staticPage);
                    _dbContext.SaveChanges();

                    var appBaseUrl = _appSettings.ChatBotBackEndUIEndPoint;

                    var staticFile = _dbContext.StaticPages.FirstOrDefault(s => s.Id == staticPage.Id);
                    var staticFileContentUrl = appBaseUrl + "/" + "StaticFiles" + "/" + nameof(GetStaticFileContent) + "?staticFileId=" + staticPage.Id;
                    staticFile.PageUrl = staticFileContentUrl;
                    _dbContext.StaticPages.Update(staticFile);
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

        [HttpPut]
        public IActionResult UpdateStatus(StaticPage vm)
        {
            var staticFile = _dbContext.StaticPages.FirstOrDefault(s => s.Id == vm.Id);
            // var staticFile = _dbContext.WebPageScrapeRequests.FirstOrDefault(s => s.Id == vm.Id);
            if (staticFile != null)
            {
                staticFile.ScrapeStatus = vm.ScrapeStatus;
                staticFile.CreatedOn = vm.CreatedOn;
                _dbContext.StaticPages.Update(staticFile);
                _dbContext.SaveChanges();
                return Ok();
            }
            return NotFound($"Static Page with the Id {vm.Id} is not found");
        }
    }
}
