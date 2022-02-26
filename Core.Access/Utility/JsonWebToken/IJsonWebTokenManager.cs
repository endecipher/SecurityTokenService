using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Access.Utility
{
    public interface IJsonWebTokenManager
    {
        JwtSecurityToken GetJwtSecurityToken(IEnumerable<Claim> Claims);

        string GetRefreshToken();

        TokenValidationParameters TokenValidationParameters { get; }

        ClaimsPrincipal ReadJsonWebToken(string accessToken, out SecurityToken token);
    }
}