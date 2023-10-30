// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using System.Collections.Generic;

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

        private void RefineRedeemCodeResult(Result<FulfillmentResult> apiCallResult, string code)
        {
            IAccelByteTelemetryPayload payload;
            payload = CreatePredefinedPayload<FulfillmentResult>(apiCallResult, PredefinedEventMode.RedeemCampaignCode, code);
            SendPredefinedEvent(payload);
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
            Assert.IsNotNull(code, "Can't redeem code! code parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Action<Result<FulfillmentResult>, string> onPredefinedEventTrigger = null;
            if(predefinedEventScheduler != null)
            {
                onPredefinedEventTrigger = RefineRedeemCodeResult;
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
                    callback,
                    onPredefinedEventTrigger));
        }

        #region PredefinedEvents

        private PredefinedEventScheduler predefinedEventScheduler;

        /// <summary>
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventScheduler">Predefined event scheduler object reference</param>
        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;
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
            if (payload != null)
            {
                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
        }

        #endregion
    }
}
