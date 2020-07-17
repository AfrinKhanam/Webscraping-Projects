using System.Collections.Generic;

namespace IndianBank_ChatBOT.Models
{
    public class UnSatisfiedVisitorsViewModel : ReportParams
    {
        public List<UnAnsweredQueries> UnAnsweredQueries { get; set; }
    }
}
