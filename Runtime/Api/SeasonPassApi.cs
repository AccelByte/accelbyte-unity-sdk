// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class SeasonPassApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==SeasonPassServerUrl</param>
        /// <param name="session"></param>
        internal SeasonPassApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.SeasonPassServerUrl, session )
        {
        }

        public IEnumerator GetCurrentSeason( string language
            , ResultCallback<SeasonInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Current Season! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Get Current Season! AccessToken parameter is null!");
            Assert.IsNotNull(language, "Can't Get Current Season! language parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/seasons/current")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("language", language)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserSeason( string userId
            , string seasonId
            , ResultCallback<UserSeasonInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get User Season! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Get User Season! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Get User Season! userId parameter is null!");
            Assert.IsNotNull(seasonId, "Can't Get User Season! seasonId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/{seasonId}/data")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<UserSeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator GetCurrentUserSeason( string userId
            , ResultCallback<UserSeasonInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get User Current Season! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Get User Current Season! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Get User Current Season! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/data")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<UserSeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator ClaimRewards( string userId
            , SeasonClaimRewardRequest rewardRequest
            , ResultCallback<SeasonClaimRewardResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Claim Rewards! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Claim Rewards! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Claim Rewards! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/rewards")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(rewardRequest.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonClaimRewardResponse>();
            callback.Try(result);
        }

        public IEnumerator BulkClaimRewards( string userId
            , ResultCallback<SeasonClaimRewardResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Claim Rewards! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Claim Rewards! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Claim Rewards! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/seasons/current/rewards/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SeasonClaimRewardResponse>();
            callback.Try(result);
        }

    }
}


