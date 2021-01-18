// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class UserRecord
    {
        [DataMember] public string key { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string user_id { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
        [DataMember] public DateTime created_at { get; set; }
        [DataMember] public DateTime updated_at { get; set; }
    }

    [DataContract]
    public class GameRecord
    {
        [DataMember] public string key { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
        [DataMember] public DateTime created_at { get; set; }
        [DataMember] public DateTime updated_at { get; set; }
    }

    [DataContract]
    public class ConcurrentReplaceRequest
    {
        [DataMember] public DateTime updatedAt { get; set; }
        [DataMember] public Dictionary<string, object> value { get; set; }
    }
}
