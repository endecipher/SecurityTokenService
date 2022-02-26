using Core.Access.Identity.DB;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public class TokenValidationStrategy : AbstractStrategy<OAuthModel, OnTokenValidationContext>
    {
        public TokenValidationStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) : base(repository, userManager, utilities)
        {
        }

        protected override void Initialize()
        {
            Context.access_token = Utilities.HttpHelper.GetQueryValue(Context.access_token);
        }

        protected override async Task<bool> Validate()
        {
            if (string.IsNullOrEmpty(Context.access_token))
            {
                Result = new BadRequestStrategyResult
                {
                    error = Strings.OAuthFlow.invalid_request,
                    error_description = "Access Token Not provided"
                };

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        protected override async Task<StrategyResult> Process()
        {
            try
            {
                var principal = Utilities.JsonWebTokenManager.ReadJsonWebToken(Context.access_token, out var token);

                Result = new OkStrategyResult();
            }
            catch (SecurityTokenExpiredException ex)
            {
                Result = new UnAuthorizedStrategyResult
                {
                    error = Strings.OAuthFlow.invalid_request,
                    error_description = Resource.TokenExpired + ex.Message
                };
            }

            return await Task.FromResult(Result);
        }
    }
}
