using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddDirectLine(this AuthenticationBuilder builder, DirectLineAuthenticationOptions options)
        {

            // a drity hack to configure options
            builder.Services.Configure<DirectLineAuthenticationOptions>(opt =>
            {
                foreach (var pi in opt.GetType().GetProperties())
                {
                    var propValue = pi.GetValue(options);
                    pi.SetValue(opt, propValue);
                }
            });

            return builder.AddJwtBearer(DirectLineDefaults.AuthenticationSchemeName, opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key))
                };
                opt.Events = new JwtBearerEvents
                {
                    // dynamically get token by querystring if the request is a websocket request
                    OnMessageReceived = ctx =>
                    {
                        if (ctx.HttpContext.WebSockets.IsWebSocketRequest && ctx.Request.Query.ContainsKey("t"))
                        {
                            ctx.Token = ctx.Request.Query["t"];
                        }
                        return Task.CompletedTask;
                    },
                };
            });
        }

    }


}