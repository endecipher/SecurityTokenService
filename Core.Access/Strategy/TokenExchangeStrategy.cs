using Core.Access.Identity.DB;
using Core.Access.Identity.DB.EntitySets;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using static Core.Access.Utility.Strings;

namespace Core.Access.Models.Strategy
{
    public class TokenExchangeStrategy : AbstractStrategy<OAuthModel, OnTokenExchangeContext>
    {
        public TokenExchangeStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) : base(repository, userManager, utilities)
        {
        }

        protected override void Initialize()
        {
            var formData = Utilities.HttpHelper.GetFormData();

            Context.grant_type = Utilities.HttpHelper.GetValue(formData, Context.grant_type);
            Context.client_id = Utilities.HttpHelper.GetValue(formData, Context.client_id);
            Context.client_secret = Utilities.HttpHelper.GetValue(formData, Context.client_secret);
            Context.redirect_uri = Utilities.HttpHelper.GetValue(formData, Context.redirect_uri);
            Context.code = Utilities.HttpHelper.GetValue(formData, Context.code);
            Context.refresh_token = Utilities.HttpHelper.GetValue(formData, Context.refresh_token);
        }

        protected override async Task<bool> Validate()
        {
            /// <remarks>
            /// Basic Authentication to validate client identity.
            /// </remarks>
            var isAuthHeaderPresent = Utilities.HttpHelper.TryGetHeaderValue(Common.Authorization, out var header);

            string headerVal = header.ToString();

            if (!isAuthHeaderPresent || !headerVal.StartsWith(Common.Basic))
            {
                Result = new UnAuthorizedStrategyResult
                {
                    error = OAuthFlow.invalid_client
                };

                return await Task.FromResult(false);
            }

            Utility.Authentication.BasicCredentials basicCredentials = Utilities.HttpHelper.DecodeBasicCredentials(header.ToString());

            if (!basicCredentials.IsIdentifierMatchingWith(Context.client_id))
            {
                Result = new BadRequestStrategyResult
                {
                    error = OAuthFlow.invalid_client,
                };

                return await Task.FromResult(false);
            }

            if (Repository.FetchClientById(Context.client_id).Evaluate(out Client clientEntity,
                OnInternalFailure: (op) => throw op.Exception,
                OnEntityNotFound: (op) =>
                {
                    Result = new BadRequestStrategyResult
                    {
                        error = OAuthFlow.invalid_client,
                        error_description = Resource.UnknownClient,
                    };
                }))
            {
                /// <remarks>
                /// Set ClientName in HttpContext for preventing DB hit from ClaimsManager
                /// </remarks>
                Utilities.HttpHelper.StoreItem(Strings.Common.client_name, clientEntity.FriendlyName);

                bool isPasswordMatching = Client.Concatenate(clientEntity.ClientId, clientEntity.FriendlyName, basicCredentials.Secret)
                    .Equals(Utilities.EncryptionHelper.DecryptString(clientEntity.Password));

                if (!isPasswordMatching)
                {
                    Result = new BadRequestStrategyResult
                    {
                        error = OAuthFlow.invalid_client,
                        error_description = Resource.UnknownClient,
                    };

                    return await Task.FromResult(false);
                }

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        protected override async Task<StrategyResult> Process()
        {
            /// <remarks>
            /// See https://datatracker.ietf.org/doc/html/rfc6749#section-5.2 for error formats within the flows.
            /// </remarks>
            switch (Context.grant_type)
            {
                case OAuthFlow.authorization_code:
                    await AuthorizationCodeFlow();
                    break;
                case OAuthFlow.refresh_token:
                    await RefreshTokenCodeFlow();
                    break;
                default:
                    Result = new BadRequestStrategyResult
                    {
                        error = Strings.OAuthFlow.unsupported_grant_type
                    };
                    break;
            }


            return await Task.FromResult(Result);
        }

        private async Task RefreshTokenCodeFlow()
        {
            if (string.IsNullOrEmpty(Context.refresh_token))
            {
                Result = new BadRequestStrategyResult
                {
                    error = Strings.OAuthFlow.invalid_request,
                    error_description = "Refresh Token not supplied"
                };

                return;
            }

            if (Repository.FetchClientTokenRequest(x => x.RefreshToken == Context.refresh_token && x.ClientId == Context.client_id)
                .Evaluate<ClientTokenRequest>(out var tokenRequest, OnInternalFailure: (op) => throw op.Exception,
                OnEntityNotFound: (op) =>
                {
                    Result = new BadRequestStrategyResult
                    {
                        error = Strings.OAuthFlow.invalid_grant,
                        error_description = "Client Token Request not registered",
                    };
                }))
            {
                var user = await UserManager.FindByIdAsync(tokenRequest.UserId);

                var jsonResponse = await Utilities.GenerateTokenResponse(onRefresh: true, user: user);

                //var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(JSONResponse));

                var txnResult = Repository.Transaction.Execute<long>(tokenRequest.Id, (stateResult, dbContext) =>
                {
                    /// <remarks>
                    /// Inactivating token request to issue a new one.
                    /// </remarks>
                    tokenRequest.IsActive = false;

                    dbContext.ClientTokenRequests.Update(tokenRequest);

                    dbContext.ClientTokenRequests.Add(new ClientTokenRequest
                    {
                        ClientId = Context.client_id,
                        AccessToken = jsonResponse.access_token,
                        RefreshToken = jsonResponse.refresh_token,
                        UserId = user.Id,
                        IsActive = true
                    });

                    dbContext.SaveChanges();
                });

                if (txnResult.IsSuccessful)
                {
                    Result = new JsonStrategyResult(jsonResponse);
                }
                else
                {
                    Result = new ExceptionResult(txnResult.Exception);
                }
            }

            return; 
        }

        private async Task AuthorizationCodeFlow()
        {
            if (Repository.
                FetchClientCodeRequest(x =>
                    x.IsActive &&
                    x.RedirectURI == Context.redirect_uri &&
                    x.ClientId == Context.client_id &&
                    x.Code == Context.code)
                .Evaluate<ClientCodeRequest>(out var codeRequest, OnInternalFailure: (op) => throw op.Exception,
                OnEntityNotFound: (op) =>
                {
                    Result = new BadRequestStrategyResult
                    {
                        error = Strings.OAuthFlow.invalid_grant,
                        error_description = "Client Code Request not found"
                    };
                }))
            {
                var user = await UserManager.FindByIdAsync(codeRequest.UserId);

                var jsonResponse = await Utilities.GenerateTokenResponse(onRefresh: false, user: user);

                var txnResult = Repository.Transaction.Execute<long>(codeRequest.Id, (stateResult, dbContext) =>
                {
                    /// <remarks>
                    /// Since Code Request will be used, we can make it inactive
                    /// </remarks>
                    codeRequest.IsActive = false;

                    dbContext.ClientCodeRequests.Update(codeRequest);

                    dbContext.ClientTokenRequests.Add(new ClientTokenRequest
                    {
                        ClientId = Context.client_id,
                        AccessToken = jsonResponse.access_token,
                        RefreshToken = jsonResponse.refresh_token,
                        UserId = user.Id,
                        IsActive = true
                    });

                    dbContext.SaveChanges();
                });

                if (txnResult.IsSuccessful)
                {
                    Result = new JsonStrategyResult(jsonResponse);
                }
                else
                {
                    Result = new ExceptionResult(txnResult.Exception);
                }
            }
        }
    }
}
