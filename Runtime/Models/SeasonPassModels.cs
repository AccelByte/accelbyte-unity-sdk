// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    #region enum
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SeasonPassStrategyMethod
    {
        NONE = 0,
        CURRENCY
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SeasonPassStatus
    {
        DRAFT = 0,
        PUBLISHED,
        RETIRED
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SeasonPassRewardType
    {
        ITEM //currently only support this type
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeasonPassSource
    {
        SWEAT = 0,
        PAID_FOR
    }
    #endregion enum

    [DataContract]
    public class SeasonPassExcessStrategy
    {
        [DataMember] public SeasonPassStrategyMethod method { get; set; }
        [DataMember] public string currency { get; set; }
        [DataMember] public int percentPerExp { get; set; }
    }

    [DataContract]
    public class SeasonPassModel
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string seasonId { get; set; }
        [DataMember] public string code { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string displayOrder { get; set; }
        [DataMember] public bool autoEnroll { get; set; }
        [DataMember] public string passItemId { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updateAt { get; set; }
    }

    [DataContract]
    public class SeasonPassTierJsonObject
    {
        [DataMember] public string id { get; set; }
        [DataMember] public int requiredExp { get; set; }
        [DataMember] public Dictionary<string, object> rewards { get; set; }
    }

    [DataContract] 
    public class SeasonPassTier
    {
        [DataMember] public string id { get; set; }
        [DataMember] public int requiredExp { get; set; }
        [DataMember] public Dictionary<string, string[]> rewards { get; set; }
    }

    [DataContract]
    public class SeasonPassReward
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string seasonId { get; set; }
        [DataMember] public string code { get; set; }
        [DataMember] public SeasonPassRewardType type { get; set; }
        [DataMember] public string itemId {get; set; }
        [DataMember] public string itemName { get; set; }
        [DataMember] public int quantity { get; set; }
        [DataMember] public Image image { get; set; }
    }

    [DataContract]
    public class Season
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public DateTime start { get; set; }
        [DataMember] public DateTime end { get; set; }
        [DataMember] public SeasonPassStatus status { get; set; }
        [DataMember] public DateTime publishedAt { get; set; }
    }

    [DataContract]
    public class SeasonInfo
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public DateTime start { get; set; }
        [DataMember] public DateTime end { get; set; }
        [DataMember] public string tierItemId { get; set; }
        [DataMember] public bool autoClaim { get; set; }
        [DataMember] public Image[] images { get; set; }
        [DataMember] public string[] passCodes { get; set; }
        [DataMember] public SeasonPassStatus status { get; set; }
        [DataMember] public DateTime publishedAt { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public SeasonPassModel[] passes { get; set; }
        [DataMember] public Dictionary<string, SeasonPassReward> rewards { get; set; }
        [DataMember] public SeasonPassTier[] tiers { get; set; }
    }

    [DataContract]
    public class UserSeasonInfo
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string seasonId { get; set; }
        [DataMember] public DateTime enrolledAt { get; set; }
        [DataMember] public string[] enrolledPasses { get; set; }
        [DataMember] public int currentTierIndex { get; set; }
        [DataMember] public int lastTierIndex { get; set; }
        [DataMember] public int requiredExp { get; set; }
        [DataMember] public int currentExp { get; set; }
        [DataMember] public bool cleared { get; set; }
        [DataMember] public Season season { get; set; }
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> toClaimRewards { get; set; }
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> claimingRewards { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public int totalPaidForExp { get; set; }
        [DataMember] public int totalSweatExp { get; set; }
        [DataMember] public int totalExp { get; set; }
        [DataMember] public int accumulatedXpBoost { get; set; }
    }

    [DataContract]
    public class UserSeasonInfoWithoutReward
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string seasonId { get; set; }
        [DataMember] public DateTime enrolledAt { get; set; }
        [DataMember] public string[] enrolledPasses { get; set; }
        [DataMember] public int currentTierIndex { get; set; }
        [DataMember] public int lastTierIndex { get; set; }
        [DataMember] public int requiredExp { get; set; }
        [DataMember] public int currentExp { get; set; }
        [DataMember] public bool cleared { get; set; }
        [DataMember] public Season season { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class GrantExpRequest
    {
        [DataMember] public int exp { get; set; }
        [DataMember] public SeasonPassSource source { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class GrantTierRequest
    {
        [DataMember] public int count { get; set; }
        [DataMember] public SeasonPassSource source { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class SeasonClaimRewardRequest
    {
        [DataMember] public string passCode { get; set; }
        [DataMember] public int tierIndex { get; set; }
        [DataMember] public string rewardCode { get; set; }
    }

    [DataContract]
    public class SeasonClaimRewardResponse
    {
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> toClaimRewards { get; set; }
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> claimingRewards { get; set; }
    }

    [DataContract]
    public class UserSeasonData
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string seasonId { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public int grantExp { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public SeasonPassSource source { get; set; }
    }

    [DataContract]
    public class UserSeasonPaging
    {
        [DataMember] public string previous { get; set; }
        [DataMember] public string next { get; set; }
    }

    [DataContract]
    public class UserSeasonExpHistory
    {
        [DataMember] public UserSeasonData[] data { get; set; }
        [DataMember] public UserSeasonPaging paging { get; set; }
        [DataMember] public int total { get; set; }
    }

    [DataContract]
    public class QueryUserSeasonExp
    {
        [DataMember] public string[] tags { get; set; }
    }
}