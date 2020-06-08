


using BOTAIML.ChatBot.DirectLineServer.Core.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Services
{
    public class TokenBuilder
    {

        public const string ClaimTypeConversationID = "Claim_Type:BOTAIML.ChatBot.DirectLineServer.Core:ConversationID";
        private DirectLineAuthenticationOptions _authenticationOpts;

        public TokenBuilder(IOptions<DirectLineAuthenticationOptions> DirectLineAuthenticatoinOpts)
        {
            this._authenticationOpts = DirectLineAuthenticatoinOpts.Value ?? throw new ArgumentNullException(nameof(DirectLineAuthenticatoinOpts));
        }

        public string BuildToken(string userName, IList<Claim> claims, int expireTime)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            claims.Add(new Claim(ClaimTypes.Name, userName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._authenticationOpts.Key));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiredAt = DateTime.UtcNow.AddSeconds(expireTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiredAt,
                Issuer = this._authenticationOpts.Issuer,
                Audience = this._authenticationOpts.Audience,
                SigningCredentials = sign,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string RefreshToken()
        {
            // todo
            return "";
        }


    }
}