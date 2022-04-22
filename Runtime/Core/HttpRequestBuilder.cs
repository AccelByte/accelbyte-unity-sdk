// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccelByte.Api;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public class HttpRequestBuilder
    {
        private readonly StringBuilder formBuilder = new StringBuilder(1024);
        private readonly StringBuilder queryBuilder = new StringBuilder(256);
        private readonly StringBuilder urlBuilder = new StringBuilder(256);
        private HttpRequestPrototype result;

        private static HttpRequestBuilder CreatePrototype(string method, string url)
        {
            var builder = new HttpRequestBuilder
            {
                result = new HttpRequestPrototype
                {
                    Method = method,
                    Headers = { ["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId() }
                }
            };

            builder.urlBuilder.Append(url);

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
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new Exception($"Path parameter with key={key} is null or empty.");
            }

            this.urlBuilder.Replace("{" + key + "}", Uri.EscapeDataString(value));

            return this;
        }

        public HttpRequestBuilder WithPathParams(IDictionary<string, string> pathParams)
        {
            foreach (var param in pathParams)
            {
                WithPathParam(param.Key, param.Value);
            }

            return this;
        }

        public HttpRequestBuilder WithQueryParam(string key, string value)
        {
            Assert.IsNotNull(key, "query key is null");
            Assert.IsNotNull(value, $"query value is null for key {key}");
            
            if (string.IsNullOrEmpty(value)) return this;
            
            if (this.queryBuilder.Length > 0)
            {
                this.queryBuilder.Append("&");
            }
                
            this.queryBuilder.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");

            return this;
        }

        public HttpRequestBuilder WithQueryParam(string key, ICollection<string> values)
        {
            foreach (string value in values)
            {
                WithQueryParam(Uri.EscapeDataString(key), Uri.EscapeDataString(value));
            }

            return this;
        }

        public HttpRequestBuilder WithQueries(Dictionary<string, string> queryMap)
        {
            foreach (var queryPair in queryMap)
            {
                WithQueryParam(queryPair.Key, queryPair.Value);
            }

            return this;
        }

        public HttpRequestBuilder WithQueries<T>(T queryObject)
        {
            if (this.queryBuilder.Length > 0)
            {
                this.queryBuilder.Append("&");
            }
            
            this.queryBuilder.Append(queryObject.ToForm());

            return this;
        }

        public HttpRequestBuilder WithBasicAuth()
        {
            this.result.AuthType = HttpAuth.Basic;

            return this;
        }

        public HttpRequestBuilder WithBasicAuth(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("username and password for Basic Authorization shouldn't be empty or null");
            }

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            this.result.Headers["Authorization"] = "Basic " + credentials;
            this.result.AuthType = HttpAuth.Basic;

            return this;
        }

        public HttpRequestBuilder WithBearerAuth()
        {
            this.result.AuthType = HttpAuth.Bearer;

            return this;
        }

        public HttpRequestBuilder WithBearerAuth(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("token for Bearer Authorization shouldn't be empty or null");
            }
            
            this.result.Headers["Authorization"] = "Bearer " + token;
            this.result.AuthType = HttpAuth.Bearer;

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
            Assert.IsNotNull(key, "form key is null");
            Assert.IsNotNull(value, $"form value is null for key {key}");

            if (string.IsNullOrEmpty(value)) return this;
            
            if (this.formBuilder.Length > 0)
            {
                this.formBuilder.Append("&");
            }
                
            this.formBuilder.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");

            return this;
        }

        public HttpRequestBuilder WithBody(string body)
        {
            if (!this.result.Headers.ContainsKey("Content-Type"))
            {
                this.result.Headers.Add("Content-Type", MediaType.TextPlain.ToString());
            }
            
            this.result.BodyBytes = Encoding.UTF8.GetBytes(body);

            return this;
        }
        
        public HttpRequestBuilder WithBody(byte[] body)
        {
            if (!this.result.Headers.ContainsKey("Content-Type"))
            {
                this.result.Headers.Add("Content-Type", MediaType.ApplicationOctetStream.ToString());
            }
            
            this.result.BodyBytes = body;

            return this;
        }

        public HttpRequestBuilder WithBody(FormDataContent body)
        {
            if (!this.result.Headers.ContainsKey("Content-Type"))
            {
                this.result.Headers.Add("Content-Type", body.GetMediaType());
            }
            
            this.result.BodyBytes = body.Get();

            return this;
        }

        public HttpRequestBuilder WithFormBody<T>(T body)
        {
            if (!this.result.Headers.ContainsKey("Content-Type"))
            {
                this.result.Headers.Add("Content-Type", MediaType.ApplicationForm.ToString());
            }

            this.result.BodyBytes = Encoding.UTF8.GetBytes(body.ToForm());

            return this;
        }

        public HttpRequestBuilder WithJsonBody<T>(T body)
        {
            if (!this.result.Headers.ContainsKey("Content-Type"))
            {
                this.result.Headers.Add("Content-Type", MediaType.ApplicationJson.ToString());
            }

            this.result.BodyBytes = body.ToUtf8Json();

            return this;
        }

        public IHttpRequest GetResult()
        {
            if (this.queryBuilder.Length > 0)
            {
                this.urlBuilder.Append("?");
                this.urlBuilder.Append(this.queryBuilder);
            }
            
            if (this.formBuilder.Length > 0)
            {
                this.result.Headers["Content-Type"] = MediaType.ApplicationForm.ToString();
                this.result.BodyBytes = Encoding.UTF8.GetBytes(this.formBuilder.ToString());
            }

            this.result.Url = this.urlBuilder.ToString();

            return this.result;
        }

        private class HttpRequestPrototype : IHttpRequest
        {
            public string Method { get; set; }
            public string Url { get; set; }
            
            public HttpAuth AuthType { get; set; }
            public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();
            public byte[] BodyBytes { get; set; }
        }
    }
}
