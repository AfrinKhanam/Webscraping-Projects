using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Dialogs.Debugging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace IndianBank_ChatBOT.Controllers
{
    [Route("[controller]")]
    public class WebPageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public WebPageController(IOptions<AppSettings> appsettings, AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
        }

        [HttpGet]
        [Route(nameof(Index))]
        public ActionResult Index()
        {
            var webPages = _dbContext.WebScapeConfig.OrderBy(p => p.PageName).ToList();
            return View(webPages);
        }

        [HttpGet]
        [Route(nameof(AddNew))]
        public ActionResult AddNew()
        {
            var webPage = new WebScapeConfig();
            return View(webPage);
        }

        [HttpPost]
        [Route(nameof(AddNew))]
        public ActionResult AddNew(WebScapeConfig webPage)
        {
            webPage.ScrapeStatus = ScrapeStatus.YetToScrape;
            webPage.CreatedOn = DateTime.Now;
            webPage.LastScrapedOn = null;
            webPage.IsActive = true;

            _dbContext.WebScapeConfig.Add(webPage);
            _dbContext.SaveChanges();

            ViewBag.insertWebPageStatus = "New Web Page is added successfully.";
            ModelState.Clear();

            return View();
        }


        [HttpGet]
        [Route(nameof(Edit))]
        public ActionResult Edit(int pageId)
        {
            var webPage = _dbContext.WebScapeConfig.Find(pageId);
            return View(webPage);
        }

        [HttpPost]
        [Route(nameof(Edit))]
        public ActionResult Edit(WebScapeConfig webPage)
        {
            _dbContext.WebScapeConfig.Update(webPage);
            _dbContext.SaveChanges();

            ViewBag.editWebPageStatus = "Web Page is updates successfully.";
            ModelState.Clear();

            return View(webPage);
        }


        [HttpDelete]
        [Route(nameof(DeleteById))]
        public IActionResult DeleteById(int pageId)
        {
            if (pageId != 0)
            {
                var webpage = _dbContext.WebScapeConfig.FirstOrDefault(w => w.Id == pageId);
                if (webpage != null)
                {
                    _dbContext.WebScapeConfig.Remove(webpage);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Web Page with the id {pageId} not found!");
        }

        [HttpGet]
        [Route(nameof(GetAllPages))]
        public IActionResult GetAllPages(bool isActive = true)
        {
            var webPages = _dbContext.WebScapeConfig.Where(w => w.IsActive == isActive).ToList();
            return Ok(webPages);
        }

        [HttpGet]
        [Route(nameof(GetPageById))]
        public IActionResult GetPageById(int pageId)
        {
            var webPage = _dbContext.WebScapeConfig.Find(pageId);
            return Ok(webPage);
        }


        // [HttpPut]
        // [Route(nameof(UpdateStatus))]
        // public IActionResult UpdateStatus([FromBody] WebScapeConfig vm)
        // {
        //     Console.WriteLine("000000000000000000", vm.Id);
        //     var webPage = _dbContext.WebScapeConfig.FirstOrDefault(s => s.Id == vm.Id);
        //     if (webPage != null)
        //     {
        //         webPage.ErrorMessage = vm.ErrorMessage;
        //         webPage.ScrapeStatus = vm.ScrapeStatus;
        //         webPage.LastScrapedOn = vm.LastScrapedOn;
        //         _dbContext.WebScapeConfig.Update(webPage);
        //         _dbContext.SaveChanges();
        //         return Ok();
        //     }
        //     return NotFound($"Web Page with the Id {vm.Id} is not found");
        // }
        [HttpPut]
        [Route(nameof(UpdateStatus))]
        public IActionResult UpdateStatus(int Id, ScrapeStatus ScrapeStatus, string ErrorMessage=null)
        { 
            var webPage = _dbContext.WebScapeConfig.FirstOrDefault(s => s.Id == Id);
            if (webPage != null)
            {
                webPage.ErrorMessage = ErrorMessage;
                webPage.ScrapeStatus = ScrapeStatus;
                webPage.LastScrapedOn = DateTime.Now;
                _dbContext.WebScapeConfig.Update(webPage);
                _dbContext.SaveChanges();
                return Ok();
            }
            return NotFound($" Web Page with the Id {Id} is not found");
        }

        [HttpPost]
        [Route(nameof(RescrapeAllPages))]
        public IActionResult RescrapeAllPages()
        {
            string WebscrapeUrl = _appSettings.WebscrapeUrl;
            if (!string.IsNullOrEmpty(WebscrapeUrl))
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(WebscrapeUrl)
                };

                var responseTask = client.GetAsync("");
                responseTask.Wait();

                var result = responseTask.Result;
                return Ok();
            }
            return BadRequest("Web Scrape Url not found. Please check the configuration");
        }
    }
}
