using Core.Access.Encryption;
using Core.Access.Utility.Authentication;
using Core.Access.Utility.Claims;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Core.Access.Utility
{
    public class Utilities : IUtilities
    {
        public Utilities(IHttpHelper httpAuthHelper, IJsonWebTokenManager jsonWebTokenManager, IClaimManager claimManager, IEncryptionHelper encryptionHelper)
        {
            HttpHelper = httpAuthHelper;
            JsonWebTokenManager = jsonWebTokenManager;
            ClaimManager = claimManager;
            EncryptionHelper = encryptionHelper;
        }

        public IHttpHelper HttpHelper { get; }
        public IJsonWebTokenManager JsonWebTokenManager { get; }
        public IClaimManager ClaimManager { get; }
        public IEncryptionHelper EncryptionHelper { get; }

        public async Task<JsonResponse> GenerateTokenResponse(IdentityUser user, bool onRefresh = false)
        {
            List<System.Security.Claims.Claim> claims;

            if (onRefresh)
            {
                claims = await ClaimManager.GetClaimsForAccessToken(user);
            }
            else
            {
                claims = await ClaimManager.GetClaimsForAccessToken(user);
            }

            var accessToken = new JwtSecurityTokenHandler().WriteToken(JsonWebTokenManager.GetJwtSecurityToken(claims));
            var refreshToken = JsonWebTokenManager.GetRefreshToken();

            return new JsonResponse
            {
                access_token = accessToken,
                refresh_token = refreshToken
            };
        }
    }
}
