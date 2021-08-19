// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class UGCPayloadUrl
    {
        [DataMember] public string source { get; set; }
        [DataMember] public string url { get; set; }
    }

    [DataContract]
    public class UGCCreatorState
    {
        [DataMember] public bool state { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class UGCTagResponse
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string tag { get; set; }
    }

    [DataContract]
    public class UGCChannelResponse
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class UGCTypeResponse
    {
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string[] subtype { get; set; }
        [DataMember] public string type { get; set; }
    }

    [DataContract]
    public class UGCRequest
    {
        [DataMember] public string fileExtension { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string preview { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string subtype { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class UGCResponse
    {
        [DataMember] public string channelId { get; set; }
        [DataMember] public DateTime createdTime { get; set; }
        [DataMember] public string creatorName { get; set; }
        [DataMember] public string fileExtension { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember] public bool isOfficial { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public UGCPayloadUrl[] payloadURL { get; set; }
        [DataMember] public string preview { get; set; }
        [DataMember] public string shareCode { get; set; }
        [DataMember] public string subType { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class UGCContentResponse
    {
        [DataMember] public string channelId { get; set; }
        [DataMember] public DateTime createdTime { get; set; }
        [DataMember] public UGCCreatorState creatorFollowState { get; set; }
        [DataMember] public string creatorName { get; set; }
        [DataMember] public int downloadCount { get; set; }
        [DataMember] public string fileExtension { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember] public bool isOfficial { get; set; }
        [DataMember] public int likeCount { get; set; }
        [DataMember] public UGCCreatorState likeState { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string payload { get; set; }
        [DataMember] public UGCPayloadUrl[] payloadURL { get; set; }
        [DataMember] public string shareCode { get; set; }
        [DataMember] public string subType { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class UGCTagsPagingResponse
    {
        [DataMember] public UGCTagResponse[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UGCTypesPagingResponse
    {
        [DataMember] public UGCTypeResponse[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UGCChannelPagingResponse
    {
        [DataMember] public UGCChannelResponse[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UGCPreview
    {
        [DataMember] public string preview { get; set; }
    }

}
