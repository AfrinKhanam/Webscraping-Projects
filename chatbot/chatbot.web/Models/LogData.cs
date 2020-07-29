using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UjjivanBank_ChatBOT.Models
{
    public class LogData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
        public string ReplyToActivityId { get; set; }
        public string ConversationId { get; set; }
        public string ConversationType { get; set; }
        public string ConversationName { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Text { get; set; }
        public string RasaIntent { get; set; }
        public double RasaScore { get; set; }
        public string RasaEntities { get; set; }
        public DateTime? TimestampUtc { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
