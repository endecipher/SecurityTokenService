using Core.UserClient.Utility;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.UserClient.Handlers
{
    public class CustomOAuthHandler : OAuthHandler<OAuthOptions>
    {
        public CustomOAuthHandler(IOptionsMonitor<OAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, Microsoft.AspNetCore.Authentication.ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
                {
                    { Constants.client_id, Options.ClientId },
                    { Constants.redirect_uri, context.RedirectUri },
                    { Constants.client_secret, Options.ClientSecret },
                    { Constants.code, context.Code },
                    { Constants.grant_type, Constants.authorization_code },
                };

            // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
            if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
            {
                tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
                context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
            }

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);

            HttpResponseMessage response;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint))
            {
                /// <remarks>
                /// Adding Custom Logic for OAuthHandler.cs -- add Client Authentication for Backchannel communication via Headers
                /// See https://github.com/dotnet/aspnetcore/tree/8bb128185b58a26065d0f29e695a2410cf0a3c68/src/Security/Authentication/OAuth/src 
                /// </remarks>
                requestMessage.Headers.Add(Constants.Authorization, Helper.AppendBasic(Helper.FetchEncodedCredentialsForBasicAuth(Options.ClientId, Options.ClientSecret)));
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ContentTypeApplicationJson));
                requestMessage.Content = requestContent;
                requestMessage.Version = Backchannel.DefaultRequestVersion;
                response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            }

            using (response)
            {
                if (response.IsSuccessStatusCode)
                {
                    var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
                    return OAuthTokenResponse.Success(payload);
                }
                else
                {
                    return OAuthTokenResponse.Failed(new ApplicationException(Resource.BackchannelError));
                }
            }
        }
    }
}
