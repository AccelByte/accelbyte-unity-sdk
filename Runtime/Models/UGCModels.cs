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
    #region enum

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCSortBy
    {
        NONE,
        NAME = 0,
        DOWNLOAD,
        LIKE,
        DATE
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCOrderBy
    {
        NONE,
        ASC = 0,
        DESC
    }

    #endregion

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
        [DataMember] public string contentType { get; set; }
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
        [DataMember] public string contentType { get; set; }
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

    [DataContract]
    public class UserIdState
    {
        [DataMember] public bool state { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class PayLoadUrl
    {
        [DataMember] public string source { get; set; }
        [DataMember] public string url { get; set; }
    }

    [DataContract]
    public class Screenshots
    {
        [DataMember] public string contentType { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string fileExtension { get; set; }
        [DataMember] public string screenshootId { get; set; }
        [DataMember] public string source { get; set; }
        [DataMember] public string url { get; set; }
    }

    [DataContract]
    public class SearchContentRequest
    {
        [DataMember] public string name { get; set; }
        [DataMember] public string creator { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string subtype { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public bool isOfficial { get; set; }
        [DataMember] public UGCSortBy sortBy { get; set; } = UGCSortBy.DATE; //default to sorting criteria = date
        [DataMember] public UGCOrderBy orderBy { get; set; } = UGCOrderBy.DESC; //default to sorting order = desc
        [DataMember] public int limit { get; set; } = 1000; // Default value = 1000
        [DataMember] public int offset { get; set; } = 0; //Default value = 0
    }

    [DataContract]
    public class UGCSearchContentsResponse
    {
        [DataMember] public string channelId { get; set; }
        [DataMember] public string createdTime { get; set; }
        [DataMember] public UserIdState creatorFollowState { get; set; }
        [DataMember] public string creatorName { get; set; }
        [DataMember] public int downloadCount { get; set; }
        [DataMember] public string fileExtension { get; set; }
        [DataMember] public string[] groups { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember] public bool isHidden { get; set; }
        [DataMember] public bool isOfficial { get; set; }
        [DataMember] public int likeCount { get; set; }
        [DataMember] public UserIdState likeState { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string payload { get; set; }
        [DataMember] public PayLoadUrl[] payloadUrl { get; set; }
        [DataMember] public PayLoadUrl[] previewUrl { get; set; }
        [DataMember] public Screenshots[] screenshots { get; set; }
        [DataMember] public string shareCode { get; set; }
        [DataMember] public string subType { get; set; }
        [DataMember] public string[] tags { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public string updatedTime { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class UGCSearchContentsPagingResponse
    {
        [DataMember] public UGCSearchContentsResponse[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UGCUpdateLikeStatusToContentResponse
    {
        [DataMember] public string contentId { get; set; }
        [DataMember] public bool likeStatus { get; set; }
    }

    [DataContract]
    public class UGCGetListFollowersResponse
    {
        [DataMember] public int followCount { get; set; }
        [DataMember] public int followingCount { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public int totalLikedContent { get; set; }
    }

    [DataContract]
    public class UGCGetListFollowersPagingResponse
    {
        [DataMember] public UGCGetListFollowersResponse[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class UGCUpdateFollowStatusToUserResponse
    {
        [DataMember] public bool followStatus { get; set; }
        [DataMember] public string userId { get; set; }
    }
}
