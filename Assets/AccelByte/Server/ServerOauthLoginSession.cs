// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
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
    public class ServerOauthLoginSession : IServerSession
    {
        private const uint MaxWaitTokenRefresh = 60000;
        private const uint WaitExpiryDelay = 100;
        private static readonly TimeSpan MaxBackoffInterval = TimeSpan.FromDays(1);

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly UnityHttpWorker httpWorker;
        private readonly CoroutineRunner coroutineRunner;

        private readonly string baseUrl;

        private Coroutine maintainAccessTokenCoroutine;
        private TokenData tokenData;
        private DateTime nextRefreshTime;
        private string clientToken;
        private DateTime clientTokenExpiryTime;

        internal ServerOauthLoginSession(string baseUrl, string clientId, string clientSecret, UnityHttpWorker httpWorker, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(clientId, "Creating " + GetType().Name + " failed. ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Creating " + GetType().Name + " failed. ClientSecret parameter is null!");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            Assert.IsNotNull(coroutineRunner, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.httpWorker = httpWorker;
            this.coroutineRunner = coroutineRunner;
        }

        public string AuthorizationToken { get { return this.tokenData != null ? this.tokenData.access_token : null; } }

        public IEnumerator LoginWithClientCredentials(ResultCallback callback)
        {
            Result<TokenData> getClientTokenResult = null;

            yield return GetClientToken(r => getClientTokenResult = r);

            this.tokenData = getClientTokenResult.Value;

            if (!getClientTokenResult.IsError)
            {
                if (this.maintainAccessTokenCoroutine == null)
                {
                    this.maintainAccessTokenCoroutine = this.coroutineRunner.Run(MaintainAccessToken());
                }

                callback.TryOk();
            }
            else
            {
                callback.TryError(getClientTokenResult.Error);
            }
        }

        private IEnumerator GetClientToken(ResultCallback<TokenData> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        public IEnumerator Logout(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v3/oauth/revoke/token")
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
            this.maintainAccessTokenCoroutine = null;
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

                if (this.tokenData == null || DateTime.UtcNow < this.nextRefreshTime)
                {
                    yield return new WaitForSeconds(ServerOauthLoginSession.WaitExpiryDelay / 1000f);

                    continue;
                }

                Result refreshResult = null;

                yield return LoginWithClientCredentials(result => refreshResult = result);

                if (!refreshResult.IsError)
                {
                    this.nextRefreshTime = ServerOauthLoginSession.ScheduleNormalRefresh(this.tokenData.expires_in);
                }
                else
                {
                    refreshBackoff = ServerOauthLoginSession.CalculateBackoffInterval(refreshBackoff, rand.Next(1, 60));

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
