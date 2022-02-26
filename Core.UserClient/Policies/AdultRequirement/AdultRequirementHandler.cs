using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Threading.Tasks;

namespace Core.UserClient.Policies.AdultRequirement
{
    public class AdultRequirementHandler : AuthorizationHandler<AdultRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdultRequirement requirement)
        {
            var dateOfBirth = context.User.FindFirst(JwtRegisteredClaimNames.Birthdate)?.Value;

            if (dateOfBirth != null && AdultRequirement.IsAdult(dateOfBirth))
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
