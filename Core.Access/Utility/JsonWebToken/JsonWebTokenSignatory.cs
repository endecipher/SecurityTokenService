using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Access.Utility
{
    public class JsonWebTokenSignatory
    {
        private const string SecretKey = "VerySecretJwtKeyUsedForSigning";

        public static JsonWebTokenSignatory Instance = new JsonWebTokenSignatory();

        public SymmetricSecurityKey Key { get; private set; }

        public SigningCredentials SigningCredentials { get; private set; }

        private JsonWebTokenSignatory()
        {
            Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(SecretKey)
            );

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
