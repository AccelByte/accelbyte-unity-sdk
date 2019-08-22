// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information virtual currency owned by the user
    /// </summary>
    public class Wallet
    {
        private readonly WalletApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Wallet(WalletApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get wallet information owned by a user
        /// </summary>
        /// <param name="currencyCode">Currency code for the wallet</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void GetWalletInfoByCurrencyCode(string currencyCode, ResultCallback<WalletInfo> callback)
        {
            Assert.IsNotNull(currencyCode, "Can't get wallet info by currency code; CurrencyCode is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(this.api.GetWalletInfoByCurrencyCode(
                        this.@namespace,
                        this.session.UserId,
                        this.session.AuthorizationToken,
                        currencyCode,
                        callback));
        }
    }
}