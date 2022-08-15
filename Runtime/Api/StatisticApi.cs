// Copyright (c) 2019-2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class StatisticApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==StatisticServerUrl</param>
        /// <param name="session"></param>
        internal StatisticApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.StatisticServerUrl, session )
        {
        }

        public IEnumerator CreateUserStatItems( string userId
            , string accessToken
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(statItems, nameof(statItems) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
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

        public IEnumerator GetUserStatItems( string userId
            , string accessToken
            , ICollection<string> statCodes
            , ICollection<string> tags
            , ResultCallback<PagedStatItems> callback )
        {
            Assert.IsNotNull(Namespace_, "Can't get stat items! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get stat items! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get stat items! accessToken parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(
                    BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
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

        public IEnumerator IncrementUserStatItems( string userId
            , StatItemIncrement[] increments
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(increments.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ResetUserStatItems( string userId
            , StatItemReset[] resets
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(resets.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserStatItems( string userId
            , string additionalKey
            , StatItemUpdate[] updates
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ListUserStatItems(string userId
            , string[] statCodes
            , string[] tags
            , string additionalKey
            , string accessToken
            , ResultCallback<FetchUser[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(statCodes, nameof(statCodes) + " cannot be null");
            Assert.IsNotNull(tags, nameof(tags) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            foreach (var statCode in statCodes)
            {
                builder.WithQueryParam("statCodes", statCode);
            }
            foreach (var tag in tags)
            {
                builder.WithQueryParam("tags", tag);
            }
            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<FetchUser[]>();

            callback.Try(result);
        } 

        public IEnumerator UpdateUserStatItemsValue(string userId
            , string statCode 
            , string additionalKey
            , PublicUpdateUserStatItem updateUserStatItem
            , string accessToken
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(updateUserStatItem, nameof(updateUserStatItem) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("statCode", statCode)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserStatItem.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemValueResponse>();

            callback.Try(result);
        }
    }
}
