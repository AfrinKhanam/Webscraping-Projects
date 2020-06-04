using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class SynonymsController : Controller
    {
        private readonly AppDbContext _dbContext;
        public SynonymsController(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public ActionResult Index()
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).OrderBy(s => s.Word).ToList();
            return View(Synonyms);
        }

        public IActionResult GetAllWords()
        {
            var Synonyms = _dbContext.Synonyms.Include(s => s.SynonymWords).ToList();
            return Ok(Synonyms);
        }

        [HttpDelete]
        public IActionResult DeleteWordById(int id)
        {
            if (ModelState.IsValid)
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

            return BadRequest("Invalid Input Data");
        }

        [HttpPut]
        public IActionResult UpdateWordById(Synonym synonym)
        {
            if (ModelState.IsValid)
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

            return BadRequest("Invalid Input Data");
        }

        [HttpPost]
        public IActionResult AddNewWord(Synonym synonym)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Synonyms.Add(synonym);
                _dbContext.SaveChanges();
                return Ok();
            }

            return BadRequest("Invalid Input Data");
        }

        [HttpPost]
        public IActionResult AddNewSynonymWord(SynonymWord synonymWord)
        {
            if (ModelState.IsValid)
            {
                _dbContext.SynonymWords.Add(synonymWord);
                _dbContext.SaveChanges();
                return Ok();
            }

            return BadRequest("Invalid Input Data");
        }

        [HttpPut]
        public IActionResult UpdateSynonymWord(SynonymWord synonymWord)
        {
            if (ModelState.IsValid)
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

            return BadRequest("Invalid Input Data");
        }

        [HttpDelete]
        public IActionResult DeleteSynonymWordById(int id)
        {
            if (ModelState.IsValid)
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

            return BadRequest("Invalid Input Data");
        }
    }
}
