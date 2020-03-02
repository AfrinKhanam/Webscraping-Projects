using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class ChatBotVisitorsViewModel : ReportParams
    {
        public int TotalVisits { get; set; }
        public int TotalQueries { get; set; }
        public List<ChatBotVisitorDetail> ChatBotVisitorDetails { get; set; }
    }
    public class ChatBotVisitorDetail
    {
        public string Visitor { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfVisits { get; set; }
        public int NumberOfQueries { get; set; }
        public DateTime LastVisited { get; set; }
    }
}