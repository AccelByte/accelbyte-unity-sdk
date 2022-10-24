// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MatchmakingV2NotifType
    {
        OnMatchFound,
        OnMatchmakingStarted,
        OnTicketExpired,
    }

    [DataContract]
    public class MatchmakingV2CreateTicketRequest
    {
        [DataMember] public string matchPool;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public string sessionId;
    }

    [DataContract]
    public class MatchmakingV2CreateTicketRequestOptionalParams
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public string sessionId;
    }

    [DataContract]
    public class MatchmakingV2CreateTicketResponse
    {
        [DataMember] public string matchTicketId;
    }

    [DataContract]
    public class MatchmakingV2MatchTicketStatus
    {
        [DataMember] public string sessionId;
        [DataMember] public bool matchFound;
    }

    [DataContract]
    public class MatchmakingV2Ticket
    {
        [DataMember] public string ticketId;
    }

    #region Backfill

    [DataContract]
    public class ServerMatchmakingV2BackfillRequest
    {
        [DataMember] public string proposalId;
        [DataMember(Name = "stop")] public bool isStoppingBackfill;
    }

    [DataContract]
    public class MatchmakingV2BackfillProposalNotification
    {
        [DataMember] public string backfillTicketId;
        [DataMember] public string proposalId;
        [DataMember] public string matchPool;
        [DataMember] public string matchSessionId;
        [DataMember] public SessionV2TeamData[] proposedTeams;
        [DataMember] public MatchmakingV2Ticket[] addedTickets;
    }
    
    [DataContract]
    public class ServerMatchmakingV2Ticket
    {
        [DataMember] public string ticketId;
        [DataMember] public string matchPool;
        [DataMember] public DateTime createdAt;
        [DataMember] public ServerMatchmakingV2Players[] players;
        [DataMember] public ServerMatchmakingV2TicketAttributes ticketAttributes;
        [DataMember] public Dictionary<string, int> latencies;
    }
    
    [DataContract]
    public class ServerMatchmakingV2TicketAttributes
    {
        [DataMember(Name = "server_name")] public string serverName;
        [DataMember(Name = "member_attributes")] public Dictionary<string, object> ticketAttributes;
    }
    
    [DataContract]
    public class ServerMatchmakingV2Players
    {
        [DataMember] public string playerId;
        [DataMember] public Dictionary<string, object> attributes;
    }

    #endregion

    #region Notifications

    [DataContract]
    public class MatchmakingV2MatchFoundNotification
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
        [DataMember] public SessionV2TeamData[] teams;
        [DataMember] public MatchmakingV2Ticket[] tickets;
    }

    [DataContract]
    public class MatchmakingV2MatchmakingStartedNotification
    {
        [DataMember] public string ticketId;
        [DataMember] public string partyId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
    }

    [DataContract]
    public class MatchmakingV2TicketExpiredNotification
    {
        [DataMember] public string ticketId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
    }

    #endregion
}