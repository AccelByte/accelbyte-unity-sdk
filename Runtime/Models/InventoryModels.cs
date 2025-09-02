// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum InventoryConfigurationStatus
    {
        [System.ComponentModel.Description("INIT"), EnumMember(Value = "INIT")]
        Init,
        [System.ComponentModel.Description("TIED"), EnumMember(Value = "TIED")]
        Tied
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum InventoryConfigurationSortBy
    {
        [System.ComponentModel.Description("createdAt")]
        CreatedAt,
        [System.ComponentModel.Description("createdAt:asc")]
        CreatedAtAsc,
        [System.ComponentModel.Description("createdAt:desc")]
        CreatedAtDesc,
        [System.ComponentModel.Description("updatedAt")]
        UpdatedAt,
        [System.ComponentModel.Description("updatedAt:asc")]
        UpdatedAtAsc,
        [System.ComponentModel.Description("updatedAt:desc")]
        UpdatedAtDesc,
        [System.ComponentModel.Description("code")]
        Code,
        [System.ComponentModel.Description("code:asc")]
        CodeAsc,
        [System.ComponentModel.Description("code:desc")]
        CodeDesc,
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum UserInventorySortBy
    {
        [System.ComponentModel.Description("createdAt")]
        CreatedAt,
        [System.ComponentModel.Description("createdAt:asc")]
        CreatedAtAsc,
        [System.ComponentModel.Description("createdAt:desc")]
        CreatedAtDesc,
        [System.ComponentModel.Description("updatedAt")]
        UpdatedAt,
        [System.ComponentModel.Description("updatedAt:asc")]
        UpdatedAtAsc,
        [System.ComponentModel.Description("updatedAt:desc")]
        UpdatedAtDesc
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ItemTypeSortBy
    {
        [System.ComponentModel.Description("createdAt")]
        CreatedAt,
        [System.ComponentModel.Description("createdAt:asc")]
        CreatedAtAsc,
        [System.ComponentModel.Description("createdAt:desc")]
        CreatedAtDesc,
        [System.ComponentModel.Description("name")]
        Name,
        [System.ComponentModel.Description("name:asc")]
        NameAsc,
        [System.ComponentModel.Description("name:desc")]
        NameDesc
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum UserItemSortBy
    {
        [System.ComponentModel.Description("createdAt")]
        CreatedAt,
        [System.ComponentModel.Description("createdAt:asc")]
        CreatedAtAsc,
        [System.ComponentModel.Description("createdAt:desc")]
        CreatedAtDesc,
        [System.ComponentModel.Description("updatedAt")]
        UpdatedAt,
        [System.ComponentModel.Description("updatedAt:asc")]
        UpdatedAtAsc,
        [System.ComponentModel.Description("updatedAt:desc")]
        UpdatedAtDesc
    }

    [DataContract, Preserve]
    public class InventoryConfigurationsPagingResponse
    {
        [DataMember(Name = "data")] public InventoryConfiguration[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class InventoryConfiguration
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "createdAt")] public DateTime createdAt;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "initialMaxSlots")] public int InitialMaxSlots;
        [DataMember(Name = "maxInstancesPerUser")] public int MaxInstancesPerUser;
        [DataMember(Name = "maxUpgradeSlots")] public int MaxUpgradeSlots;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "status")] public InventoryConfigurationStatus Status;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class InventoryItemType
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
    }

    [DataContract, Preserve]
    public class ItemTypesPagingResponse
    {
        [DataMember(Name = "data")] public InventoryItemType[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class InventoryTagBase
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "owner")] public string Owner;
    }

    [DataContract, Preserve]
    public class InventoryTag : InventoryTagBase
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
    }

    [DataContract, Preserve]
    public class InventoryTagsPagingResponse
    {
        [DataMember(Name = "data")] public InventoryTag[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UserInventory
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "inventoryConfigurationCode")] public string InventoryConfigurationCode;
        [DataMember(Name = "inventoryConfigurationId")] public string InventoryConfigurationId;
        [DataMember(Name = "maxSlots")] public int MaxSlots;
        [DataMember(Name = "maxUpgradeSlots")] public int MaxUpgradeSlots;
        [DataMember(Name = "usedCountSlots")] public int UsedCountSlots;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class UserInventoriesPagingResponse
    {
        [DataMember(Name = "data")] public UserInventory[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UserItemBase
    {
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "slotUsed")] public int SlotUsed;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
        [DataMember(Name = "serverCustomAttributes")] public Dictionary<string, object> ServerCustomAttributes;
    }

    [DataContract, Preserve]
    public class UserItemWithSourceBase : UserItemBase
    {
        [DataMember(Name = "source")] public string Source;
    }

    [DataContract, Preserve]
    public class UserItem : UserItemWithSourceBase
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "inventoryId")] public string InventoryId;
        [DataMember(Name = "itemInfo")] public Dictionary<string, object> ItemInfo;
        [DataMember(Name = "platformAvailable")] public bool IsPlatformAvailable;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
    }
    
    [Preserve]
    public class GetUserInventoryAllItemsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Optional parameter to get the result sortedBy 
        /// </summary>
        public UserItemSortBy SortBy = UserItemSortBy.CreatedAt;
        /// <summary>
        /// Optional parameter to define limit for the result length
        /// </summary>
        public int Limit = 25;
        /// <summary>
        /// Optional parameter to define offset for the result
        /// </summary>
        public int Offset = 0;
        /// <summary>
        /// Optional parameter to query based on a specific source item id
        /// </summary>
        public string SourceItemId;
        /// <summary>
        /// Optional parameter to query based on a tagBuilder
        /// We provide query builder helper to ease build the query using AND, OR statement
        /// </summary>
        public TagQueryBuilder TagQueryBuilder;
        /// <summary>
        /// Optional parameter to query based on a tag
        /// </summary>
        public string Tags;
    }

    [DataContract, Preserve]
    public class UserItemsPagingResponse
    {
        [DataMember(Name = "data")] public UserItem[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UserItemRequestBase
    {
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class UpdateUserInventoryItemRequest : UserItemRequestBase
    {
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
    }

    [DataContract, Preserve]
    public class BulkUpdateInventoryItemsPayload
    {
        public BulkUpdateInventoryItemsPayload(string sourceItemId)
        {
            SourceItemId = sourceItemId;
        }

        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class UserItemResponseBase
    {
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
        [DataMember(Name = "success")] public bool IsSuccess;
        [DataMember(Name = "errorDetails")] public Error ErrorDetails;
    }

    [DataContract, Preserve]
    public class UpdateUserInventoryItemResponse : UserItemResponseBase
    {
    }

    [DataContract, Preserve]
    public class DeleteUserInventoryItemRequest : UserItemRequestBase
    {
    }

    [DataContract, Preserve]
    public class BulkDeleteUserInventoryItemsPayload
    {
        public BulkDeleteUserInventoryItemsPayload(string sourceItemId)
        {
            SourceItemId = sourceItemId;
        }

        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class DeleteUserInventoryItemResponse : UserItemResponseBase
    {
    }

    [DataContract, Preserve]
    public class InventoryOperationBase : UserItemRequestBase
    {
        [DataMember(Name = "qty")] public int Quantity;
    }

    [DataContract, Preserve]
    public class MoveUserItemRequestData : InventoryOperationBase
    {
    }

    [DataContract, Preserve]
    public class MoveUserItemResponseData
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class MoveUserItemsBetweenInventoriesRequest
    {
        public MoveUserItemsBetweenInventoriesRequest()
        {
        }

        internal MoveUserItemsBetweenInventoriesRequest(MoveUserItemsBetweenInventoriesPayload[] payload, string srcInventoryId)
        {
            items = new MoveUserItemRequestData[payload.Length];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new MoveUserItemRequestData()
                {
                    Quantity = payload[i].Quantity,
                    SlotId = payload[i].SlotId,
                    SourceItemId = payload[i].SourceItemId
                };
            }

            SourceInventoryId = srcInventoryId;
        }

        [DataMember(Name = "items")] public MoveUserItemRequestData[] items;
        [DataMember(Name = "srcInventoryId")] public string SourceInventoryId;
    }

    [DataContract, Preserve]
    public class MoveUserItemsBetweenInventoriesPayload
    {
        public MoveUserItemsBetweenInventoriesPayload(uint qty, string slotId, string sourceItemId)
        {
            Quantity = (int)qty;
            SlotId = slotId;
            SourceItemId = sourceItemId;
        }

        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class MoveUserItemsBetweenInventoriesResponse
    {
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "dstInventoryId")] public string DestinationInventoryId;
        [DataMember(Name = "srcInventoryId")] public string SourceInventoryId;
        [DataMember(Name = "items")] public MoveUserItemResponseData[] Items;
        [DataMember(Name = "namespace")] public string Namespace;
    }

    /// <summary>
    /// Optional parameters for ConsumeUserInventoryItem. Can be null.
    /// </summary>
    public class ConsumeUserInventoryItemOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Only available when item type is OPTIONBOX, values should be item ID.
        /// </summary>
        public string[] Options = null;

        /// <summary>
        /// Slot Id being occupied by the item to consume.
        /// </summary>
        public string SlotId = null;
    }

    [DataContract, Preserve]
    public class ConsumeUserItemsRequest
    {
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
        [DataMember(Name = "options")] public string[] Options;
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ServerIntegrationConfigurationSortBy
    {
        [System.ComponentModel.Description("createdAt")]
        CreatedAt,
        [System.ComponentModel.Description("createdAt:asc")]
        CreatedAtAsc,
        [System.ComponentModel.Description("createdAt:desc")]
        CreatedAtDesc
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum IntegrationConfigurationStatus
    {
        [System.ComponentModel.Description("INIT"), EnumMember(Value = "INIT")]
        Init,
        [System.ComponentModel.Description("TIED"), EnumMember(Value = "TIED")]
        Tied
    }

    [DataContract, Preserve]
    public class ServerIntegrationConfiguration
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "itemTypes")] public string[] ItemTypes;
        [DataMember(Name = "serviceName")] public string ServiceName;
        [DataMember(Name = "status")] public IntegrationConfigurationStatus Status;
        [DataMember(Name = "targetInventoryCode")] public string TargetInventoryCode;
    }

    [DataContract, Preserve]
    public class ServerIntegrationConfigurationsPagingResponse
    {
        [DataMember(Name = "data")] public ServerIntegrationConfiguration[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class ServerIntegrationConfigurationRequestBase
    {
        [DataMember(Name = "serviceName")] public string ServiceName;
        [DataMember(Name = "mapItemType")] public string[] MapItemType;
        [DataMember(Name = "targetInventoryCode")] public string TargetInventoryCode;
    }

    [DataContract, Preserve]
    public class ServerCreateIntegrationConfigurationRequest : ServerIntegrationConfigurationRequestBase
    {
    }

    [DataContract, Preserve]
    public class ServerUpdateIntegrationConfigurationRequest : ServerIntegrationConfigurationRequestBase
    {
    }

    [DataContract, Preserve]
    public class ServerUpdateIntegrationConfigurationStatusRequest
    {
        [DataMember(Name = "status")] public IntegrationConfigurationStatus Status;
    }

    [DataContract, Preserve]
    public class ServerCreateInventoryRequest
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "inventoryConfigurationCode")] public string InventoryConfigurationCode;
    }

    [DataContract, Preserve]
    public class ServerUpdateInventoryRequest
    {
        [DataMember(Name = "incMaxSlots")] public int IncMaxSlots;
    }

    [DataContract, Preserve]
    public class ServerDeleteInventoryRequest
    {
        [DataMember(Name = "message")] public string Message;
    }

    [DataContract, Preserve]
    public class ServerInventoryConfig
    {
        [DataMember(Name = "slotUsed")] public int SlotUsed;
    }

    [DataContract, Preserve]
    public class ServerInventoryItemData
    {
        [DataMember(Name = "sku")] public string SKU;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "useCount")] public int UseCount;
        [DataMember(Name = "itemType")] public string ItemType;
        [DataMember(Name = "bundledQty")] public int BundledQty;
        [DataMember(Name = "entitlementType")] public EntitlementType EntitlementType;
        [DataMember(Name = "inventoryConfig")] public ServerInventoryConfig InventoryConfig;
    }

    [DataContract, Preserve]
    public class ServerValidateUserInventoryCapacityRequest
    {
        [DataMember(Name = "sku")] public string SKU;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "quantity")] public int Quantity;
        [DataMember(Name = "useCount")] public int UseCount;
        [DataMember(Name = "itemType")] public string ItemType;
        [DataMember(Name = "items")] public ServerInventoryItemData[] Items;
        [DataMember(Name = "entitlementType")] public EntitlementType EntitlementType;
        [DataMember(Name = "inventoryConfig")] public ServerInventoryConfig InventoryConfig;
    }

    [DataContract, Preserve]
    public class ServerUpdateUserInventoryItemRequest : UpdateUserInventoryItemRequest
    {
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "serverCustomAttributes")] public Dictionary<string, object> ServerCustomAttributes;
    }

    [DataContract, Preserve]
    public class ServerUpdateUserInventoryItemPayload
    {
        public ServerUpdateUserInventoryItemPayload(string sourceItemId)
        {
            SourceItemId = sourceItemId;
        }

        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "serverCustomAttributes")] public Dictionary<string, object> ServerCustomAttributes;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    /// <summary>
    /// Optional parameters for SaveUserInventoryItemToInventory endpoint. Can be null.
    /// </summary>
    public class SaveUserInventoryItemToInventoryOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Custom metadata for the item.
        /// </summary>
        public Dictionary<string, object> CustomAttributes = null;

        /// <summary>
        /// Custom server-side metadata for the item.
        /// </summary>
        public Dictionary<string, object> ServerCustomAttributes = null;

        /// <summary>
        /// ID of slot to save item to.
        /// </summary>
        public string SlotId = null;

        /// <summary>
        /// Number of slots used by saved item.
        /// </summary>
        public int? SlotUsed = null;

        /// <summary>
        /// Tags related to the item.
        /// </summary>
        public string[] Tags = null;
    }

    [DataContract, Preserve]
    public class ServerSaveUserInventoryItemRequest : UserItemWithSourceBase
    {
    }

    [DataContract, Preserve]
    internal class ServerSaveUserInventoryItemToUserPayload
    {
        [DataMember(Name = "inventoryConfigurationCode")] public string InventoryConfigurationCode;
        [DataMember(Name = "source")] public string Source;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "slotUsed")] public int SlotUsed;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
        [DataMember(Name = "serverCustomAttributes")] public Dictionary<string, object> ServerCustomAttributes;
    }

    [DataContract, Preserve]
    public class ServerInventoryConfigurationRequestBase
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "initialMaxSlots")] public int InitialMaxSlots;
        [DataMember(Name = "maxUpgradeSlots")] public int MaxUpgradeSlots;
        [DataMember(Name = "maxInstancesPerUser")] public int MaxInstancesPerUser;
    }

    [DataContract, Preserve]
    public class ServerCreateInventoryConfigurationRequest : ServerInventoryConfigurationRequestBase
    {
    }

    /// <summary>
    /// Optional parameters for CreateInventoryConfiguration. Can be null.
    /// </summary>
    public class CreateInventoryConfigurationOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Plain text description for the inventory configuration.
        /// </summary>
        public string Description = null;

        /// <summary>
        /// Name identifier for the inventory configuration.
        /// </summary>
        public string Name = null;
    }

    [DataContract, Preserve]
    public class ServerUpdateInventoryConfigurationRequest : ServerInventoryConfigurationRequestBase
    {
    }

    [DataContract, Preserve]
    public class ServerCreateItemTypeRequest
    {
        [DataMember(Name = "name")] public string Name;
    }

    [DataContract, Preserve]
    public class ServerCreateInventoryTagRequest : InventoryTagBase
    {
    }

    [DataContract, Preserve]
    public class ServerInventoryChainingOperationRequest
    {
        [DataMember(Name = "message")] public string Message;
        [DataMember(Name = "requestId")]public string RequestId;
        [DataMember(Name = "operations")] public InventoryChainingOperation[] Operations;
    }

    [DataContract, Preserve]
    public class InventoryChainingOperation
    {
        [DataMember(Name = "targetUserId")] public string TargetUserId;
        [DataMember(Name = "consumeItems")] public ChainingConsumeUserItemRequest[] ConsumeItems;
        [DataMember(Name = "createItems")] public ChainingSaveUserItemRequest[] CreateItems;
        [DataMember(Name = "removeItems")] public ChainingDeleteUserItemRequest[] RemoveItems;
        [DataMember(Name = "updateItems")] public ChainingUpdateUserItemRequest[] UpdateItems;
    }

    [DataContract, Preserve]
    public class ChainingConsumeUserItemRequest : ConsumeUserItemsRequest
    {
        [DataMember(Name = "inventoryId")] public string InventoryId;
    }

    [DataContract, Preserve]
    public class ChainingSaveUserItemRequest : UserItemBase
    {
        [DataMember(Name = "inventoryId")] public string InventoryId;
        [DataMember(Name = "toSpecificInventory")] public bool ToSpecificInventory;
        [DataMember(Name = "inventoryConfigurationCode")] public string InventoryConfigurationCode;
    }

    [DataContract, Preserve]
    public class ChainingDeleteUserItemRequest : UserItemRequestBase
    {
        [DataMember(Name = "inventoryId")] public string InventoryId;
    }

    [DataContract, Preserve]
    public class ChainingUpdateUserItemRequest : ServerUpdateUserInventoryItemRequest
    {
        [DataMember(Name = "inventoryId")] public string InventoryId;
    }

    [DataContract, Preserve]
    public class InventoryChainingOperationResponse
    {
        [DataMember(Name = "errorDetails")] public Error ErrorDetails;
        [DataMember(Name = "message")] public string Message;
        [DataMember(Name = "replayed")] public bool Replayed;
        [DataMember(Name = "requestId")] public string RequestId;
    }

    [Preserve]
    public class SaveUserInventoryItemOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Optional parameter to specify item with tags
        /// </summary>
        public string[] Tags;
        /// <summary>
        /// Optional parameter to define the slot Id 
        /// </summary>
        public string SlotId;
        /// <summary>
        /// Optional parameter to define number of slot used
        /// </summary>
        public int SlotUsed;
        /// <summary>
        /// Optional parameter to set its custom attributes
        /// </summary>
        public Dictionary<string, object> CustomAttributes;
        /// <summary>
        /// Optional parameter to set its custom attribute on server side
        /// </summary>
        public Dictionary<string, object> ServerCustomAttributes;
    }
}
