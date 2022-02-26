using Newtonsoft.Json;
using System.Text;

namespace Core.Access.Utility
{
    public class JsonResponse
    {
        public JsonResponse()
        {

        }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string token_type { get; } = Strings.Common.Bearer;

        public byte[] GetSerializedEncodedBytes()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        } 
    }
}
