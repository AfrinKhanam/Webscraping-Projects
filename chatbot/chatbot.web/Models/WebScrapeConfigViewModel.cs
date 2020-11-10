using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class WebScrapeConfigViewModel
    {
        public WebScrapeConfig WebScrapeConfig { get; set; }
        public List<SelectListItem> Languages { get; set; }
    }
}
