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

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ReportingCategory
    {
        UGC,
        USER,
        CHAT
    }
    
    [DataContract, Preserve]
    public class ReportingSubmitDataBase
    {
        [DataMember] public ReportingCategory category;
        [DataMember] public string comment;
        [DataMember] public string objectId;
        [DataMember] public string objectType;
        [DataMember] public string reason;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class ReportingAdditionalInfo
    {
        [DataMember] public string[] screenshots;
    }

    [DataContract, Preserve]
    public class ReportingSubmitData : ReportingSubmitDataBase
    {
        [DataMember] public ReportingAdditionalInfo additionalInfo;
    }
    
    [DataContract, Preserve]
    public class ReportingAdditionalInfoChat
    {
        [DataMember] public string topicId;
        
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime chatCreatedAt;
    }

    [DataContract, Preserve]
    public class ReportingSubmitDataChat : ReportingSubmitDataBase
    {
        [DataMember] public ReportingAdditionalInfoChat additionalInfo;
    }

    [DataContract, Preserve]
    public class ReportingSubmitResponse
    {
        [DataMember] public ReportingCategory category;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string objectId;
        [DataMember] public string objectType;
        [DataMember] public string status;
        [DataMember] public string ticketId;
        [DataMember] public string updateAt;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class ReportingReasonItem
    {
        [DataMember] public string title;
        [DataMember] public string description;
    }

    [DataContract, Preserve]
    public class ReportingReasonsResponse
    {
        [DataMember] public ReportingReasonItem[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class ReportingReasonGroupItem
    {
        [DataMember] public string id;
        [DataMember] public string title;
    }

    [DataContract, Preserve]
    public class ReportingReasonGroupsResponse
    {
        [DataMember] public ReportingReasonGroupItem[] data;
        [DataMember] public Paging paging;
    }

}
