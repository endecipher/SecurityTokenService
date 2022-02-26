using Core.UserClient.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Threading.Tasks;

namespace Core.UserClient.Policies.OAuthRequirement
{
    public class OAuthRequirementHandler : AuthorizationHandler<OAuthRequirement>
    {
        public OAuthRequirementHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OAuthRequirement requirement)
        {
            if (context.User.Identity.AuthenticationType.Equals(AuthenticationSchemes.OAuth) &&
                context.User.HasClaim(JwtRegisteredClaimNames.Iss, Configuration["AuthorizationServers:Core.Access:Host"]))
            {
                context.Succeed(requirement); 
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
