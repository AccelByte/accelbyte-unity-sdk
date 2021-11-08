// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using System.Collections;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Entitlement
    {
        private readonly string @namespace;
        private readonly EntitlementApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Entitlement(EntitlementApi api, ISession session, string ns, CoroutineRunner coroutineRunner)
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
        /// Query user entitlements.
        /// </summary>
        /// <param name="entitlementName">The name of the entitlement (optional)</param>
        /// <param name="itemId">Item's id (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="callback">Returns a Result that contains EntitlementPagingSlicedResult via callback when completed</param>
        /// <param name="entitlementClazz">Class of the entitlement (optional)</param>
        /// <param name="entitlementAppType">This is the type of application that entitled (optional)</param>
        public void QueryUserEntitlements(string entitlementName, string itemId, int offset, int limit, ResultCallback<EntitlementPagingSlicedResult> callback,
            EntitlementClazz entitlementClazz = EntitlementClazz.NONE, EntitlementAppType entitlementAppType = EntitlementAppType.NONE)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementName, "Can't query user entitlements! EntitlementName parameter is null!");
            Assert.IsNotNull(itemId, "Can't query user entitlements! ItemId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.QueryUserEntitlements(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementName,
                    itemId,
                    offset,
                    limit,
                    entitlementClazz,
                    entitlementAppType,
                    callback));
        }

        /// <summary>
        /// Get user's entitlement by the entitlementId.
        /// </summary>
        /// <param name="entitlementId">The id of the entitlement</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void GetUserEntitlementById(string entitlementId, ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementId, "Can't get user entitlement by id! entitlementId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementById(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by app id.
        /// </summary>
        /// <param name="appId">The game's app id</param>
        /// <param name="callback">Returns a result that contains user ownership info via callback when completed</param>
        public void GetUserEntitlementOwnershipByAppId(string appId, ResultCallback<Ownership> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(appId, "Can't get user entitlement ownership by appId! appId parameter is null!");

            if(!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementOwnershipByAppId(
                    AccelBytePlugin.Config.PublisherNamespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    appId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by SKU
        /// </summary>
        /// <param name="sku">the item's SKU</param>
        /// <param name="callback">Returns user's entitlement ownership result via callback when completed</param>
        public void GetUserEntitlementOwnershipBySku(string sku, ResultCallback<Ownership> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(sku, "Can't get user entitlement ownership by SKU! SKU parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementOwnershipBySku(
                    AccelBytePlugin.Config.PublisherNamespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    sku,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by ItemId
        /// </summary>
        /// <param name="ItemId">the item's ItemId</param>
        /// <param name="callback">Returns user's entitlement ownership result via callback when completed</param>
        public void GetUserEntitlementOwnershipByItemId(string ItemId, ResultCallback<Ownership> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(ItemId, "Can't get user entitlement ownership by ItemId! ItemId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementOwnershipByItemId(
                    AccelBytePlugin.Config.Namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    ItemId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership if any of item IDs, app IDs, or SKUs are true
        /// </summary>
        /// <param name="itemIds">the item Ids</param>
        /// <param name="appIds">the app Ids</param>
        /// <param name="skus">the skus</param>
        /// <param name="callback">Returns user's entitlement ownership result if any parameters are true via callback when completed</param>
        public void GetUserEntitlementOwnershipAny(string[] itemIds, string[] appIds, string[] skus, ResultCallback<Ownership> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't get user entitlement any ownership! all itemIds, appIds and skus parameters are null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementOwnershipAny(
                    AccelBytePlugin.Config.PublisherNamespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    itemIds,
                    appIds,
                    skus,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership if any of item IDs, app IDs, or SKUs are true
        /// </summary>
        /// <param name="key">the public key</param>
        /// <param name="itemIds">the item Ids</param>
        /// <param name="appIds">the app Ids</param>
        /// <param name="skus">the skus</param>
        /// <param name="callback">Returns user's entitlement ownership result if any parameters are true via callback when completed</param>
        /// <param name="verifyPublicKey">Do verification on public key. Set False to skip this.</param>
        /// <param name="verifyExpiration">Do verification on expiration. Set False to skip this.</param>
        /// <param name="verifyUserId">Do verification on current user id and sub. Set False to skip this.</param>
        public void GetUserEntitlementOwnershipToken(string key, string[] itemIds, string[] appIds, string[] skus, ResultCallback<OwnershipEntitlement[]> callback, 
            bool verifyPublicKey = true, bool verifyExpiration = true, bool verifyUserId = true)
        {
            Report.GetFunctionLog(this.GetType().Name);

            Assert.IsFalse(string.IsNullOrEmpty(key), "Can't get user entitlement any ownership! public key is null!");
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't get user entitlement any ownership! all itemIds, appIds and skus parameters are null!");

            this.coroutineRunner.Run(
                GetUserEntitlementOwnershipTokenAsync(
                    key, 
                    itemIds, 
                    appIds, 
                    skus, 
                    verifyPublicKey, 
                    verifyExpiration, 
                    verifyUserId, 
                    callback));
        }

        private IEnumerator GetUserEntitlementOwnershipTokenAsync(string key, string[] itemIds, string[] appIds, string[] skus, bool verifyPublicKey, bool verifyExpiration, bool verifyUserId, 
            ResultCallback<OwnershipEntitlement[]> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                yield break;
            }

            Result<OwnershipToken> result = null;

            yield return this.api.GetUserEntitlementOwnershipToken(
                    AccelBytePlugin.Config.PublisherNamespace,
                    this.session.AuthorizationToken,
                    itemIds,
                    appIds,
                    skus,
                    r => result = r);

            if (result.IsError)
            {
                callback.TryError(result.Error.Code);
                yield break;
            }

            if (!JsonWebToken.TryDecodeToken<OwnershipTokenPayload>(key, result.Value.ownershipToken, out var payloadResult, verifyPublicKey, verifyExpiration))
            {
                callback.TryError(ErrorCode.InvalidResponse);
                yield break;
            }

            if (verifyUserId && this.session.UserId != payloadResult.sub)
            {
                callback.TryError(ErrorCode.InvalidResponse);
                yield break;
            }

            callback.TryOk(payloadResult.entitlements);
        }

        /// <summary>
        /// Get user entitlement ownership token if any of item IDs, app IDs, or SKUs are true
        /// </summary>
        /// <param name="itemIds">the item Ids</param>
        /// <param name="appIds">the app Ids</param>
        /// <param name="skus">the skus</param>
        /// <param name="callback">Returns user's entitlement ownership token if any parameters are true via callback when completed</param>
        public void GetUserEntitlementOwnershipTokenOnly(string[] itemIds, string[] appIds, string[] skus,
            ResultCallback<OwnershipToken> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't get user entitlement any ownership! all itemIds, appIds and skus parameters are null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlementOwnershipToken(
                    AccelBytePlugin.Config.PublisherNamespace,
                    this.session.AuthorizationToken,
                    itemIds,
                    appIds,
                    skus,
                    callback));
        }

        /// <summary>
        /// Consume a user entitlement.
        /// </summary>
        /// <param name="entitlementId">The id of the user entitlement.</param>
        /// <param name="useCount">Number of consumed entitlement</param>
        /// <param name="callback">Returns a Result that contains EntitlementInfo via callback when completed</param>
        public void ConsumeUserEntitlement(string entitlementId, int useCount, ResultCallback<EntitlementInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(entitlementId, "Can't consume user entitlement! entitlementId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ConsumeUserEntitlement(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    entitlementId,
                    useCount,
                    callback));
        }

        /// <summary>
        /// Create distribution receiver for current user
        /// </summary>
        /// <param name="extUserId"> External user id is a random string that can be generated by yourself for the receiver</param>
        /// <param name="attributes"> Attributes that contain of serverId, serverName, characterId, and characterName</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void CreateDistributionReceiver(string extUserId, Attributes attributes, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateDistributionReceiver(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    extUserId,
                    attributes,
                    callback
                    ));
        }

        /// <summary>
        /// Delete distribution receiver from user
        /// </summary>
        /// <param name="extUserId"> External User Id that want to be deleted in receiver</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void DeleteDistributionReceiver(string extUserId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteDistributionReceiver(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    extUserId,
                    callback
                    ));
        }


        /// <summary>
        /// Get distribution receiver from user
        /// You can target Publisher Namespace and Publisher UserId or Game Namespace and Game UserId
        /// </summary>
        /// <param name="publisherNamespace"> Publisher Namespace that used to find user</param>
        /// <param name="publisherUserId"> Publisher UserId that want to be get</param>
        /// <param name="callback"> Returns a Result of Distribution Receivers via callback when completed</param>
        public void GetDistributionReceiver(string publisherNamespace, string publisherUserId, ResultCallback<DistributionReceiver[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetDistributionReceiver(
                    publisherNamespace,
                    publisherUserId,
                    this.@namespace,
                    this.session.AuthorizationToken,
                    callback
                    ));
        }

        /// <summary>
        /// Update distribution receiver for current user
        /// </summary>
        /// <param name="extUserId"> External User Id that has been created before for receiver</param>
        /// <param name="attributes"> Attributes that contain of serverId, serverName, characterId, and characterName</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void UpdateDistributionReceiver(string extUserId, Attributes attributes, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateDistributionReceiver(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    extUserId,
                    attributes,
                    callback
                    ));
        }
    }
}
