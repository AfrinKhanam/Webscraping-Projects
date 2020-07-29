using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UjjivanBank_ChatBOT.Models
{
    public class WebPageViewModel
    {
        public WebScapeConfig[] WebScapeConfigs { get; set; }
        public bool IsFullWebScrapingInProgress { get; set; }
    }
}
