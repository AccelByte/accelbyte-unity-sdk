// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core {
    public enum HttpAuth
    {
        None,
        Basic,
        Bearer
    }

    public interface IHttpRequest
    {
        string Id { get; set; }
        string Method { get; }
        string Url { get; set; }
        string UrlFormat { get; set; }
        HttpAuth AuthType { get; }
        IDictionary<string, string> Headers { get; }
        byte[] BodyBytes { get; }
        int Priority { get; set; }
        DateTime Timestamp { get; set; }
    }

    public interface IHttpResponse 
    {
        string Url { get; }
        string Method { get; }
        long Code { get; }
        IDictionary<string, string> Headers { get; }
        byte[] BodyBytes { get; }
        DateTime Timestamp { get; set; }
    }

    internal class HttpHeaderHelper
    {
        public static string GetHeaderValue(IDictionary<string, string> header, string key)
        {
            string retval = null;
            if (header != null)
            {
                if (header.ContainsKey(key))
                {
                    retval = header[key];
                }

                if (retval == null)
                {
                    string loweredKey = key.ToLower();
                    if (header.ContainsKey(loweredKey))
                    {
                        retval = header[loweredKey];
                    }
                }
            }
            return retval;
        }
    }

    public struct HttpSendResult
    {
        public readonly IHttpResponse CallbackResponse;
        public readonly Error CallbackError;

        public HttpSendResult(IHttpResponse callbackResponse, Error callbackError)
        {
            CallbackResponse = callbackResponse;
            CallbackError = callbackError;
        }
    }

    public interface IHttpRequestSender
    {
        void SetLogger(IDebugger logger);
        void AddTask(IHttpRequest request, Action<HttpSendResult> callback, int timeoutMs, uint delayTimeMs);
        void AddTask(IHttpRequest request, Action<HttpSendResult> callback, int timeoutMs, uint delayTimeMs, AdditionalHttpParameters additionalHttpParameters);
        void ClearTasks();
        void ClearCookies(Uri baseUri);
    }

    public struct HttpCredential
    {
        public string ClientId;
        public string ClientSecret;

        public HttpCredential(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }

    public interface IHttpClient
    {
        event Action<IHttpRequest> ServerErrorOccured;
        event Action<IHttpRequest> NetworkErrorOccured;
        IEnumerator SendRequest(IHttpRequest request, Action<IHttpResponse, Error> callback);
        IEnumerator SendRequest(AdditionalHttpParameters additionalParams, IHttpRequest request, Action<IHttpResponse, Error> callback);
        System.Threading.Tasks.Task<HttpSendResult> SendRequestAsync(IHttpRequest request);
        System.Threading.Tasks.Task<HttpSendResult> SendRequestAsync(AdditionalHttpParameters additionalParams, IHttpRequest request);
        void SetCredentials(string clientId, string clientSecret);
        HttpCredential GetCredentials();
        void SetImplicitBearerAuth(string accessToken);
        void AddAdditionalHeaderInfo(string headerKey, string headerValue);
        void SetImplicitPathParams(IDictionary<string, string> pathParams);
        void ClearImplicitPathParams();
        void SetBaseUri(Uri baseUri);
        void ClearCookies();
    }

    public static class HttpClientExtension
    {
        public static IEnumerator SendRequest(this IHttpClient client, IHttpRequest request, Action<IHttpResponse> callback)
        {
            return client.SendRequest(request, (response, err) => callback?.Invoke(response));
        }

        public static IEnumerator SendRequest(this IHttpClient client, AdditionalHttpParameters additionalParams, IHttpRequest request, Action<IHttpResponse> callback)
        {
            return client.SendRequest(additionalParams, request, (response, err) => callback?.Invoke(response));
        }

        public static async System.Threading.Tasks.Task<IHttpResponse> SendRequestAsync(this IHttpClient client, IHttpRequest request)
        {
            var sendTask = client.SendRequestAsync(request);
            HttpSendResult result = await sendTask;
            return result.CallbackResponse;
        }

        public static IEnumerator Get<T,R>(this IHttpClient client, string url, T queries, ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreateGet(url).WithBearerAuth().WithQueries(queries).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator Get<T>(this IHttpClient client, string url, ResultCallback<T> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreateGet(url).WithBearerAuth().GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator PostForm<T>(this IHttpClient client, string url, T body, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(url).WithBearerAuth().WithFormBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator Post<T>(this IHttpClient client, string url, T body, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(url).WithBearerAuth().WithJsonBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator Post<R>(this IHttpClient client,string url,ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                                                    .CreatePost(url)
                                                    .WithBearerAuth()
                                                    .WithContentType(MediaType.ApplicationJson)
                                                    .GetResult();

            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator Post<T,R>(this IHttpClient client, string url, T body, ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(url).WithBearerAuth().WithJsonBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator PostForm<T,R>(this IHttpClient client, string url, T body, ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(url).WithBearerAuth().WithFormBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator Put<T, R>(this IHttpClient client, string url, T body, ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePut(url).WithBearerAuth().WithJsonBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator Patch<T, R>(this IHttpClient client, string url, T body, ResultCallback<R> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePatch(url).WithBearerAuth().WithJsonBody(body).GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator Delete(this IHttpClient client, string url, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreateDelete(url).WithBearerAuth().GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator Delete<T>(this IHttpClient client, string url, T body, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreateDelete(url).WithBearerAuth().GetResult();
            
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        public static IEnumerator Send<T>(this IHttpClient client, IHttpRequest request, ResultCallback<T> callback)
        {
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }

        public static IEnumerator Send(this IHttpClient client, IHttpRequest request, ResultCallback callback)
        {
            return client?.SendRequest(request, HttpClientExtension.CreateResponseHandler(callback));
        }
        
        private static Action<IHttpResponse, Error> CreateResponseHandler<R>(ResultCallback<R> callback)
        {
            return (rsp, err) =>
            {
                if (err != null)
                {
                    callback.TryError(err);
                }
                else
                {
                    var result = rsp.TryParseJson<R>();
                    callback.Try(result);
                }
            };
        }

        private static Action<IHttpResponse, Error> CreateResponseHandler(ResultCallback callback)
        {
            return (rsp, err) =>
            {
                if (err != null)
                {
                    callback.TryError(err);
                }
                else
                {
                    callback.Try(rsp.TryParse());
                }
            };
        }
    }
}