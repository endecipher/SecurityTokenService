using System;
using System.Text;

namespace Core.UserClient.Utility
{
    public static class Helper
    {
        public static Func<string, string> AppendBearer = (x) => $"{Constants.Bearer} {x}";

        public static Func<string, string> AppendBasic = (x) => $"{Constants.Basic} {x}";

        public static string FetchEncodedCredentialsForBasicAuth(string value1, string value2)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{value1}:{value2}"));
        }

        public static string GetJSONSerializedPayloadFromAccessToken(string token)
        {
            var payload = token.Split('.')[1];

            var remainderLengthUntilMultipleOfFour = (4 - (payload.Length % 4)) % 4;

            for (int i = 1; i <= remainderLengthUntilMultipleOfFour; i++)
            {
                payload += "=";
            }

            var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            return decodedString;
        }
    }
}
