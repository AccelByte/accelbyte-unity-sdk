// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Net;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class EntitlementApi
    {
        private readonly string baseUrl;

        internal EntitlementApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct entitlements service! BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetUserEntitlements(string @namespace, string userId, string userAccessToken,
            int page, int size, ResultCallback<PagedEntitlements> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/platform/public/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("page", page.ToString())
                .WithQueryParam("size", size.ToString())
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PagedEntitlements> result = response.TryParseJsonBody<PagedEntitlements>();
            callback.Try(result);
        }
    }
}