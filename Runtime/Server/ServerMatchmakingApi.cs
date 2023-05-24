// Copyright (c) 20?? - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerMatchmakingApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==MatchmakingServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        public ServerMatchmakingApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.MatchmakingServerUrl, session )
        {
        }

        public IEnumerator EnqueueJoinableSession( MatchmakingResult body
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "RegisterSession failed. accessToken parameter is null!");
            Assert.IsNotNull(body, "RegisterSession failed. body parameter is null");

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/sessions")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(body.ToJsonString())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator DequeueJoinableSession(DequeueRequest body
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "DequeueJoinableSession failed. accessToken parameter is null!");
            Assert.IsNotNull(body, "DequeueJoinableSession failed. body parameter is null");

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/sessions/dequeue")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(body.ToJsonString())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator QuerySessionStatus( string matchId
            ,  ResultCallback<MatchmakingResult> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "RegisterSession failed. accessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RegisterSession failed. accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/namespaces/{namespace}/sessions/{matchID}/status")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("matchID", matchId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<MatchmakingResult>();

            callback.Try(result);
        }

        public IEnumerator AddUserToSession( string channelName
            , string matchId
            , string userId
            , string partyId
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(channelName), "RemoveUserFromSession failed, channelName is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "RemoveUserFromSession failed, userId is null or empty!");

            AddUserIntoSessionRequest body = new AddUserIntoSessionRequest()
            {
                user_id = userId,
                party_id = partyId
            };

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/channels/{channelName}/sessions/{matchId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("channelName", channelName)
                .WithPathParam("matchId", matchId)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse respose = null;

            yield return HttpClient.SendRequest(request, rsp => respose = rsp);

            var result = respose.TryParse();

            callback.Try(result);
        }

        public IEnumerator RemoveUserFromSession( string channelName
            , string matchId
            , string userId
            , MatchmakingResult body
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(channelName), "RemoveUserFromSession failed, channelName is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "RemoveUserFromSession failed, userId is null or empty!");

            var requestBuilder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/channels/{channelName}/sessions/{matchId}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("channelName", channelName)
                .WithPathParam("matchId", matchId)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (body != null)
                requestBuilder.WithBody(body.ToJsonString());

            var request = requestBuilder.GetResult();

            IHttpResponse respose = null;

            yield return HttpClient.SendRequest(request, rsp => respose = rsp);

            var result = respose.TryParse();

            callback.Try(result);
        }
    }
}