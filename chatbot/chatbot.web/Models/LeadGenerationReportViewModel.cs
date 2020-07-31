using System.Collections.Generic;

namespace UjjivanBank_ChatBOT.Models
{
    public class LeadGenerationReportViewModel : ReportParams
    {
        public List<LeadGenerationAction> LeadGenerationActions { get; set; }
        public List<ConversationByIntent> ConversationsByIntent { get; set; }
    }
}
