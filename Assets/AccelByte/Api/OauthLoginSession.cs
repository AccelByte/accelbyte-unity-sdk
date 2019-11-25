// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace AccelByte.Api
{
    internal class OauthLoginSession : ILoginSession
    {
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirecUri;
        private readonly IHttpWorker httpWorker;
        private readonly CoroutineRunner coroutineRunner;

        private Coroutine maintainAccessTokenCoroutine;
        private TokenData tokenData;
        private DateTime nextRefreshTime;
        private string clientToken;
        private DateTime clientTokenExpiryTime;

        internal OauthLoginSession(string baseUrl, string @namespace, string clientId, string clientSecret,
            string redirecUri, IHttpWorker httpWorker, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(@namespace, "Creating " + GetType().Name + " failed. Namespace parameter is null!");
            Assert.IsNotNull(clientId, "Creating " + GetType().Name + " failed. ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Creating " + GetType().Name + " failed. ClientSecret parameter is null!");
            Assert.IsNotNull(redirecUri, "Creating " + GetType().Name + " failed. RedirectUri parameter is null!");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.@namespace = @namespace;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirecUri = redirecUri;
            this.httpWorker = httpWorker;
            this.coroutineRunner = coroutineRunner;
        }

        public string AuthorizationToken { get { return this.tokenData != null ? this.tokenData.access_token : null; } }

        public string UserId { get { return this.tokenData != null ? this.tokenData.user_id : null; } }

        public IEnumerator LoginWithUsername(string username, string password, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            this.tokenData = result.Value;

            if (!result.IsError)
            {
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithDeviceId(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();

            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            this.tokenData = result.Value;

            if (!result.IsError)
            {
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithOtherPlatform(PlatformType platformType, string platformToken,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            this.tokenData = result.Value;

            if (!result.IsError)
            {
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithAuthorizationCode(string code, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(code, "Code parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", this.redirecUri)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            this.tokenData = result.Value;

            if (!result.IsError)
            {
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator Logout(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/revoke/token")
                .WithBearerAuth(this.AuthorizationToken)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", this.AuthorizationToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            this.tokenData = null;
            var result = response.TryParse();
            this.coroutineRunner.Stop(this.maintainAccessTokenCoroutine);
            callback.Try(result);
        }

        private IEnumerator RefreshToken(ResultCallback<TokenData> callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", this.tokenData.refresh_token)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            this.tokenData = result.Value;
            callback.Try(result);
        }

        private IEnumerator MaintainAccessToken()
        {
            TimeSpan refreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (true)
            {
                if (refreshBackoff >= OauthLoginSession.MaxBackoffInterval)
                {
                    yield break;
                }

                if (this.tokenData == null || DateTime.UtcNow < this.nextRefreshTime)
                {
                    yield return new WaitForSeconds(OauthLoginSession.WaitExpiryDelay / 1000f);

                    continue;
                }

                Result<TokenData> refreshResult = null;

                yield return RefreshToken(result => refreshResult = result);

                if (!refreshResult.IsError)
                {
                    this.nextRefreshTime = OauthLoginSession.ScheduleNormalRefresh(this.tokenData.expires_in);
                }
                else
                {
                    refreshBackoff = OauthLoginSession.CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                    this.nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                }
            }
        }

        private static DateTime ScheduleNormalRefresh(int expiresIn)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn + 1) * 0.8);
        }

        private static TimeSpan CalculateBackoffInterval(TimeSpan previousRefreshBackoff, int randomNum)
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);

            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
        }
    }
}