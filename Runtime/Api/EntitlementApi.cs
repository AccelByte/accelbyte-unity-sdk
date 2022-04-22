// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using System;

namespace AccelByte.Api
{
    internal class EntitlementApi
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        #endregion

        #region Constructor

        internal EntitlementApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Public Methods

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipByAppId(string publisherNamespace, string userId, string userAccessToken, string appId, 
            ResultCallback<Ownership> callback)
        {
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsNotNull(appId, "Can't get user entitlements! appId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/byAppId")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("appId", appId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipBySku(string publisherNamespace, string userId, string userAccessToken, string sku, 
            ResultCallback<Ownership> callback)
        {
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsNotNull(sku, "Can't get user entitlements! sku parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/bySku")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("sku", sku)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipByItemId(string Namespace, string userId, string userAccessToken, string itemId, 
            ResultCallback<Ownership> callback)
        {
            Assert.IsNotNull(Namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsNotNull(itemId, "Can't get user entitlements! itemId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/byItemId")
                .WithPathParam("namespace", Namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("itemId", itemId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipAny(string publisherNamespace, string userId, string userAccessToken,
            string[] itemIds, string[] appIds, string[] skus, ResultCallback<Ownership> callback)
        {
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't get user entitlements! all itemIds, appIds and skus parameter are null");

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/any")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (itemIds != null)
                builder.WithQueryParam("itemIds", itemIds);
            if (appIds != null)
                builder.WithQueryParam("appIds", appIds);
            if (skus != null)
                builder.WithQueryParam("skus", skus);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipToken(string Namespace, string userAccessToken,
            string[] itemIds, string[] appIds, string[] skus, ResultCallback<OwnershipToken> callback)
        {
            Assert.IsNotNull(Namespace, "Can't get user entitlements! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user entitlements! UserAccessToken parameter is null!");
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't get user entitlements! all itemIds, appIds and skus parameter are null");

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownershipToken")
                .WithPathParam("namespace", Namespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (itemIds != null)
                builder.WithQueryParam("itemIds", itemIds);
            if (appIds != null)
                builder.WithQueryParam("appIds", appIds);
            if (skus != null)
                builder.WithQueryParam("skus", skus);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<OwnershipToken>();
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

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator CreateDistributionReceiver(string @namespace, string userId, string userAccessToken, string extUserId,
            Attributes currentAttributes, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't create distribution receiver! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't create distribution receiver! extUserId parameter is null!");
            Assert.IsNotNull(currentAttributes, "Can't create distribution receiver! distributionAttributes parameter is null!");

            DistributionAttributes distributionAttributes = new DistributionAttributes
            {
                attributes = currentAttributes
            };

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(distributionAttributes.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator DeleteDistributionReceiver(string @namespace, string userId, string userAccessToken, string extUserId,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't delete distribution receiver! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't delete distribution receiver! extUserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator GetDistributionReceiver(string publisherNamespace, string publisherUserId, string targetNamespace, string userAccessToken,
            ResultCallback<DistributionReceiver[]> callback)
        {
            Assert.IsNotNull(publisherNamespace, "Can't get distribution receiver! PublisherNamespace parameter is null!");
            Assert.IsNotNull(publisherUserId, "Can't get distribution receiver! PublisherUserId parameter is null!");
            Assert.IsNotNull(targetNamespace, "Can't get distribution receiver! TargetNamespace parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", publisherUserId)
                .WithQueryParam("targetNamespace", targetNamespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<DistributionReceiver[]>();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator UpdateDistributionReceiver(string @namespace, string userId, string userAccessToken, string extUserId,
            Attributes currentAttributes, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't update distribution receiver! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't update distribution receiver! extUserId parameter is null!");
            Assert.IsNotNull(currentAttributes, "Can't update distribution receiver! distributionAttributes parameter is null!");

            DistributionAttributes distributionAttributes = new DistributionAttributes
            {
                attributes = currentAttributes
            };

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(distributionAttributes.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncMobilePlatformPurchaseGoogle(string @namespace, string userId, string userAccessToken, PlatformSyncMobileGoogle syncRequest, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't update distribution receiver! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/google/receipt")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncMobilePlatformPurchaseApple(string @namespace, string userId, string userAccessToken, PlatformSyncMobileApple syncRequest, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't update distribution receiver! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/apple/receipt")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncXBoxDLC(string @namespace, string userId, string userAccessToken, XBoxDLCSync XBoxDLCSync, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't sync DLC item! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't sync DLC item! userAccessToken parameter is null!");

            string content = "{}";

            if (!string.IsNullOrEmpty(XBoxDLCSync.xstsToken))
            {
                content = string.Format("{\"xstsToken\": \"{0}\"}", XBoxDLCSync.xstsToken);
            }
            
            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/xbl/sync")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(content)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncSteamDLC(string @namespace, string userId, string userAccessToken,
            string userSteamId, string userAppId, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't sync DLC item! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't sync DLC item! userAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/steam/sync")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{\"steamId\": \"{0}\", \"appId\": \"{1}\"}", userSteamId, userAppId))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncPSNDLC(string @namespace, string userId, string userAccessToken, PlayStationDLCSync playStationDLCSync,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't sync DLC item! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't sync DLC item! userAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/psn/sync")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{\"serviceLabel\": \"{0}\"}", playStationDLCSync.serviceLabel))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncTwitchDropItem(string @namespace, string userId, string userAccessToken, TwitchDropSync TwitchDropSyncReq, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't sync Twitch drop item! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't sync Twitch drop item! UserId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't sync Twitch drop item! userAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/twitch/sync")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(TwitchDropSyncReq.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion
    }
}
