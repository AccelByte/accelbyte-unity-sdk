// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

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
