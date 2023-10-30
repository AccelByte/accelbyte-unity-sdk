// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;
using Random = System.Random;

namespace AccelByte.Core
{
    public abstract class ISession
    {
        //CONST
        const uint MaxWaitTokenRefresh = 60000;
        const uint WaitExpiryDelay = 100;
        readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);


        protected CoroutineRunner coroutineRunner { get; set; }
        protected TokenData tokenData { get; set; }
        protected Coroutine maintainAccessTokenCoroutine { get; set; }
        protected DateTime nextRefreshTime { get; set; }
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

        bool twoFAEnable { get; set; } = false; //Set false for DSSession

        // To be set by OAuth and allow UserSession call the Refresh API call
        internal System.Action<string, ResultCallback<TokenData, OAuthError>> CallRefresh;

        //Token Related Functions

        /// <summary>
        /// Purpose: refresh the token
        /// * UserSession : ISession should implement RefreshSession /iam/v3/oauth/token grant_type:refresh_token
        /// * ServerSession : ISession should implement LoginWithClientCredentials
        /// </summary>
        /// <returns></returns>
        public IEnumerator RefreshSession(ResultCallback<TokenData, OAuthError> callback)
        {
            yield return RefreshSessionApiCall(result =>
            {
                if (!result.IsError && result.Value != null)
                {
                    SetSession(result.Value);
                }
                callback.Invoke(result);
            });
        }

        public abstract IEnumerator RefreshSessionApiCall(ResultCallback<TokenData, OAuthError> callback);

        //UserSession please implement this
        public abstract void SetSession(TokenData loginResponse);

        public void ForceSetTokenData(TokenData inTokenData)
        {
            tokenData = inTokenData;
        }

        protected IEnumerator MaintainToken()
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

                if (twoFAEnable)
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
                    Result<TokenData, OAuthError> refreshResult = null;

                    yield return RefreshSession(result =>
                    {
                        refreshResult = result;
                    });

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
        
        //set public to allow override for testing purpose
        protected virtual DateTime ScheduleNormalRefresh(int expiresIn)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn - 1) * 0.8);
        }

        TimeSpan CalculateBackoffInterval(TimeSpan previousRefreshBackoff, int randomNum)
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);
            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
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