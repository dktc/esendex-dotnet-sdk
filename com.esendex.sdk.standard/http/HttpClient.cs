using System;
using System.Net;
using System.Reflection;

namespace com.esendex.sdk.http
{
    internal class HttpClient : IHttpClient
    {
        public HttpClient(EsendexCredentials credentials, Uri uri, IHttpRequestHelper httpRequestHelper, IHttpResponseHelper httpResponseHelper)
        {
            HttpRequestHelper = httpRequestHelper;
            HttpResponseHelper = httpResponseHelper;
            Credentials = credentials;
            Uri = uri;
        }

        public EsendexCredentials Credentials { get; private set; }
        public IHttpRequestHelper HttpRequestHelper { get; private set; }
        public IHttpResponseHelper HttpResponseHelper { get; private set; }
        public Uri Uri { get; private set; }

        public HttpResponse Submit(HttpRequest request)
        {
            var webRequest = HttpRequestHelper.Create(request, Uri, "com.esendex.sdk.standard");
            HttpRequestHelper.AddCredentials(webRequest, Credentials);
            HttpRequestHelper.AddProxy(webRequest, Credentials.WebProxy);
            HttpRequestHelper.AddContent(webRequest, request);

            var webResponse = webRequest.GetResponse();

            return HttpResponseHelper.Create(webResponse);
        }
    }
}