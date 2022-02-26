namespace Core.Access.Utility
{
    public class Strings
    {
        public class AuthenticationSchemes
        {
            public const string Default = nameof(Default);
        }

        public class Common
        {
            public const string Authorization = nameof(Authorization);

            public const string Bearer = nameof(Bearer);

            public const string Basic = nameof(Basic);

            public const string access_token = nameof(access_token);

            public const string client_name = nameof(client_name);

            public const string UrlEncodedContentType = "application/x-www-form-urlencoded";

            public const string JsonContentType = "application/json";
        }

        public class OAuthFlow
        {
            public const string code = nameof(code);

            public const string state = nameof(state);

            public const string unsupported_response_type = nameof(unsupported_response_type);

            public const string access_denied = nameof(access_denied);

            public const string invalid_client = nameof(invalid_client);

            public const string authorization_code = nameof(authorization_code);

            public const string refresh_token = nameof(refresh_token);

            public const string unsupported_grant_type = nameof(unsupported_grant_type);

            public const string invalid_grant = nameof(invalid_grant);

            public const string invalid_request = nameof(invalid_request);
        }

        public class ClaimTypes
        {
            public static string Client => Common.client_name;

            public const string security_level = nameof(security_level);

            public const string refresh_hint = nameof(refresh_hint);
        }
    }
}
