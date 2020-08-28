using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IndianBank_ChatBOT.Controllers
{
    [Route("[controller]")]
    public class WebPageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppSettings _appSettings;

        private static bool _isFullWebScrapingInProgress = false;

        public WebPageController(IOptions<AppSettings> appsettings, AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
        }

        #region Private Methods

        [Authorize]
        [HttpGet]
        [Route(nameof(Index))]
        public ActionResult Index()
        {
            var webPages = _dbContext.WebScapeConfig.OrderBy(p => p.PageName).ToArray();

            WebPageViewModel vm = new WebPageViewModel
            {
                IsFullWebScrapingInProgress = _isFullWebScrapingInProgress,
                WebScapeConfigs = webPages
            };

            return View(vm);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(AddNew))]
        public ActionResult AddNew()
        {
            var webPage = new WebScapeConfig();
            return View(webPage);
        }

        [Authorize]
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

        [Authorize]
        [HttpGet]
        [Route(nameof(Edit))]
        public ActionResult Edit(int pageId)
        {
            var webPage = _dbContext.WebScapeConfig.Find(pageId);
            return View(webPage);
        }

        [Authorize]
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

        [Authorize]
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

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllPages))]
        public IActionResult GetAllPages(bool isActive = true)
        {
            var webPages = _dbContext.WebScapeConfig.Where(w => w.IsActive == isActive).ToArray();

            return Ok(webPages);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(GetPageById))]
        public IActionResult GetPageById(int pageId)
        {
            var webPage = _dbContext.WebScapeConfig.Find(pageId);

            return Ok(webPage);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route(nameof(UpdateStatus))]
        public IActionResult UpdateStatus(int Id, ScrapeStatus ScrapeStatus, string ErrorMessage = null)
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

        [Authorize]
        [HttpPost]
        [Route(nameof(RescrapeAllPages))]
        public async Task<IActionResult> RescrapeAllPages()
        {
            string rescrapeAllPagesEndPoint = _appSettings.RescrapeAllPagesEndPoint;

            if (!string.IsNullOrEmpty(rescrapeAllPagesEndPoint))
            {
                try
                {
                    ResetWebPageScrapeStatus();
                    _isFullWebScrapingInProgress = true;

                    using var client = new HttpClient
                    {
                        BaseAddress = new Uri(rescrapeAllPagesEndPoint)
                    };

                    var response = await client.GetAsync("");
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(responseContent);
                    }
                    else
                    {
                        return BadRequest($"Failed to Start the Web Scraping. Error : {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    _isFullWebScrapingInProgress = false;
                    return BadRequest($"Failed to Start the Web Scraping. Error : {ex.Message} ");
                }
            }
            return BadRequest("Web Scrape Url not found. Please check the configuration");
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(RescrapePage))]
        public async Task<IActionResult> RescrapePage(int pageId)
        {
            var webPage = _dbContext.WebScapeConfig.FirstOrDefault(w => w.Id == pageId);
            if (webPage == null)
            {
                return BadRequest($"Web Page with the Id {pageId} does not exists");
            }

            string WebscrapeUrl = _appSettings.RescrapeWebPageEndPoint;

            if (!string.IsNullOrEmpty(WebscrapeUrl))
            {
                try
                {
                    webPage.ScrapeStatus = ScrapeStatus.YetToScrape;
                    webPage.ErrorMessage = string.Empty;

                    _dbContext.WebScapeConfig.Update(webPage);
                    _dbContext.SaveChanges();

                    using var client = new HttpClient
                    {
                        BaseAddress = new Uri(WebscrapeUrl)
                    };

                    var json = JsonConvert.SerializeObject(webPage, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        }
                    });

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("", data);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(responseContent);
                    }
                    else
                    {
                        return BadRequest($"Failed to Start the Web Scraping. Error : {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to Start the Web Scraping. Error : {ex.Message} ");
                }
            }
            return BadRequest("Web Scrape Url not found. Please check the configuration");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(OnScrapingCompleted))]
        public IActionResult OnScrapingCompleted()
        {
            _isFullWebScrapingInProgress = false;
            return Ok(_isFullWebScrapingInProgress);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(IsFullWebScrapingInProgress))]
        public IActionResult IsFullWebScrapingInProgress()
        {
            return Ok(_isFullWebScrapingInProgress);
        }

        #endregion



        #region Private Methods

        private void ResetWebPageScrapeStatus()
        {
            var webPages = _dbContext.WebScapeConfig.ToArray();

            foreach (var webPage in webPages)
            {
                webPage.ScrapeStatus = ScrapeStatus.YetToScrape;
                webPage.ErrorMessage = string.Empty;
            }

            _dbContext.WebScapeConfig.UpdateRange(webPages);
            _dbContext.SaveChanges();
        }

        #endregion
    }
}