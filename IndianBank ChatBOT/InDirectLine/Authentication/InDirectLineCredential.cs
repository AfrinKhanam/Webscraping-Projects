using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Connector.Authentication;

namespace InDirectLine.Authentication
{
    public class InDirectLineCredential : ICredentialProvider
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
