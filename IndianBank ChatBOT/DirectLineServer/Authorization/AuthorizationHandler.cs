using BOTAIML.ChatBot.DirectLineServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Authorization
{
    public class MatchConversationAuthzHandler : AuthorizationHandler<MatchConversationAuthzRequirement>
    {
        private readonly ILogger<MatchConversationAuthzHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MatchConversationAuthzHandler(ILogger<MatchConversationAuthzHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MatchConversationAuthzRequirement requirement)
        {
            var httpContext = this._httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
            }
            else
            {
                var conversationId = httpContext.GetRouteValue("conversationid") as string;
                if (string.IsNullOrEmpty(conversationId))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
                var conversationMatches = context.User.HasClaim(c =>
                      c.Type == TokenBuilder.ClaimTypeConversationID &&
                      c.Value == conversationId
                );
                if (conversationMatches)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }

    }
}