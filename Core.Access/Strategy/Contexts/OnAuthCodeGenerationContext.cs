namespace Core.Access.Models.Strategy
{
    public class OnAuthCodeGenerationContext : AbstractStrategyContext<OAuthModel>
    {
        internal string userId { get; set; }
    }
}
