using Microsoft.Bot.Connector.Authentication;
using System.Threading.Tasks;

namespace DirectLine.Authentication
{
    public class DirectLineCredential : ICredentialProvider
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> GetAppPasswordAsync(string appId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return string.Empty;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> IsAuthenticationDisabledAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return true;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> IsValidAppIdAsync(string appId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return true;
        }
    }
}
