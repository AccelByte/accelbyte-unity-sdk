// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
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
        internal const string FlightIdKey = "x-flight-id";
        internal const string NamespaceHeaderKey = "Namespace";
        internal const string GameClientVersionHeaderKey = "Game-Client-Version";
        internal const string SdkVersionHeaderKey = "AccelByte-SDK-Version";
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

        public event Func<string, Action<string>, bool> BearerAuthRejected;

        public event Action<string> UnauthorizedOccured;

        private AccelByteHttpCache<IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>> httpCache;

        private IDebugger logger;

        public AccelByteHttpClient(IHttpRequestSender requestSender = null, IDebugger logger = null)
        {
            SetLogger(logger);
            if(requestSender != null)
            {
                this.Sender = requestSender;
            }
            else
            {
                var defaultSenderScheduler = new WebRequestSchedulerAsync();
                UnityHttpRequestSender defaultSender = new UnityHttpRequestSender(defaultSenderScheduler);
                defaultSenderScheduler.SetLogger(logger);
                
                CoreHeartBeat coreHeartBeat = new CoreHeartBeat();
                defaultSender.SetLogger(logger);
                defaultSender.SetHeartBeat(coreHeartBeat);
                this.Sender = defaultSender;
            }
            this.SetRetryParameters();
        }

        internal void SetCacheImplementation(IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>> cacheImplementation, int cacheMaxLifeTime)
        {
            httpCache = new AccelByteHttpCache<IAccelByteCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>>(cacheImplementation, cacheMaxLifeTime);
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }

        public void SetCredentials(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public void SetFlightId(string newFlightId)
        {
            AddAdditionalHeaderInfo(FlightIdKey, newFlightId);
        }

        public string GetFlightId()
        {
            string retval = null;
            if (AdditionalHeaderInfo.ContainsKey(FlightIdKey))
            {
                retval = AdditionalHeaderInfo[FlightIdKey];
            }
            return retval;
        }

        public HttpCredential GetCredentials()
        {
            HttpCredential retval = new HttpCredential(this.clientId, this.clientSecret);
            return retval;
        }

        public void AddAdditionalHeaderInfo(string headerKey, string headerValue)
        {
            AdditionalHeaderInfo[headerKey] = headerValue;
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
            this.Sender.ClearCookies(this.baseUri);
        }

        public void SetBaseUri(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public void SetSender(IHttpRequestSender newRequestSender)
        {
            if(newRequestSender != null)
            {
                this.Sender = newRequestSender;
            }
        }

        public void OnBearerAuthRejected(Action<string> callback)
        {
            PauseBearerAuthRequest();
            if (IsRequestingNewAccessToken())
            {
                return;
            }
            
            if (BearerAuthRejected == null)
            {
                callback?.Invoke(null);
                return;
            }
            
            bool isRequestAccessTokenStart = BearerAuthRejected.Invoke(accessToken, result =>
            {
                SetRequestingNewAccessToken(false);
                if (result != null)
                {
                    ResumeBearerAuthRequest();
                }
                callback?.Invoke(result);
            });
            
            if (isRequestAccessTokenStart)
            {
                SetRequestingNewAccessToken(true);
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
            
            this.ApplyImplicitAuthorization(request, out Error applyAuthErr);
            if (applyAuthErr.Code != ErrorCode.None)
            {
                return new HttpSendResult(null, applyAuthErr);
            }
            this.ApplyImplicitPathParams(request);
            
            this.ApplyAdditionalData(request, AdditionalHeaderInfo, out Error applyAdditionalDataErr);
            if (applyAdditionalDataErr.Code != ErrorCode.None)
            {
                return new HttpSendResult(null, applyAdditionalDataErr);
            }

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
                            logger?.LogWarning("Failed retrieving new access token, resuming");
                        }
                    }

                    state = RequestState.Resumed;
                    stopwatch.Restart();
                    if (IsBearerAuthRequestPaused())
                    {
                        request.Headers.Remove("Authorization");
                        ApplyImplicitAuthorization(request, out Error applyErr);
                        if (applyErr.Code != ErrorCode.None)
                        {
                            return new HttpSendResult(null, applyErr);
                        }
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
                        logger?.LogWarning($"Send request failed, Retrying {retryTimes}/{maxRetries}");
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
                    Sender.AddTask(request, onSendRequestComplete, timeoutMs, delayMs);
                    while (sendResult == null)
                    {
                        await AccelByteHttpHelper.HttpDelayOneFrame;
                    }
                    httpResponse = sendResult.Value.CallbackResponse;
                    error = sendResult.Value.CallbackError;
                }

                if (httpResponse == null)
                {
                    return new HttpSendResult(httpResponse, error);
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
                            logger?.LogWarning("Failed retrieving new access token");
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

            this.ApplyImplicitAuthorization(request, out Error applyAuthErr);
            if (applyAuthErr.Code != ErrorCode.None)
            {
                callback?.Invoke(null, applyAuthErr);
                yield break;
            }
            this.ApplyImplicitPathParams(request);

            this.ApplyAdditionalData(request, AdditionalHeaderInfo, out Error applyAdditionalDataErr);
            if (applyAdditionalDataErr.Code != ErrorCode.None)
            {
                callback?.Invoke(null, applyAdditionalDataErr);
                yield break;
            }

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
                            logger?.LogWarning("Failed retrieving new access token, resuming");
                        }
                    }

                    state = RequestState.Resumed;
                    stopwatch.Restart();
                    if(IsBearerAuthRequestPaused())
                    {
                        request.Headers.Remove("Authorization");
                        this.ApplyImplicitAuthorization(request, out Error applyError);
                        if (applyError.Code != ErrorCode.None)
                        {
                            callback?.Invoke(null, applyAuthErr);
                            yield break;
                        }
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
                        logger?.LogWarning($"Send request failed, Retry {retryTimes}/{maxRetries}");
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
                    Sender.AddTask(request, onSendRequestComplete, timeoutMs, delayMs);
                    yield return new WaitUntil(() => sendResult != null);
                    httpResponse = sendResult.Value.CallbackResponse;
                    error = sendResult.Value.CallbackError;
                }

                if (httpResponse == null)
                {
                    callback?.Invoke(httpResponse, error);
                    yield break;
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
                            logger?.LogWarning("Failed retrieving new access token");
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

        private void ApplyImplicitAuthorization(IHttpRequest request, out Error err)
        {
            Error errorResult = new Error(ErrorCode.None);
            err = errorResult;

            if (request == null)
            {
                errorResult = new Error(ErrorCode.InvalidRequest, "request object is null");
                err = errorResult;
                return;
            }

            const string authHeaderKey = "Authorization";

            if (request.Headers != null && request.Headers.ContainsKey(authHeaderKey))
            {
                errorResult = new Error(ErrorCode.None);
                err = errorResult;
                return;
            }

            switch (request.AuthType)
            {
                case HttpAuth.Basic:
                    if (string.IsNullOrEmpty(this.clientId))
                    {
                        errorResult = new Error(ErrorCode.InvalidRequest, "failed to apply auth code from url :", request.Url);
                        err = errorResult;
                        break;
                    }

                    string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.clientSecret}"));
                    request.Headers[authHeaderKey] = "Basic " + base64;
                    break;
                case HttpAuth.Bearer:
                    if (string.IsNullOrEmpty(this.accessToken))
                    {
                        errorResult = new Error(ErrorCode.IsNotLoggedIn, "failed to apply auth code from url :", request.Url);
                        err = errorResult;
                        break;
                    }

                    request.Headers[authHeaderKey] = "Bearer " + this.accessToken;
                    break;
            }
        }

        private void ApplyAdditionalData(IHttpRequest request, IDictionary<string,string> additionalHeaders, out Error err)
        {
            err = new Error(ErrorCode.None);

            if (request == null)
            {
                err = new Error(ErrorCode.InvalidRequest, "request object is null");
                return;
            }

            if(additionalHeaders != null && additionalHeaders.Count > 0)
            {
                foreach(KeyValuePair<string,string> headerKeyValue in additionalHeaders)
                {
                    if(!request.Headers.ContainsKey(headerKeyValue.Key) && !string.IsNullOrEmpty(headerKeyValue.Value))
                    {
                        request.Headers.Add(headerKeyValue);
                    }
                }
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

        internal IHttpRequestSender Sender
        {
            get;
            private set;
        }
        private uint maxRetries;
        private uint totalTimeoutMs;
        private uint initialDelayMs;
        private uint maxDelayMs;
        private string clientId;
        private string clientSecret;
        private string accessToken;
        private readonly IDictionary<string,string> pathParams = new Dictionary<string, string>();
        internal readonly IDictionary<string, string> AdditionalHeaderInfo = new Dictionary<string, string>();
        private Uri baseUri;

        private bool isBanDetected;
        private bool isRequestingNewAccessToken;
    }
}