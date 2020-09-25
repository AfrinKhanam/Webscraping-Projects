using System;
using System.Linq;
using System.Threading.Tasks;

using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.ViewModel;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace IndianBank_ChatBOT.Utils
{
    public class BotChatActivityLogger : ITranscriptLogger
    {
        static string connectionString = string.Empty;

        public BotChatActivityLogger(string _connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                connectionString = _connectionString;
        }

        public async Task LogActivityAsync(IActivity activity)
        {
            var msg = activity.AsMessageActivity();

            var theActivity = activity as Activity;

            ExtendedLogData extendedLogData = null;

            activity.Conversation.Properties.TryGetValue(nameof(ExtendedLogData), out var jToken);

            if (jToken != null)
            {
                extendedLogData = jToken.ToObject<ExtendedLogData>();

                activity.Conversation.Properties.Remove(nameof(ExtendedLogData));
            }

            if (msg != null)
            {
                using (var dbContext = new AppDbContext(connectionString))
                {
                    await dbContext.ChatLogs.AddAsync(new ChatLog
                    {
                        Id = 0,
                        ActivityId = msg.Id,
                        ActivityType = msg.Type,
                        ReplyToActivityId = msg.ReplyToId,
                        ConversationId = msg.Conversation.Id,
                        ConversationType = msg.Conversation.ConversationType,
                        ConversationName = msg.Conversation.Name,
                        RecipientId = msg.Recipient?.Id,
                        RecipientName = msg.Recipient?.Name,
                        RasaEntities = extendedLogData?.Entity,
                        RasaIntent = extendedLogData?.IntentName,
                        RasaScore = extendedLogData?.IntentScore,
                        FromId = msg.From?.Id,
                        FromName = msg.From?.Name,
                        Text = msg.Text,
                        ResponseJsonText = extendedLogData?.ResponseJson,
                        ResonseFeedback = null,
                        TimeStamp = msg.Timestamp?.LocalDateTime,
                        ResponseSource = extendedLogData?.ResponseSource,
                        MainTitle = extendedLogData?.MainTitle,
                        SubTitle = extendedLogData?.SubTitle
                    });

                    await dbContext.SaveChangesAsync();
                }
            }

            if (extendedLogData != null)
                activity.ChannelData = null;
        }
    }
}
