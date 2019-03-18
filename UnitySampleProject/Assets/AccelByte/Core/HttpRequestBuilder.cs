// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using AccelByte.Api;
using UnityEngine;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    public class HttpRequestBuilder
    {
        private string verb = "";
        private string url = "";
        private readonly IDictionary<string, string> pathParams = new Dictionary<string, string>();
        private readonly IDictionary<string, string> queryParams = new Dictionary<string, string>();
        private readonly IDictionary<string, ICollection<string>> queryArrayParams = new Dictionary<string, ICollection<string>>();
        private readonly IDictionary<string, string> headers = new Dictionary<string, string>();
        private readonly IDictionary<string, string> formParams = new Dictionary<string, string>();
        private string body;
        private byte[] payloadBytes;
        private bool autoRedirect = true;
        public static HttpRequestBuilder CreateGet(string url)
        {
            var request = new HttpRequestBuilder();
            request.verb = WebRequestMethods.Http.Get;
            request.url = url;
            request.headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();
            return request;
        }
        public static HttpRequestBuilder CreatePost(string url)
        {
            var request = new HttpRequestBuilder();
            request.verb = WebRequestMethods.Http.Post;
            request.url = url;
            request.headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();
            return request;
        }
        public static HttpRequestBuilder CreatePut(string url)
        {
            var request = new HttpRequestBuilder();
            request.verb = WebRequestMethods.Http.Put;
            request.url = url;
            request.headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();
            return request;
        }
        public static HttpRequestBuilder CreatePatch(string url)
        {
            var request = new HttpRequestBuilder();
            request.verb = "PATCH";
            request.url = url;
            request.headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();
            return request;
        }
        public static HttpRequestBuilder CreateDelete(string url)
        {
            var request = new HttpRequestBuilder();
            request.verb = "DELETE";
            request.url = url;
            request.headers["X-Amzn-TraceId"] = AwsXRayTraceIdFactory.GetNewXRayTraceId();
            return request;
        }
        public HttpRequestBuilder WithPathParam(string key, string value)
        {
            this.pathParams[key] = value;
            return this;
        }
        public HttpRequestBuilder WithQueryParam(string key, string value)
        {
            this.queryParams[key] = value;
            return this;
        }
        public HttpRequestBuilder WithQueryParam(string key, ICollection<string> value)
        {
            this.queryArrayParams[key] = value;
            return this;
        }
        public HttpRequestBuilder WithQueries(Dictionary<string, string> queries)
        {
            foreach (var queryPair in queries)
            {
                this.queryParams.Add(queryPair);
            }
            return this;
        }
        public HttpRequestBuilder WithBasicAuth(string username, string password)
        {
            string svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            this.headers["Authorization"] = "Basic " + svcCredentials;
            return this;
        }
        public HttpRequestBuilder WithBearerAuth(string token)
        {
            this.headers["Authorization"] = "Bearer " + token;
            return this;
        }
        public HttpRequestBuilder WithContentType(MediaType mediaType)
        {
            this.headers["Content-Type"] = mediaType.ToString();
            return this;
        }
        public HttpRequestBuilder WithContentType(string rawMediaType)
        {
            this.headers["Content-Type"] = rawMediaType;
            return this;
        }
        public HttpRequestBuilder Accepts(MediaType mediaType)
        {
            this.headers["Accept"] = mediaType.ToString();
            return this;
        }
        public HttpRequestBuilder WithFormParam(string key, string value)
        {
            this.formParams[key] = value;
            return this;
        }
        public HttpRequestBuilder WithBody(string body)
        {
            this.body = body;
            return this;
        }
        public HttpRequestBuilder WithBody(FormDataContent formDataContent)
        {
            this.payloadBytes = formDataContent.Get();
            return this;
        }
        public HttpRequestBuilder DisableAutoRedirect()
        {
            this.autoRedirect = false;
            
            return this;
        }
        
        public HttpWebRequest ToRequest()
        {
            var urlBuilder = new StringBuilder(256);
            urlBuilder.Append(this.url);
            foreach (var pathParamPair in this.pathParams)
            {
                urlBuilder.Replace("{" + pathParamPair.Key + "}", UnityWebRequest.EscapeURL(pathParamPair.Value));
            }
            if (this.queryParams.Count > 0 || this.queryArrayParams.Count > 0)
            {
                urlBuilder.Append("?");
                foreach (var paramPair in this.queryParams)
                {
                    urlBuilder.Append(UnityWebRequest.EscapeURL(paramPair.Key));
                    urlBuilder.Append("=");
                    urlBuilder.Append(UnityWebRequest.EscapeURL(paramPair.Value));
                    urlBuilder.Append("&");
                }

                foreach(var paramPair in this.queryArrayParams)
                {
                    foreach(var param in paramPair.Value)
                    {
                        urlBuilder.Append(UnityWebRequest.EscapeURL(paramPair.Key));
                        urlBuilder.Append("=");
                        urlBuilder.Append(UnityWebRequest.EscapeURL(param));
                        urlBuilder.Append("&");
                    }
                }

                this.url = urlBuilder.ToString(0, urlBuilder.Length - 1);
            }
            else
            {
                this.url = urlBuilder.ToString();
            }
            var bodyBuilder = new StringBuilder(1024);
            if (this.formParams.Count > 0 && this.headers["Content-Type"] == MediaType.ApplicationForm.ToString())
            {
                foreach (var paramPair in this.formParams)
                {
                    bodyBuilder.Append(UnityWebRequest.EscapeURL(paramPair.Key));
                    bodyBuilder.Append("=");
                    bodyBuilder.Append(UnityWebRequest.EscapeURL(paramPair.Value));
                    bodyBuilder.Append("&");
                }
                this.body = bodyBuilder.ToString(0, bodyBuilder.Length - 1);
            }
            if (!String.IsNullOrEmpty(this.body))
            {
                this.payloadBytes = Encoding.UTF8.GetBytes(this.body);
            }
            
            Uri uri = new Uri(this.url);
            #if NET_2_0_SUBSET || NET_2_0 || !UNITY_2017_1_OR_NEWER
            UriHelper.ForceCanonicalPathAndQuery(uri);
            #endif
            
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = this.verb;
            foreach (var headerPair in this.headers)
            {
                switch (headerPair.Key)
                {
                    case "Content-Type":
                        request.ContentType = headerPair.Value;
                        break;
                    case "Accept":
                        request.Accept = headerPair.Value;
                        break;
                    default:
                        request.Headers.Add(headerPair.Key, headerPair.Value);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(this.body) || this.payloadBytes != null)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(this.payloadBytes, 0, this.payloadBytes.Length);
                }
            }
            if (!this.autoRedirect)
            {
                request.AllowAutoRedirect = false;
            }
            
            return request;
        }
        
        private HttpRequestBuilder()
        {
        }
    }
    
