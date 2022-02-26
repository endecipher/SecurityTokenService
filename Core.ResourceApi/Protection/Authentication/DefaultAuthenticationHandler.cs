using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Core.ResourceApi.Protection.Authentication
{
    public class DefaultAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DefaultAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        /// <summary>
        /// Standalone Authentication not required since each request will be authorized externally
        /// </summary>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail(Resource.AuthFailed));
        }
    }
}
