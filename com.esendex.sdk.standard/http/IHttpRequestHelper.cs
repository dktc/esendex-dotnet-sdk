using System;
using System.Net;
using com.esendex.sdk.adapters;

namespace com.esendex.sdk.http
{
    internal interface IHttpRequestHelper
    {
        void AddContent(IHttpWebRequestAdapter httpRequest, HttpRequest request);

        void AddCredentials(IHttpWebRequestAdapter httpRequest, EsendexCredentials credentials);

        void AddProxy(IHttpWebRequestAdapter httpRequest, IWebProxy proxy);

        IHttpWebRequestAdapter Create(HttpRequest httpRequest, Uri uri, string version);
    }
}