// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Api.Interface;
using System;
using NUnit.Framework;

namespace AccelByte.Api
{
    public class Inventory : WrapperBase, IClientInventory
    {
        private readonly InventoryApi api;
        private readonly UserSession session;

        [UnityEngine.Scripting.Preserve]
        internal Inventory(InventoryApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");

            api = inApi;
            session = inSession;
        }

        public void BulkDeleteInventoryItems(
            string inventoryId
            , DeleteUserInventoryItemRequest[] deletedItemsRequest
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkDeleteInventoryItems(inventoryId, deletedItemsRequest, callback);
        }

        public void BulkUpdateInventoryItems(
            string inventoryId
            , UpdateUserInventoryItemRequest[] updatedItemsRequest
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkUpdateInventoryItems(inventoryId, updatedItemsRequest, callback);
        }

        public void ConsumeUserInventoryItem(
            string inventoryId
            , ConsumeUserItemsRequest consumedItemsRequest
            , ResultCallback<UserItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ConsumeUserInventoryItem(inventoryId, consumedItemsRequest, callback);
        }

        public void GetInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryConfigurations(callback, sortBy, limit, offset, inventoryConfigurationCode);
        }

        public void GetInventoryTags(
            ResultCallback<InventoryTagsPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryTags(callback, sortBy, limit, offset);
        }

        public void GetItemTypes(ResultCallback<ItemTypesPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetItemTypes(callback, sortBy, limit, offset);
        }

        public void GetUserInventories(
            ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventories(callback, sortBy, limit, offset, inventoryConfigurationCode);
        }

        public void GetUserInventoryAllItems(
            string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback
            , UserItemSortBy sortBy = UserItemSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string sourceItemId = ""
            , TagQueryBuilder tagBuilder = null
            , int? quantity = null
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryAllItems(inventoryId
                , callback
                , sortBy
                , limit
                , offset
                , sourceItemId
                , tagBuilder
                , quantity);
        }

        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryItem(inventoryId, slotId, sourceItemId, callback);
        }

        public void MoveItemsBetweenInventories(
            string targetInventoryId
            , MoveUserItemsBetweenInventoriesRequest moveItemsRequest
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.MoveItemsBetweenInventories(targetInventoryId, moveItemsRequest, callback);
        }
    }
}