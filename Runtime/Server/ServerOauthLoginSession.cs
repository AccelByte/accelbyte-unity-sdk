﻿// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace AccelByte.Server
{
    public class ServerOauthLoginSession : ISession
    {
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly IHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;

        private readonly string baseUrl;

        private Coroutine maintainAccessTokenCoroutine;
        private TokenData tokenData;
        private DateTime nextRefreshTime;
        private string clientToken;
        private DateTime clientTokenExpiryTime;

        internal ServerOauthLoginSession( string inBaseUrl
            , string inClientId
            , string inClientSecret
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inBaseUrl, $"Creating {GetType().Name} failed. Parameter inBaseUrl is null");
            Assert.IsNotNull(inClientId, "Creating " + GetType().Name + " failed. inClientId parameter is null!");
            Assert.IsNotNull(inClientSecret, "Creating " + GetType().Name + " failed. inClientSecret parameter is null!");
            Assert.IsNotNull(inHttpClient, "Creating " + GetType().Name + " failed. Parameter inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "Creating " + GetType().Name + " failed. Parameter inCoroutineRunner is null" );

            baseUrl = inBaseUrl;
            clientId = inClientId;
            clientSecret = inClientSecret;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
        }

        public string AuthorizationToken 
        { 
            get { return tokenData != null ? tokenData.access_token : null; }
            set { tokenData.access_token = value; }
        }

        public IEnumerator LoginWithClientCredentials( ResultCallback callback )
        {
            Result<TokenData> getClientTokenResult = null;

            yield return GetClientToken(r => getClientTokenResult = r);

            tokenData = getClientTokenResult.Value;

            if (!getClientTokenResult.IsError)
            {
                if (maintainAccessTokenCoroutine == null)
                {
                    maintainAccessTokenCoroutine = coroutineRunner.Run(MaintainAccessToken());
                }

                callback.TryOk();
            }
            else
            {
                callback.TryError(getClientTokenResult.Error);
            }
        }

        private IEnumerator GetClientToken( ResultCallback<TokenData> callback )
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        public IEnumerator Logout( ResultCallback callback )
        {
            var request = HttpRequestBuilder.CreatePost(baseUrl + "/v3/oauth/revoke/token")
                .WithBearerAuth(AuthorizationToken)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", AuthorizationToken)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, 
                rsp => response = rsp);

            tokenData = null;
            var result = response.TryParse();
            coroutineRunner.Stop(maintainAccessTokenCoroutine);
            maintainAccessTokenCoroutine = null;
            callback.Try(result);
        }

        private IEnumerator MaintainAccessToken()
        {
            TimeSpan refreshBackoff = TimeSpan.FromSeconds(10);
            var rand = new Random();

            while (true)
            {
                if (refreshBackoff >= ServerOauthLoginSession.MaxBackoffInterval)
                {
                    yield break;
                }

                if (tokenData == null || DateTime.UtcNow < nextRefreshTime)
                {
                    yield return new WaitForSeconds(ServerOauthLoginSession.WaitExpiryDelay / 1000f);

                    continue;
                }

                Result refreshResult = null;

                yield return LoginWithClientCredentials(result => refreshResult = result);

                if (!refreshResult.IsError)
                {
                    nextRefreshTime = ServerOauthLoginSession.ScheduleNormalRefresh(tokenData.expires_in);
                }
                else
                {
                    refreshBackoff = ServerOauthLoginSession.CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

                    nextRefreshTime = DateTime.UtcNow + refreshBackoff;
                }
            }
        }

        private static DateTime ScheduleNormalRefresh( int expiresIn )
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds((expiresIn + 1) * 0.8);
        }

        private static TimeSpan CalculateBackoffInterval( TimeSpan previousRefreshBackoff
            , int randomNum )
        {
            previousRefreshBackoff = TimeSpan.FromSeconds(previousRefreshBackoff.Seconds * 2);

            return previousRefreshBackoff + TimeSpan.FromSeconds(randomNum);
        }
    }
}
