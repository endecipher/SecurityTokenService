using Core.Access.Identity.DB;
using Core.Access.Identity.DB.EntitySets;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public class CodeGenerationStrategy : AbstractStrategy<OAuthModel, OnAuthCodeGenerationContext>
    {
        public CodeGenerationStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) : base(repository, userManager, utilities)
        {
        }

        protected override async Task<bool> Validate()
        {
            var user = await UserManager.FindByNameAsync(Model.Username);

            /// <remarks>
            /// If no user found in Identity store, then return error.
            /// </remarks>
            if (user == null)
            {
                Result = new BadRequestStrategyResult
                {
                    error = Strings.OAuthFlow.access_denied,
                    error_description = Resource.UnknownUser
                };

                return await Task.FromResult(false);
            }

            if (!await UserManager.CheckPasswordAsync(user, Context.Model.Password))
            {
                Result = new BadRequestStrategyResult
                {
                    error = Strings.OAuthFlow.access_denied,
                    error_description = Resource.PasswordsDoNotMatch
                };

                return await Task.FromResult(false);
            }

            Context.userId = user.Id;

            return await Task.FromResult(true);
        }

        protected override async Task<StrategyResult> Process()
        {
            ClientCodeRequest request = new ClientCodeRequest
            {
                ClientId = Model.ClientId,
                UserId = Context.userId,
                IsActive = true,
                Code = Guid.NewGuid().ToString(),
                RedirectURI = Model.ReturnUrl
            };

            if (Repository.CreateClientCodeRequest(request).Evaluate(OnInternalFailure: (op) => throw op.Exception))
            {
                var query = new QueryBuilder();
                query.Add(Strings.OAuthFlow.state, Model.State);
                query.Add(Strings.OAuthFlow.code, request.Code);

                return await Task.FromResult(new RedirectionStrategyResult($"{Model.ReturnUrl}{query}"));
            }

            return await Task.FromResult(Result);
        }
    }
}
