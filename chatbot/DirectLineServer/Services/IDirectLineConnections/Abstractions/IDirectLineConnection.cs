

using System;
using System.Threading.Tasks;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Services.IDirectLineConnections
{
    public interface IDirectLineConnection
    {
        Task<(bool, ArraySegment<byte>)> ReceiveAsync();
        Task SendAsync(ArraySegment<byte> buffer);
        Task CloseAsync(object status, string reason);
        bool Available { get; }
    }

}