// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace AccelByte.Core
{
    public class AccelByteHttpClient : IHttpClient
    {
        enum RequestState
        {
            Invalid = 0,
            Running,
            Paused,
            Resumed,
            Stoped
        }

        public event Action<IHttpRequest> ServerErrorOccured;
        
        public event Action<IHttpRequest> NetworkErrorOccured;

        public event Action<string, Action<string>> BearerAuthRejected;

        public event Action<string> UnauthorizedOccured;

        private AccelByteHttpCache<IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>> httpCache;

        public AccelByteHttpClient(IHttpRequestSender requestSender = null)
        {
            if(requestSender != null)
            {
                this.sender = requestSender;
            }
            else
            {
                var defaultSenderScheduler = new WebRequestSchedulerAsync();
                var defaultSender = new UnityHttpRequestSender(defaultSenderScheduler);
                this.sender = defaultSender;
            }
            this.SetRetryParameters();
        }

        internal void SetCacheImplementation(IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>> cacheImplementation, int cacheMaxLifeTime)
        {
            httpCache = new AccelByteHttpCache<IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>>(cacheImplementation, cacheMaxLifeTime);
        }

        public void SetCredentials(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public void SetImplicitPathParams(IDictionary<string, string> pathParams)
        {
            foreach (var param in pathParams)
            {
                this.pathParams[param.Key] = param.Value;
            }
        }

        public void ClearImplicitPathParams()
        {
            this.pathParams.Clear();
        }

        public void ClearCookies()
        {
            if (this.baseUri != null)
            {
                this.sender.ClearCookies(this.baseUri);
            }
        }

        public void SetBaseUri(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public void SetSender(IHttpRequestSender newRequestSender)
        {
            if(newRequestSender != null)
            {
                this.sender = newRequestSender;
            }
        }

        public void OnBearerAuthRejected(Action<string> callback)
        {
            PauseBearerAuthRequest();
            if (!IsRequestingNewAccessToken())
            {
                if (BearerAuthRejected == null)
                {
                    callback?.Invoke(null);
                }
                else
                {
                    SetRequestingNewAccessToken(true);
                    BearerAuthRejected?.Invoke(accessToken, result =>
                    {
                        SetRequestingNewAccessToken(false);
                        if (result != null)
                        {
                            ResumeBearerAuthRequest();
                        }
                        callback?.Invoke(result);
                    });
                }
            }
        }

        public void SetImplicitBearerAuth(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public void SetRetryParameters(uint totalTimeoutMs = 60000, uint initialDelayMs = 1000, uint maxDelayMs = 30000, uint maxRetries = 3)
        {
            this.totalTimeoutMs = totalTimeoutMs;
            this.initialDelayMs = initialDelayMs;
            this.maxDelayMs = maxDelayMs;
            this.maxRetries = maxRetries;
        }

        public async Task<HttpSendResult> SendRequestAsync(IHttpRequest requestInput)
        {
            var rand = new Random();
            uint retryDelayMs = this.initialDelayMs;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IHttpRequest request;
            HttpCacheRetrieveResult getHttpCacheResult = null;
            if (httpCache != null)
            {
                getHttpCacheResult = httpCache.TryRetrieving(requestInput);
            }

            if (getHttpCacheResult != null && getHttpCacheResult.Request != null)
            {
                request = getHttpCacheResult.Request;
            }
            else
            {
                request = requestInput;
            }

            this.ApplyImplicitAuthorization(request);
            this.ApplyImplicitPathParams(request);

            if (!this.TryResolveUri(request))
            {
                return new HttpSendResult(null, new Error(ErrorCode.InvalidRequest, "Invalid uri format: ", request.Url));
            }

            IHttpResponse httpResponse;
            Error error;

            RequestState state;
            if (IsBearerAuthRequestPaused() && request.AuthType == HttpAuth.Bearer)
            {
                state = RequestState.Paused;
            }
            else
            {
                state = RequestState.Running;
            }

            int retryTimes = 0;
            do
            {
                if (state == RequestState.Paused)
                {
                    if (IsBearerAuthRequestPaused())
                    {
                        OnBearerAuthRejected(null);
                        while (IsBearerAuthRequestPaused() && IsRequestingNewAccessToken())
                        {
                            await AccelByteHttpHelper.HttpDelayOneFrame;
                        }

                        if (IsBearerAuthRequestPaused())
                        {
                            AccelByteDebug.LogWarning("Failed retrieving new access token, resuming");
                        }
                    }

                    state = RequestState.Resumed;
                    stopwatch.Restart();
                    if (IsBearerAuthRequestPaused())
                    {
                        request.Headers.Remove("Authorization");
                        ApplyImplicitAuthorization(request);
                    }
                }

                int timeoutMs = (int)(this.totalTimeoutMs - stopwatch.ElapsedMilliseconds);

                if (getHttpCacheResult != null && getHttpCacheResult.Response != null)
                {
                    httpResponse = getHttpCacheResult.Response;
                    error = getHttpCacheResult.ResponseError;
                }
                else
                {
                    uint delayMs = 0;
                    if (retryTimes > 0)
                    {
                        AccelByteDebug.LogWarning($"Send request failed, Retrying {retryTimes}/{maxRetries}");
                        request.Priority = AccelByteHttpHelper.HttpRequestDefaultPriority - 1;

                        int jitterMs = rand.Next((int)(-retryDelayMs / 4), (int)(retryDelayMs / 4));
                        delayMs = (uint)(retryDelayMs + jitterMs);

                        retryDelayMs = Math.Min(retryDelayMs * 2, this.maxDelayMs);
                    }

                    HttpSendResult? sendResult = null;
                    Action<HttpSendResult> onSendRequestComplete = (result) =>
                    {
                        sendResult = result;
                    };
                    sender.AddTask(request, onSendRequestComplete, timeoutMs, delayMs);
                    while (sendResult == null)
                    {
                        await AccelByteHttpHelper.HttpDelayOneFrame;
                    }
                    httpResponse = sendResult.Value.CallbackResponse;
                    error = sendResult.Value.CallbackError;
                }

                bool requireToRetry = CheckRequireRetry(request, httpResponse, error, NetworkErrorOccured, ServerErrorOccured);
                
                if (requireToRetry)
                {
                    retryTimes++;
                    if (retryTimes > maxRetries)
                    {
                        return new HttpSendResult(httpResponse, error);
                    }
                    continue;
                }

                if (httpResponse.Code == (long)HttpStatusCode.Unauthorized)
                {
                    if (state == RequestState.Resumed || request.AuthType != HttpAuth.Bearer)
                    {
                        UnauthorizedOccured?.Invoke(accessToken);
                        return new HttpSendResult(httpResponse, error);
                    }
                    else
                    {
                        state = RequestState.Paused;
                        OnBearerAuthRejected(null);
                        while (IsBearerAuthRequestPaused() && IsRequestingNewAccessToken())
                        {
                            await AccelByteHttpHelper.HttpDelayOneFrame;
                        }
                        if (IsBearerAuthRequestPaused())
                        {
                            AccelByteDebug.LogWarning("Failed retrieving new access token");
                            return new HttpSendResult(httpResponse, error);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(this.totalTimeoutMs) || IsBearerAuthRequestPaused());

            if (httpCache != null)
            {
                httpCache.TryStoring(request, httpResponse, error);
            }
            return new HttpSendResult(httpResponse, error);
        }

        public IEnumerator SendRequest(IHttpRequest requestInput, Action<IHttpResponse, Error> callback)
        {
            var rand = new Random();
            uint retryDelayMs = this.initialDelayMs;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IHttpRequest request = null;
            HttpCacheRetrieveResult getHttpCacheResult = null;
            if (httpCache != null)
            {
                getHttpCacheResult = httpCache.TryRetrieving(requestInput);
            }

            if(getHttpCacheResult != null && getHttpCacheResult.Request != null)
            {
                request = getHttpCacheResult.Request;
            } 
            else
            {
                request = requestInput;
            }

            this.ApplyImplicitAuthorization(request);
            this.ApplyImplicitPathParams(request);

            if (!this.TryResolveUri(request))
            {
                callback?.Invoke(null, new Error(ErrorCode.InvalidRequest, "Invalid uri format: ", request.Url));

                yield break;
            }
            
            IHttpResponse httpResponse = null;
            Error error = null;

            RequestState state = RequestState.Invalid;
            if (IsBearerAuthRequestPaused() && request.AuthType == HttpAuth.Bearer )
            {
                state = RequestState.Paused;
            }
            else
            {
                state = RequestState.Running;
            }

            int retryTimes = 0;
            do
            {
                if (state == RequestState.Paused)
                {
                    if (IsBearerAuthRequestPaused())
                    {
                        OnBearerAuthRejected(result => { });
                        yield return new WaitWhile(() => 
                        { 
                            return IsBearerAuthRequestPaused() && IsRequestingNewAccessToken(); 
                        });

                        if (IsBearerAuthRequestPaused())
                        {
                            AccelByteDebug.LogWarning("Failed retrieving new access token, resuming");
                        }
                    }

                    state = RequestState.Resumed;
                    stopwatch.Restart();
                    if(IsBearerAuthRequestPaused())
                    {
                        request.Headers.Remove("Authorization");
                        ApplyImplicitAuthorization(request);
                    }
                }

                int timeoutMs = (int)(this.totalTimeoutMs - stopwatch.ElapsedMilliseconds);

                if (getHttpCacheResult != null && getHttpCacheResult.Response != null)
                {
                    httpResponse = getHttpCacheResult.Response;
                    error = getHttpCacheResult.ResponseError;
                }
                else
                {
                    uint delayMs = 0;
                    if (retryTimes > 0)
                    {
                        AccelByteDebug.LogWarning($"Send request failed, Retry {retryTimes}/{maxRetries}");
                        request.Priority = AccelByteHttpHelper.HttpRequestDefaultPriority - 1;

                        int jitterMs = rand.Next((int)(-retryDelayMs / 4), (int)(retryDelayMs / 4));
                        delayMs = (uint)(retryDelayMs + jitterMs);

                        retryDelayMs = Math.Min(retryDelayMs * 2, this.maxDelayMs);
                    }

                    HttpSendResult? sendResult = null;
                    Action<HttpSendResult> onSendRequestComplete = (result) =>
                    {
                        sendResult = result;
                    };
                    sender.AddTask(request, onSendRequestComplete, timeoutMs, delayMs);
                    yield return new WaitUntil(() => sendResult != null);
                    httpResponse = sendResult.Value.CallbackResponse;
                    error = sendResult.Value.CallbackError;
                }

                bool requireToRetry = CheckRequireRetry(request, httpResponse, error, NetworkErrorOccured, ServerErrorOccured);
                if (requireToRetry)
                {
                    retryTimes++;
                    if (retryTimes > maxRetries)
                    {
                        callback?.Invoke(httpResponse, error);
                        yield break;
                    }
                    continue;
                }

                if (httpResponse.Code == (long)HttpStatusCode.Unauthorized)
                {
                    if (state == RequestState.Resumed || request.AuthType != HttpAuth.Bearer)
                    {
                        state = RequestState.Stoped;
                        callback?.Invoke(httpResponse, error);
                        UnauthorizedOccured?.Invoke(accessToken);
                        yield break;
                    }
                    else
                    {
                        state = RequestState.Paused;
                        OnBearerAuthRejected(result => { });
                        yield return new WaitWhile(() =>
                        {
                            return IsBearerAuthRequestPaused() && IsRequestingNewAccessToken();
                        });
                        if (IsBearerAuthRequestPaused())
                        {
                            state = RequestState.Stoped;
                            callback?.Invoke(httpResponse, error);
                            AccelByteDebug.LogWarning("Failed retrieving new access token");
                            yield break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(this.totalTimeoutMs) || IsBearerAuthRequestPaused());

            if (httpCache != null)
            {
                httpCache.TryStoring(request, httpResponse, error);
            }
            callback?.Invoke(httpResponse, error);
        }

        private bool TryResolveUri(IHttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Url)) return false;

            if (request.Url.Contains("{") || request.Url.Contains("}")) return false;

            if (!Uri.TryCreate(this.baseUri, request.Url, out Uri uri)) return false;

            if (uri.Scheme != "https" && uri.Scheme != "http") return false;
            
            request.Url = uri.AbsoluteUri;
                
            return true;

        }

        private void ApplyImplicitPathParams(IHttpRequest request)
        {
            if (request == null) return;
            
            if (this.pathParams == null || this.pathParams.Count == 0) return;
            
            string formattedUrl = request.Url;
            
            foreach (var param in this.pathParams)
            {
                formattedUrl = formattedUrl.Replace("{" + param.Key + "}", Uri.EscapeDataString(param.Value));
            }

            request.Url = formattedUrl;
        }

        private void ApplyImplicitAuthorization(IHttpRequest request)
        {
            if (request == null) return;

            const string authHeaderKey = "Authorization";

            if (request.Headers != null && request.Headers.ContainsKey(authHeaderKey))
            {
                return;
            }

            switch (request.AuthType)
            {
                case HttpAuth.Basic:
                    if (string.IsNullOrEmpty(this.clientId))
                    {
                        return;
                    }

                    string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.clientSecret}"));
                    request.Headers[authHeaderKey] = "Basic " + base64;
                    break;
                case HttpAuth.Bearer:
                    if (string.IsNullOrEmpty(this.accessToken))
                    {
                        return;
                    }

                    request.Headers[authHeaderKey] = "Bearer " + this.accessToken;
                    break;
            }
        }

        private bool CheckRequireRetry(IHttpRequest httpRequest, IHttpResponse httpResponse, Error error, Action<IHttpRequest> onNetworkError, Action<IHttpRequest> onServerError)
        {
            bool requireToRetry = false;
            if (error != null)
            {
                onNetworkError?.Invoke(httpRequest);
                requireToRetry = true;
            }
            else if (HttpErrorParser.IsHasServerError(httpResponse))
            {
                onServerError?.Invoke(httpRequest);
                requireToRetry = true;
            }
            return requireToRetry;
        }

        private bool IsBearerAuthRequestPaused() 
        { 
            return isBanDetected;
        }
        private void PauseBearerAuthRequest()
        {
            isBanDetected = true;
        }
        private void ResumeBearerAuthRequest()
        {
            isBanDetected = false;
        }
        private bool IsRequestingNewAccessToken()
        {
            return isRequestingNewAccessToken;
        }
        private void SetRequestingNewAccessToken(bool isRequesting) 
        { 
            isRequestingNewAccessToken = isRequesting; 
        }

        private IHttpRequestSender sender;
        private uint maxRetries;
        private uint totalTimeoutMs;
        private uint initialDelayMs;
        private uint maxDelayMs;
        private string clientId;
        private string clientSecret;
        private string accessToken;
        private readonly IDictionary<string,string> pathParams = new Dictionary<string, string>();
        private Uri baseUri;

        private bool isBanDetected;
        private bool isRequestingNewAccessToken;
    }
}