// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Api.Interface;
using System;
using UnityEngine.Assertions;

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (deletedItemsRequest == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var payload = new BulkDeleteUserInventoryItemsPayload[deletedItemsRequest.Length];
            for (int i = 0; i < payload.Length; i++)
            {
                if (deletedItemsRequest[i] == null)
                {
                    continue;
                };

                payload[i] = new BulkDeleteUserInventoryItemsPayload(deletedItemsRequest[i].SourceItemId);
                payload[i].SlotId = deletedItemsRequest[i].SlotId;
            }

            BulkDeleteInventoryItems(inventoryId, payload, callback);
        }

        public void BulkDeleteInventoryItems(string inventoryId
            , BulkDeleteUserInventoryItemsPayload[] payload
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkDeleteInventoryItems(inventoryId, payload, callback);
        }

        public void BulkUpdateInventoryItems(
            string inventoryId
            , UpdateUserInventoryItemRequest[] updatedItemsRequest
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (updatedItemsRequest == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var payload = new BulkUpdateInventoryItemsPayload[updatedItemsRequest.Length];
            for (int i = 0; i < payload.Length; i++)
            {
                if (updatedItemsRequest[i] == null)
                {
                    continue;
                };

                payload[i] = new BulkUpdateInventoryItemsPayload(updatedItemsRequest[i].SourceItemId);
                payload[i].SlotId = updatedItemsRequest[i].SlotId;
                payload[i].CustomAttributes = updatedItemsRequest[i].CustomAttributes;
                payload[i].Tags = updatedItemsRequest[i].Tags;
            }

            BulkUpdateInventoryItems(inventoryId, payload, callback);
        }

        public void BulkUpdateInventoryItems(string inventoryId
            , BulkUpdateInventoryItemsPayload[] payload
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkUpdateInventoryItems(inventoryId, payload, callback);
        }

        public void ConsumeUserInventoryItem(
            string inventoryId
            , ConsumeUserItemsRequest consumedItemsRequest
            , ResultCallback<UserItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (consumedItemsRequest == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var optionalParams = new ConsumeUserInventoryItemOptionalParameters()
            {
                Options = consumedItemsRequest.Options,
                SlotId = consumedItemsRequest.SlotId
            };

            ConsumeUserInventoryItem(inventoryId
                , (uint)consumedItemsRequest.Quantity
                , consumedItemsRequest.SourceItemId
                , optionalParams
                , callback);
        }

        public void ConsumeUserInventoryItem(string inventoryId
            , uint qty
            , string sourceItemId
            , ConsumeUserInventoryItemOptionalParameters optionalParams
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ConsumeUserItemsRequest
            {
                Quantity = (int)qty,
                SourceItemId = sourceItemId,
                SlotId = optionalParams?.SlotId,
                Options = optionalParams?.Options
            };

            api.ConsumeUserInventoryItem(inventoryId, request, optionalParams, callback);
        }

        public void GetInventoryConfigurations(
            ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = ""
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventories(callback, sortBy, limit, offset, inventoryConfigurationCode);
        }

        public void GetUserInventoryAllItems(
            string inventoryId
            , ResultCallback<UserItemsPagingResponse> callback
        )
        {
            GetUserInventoryAllItems(inventoryId, null, callback);
        }
        
        public void GetUserInventoryAllItems(
            string inventoryId
            , GetUserInventoryAllItemsOptionalParameters optionalParameters
            , ResultCallback<UserItemsPagingResponse> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserInventoryAllItems(inventoryId
                , optionalParameters
                , callback);
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

        public void GetUserInventoryItem(
            string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (moveItemsRequest?.items == null)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            var payload = new MoveUserItemsBetweenInventoriesPayload[moveItemsRequest.items.Length];
            for(int i = 0; i < payload.Length; i++)
            {
                payload[i] = new MoveUserItemsBetweenInventoriesPayload(
                    (uint)moveItemsRequest.items[i].Quantity
                    , moveItemsRequest.items[i].SlotId
                    , moveItemsRequest.items[i].SourceItemId);
            }

            MoveItemsBetweenInventories(targetInventoryId, moveItemsRequest.SourceInventoryId, payload, callback);
        }

        public void MoveItemsBetweenInventories(string targetInventoryId
            , string sourceInventoryId
            , MoveUserItemsBetweenInventoriesPayload[] payload
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.MoveItemsBetweenInventories(targetInventoryId, sourceInventoryId, payload, callback);
        }
    }
}