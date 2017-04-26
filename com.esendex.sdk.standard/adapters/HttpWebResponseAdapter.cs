using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace com.esendex.sdk.adapters
{
    internal class HttpWebResponseAdapter : IHttpWebResponseAdapter
    {
        private readonly HttpResponseMessage httpWebResponse;
        private readonly Stream responseStream;

        public HttpWebResponseAdapter(HttpResponseMessage response)
        {
            httpWebResponse = response;
            responseStream = response.Content.ReadAsStreamAsync().Result;
        }

        public string ContentEncoding
        {
            get { return httpWebResponse.Content.Headers.ContentEncoding.FirstOrDefault(); }
        }

        public long ContentLength
        {
            get { return responseStream.Length; }
            set { throw new InvalidOperationException(); }
        }

        public string ContentType
        {
            get { return httpWebResponse.Content.Headers.ContentType.MediaType; }
            set { throw new InvalidOperationException(); }
        }

        public HttpStatusCode StatusCode
        {
            get { return httpWebResponse.StatusCode; }
        }

        public Stream GetResponseStream()
        {
            return responseStream; ;
        }
    }
}