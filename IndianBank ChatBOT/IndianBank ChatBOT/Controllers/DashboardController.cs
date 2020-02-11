using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public DashboardController(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVisitsReport(string from, string to)
        {
            var fromDate = from;
            var toDate = to;
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                fromDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                toDate = Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd");
            }

            //var query = $"select * from \"ChatLogs\" where \"RasaIntent\" not in ('about_us_intent','greet','bye_intent', 'lost_intent') and \"TimeStamp\" between '{fromDate}' AND '{toDate}' order by \"TimeStamp\"";
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries , Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and \"TimeStamp\" between '{fromDate}' AND '{toDate}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var chatLogs = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList();

            return null;
        }
    }
}
