using Core.UserClient.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Core.UserClient.Policies.SecurityLevelRequirement
{
    public class SecurityLevelHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecurityLevelRequirement requirement)
        {
            var claimSecurityLevel = context.User.FindFirst(ClaimTypes.security_level)?.Value;

            if (claimSecurityLevel != null && SecurityLevelRequirement.IsAcceptable(claimSecurityLevel, requirement.Level))
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
