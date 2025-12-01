// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    #region General

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MessageType
    {
        unknown,
        connectNotif,
        disconnectNotif,
        partyInfoRequest,
        partyInfoResponse,
        partyCreateRequest,
        partyCreateResponse,
        partyLeaveRequest,
        partyLeaveResponse,
        partyLeaveNotif,
        partyInviteRequest,
        partyInviteResponse,
        partyInviteNotif,
        partyGetInvitedNotif,
        partyJoinRequest,
        partyJoinResponse,
        partyJoinNotif,
        partyKickRequest,
        partyKickResponse,
        partyKickNotif,
        partyRejectRequest,
        partyRejectResponse,
        partyRejectNotif,
        partyGenerateCodeRequest,
        partyGenerateCodeResponse,
        partyGetCodeRequest,
        partyGetCodeResponse,
        partyDeleteCodeRequest,
        partyDeleteCodeResponse,
        partyJoinViaCodeRequest,
        partyJoinViaCodeResponse,
        partyPromoteLeaderRequest,
        partyPromoteLeaderResponse,
        partySendNotifRequest,
        partySendNotifResponse,
        partyNotif,
        personalChatRequest,
        personalChatResponse,
        personalChatNotif,
        partyChatRequest,
        partyChatResponse,
        partyChatNotif,
        setUserStatusRequest,
        setUserStatusResponse,
        userStatusNotif,
        friendsStatusRequest,
        friendsStatusResponse,
        messageNotif,
        offlineNotificationRequest,
        offlineNotificationResponse,
        requestFriendsRequest,
        requestFriendsResponse,
        requestFriendsByPublicIDRequest,
        requestFriendsByPublicIDResponse,
        unfriendRequest,
        unfriendResponse,
        listOutgoingFriendsRequest,
        listOutgoingFriendsResponse,
        listOutgoingFriendsWithTimeRequest,
        listOutgoingFriendsWithTimeResponse,
        cancelFriendsRequest,
        cancelFriendsResponse,
        listIncomingFriendsRequest,
        listIncomingFriendsResponse,
        listIncomingFriendsWithTimeRequest,
        listIncomingFriendsWithTimeResponse,
        acceptFriendsRequest,
        acceptFriendsResponse,
        rejectFriendsRequest,
        rejectFriendsResponse,
        listOfFriendsRequest,
        listOfFriendsResponse,
        getFriendshipStatusRequest,
        getFriendshipStatusResponse,
        acceptFriendsNotif,
        requestFriendsNotif,
        unfriendNotif,
        cancelFriendsNotif,
        rejectFriendsNotif,
        startMatchmakingRequest,
        startMatchmakingResponse,
        createDSRequest,
        cancelMatchmakingRequest,
        cancelMatchmakingResponse,
        matchmakingNotif,
        dsNotif,
        setReadyConsentRequest,
        setReadyConsentResponse,
        setReadyConsentNotif,
        rematchmakingNotif,
        joinDefaultChannelRequest,
        joinDefaultChannelResponse,
        sendChannelChatRequest,
        sendChannelChatResponse,
        channelChatNotif,
        systemComponentsStatus,
        partyDataUpdateNotif,
        blockPlayerRequest,
        blockPlayerResponse,
        unblockPlayerRequest,
        unblockPlayerResponse,
        blockPlayerNotif,
        unblockPlayerNotif,
        setSessionAttributeRequest,
        setSessionAttributeResponse,
        getSessionAttributeRequest,
        getSessionAttributeResponse,
        getAllSessionAttributeRequest,
        getAllSessionAttributeResponse,
        userBannedNotification,
        userUnbannedNotification,
        refreshTokenRequest,
        refreshTokenResponse,
        signalingP2PNotif,
        messageSessionNotif,
        setRejectConsentRequest,
        setRejectConsentResponse,
        setRejectConsentNotif,
        changeRegionRequest,
        changeRegionResponse,
        errorNotif
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum MultiplayerV2NotifType
    {
        // SessionV2 - Party
        OnPartyCreated,
        OnPartyInvited,
        OnPartyJoined,
        OnPartyMembersChanged,
        OnPartyRejected,
        OnPartyKicked,
        OnPartyUpdated,
        OnPartyCancelled,

        // SessionV2 - Game
        OnSessionInvited,
        OnSessionJoined,
        OnSessionKicked,
        OnSessionRejected,
        OnSessionMembersChanged,
        OnGameSessionUpdated,
        OnSessionEnded,

        // SessionV2 - DS
        OnDSStatusChanged,
        OnSessionJoinedSecret,

        // MatchmakingV2
        OnMatchFound,
        OnMatchmakingStarted,
        OnMatchmakingTicketCanceled,
        OnMatchmakingTicketExpired,

        // SessionV2Storage
        OnSessionStorageChanged,
    }
    
    [DataContract, Preserve]
    public class DisconnectNotif
    {
        [DataMember, Preserve] public string message;
    }

    [DataContract, Preserve]
    public class LobbySessionId
    {
        [DataMember, Preserve] public string lobbySessionID;
        [DataMember(Name = "sequenceID"), Preserve] public string SequenceId;
        [DataMember(Name = "sequenceNumber"), Preserve] public int SequenceNumber;
    }

    #endregion

    #region Notification

    [DataContract, Preserve]
    public class Notification
    {
        [DataMember, Preserve] public string id;
        [DataMember, Preserve] public string from;
        [DataMember, Preserve] public string to;
        [DataMember, Preserve] public string topic;
        [DataMember, Preserve] public string payload;
        [DataMember, Preserve] public string sentAt;
        [DataMember, Preserve] public string sequenceID;
        [DataMember, Preserve] public int sequenceNumber;
        [DataMember, Preserve] public string type;

        internal int SequenceIdInt
        {
            get
            {
                int retVal = -1;

                if (int.TryParse(this.sequenceID, out retVal))
                {
                    return retVal;
                }

                return retVal;
            }
        }
    }

    [DataContract, Preserve]
    public class ErrorNotif
    {
        [DataMember, Preserve] public string id;
        [DataMember, Preserve] public string requestType;
        [DataMember, Preserve] public string message;
        [DataMember, Preserve] public int code;
    }

    [DataContract, Preserve]
    public class UserNotification
    {
        [DataMember(Name = "from"), Preserve] public string From;
        [DataMember(Name = "id"), Preserve] public string Id;
        [DataMember(Name = "lobbySessionId"), Preserve] public string LobbySessionId;
        [DataMember(Name = "loginType"), Preserve] public string LoginType;
        [DataMember(Name = "payload"), Preserve] public string Payload;
        [DataMember(Name = "reconnectFromCode"), Preserve] public int ReconnectFromCode;
        [DataMember(Name = "sentAt"), Preserve] public DateTime SentAt;
        [DataMember(Name = "sequenceID"), Preserve] public string SequenceId;
        [DataMember(Name = "sequenceNumber"), Preserve] public int SequenceNumber;
        [DataMember(Name = "to"), Preserve] public string To;
        [DataMember(Name = "topic"), Preserve] public string Topic;
        [DataMember(Name = "type"), Preserve] public string Type;

        internal int SequenceIdInt
        {
            get
            {
                int retVal = -1;

                if (int.TryParse(this.SequenceId, out retVal))
                {
                    return retVal;
                }

                return retVal;
            }
        }

        public override int GetHashCode()
        {
#if UNITY_2021_3_OR_NEWER
            return HashCode.Combine(SequenceId, SequenceNumber);
#else
            return (SequenceId, SequenceNumber).GetHashCode();
#endif
        }

        public override bool Equals(object obj)
        {
            UserNotification otherNotif = obj as UserNotification;

            bool retVal = 
                otherNotif != null
                && SequenceId == otherNotif.SequenceId
                && SequenceNumber == otherNotif.SequenceNumber;

            return retVal;
        }
    }

    [DataContract, Preserve]
    public class GetUserNotificationsResponse
    {
        [DataMember(Name = "notifications"), Preserve] public UserNotification[] Notifications;
    }

    #endregion

    #region Personal Chat

    [DataContract, Preserve]
    public class ChatMessage
    {
        [DataMember, Preserve] public string id;
        [DataMember, Preserve] public string from;
        [DataMember, Preserve] public string to;
        [DataMember, Preserve] public string payload;
        [DataMember, Preserve] public string receivedAt;
    }

    [DataContract, Preserve]
    public class PersonalChatRequest
    {
        [DataMember, Preserve] public string to;
        [DataMember, Preserve] public string payload;
    }

    #endregion

    #region Global Chat

    [DataContract, Preserve]
    public class ChatChannelSlug
    {
        [DataMember, Preserve] public string channelSlug;
    }

    [DataContract, Preserve]
    public class ChannelChatRequest
    {
        [DataMember, Preserve] public string channelSlug;
        [DataMember, Preserve] public string payload;
    }

    [DataContract, Preserve]
    public class ChannelChatMessage
    {
        [DataMember, Preserve] public string from;
        [DataMember, Preserve] public string channelSlug;
        [DataMember, Preserve] public string payload;
        [DataMember, Preserve] public DateTime sentAt;
    }

    #endregion

    #region Party

    [DataContract, Preserve]
    public class PartyCreateResponse : PartyInfo
    {
        [DataMember, Preserve] public string partyCode; // Can be used by another player to join the recently created party
    }

    [DataContract, Preserve]
    public class PartyInfo
    {
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string leaderID;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string[] members;
        [DataMember, Preserve] public string[] invitees;
        [DataMember, Preserve] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyDataUpdateNotif
    {
        [DataMember, Preserve] public long updatedAt;
        [DataMember, Preserve] public string partyId;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string[] members;
        [DataMember, Preserve] public string[] invitees;
        [DataMember, Preserve] public Dictionary<string, object> custom_attribute;
    }

    [DataContract, Preserve]
    public class PartyDataUpdateRequest
    {
        [DataMember, Preserve] public long updatedAt;
        [DataMember, Preserve] public Dictionary<string, object> custom_attribute;
    }
    
    [DataContract, Preserve]
    public class PartySendNotifRequest
    {
        [DataMember, Preserve] public string topic;
        [DataMember, Preserve] public string payload;
    }
    
    [DataContract, Preserve]
    public class PartySendNotifResponse 
    {
        [DataMember, Preserve] public string code;
    }
    
    [DataContract, Preserve]
    public class PartyNotif
    {
        [DataMember, Preserve] public string sender;
        [DataMember, Preserve] public string topic;
        [DataMember, Preserve] public string payload;
    }

    [DataContract, Preserve]
    public class SetPartySizeLimitRequest
    {
        [DataMember, Preserve] public int limit;
    }

    [DataContract, Preserve]
    public class ActivePartiesData
    {
        [DataMember, Preserve] public PartyDataUpdateNotif[] data;
        [DataMember, Preserve] public Paging paging;
    }

    [DataContract, Preserve]
    public class PartyInviteRequest
    {
        [DataMember, Preserve] public string friendID;
    }

    [DataContract, Preserve]
    public class PartyInviteResponse
    {
        [DataMember, Preserve] public string inviteeID;
    }

    [DataContract, Preserve]
    public class PartyInvitation
    {
        [DataMember, Preserve] public string from;
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyChatRequest
    {
        [DataMember, Preserve] public string payload;
    }

    [DataContract, Preserve]
    public class PartyJoinRequest
    {
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyKickRequest
    {
        [DataMember, Preserve] public string memberID;
    }

    [DataContract, Preserve]
    public class KickResponse
    {
        [DataMember, Preserve] public string userID;
    }

    [DataContract, Preserve]
    public class JoinNotification
    {
        [DataMember, Preserve] public string userID;
    }

    [DataContract, Preserve]
    public class KickNotification
    {
        [DataMember, Preserve] public string leaderID;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string userID;
        [DataMember, Preserve] public string partyID;
    }

    [DataContract, Preserve]
    public class LeaveNotification
    {
        [DataMember, Preserve] public string leaderID;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string userID;
    }

    [DataContract, Preserve]
    public class PartyRejectRequest
    {
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyRejectResponse
    {
        [DataMember, Preserve] public string partyID;
    }

    [DataContract, Preserve]
    public class PartyRejectNotif
    {
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string leaderID;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string userID;
    }

    [DataContract, Preserve]
    public class PartyPromoteLeaderRequest
    {
        [DataMember, Preserve] public string newLeaderUserId;
    }

    [DataContract, Preserve]
    public class PartyPromoteLeaderResponse
    {
        [DataMember, Preserve] public string partyID;
        [DataMember, Preserve] public string leaderID;
        [DataMember, Preserve] public string leader;
        [DataMember, Preserve] public string members;
        [DataMember, Preserve] public string invitees;
        [DataMember, Preserve] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyGenerateCodeResponse
    {
        [DataMember, Preserve] public string partyCode;
    }

    [DataContract, Preserve]
    public class PartyGetCodeResponse
    {
        [DataMember, Preserve] public string partyCode;
    }

    [DataContract, Preserve]
    public class PartyJoinViaCodeRequest
    {
        [DataMember, Preserve] public string partyCode;
    }

    #endregion

    #region Matchmaking

    [DataContract, Preserve]
    public class StartMatchmakingRequest
    {
        [DataMember, Preserve] public string gameMode;
        [DataMember, Preserve] public string serverName;
        [DataMember, Preserve] public string clientVersion;
        [DataMember, Preserve] public string latencies;
        [DataMember, Preserve] public string partyAttributes;

        [DataMember, Preserve]
        public string
            tempParty; // used to store userIds to form temp party with (please include the matchmaking starter's userIds)

        [DataMember, Preserve] public bool isTempParty; // used for cancel matchmaking when using temp party
        [DataMember, Preserve] public string extraAttributes;
    }

    [DataContract, Preserve]
    public class MatchmakingNotifAllies
    {
        [DataMember, Preserve] public MatchingAlly[] data;
    }

    [DataContract, Preserve]
    public class MatchmakingNotif
    {
        [DataMember, Preserve] public string status;
        [DataMember, Preserve] public string matchId;
        [DataMember, Preserve] public string gameMode;
        [DataMember, Preserve] public string deployment;
        [DataMember, Preserve] public string clientVersion;
        [DataMember, Preserve] public bool joinable;
        [DataMember, Preserve] public MatchmakingNotifAllies matchingAllies;
        // rejected status message
        [DataMember, Preserve] public string message;
        // rejected status error code
        [DataMember, Preserve] public int errorCode;
        [DataMember, Preserve] public string region;
    }

    [DataContract, Preserve]
    public class DsNotif
    {
        [DataMember, Preserve] public string status;
        [DataMember, Preserve] public string matchId;
        [DataMember, Preserve] public string podName;
        [DataMember, Preserve] public string ip;
        [DataMember, Preserve] public int port;
        [DataMember, Preserve] public Dictionary<string, int> ports;
        [DataMember, Preserve] public string customAttribute;
        [DataMember, Preserve] public string message;
        [DataMember, Preserve] public string isOK;
    }

    [DataContract, Preserve]
    public class MatchmakingCode
    {
        [DataMember, Preserve] public int code;
    }

    [DataContract, Preserve]
    public class ReadyConsentRequest
    {
        [DataMember, Preserve] public string matchId;
    }

    [DataContract, Preserve]
    public class ReadyForMatchConfirmation
    {
        [DataMember, Preserve] public string matchId;
        [DataMember, Preserve] public string userId;
    }

    [DataContract, Preserve]
    public class RematchmakingNotification
    {
        [DataMember, Preserve] public int banDuration;
    }

    [DataContract, Preserve]
    public class MatchmakingOptionalParam : OptionalParametersBase
    {
        [DataMember, Preserve] public string serverName;
        [DataMember, Preserve] public string clientVersion;
        [DataMember, Preserve] public Dictionary<string, int> latencies;
        [DataMember, Preserve] public Dictionary<string, object> partyAttributes;
        [DataMember, Preserve] public string[] tempPartyUserIds;
        [DataMember, Preserve] public string[] extraAttributes;
        [DataMember, Preserve] public string[] subGameModes;
        [DataMember, Preserve] public bool newSessionOnly;
    }

    [DataContract, Preserve]
    public class CustomDsCreateRequest
    {
        [DataMember, Preserve] public string matchId;
        [DataMember, Preserve] public string gameMode;
        [DataMember, Preserve] public string servername; //The Local DS name, fill it blank if you don't use Local DS.
        [DataMember, Preserve] public string clientVersion;
        [DataMember, Preserve] public string region;
        [DataMember, Preserve] public string deployment;
    }

    #endregion

    #region Friends

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    [DataContract, Preserve]
    public enum RelationshipStatusCode
    {
        [EnumMember] NotFriend = 0,
        [EnumMember] Outgoing = 1,
        [EnumMember] Incoming = 2,
        [EnumMember] Friend = 3
    }

    [DataContract, Preserve]
    public class FriendshipStatus
    {
        [DataMember, Preserve] public RelationshipStatusCode friendshipStatus;
    }

    [DataContract, Preserve]
    public class Friends
    {
        [DataMember, Preserve] public string[] friendsId;
    }
    
    [DataContract, Preserve]
    public class FriendWithTimestampData
    {
        [DataMember, Preserve] public string friendId;
        [DataMember, Preserve] public DateTime requestedAt;
    }
    
    [DataContract, Preserve]
    public class FriendsWithTimestamp
    {
        [DataMember, Preserve] public FriendWithTimestampData[] data;
    }

    [DataContract, Preserve]
    public class Friend
    {
        [DataMember, Preserve] public string friendId;
    }

    [DataContract, Preserve]
    public class RequestFriend : Friend
    {
        [DataMember, Preserve] public Dictionary<string, string> metadata;
    }

    [DataContract, Preserve]
    public class FriendByPublicId
    {
        [DataMember, Preserve] public string friendPublicId;
    }

    [DataContract, Preserve]
    public class Acquaintance
    {
        [DataMember, Preserve] public string userId;
    }

    [DataContract, Preserve]
    public class BulkFriendsRequest
    {
        [DataMember, Preserve] public string[] friendIds;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendInfo
    {
        [DataMember(Name = "isLogin"), Preserve] public bool IsLogin;
        [DataMember(Name = "platformId", EmitDefaultValue = false), Preserve] public string PlatformId;
        [DataMember(Name = "platformToken", EmitDefaultValue = false), Preserve] public string PlatformToken;
        [DataMember(Name = "psnEnv", EmitDefaultValue = false), Preserve] public string PsnEnv;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendsRequest
    {
        [DataMember(Name = "friendSyncDetails"), Preserve] public SyncThirdPartyFriendInfo[] FriendSyncDetails;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendsResponse
    {
        [DataMember(Name = "detail"), Preserve] public string Detail;
        [DataMember(Name = "platformId"), Preserve] public string PlatformId;
        [DataMember(Name = "status"), Preserve] public string Status;
    }

    #endregion

    #region Presence

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum UserStatus
    {
        Offline,
        Online,
        Busy,
        Invisible,
        Away
    }

    [DataContract, Preserve]
    public class FriendsStatus
    {
        [DataMember, Preserve] public string[] friendsId;
        [DataMember, Preserve] public UserStatus[] availability;
        [DataMember, Preserve] public string[] activity;
        [DataMember, Preserve] public DateTime[] lastSeenAt;
    }

    [DataContract, Preserve]
    public class FriendsStatusNotif
    {
        [DataMember, Preserve] public string userID;
        [DataMember, Preserve] public UserStatus availability;
        [DataMember, Preserve] public string activity;
        [DataMember, Preserve] public DateTime lastSeenAt;
    }

    [DataContract, Preserve]
    public class SetUserStatusRequest
    {
        [DataMember, Preserve] public string availability;
        [DataMember, Preserve] public string activity;
    }

    [DataContract, Preserve]
    public class OnlineFriends
    {
        [DataMember, Preserve] public string[] onlineFriendsId;
    }

    [DataContract, Preserve]
    public class UserStatusNotif
    {
        [DataMember, Preserve] public string userID;
        [DataMember, Preserve] public UserStatus availability;
        [DataMember, Preserve] public string activity;
        [DataMember(Name = "namespace"), Preserve] public string namespace_;
        [DataMember, Preserve] public DateTime lastSeenAt;
    }

    [DataContract, Preserve]
    public class BulkUserStatusNotif
    {
        [DataMember, Preserve] public UserStatusNotif[] data;
        [DataMember, Preserve] public int online;
        [DataMember, Preserve] public int busy;
        [DataMember, Preserve] public int invisible;
        [DataMember, Preserve] public int offline;
        [DataMember, Preserve] public int away;
    }

    #endregion

    #region Block/Unblock

    [DataContract, Preserve]
    public class BlockPlayerRequest
    {
        [DataMember, Preserve] public string userId;
        [DataMember(Name = "namespace"), Preserve] public string Namespace;
        [DataMember, Preserve] public string blockedUserId;
    }

    [DataContract, Preserve]
    public class BlockPlayerResponse
    {
        [DataMember, Preserve] public string blockedUserId;
    }

    [DataContract, Preserve]
    public class UnblockPlayerRequest
    {
        [DataMember, Preserve] public string userId;
        [DataMember(Name = "namespace"), Preserve] public string Namespace;
        [DataMember, Preserve] public string unblockedUserId;
    }

    [DataContract, Preserve]
    public class UnblockPlayerResponse
    {
        [DataMember, Preserve] public string unblockedUserId;
    }

    [DataContract, Preserve]
    public class PlayerBlockedNotif
    {
        [DataMember, Preserve] public string userId; //ID of the blocker
        [DataMember, Preserve] public string blockedUserId; //ID of the blocked user
    }

    [DataContract, Preserve]
    public class PlayerUnblockedNotif
    {
        [DataMember, Preserve] public string userId; //ID of user that lift the block
        [DataMember, Preserve] public string unblockedUserId; // ID of the unblocked user
    }

    [DataContract, Preserve]
    public class BlockedData
    {
        [DataMember, Preserve] public string blockedUserId;
        [DataMember, Preserve] public DateTime blockedAt;
    }

    [DataContract, Preserve]
    public class BlockedList
    {
        [DataMember, Preserve] public BlockedData[] data;
    }

    [DataContract, Preserve]
    public class BlockerData
    {
        [DataMember, Preserve] public string userId;
        [DataMember, Preserve] public DateTime blockedAt;
    }

    [DataContract, Preserve]
    public class BlockerList
    {
        [DataMember, Preserve] public BlockerData[] data;
    }

    #endregion

    #region Session Attribute

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SessionAttributeName
    {
        profanity_filtering_level
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum ProfanityFilterLevel
    {
        all,
        mandatory,
        none
    }

    [DataContract, Preserve]
    public class SetSessionAttributeRequest
    {
        [DataMember(Name = "namespace"), Preserve] public string Namespace;
        [DataMember, Preserve] public string key;
        [DataMember, Preserve] public string value;
    }

    [DataContract, Preserve]
    public class GetSessionAttributeRequest
    {
        [DataMember, Preserve] public string key;
    }

    [DataContract, Preserve]
    public class GetSessionAttributeResponse
    {
        [DataMember, Preserve] public string value;
    }

    [DataContract, Preserve]
    public class ServerGetSessionAttributeResponse
    {
        [DataMember, Preserve] public string key;
        [DataMember, Preserve] public string value;
    }

    [DataContract, Preserve]
    public class GetSessionAttributeAllResponse
    {
        [DataMember, Preserve] public Dictionary<string, string> attributes;
    }

    [DataContract, Preserve]
    public class ServerSetSessionAttributeRequest
    {
        [DataMember, Preserve] public Dictionary<string, string> attributes;
    }
    
    public static class MatchExtraAttributes
    {
        /** Attributes to start role based matchmaking */
        public const string Role = "role";
    }
    
    public static class SessionAttributeKeys
    {
        /** Attributes for assigning role to user when doing role based matchmaking */
        public const string Role = "role";
    }

    #endregion

    #region Refresh Access Token

    [DataContract, Preserve]
    public class RefreshAccessTokenRequest
    {
        [DataMember, Preserve] public string token;
    }

    #endregion

    #region Ban

    /// <summary>
    ///  Information about user that got banned
    /// </summary>
    /// <param name="userId"> banned user ID</param>
    /// <param name="Namespace">Namespace that user got banned</param>
    /// <param name="banType">The type of Ban</param>
    /// <param name="endDate">The date when the ban is lifted with format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz"</param>
    /// <param name="reason">The reason of Banning</param>
    /// <param name="enable">is the ban still going for the user</param>
    [DataContract, Preserve]
    public class UserBannedNotification
    {
        [DataMember(Name = "id"), Preserve] public string Id;
        [DataMember(Name = "code"), Preserve] public string Code;
        [DataMember, Preserve] public string userId;
        [DataMember(Name = "namespace"), Preserve] public string Namespace;
        [DataMember, Preserve] public BanType ban;
        [DataMember, Preserve] public string endDate;
        [DataMember, Preserve] public BanReason reason;
        [DataMember, Preserve] public bool enable;
    }

    #endregion

    #region Signaling

    /// <summary>
    /// Struct to send signaling message and parsing incoming notification.
    /// As the sender: the destinationId is the targeted user ID.
    /// As the receiver (handle notification): the destinationId is the sender's user ID.
    /// </summary>
    /// <param name="destinationId"> The targeted user ID or the sender's UserID</param>
    /// <param name="message"> The content</param>
    [DataContract, Preserve]
    public class SignalingP2P
    {
        [DataMember, Preserve] public string destinationId;
        [DataMember, Preserve] public string message;
    }

    #endregion

    #region SessionStorage

    [DataContract, Preserve]
    public class SessionStorageChangedNotification
    {
        [DataMember(Name = "namespace"), Preserve] public string Namespace;
        [DataMember(Name = "sessionID"), Preserve] public string SessionID;
        [DataMember(Name = "actorUserID"), Preserve] public string ActorUserID;
        [DataMember(Name = "isLeader"), Preserve] public bool IsLeader;
        [DataMember(Name = "storageChanges"), Preserve] public JObject StorageChanges;
    }

    #endregion

    #region Region

    [DataContract, Preserve]
    public class ChangeRegionRequest
    {
        [DataMember(Name = "region"), Preserve] public string Region;
    }
    #endregion
}