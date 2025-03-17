// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server.Interface
{
    public interface IServerInventory
    {
        /// <summary>
        /// Retrieves all available integration configurations on the current namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result data by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
        public void GetIntegrationConfigurations(
            ResultCallback<ServerIntegrationConfigurationsPagingResponse> callback
            , ServerIntegrationConfigurationSortBy sortBy = ServerIntegrationConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
        );

        /// <summary>
        /// Creates a new integration configuration on the current namespace.
        /// </summary>
        /// <param name="request">Data of integration configuration to create</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CreateIntegrationConfiguration(
            ServerCreateIntegrationConfigurationRequest request
            , ResultCallback<ServerIntegrationConfiguration> callback
        );

        /// <summary>
        /// Updates an integration configuration via its id.
        /// </summary>
        /// <param name="integrationConfigurationId">Id of integration configuration to update</param>
        /// <param name="request">Data of integration configuration to update</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UpdateIntegrationConfiguration(
            string integrationConfigurationId
            , ServerUpdateIntegrationConfigurationRequest request
            , ResultCallback<ServerIntegrationConfiguration> callback
        );

        /// <summary>
        /// Updates an integration configuration's status via its id.
        /// </summary>
        /// <param name="integrationConfigurationId">Id of integration configuration to update</param>
        /// <param name="request">Data of integration configuration status to update</param>
        /// <param name="callback">Returns a result via callback when completed</param>
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

        /// <summary>
        /// Validates the capacity of a user's inventory
        /// </summary>
        /// <param name="userId">Id of user to validate inventory capacity for</param>
        /// <param name="request">Request data of inventory to validate</param>
        /// <param name="callback">Returns a result via callback when completed</param>
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
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with optional parameters instead.")]
        public void ConsumeUserInventoryItem(
            string inventoryId
            , string userId
            , ConsumeUserItemsRequest request
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Make a user consume item
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">User id to be processed</param>
        /// <param name="qty">Amount of item to consume</param>
        /// <param name="sourceItemId">Item id to consume</param>
        /// <param name="optionalParameters">Optional parameters. Can be null.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ConsumeUserInventoryItem(
            string inventoryId
            , string userId
            , uint qty
            , string sourceItemId
            , ConsumeUserInventoryItemOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Update item attributes and tags for a specidic user
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">User id to be processed</param>
        /// <param name="request">Detailed data to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with payload parameter instead.")]
        public void BulkUpdateUserInventoryItems(
            string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemRequest[] request
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Update item attributes and tags for a specidic user
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">User id to be processed</param>
        /// <param name="payload">Detailed data to be processed</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkUpdateUserInventoryItems(
            string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemPayload[] payload
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Add item to player inventory using inventoryId
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="request">request body contains the item detail</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with optional parameters instead.")]
        public void SaveUserInventoryItemToInventory(
            string inventoryId
            , string userId
            , ServerSaveUserInventoryItemRequest request
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Add item to player inventory using inventoryId
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="source">Source of item to be added, can be ECOMMERCE or OTHER</param>
        /// <param name="sourceItemId">Item id of item to be added</param>
        /// <param name="type">Type of item to be added</param>
        /// <param name="quantity">Quantity of item to be added</param>
        /// <param name="optionalParameters">Optional parameters. Can be null.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserInventoryItemToInventory(
            string inventoryId
            , string userId
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , SaveUserInventoryItemToInventoryOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback
        );

        /// <summary>
        /// Remove items from player's inventory
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="request">request body contains the item detail</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with payload parameter instead.")]
        public void BulkDeleteUserInventoryItems(
            string inventoryId
            , string userId
            , DeleteUserInventoryItemRequest[] request
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback
        );

        /// <summary>
        /// Remove items from player's inventory
        /// </summary>
        /// <param name="inventoryId">InventoryId to be process</param>
        /// <param name="userId">user to be process</param>
        /// <param name="payload">request body contains the item detail</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void BulkDeleteUserInventoryItems(
            string inventoryId
            , string userId
            , BulkDeleteUserInventoryItemsPayload[] payload
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

        /// <summary>
        /// Synchronizes user entitlement data to Inventory service
        /// </summary>
        /// <param name="userId">Id of user to sync data for</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void SyncUserEntitlements(
            string userId
            , ResultCallback callback
        );

        /// <summary>
        /// Retrieves all inventory configurations on the namespace.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="sortBy">Optional parameter to sort result items by</param>
        /// <param name="limit">Optional parameter to limit amount of items returned in a page</param>
        /// <param name="offset">Optional parameter to traverse pagination by amount of items</param>
        /// <param name="inventoryConfigurationCode">Optional parameter to filter result items by inventory configuration code</param>
        public void GetAllInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        );

        /// <summary>
        /// Creates a new inventory configuration for the current namespace.
        /// </summary>
        /// <param name="request">Inventory configuration request data</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with optional parameters instead.")]
        public void CreateInventoryConfiguration(
            ServerCreateInventoryConfigurationRequest request
            , ResultCallback<InventoryConfiguration> callback
        );

        /// <summary>
        /// Creates a new inventory configuration for the current namespace.
        /// </summary>
        /// <param name="code">Configuration code for the new inventory configuration</param>
        /// <param name="initialMaxSlots">Starting slots when an inventory is created using the configuration</param>
        /// <param name="maxInstancesPerUser">Maximum instances of the inventory type per user</param>
        /// <param name="maxUpgradeSlots">Maximum slots of the inventory when fully upgraded</param>
        /// <param name="optionalParameters">Optional parameters. Can be null.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CreateInventoryConfiguration(
            string code
            , uint initialMaxSlots
            , uint maxInstancesPerUser
            , uint maxUpgradeSlots
            , CreateInventoryConfigurationOptionalParameters optionalParameters
            , ResultCallback<InventoryConfiguration> callback
        );

        /// <summary>
        /// Retrieves inventory configuration data via its configuration Id.
        /// </summary>
        /// <param name="inventoryConfigurationId">Id of inventory configuration to fetch data for</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetInventoryConfiguration(
            string inventoryConfigurationId
            , ResultCallback<InventoryConfiguration> callback
        );

        /// <summary>
        /// Updates inventory configuration data via its configuration Id.
        /// </summary>
        /// <param name="inventoryConfigurationId">Id of inventory configuration to update</param>
        /// <param name="request">Inventory configuration data to update</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UpdateInventoryConfiguration(
            string inventoryConfigurationId
            , ServerUpdateInventoryConfigurationRequest request
            , ResultCallback<InventoryConfiguration> callback
        );

        /// <summary>
        /// Deletes inventory configuration data via its configuration Id.
        /// </summary>
        /// <param name="inventoryConfigurationId">Id of inventory configuration to delete</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void DeleteInventoryConfiguration(
            string inventoryConfigurationId
            , ResultCallback callback
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
        /// Create a new item type on the current namespace.
        /// </summary>
        /// <param name="request">Data of item type to create.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with string parameter instead.")]
        public void CreateItemTypes(
            ServerCreateItemTypeRequest request,
            ResultCallback<InventoryItemType> callback
        );

        /// <summary>
        /// Create a new item type on the current namespace.
        /// </summary>
        /// <param name="name">Name of item type to create.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CreateItemTypes(
            string name
            , ResultCallback<InventoryItemType> callback
        );

        /// <summary>
        /// Delete an item type on the current namespace via its name.
        /// </summary>
        /// <param name="itemTypeName">Name of item type to delete.</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void DeleteItemTypes(
            string itemTypeName
            , ResultCallback callback
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
        /// Create an inventory tag on the current namespace
        /// </summary>
        /// <param name="request">Data of tag to create</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        [System.Obsolete("This method is deprecated and will be removed on 2025.5.AGS. " +
            "Please use overload with string parameters instead.")]
        public void CreateInventoryTag(
            ServerCreateInventoryTagRequest request
            , ResultCallback<InventoryTag> callback
        );

        /// <summary>
        /// Create an inventory tag on the current namespace
        /// </summary>
        /// <param name="name">Name of the inventory tag to create</param>
        /// <param name="inventoryOwner">Owner of inventory tag to create</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CreateInventoryTag(
            string name
            , string inventoryOwner
            , ResultCallback<InventoryTag> callback
        );

        /// <summary>
        /// Delete an inventory tag on the current namespace via its name
        /// </summary>
        /// <param name="tagName">Inventory tag name to delete</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void DeleteInventoryTag(
            string tagName
            , ResultCallback callback
        );

        /// <summary>
        /// Run a series of inventory operations as defined on request
        /// </summary>
        /// <param name="request">Operations to run, sequentially by index</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void RunChainingOperation(
            ServerInventoryChainingOperationRequest request
            , ResultCallback<InventoryChainingOperationResponse> callback
        );
    }
}
