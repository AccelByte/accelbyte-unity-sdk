// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class EntitlementApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        internal EntitlementApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator QueryUserEntitlements( string userId
            , string entitlementName
            , string itemId
            , int offset
            , int limit
            , EntitlementClazz entitlementClazz 
            , EntitlementAppType entitlementAppType 
            , ResultCallback<EntitlementPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user entitlements! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("entitlementClazz", 
                    (entitlementClazz == EntitlementClazz.NONE) ? "" : entitlementClazz.ToString())
                .WithQueryParam("entitlementAppType", 
                    (entitlementAppType == EntitlementAppType.NONE) ? "" : entitlementAppType.ToString())
                .WithQueryParam("entitlementName", entitlementName)
                .WithQueryParam("itemId", itemId)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit >= 0)? limit.ToString() : "")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<EntitlementPagingSlicedResult>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementById( string userId
            , string entitlementId
            , ResultCallback<EntitlementInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user entitlements! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(entitlementId, "Can't get user entitlements! entitlementId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
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

        /// <summary>
        /// </summary>
        /// <param name="publisherNamespace">Not to be confused with Namespace_</param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator GetUserEntitlementOwnershipByAppId( string publisherNamespace
            , string userId
            , string appId
            , ResultCallback<Ownership> callback )
        {
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! publisherNamespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(appId, "Can't get user entitlements! appId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/byAppId")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("appId", appId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="publisherNamespace">Not to be confused with Namespace_</param>
        /// <param name="userId"></param>
        /// <param name="sku"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator GetUserEntitlementOwnershipBySku( string publisherNamespace
            , string userId
            , string sku
            , ResultCallback<Ownership> callback )
        {
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(sku, "Can't get user entitlements! sku parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/bySku")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("sku", sku)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipByItemId( string userId
            , string itemId 
            , ResultCallback<Ownership> callback )
        {
            Assert.IsNotNull(Namespace_, "Can't get user entitlements! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't get user entitlements! UserId parameter is null!");
            Assert.IsNotNull(itemId, "Can't get user entitlements! itemId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/byItemId")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("itemId", itemId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="publisherNamespace">Different than Config's Namespace</param>
        /// <param name="userId"></param>
        /// <param name="itemIds"></param>
        /// <param name="appIds"></param>
        /// <param name="skus"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator GetUserEntitlementOwnershipAny( string publisherNamespace
            , string userId
            , string[] itemIds
            , string[] appIds
            , string[] skus
            , ResultCallback<Ownership> callback )
        {
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! publisherNamespace parameter is null!");
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, 
                "Can't get user entitlements! all itemIds, appIds and skus parameter are null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownership/any")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
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

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Ownership>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipToken( string publisherNamespace
            , string[] itemIds
            , string[] appIds
            , string[] skus
            , ResultCallback<OwnershipToken> callback )
        {
            Assert.IsNotNull(AuthToken, "Can't get user entitlements! AccessToken from parent is null!");
            Assert.IsNotNull(publisherNamespace, "Can't get user entitlements! publisherNamespace parameter is null!");
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, 
                "Can't get user entitlements! all itemIds, appIds and skus parameter are null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/me/entitlements/ownershipToken")
                .WithPathParam("namespace", publisherNamespace)
                .WithBearerAuth(AuthToken)
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

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<OwnershipToken>();
            callback.Try(result);
        }

        public IEnumerator ConsumeUserEntitlement(string userId
            , string entitlementId
            , int useCount
            , ResultCallback<EntitlementInfo> callback
            , string[] options )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't consume user entitlement! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't consume user entitlement! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't consume user entitlement! userId parameter is null!");
            Assert.IsNotNull(entitlementId, "Can't consume user entitlement! entitlementId parameter is null!");

            ConsumeUserEntitlementRequest consumeUserEntitlement = new ConsumeUserEntitlementRequest
            {
                useCount = useCount,
                options = options

            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}/decrement")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("entitlementId", entitlementId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(consumeUserEntitlement.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<EntitlementInfo>();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator CreateDistributionReceiver( string userId
            , string extUserId
            , Attributes currentAttributes
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't create distribution receiver! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't create distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't create distribution receiver! extUserId parameter is null!");
            Assert.IsNotNull(currentAttributes, "Can't create distribution receiver! distributionAttributes parameter is null!");

            DistributionAttributes distributionAttributes = new DistributionAttributes
            {
                attributes = currentAttributes,
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(distributionAttributes.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator DeleteDistributionReceiver( string userId
            , string extUserId
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't delete distribution receiver! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't delete distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't delete distribution receiver! extUserId parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        /// <summary>
        /// Different than Config's Namespace
        /// </summary>
        /// <param name="publisherNamespace"></param>
        /// <param name="publisherUserId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator GetDistributionReceiver( string publisherNamespace
            , string publisherUserId
            , ResultCallback<DistributionReceiver[]> callback )
        {
            Assert.IsNotNull(publisherNamespace, "Can't get distribution receiver! PublisherNamespace_ from parent  is null!");
            Assert.IsNotNull(publisherUserId, "Can't get distribution receiver! PublisherUserId parameter is null!");
            Assert.IsNotNull(Namespace_, "Can't get distribution receiver! @Namespace_ from parent  is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers")
                .WithPathParam("namespace", publisherNamespace)
                .WithPathParam("userId", publisherUserId)
                .WithQueryParam("targetNamespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<DistributionReceiver[]>();
            callback.Try(result);
        }

        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.)")]
        public IEnumerator UpdateDistributionReceiver( string userId
            , string extUserId
            , Attributes currentAttributes
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't update distribution receiver! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");
            Assert.IsNotNull(extUserId, "Can't update distribution receiver! extUserId parameter is null!");
            Assert.IsNotNull(currentAttributes, "Can't update distribution receiver! distributionAttributes parameter is null!");

            DistributionAttributes distributionAttributes = new DistributionAttributes
            {
                attributes = currentAttributes
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/receivers/{extUserId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("extUserId", extUserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(distributionAttributes.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncMobilePlatformPurchaseGoogle( string userId
            , PlatformSyncMobileGoogle syncRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't update distribution receiver! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/google/receipt")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncMobilePlatformPurchaseApple( string userId
            , PlatformSyncMobileApple syncRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't update distribution receiver! Namespace_ from parent  is null!");
            Assert.IsNotNull(userId, "Can't update distribution receiver! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/apple/receipt")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncXBoxDLC( string userId
            , XBoxDLCSync XBoxDLCSync
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't sync DLC item! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't sync DLC item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");

            string content = "{}";

            if (!string.IsNullOrEmpty(XBoxDLCSync.xstsToken))
            {
                content = string.Format("{\"xstsToken\": \"{0}\"}", XBoxDLCSync.xstsToken);
            }
            
            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/xbl/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(content)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncSteamDLC( string userId
            , string userSteamId
            , string userAppId
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't sync DLC item! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't sync DLC item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/steam/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{\"steamId\": \"{0}\", \"appId\": \"{1}\"}", userSteamId, userAppId))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncPSNDLC( string userId 
            , PlayStationDLCSync playStationDLCSync
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't sync DLC item! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't sync DLC item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/psn/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{\"serviceLabel\": \"{0}\"}", playStationDLCSync.serviceLabel))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncTwitchDropItem( string userId
            , TwitchDropSync TwitchDropSyncReq
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't sync Twitch drop item! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't sync Twitch drop item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync Twitch drop item! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/twitch/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(TwitchDropSyncReq.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SyncEpicGamesDurableItems(string userId
            , string epicGamesJwtToken
            , ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, "Can't sync EpicGames durable item! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't sync EpicGames durable item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync EpicGames durable item! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/epicgames/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(epicGamesJwtToken.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ValidateUserItemPurchaseCondition(string[] items
           , ResultCallback<PlatformValidateUserItemPurchaseResponse[]> callback)
        {
            Assert.IsNotNull(Namespace_, "Can't validate user item purchase condition! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't validate user item purchase condition! AccessToken from parent is null!");
            Assert.IsNotNull(items, "Can't validate user item purchase condition! items parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/items/purchase/conditions/validate")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(new{itemIds = items}.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PlatformValidateUserItemPurchaseResponse[]>();
            callback.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipByItemIds(string userId
            ,string[] ids
            , ResultCallback<EntitlementOwnershipItemIds[]> callback)
        {
            Assert.IsNotNull(Namespace_, "Can't validate user item purchase condition! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't validate user item purchase condition! AccessToken from parent is null!");
            Assert.IsNotNull(ids, "Can't Get ids condition! ids parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/ownership/byItemIds")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson); 
                if (ids != null) 
                { foreach (var id in ids) 
                    { 
                        builder.WithQueryParam("ids", id); 
                    } 
                }
                var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<EntitlementOwnershipItemIds[]>();
            callback.Try(result);
        }
    }
}
