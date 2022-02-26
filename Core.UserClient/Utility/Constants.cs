namespace Core.UserClient.Utility
{
    public class Constants
    {
        public const string access_token = nameof(access_token);

        public const string refresh_token = nameof(refresh_token);

        public const string grant_type = nameof(grant_type);

        public const string client_id = nameof(client_id);

        public const string Authorization = nameof(Authorization);

        public const string Bearer = nameof(Bearer);

        public const string Basic = nameof(Basic);

        public const string redirect_uri = nameof(redirect_uri);

        public const string client_secret = nameof(client_secret);

        public const string code = nameof(code);
        
        public const string authorization_code = nameof(authorization_code);
        
        public const string ContentTypeApplicationJson = "application/json";
    }

    public class AuthenticationSchemes
    {
        public const string Cookie = nameof(Cookie);

        public const string OAuth = nameof(OAuth);
    }

    public class Policies
    {
        public const string AvailableSchemes = AuthenticationSchemes.OAuth + "," + AuthenticationSchemes.Cookie;

        public const string AdultPolicy = nameof(AdultPolicy);

        public const string HighestSecurityPolicy = nameof(HighestSecurityPolicy);

        public const string OAuthSignedInPolicy = nameof(OAuthSignedInPolicy);
    }

    public class ClaimTypes
    {
        public const string security_level = nameof(security_level);
    }
}
