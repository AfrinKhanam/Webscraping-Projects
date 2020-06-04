namespace IndianBank_ChatBOT.Models
{
    public class AppSettings
    {
        public string TrainingEndPoint { get; set; }
        public string TestingEndPoint { get; set; }
        public string DeepPavlovPath { get; set; }
        public string ConnectionString { get; set; }
        public string RabbitmqUsername { get; set; }
        public string RabbitmqPassword { get; set; }
        public string RabbitmqVirtualHost { get; set; }
        public string RabbitmqHostName { get; set; }
        public string AutoSuggestionUrl { get; set; }
        public string DirectLineBaseUrl { get; set; }
    }
}
