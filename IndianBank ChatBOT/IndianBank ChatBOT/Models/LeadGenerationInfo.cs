using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Visitor { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime QueriedOn { get; set; }
        public string DomainName { get; set; }  
        public int UserInfoId { get; set; }
        public string ConversationId { get; set; }
        public int? LeadGenerationActionId { get; set; }
    }
}
