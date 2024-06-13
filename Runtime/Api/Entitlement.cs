// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using AccelByte.Utils;
using System;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Entitlement : WrapperBase
    {
        private readonly EntitlementApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Entitlement( EntitlementApi inApi
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
        internal Entitlement( EntitlementApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Query user entitlements.
        /// </summary>
        /// <param name="entitlementName">The name of the entitlement (optional)</param>
        /// <param name="itemId">Item's id (optional)</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="callback">
        /// Returns a Result that contains EntitlementPagingSlicedResult via callback when completed
        /// </param>
        /// <param name="entitlementClazz">Class of the entitlement (optional)</param>
        /// <param name="entitlementAppType">This is the type of application that entitled (optional)</param>
        /// <param name="features">The feature array</param>
        public void QueryUserEntitlements(string entitlementName = null
            , string itemId = null
            , int offset = 0
            , int limit = 20
            , ResultCallback<EntitlementPagingSlicedResult> callback = null
            , EntitlementClazz entitlementClazz = EntitlementClazz.NONE
            , EntitlementAppType entitlementAppType = EntitlementAppType.NONE
            , string[] features = null)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryUserEntitlements(
                    session.UserId,
                    entitlementName,
                    itemId,
                    features, 
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
        public void GetUserEntitlementById( string entitlementId
            , ResultCallback<EntitlementInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(entitlementId);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!ValidateAccelByteId(entitlementId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetEntitlementIdInvalidMessage(entitlementId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementById(
                    session.UserId,
                    entitlementId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by app id.
        /// </summary>
        /// <param name="appId">The game's app id</param>
        /// <param name="callback">
        /// Returns a result that contains user ownership info via callback when completed
        /// </param>
        public void GetUserEntitlementOwnershipByAppId( string appId
            , ResultCallback<Ownership> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(appId);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipByAppId(
                   api.Config.PublisherNamespace,
                    session.UserId,
                    appId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by SKU
        /// </summary>
        /// <param name="sku">the item's SKU</param>
        /// <param name="callback">
        /// Returns user's entitlement ownership result via callback when completed
        /// </param>
        public void GetUserEntitlementOwnershipBySku( string sku
            , ResultCallback<Ownership> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(sku);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipBySku(
                    api.Config.PublisherNamespace,
                    session.UserId,
                    sku,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership by ItemId
        /// </summary>
        /// <param name="ItemId">the item's ItemId</param>
        /// <param name="callback">
        /// Returns user's entitlement ownership result via callback when completed
        /// </param>
        public void GetUserEntitlementOwnershipByItemId( string ItemId
            , ResultCallback<Ownership> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(ItemId);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipByItemId(
                    session.UserId,
                    ItemId,
                    callback));
        }

        /// <summary>
        /// Get user entitlement ownership if any of item IDs, app IDs, or SKUs are true
        /// </summary>
        /// <param name="itemIds">the item Ids</param>
        /// <param name="appIds">the app Ids</param>
        /// <param name="skus">the skus</param>
        /// <param name="callback">
        /// Returns user's entitlement ownership result if any parameters are true via callback when completed
        /// </param>
        public void GetUserEntitlementOwnershipAny( string[] itemIds
            , string[] appIds
            , string[] skus
            , ResultCallback<Ownership> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (itemIds == null && appIds == null && skus == null)
            {
                callback.TryError(new Error(
                    ErrorCode.InvalidRequest
                    , "Can't get user entitlement any ownership! All itemIds, " +
                    "appIds, and skus parameters are null!")
                );
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipAny(
                    api.Config.PublisherNamespace,
                    session.UserId,
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
        /// <param name="callback">R
        /// eturns user's entitlement ownership result if any parameters are true via callback when completed
        /// </param>
        /// <param name="verifyPublicKey">Do verification on public key. Set False to skip </param>
        /// <param name="verifyExpiration">Do verification on expiration. Set False to skip </param>
        /// <param name="verifyUserId">Do verification on current user id and sub. Set False to skip </param>
        public void GetUserEntitlementOwnershipToken( string key
            , string[] itemIds
            , string[] appIds
            , string[] skus
            , ResultCallback<OwnershipEntitlement[]> callback
            , bool verifyPublicKey = true
            , bool verifyExpiration = true
            , bool verifyUserId = true )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(key);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (itemIds == null && appIds == null && skus == null)
            {
                callback.TryError(new Error(
                    ErrorCode.InvalidRequest
                    , "Can't get user entitlement any ownership! All itemIds, " +
                    "appIds, and skus parameters are null!")
                );
                return;
            }

            coroutineRunner.Run(
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

        private IEnumerator GetUserEntitlementOwnershipTokenAsync( string key
            , string[] itemIds
            , string[] appIds
            , string[] skus
            , bool verifyPublicKey
            , bool verifyExpiration
            , bool verifyUserId
            , ResultCallback<OwnershipEntitlement[]> callback )
        {
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                yield break;
            }

            Result<OwnershipToken> result = null;

            yield return api.GetUserEntitlementOwnershipToken(
                    api.Config.PublisherNamespace,
                    itemIds,
                    appIds,
                    skus,
                    r => result = r);

            if (result.IsError)
            {
                callback.TryError(result.Error.Code);
                yield break;
            }

            if (!JsonWebToken.TryDecodeToken<OwnershipTokenPayload>(
                    key, 
                    result.Value.ownershipToken, 
                    out var payloadResult, 
                    verifyPublicKey, 
                    verifyExpiration))
            {
                callback.TryError(ErrorCode.InvalidResponse);
                yield break;
            }

            if (verifyUserId && session.UserId != payloadResult.sub)
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
        /// <param name="callback">
        /// Returns user's entitlement ownership token if any parameters are true via callback when completed
        /// </param>
        public void GetUserEntitlementOwnershipTokenOnly( string[] itemIds
            , string[] appIds
            , string[] skus
            , ResultCallback<OwnershipToken> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (itemIds == null && appIds == null && skus == null)
            {
                callback.TryError(new Error(
                    ErrorCode.InvalidRequest
                    , "Can't get user entitlement any ownership! All itemIds, " +
                    "appIds, and skus parameters are null!")
                );
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipToken(
                    api.Config.PublisherNamespace,
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
        /// <param name="callback">
        /// Returns a Result that contains EntitlementInfo via callback when completed
        /// <param name="options">Options of consumed entitlement</param>
        /// <param name="requestId">Request id(Optional), A unique tracking ID provided by the developer, can just left it empty if they don't want to track,
        /// When a request id is submitted, it will return original successful response</param>
        /// </param>
        public void ConsumeUserEntitlement( string entitlementId
            , int useCount
            , ResultCallback<EntitlementInfo> callback
            , string[] options = null
            , string requestId = null)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(entitlementId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetEntitlementIdInvalidMessage(entitlementId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ConsumeUserEntitlement(
                    session.UserId,
                    entitlementId,
                    useCount,
                    callback,
                    options,
                    requestId));
        }

        /// <summary>
        /// Create distribution receiver for current user
        /// </summary>
        /// <param name="extUserId">
        /// External user id is a random string that can be generated by yourself for the receiver</param>
        /// <param name="attributes">
        /// Attributes that contain of serverId, serverName, characterId, and characterName</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.")]
        public void CreateDistributionReceiver( string extUserId
            , Attributes attributes
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateDistributionReceiver(
                    session.UserId,
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
        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.")]
        public void DeleteDistributionReceiver( string extUserId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteDistributionReceiver(
                    session.UserId,
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
        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.")]
        public void GetDistributionReceiver( string publisherNamespace
            , string publisherUserId
            , ResultCallback<DistributionReceiver[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
        
            coroutineRunner.Run(
                api.GetDistributionReceiver(
                    publisherNamespace,
                    publisherUserId,
                    callback));
        }

        /// <summary>
        /// Update distribution receiver for current user
        /// </summary>
        /// <param name="extUserId">External User Id that has been created before for receiver</param>
        /// <param name="attributes">Attributes that contain of serverId, serverName, characterId, and characterName</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [Obsolete("Platform Service version 3.4.0 and above doesn't support this anymore, This feature already removed.")]
        public void UpdateDistributionReceiver( string extUserId
            , Attributes attributes
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateDistributionReceiver(
                    session.UserId,
                    extUserId,
                    attributes,
                    callback
                    ));
        }

        /// <summary>
        /// Sync (Verify and fulfil) item entitlement from Google Play platform purchase.
        /// </summary>
        /// <param name="syncRequest">
        /// That contain of OrderId, PackageName, ProductId,
        /// PurchaseTime, and PurchaseToken to verify and sync item user bought from Google Play.
        /// </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncMobilePlatformPurchaseGoogle( PlatformSyncMobileGoogle syncRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncMobilePlatformPurchaseGoogle(
                    session.UserId,
                    syncRequest,
                    callback
                    ));
        }

        /// <summary>
        /// Sync (Verify and fulfil) item entitlement from Apple Store platform purchase.
        /// </summary>
        /// <param name="syncRequest">
        /// That contain of ProductId, TransactionId, ReceiptData,
        /// and ExcludeOldTransactions to verify and sync item user bought from Apple Store.
        /// </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncMobilePlatformPurchaseApple( PlatformSyncMobileApple syncRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncMobilePlatformPurchaseApple(
                    session.UserId,
                    syncRequest,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize Xbox inventory's DLC items.
        /// </summary>
        /// <param name="XboxDLCSync">Contains XSTSToken needed for Xbox DLC sync</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncXBoxDLC( XBoxDLCSync XboxDLCSync
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncXBoxDLC(
                    session.UserId,
                    XboxDLCSync,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize Steam DLC.
        /// </summary>
        /// <param name="userSteamId">Steam ID of the user</param>
        /// <param name="userAppId">Steam app id</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncSteamDLC( string userSteamId
            , string userAppId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncSteamDLC(
                    session.UserId,
                    userSteamId,
                    userAppId,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize with DLC entitlements in PSN Store.
        /// </summary>
        /// <param name="PSSyncModel">Contains ServiceLabel needed for PlayStation DLC sync</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncPSNDLC( PlayStationDLCSync PSSyncModel
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncPSNDLC(
                    session.UserId,
                    PSSyncModel,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize with Twitch entitlements in Twitch Drop.
        /// </summary>
        /// <param name="TwitchDropSyncReq"> Contains gameId, region, and language needed for Twitch Drop sync</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncTwitchDropItem( TwitchDropSync TwitchDropSyncReq
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                api.SyncTwitchDropItem(
                    session.UserId,
                    TwitchDropSyncReq,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize Epic Game Durable/dlc Items.
        /// </summary>
        /// <param name="epicGamesJwtToken"> Contains epicGamesJwtToken needed for EpicGames Durable sync</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncEpicGamesDurableItems(string epicGamesJwtToken
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                api.SyncEpicGamesDurableItems(
                    session.UserId,
                    epicGamesJwtToken,
                    callback
                    ));
        }
        
        /// <summary>
        /// Synchronize with dlc entitlements in PSN Store with multiple service labels.
        /// </summary>
        /// <param name="playStationDlcSyncMultipleService">Contains ServiceLabel needed for PlayStation DLC Multiple Service sync</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SyncPSNDLCMultipleService( PlayStationDLCSyncMultipleService playStationDlcSyncMultipleService
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncPSNDLCMultipleService(
                    session.UserId,
                    playStationDlcSyncMultipleService,
                    callback
                ));
        }

        /// <summary>
        /// Validate User Item Purchase Condition.
        /// </summary>
        /// <param name="items"> Contains item that needed for Validate User Item Purchase Condition</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void ValidateUserItemPurchaseCondition(string[] items
            , ResultCallback<PlatformValidateUserItemPurchaseResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            var requestModel = new ValidateUserItemPurchaseConditionRequest()
            {
                ItemIds = items
            };

            coroutineRunner.Run(api.ValidateUserItemPurchaseCondition(requestModel, callback));
        }

        /// <summary>
        /// Get user entitlement ownership by itemIds.
        /// </summary>
        /// <param name="ids"> Contains ids ItemIds</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void GetUserEntitlementOwnershipByItemIds(string[] ids
            , ResultCallback<EntitlementOwnershipItemIds[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserEntitlementOwnershipByItemIds(
                    session.UserId,
                    ids,
                    callback
                    ));
        }

        /// <summary>
        /// Synchronize with entitlements in PSN Store.
        /// </summary>
        /// <param name="psnModel"> Playstation's DLC Model Sync Request</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncEntitlementPSNStore(PlayStationDLCSync psnModel
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncEntitlementPSNStore(
                    session.UserId,
                    psnModel,
                    callback
                ));
        }

        /// <summary>
        /// Synchronize Comsumable Entitlement in XBOX Inventory.
        /// </summary>
        /// <param name="model"> XBOX's Consumable Entitlement Model Sync Request</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncXboxInventory(XboxInventoryRequest model
            , ResultCallback<XboxInventoryResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncXboxInventory(
                    session.UserId,
                    model,
                    callback
                ));
        }

        /// <summary>
        /// Synchronize with entitlements in PSN Store with multiple service labels.
        /// </summary>
        /// <param name="playStationMultipleServiceRequest"> Playstation's Multiple Service Labels Model Sync Request</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncEntitlementPSNMultipleService(PlayStationMultipleServiceRequest playStationMultipleServiceRequest
            , ResultCallback<PlayStationMultipleServiceResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncEntitlementPSNMultipleService(
                    session.UserId,
                    playStationMultipleServiceRequest,
                    callback
                ));
        }
        
        /// <summary>
        /// Sell User Entitlement.
        /// </summary>
        /// <param name="userEntitlementSoldParams">The user entitlement parameters containing the user ID and entitlement ID to sell.</param>
        /// <param name="entitlementSoldRequest">The entitlement sold request containing the use count and request ID.</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SellUserEntitlement(UserEntitlementSoldParams userEntitlementSoldParams
            , EntitlementSoldRequest entitlementSoldRequest
            , ResultCallback<SellItemEntitlementInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            userEntitlementSoldParams.UserId = session.UserId;
            coroutineRunner.Run(
                api.SellUserEntitlement(
                    userEntitlementSoldParams,
                    entitlementSoldRequest,
                    callback
                ));
        }

        /// <summary>
        /// Synchronize Oculus Consumable Entitlement/IAP.
        /// </summary>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncOculusConsumableEntitlements(ResultCallback<SyncOculusConsumableEntitlementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncOculusConsumableEntitlements(
                    session.UserId,
                    callback
                ));
        }

        /// <summary>
        /// Synchronize Oculus Downloadable Content/DLC.
        /// </summary>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncOculusDLC(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncOculusDLC(
                    session.UserId,
                    callback
                ));
        }

        /// <summary>
        /// Synchronize Epic Games Inventory/IAP.
        /// </summary>
        /// <param name="epicGamesJwtToken">Epic Game Jwt Token.</param>
        /// <param name="callback"> Returns a Result via callback when completed</param>
        public void SyncEpicGameInventory(string epicGamesJwtToken
            , ResultCallback<SyncEpicGamesInventoryResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SyncEpicGameInventory(
                    session.UserId,
                    epicGamesJwtToken,
                    callback
                ));
        }

        /// <summary>
        /// Get any "Durable" DLC type from a specific namespace
        /// It will not get "Consumables"
        /// </summary>
        /// <param name="platformType">Target platform to be queried</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetDlcDurableRewardSimpleMap(PlatformType platformType, ResultCallback<DlcConfigRewardShortInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            string targetPlatform = ConvertActivePlatformToGetDlcDurableReward(platformType);

            if (string.IsNullOrEmpty(targetPlatform))
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "Available Platforms: STEAM, EPICGAMES, OCULUS, PS4, PS5, LIVE"));
                return;
            }

            GetDlcDurableRewardSimpleMap(targetPlatform, callback);
        }

        /// <summary>
        /// Get any "Durable" DLC type from a specific namespace
        /// It will not get "Consumables"
        /// </summary>
        /// <param name="platformTypeId">Target platform to be queried</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetDlcDurableRewardSimpleMap(string platformTypeId, ResultCallback<DlcConfigRewardShortInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var error = ApiHelperUtils.CheckForNullOrEmpty(platformTypeId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            api.GetDlcDurableRewardSimpleMap(platformTypeId, callback);
        }

        #region Utils
        static public string ConvertActivePlatformToGetDlcDurableReward(PlatformType platformType)
        {
            string targetPlatform = string.Empty;
            switch (platformType)
            {
                case PlatformType.Steam:
                case PlatformType.EpicGames:
                case PlatformType.Oculus:
                    targetPlatform = platformType.ToString().ToUpper();
                    break;
                case PlatformType.PS4:
                case PlatformType.PS5:
                    targetPlatform = "PSN";
                    break;
                case PlatformType.Live:
                    targetPlatform = "XBOX";
                    break;
                default:
                    break;
            }

            return targetPlatform;
        }
        #endregion
    }
}
