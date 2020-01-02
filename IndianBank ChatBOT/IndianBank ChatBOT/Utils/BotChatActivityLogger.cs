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
        static AppDbContext _dbContext = null;
        private string _connectionString;
        static string _intentName = string.Empty;
        static double _intentScore = 0.0;
        static string _entities = string.Empty;
        static string _responseJsonText = string.Empty;
        static ResponseSource? _responseSource = null;

        static string _mainTitle = string.Empty;
        static string _subTitle = string.Empty;

        public BotChatActivityLogger(string connectionString)
        {
            this._connectionString = connectionString;
            _dbContext = new AppDbContext(this._connectionString);
        }

        public static UserInfo GetUserDetails(string conversationId)
        {
            UserInfo userInfo = _dbContext.UserInfos.FirstOrDefault(e => e.ConversationId.Equals(conversationId));
            return userInfo;
        }

        private static void ReSetValues()
        {
            _intentName = string.Empty;
            _intentScore = 0.0;
            _entities = string.Empty;
            _responseJsonText = string.Empty;
            _responseSource = null;
            _mainTitle = string.Empty;
            _subTitle = string.Empty;
        }
        public async Task LogActivityAsync(IActivity activity)
        {
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
                            ResponseSource = _responseSource,
                            MainTitle = _mainTitle,
                            SubTitle = _subTitle
                        };

                        var data = await dbContext.ChatLogs.AddAsync(logData);

                        var result = await dbContext.SaveChangesAsync();

                        ReSetValues();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static void UpdateOnBoardingMessageFlag(string conversationId)
        {
            var chatLogs = _dbContext.ChatLogs.Where(c => c.ConversationId == conversationId).ToList();
            chatLogs.ForEach(c => c.IsOnBoardingMessage = true);
            _dbContext.ChatLogs.UpdateRange(chatLogs);
            _dbContext.SaveChanges();
        }

        public static async Task LogActivityCustom(IActivity activity, string connectionString)
        {
            using (var dbContext = new AppDbContext(connectionString))
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
                            ResponseSource = _responseSource,
                            MainTitle = _mainTitle,
                            SubTitle = _subTitle
                        };

                        var data = await dbContext.ChatLogs.AddAsync(logData);

                        var result = await dbContext.SaveChangesAsync();

                        ReSetValues();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
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
        public static void UpdateSubTitle(string subTitle)
        {
            _subTitle = subTitle;
        }
        public static void UpdateMainTitle(string mainTitle)
        {
            _mainTitle = mainTitle;
        }
        public static void UpdateResponseJsonText(string responseJsonText)
        {
            _responseJsonText = responseJsonText;
        }
        public static void UpdateSource(ResponseSource responseSource)
        {
            _responseSource = responseSource;
        }
    }
}
