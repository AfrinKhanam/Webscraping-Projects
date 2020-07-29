using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using UjjivanBank_ChatBOT.Utils;

namespace UjjivanBank_ChatBOT.Models
{
    public class StaticPage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PageConfig { get; set; }
        public string PageUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        [Column(TypeName = "text")]
        public string FileData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastScrapedOn { get; set; }
        public ScrapeStatus ScrapeStatus { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsActive { get; set; }
        public string GetEnumDescription()
        {
            return ScrapeStatus.GetEnumDescription();
        }
    }
}
