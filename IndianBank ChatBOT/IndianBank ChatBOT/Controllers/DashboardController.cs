﻿using IndianBank_ChatBOT.Models;
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

        public ActionResult RealTimeDashboard()
        {
            int currentMonth = DateTime.Now.Month;
            int lastMonth = DateTime.Now.Month - 1;
            int currentYear = DateTime.Now.Year;
            int lastYear = DateTime.Now.Year - 1;
            var today = DateTime.Now;
            var tomorrow = DateTime.Now.AddDays(1);
            var yesterday = DateTime.Now.AddDays(-1);

            var vm = new VisitorsStatisticsViewModel
            {
                NumberOfVisitsCurrentYear = VisitorsByYear(currentYear).Sum(u => u.NumberOfVisits),
                NumberOfVisitsLastYear = VisitorsByYear(lastYear).Sum(u => u.NumberOfVisits),
                NumberOfVisitsCurrentMonth = VisitorsByMonth(currentYear, currentMonth).Sum(u => u.NumberOfVisits),
                NumberOfVisitsLastMonth = VisitorsByMonth(currentYear, lastMonth).Sum(u => u.NumberOfVisits),
                NumberOfVisitsToday = VisitorsByDateRange(today, today).Sum(u => u.NumberOfVisits),
                NumberOfVisitsYesterday = VisitorsByDateRange(yesterday, yesterday).Sum(u => u.NumberOfVisits),

                NumberOfUnsatisfactoryVisitsCurrentYear = UnsatisfactoryVisitorsByYear(currentYear).Sum(u => u.NumberOfVisits),
                NumberOfUnsatisfactoryVisitsLastYear = UnsatisfactoryVisitorsByYear(lastYear).Sum(u => u.NumberOfVisits),
                NumberOfUnsatisfactoryVisitsCurrentMonth = UnsatisfactoryVisitorsByMonth(currentYear, currentMonth).Sum(u => u.NumberOfVisits),
                NumberOfUnsatisfactoryVisitsLastMonth = UnsatisfactoryVisitorsByMonth(currentYear, lastMonth).Sum(u => u.NumberOfVisits),
                NumberOfUnsatisfactoryVisitsToday = UnsatisfactoryVisitorsByDateRange(today, today).Sum(u => u.NumberOfVisits),
                NumberOfUnsatisfactoryVisitsYesterday = UnsatisfactoryVisitorsByDateRange(yesterday, yesterday).Sum(u => u.NumberOfVisits)
            };

            return View(vm);
        }

        private List<ChatBotVisitorDetail> VisitorsByDateRange(DateTime from, DateTime to)
        {
            var fromDate = from.ToString("yyyy-MM-dd");
            var toDate = to.AddDays(1).ToString("yyyy-MM-dd");
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries , Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and \"TimeStamp\" between '{fromDate}' AND '{toDate}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";
            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();
            return users;
        }

        private List<ChatBotVisitorDetail> VisitorsByYear(int toYear)
        {
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries, Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and extract(year from C.\"TimeStamp\") = '{toYear}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();

            return users;
        }

        private List<ChatBotVisitorDetail> VisitorsByMonth(int toYear, int toMonth)
        {
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries, Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and extract(year from C.\"TimeStamp\") = '{toYear}' and extract(month from C.\"TimeStamp\") = '{toMonth}' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();

            return users;
        }

        private List<ChatBotVisitorDetail> UnsatisfactoryVisitorsByDateRange(DateTime from, DateTime to)
        {
            var fromDate = from.ToString("yyyy-MM-dd");
            var toDate = to.AddDays(1).ToString("yyyy-MM-dd");
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries , Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and \"TimeStamp\" between '{fromDate}' AND '{toDate}' and C.\"ResonseFeedback\" = '-1' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";
            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();
            return users;
        }

        private List<ChatBotVisitorDetail> UnsatisfactoryVisitorsByYear(int toYear)
        {
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries, Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and extract(year from C.\"TimeStamp\") = '{toYear}' and C.\"ResonseFeedback\" = '-1' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();

            return users;
        }

        private List<ChatBotVisitorDetail> UnsatisfactoryVisitorsByMonth(int toYear, int toMonth)
        {
            var query = $"select Visitor, PhoneNumber, Sum(NumberOfQueriesAsked) as NumberOfQueries, Sum(NumberOfVisits) as NumberOfVisits, Max(LastVisit) as LastVisited from (select Visitor, PhoneNumber, LastVisit, Sum(CountOfConversation) as NumberOfQueriesAsked, Count(ConversationId) as NumberOfVisits from (select U.\"Name\" as Visitor, U.\"PhoneNumber\" as PhoneNumber, Count(U.\"Id\") as CountOfConversation, C.\"ConversationId\" as ConversationId, Max(C.\"TimeStamp\") as LastVisit from \"ChatLogs\" C inner join \"UserInfos\" U on C.\"ConversationId\" = U.\"ConversationId\" where C.\"IsOnBoardingMessage\" is null and C.\"ReplyToActivityId\" is null and coalesce(C.\"Text\", '') != '' and extract(year from C.\"TimeStamp\") = '{toYear}' and extract(month from C.\"TimeStamp\") = '{toMonth}' and C.\"ResonseFeedback\" = '-1' group by U.\"Name\", U.\"PhoneNumber\", C.\"ConversationId\") as tmp group by tmp.Visitor, tmp.PhoneNumber, tmp.LastVisit) as queryResult group by queryResult.Visitor, queryResult.PhoneNumber";

            var users = _dbContext.ChatBotVisitorDetails.FromSql(query).ToList().OrderByDescending(u => u.LastVisited).ToList();

            return users;
        }
    }
}
