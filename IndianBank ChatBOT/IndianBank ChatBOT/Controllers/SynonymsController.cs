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
    public class SynonymsController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public SynonymsController(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public ActionResult Index()
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            return View(Synonyms);
        }

        public IActionResult GetAllWords()
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            return Ok(Synonyms);
        }

        public IActionResult GetAllWordsCsv()
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            var wordsCsv = new List<string>();
            foreach (var item in Synonyms)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(item.Word);
                var SynonymWords = item.SynonymWords.ToList();
                for (int i = 0; i < SynonymWords.Count; i++)
                {
                    sb.Append(",");
                    sb.Append(SynonymWords[i].Name);
                }
                wordsCsv.Add(sb.ToString());
            }
            return Ok(wordsCsv);
        }

        [HttpDelete]
        public IActionResult DeleteWordById(int id)
        {
            if (id != 0)
            {
                var synonym = _dbContext.Synonyms.FirstOrDefault(s => s.Id == id);
                if (synonym != null)
                {
                    _dbContext.Synonyms.Remove(synonym);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Synonym with the id {id} not found!");
        }

        [HttpPut]
        public IActionResult UpdateWordById(Synonym synonym)
        {
            if (synonym.Id != 0)
            {
                var synonymObj = _dbContext.Synonyms.FirstOrDefault(s => s.Id == synonym.Id);
                if (synonymObj != null)
                {
                    _dbContext.Synonyms.Update(synonymObj);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Synonym with the id {synonym.Id} not found!");
        }


        [HttpPost]
        public IActionResult AddNewWord(Synonym synonym)
        {
            _dbContext.Synonyms.Add(synonym);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult AddNewSynonymWord(SynonymWord synonymWord)
        {
            _dbContext.SynonymWords.Add(synonymWord);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateSynonymWord(SynonymWord synonymWord)
        {
            if (synonymWord.Id != 0)
            {
                var synonymWordObj = _dbContext.SynonymWords.FirstOrDefault(s => s.Id == synonymWord.Id);
                if (synonymWordObj != null)
                {
                    _dbContext.SynonymWords.Update(synonymWordObj);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Synonym Word with the id {synonymWord.Id} not found!");
        }

        [HttpDelete]
        public IActionResult DeleteSynonymWordById(int id)
        {
            if (id != 0)
            {
                var synonymWordObj = _dbContext.SynonymWords.FirstOrDefault(s => s.Id == id);
                if (synonymWordObj != null)
                {
                    _dbContext.SynonymWords.Remove(synonymWordObj);
                    _dbContext.SaveChanges();
                    return Ok();
                }
            }
            return NotFound($"Synonym Word with the id {id} not found!");
        }

    }
}
