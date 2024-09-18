// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public enum P2PConnectionType
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "host")] Host,
        [EnumMember(Value = "srflx")] Srflx,
        [EnumMember(Value = "prflx")] Prflx,
        [EnumMember(Value = "relay")] Relay
    }
    
    [DataContract, Preserve]
    public enum TurnServerStatus
    {
        [EnumMember(Value = "UNREACHABLE")] UNREACHABLE,
        [EnumMember(Value = "ACTIVE")] ACTIVE
    }

    [DataContract, Preserve]
    public class TurnServerList
    {
        [DataMember] public TurnServer[] servers;
    }

    [DataContract, Preserve]
    public class TurnServer
    {
        [DataMember(Name = "alias")] public string Alias;
        [DataMember(Name = "cpu_usage")] public float CpuUsage;
        [DataMember(Name = "mem_usage")] public TurnServerMemoryUsage MemUsage;
        [DataMember(Name = "net_usage")] public TurnServerNetUsage NetUsage;
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public int qos_port;
        [DataMember] public string region;
        [DataMember] public TurnServerStatus status;
        [DataMember] public string last_update;
        [DataMember] public int current_time;
    }

    [DataContract, Preserve]
    public class TurnServerMemoryUsage
    {
        [DataMember(Name = "total")] public int Total;
        [DataMember(Name = "used")] public int Used;
    }

    [DataContract, Preserve]
    public class TurnServerNetUsage
    {
        [DataMember(Name = "rx")] public int Rx;
        [DataMember(Name = "tx")] public int Tx;
    }

    [DataContract, Preserve]
    public class TurnServerCredential
    {
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public string region;
        [DataMember] public string username;
        [DataMember] public string password;
    }

    [DataContract, Preserve]
    public class TurnServerMetricRequest
    {
        [DataMember(Name = "region")] public string Region;
        [DataMember(Name = "type")] public P2PConnectionType Type;
    }

    [DataContract, Preserve]
    public class DisconnectTurnServerRequest
    {
        [DataMember(Name = "user_id")] public string UserId;
    }
}
