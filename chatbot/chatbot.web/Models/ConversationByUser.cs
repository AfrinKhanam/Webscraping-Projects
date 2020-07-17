using System;
using System.Collections.Generic;

namespace IndianBank_ChatBOT.Models
{
    public class ConversationByUser
    {
        public int UserInfoId { get; set; }
        public string ConversationId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<ChatLog> ChatLogs { get; set; }
        public LeadGenerationInfo LeadGenerationInfo { get; set; }
        public List<TurnConversation> TurnConversations { get; set; }
    }
}
