using IndianBank_ChatBOT.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Utils
{
    public class BotChatActivityLogger : ITranscriptLogger
    {
        private string value;
         static string IntentName=string.Empty;
         static double IntentScore=0.0;
        static string Entities = String.Empty;
        public BotChatActivityLogger(string value)
        {
            this.value = value;
        }

        
        public async Task LogActivityAsync(IActivity activity)
        {
            System.Diagnostics.Debug.Write("[INFO] Message: " + JsonConvert.SerializeObject(activity) + "SSSSSSSSSSSSSSSSSSSSSSSSSSSS");
            using (var dbContext = new LogDataContext())
            {
                var msg = activity.AsMessageActivity();
                var logData = new LogData
                {
                    ActivityId = msg.Id,
                    ActivityType = msg.Type,
                    ReplyToActivityId = msg.ReplyToId,
                    ConversationId = msg.Conversation.Id,
                    ConversationType = msg.Conversation.ConversationType,
                    ConversationName = msg.Conversation.Name,
                    //FromId = msg.From.Id,
                    //FromName = msg.From.Name,
                    RecipientId = msg.Recipient.Id,
                    //RasaEntities = msg.Entities != null && msg.Entities.Any() ? JsonConvert.SerializeObject(msg.Entities) : null,
                    RasaEntities = Entities,
                    RasaIntent = IntentName,
                    RasaScore =IntentScore,
                    RecipientName = msg.Recipient.Name,
                    Text = msg.Text,
                    TimestampUtc = msg.Timestamp?.DateTime,
                    Timestamp = msg.Timestamp?.LocalDateTime
                };

                try
                {
                    var data = await dbContext.ChatLog.AddAsync(logData);

                    var result = await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }
        public static void GetRasaResult(string intentName,double intentScore,string  entity)
        {
            IntentName = intentName;
            IntentScore = intentScore;
            Entities = entity;
        }
        public static void GetDepavlovResult(string confidence)
        {
            IntentName = "Faq";
            IntentScore = Convert.ToDouble(confidence);
        }
    }
}
