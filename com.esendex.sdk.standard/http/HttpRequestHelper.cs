using System;
using System.Net;
using System.Text;
using com.esendex.sdk.adapters;
using System.Net.Http.Headers;
using System.IO;

namespace com.esendex.sdk.http
{
    internal class HttpRequestHelper : IHttpRequestHelper
    {
        public void AddContent(IHttpWebRequestAdapter httpRequest, HttpRequest request)
        {
            if (request.HasContent)
            {
                httpRequest.ContentType = request.ContentType;
                httpRequest.ContentLength = request.ContentLength;

                var data = request.ContentEncoding.GetBytes(request.Content);

                var requestStream = httpRequest.GetRequestStream();

                using (var stream = new MemoryStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(requestStream);
                }
                requestStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                httpRequest.ContentLength = 0;
            }
        }

        public void AddCredentials(IHttpWebRequestAdapter httpRequest, EsendexCredentials credentials)
        {
            if (credentials.UseSessionAuthentication)
            {
                var value = string.Format("Basic {0}", Convert.ToBase64String(new UTF8Encoding().GetBytes(credentials.SessionId.Value.ToString())));

                httpRequest.Headers.Add("Authorization", value);
            }
            else
            {
                var credentialCache = new CredentialCache
                {
                    {
                        httpRequest.RequestUri,
                        "Basic",
                        new NetworkCredential(credentials.Username, credentials.Password)
                    }
                };

                httpRequest.Credentials = credentialCache;
            }
        }

        public void AddProxy(IHttpWebRequestAdapter httpRequest, IWebProxy proxy)
        {
            httpRequest.Proxy = proxy;
        }

        public IHttpWebRequestAdapter Create(HttpRequest httpRequest, Uri uri, string version)
        {
            var fullUri = string.Format("{0}{1}/{2}", uri, httpRequest.ResourceVersion, httpRequest.ResourcePath);
            uri = new Uri(fullUri);

            IHttpWebRequestAdapter httpWebRequest = new HttpWebRequestAdapter(uri);
            httpWebRequest.Method = httpRequest.HttpMethod.ToString();
            httpWebRequest.UserAgent = string.Format("Esendex .NET SDK {0}", version);

            return httpWebRequest;
        }
    }
}