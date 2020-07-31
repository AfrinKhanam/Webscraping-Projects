using System.Collections.Generic;

namespace UjjivanBank_ChatBOT.Models
{
    public class LeadGenerationExportViewModel : ReportParams
    {
        public int TotalleadGenerationQueries { get; set; }
        public int UnattendedQueries { get; set; }
        public List<LeadGenerationInfo> LeadGenerationInfos { get; set; }
    }
}
