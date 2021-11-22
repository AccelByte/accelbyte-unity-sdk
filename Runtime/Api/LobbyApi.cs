// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
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
        private readonly IHttpClient httpClient;

        internal LobbyApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PartyDataUpdateNotif>();

            callback.Try(result);
        }

        public IEnumerator WritePartyStorage(string @namespace, string accessToken, PartyDataUpdateRequest data,
            string partyId, ResultCallback<PartyDataUpdateNotif> callback, Action callbackOnConflictedData = null)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(partyId, nameof(partyId) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/lobby/v1/public/party/namespaces/{namespace}/parties/{partyId}/attributes")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(accessToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PartyDataUpdateNotif>();

            if (result.IsError && (result.Error.Code == ErrorCode.PreconditionFailed || result.Error.Code == ErrorCode.PartyStorageOutdatedUpdateData))
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BlockerList>();

            callback.Try(result);
        }
        
        public IEnumerator BulkGetUserPresence(string @namespace, ICollection<string> userIds, string accessToken, ResultCallback<BulkUserStatusNotif> callback, bool countOnly = false)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/lobby/v1/public/presence/namespaces/{namespace}/users/presence")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .WithQueryParam("countOnly", countOnly ? "true" : "false")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BulkUserStatusNotif>();

            callback.Try(result);
        }
    
        public void OnBanNotificationReceived(string accessToken, Action<string> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            ((AccelByteHttpClient)this.httpClient).OnBearerAuthRejected(callback);
        }
    }
}