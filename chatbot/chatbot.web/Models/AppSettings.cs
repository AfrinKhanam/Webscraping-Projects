namespace UjjivanBank_ChatBOT.Models
{
    public class AppSettings
    {
        public string QAEndPoint { get; set; }
        public string ConnectionString { get; set; }
        public string RabbitmqUsername { get; set; }
        public string RabbitmqPassword { get; set; }
        public string RabbitmqVirtualHost { get; set; }
        public string RabbitmqHostName { get; set; }
        public string AutoSuggestionUrl { get; set; }
        public string DirectLineBaseUrl { get; set; }
        public string ChatBotBackEndUIEndPoint { get; set; }
        public string StaticFileSuspiciousContentsCSV { get; set; }
        public string RescrapeAllPagesEndPoint { get; set; }
        public string RescrapeAllStaticPagesEndPoint { get; set; }
        public string RescrapeWebPageEndPoint { get; set; }
        public string SynonymsSyncUrl { get; set; }

    }
}
