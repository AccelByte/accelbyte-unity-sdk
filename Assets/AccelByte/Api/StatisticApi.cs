// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
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
        private readonly IHttpWorker httpWorker;

        internal StatisticApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetUserStatItems(string @namespace, string userId,
            string accessToken, ICollection<string> statCodes, ICollection<string> tags, ResultCallback<StatItemPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
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

            if (statCodes != null)
            {
                builder.WithQueryParam("statCodes", statCodes);
            }

            if (tags != null)
            {
                builder.WithQueryParam("tags", tags);
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemPagingSlicedResult>();

            callback.Try(result);
        }

        public IEnumerator BulkAddStatItemValue(string @namespace, BulkUserStatItemInc[] data, string accessToken, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson < BulkStatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator BulkAddUserStatItemValue(string @namespace, string userId, BulkStatItemInc[] data, string accessToken, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<BulkStatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator AddUserStatItemValue(string @namespace, string userId, string statCode, float inc, string accessToken, ResultCallback<StatItemIncResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");
            Assert.IsNotNull(statCode, "Can't add stat item value! statCode parameter is null!");

            string jsonInfo = string.Format(
                "{{" +
                "\"inc\": {0}" +
                "}}",
                inc);

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(jsonInfo)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemIncResult>();

            callback.Try(result);
        }
    }
}