namespace Core.Access.Models.Strategy
{
    public class OnAuthAttemptContext : AbstractStrategyContext<OAuthModel>
    {
        internal string response_type = nameof(response_type);
        internal string client_id = nameof(client_id);
        internal string redirect_uri = nameof(redirect_uri);
        internal string scope = nameof(scope);
        internal string state = nameof(state);
    }
}
