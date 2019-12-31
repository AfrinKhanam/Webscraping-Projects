using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System.Collections.Generic;
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

        public ActionResult LeadGenerationReport()
        {
            var query = $"select * from \"ChatLogs\" where coalesce(\"RasaIntent\", '') != '' and \"RasaIntent\" not in ('about_us_intent','greet') and \"Text\" is not null order by \"TimeStamp\"";
            var chatLogs = _dbContext.ChatLogs.FromSql(query).ToList();
            //var chatLogs = _dbContext.ChatLogs.Where(c => c.RasaIntent != null && c.RasaIntent != "").OrderByDescending(c => c.TimeStamp).ToList();


            List<List<ChatLog>> listOfList = chatLogs.GroupBy(item => item.RasaIntent)
                                             .Select(group => group.ToList())
                                             .ToList();

            List<ConversationByIntent> conversationByIntent = new List<ConversationByIntent>();

            var conversationIds = chatLogs.Select(c => c.ConversationId).Distinct().ToList();

            var userInfo = _dbContext.UserInfos.Where(u => conversationIds.Contains(u.ConversationId)).ToList();


            foreach (var list in listOfList)
            {
                var cbi = new ConversationByIntent
                {
                    Intent = list.FirstOrDefault().RasaIntent
                };

                var li =
                list.GroupBy(item => item.ConversationId)
                                             .Select(group => new ConversationByUser
                                             {
                                                 ConversationId = group.Key,
                                                 Name = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.Name).FirstOrDefault(),
                                                 PhoneNumber = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.PhoneNumber).FirstOrDefault(),
                                                 TimeStamp = userInfo.Where(u => u.ConversationId == group.Key).Select(u => u.CreatedOn).FirstOrDefault(),
                                                 ChatLogs = group.ToList()
                                             })
                                             .ToList();
                cbi.ConversationByUsers = li;
                conversationByIntent.Add(cbi);
            }

            return View(conversationByIntent);
        }
    }
}
