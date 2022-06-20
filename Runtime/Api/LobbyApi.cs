// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
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
    public class LobbyApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param>
        /// <param name="session"></param>
        internal LobbyApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
        }

        public IEnumerator BulkFriendRequest( string ownUserId
            , BulkFriendsRequest userIds
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(ownUserId, nameof(ownUserId) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/friends/namespaces/{namespace}/users/{userId}/add/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", ownUserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(userIds.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }
        
        public IEnumerator GetPartyStorage( string partyID
            , ResultCallback<PartyDataUpdateNotif> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(partyID, nameof(partyID) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/lobby/v1/public/party/namespaces/{namespace}/parties/{partyId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyID)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PartyDataUpdateNotif>();

            callback.Try(result);
        }

        public IEnumerator WritePartyStorage( PartyDataUpdateRequest data
            , string partyId
            , ResultCallback<PartyDataUpdateNotif> callback
            , Action callbackOnConflictedData = null )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(partyId, nameof(partyId) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/lobby/v1/public/party/namespaces/{namespace}/parties/{partyId}/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PartyDataUpdateNotif>();

            if (result.IsError && (result.Error.Code == ErrorCode.PreconditionFailed || 
                result.Error.Code == ErrorCode.PartyStorageOutdatedUpdateData))
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }

        public IEnumerator SetPartySizeLimit(string partyId, int limit, ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(partyId, nameof(partyId) + " cannot be null");
            Assert.IsTrue(limit > 0, nameof(limit) + " muts be above zero");

            SetPartySizeLimitRequest body = new SetPartySizeLimitRequest() { limit = limit };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/lobby/v1/public/party/namespaces/{namespace}/parties/{partyId}/limit")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(body.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator GetListOfBlockedUser( string userId
            , ResultCallback<BlockedList> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<BlockedList>();

            callback.Try(result);
        }

        public IEnumerator GetListOfBlocker( string userId
            , ResultCallback<BlockerList> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/lobby/v1/public/player/namespaces/{namespace}/users/me/blocked-by")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<BlockerList>();

            callback.Try(result);
        }
        
        public IEnumerator BulkGetUserPresence( ICollection<string> userIds
            , ResultCallback<BulkUserStatusNotif> callback
            , bool countOnly = false )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/lobby/v1/public/presence/namespaces/{namespace}/users/presence")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .WithQueryParam("countOnly", countOnly ? "true" : "false")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<BulkUserStatusNotif>();

            callback.Try(result);
        }
    
        public void OnBanNotificationReceived( Action<string> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            ((AccelByteHttpClient)HttpClient).OnBearerAuthRejected(callback);
        }

    }
}
