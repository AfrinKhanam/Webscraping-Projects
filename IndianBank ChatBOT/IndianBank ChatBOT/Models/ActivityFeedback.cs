using System;

namespace IndianBank_ChatBOT.Models
{
    public class ActivityFeedback
    {
        public Guid ActivityId { get; set; }
        public ResonseFeedback ResonseFeedback { get; set; }
    }
}
