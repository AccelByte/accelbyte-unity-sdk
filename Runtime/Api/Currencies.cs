// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Currencies : WrapperBase
    {
        private readonly CurrenciesApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Currencies( CurrenciesApi inApi
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
        internal Currencies( CurrenciesApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get All Currency List Info by namespace
        /// <param name="currencyType">Currency Type</param>
        /// </summary>
        /// <param name="callback">Return a result that contains CurrencyList via callback</param>
        public void GetCurrencyList( ResultCallback<CurrencyList[]> callback, CurrencyType currencyType = CurrencyType.NONE)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetCurrencyListOptionalParameters()
            {
                CurrencyType = currencyType,
                Logger = SharedMemory?.Logger
            };

            GetCurrencyList(optionalParameters, callback);
        }

        /// <summary>
        /// Get All Currency List Info by namespace
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// </summary>
        /// <param name="callback">Return a result that contains CurrencyList via callback</param>
        internal void GetCurrencyList(GetCurrencyListOptionalParameters optionalParameters, ResultCallback<CurrencyList[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetCurrencyList(optionalParameters, callback);
        }
    }
}
