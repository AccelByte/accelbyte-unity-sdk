// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Models;
using AccelByte.Core;
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
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
                    callback));
        }
    }
}
