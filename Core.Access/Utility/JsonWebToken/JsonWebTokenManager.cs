using Core.Access.Utility.Authentication;
using Core.Access.Utility.ConfigParser;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Access.Utility
{
    public class JsonWebTokenManager : IJsonWebTokenManager
    {
        public IHttpHelper HttpHelper { get; }
        public IConfigParser ConfigParser { get; }

        public JsonWebTokenManager(IHttpHelper httpHelper, IConfigParser configParser)
        {
            HttpHelper = httpHelper;
            ConfigParser = configParser;
        }

        public JwtSecurityToken GetJwtSecurityToken(IEnumerable<Claim> Claims) => new JwtSecurityToken(claims: Claims, signingCredentials: JsonWebTokenSignatory.Instance.SigningCredentials);

        public string GetRefreshToken() => Guid.NewGuid().ToString().Replace('-', 'A');   
        
        public ClaimsPrincipal ReadJsonWebToken(string accessToken, out SecurityToken token)
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(accessToken, TokenValidationParameters, out var secToken);
            token = secToken;
            return principal;
        }

        public TokenValidationParameters TokenValidationParameters => new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = JsonWebTokenSignatory.Instance.Key,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = HttpHelper.GetCurrentApplicationUrl(),
            ValidAudiences = ConfigParser.JwtAudiences
        };
    }
}
