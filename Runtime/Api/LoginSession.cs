// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace AccelByte.Api
{
    public class LoginSession : IUserSession
    {
        const string RefreshTokenKey = "accelbyte_refresh_token";
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly string redirectUri;
        private readonly bool usePlayerPrefs;
        private readonly IHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;
        private TokenData tokenData;

        private Coroutine maintainAccessTokenCoroutine;
        private DateTime nextRefreshTime;

        public event Action<string> RefreshTokenCallback;
        private Coroutine bearerAuthRejectedCoroutine;

        internal LoginSession(
            string baseUrl,
            string @namespace,
            string redirectUri,
            IHttpClient httpClient,
            CoroutineRunner coroutineRunner,
            bool usePlayerPrefs = false)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + " is null");
            Assert.IsNotNull(@namespace, nameof(@namespace) + " is null");
            Assert.IsNotNull(redirectUri, nameof(redirectUri) + " is null");
            Assert.IsNotNull(httpClient, nameof(httpClient) + " is null");
            Assert.IsNotNull(coroutineRunner, nameof(coroutineRunner) + " is null");

            this.baseUrl = baseUrl;
            this.@namespace = @namespace;
            this.redirectUri = redirectUri;
            this.usePlayerPrefs = usePlayerPrefs;
            this.httpClient = httpClient;
            this.coroutineRunner = coroutineRunner;

            ((AccelByteHttpClient)this.httpClient).BearerAuthRejected += BearerAuthRejected;
            ((AccelByteHttpClient)this.httpClient).UnauthorizedOccured += UnauthorizedOccured;
        }

        public string AuthorizationToken { get => this.tokenData?.access_token; set => this.tokenData.access_token = value; }

        public string UserId => this.tokenData?.user_id;

        public bool IsComply => this.tokenData?.is_comply ?? false;

        public IEnumerator LoginWithUsername(
            string username,
            string password,
            ResultCallback callback,
            bool rememberMe = false)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", this.@namespace)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                this.SetSession(result.Value);
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

            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/platforms/device/token")
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                this.SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithOtherPlatform(
            PlatformType platformType,
            string platformToken,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

            if (platformType == PlatformType.Stadia)
            {
                platformToken = platformToken.TrimEnd('=');
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                this.SetSession(result.Value);
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
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", this.redirectUri)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                this.SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithLatestRefreshToken(string refreshToken, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (refreshToken != null)
            {
                this.tokenData = new TokenData { refresh_token = refreshToken };
                yield return this.RefreshSession(callback);
            }
            else if (this.usePlayerPrefs)
            {
                if (PlayerPrefs.HasKey(LoginSession.RefreshTokenKey))
                {
                    this.LoadRefreshToken();
                    yield return this.RefreshSession(callback);
                }
                else
                {
                    callback.TryError(ErrorCode.InvalidRequest, "Refresh token not found!");
                }
            }
            else
            {
                callback.TryError(ErrorCode.InvalidRequest, "Refresh Token is null or PlayerPrefs is disabled!");
            }
        }

        public IEnumerator Logout(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            // user already logout.
            if (!this.IsValid())
            {
                callback.TryOk();
                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/logout")
                .WithBearerAuth(this.AuthorizationToken)
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            this.ClearSession();
            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator RefreshSession(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", this.tokenData.refresh_token)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                this.SetSession(result.Value);
                callback.TryOk();
                yield break;
            }

            callback.TryError(result.Error);
        }

        private IEnumerator MaintainSession()
        {
            this.nextRefreshTime = LoginSession.ScheduleNormalRefresh(this.tokenData.expires_in);
            TimeSpan refreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (this.tokenData != null)
            {
                if (refreshBackoff >= LoginSession.MaxBackoffInterval)
                {
                    yield break;
                }

                if (this.tokenData.access_token == null || DateTime.UtcNow < this.nextRefreshTime)
                {
                    yield return new WaitForSeconds(LoginSession.WaitExpiryDelay / 1000f);

                    continue;
                }

                Result refreshResult = null;

                yield return this.RefreshSession(result => refreshResult = result);

                if (!refreshResult.IsError)
                {
                    this.nextRefreshTime = LoginSession.ScheduleNormalRefresh(this.tokenData.expires_in);
                }
                else
                {
                    refreshBackoff = LoginSession.CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                    this.nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                }
            }
        }
        private IEnumerator BearerAuthRejectRefreshToken(Action<string> callback)
        {
            Result refreshResult = null;

            yield return this.RefreshSession(result => refreshResult = result);

            callback?.Invoke(this.tokenData.access_token);
            if (this.bearerAuthRejectedCoroutine != null)
            {
                this.coroutineRunner.Stop(this.bearerAuthRejectedCoroutine);
                this.bearerAuthRejectedCoroutine = null;
            }
        }

        private void BearerAuthRejected(string accessToken, Action<string> callback)
        {
            if (accessToken == this.tokenData?.access_token && 
                this.bearerAuthRejectedCoroutine == null &&
                this.maintainAccessTokenCoroutine != null)
            {
                this.coroutineRunner.Stop(this.maintainAccessTokenCoroutine);
                this.bearerAuthRejectedCoroutine = this.coroutineRunner.Run(this.BearerAuthRejectRefreshToken(callback));
            }
        }

        private void UnauthorizedOccured(string accessToken)
        {
            if (accessToken == this.tokenData?.access_token)
            {
                this.ClearSession();
            }
        }

        private static DateTime ScheduleNormalRefresh(int expiresIn)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn - 1) * 0.8);
        }

        private static TimeSpan CalculateBackoffInterval(TimeSpan previousRefreshBackoff, int randomNum)
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);

            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
        }

        private void LoadRefreshToken()
        {
            var deviceProvider = DeviceProvider.GetFromSystemInfo();
            var refreshToken = PlayerPrefs.GetString(LoginSession.RefreshTokenKey);
            refreshToken = Convert.FromBase64String(refreshToken).ToObject<string>();
            refreshToken = LoginSession.XorString(deviceProvider.DeviceId, refreshToken);
            this.tokenData = new TokenData { refresh_token = refreshToken };
        }

        private void SaveRefreshToken()
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();
            string token = LoginSession.XorString(deviceProvider.DeviceId, this.tokenData.refresh_token);
            token = Convert.ToBase64String(token.ToUtf8Json());
            PlayerPrefs.SetString(LoginSession.RefreshTokenKey, token);
            PlayerPrefs.Save();
        }

        private static string XorString(string key, string input)
        {
            var resultBuilder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                resultBuilder.Append((char)(input[i] ^ key[(i % key.Length)]));
            }

            return resultBuilder.ToString();
        }

        private void SetSession(TokenData loginResponse)
        {
            Assert.IsNotNull(loginResponse);
            this.tokenData = loginResponse;
            this.httpClient.SetImplicitBearerAuth(this.tokenData.access_token);
            this.httpClient.SetImplicitPathParams(
                new Dictionary<string, string>
                {
                    { "namespace", this.tokenData.Namespace }, { "userId", this.tokenData.user_id }
                });
            this.httpClient.ClearCookies();

            if (this.usePlayerPrefs)
            {
                this.SaveRefreshToken();
            }

            this.RefreshTokenCallback?.Invoke(this.tokenData.refresh_token);
            if (this.maintainAccessTokenCoroutine == null)
            {
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(this.MaintainSession());
            }

        }

        private void ClearSession()
        {
            if (this.maintainAccessTokenCoroutine != null)
            {
                this.coroutineRunner.Stop(this.maintainAccessTokenCoroutine);
            }

            this.tokenData = null;
            this.httpClient.SetImplicitBearerAuth(null);
            this.httpClient.ClearImplicitPathParams();

            if (!this.usePlayerPrefs) return;

            PlayerPrefs.DeleteKey(LoginSession.RefreshTokenKey);
            PlayerPrefs.Save();
        }
    }

}