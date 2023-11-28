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
    #region enum

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCSortBy
    {
        NONE,
        NAME = 0,
        DOWNLOAD,
        LIKE,
        DATE,
        UPDATED_TIME
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCLikedContentSortBy
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCFileExtension
    {
        PJP,
        JPG,
        JPEG,
        JFIF,
        BMP,
        PNG
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCContentSortBy
    {
        Name,
        NameAsc,
        NameDesc,
        Download,
        DownloadAsc,
        DownloadDesc,
        Like,
        LikeAsc,
        LikeDesc,
        CreatedTime,
        CreatedTimeAsc,
        CreatedTimeDesc
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UGCContentDownloaderSortBy
    {
        CreatedTime,
        CreatedTimeAsc,
        CreatedTimeDesc
    }

    #endregion

    [DataContract, Preserve]
    public class UGCPayloadUrl
    {
        [DataMember] public string source;
        [DataMember] public string url;
    }

    [DataContract, Preserve]
    public class UGCCreatorState
    {
        [DataMember] public bool state;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class UGCTagResponse
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string tag;
    }

    [DataContract, Preserve]
    public class UpdateChannelRequest
    {
        [DataMember(Name = "name")] public string Name;
    }

    public class UpdateChannelParameter
    {
        public string UserId;
        public string ChannelId;
    }

    [DataContract, Preserve]
    public class UGCChannelResponse
    {
        [DataMember] public string id;
        [DataMember] public string name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class UGCTypeResponse
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string[] subtype;
        [DataMember] public string type;
    }

    [DataContract, Preserve]
    public class UGCPreviewMetadata
    {
        [DataMember] public string PreviewContentType;
        [DataMember] public string PreviewFileExtension;
    }

    [DataContract, Preserve]
    public class UGCRequest
    {
        [DataMember] public string contentType;
        [DataMember] public string fileExtension;
        [DataMember] public string name;
        [DataMember] public string preview;
        [DataMember] public string type;
        [DataMember] public string subtype;
        [DataMember] public string[] tags;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public UGCPreviewMetadata PreviewMetadata;
    }

    [DataContract, Preserve]
    public class UGCUpdateRequest
    {
        [DataMember] public string ContentType;
        [DataMember] public string FileExtension;
        [DataMember] public string Name;
        [DataMember] public string Preview;
        [DataMember] public string Type;
        [DataMember] public string Subtype;
        [DataMember] public string[] Tags;
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public UGCPreviewMetadata PreviewMetadata;
        [DataMember] public bool UpdateContentFile;
    }

    [DataContract, Preserve]
    public class UGCResponse
    {
        [DataMember] public string channelId;
        [DataMember] public string contentType;
        [DataMember] public DateTime createdTime;
        [DataMember] public string creatorName;
        [DataMember] public string fileExtension;
        [DataMember] public string id;
        [DataMember] public bool isOfficial;
        [DataMember] public string name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public UGCPayloadUrl[] payloadURL;
        [DataMember] public string preview;
        [DataMember] public string shareCode;
        [DataMember] public string subType;
        [DataMember] public string[] tags;
        [DataMember] public string type;
        [DataMember] public string userId;
        [DataMember] public Dictionary<string, object> customAttributes;
    }

    [DataContract, Preserve]
    public class UGCContentResponse
    {
        [DataMember] public string channelId;
        [DataMember] public DateTime createdTime;
        [DataMember] public UGCCreatorState creatorFollowState;
        [DataMember] public string creatorName;
        [DataMember] public int downloadCount;
        [DataMember] public string fileExtension;
        [DataMember] public string[] Groups;
        [DataMember] public string id;
        [DataMember] public bool IsHidden;
        [DataMember] public bool isOfficial;
        [DataMember] public int likeCount;
        [DataMember] public UGCCreatorState likeState;
        [DataMember] public string name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string payload;
        [DataMember] public UGCPayloadUrl[] payloadURL;
        [DataMember] public PayLoadUrl[] PreviewUrl;
        [DataMember] public Screenshots[] Screenshots;
        [DataMember] public string shareCode;
        [DataMember] public string subType;
        [DataMember] public string[] tags;
        [DataMember] public string type;
        [DataMember] public DateTime UpdatedTime;
        [DataMember] public string userId;
        [DataMember] public Dictionary<string, object> customAttributes;
    }

    [DataContract, Preserve]
    public class UGCTagsPagingResponse
    {
        [DataMember] public UGCTagResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UGCTypesPagingResponse
    {
        [DataMember] public UGCTypeResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UGCChannelPagingResponse
    {
        [DataMember] public UGCChannelResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UGCPreview
    {
        [DataMember] public string preview;
    }

    [DataContract, Preserve]
    public class UserIdState
    {
        [DataMember] public bool state;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class UGCGetCreatorStatsResponse
    {
        [DataMember] public UserIdState creatorFollowState;
        [DataMember] public int followCount;
        [DataMember] public int followingCount;
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public int totalLikedContent;
    }

    [DataContract, Preserve]
    public class PayLoadUrl
    {
        [DataMember] public string source;
        [DataMember] public string url;
    }

    [DataContract, Preserve]
    public class ScreenshotRequest
    {
        [DataMember] public string contentType;
        [DataMember] public string description;
        [DataMember] public UGCFileExtension fileExtension;
    }

    [DataContract, Preserve]
    public class ScreenshotsRequest
    {
        [DataMember] public ScreenshotRequest[] screenshots;
    }

    [DataContract, Preserve]
    public class Screenshots
    {
        [DataMember] public string contentType;
        [DataMember] public string description;
        [DataMember] public string fileExtension;
        [DataMember] public string screenshootId;
        [DataMember] public string source;
        [DataMember] public string url;
    }

    [DataContract, Preserve]
    public class ScreenshotsResponse
    {
        [DataMember] public Screenshots[] screenshots;
    }

    [DataContract, Preserve]
    public class ScreenshotsV2
    {
        [DataMember] public string ContentType;
        [DataMember] public string Description;
        [DataMember] public string FileExtension;
        [DataMember] public string ScreenshotId;
        [DataMember] public string Source;
        [DataMember] public string Url;
    }

    [DataContract, Preserve]
    public class ScreenshotsResponseV2
    {
        [DataMember] public ScreenshotsV2[] Screenshots;
    }

    [DataContract, Preserve]
    public class SearchContentRequest
    {
        [DataMember] public string name;
        [DataMember] public string creator;
        [DataMember] public string type;
        [DataMember] public string subtype;
        [DataMember] public string[] tags;
        [DataMember] public bool isOfficial;
        [DataMember] public bool IsHidden;
        [DataMember] public UGCSortBy sortBy = UGCSortBy.DATE; // Default to sorting criteria = date
        [DataMember] public UGCOrderBy orderBy = UGCOrderBy.DESC; // Default to sorting order = desc
        [DataMember] public int limit = 1000; // Default value = 1000
        [DataMember] public int offset = 0; // Default value = 0
    }

    [DataContract, Preserve]
    public class GetLikedContentRequest
    {
        [DataMember] public string[] tags;
        [DataMember] public string name;
        [DataMember] public string type;
        [DataMember] public string subtype;
        [DataMember] public bool isOfficial;
        [DataMember] public int limit = 1000; // Default value = 1000
        [DataMember] public int offset = 0; // Default value = 0
        [DataMember] public UGCSortBy sortBy = UGCSortBy.DATE; // Default to sorting criteria = date
        [DataMember] public UGCOrderBy orderBy = UGCOrderBy.DESC; // Default to sorting order = desc
    }

    [DataContract, Preserve]
    public class GetAllLikedContentRequest
    {
        [DataMember] public string[] Tags;
        [DataMember] public string Name;
        [DataMember] public string Type;
        [DataMember] public string Subtype;
        [DataMember] public bool IsOfficial;
        [DataMember] public int Limit = 1000; // Default value = 1000
        [DataMember] public int Offset = 0; // Default value = 0
        [DataMember] public UGCLikedContentSortBy SortBy = UGCLikedContentSortBy.DATE; // Default to sorting criteria = date
        [DataMember] public UGCOrderBy OrderBy = UGCOrderBy.DESC; // Default to sorting order = desc
    }

    [DataContract, Preserve]
    public class GetBulkContentIdRequest
    {
        [DataMember(Name = "contentIds")] public string[] ContentId;
    }

    [DataContract, Preserve]
    public class UGCModelsContentsResponse
    {
        [DataMember] public string channelId;
        [DataMember] public string createdTime;
        [DataMember] public UserIdState creatorFollowState;
        [DataMember] public string creatorName;
        [DataMember] public int downloadCount;
        [DataMember] public string fileExtension;
        [DataMember] public string[] groups;
        [DataMember] public string id;
        [DataMember] public bool isHidden;
        [DataMember] public bool isOfficial;
        [DataMember] public int likeCount;
        [DataMember] public UserIdState likeState;
        [DataMember] public string name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string payload;
        [DataMember] public PayLoadUrl[] payloadUrl;
        [DataMember] public PayLoadUrl[] previewUrl;
        [DataMember] public Screenshots[] screenshots;
        [DataMember] public string shareCode;
        [DataMember] public string subType;
        [DataMember] public string[] tags;
        [DataMember] public string type;
        [DataMember] public string updatedTime;
        [DataMember] public string userId;
        [DataMember] public Dictionary<string, object> customAttributes;
    }

    [DataContract, Preserve]
    public class UGCSearchContentsPagingResponse
    {
        [DataMember] public UGCModelsContentsResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UGCContentsPagingResponse
    {
        [DataMember] public UGCModelsContentsResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UpdateLikeStatusToContentRequest
    {
        [DataMember(Name = "likeStatus")] public bool LikeStatus;
    }

    public class UpdateLikeStatusToContentParameter
    {
        public string ContentId;
    }

    [DataContract, Preserve]
    public class UGCUpdateLikeStatusToContentResponse
    {
        [DataMember] public string contentId;
        [DataMember] public bool likeStatus;
    }

    [DataContract, Preserve]
    public class UGCGetListFollowersResponse
    {
        [DataMember] public int followCount;
        [DataMember] public int followingCount;
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public int totalLikedContent;
    }

    [DataContract, Preserve]
    public class UGCGetListFollowersPagingResponse
    {
        [DataMember] public UGCGetListFollowersResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UpdateFollowStatusRequest
    {
        [DataMember(Name = "followStatus")] public bool FollowStatus;
    }

    public class UpdateFollowStatusParameter
    {
        public string UserId;
    }

    [DataContract, Preserve]
    public class UGCUpdateFollowStatusToUserResponse
    {
        [DataMember] public bool followStatus;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class UGCGetUserGroupsResponse
    {
        [DataMember] public string[] contents;
        [DataMember] public string createdAt;
        [DataMember] public string id;
        [DataMember] public string name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class UGCGetUserGroupsPagingResponse
    {
        [DataMember] public UGCGetUserGroupsResponse[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class UGCModelsContentsResponseV2
    {
        [DataMember] public string ChannelId;
        [DataMember] public string CreatedTime;
        [DataMember] public UserIdState CreatorFollowState;
        [DataMember] public int DownloadCount;
        [DataMember] public string FileExtension;
        [DataMember] public string[] Groups;
        [DataMember] public string Id;
        [DataMember] public bool IsHidden;
        [DataMember] public bool IsOfficial;
        [DataMember] public int LikeCount;
        [DataMember] public UserIdState LikeState;
        [DataMember] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public PayLoadUrl[] PayloadUrl;
        [DataMember] public ScreenshotsV2[] Screenshots;
        [DataMember] public string ShareCode;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
        [DataMember] public string UpdatedTime;
        [DataMember] public string UserId;
        [DataMember] public Dictionary<string, object> CustomAttributes;
    }

    [DataContract, Preserve]
    public class UGCSearchContentsPagingResponseV2
    {
        [DataMember] public UGCModelsContentsResponseV2[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UGCGetContentFilterRequestV2
    {
        [DataMember] public string Name;
        [DataMember] public string Type;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
    }

    [DataContract, Preserve]
    public class CreateUGCRequestV2
    {
        [DataMember] public string ContentType;
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public string FileExtension;
        [DataMember] public string Name;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
    }

    [DataContract, Preserve]
    public class UGCModelsCreateUGCResponseV2
    {
        [DataMember] public string ChannelId;
        [DataMember] public string ContentType;
        [DataMember] public DateTime CreatedTime;
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public string FileExtension;
        [DataMember] public string FileLocation;
        [DataMember] public string Id;
        [DataMember] public bool IsHidden;
        [DataMember] public bool IsOfficial;
        [DataMember] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string ParentNamespace;
        [DataMember] public UGCPayloadUrl[] PayloadURL;
        [DataMember] public string ShareCode;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
        [DataMember] public DateTime UpdatedTime;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class ModifyUGCRequestV2
    {
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public string Name;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
    }

    [DataContract, Preserve]
    public class UGCModelsModifyUGCResponseV2
    {
        [DataMember] public string ChannelId;
        [DataMember] public string ContentType;
        [DataMember] public DateTime CreatedTime;
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public string FileExtension;
        [DataMember] public string Id;
        [DataMember] public bool IsHidden;
        [DataMember] public bool IsOfficial;
        [DataMember] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string ParentNamespace;
        [DataMember] public UGCPayloadUrl[] PayloadURL;
        [DataMember] public string ShareCode;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
        [DataMember] public DateTime UpdatedTime;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class UGCUploadContentURLRequestV2
    {
        [DataMember] public string ContentType;
        [DataMember] public string FileExtension;
    }

    [DataContract, Preserve]
    public class UGCModelsUploadContentURLResponseV2
    {
        [DataMember] public string ContentType;
        [DataMember] public string FileExtension;
        [DataMember] public string Url;
        [DataMember] public string FileLocation;
    }

    [DataContract, Preserve]
    public class UpdateContentFileLocationRequestV2
    {
        [DataMember] public string FileExtension;
        [DataMember] public string FileLocation;
    }

    [DataContract, Preserve]
    public class UpdateContentFileLocationResponseV2
    {
        [DataMember] public string ChannelId;
        [DataMember] public string ContentType;
        [DataMember] public DateTime CreatedTime;
        [DataMember] public Dictionary<string, object> CustomAttributes;
        [DataMember] public string FileExtension;
        [DataMember] public string FileLocation;
        [DataMember] public string Id;
        [DataMember] public bool IsHidden;
        [DataMember] public bool IsOfficial;
        [DataMember] public string Name;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string ParentNamespace;
        [DataMember] public UGCPayloadUrl[] PayloadURL;
        [DataMember] public string ShareCode;
        [DataMember] public string SubType;
        [DataMember] public string[] Tags;
        [DataMember] public string Type;
        [DataMember] public DateTime UpdatedTime;
        [DataMember] public string UserId;
    }

    [DataContract, Preserve]
    public class ScreenshotUpdateV2
    {
        [DataMember] public string Description;
        [DataMember] public string ScreenshotId;
    }

    [DataContract, Preserve]
    public class ScreenshotsUpdatesV2
    {
        [DataMember] public ScreenshotUpdateV2[] Screenshots;
    }

    [DataContract, Preserve]
    public class UGCModelsAddDownloadContentCountResponseV2
    {
        [DataMember] public string ContentId;
    }

    [DataContract, Preserve]
    public class UGCModelsUGCContentDownloader
    {
        [DataMember] public string ContentId;
        [DataMember] public DateTime CreatedTime;
        [DataMember] public string DownloadedBy;
        [DataMember(Name = "namespace")] public string Namespace;
    }

    [DataContract, Preserve]
    public class UGCModelsPaginatedContentDownloaderResponseV2
    {
        [DataMember] public UGCModelsUGCContentDownloader[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class UGCModelsUGCContentLiker
    {
        [DataMember] public string ContentId;
        [DataMember] public DateTime CreatedTime;
        [DataMember] public string LikedBy;
        [DataMember(Name = "namespace")] public string Namespace;
    }

    [DataContract, Preserve]
    public class UGCModelsPaginatedContentLikerResponseV2
    {
        [DataMember] public UGCModelsUGCContentLiker[] Data;
        [DataMember] public Paging Paging;
    }
}
