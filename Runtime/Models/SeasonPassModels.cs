// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

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

    [DataContract, Preserve]
    public class SeasonPassExcessStrategy
    {
        [DataMember] public SeasonPassStrategyMethod method;
        [DataMember] public string currency;
        [DataMember] public int percentPerExp;
    }

    [DataContract, Preserve]
    public class SeasonPassModel
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string seasonId;
        [DataMember] public string code;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string displayOrder;
        [DataMember] public bool autoEnroll;
        [DataMember] public string passItemId;
        [DataMember] public Image[] images;
        [DataMember] public string language;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updateAt;
    }

    [DataContract, Preserve]
    public class SeasonPassTierJsonObject
    {
        [DataMember] public string id;
        [DataMember] public int requiredExp;
        [DataMember] public Dictionary<string, object> rewards;
    }

    [DataContract, Preserve] 
    public class SeasonPassTier
    {
        [DataMember] public string id;
        [DataMember] public int requiredExp;
        [DataMember] public Dictionary<string, string[]> rewards;
    }

    [DataContract, Preserve]
    public class SeasonPassReward
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string seasonId;
        [DataMember] public string code;
        [DataMember] public SeasonPassRewardType type;
        [DataMember] public string itemId {get; set; }
        [DataMember] public string itemName;
        [DataMember] public int quantity;
        [DataMember] public Image image;
    }

    [DataContract, Preserve]
    public class Season
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public DateTime start;
        [DataMember] public DateTime end;
        [DataMember] public SeasonPassStatus status;
        [DataMember] public DateTime publishedAt;
    }

    [DataContract, Preserve]
    public class SeasonInfo
    {
        [DataMember] public string title;
        [DataMember] public string description;
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public DateTime start;
        [DataMember] public DateTime end;
        [DataMember] public string tierItemId;
        [DataMember] public bool autoClaim;
        [DataMember] public Image[] images;
        [DataMember] public string[] passCodes;
        [DataMember] public SeasonPassStatus status;
        [DataMember] public DateTime publishedAt;
        [DataMember] public string language;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public SeasonPassModel[] passes;
        [DataMember] public Dictionary<string, SeasonPassReward> rewards;
        [DataMember] public SeasonPassTier[] tiers;
    }

    [DataContract, Preserve]
    public class BulkGetUserSessionProgressionRequest
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
    }

    [DataContract, Preserve]
    public class UserSeasonInfo
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string seasonId;
        [DataMember] public DateTime enrolledAt;
        [DataMember] public string[] enrolledPasses;
        [DataMember] public int currentTierIndex;
        [DataMember] public int lastTierIndex;
        [DataMember] public int requiredExp;
        [DataMember] public int currentExp;
        [DataMember] public bool cleared;
        [DataMember] public Season season;
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> toClaimRewards;
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> claimingRewards;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int totalPaidForExp;
        [DataMember] public int totalSweatExp;
        [DataMember] public int totalExp;
        [DataMember] public int accumulatedXpBoost;
    }

    [DataContract, Preserve]
    public class UserSeasonInfoWithoutReward
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
        [DataMember] public string seasonId;
        [DataMember] public DateTime enrolledAt;
        [DataMember] public string[] enrolledPasses;
        [DataMember] public int currentTierIndex;
        [DataMember] public int lastTierIndex;
        [DataMember] public int requiredExp;
        [DataMember] public int currentExp;
        [DataMember] public bool cleared;
        [DataMember] public Season season;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class GrantExpRequest
    {
        [DataMember] public int exp;
        [DataMember] public SeasonPassSource source;
        [DataMember] public string[] tags;
    }

    [DataContract, Preserve]
    public class GrantTierRequest
    {
        [DataMember] public int count;
        [DataMember] public SeasonPassSource source;
        [DataMember] public string[] tags;
    }

    [DataContract, Preserve]
    public class SeasonClaimRewardRequest
    {
        [DataMember] public string passCode;
        [DataMember] public int tierIndex;
        [DataMember] public string rewardCode;
    }

    [DataContract, Preserve]
    public class SeasonClaimRewardResponse
    {
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> toClaimRewards;
        [DataMember] public Dictionary<int, Dictionary<string, string[]>> claimingRewards;
    }

    [DataContract, Preserve]
    public class UserSeasonData
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string seasonId;
        [DataMember] public string userId;
        [DataMember] public int grantExp;
        [DataMember] public string[] tags;
        [DataMember] public DateTime createdAt;
        [DataMember] public SeasonPassSource source;
    }

    [DataContract, Preserve]
    public class UserSeasonPaging
    {
        [DataMember] public string previous;
        [DataMember] public string next;
    }

    [DataContract, Preserve]
    public class UserSeasonExpHistory
    {
        [DataMember] public UserSeasonData[] data;
        [DataMember] public UserSeasonPaging paging;
        [DataMember] public int total;
    }

    [DataContract, Preserve]
    public class QueryUserSeasonExp
    {
        [DataMember] public string[] tags;
    }
}