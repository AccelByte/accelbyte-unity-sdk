// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
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

namespace AccelByte.Api
{
    /// <summary>
    /// This is the actual User's session information (userId, AuthorizationToken...)
    /// <remarks>Not to be confused with Api/User.cs, which is a gateway to the User API.</remarks> 
    /// </summary>
    public class UserSession : ISession
    {
        public const string RefreshTokenPlayerPrefKey = "accelbyte_refresh_token";
        public const string AuthTrustIdKey = "auth_trust_id";
        public const string LastLoginUserCacheKey = "LastLoginUser";
        public readonly bool usePlayerPrefs;
        private readonly IHttpClient httpClient;
        private readonly string encodeUniqueKey;

        public event Action<string> RefreshTokenCallback;
        public string TokenTableName
        {
            get;
            private set;
        }

        public RefreshTokenData localTokenData;

        private string localTokenDataCacheKey;

        IAccelByteDataStorage dataStorage;

        public override string AuthorizationToken
        {
            get => tokenData?.access_token;
            set => tokenData.access_token = value;
        }

        public string refreshToken
        {
            get
            {
                string retval = tokenData != null ? tokenData.refresh_token : null;
                return retval;
            }
        }

        public int refreshExpiresIn
        {
            get
            {
                int retval = tokenData != null ? tokenData.refresh_expires_in : -1;
                return retval;
            }
        }

        public override string UserId
        {
            get => tokenData?.user_id;
        }

        public string PlatformId
        {
            get => tokenData?.platform_id;
        }

        public string PlatformUserId
        {
            get => tokenData?.platform_user_id;
        }

        public string DeviceId
        {
            get => tokenData?.DeviceId;
        }

        public bool IsComply => tokenData?.is_comply ?? false;

        internal IAccelByteDataStorage DataStorage
        {
            get
            {
                return dataStorage;
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal UserSession
            (IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , string encodeUniqueKey
            , bool inUsePlayerPrefs = false) : this(inHttpClient, inCoroutineRunner, encodeUniqueKey, inUsePlayerPrefs, "TokenCache/TokenData", new Core.AccelByteDataStorageBinaryFile(AccelByteSDK.Implementation.FileStream))
        {
            CreateSessionMaintainer();
        }

        [UnityEngine.Scripting.Preserve]
        internal UserSession
            (IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , string encodeUniqueKey
            , bool inUsePlayerPrefs
            , string tokenTableName
            , IAccelByteDataStorage dataStorage)
        {
            Assert.IsNotNull(inHttpClient, "inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner is null");
            Assert.IsNotNull(dataStorage, "dataStorage is null");

            usePlayerPrefs = inUsePlayerPrefs;
            httpClient = inHttpClient;
            this.encodeUniqueKey = encodeUniqueKey;

            TokenTableName = tokenTableName;
            this.dataStorage = dataStorage;

            ((AccelByteHttpClient)httpClient).BearerAuthRejected += BearerAuthRejected;
            ((AccelByteHttpClient)httpClient).UnauthorizedOccured += UnauthorizedOccured;

            CreateSessionMaintainer();
        }

        public override AccelByteResult<TokenData, OAuthError> RefreshSessionApiCall()
        {
            var retval = new AccelByteResult<TokenData, OAuthError>();
            CallRefresh?.Invoke(tokenData.refresh_token, result =>
            {
                if (result.IsError)
                {
                    retval.Reject(result.Error);
                    return;
                }
                retval.Resolve(result.Value);
            });
            return retval;
        }

        public void SetScheduleRefreshToken( DateTime time )
        {
            // don't schedule refresh token if time is in the past.
            if (time < DateTime.UtcNow)
            {
                return;
            }

            nextRefreshTime = time;

            float nextRefreshInSeconds = (float) (nextRefreshTime - DateTime.UtcNow).TotalSeconds;
            SessionMaintainer?.Start(SharedMemory?.CoreHeartBeat, SharedMemory?.Logger, nextRefreshInSeconds);
        }

        private void BearerAuthRejected
            ( string accessToken
            , Action<string> callback )
        {
            if (accessToken == tokenData?.access_token)
            {
                SessionMaintainer?.Stop();
                CallRefresh?.Invoke(tokenData.refresh_token, result =>
                {
                    string newAccessToken = null;
                    if (!result.IsError && result.Value != null)
                    {
                        newAccessToken = result.Value.access_token;
                        SetSession(result.Value);
                    }

                    callback?.Invoke(newAccessToken);
                });
            }
        }

        private void UnauthorizedOccured( string accessToken )
        {
            if (accessToken == tokenData?.access_token)
            {
                ClearSession();
            }
        }

        public void LoadRefreshToken(string cacheKey, Action<bool> onDone)
        {
            Assert.IsNotNull(onDone);

            GetRefreshTokenFromCache(cacheKey, (cachedRefreshTokenData) =>
            {
                if (cachedRefreshTokenData == null)
                {
                    onDone.Invoke(false);
                    return;
                }

                string encodedToken = EncodeToken(tokenData.refresh_token);
                localTokenData = new RefreshTokenData()
                {
                    refresh_token = encodedToken,
                    expiration_date = cachedRefreshTokenData.expiration_date
                };
                tokenData = new TokenData { refresh_token = cachedRefreshTokenData.refresh_token };
                onDone.Invoke(true);
            });
        }

        public void GetRefreshTokenFromCache(string cacheKey, Action<RefreshTokenData> onDone)
        {
            Assert.IsNotNull(onDone);

            Action<bool, RefreshTokenData> onGetDone = (isSuccess, storageTokenData) =>
            {
                if(!isSuccess)
                {
                    onDone?.Invoke(null);
                    return;
                }

                string decodedRefreshToken = DecodeToken(storageTokenData.refresh_token);
                var plainTokenData = new RefreshTokenData()
                {
                    refresh_token = decodedRefreshToken,
                    expiration_date = storageTokenData.expiration_date
                };
                onDone?.Invoke(plainTokenData);
            };

            dataStorage.GetItem(cacheKey, onGetDone, TokenTableName);
        }

        internal void SaveRefreshToken(string cacheKey, bool updateLastUserLoginCache, Action<bool> onDone)
        {
            SaveRefreshToken(cacheKey, tokenData, updateLastUserLoginCache, onDone);
        }

        internal void SaveRefreshToken(string cacheKey, TokenData tokenData, bool updateLastUserLoginCache, Action<bool> onDone)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                onDone?.Invoke(false);
            }
            else
            {
                string encodedToken = EncodeToken(tokenData.refresh_token);

                int epochTimeNow = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                int expirationDate = refreshExpiresIn + epochTimeNow;

                localTokenData = new RefreshTokenData
                {
                    refresh_token = encodedToken,
                    expiration_date = expirationDate,
                };

                localTokenDataCacheKey = cacheKey;

                var keyValueSaveInfo = new List<Tuple<string, object>>()
                {
                    new Tuple<string, object>(cacheKey, localTokenData)
                };
                if (updateLastUserLoginCache)
                {
                    keyValueSaveInfo.Add(new Tuple<string, object>(LastLoginUserCacheKey, localTokenData));
                }
                dataStorage.SaveItems(keyValueSaveInfo, onDone, TokenTableName);
            }
        }

