using System.Collections.Generic;

namespace IndianBank_ChatBOT.Models
{
    public class FrequentlyAskedQueries
    {
        public List<string> ActivityIds { get; set; }
        public int Count { get; set; }
        public string Query { get; set; }
        public int PositiveFeedback { get; set; }
        public int NegetiveFeedback { get; set; }
    }
}
