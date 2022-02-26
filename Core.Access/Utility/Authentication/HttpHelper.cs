using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace Core.Access.Utility.Authentication
{
    public class HttpHelper : IHttpHelper
    {
        public IHttpContextAccessor HttpContextAccessor { get; }

        public HttpHelper(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public string GetCurrentApplicationUrl()
        {
            var request = HttpContextAccessor.HttpContext.Request;
            return string.Format("{0}://{1}", request.Scheme, request.Host);
        }

        public BasicCredentials DecodeBasicCredentials(string encodedValue)
        {
            var result = Encoding.UTF8.GetString(Convert.FromBase64String(encodedValue.Substring(6))).Split(':');
            
            return new BasicCredentials
            {
                Identifier = result[0],
                Secret = result[1]
            };
        }

        public object GetItem(string key)
        {
            return HttpContextAccessor.HttpContext.Items[key];
        }

        public void StoreItem(string key, object value)
        {
            HttpContextAccessor.HttpContext.Items[key] = value;
        }

        public IFormCollection GetFormData()
        {
            return HttpContextAccessor.HttpContext.Request.ReadFormAsync().GetAwaiter().GetResult();
        }

        public bool TryGetHeaderValue(string key, out string header)
        {
            bool isPresent = HttpContextAccessor.HttpContext.Request.Headers.TryGetValue(key, out var headerValue);
            header = headerValue;
            return isPresent;
        }

        public string GetQueryValue(string val)
        {
            if (HttpContextAccessor.HttpContext.Request.Query.TryGetValue(val, out var value))
            {
                return value.ToString();
            }

            return null;
        }

        public string GetValue(IFormCollection collection, string val)
        {
            if (collection.TryGetValue(val, out var value))
            {
                return value.ToString();
            }

            return null;
        }
    }
}
