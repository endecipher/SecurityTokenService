using Core.Access.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Access.Utility.Claims
{
    public interface IClaimManager
    {
        List<Claim> GenerateClaimsToPersist(UserModel user);

        Task<List<Claim>> GetClaimsForAccessToken(IdentityUser user);

        Task<List<Claim>> GetClaimsForRefreshToken(IdentityUser user);
    }
}