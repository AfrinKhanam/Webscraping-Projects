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
        private string _connectionString;
        static string _intentName = string.Empty;
        static double _intentScore = 0.0;
        static string _entities = string.Empty;
        static string _responseJsonText = string.Empty;
        static ResponseSource? _responseSource = null;

        public BotChatActivityLogger(string value)
        {
            this._connectionString = value;
        }

        public async Task LogActivityAsync(IActivity activity)
        {
            //var cs = "Server=localhost;Port=5432;Database=IndianBankDb;User Id=postgres;Password=postgres";

            using (var dbContext = new AppDbContext(_connectionString))
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
                            RasaEntities = _entities,
                            RasaIntent = _intentName,
                            RasaScore = _intentScore,
                            FromId = msg.From?.Id,
                            FromName = msg.From?.Name,
                            Text = msg.Text,
                            ResponseJsonText = _responseJsonText,
                            ResonseFeedback = null,
                            TimeStamp = msg.Timestamp?.LocalDateTime,
                            Id = 0,
                            ResponseSource = _responseSource
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


        public static async Task LogActivityCustom(IActivity activity)
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
                            RasaEntities = _entities,
                            RasaIntent = _intentName,
                            RasaScore = _intentScore,
                            FromId = msg.From?.Id,
                            FromName = msg.From?.Name,
                            Text = msg.Text,
                            ResponseJsonText = _responseJsonText,
                            ResonseFeedback = null,
                            TimeStamp = msg.Timestamp?.LocalDateTime,
                            Id = 0,
                            ResponseSource = _responseSource
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
        public static void UpdateRaSaData(string intentName, double intentScore, string entity)
        {
            _intentName = intentName;
            _intentScore = intentScore;
            _entities = entity;
        }
        public static void UpdateResponseJsonText(string responseJsonText)
        {
            _responseJsonText = responseJsonText;
        }
        public static void UpdateSource(ResponseSource responseSource)
        {
            _responseSource = responseSource;
        }
        public static void GetDepavlovResult(string confidence)
        {
            _intentName = "Faq";
            _intentScore = Convert.ToDouble(confidence);
        }
    }
}
