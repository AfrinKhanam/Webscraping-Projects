using System;
using System.Linq;

using UjjivanBank_ChatBOT.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UjjivanBank_ChatBOT.PartialViews
{
    public class DomainVisitors : ViewComponent
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly string _connectionString;

        public DomainVisitors(AppDbContext _dbContext, IConfiguration configuration, IOptions<AppSettings> appsettings)
        {
            this._dbContext = _dbContext;
            _appSettings = appsettings.Value;
            _connectionString = configuration.GetConnectionString("connString");
        }

        public IViewComponentResult Invoke()
        {
            int currentYear = DateTime.Now.Year;
            var query = $"select C.\"RasaIntent\" as DomainName, count(C.\"ActivityId\") as TotalHits , TRUNC(((count(C.\"ActivityId\") / SUM(count(C.\"ActivityId\")) OVER ())*100),2) as HitPercentage from \"ChatLogs\" C where C.\"ReplyToActivityId\" is not null and C.\"RasaIntent\" not in ('about_us_intent','greet', 'bye_intent', 'link_intent', 'services_intent', 'scrollbar_intent') and coalesce(C.\"RasaIntent\", '') != '' and extract(year from C.\"TimeStamp\") = '{currentYear}' group by C.\"RasaIntent\" order by count(C.\"ActivityId\") desc limit 10";
            var vm = _dbContext.Top10DomainsVisitedViewModels.FromSqlRaw(query).ToList();
            return View("DomainVisitors", vm);
        }
    }
}
