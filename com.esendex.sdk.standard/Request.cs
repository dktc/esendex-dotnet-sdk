using System;
using System.IO;
using System.Net;
using System.Reflection;
using com.esendex.sdk.surveys;
using System.Net.Http;

namespace com.esendex.sdk
{
    internal class Request
    {
        private readonly string _version = "com.esendex.sdk.standard";
        private readonly string method;
        private readonly Uri uri;
        private HttpClient client;
        private HttpContent content = null;
        private HttpClientHandler handler = new HttpClientHandler();
        private HttpResponseMessage response = null;

        private Request(string method, Uri url)
        {
            client = new HttpClient(handler);
            method = method.ToUpper();
            client.DefaultRequestHeaders.Add("User-Agent", string.Format("Esendex .NET SDK {0}", _version));
        }

        public static Request Create(string method, Uri url)
        {
            return new Request(method, url);
        }

        public HttpResponseMessage GetResponse()
        {
            HttpResponseMessage response = null;

            if (this.method == "POST")
            {
                response = this.client.PostAsync(this.uri, this.content).Result;
            }
            else if (this.method == "DELETE")
            {
                response = this.client.DeleteAsync(this.uri).Result;
            }
            else if (this.method == "GET")
            {
                response = this.client.GetAsync(this.uri).Result;
            }
            else if (this.method == "PUT")
            {
                response = this.client.PutAsync(this.uri, this.content).Result;
            }

            return response;
        }

        public Request If(bool p, Func<Request, Request> f)
        {
            return p ? f(this) : this;
        }

        public Request WithAcceptHeader(string acceptHeader)
        {
            client.DefaultRequestHeaders.Add("Accept", acceptHeader);
            return this;
        }

        public Request WithHeader(string key, string value)
        {
            client.DefaultRequestHeaders.Add(key, value);
            return this;
        }

        public Request WithProxy(IWebProxy webProxy)
        {
            handler.Proxy = webProxy;
            return this;
        }

        public Request WriteBody(string contentType, Action<StreamWriter> writer)
        {
            Stream requestStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(requestStream))
            {
                writer(streamWriter);
                requestStream.Flush();
            }
            content = new StreamContent(requestStream);
            content.Headers.ContentType.MediaType = contentType;
            return this;
        }
    }
}