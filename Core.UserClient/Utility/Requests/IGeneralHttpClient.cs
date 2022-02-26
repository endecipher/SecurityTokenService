using System.Net.Http;
using System.Threading.Tasks;

namespace Core.UserClient.Utility.Requests
{
    public interface IGeneralHttpClient
    {
        Task<HttpResponseMessage> FireWithAccessToken(string url);
    }
}
