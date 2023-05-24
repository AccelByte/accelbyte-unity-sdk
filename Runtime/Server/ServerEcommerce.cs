// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GrantUserEntitlement(
                    userId,
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
                    callback));
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
                    callback));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.FulfillUserItem(
                    userId,
                    fulfillmentRequest,
                    callback));
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
