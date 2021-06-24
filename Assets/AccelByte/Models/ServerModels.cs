// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
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

    [DataContract]
    public class RegisterServerRequest
    {
        [DataMember] public string game_version { get; set; }
        [DataMember] public string ip { get; set; }
        [DataMember] public string pod_name { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember]public string provider { get; set; }
    }

    [DataContract]
    public class ShutdownServerRequest
    {
        [DataMember] public bool kill_me { get; set; }
        [DataMember] public string pod_name { get; set; }
        [DataMember] public string session_id { get; set; }
    }

    [DataContract]
    public class RegisterLocalServerRequest
    {
        [DataMember] public string ip { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public uint port { get; set; }
    }

    [DataContract]
    public class PartyMember
    {
        [DataMember] public string user_id { get; set; }
    }

    [DataContract]
    public class MatchParty
    {
        [DataMember] public string party_id { get; set; }
        [DataMember] public PartyMember[] party_members { get; set; }
        [DataMember] public Dictionary<string, object> party_attributes { get; set; }
    }

    [DataContract]
    public class MatchingAlly
    {
        [DataMember] public MatchParty[] matching_parties { get; set; }
    }

    [DataContract]
    public class MatchRequest
    {
        [DataMember] public string game_mode { get; set; }
        [DataMember] public MatchingAlly[] matching_allies { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string session_id { get; set; }
    }

    [DataContract]
    public class DSMClient
    {
        [DataMember] public string host_address { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string provider { get; set; }
    }

    [DataContract]
    public class PubIp
    {
        [DataMember] public string ip { get; set; }
    }

    [DataContract]
    public class ServerInfo
    {
        [DataMember] public string pod_name { get; set; }
        [DataMember] public string image_version { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string ip { get; set; }
        [DataMember] public string[] alternate_ips { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember] public string provider { get; set; }
        [DataMember] public string game_version { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string last_update { get; set; }
    }

    [DataContract]
    public class MatchmakingResult
    {
        [DataMember] public string channel { get; set; }
        [DataMember] public string client_version { get; set; }
        [DataMember] public string game_mode { get; set; }
        [DataMember] public bool joinable { get; set; }
        [DataMember] public string match_id { get; set; }
        [DataMember] public MatchingAlly[] matching_allies { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public Dictionary<string, object> party_attributes { get; set; }
        [DataMember] public string party_id { get; set; }
        [DataMember] public int queued_at { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string server_name { get; set; }
        [DataMember] public MatchmakingStatus status { get; set; }
    }

    [DataContract]
    public class AddUserIntoSessionRequest
    {
        [DataMember] public string user_id { get; set; }
        [DataMember] public string party_id { get; set; }
    }

    [DataContract]
    public class DequeueRequest
    {
        [DataMember] public string match_id { get; set; }
    }

    [DataContract]
    public class ServerSessionResponse
    {
        [DataMember] public string session_id { get; set; }
    }
}