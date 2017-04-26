using System.IO;
using System.Net;
using com.esendex.sdk.adapters;

namespace com.esendex.sdk.http
{
    internal class HttpResponseHelper : IHttpResponseHelper
    {
        public HttpResponse Create(IHttpWebResponseAdapter response)
        {
            if (response == null) return null;

            var content = string.Empty;

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                content = stream.ReadToEnd();
            }

            return new HttpResponse
            {
                Content = content,
                ContentType = response.ContentType,
                StatusCode = response.StatusCode,
                ContentEncoding = response.ContentEncoding
            };
        }
    }
}