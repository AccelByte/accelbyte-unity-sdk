// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class SeasonPassApi
    {
        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly ISession session;
        private readonly IHttpClient httpClient;

        internal SeasonPassApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GetCurrentSeason(string @namespace, string accessToken, string language, ResultCallback<SeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get Current Season! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get Current Season! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't Get Current Season! language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/seasons/current")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("language", language)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserSeason(string @namespace, string accessToken, string userId, string seasonId, ResultCallback<UserSeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get User Season! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get User Season! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Get User Season! userId parameter is null!");
            Assert.IsNotNull(seasonId, "Can't Get User Season! seasonId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/{seasonId}/data")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<UserSeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator GetCurrentUserSeason(string @namespace, string accessToken, string userId, ResultCallback<UserSeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get User Current Season! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get User Current Season! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Get User Current Season! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/data")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<UserSeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator ClaimRewards(string @namespace, string accessToken, string userId, SeasonClaimRewardRequest rewardRequest,
            ResultCallback<SeasonClaimRewardResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Claim Rewards! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Claim Rewards! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Claim Rewards! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/rewards")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(rewardRequest.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonClaimRewardResponse>();
            callback.Try(result);
        }

        public IEnumerator BulkClaimRewards(string @namespace, string accessToken, string userId, ResultCallback<SeasonClaimRewardResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Claim Rewards! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Claim Rewards! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Claim Rewards! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/rewards/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonClaimRewardResponse>();
            callback.Try(result);
        }
    }
}


