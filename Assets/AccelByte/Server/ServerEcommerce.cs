// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerEcommerce
    {
        private readonly ServerEcommerceApi api;
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerEcommerce(ServerEcommerceApi api, IServerSession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct Ecommerce manager; api is null!");
            Assert.IsNotNull(session, "Can not construct Ecommerce manager; session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can not construct Ecommerce manager; ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "Can not construct Ecommerce manager; coroutineRunner is null!");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        ///  Get a user's entitlement by the entitlementId.
        /// </summary>
        /// <param name="entitlementId">The id of the entitlement</param>
        /// <param name="callback">Returns all StackableEntitlements Info via callback when completed</param>
        public void GetUserEntitlementById(string entitlementId, ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementById(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    entitlementId,
                    callback));
        }

        /// <summary>
        /// Granting Entitlement(s) to a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="grantUserEntitlementsRequest"> Consist of the entitlement(s) that will be granted</param>
        /// <param name="callback">Returns all StackableEntitlements Info via callback when completed</param>
        public void GrantUserEntitlement(string userId, GrantUserEntitlementRequest[] grantUserEntitlementsRequest,
            ResultCallback<StackableEntitlementInfo[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GrantUserEntitlement(
                    this.namespace_,
                    userId,
                    this.session.AuthorizationToken,
                    grantUserEntitlementsRequest,
                    callback));
        }

        /// <summary>
        /// Credit a user wallet by currency code, if the wallet does not exist, it will create a new wallet.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="creditUserWalletRequest">The request to credit a user wallet</param>
        /// <param name="callback">Returns Wallet info via callback when completed</param>
        public void CreditUserWallet(string userId, string currencyCode, CreditUserWalletRequest creditUserWalletRequest,
            ResultCallback<WalletInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreditUserWallet(
                    this.namespace_,
                    userId,
                    this.session.AuthorizationToken,
                    currencyCode,
                    creditUserWalletRequest,
                    callback));
        }
    }
}
