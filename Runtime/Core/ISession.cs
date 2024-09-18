// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using UnityEngine.Assertions;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
    public abstract class ISession
    {
        //CONST
        const uint MaxWaitTokenRefresh = 60000;
        const uint WaitExpiryDelay = 100;
        readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        internal LoginSessionMaintainer SessionMaintainer
        {
            get;
            private set;
        }

        protected TokenData tokenData { get; set; }
        internal TokenData TokenData
        {
            get
            {
                return tokenData;
            }
        }

        protected DateTime nextRefreshTime { get; set; }
        protected Dictionary<string, ThirdPartyPlatformTokenData> thirdPartyPlatformTokenData { get; set; }
            = new Dictionary<string, ThirdPartyPlatformTokenData>();
        public abstract string AuthorizationToken { get; set; }
        public virtual string UserId
        {
            get
            {
                return null;
            }
        }
        
        public virtual string Namespace
        {
            get
            {
                string retval = null;
                if(tokenData != null)
                {
                    retval = tokenData.Namespace;
                }
                return retval;
            }
        }
        
        internal ApiSharedMemory SharedMemory;

        bool twoFAEnable { get; set; } = false; //Set false for DSSession

        // To be set by OAuth and allow UserSession call the Refresh API call
        internal System.Action<string, ResultCallback<TokenData, OAuthError>> CallRefresh;

        internal void CreateSessionMaintainer()
        {
            SessionMaintainer = new LoginSessionMaintainer();
            SessionMaintainer.RefreshSession = RefreshSessionApiCall;
        }

        //Token Related Functions

        /// <summary>
        /// Purpose: refresh the token
        /// * UserSession : ISession should implement RefreshSession /iam/v3/oauth/token grant_type:refresh_token
        /// * ServerSession : ISession should implement LoginWithClientCredentials
        /// </summary>
        /// <returns></returns>
        public IEnumerator RefreshSession(ResultCallback<TokenData, OAuthError> callback)
        {
            Result<TokenData, OAuthError> result = null; 
            bool refreshSessionCompleted = false;
            RefreshSessionApiCall()
                .OnSuccess((token) =>
                {
                    SetSession(token);
                    result = Result<TokenData, OAuthError>.CreateOk(token);
                })
                .OnFailed((error) =>
                {
                    result = Result<TokenData, OAuthError>.CreateError(error);
                })
                .OnComplete(() =>
                {
                    refreshSessionCompleted = true;
                });

            yield return new WaitUntil(() => refreshSessionCompleted);

            callback?.Invoke(result);
        }

        public abstract IEnumerator RefreshSessionApiCall(ResultCallback<TokenData, OAuthError> callback);
        public abstract AccelByteResult<TokenData, OAuthError> RefreshSessionApiCall();

        //UserSession please implement this
        public abstract void SetSession(TokenData loginResponse);

        public void ForceSetTokenData(TokenData inTokenData)
        {
            tokenData = inTokenData;
        }

        public Dictionary<string, ThirdPartyPlatformTokenData> GetThirdPartyPlatformTokenData()
        {
            return thirdPartyPlatformTokenData;
        }

        public void ClearThirdPartyPlatformTokenData()
        {
            thirdPartyPlatformTokenData.Clear();
        }

        //set public to allow override for testing purpose
        protected virtual DateTime ScheduleNormalRefresh(int expiresIn)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn - 1) * 0.8);
        }

        internal void SetThirdPartyPlatformTokenData(string key, ThirdPartyPlatformTokenData inThirdPartyPlatformTokenData)
        {
            thirdPartyPlatformTokenData[key] = inThirdPartyPlatformTokenData;
        }
        
        internal void SetSharedMemory(ApiSharedMemory sharedMemory)
        {
            this.SharedMemory = sharedMemory;
        }
    }

    public static class SessionExtension
    {
        public static bool IsValid(this ISession session )
        {
            return !string.IsNullOrEmpty(session.AuthorizationToken);
        }

        public static void AssertValid(this ISession session )
        {
            Assert.IsNotNull(session);
            Assert.IsFalse(string.IsNullOrEmpty(session.AuthorizationToken));
        }
    }
}