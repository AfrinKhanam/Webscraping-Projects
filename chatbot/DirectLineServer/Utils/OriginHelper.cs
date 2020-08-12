
using System;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Utils
{
    public static class UtilsEx
    {
        public static string GetOrigin(string url)
        {
            var endpoint = new UriBuilder(url);
            var b = new UriBuilder();
            b.Scheme = endpoint.Scheme;
            b.Host = endpoint.Host;
            b.Port = endpoint.Port;
            return b.Uri.ToString().TrimEnd('/');
        }

        public static string GetWebSocketOrigin(string scheme, string url)
        {
            var endpoint = new UriBuilder(url);
            
            var b = new UriBuilder
            {
                Scheme = scheme,
                Host = endpoint.Host,
                Port = endpoint.Port
            };

            return b.Uri.ToString().TrimEnd('/');
        }

    }
}