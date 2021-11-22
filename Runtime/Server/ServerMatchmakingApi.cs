using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using System;

namespace AccelByte.Server
{
    public class ServerMatchmakingApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        public  ServerMatchmakingApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator EnqueueJoinableSession(string @namespace, string accessToken, MatchmakingResult body, 
            ResultCallback callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "RegisterSession failed. accessToken parameter is null!");
            Assert.IsNotNull(body, "RegisterSession failed. body parameter is null");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/sessions")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(body.ToJsonString())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator DequeueJoinableSession(string @namespace, string accessToken, DequeueRequest body, 
            ResultCallback callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "DequeueJoinableSession failed. accessToken parameter is null!");
            Assert.IsNotNull(body, "DequeueJoinableSession failed. body parameter is null");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/sessions/dequeue")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(body.ToJsonString())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator QuerySessionStatus(string @namespace, string accessToken, string matchId, 
            ResultCallback<MatchmakingResult> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "RegisterSession failed. accessToken parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RegisterSession failed. accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/namespaces/{namespace}/sessions/{matchID}/status")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("matchID", matchId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<MatchmakingResult>();

            callback.Try(result);
        }

        public IEnumerator AddUserToSession(string @namespace, string accessToken, string channelName, string matchId, string userId, string partyId, ResultCallback callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(channelName), "RemoveUserFromSession failed, channelName is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "RemoveUserFromSession failed, userId is null or empty!");

            AddUserIntoSessionRequest body = new AddUserIntoSessionRequest()
            {
                user_id = userId,
                party_id = partyId
            };

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/admin/namespaces/{namespace}/channels/{channelName}/sessions/{matchId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("channelName", channelName)
                .WithPathParam("matchId", matchId)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse respose = null;

            yield return this.httpClient.SendRequest(request, rsp => respose = rsp);

            var result = respose.TryParse();

            callback.Try(result);
        }

        public IEnumerator RemoveUserFromSession(string @namespace, string accessToken, string channelName, string matchId, string userId, MatchmakingResult body, ResultCallback callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(matchId), "RemoveUserFromSession failed. accessToken parameter is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(channelName), "RemoveUserFromSession failed, channelName is null or empty!");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "RemoveUserFromSession failed, userId is null or empty!");

            var requestBuilder = HttpRequestBuilder.CreateDelete(this.baseUrl + "/v1/admin/namespaces/{namespace}/channels/{channelName}/sessions/{matchId}/users/{userId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("channelName", channelName)
                .WithPathParam("matchId", matchId)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (body != null)
                requestBuilder.WithBody(body.ToJsonString());

            var request = requestBuilder.GetResult();

            IHttpResponse respose = null;

            yield return this.httpClient.SendRequest(request, rsp => respose = rsp);

            var result = respose.TryParse();

            callback.Try(result);
        }
    }
}