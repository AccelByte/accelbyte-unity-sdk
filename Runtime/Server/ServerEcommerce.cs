// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerEcommerce : WrapperBase
    {
        private readonly ServerEcommerceApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        private PredefinedEventScheduler predefinedEventScheduler;

        private enum PredefinedEventType
        {
            Wallet,
            WalletV2,
            EntitlementGrant,
            Fulfilment
        }

        [UnityEngine.Scripting.Preserve]
        internal ServerEcommerce( ServerEcommerceApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct Ecommerce manager; api is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct Ecommerce manager; coroutineRunner is null!");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        #region Predefined Event Analytics

        /// <summary>
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventScheduler">Predefined event scheduler object reference</param>
        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;
        }

        private void SendPredefinedEntitlementEvent(Result<StackableEntitlementInfo[]> apiCallResult)
        {
            IAccelByteTelemetryPayload payload;
            payload = CreatePredefinedPayload<StackableEntitlementInfo[]>(apiCallResult, PredefinedEventType.EntitlementGrant);
            SendPredefinedEvent(payload);
        }

        private IAccelByteTelemetryPayload CreatePredefinedPayload<T>(Result<T> apiCallResult, PredefinedEventType eventType)
        {
            switch (eventType)
            {
                case PredefinedEventType.Wallet:
                    {
                        var walletInfo = apiCallResult as Result<WalletInfo>;
                        var payload = new PredefinedWalletCreditedPayload(
                            walletInfo.Value.userId,
                            walletInfo.Value.currencyCode,
                            walletInfo.Value.currencySymbol,
                            walletInfo.Value.balance,
                            walletInfo.Value.balanceOrigin,
                            walletInfo.Value.totalPermanentBalance,
                            walletInfo.Value.totalTimeLimitedBalance);
                        return payload;
                    }
                case PredefinedEventType.WalletV2:
                    {
                        var walletInfo = apiCallResult as Result<CreditUserWalletResponse>;
                        var payload = new PredefinedWalletCreditedPayload(
                            walletInfo.Value.userId,
                            walletInfo.Value.currencyCode,
                            walletInfo.Value.currencySymbol,
                            walletInfo.Value.balance,
                            walletInfo.Value.balanceOrigin,
                            walletInfo.Value.totalPermanentBalance,
                            walletInfo.Value.totalTimeLimitedBalance);
                        return payload;
                    }
                case PredefinedEventType.EntitlementGrant:
                    {
                        var entitlementInfos = apiCallResult as Result<StackableEntitlementInfo[]>;
                        List<PredefinedEntitlements> list = new List<PredefinedEntitlements>();

                        foreach (var result in entitlementInfos.Value)
                        {
                            PredefinedEntitlements entitlement = new PredefinedEntitlements(
                                result.itemId,
                                result.itemNamespace,
                                result.StoreId,
                                result.grantedCode,
                                result.source.ToString());

                            list.Add(entitlement);
                        }
                        var payload = new PredefinedEntitlementGrantedPayload(list);
                        return payload;
                    }
                case PredefinedEventType.Fulfilment:
                    {
                        var fulfilmentResult = apiCallResult as Result<FulfillmentResult>;
                        string userId = fulfilmentResult.Value.userId;

                        List<PredefinedEntitlementSummary> entitlements = new List<PredefinedEntitlementSummary>();
                        if (fulfilmentResult.Value.entitlementSummaries != null)
                        {
                            foreach (var summary in fulfilmentResult.Value.entitlementSummaries)
                            {
                                PredefinedEntitlementSummary entSummary = new PredefinedEntitlementSummary(
                                    summary.id,
                                    summary.Name,
                                    summary.type.ToString(),
                                    summary.clazz.ToString(),
                                    summary.itemId,
                                    summary.StoreId.ToString());

                                entitlements.Add(entSummary);
                            }
                        }

                        List<PredefinedCreditSummary> credits = new List<PredefinedCreditSummary>();
                        if (fulfilmentResult.Value.creditSummaries != null)
                        {
                            foreach (var summary in fulfilmentResult.Value.creditSummaries)
                            {
                                PredefinedCreditSummary credsSummary = new PredefinedCreditSummary(
                                    summary.walletId,
                                    summary.userId,
                                    summary.amount,
                                    summary.CurrencyCode);

                                credits.Add(credsSummary);
                            }
                        }

                        List<PredefinedSubscriptionSummary> subscriptions = new List<PredefinedSubscriptionSummary>();
                        if (fulfilmentResult.Value.subscriptionSummaries != null)
                        {
                            foreach (var summary in fulfilmentResult.Value.subscriptionSummaries)
                            {
                                PredefinedSubscriptionSummary subsSummary = new PredefinedSubscriptionSummary(
                                    summary.Id,
                                    summary.ItemId,
                                    summary.UserId,
                                    summary.Sku,
                                    summary.Status.ToString(),
                                    summary.SubscribedBy.ToString());

                                subscriptions.Add(subsSummary);
                            }
                        }

                        var payload = new PredefinedItemFulfilledPayload(userId, entitlements, credits, subscriptions);
                        return payload;
                    }
                default:
                    return null;
            }
        }

        private void SendPredefinedUserWalletEvent(Result<WalletInfo> apiCallResult)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload<WalletInfo>(apiCallResult, PredefinedEventType.Wallet);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedUserWalletEventV2(Result<CreditUserWalletResponse> apiCallResult)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload<CreditUserWalletResponse>(apiCallResult, PredefinedEventType.WalletV2);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            if (payload != null)
            {
                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
        }

        private void SendFulfillUserItemPredefinedEvent(Result<FulfillmentResult> apiCall)
        {
            IAccelByteTelemetryPayload payload;
            payload = CreatePredefinedPayload<FulfillmentResult>(apiCall, PredefinedEventType.Fulfilment);
            SendPredefinedEvent(payload);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }
            callback.Try(result);
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete( "namespace param is deprecated (now passed to Api from Config): Use the overload without it" ), UnityEngine.Scripting.Preserve]
        internal ServerEcommerce
            ( ServerEcommerceApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Get a user's entitlement by the entitlementId.
        /// </summary>
        /// <param name="entitlementId">The id of the entitlement</param>
        /// <param name="callback">Returns all StackableEntitlements Info via callback when completed</param>
        public void GetUserEntitlementById( string entitlementId
            , ResultCallback<EntitlementInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(entitlementId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetEntitlementIdInvalidMessage(entitlementId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementById(entitlementId, callback));
        }

        /// <summary>
        /// Granting Entitlement(s) to a user.
        /// </summary>
        /// <param name="publisherNamespace">Different than Config Namespace</param>
        /// <param name="userId">UserId of a user</param>
        /// <param name="grantUserEntitlementsRequest"> Consist of the entitlement(s) that will be granted</param>
        /// <param name="callback">Returns all StackableEntitlements Info via callback when completed</param>
        public void GrantUserEntitlement( string userId
            , GrantUserEntitlementRequest[] grantUserEntitlementsRequest
            , ResultCallback<StackableEntitlementInfo[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GrantUserEntitlement(
                    userId,
                    grantUserEntitlementsRequest,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEntitlementEvent(cb);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Credit a user wallet by currency code, if the wallet does not exist, it will create a new wallet.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="creditUserWalletRequest">The request to credit a user wallet</param>
        /// <param name="callback">Returns Wallet info via callback when completed</param>
        [Obsolete("This does not support for multiplatform wallet, use CreditUserWalletV2 instead")]
        public void CreditUserWallet( string userId
            , string currencyCode
            , CreditUserWalletRequest creditUserWalletRequest
            , ResultCallback<WalletInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreditUserWallet(
                    userId,
                    currencyCode,
                    creditUserWalletRequest,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedUserWalletEvent(cb);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Credit a user wallet by currency code, if the wallet does not exist, it will create a new wallet.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="creditUserWalletRequest">The request to credit a user wallet</param>
        /// <param name="callback">Returns Wallet info via callback when completed</param>
        public void CreditUserWalletV2( string userId
            , string currencyCode
            , CreditUserWalletRequest creditUserWalletRequest
            , ResultCallback<CreditUserWalletResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreditUserWalletV2(
                    userId,
                    currencyCode,
                    creditUserWalletRequest,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedUserWalletEventV2(cb);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Fulfill item to a user.
        /// </summary>
        /// <param name="userId">UserId of a user who will receive item.</param>
        /// <param name="fulfillmentRequest">The request to fulfill an item to user.</param>
        /// <param name="callback">Returns Wallet info via callback when completed.</param>
        public void FulfillUserItem( string userId
            , FulfillmentRequest fulfillmentRequest
            , ResultCallback<FulfillmentResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
               api.FulfillUserItem(
                   userId,
                   fulfillmentRequest,
                   cb =>
                   {
                       if (!cb.IsError && cb.Value != null)
                       {
                           SendFulfillUserItemPredefinedEvent(cb);
                       }
                       HandleCallback(cb, callback);
                   }));
        }

        /// <summary>
        /// Get List All Store.
        /// </summary>
        /// <param name="callback">Returns Store info via callback when completed.</param>
        public void GetStoreList( 
            ResultCallback<PlatformStore[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetStoreList(callback));
        }
        
        /// <summary>
        /// Get Items by criteria. Set ItemCriteria fields as null if you don't want to specify the criteria. The result
        /// callback will returns a ItemPagingSlicedResult that contains Items within it.
        /// </summary>
        /// <param name="criteria">Criteria to search items</param>
        /// <param name="callback">Returns a Result that contains ItemPagingSlicedResult via callback when completed.</param>
        public void QueryItemsByCriteria( ItemCriteriaV2 criteria
            , ResultCallback<ItemPagingSlicedResultV2> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(criteria, "Can't get items by criteria; Criteria parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryItemsByCriteria(criteria, callback));
        }
        
        /// <summary>
        /// Get Items by criteria Endpoint Version 2. Set ItemCriteria fields as null if you don't want to specify the criteria.
        /// The result callback will returns a ItemPagingSlicedResult that contains Items within it.
        /// </summary>
        /// <param name="criteria">Criteria to search items</param>
        /// <param name="callback">Returns a Result that contains ItemPagingSlicedResult via callback when completed.</param>
        public void QueryItemsByCriteriaV2( ItemCriteriaV3 criteria
            , ResultCallback<ItemPagingSlicedResultV2> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(criteria, "Can't get items by criteria; Criteria parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryItemsByCriteriaV2(criteria, callback));
        }
    }
}
