// Copyright (c) 2020 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{

    #region enum
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LeaderboardTimeFrame
    {
        ALL_TIME,
        CURRENT_SEASON,
        CURRENT_MONTH,
        CURRENT_WEEK,
        TODAY
    }
    #endregion enum

    [DataContract]
    public class UserPoint
    {
        [DataMember] public float point { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public bool hidden { get; set; }
        [DataMember] public Dictionary<string, object> additionalData { get; set; }
    }

    [DataContract]
    public class UserRanking
    {
        [DataMember] public float point { get; set; }
        [DataMember] public int rank { get; set; }
        [DataMember] public bool hidden { get; set; }
        [DataMember] public Dictionary<string, object> additionalData {get; set;}
    }

    [DataContract]
    public class UserRankingData
    {
        [DataMember] public UserRanking allTime { get; set; }
        [DataMember] public UserRanking current { get; set; }
        [DataMember] public UserRanking daily { get; set; }
        [DataMember] public UserRanking monthly { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public UserRanking weekly { get; set; }
    }

    [DataContract]
    public class LeaderboardRankingResult
    {
        [DataMember] public UserPoint[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class LeaderboardData
    {
        [DataMember] public string iconURL { get; set; }
        [DataMember] public string leaderboardCode { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class LeaderboardPaging
    {
        [DataMember] public string First { get; set; }
        [DataMember] public string Last { get; set; }
        [DataMember] public string Next { get; set; }
        [DataMember] public string Previous { get; set; }
    }

    [DataContract]
    public class LeaderboardPagedList
    {
        [DataMember] public LeaderboardData[] data { get; set; }
        [DataMember] public LeaderboardPaging paging { get; set; }
    }
}