#if NET_2_0_SUBSET || NET_2_0 || !UNITY_2017_1_OR_NEWER
    public static class UriHelper {
        private static readonly Type uriType = typeof(Uri);
        private static readonly FieldInfo sourceField;
        private static readonly FieldInfo queryField;
        private static readonly FieldInfo pathField;
        private static readonly FieldInfo cachedToStringField;
        private static readonly FieldInfo cachedAbsoluteUriField;
        static UriHelper ()
        {
            UriHelper.sourceField = UriHelper.uriType.GetField ("source", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.queryField = UriHelper.uriType.GetField ("query", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.pathField = UriHelper.uriType.GetField ("path", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.cachedToStringField = UriHelper.uriType.GetField ("cachedToString", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.cachedAbsoluteUriField = UriHelper.uriType.GetField ("cachedAbsoluteUri", BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public static void ForceCanonicalPathAndQuery(Uri uri)
        {
            var source = (string) UriHelper.sourceField.GetValue (uri);
            UriHelper.cachedToStringField.SetValue (uri, source);
            UriHelper.cachedAbsoluteUriField.SetValue (uri, source);
            var fragPos = source.IndexOf ("#");
            var queryPos = source.IndexOf ("?");
            var start = source.IndexOf (uri.Host) + uri.Host.Length;
            var pathEnd = queryPos == -1 ? fragPos : queryPos;
            
            if (pathEnd == -1)
            {
                pathEnd = source.Length + 1;
            }
            
            var path = queryPos > -1 ? source.Substring (start, pathEnd - start) : source.Substring (start);
            UriHelper.pathField.SetValue (uri, path);
            
            if (queryPos > -1)
            {
                UriHelper.queryField.SetValue(uri,
                    fragPos > -1 ? source.Substring(queryPos, fragPos - queryPos) : source.Substring(queryPos));
            }
        }
    }
#endif
}
