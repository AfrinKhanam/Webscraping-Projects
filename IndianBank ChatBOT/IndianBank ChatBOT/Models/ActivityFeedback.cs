using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class ActivityFeedback
    {
        public Guid ActivityId { get; set; }
        public ResonseFeedback ResonseFeedback { get; set; }
    }
}
