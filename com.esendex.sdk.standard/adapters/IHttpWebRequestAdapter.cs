using System;
using System.IO;
using System.Net;

using System.Net.Http.Headers;

namespace com.esendex.sdk.adapters
{
    internal interface IHttpWebRequestAdapter
    {
        long ContentLength { get; set; }
        string ContentType { get; set; }
        ICredentials Credentials { get; set; }

        HttpRequestHeaders Headers { get; set; }

        string Method { get; set; }

        IWebProxy Proxy { get; set; }

        Uri RequestUri { get; }

        string UserAgent { get; set; }

        Stream GetRequestStream();

        IHttpWebResponseAdapter GetResponse();
    }
}