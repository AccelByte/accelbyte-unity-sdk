﻿// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information virtual currency owned by the user
    /// </summary>
    public class Wallet : WrapperBase
    {
        private readonly WalletApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Wallet( WalletApi inApi
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
        internal Wallet( WalletApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get wallet information owned by a user
        /// </summary>
        /// <param name="currencyCode">Currency code for the wallet</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [Obsolete("This does not support for multiplatform wallet, use GetWalletInfoByCurrencyCodeV2 instead")]
        public void GetWalletInfoByCurrencyCode( string currencyCode
            , ResultCallback<WalletInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(currencyCode, "Can't get wallet info by currency code; CurrencyCode is null!");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetWalletInfoByCurrencyCode(
                session.UserId,
                currencyCode,
                callback));
        }
        
        /// <summary>
        /// Get wallet information owned by a user
        /// </summary>
        /// <param name="currencyCode">Currency code for the wallet</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void GetWalletInfoByCurrencyCodeV2( string currencyCode
            , ResultCallback<WalletInfoResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(currencyCode);
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

            coroutineRunner.Run(api.GetWalletInfoByCurrencyCodeV2(
                session.UserId,
                currencyCode,
                callback));
        }
    }
}