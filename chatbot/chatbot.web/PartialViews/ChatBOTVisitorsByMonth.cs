using System;
using System.Linq;

using UjjivanBank_ChatBOT.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UjjivanBank_ChatBOT.PartialViews
{
    public class ChatBOTVisitorsByMonth : ViewComponent
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public ChatBOTVisitorsByMonth(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public IViewComponentResult Invoke()
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            var query = $"select EXTRACT(day FROM U.\"CreatedOn\"):: int as DayNumber, TO_CHAR(U.\"CreatedOn\", 'Mon') AS MonthText, Count(U.\"Id\"):: int AS Visits from \"UserInfos\" U where extract(YEAR FROM U.\"CreatedOn\") = '{currentYear}' and extract(month from U.\"CreatedOn\") = '{currentMonth}' GROUP BY 1,2 order by EXTRACT(day FROM U.\"CreatedOn\"):: int asc";
            var vm = _dbContext.VisitorsByMonthViewModels.FromSqlRaw(query).ToList();
            return View("ChatBOTVisitorsByMonth", vm);
        }
    }
}
