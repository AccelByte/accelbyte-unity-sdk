// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public enum TurnServerStatus
    {
        [EnumMember(Value = "UNREACHABLE")] UNREACHABLE,
        [EnumMember(Value = "ACTIVE")] ACTIVE
    }

    [DataContract]
    public class TurnServerList
    {
        [DataMember] public TurnServer[] servers { get; set; }
    }

    [DataContract]
    public class TurnServer
    {
        [DataMember] public string ip { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember] public int qos_port { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public TurnServerStatus status { get; set; }
        [DataMember] public string last_update { get; set; }
        [DataMember] public int current_time { get; set; }
    }

    [DataContract]
    public class TurnServerCredential
    {
        [DataMember] public string ip { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string username { get; set; }
        [DataMember] public string password { get; set; }
    }
}
