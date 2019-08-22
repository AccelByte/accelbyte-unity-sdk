// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    public enum StatisticSetBy
    {
        CLIENT,
        SERVER
    }

    public enum StatisticStatus
    {
        INIT,
        TIED
    }

    [DataContract]
    public class StatInfo
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
    }

    [DataContract]
    public class StatItemInfo
    {
        [DataMember] public string createdAt { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string profileId { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string statName { get; set; }
        [DataMember] public string updatedAt { get; set; }
        [DataMember] public float value { get; set; }
    }

    [DataContract]
    public class StatItemIncResult
    {
        [DataMember] public float currentValue { get; set; }
    }

    [DataContract]
    public class StatItemPagingSlicedResult
    {
        [DataMember] public StatItemInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class BulkUserStatItemInc
    {
        [DataMember] public float inc { get; set; }
        [DataMember] public string profileId { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class BulkStatItemInc
    {
        [DataMember] public float inc { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class BulkStatItemOperationResult
    {
        [DataMember] public StatItemIncResult detail { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public bool success { get; set; }
    }
}