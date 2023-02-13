// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
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
            this.sender = requestSender ?? new UnityHttpRequestSender();
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
                    BearerAuthRejected.Invoke(accessToken, result =>
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

        public void SetRetryParameters(uint totalTimeoutMs = 60000, uint initialDelayMs = 1000, uint maxDelayMs = 30000)
        {
            this.totalTimeoutMs = totalTimeoutMs;
            this.initialDelayMs = initialDelayMs;
            this.maxDelayMs = maxDelayMs;
        }

        public IEnumerator SendRequest(IHttpRequest requestInput, Action<IHttpResponse, Error> callback)
        {
            var rand = new Random();
            uint retryDelayMs = this.initialDelayMs;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IHttpRequest request = null;
            HttpCacheRetrieveResult? getHttpCacheResult = null;
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
                    yield return this.sender.Send(request, (rsp, err) => (httpResponse, error) = (rsp, err), timeoutMs);
                }

                if (error != null)
                {
                    state = RequestState.Stoped;
                    this.NetworkErrorOccured?.Invoke(request);
                    callback?.Invoke(httpResponse, error);

                    yield break;
                }

                switch ((HttpStatusCode)httpResponse.Code)
                {
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                        this.ServerErrorOccured?.Invoke(request);

                        int jitterMs = rand.Next((int)(-retryDelayMs / 4), (int)(retryDelayMs / 4));

                        yield return new WaitForSeconds((retryDelayMs + jitterMs) / 1000f);

                        retryDelayMs = Math.Min(retryDelayMs * 2, this.maxDelayMs);

                        break;
                    case HttpStatusCode.Unauthorized:
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
                        break;
                    default:
                        state = RequestState.Stoped;

                        if (httpCache != null)
                        {
                            httpCache.TryStoring(request, httpResponse, error);
                        }
                        callback?.Invoke(httpResponse, null);
                        yield break;
                }
            }
            while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(this.totalTimeoutMs) || IsBearerAuthRequestPaused());

            if (httpCache != null)
            {
                httpCache.TryStoring(request, httpResponse, null);
            }
            callback?.Invoke(httpResponse, null);
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

            if (request.Headers.ContainsKey(authHeaderKey)) return;

            switch (request.AuthType)
            {
            case HttpAuth.Basic:
                if (string.IsNullOrEmpty(this.clientId)) return;

                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.clientId}:{this.clientSecret}"));
                request.Headers[authHeaderKey] = "Basic " + base64;
                break;
            case HttpAuth.Bearer:
                if (string.IsNullOrEmpty(this.accessToken)) return;

                request.Headers[authHeaderKey] = "Bearer " + this.accessToken;
                break;
            }
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