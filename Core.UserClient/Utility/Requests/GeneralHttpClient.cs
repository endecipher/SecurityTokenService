using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.UserClient.Utility.Requests
{
    public class GeneralHttpClient : IGeneralHttpClient
    {
        internal string RefreshTokenEndpoint => Configuration["AuthorizationServers:Core.Access:TokenEndpoint"];
        internal string ClientId => Configuration["AuthorizationServers:Core.Access:Client_Id"];
        internal string ClientSecret => Configuration["AuthorizationServers:Core.Access:Client_Secret"];

        public GeneralHttpClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            HttpContextAccessor = httpContextAccessor;
            HttpClientFactory = httpClientFactory;
            Configuration = configuration;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        public IHttpClientFactory HttpClientFactory { get; }
        public IConfiguration Configuration { get; }


        public async Task<HttpResponseMessage> FireWithAccessToken(string url)
        {
            HttpResponseMessage response = await Fire(url);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response;
                case HttpStatusCode.Unauthorized:
                    return await FireOriginalRequestWithResfreshToken(url).ContinueWith<HttpResponseMessage>((previousTask, originalUrl) =>
                    {
                        var refreshTokenResponse = previousTask.Result;

                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            return Fire(originalUrl.ToString()).GetAwaiter().GetResult();
                        }
                        else
                        {
                            return refreshTokenResponse;
                        }

                    }, state: url);
                default:
                    return response;
            }
        }

        private async Task<HttpResponseMessage> Fire(string url)
        {
            var accessToken = await HttpContextAccessor.HttpContext.GetTokenAsync(Constants.access_token);

            var client = HttpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add(Constants.Authorization, Helper.AppendBearer(accessToken));

            var response = await client.GetAsync(url);
            return response;
        }

        private async Task<HttpResponseMessage> FireOriginalRequestWithResfreshToken(string url)
        {
            var client = HttpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, RefreshTokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    [Constants.grant_type] = Constants.refresh_token,
                    [Constants.refresh_token] = await HttpContextAccessor.HttpContext.GetTokenAsync(Constants.refresh_token),
                    [Constants.client_id] = ClientId
                }),
            };

            request.Headers.Add(Constants.Authorization, Helper.AppendBasic(Helper.FetchEncodedCredentialsForBasicAuth(
                ClientId,
                ClientSecret
            )));

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());

                var authInfo = await HttpContextAccessor.HttpContext.AuthenticateAsync(UserClient.Utility.AuthenticationSchemes.Cookie);

                authInfo.Properties.UpdateTokenValue(Constants.access_token, responseData.GetValueOrDefault(Constants.access_token));
                authInfo.Properties.UpdateTokenValue(Constants.refresh_token, responseData.GetValueOrDefault(Constants.refresh_token));

                await HttpContextAccessor.HttpContext.SignInAsync(UserClient.Utility.AuthenticationSchemes.Cookie, authInfo.Principal, authInfo.Properties);
            }

            return response;
        }
    }
}
