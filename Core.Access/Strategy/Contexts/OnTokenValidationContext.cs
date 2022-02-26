namespace Core.Access.Models.Strategy
{
    public class OnTokenValidationContext : AbstractStrategyContext<OAuthModel>
    {
        internal string access_token { get; set; } = nameof(access_token);
    }
}
