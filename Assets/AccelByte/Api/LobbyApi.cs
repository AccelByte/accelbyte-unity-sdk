// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class LobbyApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal LobbyApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator BulkFriendRequest(string @namespace, string ownUserId, BulkFriendsRequest userIds, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(ownUserId, nameof(ownUserId) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/friends/namespaces/{namespace}/users/{userId}/add/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", ownUserId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(userIds.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }
        
        public IEnumerator GetPartyStorage(string @namespace, string accessToken, string partyID, ResultCallback<PartyDataUpdateNotif> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(partyID, nameof(partyID) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/lobby/v1/public/party/namespaces/{namespace}/parties/{partyId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("partyId", partyID)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PartyDataUpdateNotif>();

            callback.Try(result);
        }

        public IEnumerator GetListOfBlockedUser(string @namespace, string accessToken, string userId, ResultCallback<BlockedList> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            
            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BlockedList>();

            callback.Try(result);
        }

        public IEnumerator GetListOfBlocker(string @namespace, string accessToken, string userId, ResultCallback<BlockerList> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            
            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked-by")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BlockerList>();

            callback.Try(result);
        }
        
        public IEnumerator BulkGetUserPresence(string @namespace, ICollection<string> userIds, string accessToken, ResultCallback<BulkUserStatusNotif> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/lobby/v1/public/presence/namespaces/{namespace}/users/presence")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BulkUserStatusNotif>();

            callback.Try(result);
        }
    }
}