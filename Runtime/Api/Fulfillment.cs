// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using AccelByte.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Fulfillment : WrapperBase
    {
        private readonly FulfillmentApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Fulfillment( FulfillmentApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Fulfillment( FulfillmentApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Redeem Campaign Code to Receive In Game Item.
        /// </summary>
        /// <param name="code">The campaign code to redeem.</param>
        /// <param name="region">Region of the item. If not set, the region from the access token will be used.</param>        
        /// <param name="language">Display language.null If not set, the language from the access token will be used.</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void RedeemCode( string code
            , string region
            , string language
            , ResultCallback<FulfillmentResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(code);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            FulFillCodeRequest fulFillCodeRequest = new FulFillCodeRequest 
            {
                code = code,
                region = region,
                language = language
            };

            coroutineRunner.Run(
                api.RedeemCode(
                    session.UserId,
                    fulFillCodeRequest,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            RefineRedeemCodeResult(cb, code);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        #region PredefinedEvents
        private void RefineRedeemCodeResult(Result<FulfillmentResult> apiCallResult, string code)
        {
            if (apiCallResult.Value.entitlementSummaries == null
                || apiCallResult.Value.creditSummaries == null
                || apiCallResult.Value.subscriptionSummaries == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload;
            payload = CreatePredefinedPayload<FulfillmentResult>(apiCallResult, PredefinedEventMode.RedeemCampaignCode, code);
            SendPredefinedEvent(payload);
        }

        private enum PredefinedEventMode
        {
            RedeemCampaignCode
        }

        private IAccelByteTelemetryPayload CreatePredefinedPayload<T>(Result<T> apiCallResult, PredefinedEventMode mode, string code)
        {
            switch (mode)
            {
                case PredefinedEventMode.RedeemCampaignCode:
                    {
                        var fulfilmentResult = apiCallResult as Result<FulfillmentResult>;
                        string userId = fulfilmentResult.Value.userId;

                        List<PredefinedEntitlementSummary> entitlements = new List<PredefinedEntitlementSummary>();
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

                        List<PredefinedCreditSummary> credits = new List<PredefinedCreditSummary>();
                        foreach (var summary in fulfilmentResult.Value.creditSummaries)
                        {
                            PredefinedCreditSummary credsSummary = new PredefinedCreditSummary(
                                summary.walletId,
                                summary.userId,
                                summary.amount,
                                summary.CurrencyCode);

                            credits.Add(credsSummary);
                        }

                        List<PredefinedSubscriptionSummary> subscriptions = new List<PredefinedSubscriptionSummary>();
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

                        var payload = new PredefinedCampaignCodeRedeemedPayload(userId, code, entitlements, credits, subscriptions);
                        return payload;
                    }
                default:
                    return null;
            }
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (payload != null && predefinedEventScheduler != null)
            {
                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }
            callback?.Try(result);
        }

        #endregion
    }
}
