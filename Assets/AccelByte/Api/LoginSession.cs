// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace AccelByte.Api
{
    internal class LoginSession : ILoginSession
    {
        interface ILoginData
        {
            string AuthorizationToken { get; set; }
            string RefreshToken { get; set; }
            int ExpireIn { get; }

            string UserId { get; set; }

            string LoginWithUsernamePath { get; }
            string LoginWithDeviceIdPath { get; }
            string LoginWithOtherPlatformPath { get; }
            string LoginWithAuthorizationCodePath { get; }
            string LogoutPath { get; }
            string RefreshTokenPath { get; }
            string RefreshTokenKey { get; }

            Result TryToParse(IHttpResponse response);
            IHttpRequest CreateRefreshTokenRequest(string baseUrl, string clientId, string clientSecret);
            void Clear();
        }

        internal class OauthLoginData : ILoginData
        {
            private readonly object syncObject = new object();

            private TokenData tokenData;
            private string refreshToken;

            public string AuthorizationToken
            {
                get
                {
                    lock (this.syncObject)
                    {
                        return tokenData?.access_token;
                    }
                }
                set { tokenData.access_token = value; }
            }

            public string RefreshToken
            {
                get
                {
                    lock (this.syncObject)
                    {
                        return this.refreshToken;
                    }
                }
                set { this.refreshToken = value; }
            }

            public int ExpireIn => tokenData?.expires_in ?? 0;

            public string UserId { get => tokenData?.user_id; set => tokenData.user_id = value; }

            public string LoginWithUsernamePath => "/oauth/token";
            public string LoginWithDeviceIdPath => "/oauth/platforms/{platformId}/token";
            public string LoginWithOtherPlatformPath => "/oauth/platforms/{platformId}/token";
            public string LoginWithAuthorizationCodePath => "/oauth/token";
            public string LogoutPath => "/oauth/revoke/token";
            public string RefreshTokenPath => "/v3/oauth/token";

            public string RefreshTokenKey => "accelbyte_refresh_token";

            public Result TryToParse(IHttpResponse response)
            {
                Result<TokenData> result = response.TryParseJson<TokenData>();
                lock (this.syncObject)
                {
                    this.tokenData = result.Value;
                    this.refreshToken = tokenData.refresh_token;
                }

                if (result.IsError)
                {
                    return Result.CreateError(result.Error);
                }

                return Result.CreateOk();
            }

            public IHttpRequest CreateRefreshTokenRequest(string baseUrl, string clientId, string clientSecret)
            {
                return HttpRequestBuilder.CreatePost(baseUrl + RefreshTokenPath)
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", RefreshToken)
                .GetResult();
            }

            public void Clear()
            {
                this.tokenData = null;
            }
        }

        internal class SessionLoginData : ILoginData
        {
            private readonly object syncObject = new object();

            private SessionData sessionData;
            private string userId;
            private string refreshToken;

            public string AuthorizationToken { get => sessionData?.session_id; set => sessionData.session_id = value; }
            public string RefreshToken { get => this.refreshToken; set => this.refreshToken = value; }
            public int ExpireIn => sessionData?.expires_in ?? 0;

            public string UserId { get => this.userId ; set => this.userId = value; }

            public string LoginWithUsernamePath => "/v1/login/password";
            public string LoginWithDeviceIdPath => "/v1/login/platforms/{platformId}";
            public string LoginWithOtherPlatformPath => "/v1/login/platforms/{platformId}";
            public string LoginWithAuthorizationCodePath => "/v1/login/code";
            public string LogoutPath => "/v1/logout";
            public string RefreshTokenPath => "/v1/sessions/refresh";

            public string RefreshTokenKey => "accelbyte_api_refresh_token";

            public Result TryToParse(IHttpResponse response)
            {
                Result<SessionData> result = response.TryParseJson<SessionData>();
                lock (this.syncObject)
                {
                    this.sessionData = result.Value;
                    if (!result.IsError)
                    {
                        this.RefreshToken = sessionData.refresh_id;
                    }
                }

                if (result.IsError)
                {
                    return Result.CreateError(result.Error);
                }

                return Result.CreateOk();
            }

            public IHttpRequest CreateRefreshTokenRequest(string baseUrl, string clientId, string clientSecret)
            {
                return HttpRequestBuilder.CreatePost(baseUrl + RefreshTokenPath)
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("refresh_id", RefreshToken)
                .GetResult();
            }

            public void Clear()
            {
                this.sessionData = null;
            }
        }

        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirecUri;
        private readonly bool usePlayerPrefs;
        private readonly IHttpWorker httpWorker;
        private readonly CoroutineRunner coroutineRunner;

        private Coroutine maintainAccessTokenCoroutine;
        private DateTime nextRefreshTime;
        private ILoginData loginData;

        public event Action<string> RefreshTokenCallback;

        internal LoginSession(string baseUrl, string @namespace, string clientId, string clientSecret,
            string redirecUri, IHttpWorker httpWorker, CoroutineRunner coroutineRunner, bool useSession = false, bool usePlayerPrefs = false)
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
            this.usePlayerPrefs = usePlayerPrefs;
            this.httpWorker = httpWorker;
            this.coroutineRunner = coroutineRunner;

            if (useSession)
            {
                this.loginData = new SessionLoginData();
            }
            else
            {
                this.loginData = new OauthLoginData();
            }
        }

        public string AuthorizationToken
        {
            get { return this.loginData.AuthorizationToken; }
            set { this.loginData.AuthorizationToken = value; }
        }

        public string UserId
        {
            get { return this.loginData.UserId; }
            set { this.loginData.UserId = value; }
        }

        public IEnumerator LoginWithUsername(string username, string password, ResultCallback callback, bool rememberMe = false)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + this.loginData.LoginWithUsernamePath)
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", this.@namespace)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = this.loginData.TryToParse(response);

            if (!result.IsError)
            {
                if (usePlayerPrefs)
                {
                    SaveRefreshToken();
                }
                if (RefreshTokenCallback != null)
                {
                    RefreshTokenCallback.Invoke(this.loginData.RefreshToken);
                }
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

            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + this.loginData.LoginWithDeviceIdPath)
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = this.loginData.TryToParse(response);

            if (!result.IsError)
            {
                if (usePlayerPrefs)
                {
                    SaveRefreshToken();
                }
                if (RefreshTokenCallback != null)
                {
                    RefreshTokenCallback.Invoke(this.loginData.RefreshToken);
                }
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

            if (platformType == PlatformType.Stadia)
            {
                platformToken = platformToken.TrimEnd('=');
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + this.loginData.LoginWithOtherPlatformPath)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = this.loginData.TryToParse(response);

            if (!result.IsError)
            {
                if (usePlayerPrefs)
                {
                    SaveRefreshToken();
                }
                if (RefreshTokenCallback != null)
                {
                    RefreshTokenCallback.Invoke(this.loginData.RefreshToken);
                }
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

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + this.loginData.LoginWithAuthorizationCodePath)
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", this.redirecUri)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = this.loginData.TryToParse(response);

            if (!result.IsError)
            {
                if (usePlayerPrefs)
                {
                    SaveRefreshToken();
                }
                if (RefreshTokenCallback != null)
                {
                    RefreshTokenCallback.Invoke(this.loginData.RefreshToken);
                }
                this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
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
            if(refreshToken != null)
            {
                this.loginData.RefreshToken = refreshToken;
                yield return RefreshToken(callback);
            }
            else if (usePlayerPrefs)
            {
                if (PlayerPrefs.HasKey(this.loginData.RefreshTokenKey))
                {
                    this.loginData.RefreshToken = LoadRefreshToken();
                    yield return RefreshToken(callback);
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

            // user already logout
            if (this.AuthorizationToken == null)
            {
                callback.TryOk();
                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + this.loginData.LogoutPath)
                .WithBearerAuth(this.AuthorizationToken)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", this.AuthorizationToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            this.coroutineRunner.Stop(this.maintainAccessTokenCoroutine);
            callback.Try(Result.CreateOk());
            if (usePlayerPrefs)
            {
                if (PlayerPrefs.HasKey(this.loginData.RefreshTokenKey))
                {
                    PlayerPrefs.DeleteKey(this.loginData.RefreshTokenKey);
                    PlayerPrefs.Save();
                }
            }
            this.loginData.Clear();
        }

        private IEnumerator RefreshToken(ResultCallback callback)
        {
            var request = this.loginData.CreateRefreshTokenRequest(this.baseUrl, this.clientId, this.clientSecret);

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = this.loginData.TryToParse(response);

            if (!result.IsError)
            {
                if (usePlayerPrefs)
                {
                    SaveRefreshToken();
                }
                if (RefreshTokenCallback != null)
                {
                    RefreshTokenCallback.Invoke(this.loginData.RefreshToken);
                }
                if (this.maintainAccessTokenCoroutine == null)
                {
                    this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                }
            }
            callback.Try(result);
        }

        private IEnumerator MaintainAccessToken()
        {
            this.nextRefreshTime = LoginSession.ScheduleNormalRefresh(this.loginData.ExpireIn);
            TimeSpan refreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (true)
            {
                if (refreshBackoff >= LoginSession.MaxBackoffInterval)
                {
                    yield break;
                }

                if (this.loginData.AuthorizationToken == null || DateTime.UtcNow < this.nextRefreshTime)
                {
                    yield return new WaitForSeconds(LoginSession.WaitExpiryDelay / 1000f);

                    continue;
                }

                Result refreshResult = null;

                yield return RefreshToken(result => refreshResult = result);

                if (!refreshResult.IsError)
                {
                    this.nextRefreshTime = LoginSession.ScheduleNormalRefresh(this.loginData.ExpireIn);
                }
                else
                {
                    refreshBackoff = LoginSession.CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                    this.nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                }
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

        private string LoadRefreshToken()
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();
            string token = PlayerPrefs.GetString(this.loginData.RefreshTokenKey);
            var bytes = Convert.FromBase64String(token);
            token = bytes.ToObject<string>();
            return xorIt(deviceProvider.DeviceId, token);
        }

        private void SaveRefreshToken()
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();
            string token = xorIt(deviceProvider.DeviceId, this.loginData.RefreshToken);
            token = Convert.ToBase64String(token.ToUtf8Json());
            PlayerPrefs.SetString(this.loginData.RefreshTokenKey, token);
            PlayerPrefs.Save();
        }

        private static string xorIt(string key, string input)
        {
            string xor = "";
            for (int i = 0; i < input.Length; i++)
                xor += (char)(input[i] ^ key[(i % key.Length)]);

            return xor;
        }
    }
}
