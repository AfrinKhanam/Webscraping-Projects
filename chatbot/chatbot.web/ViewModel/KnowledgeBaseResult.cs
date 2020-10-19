using System.Collections.Generic;

namespace IndianBank_ChatBOT.ViewModel
{
    public class KnowledgeBaseResult
    {
        public List<DOCUMENT> DOCUMENTS { get; set; }
        public int WORD_COUNT { get; set; }
        public double WORD_SCORE { get; set; }
        public string FILENAME { get; set; }
        public string AUTO_CORRECT_QUERY { get; set; }
        public string CORRECT_QUERY { get; set; }
    }

    public class DOCUMENT
    {
        public string main_title { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string value { get; set; }
    }
}