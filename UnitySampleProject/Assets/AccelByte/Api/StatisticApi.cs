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

        public IEnumerator GetAllStatItems(string @namespace, string userId, string profileId, string accessToken,
            ResultCallback<StatItemPagingSlicedResult> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get all stat items! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all stat items! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get all stat items! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get all stat items! profileId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(
                    this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemPagingSlicedResult>();

            callback.Try(result);
        }

        public IEnumerator GetStatItemsByStatCodes(string @namespace, string userId, string profileId,
            string accessToken, ICollection<string> statCodes, ResultCallback<StatItemInfo[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get stat items! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get stat items! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get stat items! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get stat items! profileId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(
                    this.baseUrl +
                    "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems/byStatCodes")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithQueryParam("statCodes", statCodes)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StatItemInfo[]>();

            callback.Try(result);
        }

        public IEnumerator BulkAddStatItemValue(string @namespace, BulkUserStatItemInc[] data, string accessToken, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/statitems/bulk/inc")
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

        public IEnumerator BulkAddUserStatItemValue(string @namespace, string userId, string profileId, BulkStatItemInc[] data, string accessToken, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");
            Assert.IsNotNull(profileId, "Can't add stat item value! profileId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems/bulk/inc")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
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

        public IEnumerator AddUserStatItemValue(string @namespace, string userId, string profileId, string statCode, float inc, string accessToken, ResultCallback<StatItemIncResult> callback)
        {
            Assert.IsNotNull(@namespace, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");
            Assert.IsNotNull(profileId, "Can't add stat item value! profileId parameter is null!");
            Assert.IsNotNull(statCode, "Can't add stat item value! statCode parameter is null!");

            string jsonInfo = string.Format(
                "{{" +
                "\"inc\": {0}" +
                "}}",
                inc);

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/stats/{statCode}/statitems/inc")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
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