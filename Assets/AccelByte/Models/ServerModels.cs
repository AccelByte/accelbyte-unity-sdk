// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class RegisterServerRequest
    {
        [DataMember] public string pod_name { get; set; }
        [DataMember] public int port { get; set; }
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
    }

    [DataContract]
    public class MatchingAlly
    {
        [DataMember] public string partyId { get; set; }
        [DataMember] public string[] partyMember { get; set; }
    }

    [DataContract]
    public class MatchRequest
    {
        [DataMember] public string game_mode { get; set; }
        [DataMember] public MatchingAlly[] matching_allies { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string session_id { get; set; }
    }
}