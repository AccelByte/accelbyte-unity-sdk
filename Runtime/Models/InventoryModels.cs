// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InventoryConfigurationStatus
    {
        [System.ComponentModel.Description("INIT"), EnumMember(Value = "INIT")]
        Init,
        [System.ComponentModel.Description("TIED"), EnumMember(Value = "TIED")]
        Tied
    }

    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
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
        UpdatedAtDesc,
        [System.ComponentModel.Description("quantity")]
        Quantity,
        [System.ComponentModel.Description("quantity:asc")]
        QuantityAsc,
        [System.ComponentModel.Description("quantity:desc")]
        QuantityDesc
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
        [DataMember(Name = "initialmaxSlots")] public int InitialMaxSlots;
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
    public class InventoryTag
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "owner")] public string Owner;
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
    public class UserItem
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "inventoryId")] public string InventoryId;
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "platformAvailable")] public bool IsPlatformAvailable;
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "source")] public string Source;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "slotUsed")] public int SlotUsed;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;
        [DataMember(Name = "serverCustomAttributes")] public Dictionary<string, object> ServerCustomAttributes;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
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
    public class DeleteUserInventoryItemResponse : UserItemResponseBase
    {
    }

    [DataContract, Preserve]
    public class MoveUserItemBase
    {
        [DataMember(Name = "qty")] public int Quantity;
        [DataMember(Name = "slotId")] public string SlotId;
        [DataMember(Name = "sourceItemId")] public string SourceItemId;
    }

    [DataContract, Preserve]
    public class MoveUserItemRequestData : MoveUserItemBase
    {
    }

    [DataContract, Preserve]
    public class MoveUserItemResponseData : MoveUserItemBase
    {
    }

    [DataContract, Preserve]
    public class MoveUserItemsBetweenInventoriesRequest
    {
        [DataMember(Name = "items")] public MoveUserItemRequestData[] items;
        [DataMember(Name = "srcInventoryId")] public string SourceInventoryId;
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

    [DataContract, Preserve]
    public class ConsumeUserItemsRequest : MoveUserItemBase
    {
    }
}
