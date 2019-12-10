// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace IndianBank_ChatBOT.ServiceClients
{
    public class GraphClient
    {
        private readonly string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphClient"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public GraphClient(string token)
        {
            _token = token;
        }
        /// <summary>
        /// Gets me.
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetMe()
        {
            var graphClient = GetAuthenticatedClient();
            var me = await graphClient.Me.Request().GetAsync();
            return me;
        }
        /// <summary>
        /// Gets the authenticated client.
        /// </summary>
        /// <returns></returns>
        private GraphServiceClient GetAuthenticatedClient()
        {
            var graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        await Task.Run(() =>
                        {
                            var accessToken = _token;
                            // Append the access token to the request.
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                            // Get event times in the current time zone.
                            requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");
                        });
                    }));

            return graphClient;
        }
    }
}
