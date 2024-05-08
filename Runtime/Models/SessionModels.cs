// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionConfigurationTemplateType
    {
        NONE,
        P2P,
        DS
    };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2Joinability
    {
        None,
        EMPTY,
        CLOSED,
        INVITE_ONLY,
        OPEN
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2MemberStatus
    {
        EMPTY,
        INVITED,
        JOINED,
        CONNECTED,
        LEFT,
        DROPPED,
        REJECTED,
        KICKED,
        TIMEOUT,
        DISCONNECTED,
        TERMINATED
    };
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2NotifType
    {
        // Party Session
        OnPartyInvited,
        OnPartyJoined,
        OnPartyMembersChanged,
        OnPartyRejected,
        OnPartyKicked,
        OnPartyUpdated,

        // Game Session
        OnSessionInvited,
        OnSessionJoined,
        OnSessionKicked,
        OnSessionRejected,
        OnSessionMembersChanged,
        OnGameSessionUpdated,

        // DS
        OnDSStatusChanged,
    }

    [DataContract, Preserve]
    public class SessionV2PublicConfiguration
    {
        [DataMember] public string name;
        [DataMember] public SessionConfigurationTemplateType type;
        [DataMember] public SessionV2Joinability joinability;
        [DataMember] public int minPlayers;
        [DataMember] public int maxPlayers;
        [DataMember] public int inviteTimeout;
        [DataMember] public int inactiveTimeout;
        [DataMember] public string deployment;
        [DataMember] public string[] requestedRegions;
        [DataMember] public string clientVersion;
        [DataMember(Name = "persistent")] public bool Persistent;
    }

    [DataContract, Preserve]
    public class SessionV2SessionInviteRequest
    {
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class SessionV2MemberData
    {
        [DataMember] public string id;
        [DataMember] public string platformId;
        [DataMember] public string platformUserId;
        [DataMember] public SessionV2MemberStatus status;
        [DataMember(Name = "statusV2")] public SessionV2MemberStatus StatusV2;
        [DataMember] public DateTime updatedAt;
    }
    
    [DataContract, Preserve]
    public class SessionV2Notification
    {
        [DataMember] public string type;
        [DataMember] public SessionV2NotifType topic;
        [DataMember] public string payload;
        [DataMember] public string sentAt;
    }

    #region SessionStorage

    [DataContract, Preserve]
    public class SessionV2Storage
    {
        [DataMember(Name = "leader")] public JObject Leader;
        [DataMember(Name = "member")] public Dictionary<string, JObject> Member;
    }

    #endregion

    #region SessionV2Base

    [DataContract, Preserve]
    public class SessionV2Base
    {
        [DataMember(Name = "id")] public string ID;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "isActive")] public bool IsActive;
        [DataMember(Name = "attributes")] public JObject Attributes;
        [DataMember(Name = "members")] public SessionV2MemberData[] Members;
        [DataMember(Name = "createdBy")] public string CreatedBy;
        [DataMember(Name = "leaderId")] public string LeaderId;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "configuration")] public SessionV2PublicConfiguration Configuration;
        [DataMember(Name = "version")] public int Version;
        [DataMember(Name = "sessionType")] public SessionV2Joinability SessionType;
        [DataMember(Name = "storage")] public SessionV2Storage Storage;
    }

    #endregion

    #region PartySession

    [DataContract, Preserve]
    public class SessionV2PartySessionCreateRequest
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public string configurationName;
        [DataMember(Name = "joinType")] public SessionV2Joinability joinability;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public bool textChat;
    }

    [DataContract, Preserve]
    public class SessionV2PartySession
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public SessionV2PublicConfiguration configuration;
        [DataMember(Name = "code")] public string Code;
        [DataMember] public DateTime createdAt;
        [DataMember] public string createdBy;
        [DataMember] public string id;
        [DataMember] public bool isActive;
        [DataMember] public string leaderId;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int version;
        [DataMember(Name = "storage")] public SessionV2Storage Storage;
    }

    [DataContract, Preserve]
    public class SessionV2PartySessionKickResponse
    {
        [DataMember] public string leaderId;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public string partyId;
    }

    [DataContract, Preserve]
    public class SessionV2PartySessionUpdateRequest
    {
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public int inactiveTimeout;
        [DataMember] public int inviteTimeout;
        [DataMember] public SessionV2Joinability joinability;
        [DataMember] public int maxPlayers;
        [DataMember] public int minPlayers;
        [DataMember] public SessionConfigurationTemplateType type;
        [DataMember] public int version;
    }
    
    [DataContract, Preserve]
    public class SessionV2PartySessionPromoteLeaderRequest
    {
        [DataMember] public string leaderId;
    }
    
    #region Notification
    
    [DataContract, Preserve]
    public class SessionV2PartyInvitationNotification
    {
        [DataMember] public string partyId;
        [DataMember] public string senderId;
    }
    
    [DataContract, Preserve]
    public class SessionV2PartyJoinedNotification
    {
        [DataMember] public string partyId;
        [DataMember] public SessionV2MemberData[] members;
    }
    
    [DataContract, Preserve]
    public class SessionV2PartyMembersChangedNotification
    {
        [DataMember] public SessionV2PartySession session;
        [DataMember] public string joinerId;
        [DataMember] public string leaderId;
    }    
    
    [DataContract, Preserve]
    public class SessionV2PartyInvitationRejectedNotification
    {
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public string partyId;
        [DataMember] public string rejectedId;
    }    
    
    [DataContract, Preserve]
    public class SessionV2PartyUserKickedNotification
    {
        [DataMember] public string partyId;
    }
    
    [DataContract, Preserve]
    public class SessionV2PartySessionUpdatedNotification
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public SessionV2PublicConfiguration configuration;
        [DataMember] public int version;
        [DataMember] public string leaderId;
        [DataMember] public string configurationName;
        [DataMember] public string createdBy;
    }

    #endregion

    #endregion

    #region GameSession

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2StatusFilter
    {
        EMPTY,
        INVITED,
        JOINED,
        CONNECTED,
    };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2DsStatus
    {
        None,
        EMPTY,
        NEED_TO_REQUEST,
        REQUESTED,
        PREPARING,
        AVAILABLE,
        FAILED_TO_REQUEST,
        ENDED,
        UNKNOWN
    };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2AttributeOrderBy
    {
        createdAt,
        updatedAt
    };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionV2AttributeOrder
    {
        Asc,
        Desc
    };

    [DataContract, Preserve]
    public class SessionV2GameSessionCreateRequest
    {
        [DataMember(EmitDefaultValue = false)] public Dictionary<string, object> attributes;
        [DataMember(EmitDefaultValue = false)] public string backfillTicketId;
        [DataMember(EmitDefaultValue = false)] public string clientVersion;
        [DataMember(EmitDefaultValue = false)] public string configurationName;
        [DataMember(EmitDefaultValue = false)] public string deployment;
        [DataMember(EmitDefaultValue = false)] public int inactiveTimeout;
        [DataMember(EmitDefaultValue = false)] public int inviteTimeout;
        [DataMember(EmitDefaultValue = false)] public SessionV2Joinability joinability;
        [DataMember(EmitDefaultValue = false)] public string matchPool;
        [DataMember(EmitDefaultValue = false)] public int maxPlayers;
        [DataMember(EmitDefaultValue = false)] public int minPlayers;
        [DataMember(EmitDefaultValue = false)] public string[] requestedRegions;
        [DataMember(EmitDefaultValue = false)] public string serverName;
        [DataMember(EmitDefaultValue = false)] public SessionV2TeamData[] teams;
        [DataMember(EmitDefaultValue = false)] public string[] ticketIds;
        [DataMember(EmitDefaultValue = false)] public SessionConfigurationTemplateType type;
        [DataMember(EmitDefaultValue = false)] public SessionV2MemberData[] members;
        [DataMember] public bool textChat;
    }

    [DataContract, Preserve]
    public class SessionV2GameSession
    {
        [DataMember] public SessionV2DsInformation dsInformation;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public string backfillTicketId;
        [DataMember(Name = "code")] public string Code;
        [DataMember] public SessionV2PublicConfiguration configuration;
        [DataMember] public DateTime createdAt;
        [DataMember] public string createdBy;
        [DataMember] public string id;
        [DataMember] public bool isActive;
        [DataMember] public string leaderId;
        [DataMember] public string matchPool;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public SessionV2TeamData[] teams;
        [DataMember] public string[] ticketIds;
        [DataMember] public DateTime updatedAt;
        [DataMember] public int version;
        [DataMember(Name = "storage")] public SessionV2Storage Storage;
    }

    [DataContract, Preserve]
    public class SessionV2GameSessionPagingResponse
    {
        [DataMember(Name = "data")] public SessionV2GameSession[] Data;
        [DataMember(Name = "paging")] public Paging Paging;
    }

    [DataContract, Preserve]
    public class SessionV2GameSessionUpdateRequest
    {
        [DataMember(EmitDefaultValue = false)] public Dictionary<string, object> attributes;
        [DataMember(EmitDefaultValue = false)] public string backfillTicketId;
        [DataMember(EmitDefaultValue = false)] public string clientVersion;
        [DataMember(EmitDefaultValue = false)] public string deployment;
        [DataMember(EmitDefaultValue = false)] public int inactiveTimeout;
        [DataMember(EmitDefaultValue = false)] public int inviteTimeout;
        [DataMember(EmitDefaultValue = false)] public SessionV2Joinability joinability;
        [DataMember(EmitDefaultValue = false)] public string matchPool;
        [DataMember(EmitDefaultValue = false)] public int maxPlayers;
        [DataMember(EmitDefaultValue = false)] public int minPlayers;
        [DataMember(EmitDefaultValue = false)] public string[] requestedRegions;
        [DataMember(EmitDefaultValue = false)] public SessionV2TeamData[] teams;
        [DataMember(EmitDefaultValue = false)] public string[] ticketIds;
        [DataMember(EmitDefaultValue = false)] public SessionConfigurationTemplateType type;
        [DataMember(EmitDefaultValue = false)] public int version;
    }

    [DataContract, Preserve]
    public class SessionV2DsSessionReadyRequest
    {
        [DataMember] public bool Ready = false;
    }

    [DataContract, Preserve]
    public class SessionV2DsInformation
    {
        [DataMember] public SessionV2GameServer server;
        [DataMember] public SessionV2DsStatus status;
        [DataMember(Name = "statusV2")] public SessionV2DsStatus StatusV2;
        
        [DataMember( EmitDefaultValue = false, Name = "requestedAt" )]
        [CanBeNull]
        private string RequestedAt
        {
            get => requestedAt?.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
            set
            {
                if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo,
                        DateTimeStyles.None,
                        out var requestedAtResult))
                {
                    requestedAt = requestedAtResult;
                }
                else
                {
                    requestedAt = null;
                }
            }
        }

        public DateTime? requestedAt;
    }

    [DataContract, Preserve]
    public class SessionV2GameServer
    {
        [DataMember(Name = "alternate_ips")] public string[] alternateIps;

        [DataMember(Name = "custom_attribute")]
        public string customAttribute;

        [DataMember] public string deployment;
        [DataMember(Name = "game_version")] public string gameVersion;
        [DataMember(Name = "image_version")] public string imageVersion;
        [DataMember] public string ip;

        [DataMember(Name = "is_override_game_version")]
        public bool isOverrideGameVersion;

        [DataMember(Name = "last_update")] public DateTime lastUpdate;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember(Name = "pod_name")] public string podName;
        [DataMember] public int port;
        [DataMember] public Dictionary<string, int> ports;
        [DataMember] public string protocol;
        [DataMember] public string provider;
        [DataMember] public string region;
        [DataMember(Name = "session_id")] public string sessionId;
        [DataMember] public string status;
    }

    [DataContract, Preserve]
    public class SessionV2TeamData
    {
        [DataMember] public string[] userIds;
    }

    [DataContract, Preserve]
    public class SessionV2JoinByCodeRequest
    {
        [DataMember(Name = "code")] public string Code;
    }

    [DataContract, Preserve]
    public class SessionV2GameSessionPromoteLeaderRequest
    {
        [DataMember(Name = "leaderID")] public string LeaderId;
    }

    [DataContract, Preserve]
    public class SessionV2MemberActiveSession
    {
        [DataMember(Name = "CreatedAt")] public string CreatedAt;
        [DataMember(Name = "ID")] public string Id;
        [DataMember(Name = "MemberID")] public string MemberId;
        [DataMember(Name = "Namespace")] public string Namespace;
        [DataMember(Name = "SessionIDs")] public string[] SessionIds;
        [DataMember(Name = "SessionTemplate")] public string SessionTemplate;
        [DataMember(Name = "Total")] public int Total;
        [DataMember(Name = "UpdatedAt")] public DateTime UpdatedAt;
    }

    #region Notification

    [DataContract, Preserve]
    public class SessionV2GameInvitationNotification
    {
        [DataMember] public string sessionId;
    }    
    
    [DataContract, Preserve]
    public class SessionV2GameJoinedNotification
    {
        [DataMember] public string sessionId;
        [DataMember] public SessionV2MemberData[] members;
    }
    
    [DataContract, Preserve]
    public class SessionV2GameUserKickedNotification
    {
        [DataMember] public string partyId;
    }
    
    [DataContract, Preserve]
    public class SessionV2GameInvitationRejectedNotification
    {
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public string sessionId;
        [DataMember] public string rejectedId;
    }
    
    [DataContract, Preserve]
    public class SessionV2GameMembersChangedNotification
    {
        [DataMember] public SessionV2GameSession session;
        [DataMember] public string joinerId;
        [DataMember] public string leaderId;
    }

    [DataContract, Preserve]
    public class SessionV2GameSessionUpdatedNotification
    {
        [DataMember] public string id;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public SessionV2MemberData[] members;
        [DataMember] public Dictionary<string, object> attributes;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime updatedAt;
        [DataMember] public SessionV2PublicConfiguration configuration;
        [DataMember] public int version;
        [DataMember(Name = "match_pool")] public string matchPool;
        [DataMember(Name = "game_mode")] public string gameMode;
        [DataMember(Name = "backfill_ticket_id")] public string backfillTicketId;
        [DataMember] public SessionV2TeamData[] teams;
        [DataMember(Name = "ds_information")] public SessionV2DsInformation dsInformation;
        [DataMember(Name = "ticket_ids")] public string[] ticketIds;
        [DataMember(Name = "LeaderID")] public string LeaderId;
    }
    
    [DataContract, Preserve]
    public class SessionV2DsStatusUpdatedNotification
    {
        [DataMember] public SessionV2GameSession session;
        [DataMember] public string sessionId;
        [DataMember] public string error;
    }

    [DataContract, Preserve]
    public class SessionV2GameSessionEndedNotification
    {
        [DataMember] public string SessionID;
        [DataMember] public bool TextChat;
    }
    
    #endregion

    #endregion
}