using Core.Access.Identity.DB;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public class UserGenerationStrategy : AbstractStrategy<UserModel, OnUserGenerationContext>
    {
        public UserGenerationStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) : base(repository, userManager, utilities)
        {
        }

        protected override async Task<bool> Validate()
        {
            var userRegistered = await UserManager.FindByNameAsync(Model.Username);
            var isUserRegistered = !string.IsNullOrEmpty(userRegistered?.Id);

            if (isUserRegistered)
            {
                Model.Errors.Add(Resource.UserAlreadyRegistered);
                Result = new ViewableStrategyResult(Model);

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        protected override async Task<StrategyResult> Process()
        {
            var user = new IdentityUser(Model.Username);

            var claims = Utilities.ClaimManager.GenerateClaimsToPersist(Model);

            var txnResult = Repository.Transaction.Execute(user, (userState, context) =>
            {
                var result = UserManager.CreateAsync(userState, Model.Password).GetAwaiter().GetResult();
                var claimsResult = UserManager.AddClaimsAsync(userState,
                    claims).GetAwaiter().GetResult();

                context.SaveChanges();
            });

            if (txnResult.IsSuccessful)
            {
                Result = new ViewableStrategyResult(Model, "~/Views/Shared/DisplayDetails.cshtml", clearModelState: true, new Dictionary<string, object>
                {
                    ["Info"] = $"{user.UserName} has been registered successfully! Thanks for choosing us!"
                });
            }
            else
            {
                Model.Errors.Add(txnResult.Exception.ToString());
                Result = new ViewableStrategyResult(Model);
            }

            return await Task.FromResult(Result);
        }
    }
}
