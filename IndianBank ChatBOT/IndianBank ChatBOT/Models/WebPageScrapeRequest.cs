using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
    }

    public enum ScrapeStatus
    {
        YetToScrape,
        ScrapeSuccess,
        ScrapeFailed
    }
}
