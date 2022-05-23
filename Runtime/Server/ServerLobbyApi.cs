// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerLobbyApi : ServerApiBase
    {        
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==LobbyServerUrl</param>
        /// <param name="session"></param>
        internal ServerLobbyApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.LobbyServerUrl, session )
        {
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
                .CreatePut(BaseUrl + "/v1/admin/party/namespaces/{namespace}/parties/{partyId}/attributes")
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

            if (result.IsError && (result.Error.Code == ErrorCode.PreconditionFailed || result.Error.Code == ErrorCode.PartyStorageOutdatedUpdateData))
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }
        
        public IEnumerator GetPartyStorage( string partyID
            , ResultCallback<PartyDataUpdateNotif> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(partyID, nameof(partyID) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/party/namespaces/{namespace}/parties/{partyId}")
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

        public IEnumerator GetPartyDataByUserId( string userId
            , ResultCallback<PartyDataUpdateNotif> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(Namespace_), "namespace cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "accessToken cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "userId cannot be null or empty");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/party/namespaces/{namespace}/users/{userId}/party")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
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

        public IEnumerator GetSessionAttribute( string userId
            , string key
            , ResultCallback<ServerGetSessionAttributeResponse> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(Namespace_), "namespace cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "accessToken cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "userId cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(key), "key cannot be null or empty");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/player/namespaces/{namespace}/users/{userId}/attributes/{attribute}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("attribute", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ServerGetSessionAttributeResponse>();

            callback.Try(result);
        }

        public IEnumerator GetSessionAttributeAll( string userId
            , ResultCallback<GetSessionAttributeAllResponse> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(Namespace_), "namespace cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "accessToken cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "userId cannot be null or empty");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/player/namespaces/{namespace}/users/{userId}/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GetSessionAttributeAllResponse>();

            callback.Try(result);
        }

        public IEnumerator SetSessionAttribute( string userId
            , Dictionary<string, string> attributes
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(Namespace_), "namespace cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(AuthToken), "accessToken cannot be null or empty");
            Assert.IsFalse(string.IsNullOrEmpty(userId), "userId cannot be null or empty");
            Assert.IsFalse(attributes == null || attributes.Count == 0, "attributes cannot be null or empty.");

            ServerSetSessionAttributeRequest body = new ServerSetSessionAttributeRequest() { attributes = attributes };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/player/namespaces/{namespace}/users/{userId}/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(body.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }
    }
}