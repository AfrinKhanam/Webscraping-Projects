using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Models;
using Newtonsoft.Json;

namespace IndianBank_ChatBOT.ViewModel
{
    public class ScrapePageViewModel
    {
        public ScrapePageViewModel()
        {
        }

        public ScrapePageViewModel(WebScapeConfig webScapeConfig)
        {
            Id = webScapeConfig.Id;
            Url = webScapeConfig.Url;
            PageName = webScapeConfig.PageName;
            PageConfig = JsonConvert.DeserializeObject(webScapeConfig.PageConfig);
            Description = webScapeConfig.Description;
        }

        public int Id { get; set; }

        public string Url { get; set; }

        public string PageName { get; set; }

        public object PageConfig { get; set; }

        public string Description { get; set; }
    }
}
