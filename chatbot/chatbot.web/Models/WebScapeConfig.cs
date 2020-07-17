using IndianBank_ChatBOT.Utils;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndianBank_ChatBOT.Models
{
    public class WebScapeConfig
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string PageName { get; set; }

        public string PageConfig { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastScrapedOn { get; set; }

        public ScrapeStatus ScrapeStatus { get; set; }

        public string GetEnumDescription()
        {
            return ScrapeStatus.GetEnumDescription();
        }

        public string ErrorMessage { get; set; }

        public bool IsActive { get; set; }
    }
    public enum ScrapeStatus
    {
        [Description("Yet to scrape")]
        YetToScrape,
        [Description("Successfully Scraped")]
        ScrapeSuccess,
        [Description("Failed to scrape")]
        ScrapeFailed
    }
}
