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
        static string IntentName = string.Empty;
        static double IntentScore = 0.0;
        static string Entities = String.Empty;
        public BotChatActivityLogger(string value)
        {
            this.value = value;
        }


        public async Task LogActivityAsync(IActivity activity)
        {
            var cs = "Server=localhost;Port=5432;Database=IndianBankDb;User Id=postgres;Password=postgres";

            using (var dbContext = new AppDbContext(cs))
            {
                var msg = activity.AsMessageActivity();
                try
                {
                    if (msg != null)
                    {
                        var logData = new ChatLog
                        {
                            ActivityId = msg.Id,
                            ActivityType = msg.Type,
                            ReplyToActivityId = msg.ReplyToId,
                            ConversationId = msg.Conversation.Id,
                            ConversationType = msg.Conversation.ConversationType,
                            ConversationName = msg.Conversation.Name,
                            RecipientId = msg.Recipient?.Id,
                            RecipientName = msg.Recipient?.Name,
                            //TODO
                            //RasaEntities = Entities,
                            //RasaIntent = IntentName,
                            //RasaScore = IntentScore,
                            FromId = msg.From?.Id,
                            FromName = msg.From?.Name,
                            Text = msg.Text,
                            //TODO
                            ResponseJsonText = "",
                            ResonseFeedback = null,
                            TimeStamp = msg.Timestamp?.LocalDateTime,
                            Id = 0,
                            ResponseSource = ResponseSource.ElasticSearch
                        };

                        var data = await dbContext.ChatLogs.AddAsync(logData);

                        var result = await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public static void GetRasaResult(string intentName, double intentScore, string entity)
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
