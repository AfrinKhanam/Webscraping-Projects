﻿using IndianBank_ChatBOT.Utils;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndianBank_ChatBOT.Models
{
    public class WebPageScrapeRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public ScrapeStatus ScrapeStatus { get; set; }

        [Required, ForeignKey(nameof(WebPage))]
        public int WebPageId { get; set; }

        public virtual WebPage WebPage { get; set; }

        public string GetEnumDescription()
        {
            return ScrapeStatus.GetEnumDescription();
        }
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
