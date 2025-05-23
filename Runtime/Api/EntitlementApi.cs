﻿// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class EntitlementApi : ApiBase
    {

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal EntitlementApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator QueryUserEntitlements(string userId
            , string entitlementName
            , string itemId
            , string[] features
            , int offset
            , int limit
            , EntitlementClazz entitlementClazz
            , EntitlementAppType entitlementAppType
            , ResultCallback<EntitlementPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var httpBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (offset >= 0)
            {
                httpBuilder.WithQueryParam("offset", offset.ToString());
            }
            else
            {
                httpBuilder.WithQueryParam("offset", "0");
            }

            if (limit >= 0)
            {
                httpBuilder.WithQueryParam("limit", limit.ToString());
            }
            else
            {
                httpBuilder.WithQueryParam("limit", "20");
            }

            if (features != null && features.Length > 0)
            {
                httpBuilder.WithQueryParam("features", features);
            }

            if (entitlementClazz != EntitlementClazz.NONE)
            {
                httpBuilder.WithQueryParam("entitlementClazz", entitlementClazz.ToString());
            }

            if (entitlementAppType != EntitlementAppType.NONE)
            {
                httpBuilder.WithQueryParam("entitlementAppType", entitlementAppType.ToString());
            }

            if (!string.IsNullOrEmpty(entitlementName))
            {
                httpBuilder.WithQueryParam("entitlementName", entitlementName);
            }

            if (!string.IsNullOrEmpty(itemId))
            {
                httpBuilder.WithQueryParam("itemId", itemId);
            }

            IHttpRequest request = httpBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, result =>
            {
                response = result;
            });

            var result = response.TryParseJson<EntitlementPagingSlicedResult>();
            callback?.Try(result);
        }

        internal void QueryUserSubscription(string userId, PlatformStoreId platformStoreId, ResultCallback<SubscriptionPagingSlicedResult> callback, QueryUserSubscriptionRequestOptionalParameters optionalParameters)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, platformStoreId.GetStorePlatformName(), userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            HttpRequestBuilder builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/subscriptions/platforms/{platform}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("platform", platformStoreId.GetStorePlatformName().ToUpper())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (optionalParameters.Limit >= 0)
                {
                    builder.WithQueryParam("limit", optionalParameters.Limit.ToString());
                }
                
                builder.WithQueryParam("offset", optionalParameters.Offset.ToString());
                
                if (optionalParameters.ActiveOnly != null)
                {
                    builder.WithQueryParam("activeOnly", optionalParameters.ActiveOnly.ToString().ToLower());
                }
                
                if (!string.IsNullOrEmpty(optionalParameters.ProductId))
                {
                    builder.WithQueryParam("productId", optionalParameters.ProductId);
                }

                if (!string.IsNullOrEmpty(optionalParameters.GroupId))
                {
                    builder.WithQueryParam("groupId", optionalParameters.GroupId);
                }
            }
            
            IHttpRequest request = builder.GetResult();
            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<SubscriptionPagingSlicedResult>();
                callback?.Try(result);
            });
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
        }

        public void GetUserEntitlementHistory(string userId
            , GetUserEntitlementHistoryOptionalParams optionalParams
            , ResultCallback<UserEntitlementHistoryResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var httpBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/history")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId);

            if (optionalParams != null)
            {
                if (optionalParams.EntitlementClazz != EntitlementClazz.NONE)
                {
                    httpBuilder.WithQueryParam("entitlementClazz", optionalParams.EntitlementClazz.ToString());
                }
                if (optionalParams.StartDate != null)
                {
                    httpBuilder.WithQueryParam("startDate"
                        , ((DateTime)optionalParams.StartDate).ToString("yyyy-MM-ddThh:mm:ssZ"));
                }
                if (optionalParams.EndDate != null)
                {
                    httpBuilder.WithQueryParam("endDate"
                        , ((DateTime)optionalParams.EndDate).ToString("yyyy-MM-ddThh:mm:ssZ"));
                }
                if (optionalParams.Offset > 0)
                {
                    httpBuilder.WithQueryParam("offset", optionalParams.Offset.ToString());
                }
                if (optionalParams.Limit > 0)
                {
                    httpBuilder.WithQueryParam("limit", optionalParams.Limit.ToString());
                }
            }

            var request = httpBuilder.GetResult();

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParams?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<UserEntitlementHistoryResponse>();
                callback?.Try(result);
            });
        }

        public IEnumerator ConsumeUserEntitlement(string userId
            , string entitlementId
            , int useCount
            , ResultCallback<EntitlementInfo> callback
            , string[] options
            , string requestId)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't consume user entitlement! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't consume user entitlement! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't consume user entitlement! userId parameter is null!");
            Assert.IsNotNull(entitlementId, "Can't consume user entitlement! entitlementId parameter is null!");

            ConsumeUserEntitlementRequest consumeUserEntitlement = new ConsumeUserEntitlementRequest
            {
                useCount = useCount,
                options = options,
                requestId = requestId
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
        }


        [Obsolete("Please access the api from Api.GetEntitlement().SyncMobilePlatformPurchaseGoogle")]
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
            callback?.Try(result);
        }

        internal void SyncMobilePlatformPurchaseGoogle(string orderId
            , string packageName
            , string productId
            , long purchaseTime
            , string purchaseToken
            , bool autoAck
            , bool subscriptionPurchase
            , PlatformSyncMobileGoogleOptionalParameters optionalParameters
            , ResultCallback<GoogleReceiptResolveResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(orderId
                , packageName
                , productId
                , purchaseTime
                , purchaseToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var syncRequest = new PlatformSyncMobileGoogle(
                orderId
                , packageName
                , productId
                , purchaseTime
                , purchaseToken
                , autoAck
                , subscriptionPurchase
                , optionalParameters);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/google/receipt")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<GoogleReceiptResolveResult>();
                callback?.Try(result);
            });
        }

        internal void SyncMobilePlatformPurchaseApple(string productId
            , string transactionId
            , string receiptData
            , PlatformSyncMobileAppleOptionalParam optionalParam
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(productId, transactionId, receiptData, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }
            
            PlatformSyncMobileApple syncRequest = new PlatformSyncMobileApple(productId: productId
                , transactionId: transactionId
                , receiptData: receiptData
                , optionalParam: optionalParam);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/apple/receipt")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParam?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }
        
        internal void SyncMobilePlatformPurchaseAppleV2(string transactionId
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(transactionId, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            PlatformSyncMobileAppleV2 syncRequest = new PlatformSyncMobileAppleV2()
            {
                TransactionId = transactionId
            };
                
            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/iap/apple/receipt")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
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
                content = string.Format("{{\"xstsToken\": \"{0}\"}}", XBoxDLCSync.xstsToken);
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
            callback?.Try(result);
        }

        public IEnumerator SyncSteamDLC( string userId
            , string userSteamId
            , string userAppId
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't sync DLC item! Namespace_ from parent  is null!");
            Assert.IsNotNull(AuthToken, "Can't sync DLC item! AccessToken from parent is null!");
            Assert.IsNotNull(userId, "Can't sync DLC item! UserId parameter is null!");
            
            var body = new Dictionary<string, string>();
            body.Add("steamId", userSteamId);
            body.Add("appId", userAppId);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/steam/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(body.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        internal void SyncSteamInventory(string userSteamId
            , string appId
            , string productId
            , double price
            , string currencyCode
            , SyncSteamInventoryOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(userSteamId
                , appId
                , productId
                , price
                , currencyCode);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var syncRequest = new SyncSteamInventoryRequest(userSteamId
                , appId
                , productId
                , price
                , currencyCode
                , optionalParameters);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/steam/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(syncRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
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
                .WithBody(playStationDLCSync.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
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
            callback?.Try(result);
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
            callback?.Try(result);
        }
        
        public IEnumerator SyncPSNDLCMultipleService(string userId 
            , PlayStationDLCSyncMultipleService playStationDlcSyncMultipleService
            , ResultCallback callback )
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/psn/sync/multiServiceLabels")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(playStationDlcSyncMultipleService.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator ValidateUserItemPurchaseCondition(ValidateUserItemPurchaseConditionRequest requestModel
           , ResultCallback<PlatformValidateUserItemPurchaseResponse[]> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, requestModel, requestModel?.ItemIds);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/items/purchase/conditions/validate")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestModel.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PlatformValidateUserItemPurchaseResponse[]>();
            callback?.Try(result);
        }

        public IEnumerator GetUserEntitlementOwnershipByItemIds(string userId
            ,string[] ids
            , ResultCallback<EntitlementOwnershipItemIds[]> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, ids);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/ownership/byItemIds")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
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
            callback?.Try(result);
        }

        public IEnumerator SyncEntitlementPSNStore(string userId
            , PlayStationDLCSync psnModel
            , ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, "Can't validate user item purchase condition! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't validate user item purchase condition! AccessToken from parent is null!");

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/psn/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(psnModel.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator SyncXboxInventory(string userId
           , XboxInventoryRequest model
           , ResultCallback<XboxInventoryResponse[]> callback)
        {
            Assert.IsNotNull(Namespace_, "Can't validate user item purchase condition! Namespace_ from parent is null!");
            Assert.IsNotNull(AuthToken, "Can't validate user item purchase condition! AccessToken from parent is null!");

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/xbl/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(model.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp => 
            {
                response = rsp; 
            });

            var result = response.TryParseJson<XboxInventoryResponse[]>();
            callback?.Try(result);
        }

        public IEnumerator SyncEntitlementPSNMultipleService(string userId
            , PlayStationMultipleServiceRequest playStationMultipleServiceRequest
            , ResultCallback<PlayStationMultipleServiceResponse[]> callback)
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/psn/sync/multiServiceLabels")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(playStationMultipleServiceRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<PlayStationMultipleServiceResponse[]>();
            callback?.Try(result);
        }
        
        public IEnumerator SellUserEntitlement(UserEntitlementSoldParams userEntitlementSoldParams
            , EntitlementSoldRequest entitlementSoldRequest
            , ResultCallback<SellItemEntitlementInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , userEntitlementSoldParams
                , userEntitlementSoldParams?.UserId
                , userEntitlementSoldParams?.EntitlementId
                , entitlementSoldRequest
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            EntitlementSoldRequest entitlementSoldRequestBody = new EntitlementSoldRequest
            {
                UseCount = entitlementSoldRequest.UseCount,
                RequestId = string.IsNullOrEmpty(entitlementSoldRequest.RequestId) ? null : entitlementSoldRequest.RequestId
            };
            
            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/entitlements/{entitlementId}/sell")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userEntitlementSoldParams.UserId)
                .WithPathParam("entitlementId", userEntitlementSoldParams.EntitlementId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(entitlementSoldRequestBody.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SellItemEntitlementInfo>();
            callback?.Try(result);
        }

        public IEnumerator SyncOculusConsumableEntitlements(string userId
            , ResultCallback<SyncOculusConsumableEntitlementResponse[]> callback)
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/oculus/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SyncOculusConsumableEntitlementResponse[]>();
            callback?.Try(result);
        }

        public IEnumerator SyncOculusDLC(string userId
            , ResultCallback callback)
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/dlc/oculus/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();
            callback?.Try(result);
        }

        public IEnumerator SyncEpicGameInventory(string userId
            , string epicGamesJwtToken
            , ResultCallback<SyncEpicGamesInventoryResponse[]> callback)
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(AuthToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(epicGamesJwtToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(epicGamesJwtToken) + " cannot be null or empty"));
                yield break;
            }

            var bodyRequest = new { epicGamesJwtToken = epicGamesJwtToken };
            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/iap/epicgames/sync")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(bodyRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SyncEpicGamesInventoryResponse[]>();
            callback?.Try(result);
        }

        public void GetDlcDurableRewardSimpleMap(string platformType, ResultCallback<DlcConfigRewardShortInfo> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, platformType);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/dlc/rewards/durable/map")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("dlcType", platformType.ToUpper())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<DlcConfigRewardShortInfo>();
                callback?.Try(result);
            });
        }

        internal void GetCurrentConfigFromServer(ResultCallback<CurrentAppleConfigVersion> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/iap/apple/config/version")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson);
            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response => 
            {
                var result = response.TryParseJson<CurrentAppleConfigVersion>();
                callback?.Try(result);
            });
        }
    }
}