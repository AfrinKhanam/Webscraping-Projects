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
    //[Route("[controller]")]
    public class WebPageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public WebPageController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var webPages = _dbContext.WebPages.ToList();
            return View(webPages);
        }


        [HttpGet]
        public ActionResult AddNew()
        {
             var webPage = new WebPage();
            return View(webPage);
        }

        [HttpPost]
        public ActionResult AddNew([FromBody]WebPage webPage)
        {
            _dbContext.WebPages.Add(webPage);
            _dbContext.SaveChanges();

            ViewBag.InsertFAQStatus = "New Web Page is added successfully.";
            ModelState.Clear();

            return View();
        }


        [HttpGet]
        public ActionResult Edit(int pageId)
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            return Ok(Synonyms);
        }

        [HttpGet]
        public ActionResult Edit([FromBody]WebPage webPage)
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            return Ok(Synonyms);
        }
    }
}
