using Core.Access.Identity.DB;
using Core.Access.Identity.DB.EntitySets;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public class AuthAttemptStrategy : AbstractStrategy<OAuthModel, OnAuthAttemptContext>
    {
        public AuthAttemptStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) 
            : base(repository, userManager, utilities)
        {

        }

        protected override void Initialize()
        {
            Context.response_type = Utilities.HttpHelper.GetQueryValue(Context.response_type);
            Context.client_id = Utilities.HttpHelper.GetQueryValue(Context.client_id);
            Context.redirect_uri = Utilities.HttpHelper.GetQueryValue(Context.redirect_uri);
            Context.scope = Utilities.HttpHelper.GetQueryValue(Context.scope);
            Context.state = Utilities.HttpHelper.GetQueryValue(Context.state);
        }

        protected override async Task<bool> Validate()
        {
            /// <remarks>
            /// See https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1 for error formats.
            /// </remarks>

            if (Context.response_type != Strings.OAuthFlow.code)
            {
                Result = new BadRequestStrategyResult
                {
                    error = Strings.OAuthFlow.unsupported_response_type,
                    error_description = Resource.OnlyAuthorizeCodeFlowEnabled,
                };

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        protected override async Task<StrategyResult> Process()
        {
            /// <remarks>
            /// If no client found in persistence store with the "client_id", then return error.
            /// </remarks>

            if (Repository.FetchClientById(Context.client_id).Evaluate(out Client clientEntity,
                OnInternalFailure: (op) => throw op.Exception,
                OnEntityNotFound: (op) =>
                {
                    Result = new BadRequestStrategyResult
                    {
                        error = Strings.OAuthFlow.unsupported_response_type,
                        error_description = Resource.UnknownClient,
                    };
                }))
            {
                Context.Model = new OAuthModel
                {
                    ReturnUrl = Context.redirect_uri,
                    State = Context.state,
                    ClientId = clientEntity.ClientId,
                    ClientName = clientEntity.FriendlyName
                };

                Result = new ViewableStrategyResult(Context.Model);
            }

            return await Task.FromResult(Result);
        }
    }
}
