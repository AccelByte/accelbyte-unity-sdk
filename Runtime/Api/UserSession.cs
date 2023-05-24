// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        public static readonly string TokenPath = Path.Combine(Application.persistentDataPath, "TokenData");
        public const string RefreshTokenPlayerPrefKey = "accelbyte_refresh_token";
        public const string AuthTrustIdKey = "auth_trust_id";
        public readonly bool usePlayerPrefs;
        private readonly IHttpClient httpClient;
        private readonly string encodeUniqueKey;

        public event Action<string> RefreshTokenCallback;
        private Coroutine bearerAuthRejectedCoroutine;

        [UnityEngine.Scripting.Preserve]
        internal UserSession
            (IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , string encodeUniqueKey
            , bool inUsePlayerPrefs = false)
        {
            Assert.IsNotNull(inHttpClient, "inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner is null");

            usePlayerPrefs = inUsePlayerPrefs;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
            this.encodeUniqueKey = encodeUniqueKey;

            ((AccelByteHttpClient)httpClient).BearerAuthRejected += BearerAuthRejected;
            ((AccelByteHttpClient)httpClient).UnauthorizedOccured += UnauthorizedOccured;
        }

        public override string AuthorizationToken
        {
            get => tokenData?.access_token; 
            set => tokenData.access_token = value;
        }

        public string refreshToken => tokenData.refresh_token;
        
        public int refreshExpiresIn => tokenData.refresh_expires_in;

        public string UserId => tokenData?.user_id;

        public bool IsComply => tokenData?.is_comply ?? false;
        
        public RefreshTokenData localTokenData;

        public void ForceSetTokenData(TokenData inTokenData)
        {
            tokenData = inTokenData;
        }

        public void SetScheduleRefreshToken( DateTime time )
        {
            // don't schedule refresh token if time is in the past.
            if (time < DateTime.UtcNow)
            {
                return;
            }

            nextRefreshTime = time;

            AccelByteDebug.LogVerbose($"Set token refresh time to {nextRefreshTime}");

            if (maintainAccessTokenCoroutine == null)
            {
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainToken());
            }
        }

        private IEnumerator BearerAuthRejectRefreshToken( Action<string> callback )
        {
            yield return RefreshSession(refreshResult =>
            {
                callback?.Invoke(refreshResult.IsError ? null : refreshResult.Value.access_token);

                if (bearerAuthRejectedCoroutine != null)
                {
                    coroutineRunner.Stop(bearerAuthRejectedCoroutine);
                    bearerAuthRejectedCoroutine = null;
                }
            });
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

        public void LoadRefreshToken()
        {
            if (File.Exists(UserSession.TokenPath))
            {
                FileStream dataStream = new FileStream(UserSession.TokenPath, FileMode.Open);
                if (dataStream.Length == 0)
                {
                    AccelByteDebug.LogError($"[RefreshToken] Could not deserialize empty Refresh Token");
                    return;
                }
                BinaryFormatter formatter = new BinaryFormatter();
                localTokenData = formatter.Deserialize(dataStream) as RefreshTokenData;
                dataStream.Close();
                
                DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(encodeUniqueKey);
                string refreshToken = localTokenData?.refresh_token;
                refreshToken = Convert.FromBase64String(refreshToken).ToObject<string>();
                refreshToken = UserSession.XorString(deviceProvider.DeviceId, refreshToken);
                tokenData = new TokenData { refresh_token = refreshToken };
            }
            else
            {
                AccelByteDebug.LogError($"[RefreshToken] Could not find Refresh Token in specified path: {UserSession.TokenPath}");
            }
        }

        private void SaveRefreshToken()
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(encodeUniqueKey);
            string token = XorString(deviceProvider.DeviceId, tokenData.refresh_token);
            token = Convert.ToBase64String(token.ToUtf8Json());
            
            int epochTimeNow = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            int expirationDate = refreshExpiresIn + epochTimeNow;

            localTokenData = new RefreshTokenData
            {
                refresh_token = token,
                expiration_date = expirationDate,
            };

            FileStream fileStream = new FileStream(UserSession.TokenPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, localTokenData);
            fileStream.Close();
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
            HttpRequestBuilder.SetNamespace(loginResponse.Namespace);
            httpClient.SetImplicitBearerAuth(tokenData.access_token);
            httpClient.SetImplicitPathParams(
                new Dictionary<string, string>
                {
                    { "namespace", tokenData.Namespace }, { "userId", tokenData.user_id }
                });
            httpClient.ClearCookies();
            SaveRefreshToken();

            RefreshTokenCallback?.Invoke(tokenData.refresh_token);
            if (maintainAccessTokenCoroutine == null)
            {
                maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainToken());
            }
        }

        public override IEnumerator RefreshSessionApiCall(ResultCallback<TokenData, OAuthError> callback)
        {
            bool resreshDone = false;
            callback += (result) =>
            {
                resreshDone = true;
            };

            CallRefresh?.Invoke(tokenData.refresh_token, callback);
            yield return new WaitUntil(() => resreshDone);;
        }

        public void ClearSession()
        {
            if (maintainAccessTokenCoroutine != null)
            {
                coroutineRunner.Stop(maintainAccessTokenCoroutine);
                maintainAccessTokenCoroutine = null;
            }

            tokenData = null;
            localTokenData = null;
            httpClient.SetImplicitBearerAuth(null);
            httpClient.ClearImplicitPathParams();

            if (File.Exists(UserSession.TokenPath))
            {
                File.Delete(UserSession.TokenPath);
            }

            if (!usePlayerPrefs) return;

            PlayerPrefs.DeleteKey(RefreshTokenPlayerPrefKey);
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
