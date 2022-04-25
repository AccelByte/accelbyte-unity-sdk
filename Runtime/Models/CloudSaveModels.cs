// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

    [DataContract]
    public class UserRecord
    {
        [DataMember] public string key { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string user_id { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
        [DataMember] public DateTime created_at { get; set; }
        [DataMember] public DateTime updated_at { get; set; }
        [DataMember] public bool is_public { get; set; }
        [DataMember] public string set_by { get; set; }
    }

    [DataContract]
    public class PagingGameRecord
    {
        [DataMember] public string first { get; set; }
        [DataMember] public string last { get; set; }
        [DataMember] public string next { get; set; }
        [DataMember] public string previous { get; set; }
    }

    [DataContract]
    public class GameRecordList 
    {
        [DataMember] public string[] data { get; set; }
        [DataMember] public PagingGameRecord paging { get; set; }

    }

    [DataContract]
    public class GameRecord
    {
        [DataMember] public string key { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
        [DataMember] public DateTime created_at { get; set; }
        [DataMember] public DateTime updated_at { get; set; }
        [DataMember] public string set_by { get; set; }

    }

    [DataContract]
    public class ConcurrentReplaceRequest
    {
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
    }


}
