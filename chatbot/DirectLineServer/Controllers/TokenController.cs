
using BOTAIML.ChatBot.DirectLineServer.Core.Models;
using BOTAIML.ChatBot.DirectLineServer.Core.Services;
using BOTAIML.ChatBot.DirectLineServer.Core.Services.IDirectLineConnections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Controllers
{

    [ApiController]
    public class TokensController : Controller
    {
        private ILogger<TokensController> _logger;
        private readonly DirectLineHelper _helper;
        private readonly IDirectLineConnectionManager _connectionManager;
        private readonly TokenBuilder _tokenBuilder;
        private DirectLineSettings _DirectLineSettings;

        public TokensController(ILogger<TokensController> logger, IOptions<DirectLineSettings> opt, DirectLineHelper helper, IDirectLineConnectionManager connectionManager, TokenBuilder tokenBuilder)
        {
            this._logger = logger;
            this._helper = helper;
            this._connectionManager = connectionManager;
            this._tokenBuilder = tokenBuilder;
            this._DirectLineSettings = opt.Value;
        }


        [HttpPost("v3/directline/[controller]/generate")]
        public IActionResult Generate([FromBody] TokenCreationPayload payload)
        {
            // according to https://docs.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-authentication?view=azure-bot-service-4.0#generate-token-versus-start-conversation
            //     we don't start a conversation , we just issue a new token that is valid for specific conversation
            var userId = payload.UserId;
            var conversationId = Guid.NewGuid().ToString();
            var claims = new List<Claim>();
            claims.Add(new Claim(TokenBuilder.ClaimTypeConversationID, conversationId));
            var expiresIn = this._DirectLineSettings.TokenExpiresIn;
            var token = this._tokenBuilder.BuildToken(userId, claims, expiresIn);
            return new OkObjectResult(new DirectLineConversation
            {
                ConversationId = conversationId,
                Token = token,
                ExpiresIn = expiresIn,
            });
        }


        [HttpPost("v3/directline/[controller]/refresh")]
        [Authorize(AuthenticationSchemes = DirectLineDefaults.AuthenticationSchemeName)]
        public IActionResult Refresh()
        {
            var conversationId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == TokenBuilder.ClaimTypeConversationID)?.Value;
            if (string.IsNullOrEmpty(conversationId))
            {
                return BadRequest("there's no valid conversationID");
            }
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("there's no valid userId");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(TokenBuilder.ClaimTypeConversationID, conversationId));
            var expiresIn = this._DirectLineSettings.TokenExpiresIn;
            var token = this._tokenBuilder.BuildToken(userId, claims, expiresIn);
            return new OkObjectResult(new DirectLineConversation
            {
                ConversationId = conversationId,
                Token = token,
                ExpiresIn = expiresIn,
            });
        }


    }

}