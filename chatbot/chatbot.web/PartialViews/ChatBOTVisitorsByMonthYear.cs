using System;
using System.Linq;

using IndianBank_ChatBOT.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT.PartialViews
{
    public class ChatBOTVisitorsByMonthYear : ViewComponent
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public ChatBOTVisitorsByMonthYear(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public IViewComponentResult Invoke()
        {
            int currentYear = DateTime.Now.Year;
            var query = $"select EXTRACT(month FROM U.\"CreatedOn\") :: int as MonthNumber, TO_CHAR(U.\"CreatedOn\", 'Mon') AS MonthText, extract(YEAR FROM U.\"CreatedOn\") :: int AS Year, Count(U.\"Id\") :: int AS Visits from \"UserInfos\" U where extract(YEAR FROM U.\"CreatedOn\") = '{currentYear}' GROUP BY 1, 2, 3 order by EXTRACT(month FROM U.\"CreatedOn\") :: int asc ";
            var vm = _dbContext.VisitorsByYearMonthViewModels.FromSqlRaw(query).ToList();
            return View("ChatBOTVisitorsByMonthYear", vm);
        }
    }
}
