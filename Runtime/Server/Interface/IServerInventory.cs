// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
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

        public void GetUserInventories(
            ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
            , string userId = ""
        );

        public void CreateUserInventory(
            ServerCreateInventoryRequest request
            , ResultCallback<UserInventory> callback
        );

        public void GetUserInventory(
            string inventoryId
            , ResultCallback<UserInventory> callback
        );

        public void UpdateUserInventory(
            string inventoryId
            , ServerUpdateInventoryRequest request
            , ResultCallback<UserInventory> callback
        );

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
        /// List all user's Item in an inventory
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        /// <param name="sortBy">Optional parameter to get the result sortedBy</param>
        /// <param name="limit">Optional parameter to define limit for the result length</param>
        /// <param name="offset">Optional parameter to define offset for the result</param>
        /// <param name="sourceItemId">Optional parameter to query based on a specific source item id</param>
        /// <param name="tagBuilder">Optional parameter to query based on a tagBuilder</param>
        /// <param name="quantity">Optional parameter to query based on specific quantity</param>
        [Obsolete("Quantity parameter is no longer supported. Please use GetUserInventoryAllItems(inventoryId, optionalParameters, callback). This interface will be removed on AGS 3.81")]
        public void GetUserInventoryAllItems(
            string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback
            , UserItemSortBy sortBy = UserItemSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string sourceItemId = ""
            , TagQueryBuilder tagBuilder = null
            , int? quantity = null
        );

        /// <summary>
        /// Listing all items in an inventory.
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        public void GetUserInventoryAllItems(string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback);

        /// <summary>
        /// Listing all items in an inventory.
        /// </summary>
        /// <param name="inventoryId">Selected inventory Id</param>
        /// <param name="optionalParameters">Optional parameter to get specific result</param>
        /// <param name="callback">Returns a Result that contains UserItemsPagingResponse via callback when completed</param>
        public void GetUserInventoryAllItems(string inventoryId
            , GetUserInventoryAllItemsOptionalParameters optionalParameters
            , ResultCallback<UserItemsPagingResponse> callback);

        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        );

        public void ConsumeUserInventoryItem(
            string inventoryId
            , string userId
            , ConsumeUserItemsRequest request
            , ResultCallback<UserItem> callback
        );

        public void BulkUpdateUserInventoryItems(
            string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemRequest[] request
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        public void SaveUserInventoryItemToInventory(
            string inventoryId
            , string userId
            , ServerSaveUserInventoryItemRequest request
            , ResultCallback<UserItem> callback
        );

        public void BulkDeleteUserInventoryItems(
            string inventoryId
            , string userId
            , DeleteUserInventoryItemRequest[] request
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        public void SaveUserInventoryItem(
            string userId
            , ServerSaveUserInventoryItemRequest request
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
