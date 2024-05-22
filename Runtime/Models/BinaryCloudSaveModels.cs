// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TTLConfigAction
    {
        None,
        [EnumMember(Value = "DELETE")]
        Delete,
    }

    [DataContract, Preserve]
    public class SaveUserBinaryRecordRequest
    {
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "is_public")] public bool IsPublic;
        [DataMember(Name = "file_type")] public string FileType;
    }

    [DataContract, Preserve]
    public class BulkQueryBinaryRecordsRequest
    {
        [DataMember(Name = "query")] public string Query;
        [DataMember(Name = "offset")] public int Offset;
        [DataMember(Name = "limit")] public int Limit;
    }

    [DataContract, Preserve]
    public class BulkQueryPublicUserBinaryRecordsRequest
    {
        [DataMember(Name = "offset")] public int Offset;
        [DataMember(Name = "query")] public int Limit;
    }

    [DataContract, Preserve]
    public class UpdateUserBinaryRecordFileRequest
    {
        [DataMember(Name = "content_type")] public string ContentType;
        [DataMember(Name = "file_location")] public string FileLocation;
    }

    [DataContract, Preserve]
    public class UpdateUserBinaryRecordMetadataRequest
    {
        [DataMember(Name = "is_public")] public bool IsPublic;
    }

    [DataContract, Preserve]
    public class RequestUserBinaryRecordPresignedUrlRequest
    {
        [DataMember(Name = "file_type")] public string FileType;
    }

    [DataContract, Preserve]
    public class BinaryInfo
    {
        [DataMember(Name = "content_type")] public string ContentType;
        [DataMember(Name = "file_location")] public string FileLocation;
        [DataMember(Name = "url")] public string Url;
        [DataMember(Name = "version")] public int Version;
        [DataMember(Name = "created_at")] public DateTime CreatedAt;
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class UserBinaryRecord
    {
        [DataMember(Name = "binary_info")] public BinaryInfo BinaryInfo;
        [DataMember(Name = "created_at")] public DateTime CreatedAt;
        [DataMember(Name = "is_public")] public bool IsPublic;
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "set_by")] public string SetBy;
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
        [DataMember(Name = "user_id")] public string UserId;
    }

    [DataContract, Preserve]
    public class GameBinaryRecord
    {
        [DataMember(Name = "binary_info")] public BinaryInfo BinaryInfo;
        [DataMember(Name = "created_at")] public DateTime CreatedAt;
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "set_by")] public string SetBy;
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class TTLConfig
    {
        [DataMember(Name = "action")] public TTLConfigAction Action;
        [DataMember(Name = "expires_at")] public DateTime ExpiresAt;
    }

    [DataContract, Preserve]
    internal class CreateAdminGameBinaryRecordRequest
    {
        [DataMember(Name = "file_type")] public FileType FileType;
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "set_by")] public RecordSetBy SetBy;
        [DataMember(Name = "ttl_config")] public TTLConfig TtlConfig;
    }

    [DataContract, Preserve]
    internal class UpdateGameBinaryRecordFileRequest
    {
        [DataMember(Name = "content_type")] public FileType ContentType;
        [DataMember(Name = "file_location")] public string FileLocation;
    }

    [DataContract, Preserve]
    internal class UpdateGameBinaryRecordMetadataRequest
    {
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "set_by")] public RecordSetBy SetBy;
        [DataMember(Name = "ttl_config")] public TTLConfig TtlConfig;
    }

    [DataContract, Preserve]
    public class ListUserBinaryRecords
    {
        [DataMember(Name = "data")] public UserBinaryRecord[] Data;
    }

    [DataContract, Preserve]
    public class ListGameBinaryRecords
    {
        [DataMember(Name = "data")] public GameBinaryRecord[] Data;
    }

    [DataContract, Preserve]
    public class PaginatedUserBinaryRecords
    {
        [DataMember(Name = "data")] public UserBinaryRecord[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class PaginatedGameBinaryRecords
    {
        [DataMember(Name = "data")] public GameBinaryRecord[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }
}