﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace com.esendex.sdk.adapters
{
    internal class HttpWebRequestAdapter : IHttpWebRequestAdapter
    {
        private readonly HttpClient client;
        private readonly HttpClientHandler handler;
        private readonly Uri uri;
        private HttpContent content = null;
        private Stream contentStream;
        private string method;

        public HttpWebRequestAdapter(Uri url)
        {
            this.handler = new HttpClientHandler();
            this.client = new HttpClient(handler);
            this.uri = url;
            this.method = "POST";
        }

        public long ContentLength
        {
            get; set;
        }

        public string ContentType
        {
            get; set;
        }

        public ICredentials Credentials
        {
            get { return this.handler.Credentials; }
            set { this.handler.Credentials = value; }
        }

        public HttpRequestHeaders Headers
        {
            get { return client.DefaultRequestHeaders; }
            set { throw new NotSupportedException(); }
        }

        public string Method
        {
            get { return this.method; }
            set { this.method = value.ToUpper(); }
        }

        public IWebProxy Proxy
        {
            get { return handler.Proxy; }
            set { handler.Proxy = value; }
        }

        public Uri RequestUri
        {
            get { return this.uri; }
        }

        public string UserAgent
        {
            get => throw new NotImplementedException();
            set
            {
                this.client.DefaultRequestHeaders.Add("User-Agent", string.Format("Esendex .NET SDK/{0}", value));
            }
        }

        public Stream GetRequestStream()
        {
            this.contentStream = new MemoryStream();
            this.content = new StreamContent(this.contentStream);
            return this.contentStream;
        }

        public IHttpWebResponseAdapter GetResponse()
        {
            return this.GetResponseAsync().Result;
        }

        private async Task<IHttpWebResponseAdapter> GetResponseAsync()
        {
            HttpResponseMessage response = null;

            if (this.method == "POST")
            {
                response = await this.client.PostAsync(this.uri, this.content);
            }
            else if (this.method == "DELETE")
            {
                response = await this.client.DeleteAsync(this.uri);
            }
            else if (this.method == "GET")
            {
                response = await this.client.GetAsync(this.uri);
            }
            else if (this.method == "PUT")
            {
                response = await this.client.PutAsync(this.uri, this.content);
            }

            return new HttpWebResponseAdapter(response);
        }
    }
}