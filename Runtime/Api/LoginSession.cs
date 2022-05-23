// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
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
    /// <summary>
    /// This is the actual User's session information (userId, AuthorizationToken...)
    /// <remarks>Not to be confused with Api/User.cs, which is a gateway to the User API.</remarks> 
    /// </summary>
    public class LoginSession : IUserSession
    {
        const string RefreshTokenKey = "accelbyte_refresh_token";
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string baseUrl;
        private readonly string namespace_;
        private readonly string redirectUri;
        private readonly bool usePlayerPrefs;
        private readonly IHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;
        private TokenData tokenData;

        private Coroutine maintainAccessTokenCoroutine;
        private DateTime nextRefreshTime;

        public event Action<string> RefreshTokenCallback;
        private Coroutine bearerAuthRejectedCoroutine;

        internal LoginSession
            ( string inBaseUrl
            , string inNamespace
            , string inRedirectUri
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , bool inUsePlayerPrefs = false)
        {
            Assert.IsNotNull(inRedirectUri, "inRedirectUri is null");
            Assert.IsNotNull(inHttpClient, "inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner is null");

            redirectUri = inRedirectUri;
            usePlayerPrefs = inUsePlayerPrefs;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
            baseUrl = inBaseUrl;
            namespace_ = inNamespace;

            ((AccelByteHttpClient)httpClient).BearerAuthRejected += BearerAuthRejected;
            ((AccelByteHttpClient)httpClient).UnauthorizedOccured += UnauthorizedOccured;
        }

        public string AuthorizationToken
        {
            get => tokenData?.access_token; 
            set => tokenData.access_token = value;
        }

        public string UserId => tokenData?.user_id;

        public bool IsComply => tokenData?.is_comply ?? false;

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithUsername
            ( string username
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", namespace_)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithUsername
            ( string username
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/oauth/token")
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", namespace_)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.Try(result);
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithUsernameV3
            ( string username
            , string password
            , ResultCallback callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", namespace_)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithUsernameV3
            ( string username
            , string password
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberMe = false )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/token")
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", namespace_)
                .WithFormParam("extend_exp", rememberMe ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithDeviceId( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();

            IHttpRequest request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/platforms/device/token")
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithDeviceId( ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();

            IHttpRequest request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/platforms/device/token")
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithOtherPlatform
            ( PlatformType platformType
            , string platformToken
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

            if (platformType == PlatformType.Stadia)
            {
                platformToken = platformToken.TrimEnd('=');
            }

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithOtherPlatform
            ( PlatformType platformType
            , string platformToken
            , ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

            if (platformType == PlatformType.Stadia)
            {
                platformToken = platformToken.TrimEnd('=');
            }

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithAuthorizationCode
            ( string code
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "Code parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", redirectUri)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        public IEnumerator LoginWithAuthorizationCode
            ( string code
            , ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "Code parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/oauth/token")
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", redirectUri)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator LoginWithLatestRefreshToken
            ( string refreshToken
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (refreshToken != null)
            {
                tokenData = new TokenData { refresh_token = refreshToken };
                yield return RefreshSession(callback);
            }
            else if (usePlayerPrefs)
            {
                if (PlayerPrefs.HasKey(LoginSession.RefreshTokenKey))
                {
                    LoadRefreshToken();
                    yield return RefreshSession(callback);
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

        public IEnumerator LoginWithLatestRefreshToken
            ( string refreshToken
            , ResultCallback<TokenData, OAuthError> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (refreshToken != null)
            {
                tokenData = new TokenData { refresh_token = refreshToken };
                yield return RefreshSession(callback);
            }
            else if (usePlayerPrefs)
            {
                if (PlayerPrefs.HasKey(RefreshTokenKey))
                {
                    LoadRefreshToken();
                    yield return RefreshSession(callback);
                }
                else
                {
                    OAuthError error = new OAuthError()
                    {
                        error = ErrorCode.InvalidRequest.ToString(),
                        error_description = "Refresh token not found!"
                    };
                    callback.TryError(error);
                }
            }
            else
            {
                OAuthError error = new OAuthError()
                {
                    error = ErrorCode.InvalidRequest.ToString(),
                    error_description = "Refresh Token is null or PlayerPrefs is disabled!"
                };
                callback.TryError(error);
            }
        }
        
        public IEnumerator Logout( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            // user already logout.
            if (!this.IsValid())
            {
                callback.TryOk();
                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/logout")
                .WithBearerAuth(AuthorizationToken)
                .WithContentType(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            ClearSession();
            var result = response.TryParse();
            callback.Try(result);
        }

        [Obsolete("Instead, use the overload with the extended callback")]
        public IEnumerator RefreshSession( ResultCallback callback )
        {
            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/token")
                .WithBasicAuth()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", tokenData.refresh_token)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
                yield break;
            }

            callback.TryError(result.Error);
        }

        public IEnumerator RefreshSession( ResultCallback<TokenData, OAuthError> callback )
        {
            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/token")
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", tokenData.refresh_token)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
                yield break;
            }

            callback.TryError(result.Error);
        }

        public void SetScheduleRefreshToken( DateTime time )
        {
            // don't schedule refresh token if time is in the past.
            if (time < DateTime.UtcNow)
                return;

            nextRefreshTime = time;

            Debug.Log($"set refresh time to {nextRefreshTime}");

            if (maintainAccessTokenCoroutine == null)
            {
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainSession());
            }
        }

        public IEnumerator Verify2FACode
            ( string mfaToken
            , TwoFAFactorType factor
            , string code
            , ResultCallback<TokenData, OAuthError> callback
            , bool rememberDevice = false )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(mfaToken, "mfaToken parameter is null.");
            Assert.IsNotNull(code, "code parameter is null.");

            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/mfa/verify")
                .WithBasicAuthWithCookie()
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("mfaToken", mfaToken)
                .WithFormParam("factor", factor.GetString())
                .WithFormParam("code", code)
                .WithFormParam("rememberDevice", rememberDevice ? "true" : "false")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SetSession(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        }

        private IEnumerator MaintainSession()
        {
            nextRefreshTime = ScheduleNormalRefresh(tokenData.expires_in);
            TimeSpan refreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (tokenData != null)
            {
                if (refreshBackoff >= MaxBackoffInterval)
                {
                    yield break;
                }

                if (tokenData.access_token == null || DateTime.UtcNow < nextRefreshTime)
                {
                    yield return new WaitForSeconds(WaitExpiryDelay / 1000f);

                    continue;
                }

                if (AccelBytePlugin.GetUser().TwoFAEnable)
                {
                    Result<TokenData, OAuthError> refreshResult = null;

                    yield return RefreshSession(result => refreshResult = result);

                    if (!refreshResult.IsError)
                    {
                        nextRefreshTime = ScheduleNormalRefresh(tokenData.expires_in);
                    }
                    else
                    {
                        refreshBackoff = CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                        nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                    }
                }
                else
                {
                    Result refreshResult = null;

                    yield return RefreshSession(result => refreshResult = result);

                    if (!refreshResult.IsError)
                    {
                        nextRefreshTime = ScheduleNormalRefresh(tokenData.expires_in);
                    }
                    else
                    {
                        refreshBackoff = CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                        nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                    }
                }
            }
        }

        private IEnumerator BearerAuthRejectRefreshToken( Action<string> callback )
        {
            if (AccelBytePlugin.GetUser().TwoFAEnable)
            {
                Result<TokenData, OAuthError> refreshResult = null;

                yield return RefreshSession(result => refreshResult = result);
            }
            else
            {
                Result<TokenData, OAuthError> refreshResult = null;

                yield return RefreshSession(result => refreshResult = result);
            }

            callback?.Invoke(tokenData.access_token);
            if (bearerAuthRejectedCoroutine != null)
            {
                coroutineRunner.Stop(bearerAuthRejectedCoroutine);
                bearerAuthRejectedCoroutine = null;
            }
        }

        private void BearerAuthRejected
            ( string accessToken
            , Action<string> callback )
        {
            if (accessToken == tokenData?.access_token && 
                bearerAuthRejectedCoroutine == null &&
                maintainAccessTokenCoroutine != null)
            {
                coroutineRunner.Stop(maintainAccessTokenCoroutine);
                bearerAuthRejectedCoroutine = coroutineRunner.Run(BearerAuthRejectRefreshToken(callback));
            }
        }

        private void UnauthorizedOccured( string accessToken )
        {
            if (accessToken == tokenData?.access_token)
            {
                ClearSession();
            }
        }

        private static DateTime ScheduleNormalRefresh( int expiresIn )
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn - 1) * 0.8);
        }

        private static TimeSpan CalculateBackoffInterval
            ( TimeSpan previousRefreshBackoff
            , int randomNum )
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);

            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
        }

        private void LoadRefreshToken()
        {
            var deviceProvider = DeviceProvider.GetFromSystemInfo();
            var refreshToken = PlayerPrefs.GetString(RefreshTokenKey);
            refreshToken = Convert.FromBase64String(refreshToken).ToObject<string>();
            refreshToken = XorString(deviceProvider.DeviceId, refreshToken);
            tokenData = new TokenData { refresh_token = refreshToken };
        }

        private void SaveRefreshToken()
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();
            string token = XorString(deviceProvider.DeviceId, tokenData.refresh_token);
            token = Convert.ToBase64String(token.ToUtf8Json());
            PlayerPrefs.SetString(RefreshTokenKey, token);
            PlayerPrefs.Save();
        }

        private static string XorString
            ( string key
            , string input )
        {
            var resultBuilder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                resultBuilder.Append((char)(input[i] ^ key[(i % key.Length)]));
            }

            return resultBuilder.ToString();
        }

        private void SetSession( TokenData loginResponse )
        {
            Assert.IsNotNull(loginResponse);
            tokenData = loginResponse;
            httpClient.SetImplicitBearerAuth(tokenData.access_token);
            httpClient.SetImplicitPathParams(
                new Dictionary<string, string>
                {
                    { "namespace", tokenData.Namespace }, { "userId", tokenData.user_id }
                });
            httpClient.ClearCookies();

            if (usePlayerPrefs)
            {
                SaveRefreshToken();
            }

            RefreshTokenCallback?.Invoke(tokenData.refresh_token);
            if (maintainAccessTokenCoroutine == null)
            {
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainSession());
            }

        }

        private void ClearSession()
        {
            if (maintainAccessTokenCoroutine != null)
            {
                coroutineRunner.Stop(maintainAccessTokenCoroutine);
                maintainAccessTokenCoroutine = null;
            }

            tokenData = null;
            httpClient.SetImplicitBearerAuth(null);
            httpClient.ClearImplicitPathParams();

            if (!usePlayerPrefs) return;

            PlayerPrefs.DeleteKey(RefreshTokenKey);
            PlayerPrefs.Save();
        }
    }

}
