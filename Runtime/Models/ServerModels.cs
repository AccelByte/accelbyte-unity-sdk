// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MatchmakingStatus
    {
        none,
        matched,
        done, // when matchmaking request is done successfully
        cancel, // when matchmaking request is cancelled
        timeout, // when matchmaking request is timed out
        sessionInQueue, // when joinable session is in queue and players from other party can join
        sessionFull, // when joinable session is full, and removed from queue
        sessionTimeout // when joinable session is timed out, and removed from queue
    }

    [DataContract, Preserve]
    public class RegisterServerRequest
    {
        [DataMember] public string game_version;
        [DataMember] public string ip;
        [DataMember] public string pod_name;
        [DataMember] public int port;
        [DataMember]public string provider;

        [DataMember] public string custom_attribute;
    }

    [DataContract, Preserve]
    public class ShutdownServerRequest
    {
        [DataMember] public bool kill_me;
        [DataMember] public string pod_name;
        [DataMember] public string session_id;
    }

    [DataContract, Preserve]
    public class RegisterLocalServerRequest
    {
        [DataMember] public string ip;
        [DataMember] public string name;
        [DataMember] public uint port;
        [DataMember] public string custom_attribute;
    }

    [DataContract, Preserve]
    public class HeartBeatRequest
    {
        [DataMember(Name = "podName")] public string PodName;
    }

    [DataContract, Preserve]
    public class PartyMember
    {
        [DataMember] public string user_id;
        [DataMember] public Dictionary<string, object> extra_attributes;
    }

    [DataContract, Preserve]
    public class MatchParty
    {
        [DataMember] public string party_id;
        [DataMember] public PartyMember[] party_members;
        [DataMember] public Dictionary<string, object> party_attributes;
    }

    [DataContract, Preserve]
    public class MatchingAlly
    {
        [DataMember] public MatchParty[] matching_parties;
    }

    [DataContract, Preserve]
    public class MatchRequest
    {
        [DataMember] public string game_mode;
        [DataMember] public MatchingAlly[] matching_allies;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string session_id;
    }

    [DataContract, Preserve]
    public class DSMClient
    {
        [DataMember] public string host_address;
        [DataMember] public string region;
        [DataMember] public string status;
        [DataMember] public string provider;
    }

    [DataContract, Preserve]
    public class PubIp
    {
        [DataMember] public string ip;
    }

    [DataContract, Preserve]
    public class ServerInfo
    {
        [DataMember] public string pod_name;
        [DataMember] public string image_version;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string ip;
        [DataMember] public string[] alternate_ips;
        [DataMember] public int port;
        [DataMember] public string provider;
        [DataMember] public string game_version;
        [DataMember] public string status;
        [DataMember] public string last_update;
        [DataMember] public string Deployment;
    }

    [DataContract, Preserve]
    public class MatchmakingResult
    {
        [DataMember] public string channel;
        [DataMember] public string client_version;
        [DataMember] public string game_mode;
        [DataMember] public bool joinable;
        [DataMember] public string match_id;
        [DataMember] public MatchingAlly[] matching_allies;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public Dictionary<string, object> party_attributes;
        [DataMember] public string party_id;
        [DataMember] public int queued_at;
        [DataMember] public string region;
        [DataMember] public string server_name;
        [DataMember] public MatchmakingStatus status;
    }

    [DataContract, Preserve]
    public class AddUserIntoSessionRequest
    {
        [DataMember] public string user_id;
        [DataMember] public string party_id;
    }

    [DataContract, Preserve]
    public class DequeueRequest
    {
        [DataMember] public string match_id;
    }

    [DataContract, Preserve]
    public class ServerSessionResponse
    {
        [DataMember] public string session_id;
    }

    [DataContract, Preserve]
    public class ServerSessionTimeoutResponse
    {
        [DataMember(Name = "session_timeout")] public int SessionTimeout;
    }
}