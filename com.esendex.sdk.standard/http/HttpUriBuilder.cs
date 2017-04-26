using System;
using System.Collections.Specialized;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace com.esendex.sdk.http
{
    public class HttpUriBuilder
    {
        private readonly Dictionary<string, Microsoft.Extensions.Primitives.StringValues> _query;
        private readonly UriBuilder _uriBuilder;

        private HttpUriBuilder(string url)
        {
            _uriBuilder = new UriBuilder(url);
            _query = QueryHelpers.ParseQuery(string.Empty);
        }

        public static HttpUriBuilder Create(string url)
        {
            return new HttpUriBuilder(url);
        }

        public Uri Build()
        {
            _uriBuilder.Query = _query.ToString();
            return _uriBuilder.Uri;
        }

        public HttpUriBuilder WithParameter(string name, string value)
        {
            _query[name] = value;
            return this;
        }
    }
}