        internal void SetDataStorage(IAccelByteDataStorage newDataStorage)
        {
            dataStorage = newDataStorage;
        }

        private string EncodeToken(string refreshToken)
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(encodeUniqueKey, deviceIdGeneratorConfig: SharedMemory?.DeviceIdGeneratorConfig);
            string encodedToken = XorString(deviceProvider.DeviceId, refreshToken);
            encodedToken = Convert.ToBase64String(encodedToken.ToUtf8Json());
            return encodedToken;
        }

        private string DecodeToken(string encodedRefreshToken)
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(encodeUniqueKey, deviceIdGeneratorConfig: SharedMemory?.DeviceIdGeneratorConfig);
            string decodedRefreshToken = Convert.FromBase64String(encodedRefreshToken).ToObject<string>();
            decodedRefreshToken = XorString(deviceProvider.DeviceId, decodedRefreshToken);
            return decodedRefreshToken;
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

            var additionalPathParam = new Dictionary<string, string>();
            if(!string.IsNullOrEmpty(tokenData.Namespace))
            {
                const string namespaceHeaderKey = "namespace";
                additionalPathParam.Add(namespaceHeaderKey, tokenData.Namespace);
            }
            if (!string.IsNullOrEmpty(tokenData.user_id))
            {
                const string userIdHeaderKey = "userId";
                additionalPathParam.Add(userIdHeaderKey, tokenData.user_id);
            }

            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.NamespaceHeaderKey, tokenData.Namespace);
            httpClient.SetImplicitBearerAuth(tokenData.access_token);
            httpClient.SetImplicitPathParams(additionalPathParam);
            httpClient.ClearCookies();

            RefreshTokenCallback?.Invoke(tokenData.refresh_token);
            SessionMaintainer?.Start(SharedMemory?.CoreHeartBeat, SharedMemory?.Logger, tokenData.expires_in * 0.8f);
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

        public void ClearSession(bool deleteCache = false)
        {
            SessionMaintainer?.Stop();

            ClearThirdPartyPlatformTokenData();

            tokenData = null;
            localTokenData = null;
            httpClient.SetImplicitBearerAuth(null);
            httpClient.ClearImplicitPathParams();

            if(deleteCache && !string.IsNullOrEmpty(localTokenDataCacheKey))
            {
                dataStorage.DeleteItem(localTokenDataCacheKey, null, TokenTableName);
            }

            if (!usePlayerPrefs) return;
            PlayerPrefs.DeleteKey(RefreshTokenPlayerPrefKey);
            PlayerPrefs.Save();
        }

        internal void DeleteCache(string key)
        {
            dataStorage.DeleteItem(key, null, TokenTableName);
        }

        internal void LoadAuthTrustId(Action<bool, string> onDone)
        {
            dataStorage.GetItem(AuthTrustIdKey, onDone, TokenTableName);
        }
        
        internal void SaveAuthTrustId(string authTrustId, Action<bool> onDone)
        {
            if (!string.IsNullOrEmpty(authTrustId))
            {
                var keyValueSaveInfo = new List<Tuple<string, object>>()
                {
                    new Tuple<string, object>(AuthTrustIdKey, authTrustId)
                };
                dataStorage.SaveItems(keyValueSaveInfo, onDone, TokenTableName);
            }
            else
            {
                onDone?.Invoke(false);
            }
        }
        
        internal void SaveAuthTrustId(Action<bool> onDone)
        {
            SaveAuthTrustId(tokenData?.auth_trust_id, onDone);
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
