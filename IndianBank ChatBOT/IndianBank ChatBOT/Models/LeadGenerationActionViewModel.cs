using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationActionViewModel
    {
        public int Id { get; set; }
        public int? LeadGenerationActionId { get; set; }
    }
}
