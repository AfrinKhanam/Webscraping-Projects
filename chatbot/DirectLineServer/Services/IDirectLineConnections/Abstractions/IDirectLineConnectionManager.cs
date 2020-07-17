using System.Threading.Tasks;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Services.IDirectLineConnections
{

    public interface IDirectLineConnectionManager
    {
        Task<IDirectLineConnection> GetConnectionAsync(string conversationId);

        Task RegisterConnectionAsync(string conversationId, IDirectLineConnection connection);

        Task RemoveConnectionAsync(string conversationId);
        Task SendAsync(string conversationId, string txt);

    }


}
