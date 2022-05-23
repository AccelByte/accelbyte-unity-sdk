// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Items service
    /// </summary>
    public class Items : WrapperBase
    {
        private readonly ItemsApi api;
        private readonly IUserSession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Items( ItemsApi inApi
            , IUserSession inSession
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
        internal Items( ItemsApi inApi
            , IUserSession inSession
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
        public void GetItemById( string itemId
            , string region
            , string language
            , ResultCallback<PopulatedItemInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(itemId, "Can't get item; ItemId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetItem(itemId, region, language, callback));
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
            Assert.IsNotNull(criteria, "Can't get items by criteria; Criteria parameter is null!");

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
            , string language = null
            , string region = null )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(appId, "Can't get item by App ID; appId parameter is null!");

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
    }
}