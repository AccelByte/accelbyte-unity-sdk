// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerStatisticApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==StatisticServerUrl</param>
        /// <param name="session"></param>
        internal ServerStatisticApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.StatisticServerUrl, session)
        {
        }

        public IEnumerator CreateUserStatItems(string userId
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(statItems, nameof(statItems) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(statItems.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator GetUserStatItems(string userId
            , ICollection<string> statCodes
            , ICollection<string> tags
            , ResultCallback<PagedStatItems> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (statCodes != null && statCodes.Count > 0)
            {
                builder.WithQueryParam("statCodes", string.Join(",", statCodes));
            }

            if (tags != null && tags.Count > 0)
            {
                builder.WithQueryParam("tags", string.Join(",", tags));
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PagedStatItems>();

            callback.Try(result);
        }

        public IEnumerator IncrementUserStatItems(string userId
            , StatItemIncrement[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator IncrementManyUsersStatItems(UserStatItemIncrement[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/admin/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ResetUserStatItems(string userId
            , StatItemReset[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ResetManyUsersStatItems(UserStatItemReset[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserStatItems(string userId
            , string additionalKey
            , StatItemUpdate[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator UpdateManyUsersStatItems( UserStatItemUpdate[] data
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator BulkFetchUserStatItemValues(string statCode
           , string[] userIds
           , string additionalKey
           , ResultCallback<FetchUser[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk/getOrDefault")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("statCode", statCode)
                .WithQueryParam("additionalKey", additionalKey)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            foreach (var userId in userIds)
            {
                builder.WithQueryParam("userIds", userId);
            }
            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<FetchUser[]>();

            callback.Try(result);
        }

        public IEnumerator BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(bulkUpdateMultipleUserStatItem, nameof(bulkUpdateMultipleUserStatItem) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
            .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithContentType(MediaType.ApplicationJson)
            .WithBody(bulkUpdateMultipleUserStatItem.ToUtf8Json())
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();

            callback.Try(result);
        }

        public IEnumerator BulkResetUserStatItemsValues(string userId
            , string additionalKey
            , UserStatItem[] bulkUserStatItems
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(bulkUserStatItems, nameof(bulkUserStatItems) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
            .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithQueryParam("additionalKey", additionalKey)
            .WithContentType(MediaType.ApplicationJson)
            .WithBody(bulkUserStatItems.ToUtf8Json())
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();

            callback.Try(result);
        }

        public IEnumerator BulkUpdateUserStatItemValue(string userId
           , string additionalKey
           , UpdateUserStatItemWithStatCode[] BulkUpdateUserStatItem
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(BulkUpdateUserStatItem, nameof(BulkUpdateUserStatItem) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
            .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithQueryParam("additionalKey", additionalKey)
            .WithContentType(MediaType.ApplicationJson)
            .WithBody(BulkUpdateUserStatItem.ToUtf8Json())
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();

            callback.Try(result);
        }


        public IEnumerator UpdateUserStatItemValue(string userId, string statCode
           , string additionalKey
           , UpdateUserStatItem UpdateUserStatItemValue
           , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
            .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithPathParam("statCode", statCode)
            .WithQueryParam("additionalKey", additionalKey)
            .WithBody(UpdateUserStatItemValue.ToUtf8Json())
            .WithContentType(MediaType.ApplicationJson)
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemValueResponse>();

            callback.Try(result);
        }

        public IEnumerator DeleteUserStatItems(string userId, string statCode
           , string additionalKey
           , ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            var request = HttpRequestBuilder
            .CreateDelete(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithPathParam("statCode", statCode)
            .WithQueryParam("additionalKey", additionalKey)
            .WithContentType(MediaType.ApplicationJson)
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
    }        
}