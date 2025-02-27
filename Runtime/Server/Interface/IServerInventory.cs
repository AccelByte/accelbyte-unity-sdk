// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Server.Interface
{
    public interface IServerInventory
    {
        public void GetIntegrationConfigurations(
            ResultCallback<ServerIntegrationConfigurationsPagingResponse> callback
            , ServerIntegrationConfigurationSortBy sortBy = ServerIntegrationConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        public void CreateIntegrationConfiguration(
            ServerCreateIntegrationConfigurationRequest request
            , ResultCallback<ServerIntegrationConfiguration> callback
        );

        public void UpdateIntegrationConfiguration(
            string integrationConfigurationId
            , ServerUpdateIntegrationConfigurationRequest request
            , ResultCallback<ServerIntegrationConfiguration> callback
        );

        public void UpdateIntegrationConfigurationStatus(
            string integrationConfigurationId
            , ServerUpdateIntegrationConfigurationStatusRequest request
            , ResultCallback<ServerIntegrationConfiguration> callback
        );

        /// <summary>
        /// Retrieve list of user inventories
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="sortBy">Optional parameter to set inventory list order</param>
        /// <param name="limit">Optional parameter to define limit for the result length</param>
        /// <param name="offset">Optional parameter to define offset for the result</param>
        /// <param name="inventoryConfigurationCode"> Optional parameter to query based on inventory configuration code</param>
        /// <param name="userId">Optional parameter to query based on a specific userId</param>
        public void GetUserInventories(
            ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
            , string userId = ""
        );

        /// <summary>
        /// Grant inventory to the player
        /// </summary>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CreateUserInventory(
            ServerCreateInventoryRequest request
            , ResultCallback<UserInventory> callback
        );

        public void GetUserInventory(
            string inventoryId
            , ResultCallback<UserInventory> callback
        );

        /// <summary>
        /// Update user inventory slot limit
        /// </summary>
        /// <param name="inventoryId">Inventory Id to be processed</param>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UpdateUserInventory(
            string inventoryId
            , ServerUpdateInventoryRequest request
            , ResultCallback<UserInventory> callback
        );

        /// <summary>
        /// Delete user inventory based on inventory id
        /// </summary>
        /// <param name="inventoryId">Inventory Id to be processed</param>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void DeleteUserInventory(
            string inventoryId
            , ServerDeleteInventoryRequest request
            , ResultCallback callback
        );

        public void ValidateUserInventoryCapacity(
            string userId
            , ServerValidateUserInventoryCapacityRequest request
            , ResultCallback callback
        );

        /// <summary>
        /// Retrieve a list of user inventories
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetUserInventoryAllItems(string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback);

        /// <summary>
        /// Retrieve a list of user inventories
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="optionalParameters">Optional parameter to get specific result</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        public void GetUserInventoryAllItems(string inventoryId
            , GetUserInventoryAllItemsOptionalParameters optionalParameters
            , ResultCallback<UserItemsPagingResponse> callback);

        /// <summary>
        /// Listing all items in an inventory.
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="slotId">slot id to be fetch</param>
        /// <param name="sourceItemId">item id to fetch</param>
        /// <param name="callback">Returns a Result that contains UserItem via callback when completed</param>
        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Make a user consume item
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">User id to be processed</param>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ConsumeUserInventoryItem(
            string inventoryId
            , string userId
            , ConsumeUserItemsRequest request
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Update item attributes and tags for a specidic user
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">User id to be processed</param>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkUpdateUserInventoryItems(
            string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemRequest[] request
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Add item to player inventory using inventoryId
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="request">request body contains the item detail</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserInventoryItemToInventory(
            string inventoryId
            , string userId
            , ServerSaveUserInventoryItemRequest request
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Remove items from player's inventory
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="request">request body contains the item detail</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkDeleteUserInventoryItems(
            string inventoryId
            , string userId
            , DeleteUserInventoryItemRequest[] request
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Add item to player inventory using inventoryConfigurationCode
        /// </summary>
        /// <param name="userId">user to be process</param>
        /// <param name="inventoryConfigurationCode">inventory config code to be processed</param>
        /// <param name="source">item source to be processed</param>
        /// <param name="sourceItemId">item source id to be processed</param>
        /// <param name="type">item type to be processed</param>
        /// <param name="quantity">item quantity to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserInventoryItem(
            string userId
            , string inventoryConfigurationCode
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Add item to player inventory using inventoryConfigurationCode
        /// </summary>
        /// <param name="userId">user to be process</param>
        /// <param name="inventoryConfigurationCode">inventory config code to be processed</param>
        /// <param name="source">item source to be processed</param>
        /// <param name="sourceItemId">item source id to be processed</param>
        /// <param name="type">item type to be processed</param>
        /// <param name="quantity">item quantity to be processed</param>
        /// <param name="optionalParameters">optional parameters to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserInventoryItem(
            string userId
            , string inventoryConfigurationCode
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , SaveUserInventoryItemOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback
        );

        public void SyncUserEntitlements(
            string userId
            , ResultCallback callback
        );

        public void GetAllInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        public void CreateInventoryConfiguration(
            ServerCreateInventoryConfigurationRequest request
            , ResultCallback<InventoryConfiguration> callback
        );

        public void GetInventoryConfiguration(
            string inventoryConfigurationId
            , ResultCallback<InventoryConfiguration> callback
        );

        public void UpdateInventoryConfiguration(
            string inventoryConfigurationId
            , ServerUpdateInventoryConfigurationRequest request
            , ResultCallback<InventoryConfiguration> callback
        );

        public void DeleteInventoryConfiguration(
            string inventoryConfigurationId
            , ResultCallback callback
        );

        public void GetItemTypes(
            ResultCallback<ItemTypesPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        public void CreateItemTypes(
            ServerCreateItemTypeRequest request,
            ResultCallback<InventoryItemType> callback
        );

        public void DeleteItemTypes(
            string itemTypeName
            , ResultCallback callback
        );

        public void GetInventoryTags(
            ResultCallback<InventoryTagsPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        public void CreateInventoryTag(
            ServerCreateInventoryTagRequest request
            , ResultCallback<InventoryTag> callback
        );

        public void DeleteInventoryTag(
            string tagName
            , ResultCallback callback
        );

        public void RunChainingOperation(
            ServerInventoryChainingOperationRequest request
            , ResultCallback<InventoryChainingOperationResponse> callback
        );
    }
}
