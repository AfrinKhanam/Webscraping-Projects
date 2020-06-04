using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndianBank_ChatBOT.Models
{
    public class StaticPage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EncodedPageUrl { get; set; }
        public string PageUrl { get; set; }
        public string PageConfig { get; set; }
        public string FileName { get; set; }
    }
}
