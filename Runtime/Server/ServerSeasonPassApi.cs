// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerSeasonPassApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param> // TODO: Should this base BaseUrl?
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerSeasonPassApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
        }

        public IEnumerator GrantExpToUser(string userId
            , int exp
            , ResultCallback<UserSeasonInfoWithoutReward> callback
            , SeasonPassSource source = SeasonPassSource.SWEAT
            , string[] tags = null)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Grant Exp! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Grant Exp! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Grant Exp! UserId parameter is null!");

            GrantExpRequest grantExpRequest = new GrantExpRequest
            {
                exp = exp,
                source = source,
                tags = tags,
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/current/exp")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(grantExpRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfoWithoutReward>();
            callback.Try(result);
        }

        public IEnumerator GrantTierToUser(string userId
            , int count
            , ResultCallback<UserSeasonInfoWithoutReward> callback
            , SeasonPassSource source = SeasonPassSource.SWEAT
            , string[] tags = null)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't check user progression! UserId parameter is null!");

            GrantTierRequest grantTierRequest = new GrantTierRequest
            {
                count = count,
                source = source,
                tags = tags,
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/current/tiers")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(grantTierRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfoWithoutReward>();
            callback.Try(result);
        }

        public IEnumerator GetCurrentUserSeasonHistory(string userId
            , string seasonId
            , ResultCallback<UserSeasonExpHistory> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't check user progression! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/exp/history")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("seasonId", seasonId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonExpHistory>();
            callback.Try(result);
        }

        public IEnumerator GetUserSeasonData(string userId
            , string seasonId
            , ResultCallback<UserSeasonInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Get User ID! userId parameter is null!");
            Assert.IsNotNull(seasonId, "Can't Get User Season! seasonId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/{seasonId}/data")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("seasonId", seasonId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfo>();
            callback.Try(result);
        }

        public IEnumerator QueryUserSeasonExp(string userId
            , string seasonId
            , ResultCallback<QueryUserSeasonExp> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't check user progression! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/exp/history/tags")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("seasonId", seasonId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<QueryUserSeasonExp>();
            callback.Try(result);
        }

        public IEnumerator GetCurrentUserSeasonProgression( string userId
            ,  ResultCallback<UserSeasonInfoWithoutReward> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't check user progression! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/current/progression")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfoWithoutReward>();
            callback.Try(result);
        }

        public IEnumerator BulkGetUserSessionProgression(BulkGetUserSessionProgressionRequest requestModel
            , ResultCallback<UserSeasonInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't check user progression! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't check user progression! AccessToken parameter is null!");
            Assert.IsNotNull(requestModel.UserIds, "Can't check user progression! UserIds parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/seasonpass/admin/namespaces/{namespace}/seasons/current/users/bulk/progression")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfo[]>();
            callback.Try(result);
        }
    }
}
