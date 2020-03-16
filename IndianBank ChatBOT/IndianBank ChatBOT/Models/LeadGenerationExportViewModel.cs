using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationExportViewModel : ReportParams
    {
        public int TotalleadGenerationQueries { get; set; }
        public int UnattendedQueries { get; set; }
        public List<LeadGenerationInfo> LeadGenerationInfos { get; set; }
    }
}
