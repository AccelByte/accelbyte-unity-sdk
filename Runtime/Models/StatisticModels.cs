﻿// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
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

    public enum StatisticUpdateStrategy
    {
        OVERRIDE,
        INCREMENT,
        MIN,
        MAX
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
        [DataMember] public string additionalKey {get; set;}
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
}