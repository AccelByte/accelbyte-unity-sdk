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
    public class UserSession : ISession
    {
        public const string RefreshTokenKey = "accelbyte_refresh_token";
        public readonly bool usePlayerPrefs;
        private readonly IHttpClient httpClient;

        public event Action<string> RefreshTokenCallback;
        private Coroutine bearerAuthRejectedCoroutine;

        internal UserSession
            (IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , bool inUsePlayerPrefs = false)
        {
            Assert.IsNotNull(inHttpClient, "inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner is null");

            usePlayerPrefs = inUsePlayerPrefs;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;

            ((AccelByteHttpClient)httpClient).BearerAuthRejected += BearerAuthRejected;
            ((AccelByteHttpClient)httpClient).UnauthorizedOccured += UnauthorizedOccured;
        }

        public override string AuthorizationToken
        {
            get => tokenData?.access_token; 
            set => tokenData.access_token = value;
        }

        public string refreshToken => tokenData.refresh_token;

        public string UserId => tokenData?.user_id;

        public bool IsComply => tokenData?.is_comply ?? false;

        public void ForceSetTokenData(TokenData inTokenData)
        {
            tokenData = inTokenData;
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
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainToken());
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

        public void LoadRefreshToken()
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

        public override void SetSession(TokenData loginResponse)
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
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainToken());
            }
        }

        public override IEnumerator RefreshSessionApiCall(ResultCallback<TokenData, OAuthError> callback)
        {
            yield return CallRefresh(tokenData.refresh_token, callback);
        }

        public void ClearSession()
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

    public static class LoginSessionExtension
    {
        public static bool IsValid(this UserSession session)
        {
            return !string.IsNullOrEmpty(session.AuthorizationToken);
        }

        public static void AssertValid(this UserSession session)
        {
            Assert.IsNotNull(session);
            Assert.IsFalse(string.IsNullOrEmpty(session.AuthorizationToken));
        }
    }
}
