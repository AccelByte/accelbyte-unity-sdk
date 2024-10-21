// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;
using System;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide an API to access Inventory service.
    /// </summary>
    public class ServerInventory : WrapperBase, IServerInventory
    {
        private readonly ServerInventoryApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerInventory(ServerInventoryApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(inApi, "Cannot construct Inventory manager; api is null!");

            api = inApi;
            session = inSession;
        }

        public void BulkDeleteUserInventoryItems(string inventoryId, string userId, DeleteUserInventoryItemRequest[] request, ResultCallback<DeleteUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkDeleteUserInventoryItems(inventoryId, userId, request, callback);
        }

        public void BulkUpdateUserInventoryItems(string inventoryId, string userId, ServerUpdateUserInventoryItemRequest[] request, ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkUpdateUserInventoryItems(inventoryId, userId, request, callback);
        }

        public void ConsumeUserInventoryItem(string inventoryId, string userId, ConsumeUserItemsRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ConsumeUserInventoryItem(inventoryId, userId, request, callback);
        }

        public void CreateIntegrationConfiguration(ServerCreateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateIntegrationConfiguration(request, callback);
        }

        public void CreateInventoryConfiguration(ServerCreateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateInventoryConfiguration(request, callback);
        }

        public void CreateInventoryTag(ServerCreateInventoryTagRequest request, ResultCallback<InventoryTag> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateInventoryTag(request, callback);
        }

        public void CreateItemTypes(ServerCreateItemTypeRequest request, ResultCallback<InventoryItemType> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateItemTypes(request, callback);
        }

        public void CreateUserInventory(ServerCreateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateUserInventory(request, callback);
        }

        public void DeleteInventoryConfiguration(string inventoryConfigurationId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteInventoryConfiguration(inventoryConfigurationId, callback);
        }

        public void DeleteInventoryTag(string tagName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteInventoryTag(tagName, callback);
        }

        public void DeleteItemTypes(string itemTypeName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteItemTypes(itemTypeName, callback);
        }

        public void DeleteUserInventory(string inventoryId, ServerDeleteInventoryRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteUserInventory(inventoryId, request, callback);
        }

        public void GetAllInventoryConfigurations(ResultCallback<InventoryConfigurationsPagingResponse> callback, InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetAllInventoryConfigurations(callback, sortBy, limit, offset, inventoryConfigurationCode);
        }

        public void GetIntegrationConfigurations(ResultCallback<ServerIntegrationConfigurationsPagingResponse> callback, ServerIntegrationConfigurationSortBy sortBy = ServerIntegrationConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetIntegrationConfigurations(callback, sortBy, limit, offset);
        }

        public void GetInventoryConfiguration(string inventoryConfigurationId, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryConfiguration(inventoryConfigurationId, callback);
        }

        public void GetInventoryTags(ResultCallback<InventoryTagsPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryTags(callback, sortBy, limit, offset);
        }

        public void GetItemTypes(ResultCallback<ItemTypesPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetItemTypes(callback, sortBy, limit, offset);
        }

        public void GetUserInventories(ResultCallback<UserInventoriesPagingResponse> callback, UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "", string userId = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventories(callback, sortBy, limit, offset, inventoryConfigurationCode, userId);
        }

        public void GetUserInventory(string inventoryId, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventory(inventoryId, callback);
        }

        public void GetUserInventoryAllItems(string inventoryId, ResultCallback<UserItemsPagingResponse> callback)
        {
            GetUserInventoryAllItems(inventoryId, null, callback);
        }
        
        public void GetUserInventoryAllItems(string inventoryId, GetUserInventoryAllItemsOptionalParameters optionalParam, ResultCallback<UserItemsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryAllItems(inventoryId, optionalParam, callback);
        }
        
        public void GetUserInventoryAllItems(string inventoryId, ResultCallback<UserItemsPagingResponse> callback, UserItemSortBy sortBy = UserItemSortBy.CreatedAt, int limit = 25, int offset = 0, string sourceItemId = "", TagQueryBuilder tagBuilder = null, int? quantity = null)
        {
            GetUserInventoryAllItemsOptionalParameters optionalParameters = new GetUserInventoryAllItemsOptionalParameters()
            {
                SortBy = sortBy,
                Limit = limit,
                Offset = offset,
                SourceItemId = sourceItemId,
                TagQueryBuilder = tagBuilder
            };
            
            GetUserInventoryAllItems(inventoryId, optionalParameters, callback);
        }

        public void GetUserInventoryItem(string inventoryId, string slotId, string sourceItemId, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryItem(inventoryId, slotId, sourceItemId, callback);
        }

        public void RunChainingOperation(ServerInventoryChainingOperationRequest request, ResultCallback<InventoryChainingOperationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RunChainingOperation(request, callback);
        }

        public void SaveUserInventoryItem(string userId, ServerSaveUserInventoryItemRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SaveUserInventoryItem(userId, request, callback);
        }

        public void SaveUserInventoryItemToInventory(string userId, string inventoryId, ServerSaveUserInventoryItemRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SaveUserInventoryItemToInventory(userId, inventoryId, request, callback);
        }

        public void SyncUserEntitlements(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SyncUserEntitlements(userId, callback);
        }

        public void UpdateIntegrationConfiguration(string integrationConfigurationId, ServerUpdateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateIntegrationConfiguration(integrationConfigurationId, request, callback);
        }

        public void UpdateIntegrationConfigurationStatus(string integrationConfigurationId, ServerUpdateIntegrationConfigurationStatusRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateIntegrationConfigurationStatus(integrationConfigurationId, request, callback);
        }

        public void UpdateInventoryConfiguration(string inventoryConfigurationId, ServerUpdateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateInventoryConfiguration(inventoryConfigurationId, request, callback);
        }

        public void UpdateUserInventory(string inventoryId, ServerUpdateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateUserInventory(inventoryId, request, callback);
        }

        public void ValidateUserInventoryCapacity(string userId, ServerValidateUserInventoryCapacityRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ValidateUserInventoryCapacity(userId, request, callback);
        }
    }
}