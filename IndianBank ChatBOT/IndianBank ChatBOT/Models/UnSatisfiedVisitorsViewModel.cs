using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class UnSatisfiedVisitorsViewModel : ReportParams
    {
        public List<UnAnsweredQueries> UnAnsweredQueries { get; set; }
    }
}
