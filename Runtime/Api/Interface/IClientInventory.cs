// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api.Interface
{
    public interface IClientInventory
    {
        /// <summary>
        /// Retrieves all inventory configurations on the namespace.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result items by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
        /// <param name="inventoryConfigurationCode">Optional parameter to filter result items by inventory configuration code</param>
        public void GetInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        /// <summary>
        /// Retrieves all available inventory tags on the current namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result items by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
        public void GetInventoryTags(
            ResultCallback<InventoryTagsPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        /// <summary>
        /// Retrieves all of the current user's available inventories.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result items by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
        /// <param name="inventoryConfigurationCode">Optional parameter to filter result items by inventory configuration code</param>
        public void GetUserInventories(
            ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        /// <summary>
        /// Retrieves available item types on the current namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result items by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
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

        /// <summary>
        /// Get data of specific item in the user's inventory
        /// </summary>
        /// <param name="inventoryId">Id of inventory the item is in</param>
        /// <param name="slotId">Id of slot the item occupies</param>
        /// <param name="sourceItemId">Id of item to get data for</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Updates user's inventory items in bulk
        /// </summary>
        /// <param name="inventoryId">Id of inventory to update items for</param>
        /// <param name="updatedItemsRequest">Data of items to update</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with payload parameter instead.")]
        public void BulkUpdateInventoryItems(
            string inventoryId
            , UpdateUserInventoryItemRequest[] updatedItemsRequest
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Updates user's inventory items in bulk
        /// </summary>
        /// <param name="inventoryId">Id of inventory to update items for</param>
        /// <param name="payload">Data of items to update</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkUpdateInventoryItems(
            string inventoryId
            , BulkUpdateInventoryItemsPayload[] payload
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Deletes user's inventory items in bulk
        /// </summary>
        /// <param name="inventoryId">Id of inventory to delete items for</param>
        /// <param name="deletedItemsRequest">Data of items to delete</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with payload parameter instead.")]
        public void BulkDeleteInventoryItems(
            string inventoryId
            , DeleteUserInventoryItemRequest[] deletedItemsRequest
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Deletes user's inventory items in bulk
        /// </summary>
        /// <param name="inventoryId">Id of inventory to delete items for</param>
        /// <param name="payload">Data of items to delete</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkDeleteInventoryItems(
            string inventoryId
            , BulkDeleteUserInventoryItemsPayload[] payload
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Moves user's item from one inventory to another.
        /// </summary>
        /// <param name="targetInventoryId">Id of inventory to move items to</param>
        /// <param name="moveItemsRequest">Data of items to be moved</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with payload parameter instead.")]
        public void MoveItemsBetweenInventories(
            string targetInventoryId
            , MoveUserItemsBetweenInventoriesRequest moveItemsRequest
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback
        );

        /// <summary>
        /// Moves user's item from one inventory to another.
        /// </summary>
        /// <param name="targetInventoryId">Id of inventory to move items to</param>
        /// <param name="sourceInventoryId">Id of inventory to move items from</param>
        /// <param name="payload">Data of items to be moved</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void MoveItemsBetweenInventories(
            string targetInventoryId
            , string sourceInventoryId
            , MoveUserItemsBetweenInventoriesPayload[] payload
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback
        );

        /// <summary>
        /// Consume item from user's inventory
        /// </summary>
        /// <param name="inventoryId">Id of inventory to consume items from</param>
        /// <param name="consumedItemsRequest">Data of items to be consumed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with optional parameters instead.")]
        public void ConsumeUserInventoryItem(
            string inventoryId
            , ConsumeUserItemsRequest consumedItemsRequest
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Consume item from user's inventory
        /// </summary>
        /// <param name="inventoryId">Id of inventory to consume items from</param>
        /// <param name="qty">Amount of items to consume</param>
        /// <param name="sourceItemId">Id of item to consume</param>
        /// <param name="optionalParams">Optional parameters. Can be null.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ConsumeUserInventoryItem(
            string inventoryId
            , uint qty
            , string sourceItemId
            , ConsumeUserInventoryItemOptionalParameters optionalParams
            , ResultCallback<UserItem> callback
        );
    }
}