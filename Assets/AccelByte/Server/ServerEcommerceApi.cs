// Copyright (c) 2020 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerEcommerceApi
    {
        private string baseUrl;
        private IHttpClient httpClient;

        internal ServerEcommerceApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GetUserEntitlementById(string @namespace, string accessToken, string entitlementId,
            ResultCallback<EntitlementInfo> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(entitlementId, nameof(entitlementId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/admin/namespaces/{namespace}/entitlements/{entitlementId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("entitlementId", entitlementId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();

            callback.Try(result);
        }

        public IEnumerator GrantUserEntitlement(string @namespace, string userId, string accessToken,
            GrantUserEntitlementRequest[] grantUserEntitlementsRequest, ResultCallback<StackableEntitlementInfo[]> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(grantUserEntitlementsRequest, nameof(grantUserEntitlementsRequest) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/admin/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(grantUserEntitlementsRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<StackableEntitlementInfo[]>();

            callback.Try(result);
        }

        public IEnumerator CreditUserWallet(string @namespace, string userId, string accessToken, string currencyCode,
            CreditUserWalletRequest creditUserWalletRequest, ResultCallback<WalletInfo> callback)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(currencyCode, nameof(currencyCode) + " cannot be null");
            Assert.IsNotNull(creditUserWalletRequest, nameof(creditUserWalletRequest) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(creditUserWalletRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<WalletInfo>();

            callback.Try(result);
        }

        public IEnumerator FulfillUserItem(string @namespace, string userId,string accessToken,
            FulfillmentRequest fulfillmentRequest,ResultCallback<FulfillmentResult> callback)
        {
            Assert.IsNotNull(@namespace, $"{nameof(@namespace)} cannot be null");
            Assert.IsNotNull(userId, $"{nameof(userId)} cannot be null");
            Assert.IsNotNull(accessToken, $"{nameof(accessToken)} cannot be null");
            Assert.IsNotNull(fulfillmentRequest, $"{nameof(fulfillmentRequest)} cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/admin/namespaces/{namespace}/users/{userId}/fulfillment")
                .WithPathParam("namespace",@namespace)
                .WithPathParam("userId",userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(fulfillmentRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<FulfillmentResult>();

            callback.Try(result);
        }
    }
}
