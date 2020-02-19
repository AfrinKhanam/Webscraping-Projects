using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class Top10DomainsVisitedViewModel
    {
        public string DomainName { get; set; }
        public int TotalHits { get; set; }
        public float HitPercentage { get; set; }
    }
}
