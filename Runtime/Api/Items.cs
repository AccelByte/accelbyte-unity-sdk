// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
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
    /// Provide an API to access Items service
    /// </summary>
    public class Items : WrapperBase
    {
        private readonly ItemsApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Items( ItemsApi inApi
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
        internal Items( ItemsApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get ItemInfo by itemId
        /// </summary>
        /// <param name="itemId">Item ID to get item with</param>
        /// <param name="region">Region of the item</param>
        /// <param name="language">Display language</param>
        /// <param name="callback">Returns a Result that contains ItemInfo via callback when completed.</param>
        /// <param name="storeId">If it's leaved string empty, the value will be got from published store id on the namespace</param>
        /// <param name="populateBundle">Whether populate bundled items if it's a bundle, default value is false</param>
        public void GetItemById( string itemId
            , string region
            , string language
            , ResultCallback<PopulatedItemInfo> callback
            , string storeId = ""
            , bool populateBundle = false )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetItem(itemId, region, language, callback, storeId, populateBundle));
        }

        /// <summary>
        /// Get Items by criteria. Set ItemCriteria fields as null if you don't want to specify the criteria. The result
        /// callback will returns a ItemPagingSlicedResult that contains Items within it.
        /// </summary>
        /// <param name="criteria">Criteria to search items</param>
        /// <param name="callback">Returns a Result that contains ItemPagingSlicedResult via callback when completed.</param>
        public void GetItemsByCriteria( ItemCriteria criteria
            , ResultCallback<ItemPagingSlicedResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetItemsByCriteria(criteria, callback));
        }

        /// <summary>
        /// Get item info by AppId.
        /// </summary>
        /// <param name="appId">AppId of an item</param>
        /// <param name="callback">Returns a result that contain ItemInfo via callback when completed.</param>
        /// <param name="language">display language</param>
        /// <param name="region">region of items</param>
        public void GetItemByAppId( string appId
            , ResultCallback<ItemInfo> callback
            , string language = ""
            , string region = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetItemByAppId(
                    AccelBytePlugin.Config.PublisherNamespace,
                    appId,
                    callback,
                    language,
                    region));
        }

        /// <summary>
        /// Search Item.
        /// </summary>
        /// <param name="language">display language</param>
        /// <param name="keyword">Keyword Item's keyword in title or description or long description</param>
        /// <param name="offset">offset of items</param>
        /// <param name="limit">limit of items</param>
        /// <param name="region">Region ISO 3166-1 alpha-2 country tag, e.g., "US", "CN".</param>
        /// <param name="callback">Returns a result that contain ItemPagingSlicedResult via callback when completed.</param>
        public void SearchItem(string language
            , string keyword
            , int offset
            , int limit
            , string region
            , ResultCallback<ItemPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchItem(
                    language,
                    keyword,
                    offset,
                    limit,
                    region,
                    callback));
        }

        /// <summary>
        /// Get item info by sku.
        /// </summary>
        /// <param name="sku">Sku should contain specific number of item Sku</param>
        /// <param name="language">display language</param>
        /// <param name="region">region of items</param>
        /// <param name="callback">Returns a result that contain ItemInfo via callback when completed.</param>
        public void GetItemBySku(string sku
            , string language
            , string region
            , ResultCallback<ItemInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetItemBySku(
                    sku,
                    language,
                    region,
                    callback ));
        }

        /// <summary>
        /// Bulk Get Locale Items.
        /// </summary>
        /// <param name="itemIds">Item IDs to get item</param>
        /// <param name="callback">Returns a result that contain ItemInfo via callback when completed.</param>
        /// <param name="language">display language</param>
        /// <param name="region">region of items</param>
        /// <param name="storeId">If it's leaved string empty, the value will be got from published store id on the namespace</param>
        public void BulkGetLocaleItems(string[] itemIds
            , string language
            , string region
            , ResultCallback<ItemInfo[]> callback
            , string storeId = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(itemIds);
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
                api.BulkGetLocaleItems(
                    itemIds,
                    language,
                    region,
                    callback,
                    storeId ));
        }

        /// <summary>
        /// Get list all stores in a namespace.
        /// </summary>
        /// <param name="callback">Returns a result that contain PlatformStore via callback when completed.</param>
        public void GetListAllStore(ResultCallback<PlatformStore[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListAllStore(callback));
        }

        /// <summary>
        /// Get estimated prices of an item.
        /// </summary>
        /// <param name="itemIds">Item IDs to get the estimated items information</param>
        /// <param name="region">Region Code, ISO 3166-1 alpha-2 country tag, e.g., "US", "CN".Item IDs to get item</param>
        /// <param name="callback">Returns a result that contain EstimatedPricesInfo array via callback when completed.</param>
        public void GetEstimatedPrice(string[] itemIds
			, string region
			, ResultCallback<EstimatedPricesInfo[]> callback)
  
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetEstimatedPrice(itemIds, region, callback));
        }
    }
}
