using Microsoft.AspNetCore.Http;

namespace Core.Access.Utility.Authentication
{
    public interface IHttpHelper
    {
        string GetCurrentApplicationUrl();
        BasicCredentials DecodeBasicCredentials(string encodedValue);
        object GetItem(string key);
        void StoreItem(string key, object value);

        string GetQueryValue(string val);
        bool TryGetHeaderValue(string key, out string header);
        IFormCollection GetFormData();
        string GetValue(IFormCollection collection, string val);
    }
}