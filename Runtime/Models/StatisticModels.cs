// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticSetBy
    {
        CLIENT,
        SERVER
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticStatus
    {
        INIT,
        TIED
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticUpdateStrategy
    {
        OVERRIDE,
        INCREMENT,
        MIN,
        MAX
    }
    
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticCycleType
    {
        None,
        [EnumMember(Value= "DAILY")]
        Daily,
        [EnumMember(Value= "WEEKLY")]
        Weekly,
        [EnumMember(Value= "MONTHLY")]
        Monthly,
        [EnumMember(Value= "ANNUALLY")]
        Annually,
        [EnumMember(Value= "SEASONAL")]
        Seasonal
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticCycleStatus
    {
        None,
        [EnumMember(Value = "INIT")]
        Init,
        [EnumMember(Value = "ACTIVE")]
        Active,
        [EnumMember(Value = "STOPPED")]
        Stopped
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum StatisticSortBy
    {
        None,
        [EnumMember(Value = "STAT_CODE")]
        StatCode,
        [EnumMember(Value = "STAT_CODE_ASC")]
        StatCodeAsc,
        [EnumMember(Value = "STAT_CODE_DESC")]
        StatCodeDesc,
        [EnumMember(Value = "CREATED_AT")]
        CreatedAt,
        [EnumMember(Value = "CREATED_AT_ASC")]
        CreatedAtAsc,
        [EnumMember(Value = "CREATED_AT_DESC")]
        CreatedAtDesc,
        [EnumMember(Value = "UPDATED_AT")]
        UpdatedAt,
        [EnumMember(Value = "UPDATED_AT_ASC")]
        UpdatedAtAsc,
        [EnumMember(Value = "UPDATED_AT_DESC")]
        UpdatedAtDesc,
    }

    [DataContract, Preserve]
    public class StatConfig
    {
        [DataMember] public string statCode;
        [DataMember] public string name;
        [DataMember] public string description;
        [DataMember] public float minimum;
        [DataMember] public float maximum;
        [DataMember] public float defaultValue;
        [DataMember] public bool incrementOnly;
        [DataMember] public bool setAsGlobal;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "globalAggregationMethod")] public StatConfigAggregationMethod GlobalAggregationMethod;
        [DataMember] public StatisticSetBy setBy;
        [DataMember] public StatisticStatus status;
        [DataMember] public string[] cycleIds;
        [DataMember] public string[] tags;
        [DataMember] public string createdAt;
        [DataMember] public string updatedAt;
        [DataMember(Name = "isPublic")] public bool IsPublic;
        [DataMember(Name = "ignoreAdditionalDataOnValueRejected")] public bool IgnoreAdditionalDataOnValueRejected;
        [DataMember(Name = "visibility")] public StatConfigVisibility Visibility;
    }

    [Preserve]
    public enum StatConfigVisibility
    {
        [EnumMember(Value = "SHOWALL")] ShowAll,
        [EnumMember(Value = "SERVERONLY ")] ServerOnly
    }
    
    [Preserve]
    public enum StatConfigAggregationMethod
    {
        [EnumMember(Value = "TOTAL")] Total,
        [EnumMember(Value = "MIN ")] Min,
        [EnumMember(Value = "MAX")] Max,
        [EnumMember(Value = "LAST ")] Last
    }

    [DataContract, Preserve]
    public class StatItem
    {
        [DataMember] public string createdAt;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string statCode;
        [DataMember] public string statName;
        [DataMember] public string[] tags;
        [DataMember] public string updatedAt;
        [DataMember] public float value;
        [DataMember] public Dictionary<string, object> additionalData;
    }

    [DataContract, Preserve]
    public class CreateStatItemRequest
    {
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class PagedStatItems
    {
        [DataMember] public StatItem[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UserStatItemIncrement
    {
        [DataMember] public float inc;
        [DataMember] public string userId;
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class StatItemIncrement
    {
        [DataMember] public float inc;
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class StatItemOperationResult
    {
        [DataMember] public object details;
        [DataMember] public string statCode;
        [DataMember] public bool success;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class UserStatItemReset
    {
        [DataMember] public string userId;
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class StatItemReset
    {
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class UserStatItemUpdate
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy;
        [DataMember] public float value;
        [DataMember] public string statCode;
        [DataMember] public string userId;
        [DataMember] public string additionalKey;
        [DataMember] public Dictionary<string, object> additionalData;
    }

    [DataContract, Preserve]
    public class StatItemUpdate
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy;
        [DataMember] public float value;
        [DataMember] public string statCode;
        [DataMember] public Dictionary<string, object> additionalData;
    }

    [DataContract, Preserve]
    public class FetchUser
    {
        [DataMember] public string profileId;
        [DataMember] public string statCode;
        [DataMember] public float value;
    }

    [DataContract, Preserve]
    public class StatItemValue
    {
        [DataMember] public string StatCode;
        [DataMember] public string StatName;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public float Value;
        [DataMember] public string[] Tags;
        [DataMember] public string CreatedAt;
        [DataMember] public string UpdatedAt;
        [DataMember] public Dictionary<string, object> AdditionalData;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class FetchUserStatistic
    {
        [DataMember] public StatItemValue[] UserStatistic;
        [DataMember] public string[] NotProcessedUserIds;
    }

    public class UpdateUserStatItem
    {
        [DataMember] public string userId;
        [DataMember] public string additionalKey;
        [DataMember] public string statCode;
        [DataMember] public StatisticUpdateStrategy updateStrategy;
        [DataMember] public float value;
        [DataMember] public object additionalData;
    }

    [DataContract, Preserve]
    public class UpdateUserStatItemsResponse
    {
        [DataMember] public bool success;
        [DataMember] public string statCode;
        [DataMember] public object details;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class UserStatItem
    {
        [DataMember] public string statCode;
        [DataMember] public object additionalData;
    }

    [DataContract, Preserve]
    public class UpdateUserStatItemWithStatCode
    {
        [DataMember] public string statCode;
        [DataMember] public StatisticUpdateStrategy updateStrategy;
        [DataMember] public float value;
        [DataMember] public object additionalData;
    }

    [DataContract, Preserve]
    public class PublicUpdateUserStatItem
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy;
        [DataMember] public float value;
        [DataMember] public object additionalData;
    }

    [DataContract, Preserve]
    public class UpdateUserStatItemValueResponse
    {
        [DataMember] public float currentValue;
    };

    [DataContract, Preserve]
    public class GlobalStatItem
    {
        [DataMember] public string statCode;
        [DataMember] public string statName;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public float value;
        [DataMember] public string[] tags;
        [DataMember] public string createdAt;
        [DataMember] public string updatedAt;
    }
    
    
    [DataContract, Preserve]
    public class StatCycleConfig
    {
        [DataMember] public string Id;
        [DataMember] public string Namespace;
        [DataMember] public StatisticCycleType CycleType; 
        [DataMember] public string Name;
        [DataMember] public string Description;
        [DataMember] public string ResetTime;
        [DataMember] public int ResetDay;
        [DataMember] public int ResetDate;
        [DataMember] public int ResetMonth;
        [DataMember] public int SeasonPeriod;
        [DataMember] public int CurrentVersion;
        [DataMember] public StatisticCycleStatus Status;
        [DataMember] public string NextReset;
        [DataMember] public string Start;
        [DataMember] public string End;
        [DataMember] public string CreatedAt;
        [DataMember] public string UpdatedAt;
    }

    [DataContract, Preserve]
    public class PagedStatCycleConfigs
    {
        [DataMember] public StatCycleConfig[] Data;
        [DataMember] public Paging Paging;
    }
    
    [DataContract, Preserve]
    public class StatCycleItem
    {
        [DataMember] public string CreatedAt;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string UserId;
        [DataMember] public string StatCode;
        [DataMember] public string StatName;
        [DataMember] public string[] Tags;
        [DataMember] public string UpdatedAt;
        [DataMember] public float Value;
        [DataMember] public string CycleId;
        [DataMember] public string CycleName;
        [DataMember] public int CycleVersion;
    }

    [DataContract, Preserve]
    public class PagedStatCycleItem
    {
        [DataMember] public StatCycleItem[] Data;
        [DataMember] public Paging Paging;
    }

    public class StatisticOptionalParamBase : OptionalParametersBase
    {
        /// <summary>
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </summary>
        public int Offset = 0;

        /// <summary>
        /// The limit of item on page (optional, default = 20)
        /// </summary>
        public int Limit = 20;
        
        /// <summary>
        /// Array of statistic codes for statistic cycle items to be retrieved
        /// </summary>
        public string[] StatCodes;
    }

    [Preserve]
    public class GetAllUserStatItemsOptionalParam : StatisticOptionalParamBase
    {
        /// <summary>
        /// List of tags that will be included in the result
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// The sorting method of item on page (optional, default = updated at and ascending) 
        /// </summary>
        public StatisticSortBy sortBy = StatisticSortBy.UpdatedAtAsc;
    }

    [Preserve]
    public class GetUserStatItemsOptionalParam : StatisticOptionalParamBase
    {
        /// <summary>
        /// List of tags that will be included in the result
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// The sorting method of item on page (optional, default = updated at and ascending) 
        /// </summary>
        public StatisticSortBy sortBy = StatisticSortBy.UpdatedAtAsc;
    }

    [Preserve]
    public class GetMyStatCycleItemsOptionalParam : StatisticOptionalParamBase
    {
    }

    [Preserve]
    public class GetMyStatItemsOptionalParam : StatisticOptionalParamBase
    {        
        /// <summary>
        /// List of tags that will be included in the result
        /// </summary>
        public string[] Tags;
    }

    [Preserve]
    public class CreateUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class IncrementUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class ResetUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpdateUserStatItemsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// To identify multi level user statItem, such as character.
        /// </summary>
        public string AdditionalKey = string.Empty;
    }

    [Preserve]
    public class ListUserStatItemsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// This is the AdditionalKey that will be stored in the slot.
        /// </summary>
        public string AdditionalKey = string.Empty;

        /// <summary>
        /// Filter results via their stat code. 
        /// <para>If stat code does not exist, will ignore this stat code.</para>
        /// <para>If stat item does not exist, will return default value</para>
        /// </summary>
        public string[] StatCodes = null;

        /// <summary>
        /// This is the Tag array that will be stored in the slot.
        /// </summary>
        public string[] Tags = null;
    }

    [Preserve]
    public class UpdateUserStatItemsValueOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// This is the AdditionalKey that will be stored in the slot.
        /// </summary>
        public string AdditionalKey = string.Empty;
    }

    [Preserve]
    public class BulkFetchStatItemsValueOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetGlobalStatItemsByStatCodeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetStatCycleConfigOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetListStatCycleConfigsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned results via their type.
        /// </summary>
        public StatisticCycleType? Type = StatisticCycleType.None;

        /// <summary>
        /// Filter returned results via their status.
        /// </summary>
        public StatisticCycleStatus? Status = StatisticCycleStatus.None;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetListUserStatCycleItemOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned results via their stat codes.
        /// </summary>
        public string[] StatCodes = new string[0];

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    [Preserve]
    public class GetMyStatItemValuesOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// This is the AdditionalKey that will be stored in the slot.
        /// </summary>
        public string AdditionalKey = string.Empty;

        /// <summary>
        /// Filter results via their stat code. 
        /// <para>If stat code does not exist, will ignore this stat code.</para>
        /// <para>If stat item does not exist, will return default value</para>
        /// </summary>
        public string[] StatCodes = null;

        /// <summary>
        /// This is the Tag array that will be stored in the slot.
        /// </summary>
        public string[] Tags = null;
    }

    [Preserve]
    public class IncrementManyUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class ResetManyUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpdateManyUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class BulkFetchUserStatItemsValueOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// This is the AdditionalKey that will be stored in the slot.
        /// </summary>
        public string AdditionalKey = string.Empty;
    }

    [Preserve]
    public class BulkUpdateMultipleUserStatItemsValueOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class BulkResetUserStatItemsValuesOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class BulkUpdateUserStatItemValueOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpdateUserStatItemValueOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class DeleteUserStatItemsOptionalParameters : OptionalParametersBase
    {

    }
}