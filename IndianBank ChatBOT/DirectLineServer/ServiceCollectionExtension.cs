using BOTAIML.ChatBot.DirectLineServer.Core.Authorization;
using BOTAIML.ChatBot.DirectLineServer.Core.Middlewares;
using BOTAIML.ChatBot.DirectLineServer.Core.Services;
using BOTAIML.ChatBot.DirectLineServer.Core.Services.IDirectLineConnections;
using BOTAIML.ChatBot.DirectLineServer.Core.Utils;
using DirectLine.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace BOTAIML.ChatBot.DirectLineServer.Core
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddDirectLine(this IServiceCollection services, DirectLineSettings directlineOpts)
        {
            services.Configure<DirectLineSettings>(opt =>
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
            services.AddSingleton<ICredentialProvider, DirectLineCredential>();

            services.AddScoped<AuthenticationConfiguration>();

            //services.AddSingleton<IDirectLineConnection, WebSocketDirectLineConnection>();
            services.AddSingleton<IDirectLineConnectionManager, DirectLineConnectionManager>();
            services.AddSingleton<TokenBuilder>();
            services.AddScoped<ChannelServiceHandler, DirectLineConversationHandler>();
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("MatchConversation", pb => pb.Requirements.Add(new MatchConversationAuthzRequirement()));
            });
            services.AddHttpClient<DirectLineClient>();

            var botEndPointUri = UtilsEx.GetOrigin(directlineOpts.BotEndpoint);
            services.AddSingleton<IAuthorizationHandler, MatchConversationAuthzHandler>();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    DirectLineDefaults.CorsPolicyNames,
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