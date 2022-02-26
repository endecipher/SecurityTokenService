using Core.Access.Encryption;
using Core.Access.Utility.Authentication;
using Core.Access.Utility.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Core.Access.Utility
{
    public interface IUtilities
    {
        public IHttpHelper HttpHelper { get; }
        public IJsonWebTokenManager JsonWebTokenManager { get; }
        public IClaimManager ClaimManager { get; }
        public IEncryptionHelper EncryptionHelper { get; }

        Task<JsonResponse> GenerateTokenResponse(IdentityUser user, bool onRefresh = false);
    }
}