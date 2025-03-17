// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (request == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var payload = new BulkDeleteUserInventoryItemsPayload[request.Length];
            for (int i = 0; i < payload.Length; i++)
            {
                payload[i] = new BulkDeleteUserInventoryItemsPayload(request[i].SourceItemId);
                payload[i].SlotId = request[i].SlotId;
            }

            BulkDeleteUserInventoryItems(inventoryId, userId, payload, callback);
        }

        public void BulkDeleteUserInventoryItems(string inventoryId
            , string userId
            , BulkDeleteUserInventoryItemsPayload[] payload
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkDeleteUserInventoryItems(inventoryId, userId, payload, callback);
        }

        public void BulkUpdateUserInventoryItems(string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemRequest[] request
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (request == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var payload = new ServerUpdateUserInventoryItemPayload[request.Length];
            for (int i = 0; i < payload.Length; i++)
            {
                payload[i] = new ServerUpdateUserInventoryItemPayload(request[i].SourceItemId);
                payload[i].ServerCustomAttributes = request[i].ServerCustomAttributes;
                payload[i].SlotId = request[i].SlotId;
                payload[i].CustomAttributes = request[i].CustomAttributes;
                payload[i].Tags = request[i].Tags;
                payload[i].Type = request[i].Type;
            }

            BulkUpdateUserInventoryItems(inventoryId, userId, payload, callback);
        }

        public void BulkUpdateUserInventoryItems(string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemPayload[] payload
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkUpdateUserInventoryItems(inventoryId, userId, payload, callback);
        }

        public void ConsumeUserInventoryItem(string inventoryId, string userId, ConsumeUserItemsRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (request == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var optionalparams = new ConsumeUserInventoryItemOptionalParameters()
            {
                Options = request.Options,
                SlotId = request.SlotId
            };

            ConsumeUserInventoryItem(inventoryId
                , userId
                , (uint)request.Quantity
                , request.SourceItemId
                , optionalparams
                , callback);
        }

        public void ConsumeUserInventoryItem(string inventoryId
            , string userId
            , uint qty
            , string sourceItemId
            , ConsumeUserInventoryItemOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ConsumeUserItemsRequest()
            {
                Quantity = (int)qty,
                SourceItemId = sourceItemId,
                SlotId = optionalParameters?.SlotId,
                Options = optionalParameters?.Options
            };

            api.ConsumeUserInventoryItem(inventoryId, userId, request, callback);
        }

        public void CreateIntegrationConfiguration(ServerCreateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateIntegrationConfiguration(request, callback);
        }

        public void CreateInventoryConfiguration(ServerCreateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (request == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var optionalParams = new CreateInventoryConfigurationOptionalParameters()
            {
                Description = request.Description,
                Name = request.Description
            };

            CreateInventoryConfiguration(
                request.Code
                , (uint)request.InitialMaxSlots
                , (uint)request.MaxInstancesPerUser
                , (uint)request.MaxUpgradeSlots
                , optionalParams
                , callback);
        }

        public void CreateInventoryConfiguration(string code, uint initialMaxSlots, uint maxInstancesPerUser, uint maxUpgradeSlots, CreateInventoryConfigurationOptionalParameters optionalParameters, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ServerCreateInventoryConfigurationRequest()
            {
                Code = code,
                InitialMaxSlots = (int)initialMaxSlots,
                MaxInstancesPerUser = (int)maxInstancesPerUser,
                MaxUpgradeSlots = (int)maxUpgradeSlots,
                Description = optionalParameters?.Description,
                Name = optionalParameters?.Name
            };

            api.CreateInventoryConfiguration(request, callback);
        }

        public void CreateInventoryTag(ServerCreateInventoryTagRequest request, ResultCallback<InventoryTag> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            CreateInventoryTag(request?.Name, request?.Owner, callback);
        }

        public void CreateInventoryTag(string name, string inventoryOwner, ResultCallback<InventoryTag> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ServerCreateInventoryTagRequest()
            {
                Name = name,
                Owner = inventoryOwner
            };

            api.CreateInventoryTag(request, callback);
        }

        public void CreateItemTypes(ServerCreateItemTypeRequest request, ResultCallback<InventoryItemType> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            CreateItemTypes(request?.Name, callback);
        }

        public void CreateItemTypes(string name, ResultCallback<InventoryItemType> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ServerCreateItemTypeRequest()
            {
                Name = name
            };

            api.CreateItemTypes(request, callback);
        }

        public void CreateUserInventory(ServerCreateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateUserInventory(request, callback);
        }

        public void DeleteInventoryConfiguration(string inventoryConfigurationId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteInventoryConfiguration(inventoryConfigurationId, callback);
        }

        public void DeleteInventoryTag(string tagName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteInventoryTag(tagName, callback);
        }

        public void DeleteItemTypes(string itemTypeName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteItemTypes(itemTypeName, callback);
        }

        public void DeleteUserInventory(string inventoryId, ServerDeleteInventoryRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteUserInventory(inventoryId, request, callback);
        }

        public void GetAllInventoryConfigurations(ResultCallback<InventoryConfigurationsPagingResponse> callback, InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "")
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetAllInventoryConfigurations(callback, sortBy, limit, offset, inventoryConfigurationCode);
        }

        public void GetIntegrationConfigurations(ResultCallback<ServerIntegrationConfigurationsPagingResponse> callback, ServerIntegrationConfigurationSortBy sortBy = ServerIntegrationConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetIntegrationConfigurations(callback, sortBy, limit, offset);
        }

        public void GetInventoryConfiguration(string inventoryConfigurationId, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryConfiguration(inventoryConfigurationId, callback);
        }

        public void GetInventoryTags(ResultCallback<InventoryTagsPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetInventoryTags(callback, sortBy, limit, offset);
        }

        public void GetItemTypes(ResultCallback<ItemTypesPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetItemTypes(callback, sortBy, limit, offset);
        }

        public void GetUserInventories(ResultCallback<UserInventoriesPagingResponse> callback, UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "", string userId = "")
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventories(callback, sortBy, limit, offset, inventoryConfigurationCode, userId);
        }

        public void GetUserInventory(string inventoryId, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryItem(inventoryId, slotId, sourceItemId, callback);
        }

        public void RunChainingOperation(ServerInventoryChainingOperationRequest request, ResultCallback<InventoryChainingOperationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RunChainingOperation(request, callback);
        }

        public void SaveUserInventoryItem(
            string userId
            , string inventoryConfigurationCode
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , ResultCallback<UserItem> callback
        )
        {
            SaveUserInventoryItem(userId: userId
                , inventoryConfigurationCode: inventoryConfigurationCode
                , source: source
                , sourceItemId: sourceItemId
                , type: type
                , quantity: quantity
                , optionalParameters: null
                , callback: callback);
        }

        public void SaveUserInventoryItem(
            string userId
            , string inventoryConfigurationCode
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , SaveUserInventoryItemOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SaveUserInventoryItem(userId: userId
                , inventoryConfigurationCode: inventoryConfigurationCode
                , source: source
                , sourceItemId: sourceItemId
                , type: type
                , quantity: quantity
                , optionalParameters: optionalParameters
                , callback: callback);
        }

        public void SaveUserInventoryItemToInventory(string userId
            , string inventoryId
            , ServerSaveUserInventoryItemRequest request
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (request == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var optionalParams = new SaveUserInventoryItemToInventoryOptionalParameters()
            {
                CustomAttributes = request.CustomAttributes,
                ServerCustomAttributes = request.ServerCustomAttributes,
                SlotId = request.SlotId,
                SlotUsed = request.SlotUsed,
                Tags = request.Tags
            };

            SaveUserInventoryItemToInventory(inventoryId
                , userId
                , request.Source
                , request.SourceItemId
                , request.Type
                , (uint)request.Quantity
                , optionalParams
                , callback);
        }

        public void SaveUserInventoryItemToInventory(string inventoryId
            , string userId
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , SaveUserInventoryItemToInventoryOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ServerSaveUserInventoryItemRequest()
            {
                Source = source,
                SourceItemId = sourceItemId,
                Type = type,
                Quantity = (int)quantity,
                CustomAttributes = optionalParameters?.CustomAttributes,
                ServerCustomAttributes = optionalParameters?.ServerCustomAttributes,
                SlotId = optionalParameters?.SlotId,
                SlotUsed = (optionalParameters != null && optionalParameters.SlotUsed.HasValue) 
                    ? (int)optionalParameters?.SlotUsed : 0,
                Tags = optionalParameters?.Tags
            };

            api.SaveUserInventoryItemToInventory(inventoryId, userId, request, callback);
        }

        public void SyncUserEntitlements(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SyncUserEntitlements(userId, callback);
        }

        public void UpdateIntegrationConfiguration(string integrationConfigurationId, ServerUpdateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateIntegrationConfiguration(integrationConfigurationId, request, callback);
        }

        public void UpdateIntegrationConfigurationStatus(string integrationConfigurationId, ServerUpdateIntegrationConfigurationStatusRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateIntegrationConfigurationStatus(integrationConfigurationId, request, callback);
        }

        public void UpdateInventoryConfiguration(string inventoryConfigurationId, ServerUpdateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateInventoryConfiguration(inventoryConfigurationId, request, callback);
        }

        public void UpdateUserInventory(string inventoryId, ServerUpdateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateUserInventory(inventoryId, request, callback);
        }

        public void ValidateUserInventoryCapacity(string userId, ServerValidateUserInventoryCapacityRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ValidateUserInventoryCapacity(userId, request, callback);
        }
    }
}