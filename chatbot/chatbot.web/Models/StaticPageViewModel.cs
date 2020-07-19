using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class StaticPageViewModel
    {
        public List<StaticPage> StaticPages { get; set; }
        public bool IsStaticFilesScrapingInProgress { get; set; }
    }
}
