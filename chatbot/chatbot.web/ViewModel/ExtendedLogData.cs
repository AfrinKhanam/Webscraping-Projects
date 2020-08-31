using IndianBank_ChatBOT.Models;

namespace IndianBank_ChatBOT.ViewModel
{
    public class ExtendedLogData
    {
        public string IntentName { get; set; }

        public double IntentScore { get; set; }

        public string Entity { get; set; }

        public string MainTitle { get; set; }

        public string SubTitle { get; set; }

        public string ResponseJson { get; set; }

        public ResponseSource ResponseSource { get; set; }
    }
}
