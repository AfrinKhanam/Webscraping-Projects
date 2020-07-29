using System.Collections.Generic;

namespace UjjivanBank_ChatBOT.Models
{
    public class ConversationByIntent
    {
        public string Intent { get; set; }
        public List<ConversationByUser> ConversationByUsers { get; set; }
    }
}
