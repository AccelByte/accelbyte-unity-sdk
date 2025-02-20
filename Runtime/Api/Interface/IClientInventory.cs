// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Api.Interface
{
    public interface IClientInventory
    {
        public void GetInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        public void GetInventoryTags(
            ResultCallback<InventoryTagsPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        public void GetUserInventories(
            ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        public void GetItemTypes(
            ResultCallback<ItemTypesPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        /// <summary>
        /// List all user's Item in an inventory
        /// </summary>
        /// <param name="inventoryId">selected inventory Id</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        public void GetUserInventoryAllItems(
            string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback
        );

        /// <summary>
        /// List all user's Item in an inventory
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="optionalParameters">Optional parameter to get specific result</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        public void GetUserInventoryAllItems(
            string inventoryId
            , GetUserInventoryAllItemsOptionalParameters optionalParameters
            , ResultCallback<UserItemsPagingResponse> callback
        );

        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        );

        public void BulkUpdateInventoryItems(
            string inventoryId
            , UpdateUserInventoryItemRequest[] updatedItemsRequest
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        public void BulkDeleteInventoryItems(
            string inventoryId
            , DeleteUserInventoryItemRequest[] deletedItemsRequest
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        public void MoveItemsBetweenInventories(
            string targetInventoryId
            , MoveUserItemsBetweenInventoriesRequest moveItemsRequest
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback
        );

        public void ConsumeUserInventoryItem(
            string inventoryId
            , ConsumeUserItemsRequest consumedItemsRequest
            , ResultCallback<UserItem> callback
        );
    }
}