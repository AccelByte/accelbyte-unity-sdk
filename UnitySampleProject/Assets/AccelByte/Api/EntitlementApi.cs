// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
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

        public IEnumerator GetUserEntitlements(string @namespace, string userId, string userAccessToken, int offset,
            int limit, ResultCallback<PagedEntitlements> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedEntitlements>();
            callback.Try(result);
        }
    }
}