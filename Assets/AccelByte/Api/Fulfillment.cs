// Copyright (c) 2020 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Fulfillment
    {
        private readonly string @namespace;
        private readonly FulfillmentApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Fulfillment(FulfillmentApi api, ISession session, string ns, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(ns), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = ns;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Redeem Campaign Code to Receive In Game Item.
        /// </summary>
        /// <param name="code">The campaign code to redeem.</param>
        /// <param name="region">Region of the item. If not set, the region from the access token will be used.</param>        
        /// <param name="language">Display language.null If not set, the language from the access token will be used.</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void RedeemCode(string code, string region, string language, ResultCallback<FulfillmentResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(code, "Can't redeem code! code parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            FulFillCodeRequest fulFillCodeRequest = new FulFillCodeRequest {
                code = code,
                region = region,
                language = language
            };

            this.coroutineRunner.Run(
                this.api.RedeemCode(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    fulFillCodeRequest,
                    callback));
        }
    }
}
