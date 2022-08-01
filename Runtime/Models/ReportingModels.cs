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

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ReportingCategory
    {
        UGC,
        USER
    }

    [DataContract]
    public class ReportingAdditionalInfo
    {
        [DataMember] public string[] screenshots { get; set; }
    }

    [DataContract]
    public class ReportingSubmitData
    {
        [DataMember] public ReportingAdditionalInfo additionalInfo { get; set; }
        [DataMember] public ReportingCategory category { get; set; }
        [DataMember] public string comment { get; set; }
        [DataMember] public string objectId { get; set; }
        [DataMember] public string objectType { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class ReportingSubmitResponse
    {
        [DataMember] public ReportingCategory category { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string objectId { get; set; }
        [DataMember] public string objectType { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string ticketId { get; set; }
        [DataMember] public string updateAt { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class ReportingReasonItem
    {
        [DataMember] public string title { get; set; }
        [DataMember] public string description { get; set; }
    }

    [DataContract]
    public class ReportingReasonsResponse
    {
        [DataMember] public ReportingReasonItem[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class ReportingReasonGroupItem
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string title { get; set; }
    }

    [DataContract]
    public class ReportingReasonGroupsResponse
    {
        [DataMember] public ReportingReasonGroupItem[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

}
