using BOTAIML.ChatBot.DirectLineServer.Core.Models;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Services
{


    public interface IConversationHistoryStore
    {

        Task<bool> ConversationExistsAsync(string conversationId);
        Task CreateConversationIfNotExistsAsync(string conversationId);
        Task AddActivityAsync(string conversationId, Activity activity);

        Task<ActivitySet> GetActivitySetAsync(string conversationId, int watermark);
    }


}