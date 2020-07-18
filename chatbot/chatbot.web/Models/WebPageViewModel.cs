using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class WebPageViewModel
    {
        public List<WebScapeConfig> WebScapeConfigs { get; set; }
        public bool IsFullWebScrapingInProgress { get; set; }
    }
}
