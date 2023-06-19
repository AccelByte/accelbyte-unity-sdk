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

    [DataContract, Preserve]
    public class UserPoint
    {
        [DataMember] public float point;
        [DataMember] public string userId;
        [DataMember] public bool hidden;
        [DataMember] public Dictionary<string, object> additionalData;
    }

    [DataContract, Preserve]
    public class UserRanking
    {
        [DataMember] public float point;
        [DataMember] public int rank;
        [DataMember] public bool hidden;
        [DataMember] public Dictionary<string, object> additionalData {get; set;}
    }

    [DataContract, Preserve]
    public class UserRankingData
    {
        [DataMember] public UserRanking allTime;
        [DataMember] public UserRanking current;
        [DataMember] public UserRanking daily;
        [DataMember] public UserRanking monthly;
        [DataMember] public string userId;
        [DataMember] public UserRanking weekly;
    }

    [DataContract, Preserve]
    public class LeaderboardRankingResult
    {
        [DataMember] public UserPoint[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class LeaderboardData
    {
        [DataMember] public string iconURL;
        [DataMember] public string leaderboardCode;
        [DataMember] public string name;
        [DataMember] public string statCode;
    }

    [DataContract, Preserve]
    public class LeaderboardPaging
    {
        [DataMember] public string First;
        [DataMember] public string Last;
        [DataMember] public string Next;
        [DataMember] public string Previous;
    }

    [DataContract, Preserve]
    public class LeaderboardPagedList
    {
        [DataMember] public LeaderboardData[] data;
        [DataMember] public LeaderboardPaging paging;
    }

    [DataContract, Preserve]
    public class LeaderboardDataV3
    {
        [DataMember] public bool Descending;
        [DataMember] public string IconUrl;
        [DataMember] public bool IsDeleted;
        [DataMember] public string LeaderboardCode;
        [DataMember] public string Name;
        [DataMember] public string Description;
        [DataMember] public string StatCode;
        [DataMember] public string[] CycleIds;
        [DataMember] public bool AllTime;
        [DataMember] public string CreatedAt;
        [DataMember] public string UpdatedAt;
        [DataMember] public string DeletedAt;
    }

    [DataContract, Preserve]
    public class LeaderboardPagedListV3
    {
        [DataMember] public LeaderboardDataV3[] Data;
        [DataMember] public Paging Paging;
    }
    
    [DataContract, Preserve]
    public class UserCycleRanking
    {
        [DataMember] public float Point;
        [DataMember] public int Rank;
        [DataMember] public bool Hidden;
        [DataMember] public Dictionary<string, object> AdditionalData;
        [DataMember] public string CycleId;
    }
    
    [DataContract, Preserve]
    public class UserRankingDataV3
    {
        [DataMember] public UserRanking AllTime;
        [DataMember] public UserCycleRanking[] Cycles;
        [DataMember] public string UserId;
    }

}
