using IndianBank_ChatBOT.Models;
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
    public class WebPageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public WebPageController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        [Route(nameof(Index))]
        public ActionResult Index()
        {
            var webPages = _dbContext.WebPages.OrderBy(p=>p.PageName).ToList();
            return View(webPages);
        }

        [HttpGet]
        [Route(nameof(AddNew))]
        public ActionResult AddNew()
        {
            var webPage = new WebPage();
            return View(webPage);
        }

        [HttpPost]
        [Route(nameof(AddNew))]
        public ActionResult AddNew(WebPage webPage)
        {
            _dbContext.WebPages.Add(webPage);
            _dbContext.SaveChanges();

            ViewBag.insertWebPageStatus = "New Web Page is added successfully.";
            ModelState.Clear();

            return View();
        }


        [HttpGet]
        [Route(nameof(Edit))]
        public ActionResult Edit(int pageId)
        {
            var webPage = _dbContext.WebPages.Find(pageId);
            return View(webPage);
        }

        [HttpPost]
        [Route(nameof(Edit))]
        public ActionResult Edit(WebPage webPage)
        {
            _dbContext.WebPages.Update(webPage);
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
                var webpage = _dbContext.WebPages.FirstOrDefault(w => w.Id == pageId);
                if (webpage != null)
                {
                    _dbContext.WebPages.Remove(webpage);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Web Page with the id {pageId} not found!");
        }

        // Below APIs for RabbitMQ

        [HttpGet]
        [Route(nameof(GetAllPages))]
        public IActionResult GetAllPages()
        {
            var webPages = _dbContext.WebPages.ToList();
            return Ok(webPages);
        }

        [HttpGet]
        [Route(nameof(GetPageById))]
        public IActionResult GetPageById(int pageId)
        {
            var webPage = _dbContext.WebPages.Find(pageId);
            return Ok(webPage);
        }
    }
}
