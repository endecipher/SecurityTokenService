using Core.Access.Models;
using Core.Access.Utility.Authentication;
using Core.Access.Utility.ConfigParser;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Access.Utility.Claims
{
    public class ClaimManager : IClaimManager
    {
        public ClaimManager(IHttpHelper httpHelper, UserManager<IdentityUser> userManager, IConfigParser configParser)
        {
            HttpHelper = httpHelper;
            UserManager = userManager;
            ConfigParser = configParser;
        }

        public IHttpHelper HttpHelper { get; }
        public UserManager<IdentityUser> UserManager { get; }
        public IConfigParser ConfigParser { get; }

        private List<Claim> DynamicClaims => new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Iss, HttpHelper.GetCurrentApplicationUrl()),
            new Claim(JwtRegisteredClaimNames.Aud, ConfigParser.JwtAudiences.First()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddSeconds(ConfigParser.JwtExpiryInSeconds.Value).ToUnixTimeSeconds().ToString()),
            new Claim(Strings.ClaimTypes.Client, HttpHelper.GetItem(Strings.Common.client_name).ToString())
        };

        public List<Claim> GenerateClaimsToPersist(UserModel user)
        {
            return new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.ToShortDateString()),
                new Claim(Strings.ClaimTypes.security_level, user.SecurityLevel.ToString()),
            };
        }

        public async Task<List<Claim>> GetClaimsForAccessToken(IdentityUser user)
        {
            return DynamicClaims.Concat(await UserManager.GetClaimsAsync(user)).ToList();
        }

        public async Task<List<Claim>> GetClaimsForRefreshToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(Strings.ClaimTypes.refresh_hint, Resource.RefreshDesc)
            };

            claims.AddRange(await GetClaimsForAccessToken(user));

            return claims;
        }
    }
}
