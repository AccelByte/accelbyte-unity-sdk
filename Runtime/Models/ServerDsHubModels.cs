// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DsHubNotificationTopic
    {
        [EnumMember(Value = "")]EMPTY,
        serverClaimed,
        BACKFILL_PROPOSAL,
        [EnumMember(Value = "SESSION_ENDED_NOTIF")] SessionEnded,
        [EnumMember(Value = "SESSION_MEMBER_CHANGED")] SessionMemberChanged,
        [EnumMember(Value = "DSHUB_CONNECTED")] DsHubConnected,
        [EnumMember(Value = "BACKFILL_TICKET_EXPIRE")] BackfillTicketExpire
    }
    
    [DataContract, Preserve]
    public class ServerDSHubWebsocketNotificationTopic
    {
        [DataMember] public DsHubNotificationTopic topic;
    }
    
    [DataContract, Preserve]
    public class ServerDSHubWebsocketNotification<T> where T : class, new() 
    {
        [DataMember] public DsHubNotificationTopic topic;
        [DataMember] public T payload;
    }

    [DataContract, Preserve]
    public class ServerClaimedNotification
    {
        [DataMember(Name = "game_mode")] public string gameMode;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember(Name = "matching_allies")] public MatchingAlly[] matchingAllies;
        [DataMember(Name = "session_id")] public string sessionId;
    }

    [DataContract, Preserve]
    public class SessionEndedNotification
    {
        [DataMember(Name = "is_active")] public bool IsActive;
        [DataMember(Name = "session_id")] public string SessionId;
    }
}