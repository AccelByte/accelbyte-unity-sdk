// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    public enum UserRecordSetBy
    {
        CLIENT,
        SERVER
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

    [DataContract]
    public class ServerMetaRequest
    {
        [DataMember] public string set_by { get; set; }
        [DataMember] public bool is_public { get; set; }
    }

    [DataContract]
    public class UserMetaRequest
    {
        [DataMember] public bool is_public { get; set; }
    }
}
