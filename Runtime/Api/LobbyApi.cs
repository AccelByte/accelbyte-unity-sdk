// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
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
        [UnityEngine.Scripting.Preserve]
        internal LobbyApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
        }

        /// <summary>
        /// Need to be accessed by the Lobby (WrapperBase) since it has no access to the Config.
        /// </summary>
        public Config GetConfig()
        {
            return this.Config;
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

            callback?.Try(result);
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

            callback?.Try(result);
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
                callback?.Try(result);
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

            callback?.Try(result);
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

            callback?.Try(result);
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

            callback?.Try(result);
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

            callback?.Try(result);
        }

        public IEnumerator SyncThirdPartyFriends(SyncThirdPartyFriendsRequest syncRequest
            , ResultCallback<SyncThirdPartyFriendsResponse[]> callback)
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(ErrorCode.NamespaceNotFound);
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(ErrorCode.Unauthorized);
                yield break;
            }

            // The request should contain at least one friend sync detail
            if (syncRequest.FriendSyncDetails.Length < 1)
            {
                callback?.TryError(ErrorCode.BadRequest);
                yield break;
            }

            foreach (SyncThirdPartyFriendInfo syncThirdPartyFriendInfo in syncRequest.FriendSyncDetails)
            {
                // The request should not contain empty platform id and should not contain empty platform token if the user is not logged in
                if (string.IsNullOrEmpty(syncThirdPartyFriendInfo.PlatformId) ||
                    (string.IsNullOrEmpty(syncThirdPartyFriendInfo.PlatformToken) && !syncThirdPartyFriendInfo.IsLogin))
                {
                    callback?.TryError(ErrorCode.BadRequest);
                    yield break;
                }
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/friends/sync/namespaces/{namespace}/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(syncRequest.FriendSyncDetails.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SyncThirdPartyFriendsResponse[]>();

            callback?.Try(result);
        }

        public void GetNotifications(ResultCallback<GetUserNotificationsResponse> callback
            , DateTime startTime = default
            , DateTime endTime = default
            , int offset = 0
            , int limit = 25)
        {
            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/notification/namespaces/{namespace}/me")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_);

            DateTime unixEpoc = new DateTime(year: 1970, 1, 1);
            if (startTime != default)
            {
                builder.WithQueryParam("startTime", ((long)startTime.Subtract(unixEpoc).TotalSeconds).ToString());
            }

            if (endTime != default)
            {
                builder.WithQueryParam("endTime", ((long)endTime.Subtract(unixEpoc).TotalSeconds).ToString());
            }

            if (offset > 0)
            {
                builder.WithQueryParam("offset", offset.ToString());
            }

            if (limit > 20)
            {
                builder.WithQueryParam("limit", limit.ToString());
            }

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetUserNotificationsResponse>();
                
                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.TryOk(result.Value);
            });
        }

        public void OnBanNotificationReceived( Action<string> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            ((AccelByteHttpClient)HttpClient).OnBearerAuthRejected(callback);
        }
    }
}
