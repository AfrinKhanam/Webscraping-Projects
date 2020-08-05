using System.Collections.Generic;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationReportViewModel : ReportParams
    {
        public List<LeadGenerationAction> LeadGenerationActions { get; set; }
        public List<ConversationByIntent> ConversationsByIntent { get; set; }
    }
}
