// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class EntitlementApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal EntitlementApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator QueryUserEntitlements(string @namespace, string userId, string userAccessToken, string entitlementName, string itemId, int offset,
            int limit, EntitlementClazz entitlementClazz, EntitlementAppType entitlementAppType, ResultCallback<EntitlementPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("entitlementClazz", (entitlementClazz == EntitlementClazz.NONE) ? "" : entitlementClazz.ToString())
                .WithQueryParam("entitlementAppType", (entitlementAppType == EntitlementAppType.NONE) ? "" : entitlementAppType.ToString())
                .WithQueryParam("entitlementName", entitlementName)
                .WithQueryParam("itemId", itemId)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit >= 0)? limit.ToString() : "")
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementPagingSlicedResult>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementById(string @namespace, string userId, string userAccessToken, string entitlementId,
            ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsNotNull(entitlementId, "Can't get user entitlements! entitlementId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("entitlementId", entitlementId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();
            callback.Try(result);
        }

        public IEnumerator ConsumeUserEntitlement(string @namespace, string userId, string userAccessToken, string entitlementId, int useCount,
            ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't consume user entitlement! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't consume user entitlement! userId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't consume user entitlement! userAccessToken parameter is null!");
            Assert.IsNotNull(entitlementId, "Can't consume user entitlement! entitlementId parameter is null!");

            ConsumeUserEntitlementRequest consumeUserEntitlement = new ConsumeUserEntitlementRequest
            {
                useCount = useCount
            };

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}/decrement")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("entitlementId", entitlementId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(consumeUserEntitlement.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();
            callback.Try(result);
        }
    }
}
