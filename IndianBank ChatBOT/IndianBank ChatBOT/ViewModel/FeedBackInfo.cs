using IndianBank_ChatBOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.ViewModel
{
    public class FeedBackInfo
    {
        public string ActivityId { get; set; }
        public string AonversationId { get; set; }
        public ResonseFeedback ResonseFeedback { get; set; }
    }
}
