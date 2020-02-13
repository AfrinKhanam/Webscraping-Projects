using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class VisitorsStatisticsViewModel
    {
        public int NumberOfVisitsCurrentYear { get; set; }
        public int NumberOfVisitsLastYear { get; set; }
        public int NumberOfVisitsCurrentMonth { get; set; }
        public int NumberOfVisitsLastMonth { get; set; }
        public int NumberOfVisitsToday { get; set; }
        public int NumberOfVisitsYesterday { get; set; }

        public int NumberOfUnsatisfactoryVisitsCurrentYear { get; set; }
        public int NumberOfUnsatisfactoryVisitsLastYear { get; set; }
        public int NumberOfUnsatisfactoryVisitsCurrentMonth { get; set; }
        public int NumberOfUnsatisfactoryVisitsLastMonth { get; set; }
        public int NumberOfUnsatisfactoryVisitsToday { get; set; }
        public int NumberOfUnsatisfactoryVisitsYesterday { get; set; }
    }
}
