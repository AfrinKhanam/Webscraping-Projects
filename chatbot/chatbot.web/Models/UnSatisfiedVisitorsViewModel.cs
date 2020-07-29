using System.Collections.Generic;

namespace UjjivanBank_ChatBOT.Models
{
    public class UnSatisfiedVisitorsViewModel : ReportParams
    {
        public List<UnAnsweredQueries> UnAnsweredQueries { get; set; }
    }
}
