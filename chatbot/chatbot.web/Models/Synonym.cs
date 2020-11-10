using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndianBank_ChatBOT.Models
{
    public class Synonym
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Word { get; set; }
        public virtual ICollection<SynonymWord> SynonymWords { get; set; }
        public int? LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual WebPageLanguage WebPageLanguage { get; set; }

    }
}
