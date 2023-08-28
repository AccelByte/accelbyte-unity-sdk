// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;
using AccelByte.Core;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MatchmakingV2NotifType
    {
        OnMatchFound,
        OnMatchmakingStarted,
        OnTicketExpired,
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketRequest
    {
        [DataMember] public string matchPool;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public string sessionId;
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketRequestOptionalParams
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public string sessionId;
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketResponse
    {
        [DataMember] public string matchTicketId;
        [DataMember] public int queueTime;
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketError
    {
        [DataMember(Name = "ticketID")] public string TicketId;

        public static MatchmakingV2CreateTicketError GetFromError(Error error)
        {
            string jsonString = error.messageVariables.ToJsonString();

            return JsonConvert.DeserializeObject<MatchmakingV2CreateTicketError>(jsonString);
        }
    }

    [DataContract, Preserve]
    public class MatchmakingV2MatchTicketStatus
    {
        [DataMember] public string sessionId;
        [DataMember] public bool matchFound;
    }

    [DataContract, Preserve]
    public class MatchmakingV2Ticket
    {
        [DataMember] public string ticketId;
    }

    [DataContract, Preserve]
    public class MatchmakingV2Metrics
    {
        [DataMember(Name = "queueTime")] public int QueueTime;
    }

    [DataContract, Preserve]
    public class MatchmakingV2ProposedProposal
    {
        [DataMember(Name = "backfillID")] public string BackfillID;
        [DataMember(Name = "proposalID")] public string ProposalID;
        [DataMember(Name = "status")] public string Status;
    }

    [DataContract, Preserve]
    public class MatchmakingV2ActiveTicket
    {
        [DataMember(Name = "matchFound")] public bool MatchFound;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "matchTicketID")] public string MatchTicketID;
        [DataMember(Name = "proposedProposal")]
        public MatchmakingV2ProposedProposal ProposedProposal;
        [DataMember(Name = "sessionID")] public string SessionID;
    }

    [DataContract, Preserve]
    public class MatchmakingV2ActiveTickets
    {
        [DataMember(Name = "data")] public MatchmakingV2ActiveTicket[] Data;
        [DataMember(Name = "pagination")] public Paging Paging;
    }

    #region Backfill

    [DataContract, Preserve]
    public class ServerMatchmakingV2BackfillRequest
    {
        [DataMember] public string proposalId;
        [DataMember(Name = "stop")] public bool isStoppingBackfill;
    }

    [DataContract, Preserve]
    public class MatchmakingV2BackfillProposalNotification
    {
        [DataMember] public string backfillTicketId;
        [DataMember] public string proposalId;
        [DataMember] public string matchPool;
        [DataMember] public string matchSessionId;
        [DataMember] public SessionV2TeamData[] proposedTeams;
        [DataMember] public MatchmakingV2Ticket[] addedTickets;
    }
    
    [DataContract, Preserve]
    public class ServerMatchmakingV2Ticket
    {
        [DataMember] public string ticketId;
        [DataMember] public string matchPool;
        [DataMember] public DateTime createdAt;
        [DataMember] public ServerMatchmakingV2Players[] players;
        [DataMember] public ServerMatchmakingV2TicketAttributes ticketAttributes;
        [DataMember] public Dictionary<string, int> latencies;
    }
    
    [DataContract, Preserve]
    public class ServerMatchmakingV2TicketAttributes
    {
        [DataMember(Name = "server_name")] public string serverName;
        [DataMember(Name = "member_attributes")] public Dictionary<string, object> ticketAttributes;
    }
    
    [DataContract, Preserve]
    public class ServerMatchmakingV2Players
    {
        [DataMember] public string playerId;
        [DataMember] public Dictionary<string, object> attributes;
    }

    #endregion

    #region Notifications

    [DataContract, Preserve]
    public class MatchmakingV2MatchFoundNotification
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
        [DataMember] public SessionV2TeamData[] teams;
        [DataMember] public MatchmakingV2Ticket[] tickets;
    }

    [DataContract, Preserve]
    public class MatchmakingV2MatchmakingStartedNotification
    {
        [DataMember] public string ticketId;
        [DataMember] public string partyId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
    }

    [DataContract, Preserve]
    public class MatchmakingV2TicketExpiredNotification
    {
        [DataMember] public string ticketId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime createdAt;
        [DataMember] public string matchPool;
    }

    #endregion
}