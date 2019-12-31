using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class ChatLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ResponseSource? ResponseSource { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
        public string ConversationId { get; set; }
        public string ConversationType { get; set; }
        public string ConversationName { get; set; }
        public string ReplyToActivityId { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string Text { get; set; }
        public string RecipientId { get; set; }
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public string RecipientName { get; set; }
        public string ResponseJsonText { get; set; }
        public string RasaIntent { get; set; }
        public double? RasaScore { get; set; }
        public string RasaEntities { get; set; }
        public ResonseFeedback? ResonseFeedback { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
