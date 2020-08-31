using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class WebPageViewModel
    {
        public WebScrapeConfig[] WebScrapeConfigs { get; set; }
        public bool IsFullWebScrapingInProgress { get; set; }
    }
}
