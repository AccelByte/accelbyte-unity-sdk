// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DsHubNotificationTopic
    {
        serverClaimed,
        BACKFILL_PROPOSAL,
    }
    
    [DataContract]
    public class ServerDSHubWebsocketNotificationTopic
    {
        [DataMember] public DsHubNotificationTopic topic { get; set; }
    }
    
    [DataContract]
    public class ServerDSHubWebsocketNotification<T> where T : class, new() 
    {
        [DataMember] public DsHubNotificationTopic topic { get; set; }
        [DataMember] public T payload { get; set; }
    }

    [DataContract]
    public class ServerClaimedNotification
    {
        [DataMember(Name = "game_mode")] public string gameMode { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember(Name = "matching_allies")] public MatchingAlly[] matchingAllies { get; set; }
        [DataMember(Name = "session_id")] public string sessionId { get; set; }
    }
}