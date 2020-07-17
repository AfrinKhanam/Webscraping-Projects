using System.Collections.Generic;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationExportViewModel : ReportParams
    {
        public int TotalleadGenerationQueries { get; set; }
        public int UnattendedQueries { get; set; }
        public List<LeadGenerationInfo> LeadGenerationInfos { get; set; }
    }
}
