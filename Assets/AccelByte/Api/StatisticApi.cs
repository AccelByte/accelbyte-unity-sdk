// Copyright (c) 2019-2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class StatisticApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal StatisticApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator CreateUserStatItems(string @namespace, string userId, string accessToken,
            CreateStatItemRequest[] statItems, ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(statItems, nameof(statItems) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(statItems.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator GetUserStatItems(string @namespace, string userId,
            string accessToken, ICollection<string> statCodes, ICollection<string> tags, ResultCallback<PagedStatItems> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get stat items! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get stat items! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get stat items! accessToken parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(
                    this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", @namespace)
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedStatItems>();

            callback.Try(result);
        }

        public IEnumerator IncrementUserStatItems(string @namespace, string userId, StatItemIncrement[] increments, string accessToken, ResultCallback<StatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(increments.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ResetUserStatItems(string @namespace, string userId, StatItemReset[] resets, string accessToken, ResultCallback<StatItemOperationResult[]> callback) 
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(resets.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserStatItems(string @namespace, string userId, string additionalKey, StatItemUpdate[] updates, string accessToken, ResultCallback<StatItemOperationResult[]> callback) 
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }
    }
}