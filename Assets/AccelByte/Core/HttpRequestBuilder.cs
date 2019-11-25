// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AccelByte.Api;

namespace AccelByte.Core
{
    public class HttpRequestBuilder
    {
        private StringBuilder formBuilder = new StringBuilder(1024);
        private HttpRequestPrototype result;

        private static HttpRequestBuilder CreatePrototype(string method, string url)
        {
            var builder = new HttpRequestBuilder {result = new HttpRequestPrototype(url) {Method = method}};

            builder.result.Headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();

            return builder;
        }

        public static HttpRequestBuilder CreateGet(string url)
        {
            return HttpRequestBuilder.CreatePrototype("GET", url);
        }

        public static HttpRequestBuilder CreatePost(string url)
        {
            return HttpRequestBuilder.CreatePrototype("POST", url);
        }

        public static HttpRequestBuilder CreatePut(string url)
        {
            return HttpRequestBuilder.CreatePrototype("PUT", url);
        }

        public static HttpRequestBuilder CreatePatch(string url)
        {
            return HttpRequestBuilder.CreatePrototype("PATCH", url);
        }

        public static HttpRequestBuilder CreateDelete(string url)
        {
            return HttpRequestBuilder.CreatePrototype("DELETE", url);
        }

        public HttpRequestBuilder WithPathParam(string key, string value)
        {
            this.result.UrlBuilder.Replace("{" + key + "}", Uri.EscapeDataString(value));
            this.result.BaseUrlLength = this.result.UrlBuilder.Length;

            return this;
        }

        public HttpRequestBuilder WithQueryParam(string key, string value)
        {
            string formatString = this.result.BaseUrlLength == this.result.UrlBuilder.Length ? "?{0}={1}" : "&{0}={1}";
            this.result.UrlBuilder.AppendFormat(formatString, Uri.EscapeDataString(key), Uri.EscapeDataString(value));

            return this;
        }

        public HttpRequestBuilder WithQueryParam(string key, ICollection<string> values)
        {
            foreach (string value in values)
            {
                WithQueryParam(key, value);
            }

            return this;
        }

        public HttpRequestBuilder WithQueries(Dictionary<string, string> queries)
        {
            foreach (var queryPair in queries)
            {
                WithQueryParam(queryPair.Key, queryPair.Value);
            }

            return this;
        }

        public HttpRequestBuilder WithBasicAuth(string username, string password)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            this.result.Headers["Authorization"] = "Basic " + credentials;

            return this;
        }

        public HttpRequestBuilder WithBearerAuth(string token)
        {
            this.result.Headers["Authorization"] = "Bearer " + token;

            return this;
        }

        public HttpRequestBuilder WithContentType(MediaType mediaType)
        {
            this.result.Headers["Content-Type"] = mediaType.ToString();

            return this;
        }

        public HttpRequestBuilder WithContentType(string rawMediaType)
        {
            this.result.Headers["Content-Type"] = rawMediaType;

            return this;
        }

        public HttpRequestBuilder Accepts(MediaType mediaType)
        {
            this.result.Headers["Accept"] = mediaType.ToString();

            return this;
        }

        public HttpRequestBuilder WithFormParam(string key, string value)
        {
            if (this.formBuilder.Length > 0)
            {
                this.formBuilder.Append("&");
            }

            this.formBuilder.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));

            return this;
        }

        public HttpRequestBuilder WithBody(byte[] body)
        {
            this.result.BodyBytes = body;

            return this;
        }

        public HttpRequestBuilder WithBody(string body)
        {
            this.result.BodyBytes = Encoding.UTF8.GetBytes(body);

            return this;
        }

        public HttpRequestBuilder WithBody(FormDataContent formDataContent)
        {
            this.result.BodyBytes = formDataContent.Get();

            return this;
        }

        public IHttpRequest GetResult()
        {
            if (this.formBuilder.Length > 0)
            {
                this.result.BodyBytes = Encoding.UTF8.GetBytes(this.formBuilder.ToString());
            }

            return this.result;
        }

        private HttpRequestBuilder() { }

        private class HttpRequestPrototype : IHttpRequest
        {
            public string Method { get; set; }
            public string Url { get { return this.UrlBuilder.ToString(); } }
            public Dictionary<string, string> Headers { get; private set; }
            public Stream BodyStream { get; set; }
            public byte[] BodyBytes { get; set; }

            public StringBuilder UrlBuilder { get; private set; }
            public int BaseUrlLength { get; set; }

            public HttpRequestPrototype(string url)
            {
                this.UrlBuilder = new StringBuilder(url, 256);
                this.BaseUrlLength = url.Length;
                this.Headers = new Dictionary<string, string>();
            }
        }
    }
}