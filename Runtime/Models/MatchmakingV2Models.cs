// Copyright (c) 2022 - 2025 AccelByte Inc. All Rights Reserved.
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
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MatchmakingV2NotifType
    {
        OnMatchFound,
        OnMatchmakingStarted,
        OnTicketExpired,
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MatchmakingV2ExclusionType
    {
        None,
        NPastSession,
        ExplicitList,
        AllMemberCachedSession,
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketRequest
    {
        [DataMember] public string matchPool;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public string sessionId;
        [DataMember(Name = "excludedSessions"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ExcludedSessions;
    }

    [DataContract, Preserve]
    public class MatchmakingV2CreateTicketRequestOptionalParams : OptionalParametersBase
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public Dictionary<string, int> latencies;
        // <summary>
        // If matchmaking is performed by the party leader, fill this with the party session ID.
        // It will make all users in the party to be included in matchmaking and later will be notified when the match is found.
        // </summary>
        [DataMember] public string sessionId;
        /// <summary>
        /// Indicates how to exclude past session id when creating a match ticket.
        /// </summary>
        [DataMember] public MatchmakingV2ExclusionType ExclusionType;
        /// <summary>
        /// Must be filled when ExclusionType is NPastSession.
        /// Indicating how many past session that will be excluded when creating a match ticket.
        /// </summary>
        [DataMember] public int ExcludedPastSessionCount;
        /// <summary>
        /// Must be filled when start matchmaking with ExclusionType is ExplicitList.
        /// Indicating past session id that will be excluded when creating a match ticket.
        /// </summary>
        [DataMember] public string[] ExcludedGameSessionIds;
    }
    
    [Preserve]
    public class DeleteMatchmakingV2OptionalParams : OptionalParametersBase
    {
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
        [DataMember(Name = "isActive")] public bool IsActive;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "proposedProposal")] public MatchmakingV2ProposedProposal ProposedProposal;
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

    [Preserve]
    public class GetMatchmakingTicketOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class GetMatchmakingMetricsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetUserMatchmakingTicketsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Filter returned results via their match pool.
        /// </summary>
        public string MatchPool = string.Empty;

        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    #region Backfill

    [DataContract, Preserve]
    public class ServerMatchmakingV2BackfillRequest
    {
        [DataMember(Name = "acceptedTicketIds"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] AcceptedTicketIds;
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
        [DataMember(Name = "addedTickets")] public ServerMatchmakingV2Ticket[] BackfillProposalTickets;
    }

    [DataContract, Preserve]
    public class SessionSecretUpdateNotification
    {
        [DataMember] public string secret;
    }

    [DataContract, Preserve]
    public class ServerMatchmakingV2Ticket : MatchmakingV2Ticket
    {
        [DataMember] public string matchPool;
        [DataMember] public DateTime createdAt;
        [DataMember] public ServerMatchmakingV2Players[] players;
        [DataMember] public ServerMatchmakingV2TicketAttributes ticketAttributes;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember(Name = "Namespace")] public string Namespace;
        [DataMember(Name = "PartySessionID")] public string PartySessionId;
        [DataMember(Name = "CreatedAt")] public DateTime CreatedAt;
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
        [DataMember(Name = "partyId")] public string PartyId;
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
    public class MatchmakingV2MatchmakingCanceledNotification
    {
        [DataMember(Name = "ticketId")] public string TicketId;
        [DataMember(Name = "reason")] public string Reason;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "partyId")] public string PartyId;
        [DataMember(Name = "userIds")] public string[] UserIds;
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