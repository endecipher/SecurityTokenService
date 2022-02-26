using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.ResourceApi.Protection.Handlers
{
    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private HttpClient _client;
        private HttpContext _httpContext;
        const string AuthHeaderValue = "Authorization";

        public JwtRequirementHandler(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue(AuthHeaderValue, out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];

                var serverResponse = await _client
                    .GetAsync($"{Configuration["TrustedAuthorizationServers:Core.Access:ValidateAccessTokenEndpoint"]}{accessToken}");

                if (serverResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
