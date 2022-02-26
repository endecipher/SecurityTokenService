namespace Core.Access.Models.Strategy
{
    public class OnTokenExchangeContext : AbstractStrategyContext<OAuthModel>
    {
        public string grant_type { get; set; } = nameof(grant_type);
        public string client_id { get; set; } = nameof(client_id);
        public string client_secret { get; set; } = nameof(client_secret);
        public string redirect_uri { get; set; } = nameof(redirect_uri);
        public string code { get; set; } = nameof(code);
        public string refresh_token { get; set; } = nameof(refresh_token);
    }
}
