using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IndianBank_ChatBOT.Controllers
{
    [Route("[controller]")]
    public class WebPageScrapeRequestController : Controller
    {
        private readonly AppDbContext _dbContext;

        public WebPageScrapeRequestController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        [Route(nameof(Index))]
        public ActionResult Index()
        {
            var requests = _dbContext.WebPageScrapeRequests.Include(w => w.WebPage).OrderByDescending(w => w.RequestedDate).ToList();
            return View(requests);
        }

        // Below APIs for RabbitMQ

        [HttpGet]
        [Route(nameof(GetAllPendingRequests))]
        public IActionResult GetAllPendingRequests()
        {
            var requests = _dbContext.WebPageScrapeRequests
                                     .Where(r => r.ScrapeStatus == ScrapeStatus.YetToScrape)
                                     .ToList();
            return Ok(requests);
        }

        [HttpPost]
        [Route(nameof(AddNewRequest))]
        public IActionResult AddNewRequest(int pageId)
        {
            WebPageScrapeRequest vm = new WebPageScrapeRequest
            {
                RequestedDate = DateTime.Now,
                ScrapeStatus = ScrapeStatus.YetToScrape,
                WebPageId = pageId,
                Id = 0
            };

            _dbContext.WebPageScrapeRequests.Add(vm);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route(nameof(UpdateStatus))]
        public IActionResult UpdateStatus(WebPageScrapeRequest vm)
        {
            _dbContext.WebPageScrapeRequests.Update(vm);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
