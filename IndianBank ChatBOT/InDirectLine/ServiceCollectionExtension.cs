using InDirectLine.Authentication;

using Itminus.InDirectLine.Core.Authorization;
using Itminus.InDirectLine.Core.Middlewares;
using Itminus.InDirectLine.Core.Services;
using Itminus.InDirectLine.Core.Services.IDirectLineConnections;
using Itminus.InDirectLine.Core.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Itminus.InDirectLine.Core
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddInDirectLine(this IServiceCollection services, InDirectLineSettings directlineOpts)
        {
            services.Configure<InDirectLineSettings>(opt =>
            {
                foreach (var pi in opt.GetType().GetProperties())
                {
                    var propValue = pi.GetValue(directlineOpts);
                    pi.SetValue(opt, propValue);
                }
            });

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddSingleton<IConversationHistoryStore, InMemoryConversationHistoryStore>();
            services.AddSingleton<ICredentialProvider, InDirectLineCredential>();

            services.AddScoped<AuthenticationConfiguration>();

            //services.AddSingleton<IDirectLineConnection, WebSocketDirectLineConnection>();
            services.AddSingleton<IDirectLineConnectionManager, DirectLineConnectionManager>();
            services.AddSingleton<TokenBuilder>();
            services.AddScoped<ChannelServiceHandler, InDirectLineConversationHandler>();
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("MatchConversation", pb => pb.Requirements.Add(new MatchConversationAuthzRequirement()));
            });
            services.AddHttpClient<InDirectLineClient>();

            var botEndPointUri = UtilsEx.GetOrigin(directlineOpts.BotEndpoint);
            services.AddSingleton<IAuthorizationHandler, MatchConversationAuthzHandler>();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    InDirectLineDefaults.CorsPolicyNames,
                    builder =>
                    {
                        //builder.WithOrigins(botEndPointUri);
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    }
                );
            });
            services.AddScoped<WebSocketConnectionMiddleware>();
            return services.AddScoped<DirectLineHelper>();
        }

    }
}