using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public ReportController(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FrequentlyAskedQueries()
        {
            var query = $"select count(\"ActivityId\") as Count , \"Text\" as Query  from \"ChatLogs\" where \"FromId\"='IndianBank_ChatBOT' and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet') group by \"Text\" order by count desc";
            var data = _dbContext.FrequentlyAskedQueries.FromSql(query).ToList();
            return View(data);
        }

        public ActionResult AppUsers()
        {
            var users = _dbContext.UserInfos.ToList();
            return View(users);
        }

        public ActionResult UnAnsweredQueries()
        {
            var query = $"select \"Text\" as Query  from \"ChatLogs\" where \"FromId\"='IndianBank_ChatBOT' and coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" in ('bye_intent')";
            var data = _dbContext.UnAnsweredQueries.FromSql(query).ToList();
            return View(data);
        }
    }
}
