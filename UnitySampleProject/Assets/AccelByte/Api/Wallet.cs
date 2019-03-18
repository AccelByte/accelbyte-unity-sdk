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
        private readonly string @namespace;
        private readonly WalletApi api;
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal Wallet(string @namespace, WalletApi api, User user, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can't construct Wallet! api is null!");
            Assert.IsNotNull(@namespace, "Can't construct Wallet! namespace parameter is null!");
            Assert.IsNotNull(user, "Can't construct Wallet! user parameter is null!");
            Assert.IsNotNull(taskDispatcher, "taskDispatcher must not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner must not be null");

            this.api = api;
            this.@namespace = @namespace;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
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

            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetWalletInfoByCurrencyCode(
                        this.@namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        currencyCode,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<WalletInfo>) result)),
                    this.user));
        }
    }
}