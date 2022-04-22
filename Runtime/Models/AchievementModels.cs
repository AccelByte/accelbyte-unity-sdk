// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
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

    [DataContract]
    public class AchievementIcon
    {
        [DataMember] public string url { get; set; }
        [DataMember] public string slug { get; set; }
    }

    [DataContract]
    public class PublicAchievement
    {
        [DataMember] public string achievementCode { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public AchievementIcon[] lockedIcons { get; set; }
        [DataMember] public AchievementIcon[] unlockedIcons { get; set; }
        [DataMember] public bool hidden { get; set; }
        [DataMember] public int listOrder { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public bool incremental { get; set; }
        [DataMember] public float goalValue { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updateAt { get; set; }
    }

    [DataContract]
    public class PaginatedPublicAchievement
    {
        [DataMember] public PublicAchievement[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class MultiLanguageAchievement
    {
        [DataMember] public string achievementCode { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public Dictionary<string, string> name { get; set; }
        [DataMember] public Dictionary<string, string> description { get; set; }
        [DataMember] public AchievementIcon[] lockedIcons { get; set; }
        [DataMember] public AchievementIcon[] unlockedIcons { get; set; }
        [DataMember] public bool hidden { get; set; }
        [DataMember] public int listOrder { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public bool incremental { get; set; }
        [DataMember] public float goalValue { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updateAt { get; set; }
    }

    [DataContract]
    public class CountInfo
    {
        [DataMember] public int numberOfAchievements { get; set; }
        [DataMember] public int numberOfHiddenAchievements { get; set; }
        [DataMember] public int numberOfVisibleAchievements { get; set; }
    }

    [DataContract]
    public class UserAchievement
    {
        [DataMember] public string id { get; set; }
        [DataMember] public Dictionary<string, string> name { get; set; }
        [DataMember] public string achievementCode { get; set; }
        [DataMember] public string achievedAt { get; set; }
        [DataMember] public float latestValue { get; set; }
        [DataMember] public int status { get; set; } // 1: In-Progress, 2: Unlocked
    }

    [DataContract]
    public class PaginatedUserAchievement
    {
        [DataMember] public CountInfo countInfo { get; set; }
        [DataMember] public UserAchievement[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }
}
