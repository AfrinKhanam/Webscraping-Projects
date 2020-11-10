using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndianBank_ChatBOT.Models
{
    public class SynonymWord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SynonymId { get; set; }

        [ForeignKey(nameof(SynonymId))]
        public virtual Synonym Synonym { get; set; }

    }
}
