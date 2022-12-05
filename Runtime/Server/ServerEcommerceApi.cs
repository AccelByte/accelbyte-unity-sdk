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
    internal class ServerEcommerceApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        internal ServerEcommerceApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator GetUserEntitlementById( string entitlementId
            , ResultCallback<EntitlementInfo> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(entitlementId, nameof(entitlementId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/admin/namespaces/{namespace}/entitlements/{entitlementId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("entitlementId", entitlementId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();

            callback.Try(result);
        }

        public IEnumerator GrantUserEntitlement( string userId
            , GrantUserEntitlementRequest[] grantUserEntitlementsRequest
            , ResultCallback<StackableEntitlementInfo[]> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(grantUserEntitlementsRequest, nameof(grantUserEntitlementsRequest) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/admin/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(grantUserEntitlementsRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StackableEntitlementInfo[]>();

            callback.Try(result);
        }
        
        [Obsolete("This does not support for multiplatform wallet, use CreditUserWalletV2 instead")] 
        public IEnumerator CreditUserWallet( string userId
            , string currencyCode
            , CreditUserWalletRequest creditUserWalletRequest
            , ResultCallback<WalletInfo> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(currencyCode, nameof(currencyCode) + " cannot be null");
            Assert.IsNotNull(creditUserWalletRequest, nameof(creditUserWalletRequest) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(creditUserWalletRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<WalletInfo>();

            callback.Try(result);
        }

        public IEnumerator CreditUserWalletV2( string userId
            , string currencyCode
            , CreditUserWalletRequest creditUserWalletRequest
            , ResultCallback<CreditUserWalletResponse> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(currencyCode, nameof(currencyCode) + " cannot be null");
            Assert.IsNotNull(creditUserWalletRequest, nameof(creditUserWalletRequest) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/admin/namespaces/{namespace}/users/{userId}/wallets/{currencyCode}/credit")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("currencyCode", currencyCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(creditUserWalletRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CreditUserWalletResponse>();

            callback.Try(result);
        }

        public IEnumerator FulfillUserItem( string userId
            , FulfillmentRequest fulfillmentRequest
            , ResultCallback<FulfillmentResult> callback )
        {
            Assert.IsNotNull(Namespace_, $"{nameof(Namespace_)} cannot be null");
            Assert.IsNotNull(userId, $"{nameof(userId)} cannot be null");
            Assert.IsNotNull(AuthToken, $"{nameof(AuthToken)} cannot be null");
            Assert.IsNotNull(fulfillmentRequest, $"{nameof(fulfillmentRequest)} cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/admin/namespaces/{namespace}/users/{userId}/fulfillment")
                .WithPathParam("namespace",Namespace_)
                .WithPathParam("userId",userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(fulfillmentRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<FulfillmentResult>();

            callback.Try(result);
        }
        
        public IEnumerator GetStoreList(ResultCallback<PlatformStore[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/admin/namespaces/{namespace}/stores")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PlatformStore[]>();
            callback.Try(result);
        }
        
        public IEnumerator QueryItemsByCriteria( ItemCriteriaV2 criteria
            , ResultCallback<ItemPagingSlicedResultV2> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get items by criteria! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get items by criteria! AccessToken parameter is null!");
            Assert.IsNotNull(criteria, "Can't get items by criteria! Criteria parameter is null!");
            
            string availableDate = criteria.availableDate.ToUniversalTime().ToString("u").Replace(" ", "T");
            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/admin/namespaces/{namespace}/items/byCriteria")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("storeId", (criteria.storeId != null)? criteria.storeId : string.Empty)
                .WithQueryParam("categoryPath",(criteria.categoryPath != null)? criteria.categoryPath : string.Empty)
                .WithQueryParam("includeSubCategoryItem", (criteria.includeSubCategoryItem) ? "true" : "false")
                .WithQueryParam("itemType", (criteria.itemType != ItemType.NONE) ? criteria.itemType.ToString() : string.Empty)
                .WithQueryParam("appType", (criteria.appType != EntitlementAppType.NONE)? criteria.appType.ToString() : string.Empty)
                .WithQueryParam("baseAppId", (criteria.baseAppId != null)?criteria.baseAppId : string.Empty)
                .WithQueryParam("tags", (criteria.tags != null)? string.Join(",",criteria.tags) : string.Empty)
                .WithQueryParam("features", (criteria.features != null)? string.Join(",",criteria.features) : string.Empty)
                .WithQueryParam("region", (criteria.region != null)? criteria.region : string.Empty)
                .WithQueryParam("availableDate", (availableDate != string.Empty)? availableDate : string.Empty)
                .WithQueryParam("targetNamespace", (criteria.targetNamespace != null)? criteria.targetNamespace : string.Empty)
                .WithQueryParam("offset", (criteria.offset >= 0) ? criteria.offset.ToString() : string.Empty)
                .WithQueryParam("limit", (criteria.limit >= 0)? criteria.limit.ToString() : string.Empty)
                .WithQueryParam("sortBy", (criteria.sortBy != null)? criteria.sortBy : string.Empty)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);
            var result = response.TryParseJson<ItemPagingSlicedResultV2>();
            callback.Try(result);
        }
    }
}
