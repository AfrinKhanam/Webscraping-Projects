using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class ConversationByIntent
    {
        public string Intent { get; set; }
        public List<ConversationByUser> ConversationByUsers { get; set; }
    }
}
