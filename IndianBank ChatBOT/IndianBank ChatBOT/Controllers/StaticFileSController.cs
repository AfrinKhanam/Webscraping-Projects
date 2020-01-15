using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class StaticFilesController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IHostingEnvironment _hostingEnvironment;

        public StaticFilesController(AppDbContext dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
            _hostingEnvironment = hostingEnvironment;
        }

        private List<StaticFile> GetAllStaticFiles()
        {
            var appBaseUrl = AppHttpContext.AppBaseUrl;
            var urlPath = Path.Combine(appBaseUrl, "WebScrapingStaticFiles");
            var files = new List<StaticFile>();
            string projectRootPath = _hostingEnvironment.WebRootPath;
            var folderPath = Path.Combine(projectRootPath, "WebScrapingStaticFiles");

            foreach (string path in System.IO.Directory.EnumerateFiles(folderPath, "*.html"))
            {
                FileInfo fileInfo = new FileInfo(path);
                var p = Path.Combine(urlPath, fileInfo.Name);
                files.Add(new StaticFile { FileName = fileInfo.Name, Path = p });
            }
            return files;
        }

        public IActionResult GetAllStaticFileInfo()
        {
            var files = GetAllStaticFiles();
            //files.ForEach(f => f.Path = System.Net.WebUtility.UrlEncode(f.Path));
            return Ok(files);
        }

        public ActionResult Index()
        {
            var files = GetAllStaticFiles();
            files.ForEach(f => f.Path = System.Net.WebUtility.UrlEncode(f.Path));
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
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
                return Ok("File uploaded successfully ");
            }
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            string projectRootPath = _hostingEnvironment.WebRootPath;
            var folderPath = Path.Combine(projectRootPath, "WebScrapingStaticFiles");
            var fileToDelete = Path.Combine(folderPath, fileName);
            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);
                return Ok();
            }

            return NotFound($"File not found {fileName}");
        }
    }
}
