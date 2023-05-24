// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
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

    }

    [DataContract, Preserve]
    public class ConcurrentReplaceRequest
    {        
        [DataMember] public DateTime updatedAt; // Time format should style: RFC3339
        [DataMember] public Dictionary<string, object> value;
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
}
