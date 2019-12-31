using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class FrequentlyAskedQueries
    {
        public int Count { get; set; }
        public string Query { get; set; }
        public int PositiveFeedback { get; set; }
        public int NegetiveFeedback { get; set; }
    }
}
