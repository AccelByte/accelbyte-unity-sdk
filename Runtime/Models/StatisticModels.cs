// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatisticSetBy
    {
        CLIENT,
        SERVER
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatisticStatus
    {
        INIT,
        TIED
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatisticUpdateStrategy
    {
        OVERRIDE,
        INCREMENT,
        MIN,
        MAX
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
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

    [DataContract]
    public class StatConfig
    {
        [DataMember] public string createdAt { get; set; }
        [DataMember] public float defaultValue { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public bool incrementOnly { get; set; }
        [DataMember] public float maximum { get; set; }
        [DataMember] public float minimum { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public bool setAsGlobal { get; set; }
        [DataMember] public StatisticSetBy setBy { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public StatisticStatus status { get; set; }
        [DataMember] public string updatedAt { get; set; }
        [DataMember] public string[] cycleIds { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class StatItem
    {
        [DataMember] public string createdAt { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string statName { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string updatedAt { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public Dictionary<string, object> additionalData { get; set; }
    }

    [DataContract]
    public class CreateStatItemRequest
    {
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class PagedStatItems
    {
        [DataMember] public StatItem[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UserStatItemIncrement
    {
        [DataMember] public float inc { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class StatItemIncrement
    {
        [DataMember] public float inc { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class StatItemOperationResult
    {
        [DataMember] public object details { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public bool success { get; set; }
    }

    [DataContract]
    public class UserStatItemReset
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class StatItemReset
    {
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class UserStatItemUpdate
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string additionalKey { get; set; }
        [DataMember] public Dictionary<string, object> additionalData { get; set; }
    }

    [DataContract]
    public class StatItemUpdate
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public Dictionary<string, object> additionalData { get; set; }
    }

    public class FetchUser
    {
        [DataMember] public string profileId { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public float value { get; set; }
    }

    public class StatItemValue
    {
        [DataMember] public string StatCode { get; set; }
        [DataMember] public string StatName { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public float Value { get; set; }
        [DataMember] public string[] Tags { get; set; }
        [DataMember] public string CreatedAt { get; set; }
        [DataMember] public string UpdatedAt { get; set; }
        [DataMember] public Dictionary<string, object> AdditionalData { get; set; }
        [DataMember] public string UserId { get; set; }
    }

    public class FetchUserStatistic
    {
        [DataMember] public StatItemValue[] UserStatistic { get; set; }
        [DataMember] public string[] NotProcessedUserIds { get; set; }
    }

    public class UpdateUserStatItem
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public string additionalKey { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public StatisticUpdateStrategy updateStrategy { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public object additionalData { get; set; }
    }

    public class UpdateUserStatItemsResponse
    {
        [DataMember] public bool success { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public object details { get; set; }
    }

    public class UserStatItem
    {
        [DataMember] public string statCode { get; set; }
        [DataMember] public object additionalData { get; set; }
    }

    public class UpdateUserStatItemWithStatCode
    {
        [DataMember] public string statCode { get; set; }
        [DataMember] public StatisticUpdateStrategy updateStrategy { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public object additionalData { get; set; }
    }

    public class PublicUpdateUserStatItem
    {
        [DataMember] public StatisticUpdateStrategy updateStrategy { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public object additionalData { get; set; }
    }

    public class UpdateUserStatItemValueResponse
    {
        [DataMember] public float currentValue { get; set; }
    };

    [DataContract]
    public class GlobalStatItem
    {
        [DataMember] public string statCode { get; set; }
        [DataMember] public string statName { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public float value { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updatedAt { get; set; }
    }
    
    
    [DataContract]
    public class StatCycleConfig
    {
        [DataMember] public string Id { get; set; }
        [DataMember] public string Namespace { get; set; }
        [DataMember] public StatisticCycleType CycleType { get; set; } 
        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string ResetTime { get; set; }
        [DataMember] public int ResetDay { get; set; }
        [DataMember] public int ResetDate { get; set; }
        [DataMember] public int ResetMonth { get; set; }
        [DataMember] public int SeasonPeriod { get; set; }
        [DataMember] public int CurrentVersion { get; set; }
        [DataMember] public StatisticCycleStatus Status { get; set; }
        [DataMember] public string NextReset { get; set; }
        [DataMember] public string Start { get; set; }
        [DataMember] public string End { get; set; }
        [DataMember] public string CreatedAt { get; set; }
        [DataMember] public string UpdatedAt { get; set; }
    }

    [DataContract]
    public class PagedStatCycleConfigs
    {
        [DataMember] public StatCycleConfig[] Data { get; set; }
        [DataMember] public Paging Paging { get; set; }
    }
    
    [DataContract]
    public class StatCycleItem
    {
        [DataMember] public string CreatedAt { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string UserId { get; set; }
        [DataMember] public string StatCode { get; set; }
        [DataMember] public string StatName { get; set; }
        [DataMember] public string[] Tags { get; set; }
        [DataMember] public string UpdatedAt { get; set; }
        [DataMember] public float Value { get; set; }
        [DataMember] public string CycleId { get; set; }
        [DataMember] public string CycleName { get; set; }
        [DataMember] public int CycleVersion { get; set; }
    }

    [DataContract]
    public class PagedStatCycleItem
    {
        [DataMember] public StatCycleItem[] Data { get; set; }
        [DataMember] public Paging Paging { get; set; }
    }
}