// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    public enum RecordSetBy
    {
        CLIENT,
        SERVER
    }

    public static class RecordSetByExtensions
    {
        public static string GetString(this RecordSetBy me)
        {
            switch (me)
            {
                case RecordSetBy.CLIENT:
                    return "CLIENT";
                case RecordSetBy.SERVER:
                    return "SERVER";
                default:
                    return "NO VALUE GIVEN";
            }
        }
    }

    [DataContract, Preserve]
    public class UserRecord
    {
        [DataMember] public string key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string user_id;
        [DataMember] public Dictionary<string, object> value;
        [DataMember] public DateTime created_at;
        [DataMember] public DateTime updated_at;
        [DataMember] public bool is_public;
        [DataMember] public string set_by;
    }

    [DataContract, Preserve]
    public class PagingGameRecord
    {
        [DataMember] public string first;
        [DataMember] public string last;
        [DataMember] public string next;
        [DataMember] public string previous;
    }

    [DataContract, Preserve]
    public class GameRecordList 
    {
        [DataMember] public string[] data;
        [DataMember] public PagingGameRecord paging;

    }

    [DataContract, Preserve]
    public class GameRecord
    {
        [DataMember] public string key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public Dictionary<string, object> value;
        [DataMember] public DateTime created_at;
        [DataMember] public DateTime updated_at;
        [DataMember] public string set_by;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "ttl_config")] public TTLConfig TTLConfig;
    }

    [DataContract, Preserve]
    public class SaveRecordMetaDataBase
    {
        [DataMember(Name = "ttl_config")] public TTLConfig TTLConfig;
        [DataMember(Name = "tags")] public string[] Tags;
    }

    [DataContract, Preserve]
    public class SaveAdminRecordMetaData : SaveRecordMetaDataBase
    {
    }

    [DataContract, Preserve]
    public class SaveGameRecordMetaData : SaveRecordMetaDataBase
    {
        [DataMember(Name = "set_by")] public RecordSetBy SetBy;
    }

    [Preserve]
    public class MetadataOptionalParamsBase : OptionalParametersBase
    {
        /// <summary>
        /// Indicates the TTL configuration for the game record.
        /// </summary>
        public TTLConfig TTLConfig = null;

        /// <summary>
        /// Indicates the tagging for the game record.
        /// </summary>
        public string[] Tags = null;
    }

    [Preserve]
    public class AdminRecordMetadataOptionalParams : MetadataOptionalParamsBase
    {
    }

    [Preserve]
    public class GameRecordMetadataOptionalParams : MetadataOptionalParamsBase
    {
        /// <summary>
        /// Indicates which party that can modify the game record (Default: CLIENT).
        /// </summary>
        public RecordSetBy SetBy = RecordSetBy.CLIENT;
    }

    [DataContract, Preserve]
    public class ConcurrentReplaceRequest
    {        
        [DataMember] public DateTime updatedAt; // Time format should style: RFC3339
        [DataMember] public Dictionary<string, object> value;
    }
    
    [DataContract, Preserve]
    public class ConcurrentReplaceUserRecordResponse
    {        
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
    }

    [DataContract, Preserve]
    public class BulkGetPublicUserRecordsByUserIdsRequest
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
    }

    [DataContract, Preserve]
    public class BulkGetRecordsByKeyRequest
    {
        [DataMember] public string[] keys;
    }

    [DataContract, Preserve]
    public class UserRecords
    {
        [DataMember] public UserRecord[] data;
    }
    
    [DataContract, Preserve]
    public class GameRecords
    {
        [DataMember] public GameRecord[] data;
    }

    [DataContract, Preserve]
    public class PublicUserRecordKeys
    {
        [DataMember] public string Key;
        [DataMember(Name = "User_id")] public string UserId;
    }

    [DataContract, Preserve]
    public class PaginatedBulkGetPublicUserRecordKeys
    {
        [DataMember] public PublicUserRecordKeys[] Data;
        [DataMember] public Paging Paging;
    }

    [DataContract, Preserve]
    public class AdminGameRecord
    {
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "created_at")] public DateTime CreatedAt;
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
        [DataMember(Name = "value")] public Dictionary<string, object> Value;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "ttl_config")] public TTLConfig TTLConfig;
    }

    [DataContract, Preserve]
    public class BulkQueryAdminGameRecordRequest
    {
        [DataMember(Name = "keys")] public string[] Keys;
    }

    [DataContract, Preserve]
    public class BulkAdminGameRecordResponse
    {
        [DataMember(Name = "data")] public AdminGameRecord[] Data; 
    }

    [DataContract, Preserve]
    public class AdminUserRecord
    {
        [DataMember] public string Key;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "user_id")] public string UserId;
        [DataMember(Name = "created_at")] public DateTime CreatedAt;
        [DataMember(Name = "updated_at")] public DateTime UpdatedAt;
        [DataMember] public Dictionary<string, object> Value;
    }

    [DataContract, Preserve]
    public class AdminUserRecordKeys
    {
        [DataMember] public string Key;
        [DataMember(Name = "user_id")] public string UserId;
    }

    [DataContract, Preserve]
    public class PaginatedGetAdminUserRecordKeys
    {
        [DataMember] public AdminUserRecordKeys[] Data;
        [DataMember] public Paging Paging;
    }
}
