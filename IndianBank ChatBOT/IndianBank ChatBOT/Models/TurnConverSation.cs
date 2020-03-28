using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class TurnConversation
    {
        public string ActivityId { get; set; }
        public string UserQuery { get; set; }
        public string BotResponse { get; set; }
        public string UserTimeStamp { get; set; }
        public string BotTimeStamp { get; set; }
    }
}
