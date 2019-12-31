using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class UnAnsweredQueries
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public string BotResponse { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
