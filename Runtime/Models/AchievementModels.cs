// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AchievementSortBy
    {
        NONE,
        LISTORDER,
        LISTORDER_ASC,
        LISTORDER_DESC,
        CREATED_AT,
        CREATED_AT_ASC,
        CREATED_AT_DESC,
        UPDATED_AT,
        UPDATED_AT_ASC,
        UPDATED_AT_DESC,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GlobalAchievementStatus
    {
        None,
        InProgress,
        Unlocked
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GlobalAchievementListSortBy
    {
        None,
        AchievedAt,
        AchievedAtAsc,
        AchievedAtDesc,
        CreatedAt,
        CreatedAtAsc,
        CreatedAtDesc
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GlobalAchievementContributorsSortBy
    {
        NONE,
        ContributedValue,
        ContributedValueAsc,
        ContributedValueDesc
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConvertAchievementStatus: int
    {
        InProgress = 1,
        Unlocked = 2
    }

    [DataContract, Preserve]
    public class AchievementIcon
    {
        [DataMember] public string url;
        [DataMember] public string slug;
    }

    [DataContract, Preserve]
    public class PublicAchievement
    {
        [DataMember] public string achievementCode;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string name;
        [DataMember] public string description;
        [DataMember] public AchievementIcon[] lockedIcons;
        [DataMember] public AchievementIcon[] unlockedIcons;
        [DataMember] public bool hidden;
        [DataMember] public int listOrder;
        [DataMember] public string[] tags;
        [DataMember] public bool incremental;
        [DataMember] public bool global;
        [DataMember] public float goalValue;
        [DataMember] public string statCode;
        [DataMember] public string createdAt;
        [DataMember] public string updateAt;
        [DataMember] public Dictionary<string, object> CustomAttributes;
    }

    [DataContract, Preserve]
    public class PaginatedPublicAchievement
    {
        [DataMember] public PublicAchievement[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class MultiLanguageAchievement
    {
        [DataMember] public string achievementCode;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public Dictionary<string, string> name;
        [DataMember] public Dictionary<string, string> description;
        [DataMember] public AchievementIcon[] lockedIcons;
        [DataMember] public AchievementIcon[] unlockedIcons;
        [DataMember] public bool hidden;
        [DataMember] public int listOrder;
        [DataMember] public string[] tags;
        [DataMember] public bool incremental;
        [DataMember] public bool global;
        [DataMember] public float goalValue;
        [DataMember] public string statCode;
        [DataMember] public string createdAt;
        [DataMember] public string updateAt;
        [DataMember] public Dictionary<string, object> CustomAttributes;
    }

    [DataContract, Preserve]
    public class CountInfo
    {
        [DataMember] public int numberOfAchievements;
        [DataMember] public int numberOfHiddenAchievements;
        [DataMember] public int numberOfVisibleAchievements;
    }

    [DataContract, Preserve]
    public class UserAchievement
    {
        [DataMember] public string id;
        [DataMember] public Dictionary<string, string> name;
        [DataMember] public string achievementCode;
        [DataMember] public string achievedAt;
        [DataMember] public float latestValue;
        [DataMember] public int status; // 1: In-Progress, 2: Unlocked
    }

    [DataContract, Preserve]
    public class PaginatedUserAchievement
    {
        [DataMember] public CountInfo countInfo;
        [DataMember] public UserAchievement[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UserGlobalAchievement
    {
        [DataMember] public string Id;
        [DataMember] public Dictionary<string, string> Name;
        [DataMember] public string AchievementCode;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] private int Status; // 1: In-Progress, 2: Unlocked
        [DataMember] public ConvertAchievementStatus StatusCode
        {
            get
            {
                return (ConvertAchievementStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        [DataMember] public float LatestValue;
        [DataMember] public string AchievedAt;
        [DataMember] public string CreatedAt;
        [DataMember] public string UpdatedAt;
    }

    [DataContract, Preserve]
    public class PaginatedUserGlobalAchievement
    {
        [DataMember] public UserGlobalAchievement[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class GlobalAchievementContributors
    {
        [DataMember] public string Id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string AchievementCode;
        [DataMember] public string UserId;
        [DataMember] public float ContributedValue;
        [DataMember] public string CreatedAt;
        [DataMember] public string UpdatedAt;
    }

    [DataContract, Preserve]
    public class PaginatedGlobalAchievementContributors
    {
        [DataMember] public GlobalAchievementContributors[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class GlobalAchievementContributed
    {
        [DataMember] public string Id;
        [DataMember] public Dictionary<string, string> name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string AchievementCode;
        [DataMember] public string UserId;
        [DataMember] public float ContributedValue;
        [DataMember] public bool CanClaimReward;
    }

    [DataContract, Preserve]
    public class PaginatedGlobalAchievementUserContributed
    {
        [DataMember] public GlobalAchievementContributed[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class PublicTag
    {
        [DataMember] public string name;
        [DataMember] public string Namespace;
        [DataMember] public string createdAt;
    }

    [DataContract, Preserve]
    public class PaginatedPublicTag
    {
        [DataMember] public PublicTag[] data;
        [DataMember] public Paging paging;
    }
}
