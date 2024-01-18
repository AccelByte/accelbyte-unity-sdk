// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
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

    [JsonConverter(typeof(StringEnumConverter))]
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MultiplayerV2NotifType
    {
        // SessionV2 - Party
        OnPartyInvited,
        OnPartyJoined,
        OnPartyMembersChanged,
        OnPartyRejected,
        OnPartyKicked,
        OnPartyUpdated,

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
        
        // MatchmakingV2
        OnMatchFound,
        OnMatchmakingStarted,
        OnMatchmakingTicketExpired,

        // SessionV2Storage
        OnSessionStorageChanged,
    }
    
    [DataContract, Preserve]
    public class DisconnectNotif
    {
        [DataMember] public string message;
    }

    [DataContract, Preserve]
    public class LobbySessionId
    {
        [DataMember] public string lobbySessionID;
    }

    #endregion

    #region Notification

    [DataContract, Preserve]
    public class Notification
    {
        [DataMember] public string id;
        [DataMember] public string from;
        [DataMember] public string to;
        [DataMember] public string topic;
        [DataMember] public string payload;
        [DataMember] public string sentAt;
    }

    [DataContract, Preserve]
    public class ErrorNotif
    {
        [DataMember] public string id;
        [DataMember] public string requestType;
        [DataMember] public string message;
        [DataMember] public int code;
    }

    #endregion

    #region Personal Chat

    [DataContract, Preserve]
    public class ChatMessage
    {
        [DataMember] public string id;
        [DataMember] public string from;
        [DataMember] public string to;
        [DataMember] public string payload;
        [DataMember] public string receivedAt;
    }

    [DataContract, Preserve]
    public class PersonalChatRequest
    {
        [DataMember] public string to;
        [DataMember] public string payload;
    }

    #endregion

    #region Global Chat

    [DataContract, Preserve]
    public class ChatChannelSlug
    {
        [DataMember] public string channelSlug;
    }

    [DataContract, Preserve]
    public class ChannelChatRequest
    {
        [DataMember] public string channelSlug;
        [DataMember] public string payload;
    }

    [DataContract, Preserve]
    public class ChannelChatMessage
    {
        [DataMember] public string from;
        [DataMember] public string channelSlug;
        [DataMember] public string payload;
        [DataMember] public DateTime sentAt;
    }

    #endregion

    #region Party

    [DataContract, Preserve]
    public class PartyCreateResponse : PartyInfo
    {
        [DataMember] public string partyCode; // Can be used by another player to join the recently created party
    }

    [DataContract, Preserve]
    public class PartyInfo
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string[] members;
        [DataMember] public string[] invitees;
        [DataMember] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyDataUpdateNotif
    {
        [DataMember] public long updatedAt;
        [DataMember] public string partyId;
        [DataMember] public string leader;
        [DataMember] public string[] members;
        [DataMember] public string[] invitees;
        [DataMember] public Dictionary<string, object> custom_attribute;
    }

    [DataContract, Preserve]
    public class PartyDataUpdateRequest
    {
        [DataMember] public long updatedAt;
        [DataMember] public Dictionary<string, object> custom_attribute;
    }
    
    [DataContract, Preserve]
    public class PartySendNotifRequest
    {
        [DataMember] public string topic;
        [DataMember] public string payload;
    }
    
    [DataContract, Preserve]
    public class PartySendNotifResponse 
    {
        [DataMember] public string code;
    }
    
    [DataContract, Preserve]
    public class PartyNotif
    {
        [DataMember] public string sender;
        [DataMember] public string topic;
        [DataMember] public string payload;
    }

    [DataContract, Preserve]
    public class SetPartySizeLimitRequest
    {
        [DataMember] public int limit;
    }

    [DataContract, Preserve]
    public class ActivePartiesData
    {
        [DataMember] public PartyDataUpdateNotif[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class PartyInviteRequest
    {
        [DataMember] public string friendID;
    }

    [DataContract, Preserve]
    public class PartyInviteResponse
    {
        [DataMember] public string inviteeID;
    }

    [DataContract, Preserve]
    public class PartyInvitation
    {
        [DataMember] public string from;
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyChatRequest
    {
        [DataMember] public string payload;
    }

    [DataContract, Preserve]
    public class PartyJoinRequest
    {
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyKickRequest
    {
        [DataMember] public string memberID;
    }

    [DataContract, Preserve]
    public class KickResponse
    {
        [DataMember] public string userID;
    }

    [DataContract, Preserve]
    public class JoinNotification
    {
        [DataMember] public string userID;
    }

    [DataContract, Preserve]
    public class KickNotification
    {
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
        [DataMember] public string partyID;
    }

    [DataContract, Preserve]
    public class LeaveNotification
    {
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
    }

    [DataContract, Preserve]
    public class PartyRejectRequest
    {
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyRejectResponse
    {
        [DataMember] public string partyID;
    }

    [DataContract, Preserve]
    public class PartyRejectNotif
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
    }

    [DataContract, Preserve]
    public class PartyPromoteLeaderRequest
    {
        [DataMember] public string newLeaderUserId;
    }

    [DataContract, Preserve]
    public class PartyPromoteLeaderResponse
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string members;
        [DataMember] public string invitees;
        [DataMember] public string invitationToken;
    }

    [DataContract, Preserve]
    public class PartyGenerateCodeResponse
    {
        [DataMember] public string partyCode;
    }

    [DataContract, Preserve]
    public class PartyGetCodeResponse
    {
        [DataMember] public string partyCode;
    }

    [DataContract, Preserve]
    public class PartyJoinViaCodeRequest
    {
        [DataMember] public string partyCode;
    }

    #endregion

    #region Matchmaking

    [DataContract, Preserve]
    public class StartMatchmakingRequest
    {
        [DataMember] public string gameMode;
        [DataMember] public string serverName;
        [DataMember] public string clientVersion;
        [DataMember] public string latencies;
        [DataMember] public string partyAttributes;

        [DataMember]
        public string
            tempParty; // used to store userIds to form temp party with (please include the matchmaking starter's userIds)

        [DataMember] public bool isTempParty; // used for cancel matchmaking when using temp party
        [DataMember] public string extraAttributes;
    }

    [DataContract, Preserve]
    public class MatchmakingNotifAllies
    {
        [DataMember] public MatchingAlly[] data;
    }

    [DataContract, Preserve]
    public class MatchmakingNotif
    {
        [DataMember] public string status;
        [DataMember] public string matchId;
        [DataMember] public string gameMode;
        [DataMember] public string deployment;
        [DataMember] public string clientVersion;
        [DataMember] public bool joinable;
        [DataMember] public MatchmakingNotifAllies matchingAllies;
        // rejected status message
        [DataMember] public string message;
        // rejected status error code
        [DataMember] public int errorCode;
        [DataMember] public string region;
    }

    [DataContract, Preserve]
    public class DsNotif
    {
        [DataMember] public string status;
        [DataMember] public string matchId;
        [DataMember] public string podName;
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public Dictionary<string, int> ports;
        [DataMember] public string customAttribute;
        [DataMember] public string message;
        [DataMember] public string isOK;
    }

    [DataContract, Preserve]
    public class MatchmakingCode
    {
        [DataMember] public int code;
    }

    [DataContract, Preserve]
    public class ReadyConsentRequest
    {
        [DataMember] public string matchId;
    }

    [DataContract, Preserve]
    public class ReadyForMatchConfirmation
    {
        [DataMember] public string matchId;
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class RematchmakingNotification
    {
        [DataMember] public int banDuration;
    }

    [DataContract, Preserve]
    public class MatchmakingOptionalParam
    {
        [DataMember] public string serverName;
        [DataMember] public string clientVersion;
        [DataMember] public Dictionary<string, int> latencies;
        [DataMember] public Dictionary<string, object> partyAttributes;
        [DataMember] public string[] tempPartyUserIds;
        [DataMember] public string[] extraAttributes;
        [DataMember] public string[] subGameModes;
        [DataMember] public bool newSessionOnly;
    }

    [DataContract, Preserve]
    public class CustomDsCreateRequest
    {
        [DataMember] public string matchId;
        [DataMember] public string gameMode;
        [DataMember] public string servername; //The Local DS name, fill it blank if you don't use Local DS.
        [DataMember] public string clientVersion;
        [DataMember] public string region;
        [DataMember] public string deployment;
    }

    #endregion

    #region Friends

    [JsonConverter(typeof(StringEnumConverter))]
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
        [DataMember] public RelationshipStatusCode friendshipStatus;
    }

    [DataContract, Preserve]
    public class Friends
    {
        [DataMember] public string[] friendsId;
    }
    
    [DataContract, Preserve]
    public class FriendWithTimestampData
    {
        [DataMember] public string friendId;
        [DataMember] public DateTime requestedAt;
    }
    
    [DataContract, Preserve]
    public class FriendsWithTimestamp
    {
        [DataMember] public FriendWithTimestampData[] data;
    }

    [DataContract, Preserve]
    public class Friend
    {
        [DataMember] public string friendId;
    }

    [DataContract, Preserve]
    public class FriendByPublicId
    {
        [DataMember] public string friendPublicId;
    }

    [DataContract, Preserve]
    public class Acquaintance
    {
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class BulkFriendsRequest
    {
        [DataMember] public string[] friendIds;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendInfo
    {
        [DataMember(Name = "isLogin")] public bool IsLogin;
        [DataMember(Name = "platformId", EmitDefaultValue = false)] public string PlatformId;
        [DataMember(Name = "platformToken", EmitDefaultValue = false)] public string PlatformToken;
        [DataMember(Name = "psnEnv", EmitDefaultValue = false)] public string PsnEnv;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendsRequest
    {
        [DataMember(Name = "friendSyncDetails")] public SyncThirdPartyFriendInfo[] FriendSyncDetails;
    }

    [DataContract, Preserve]
    public class SyncThirdPartyFriendsResponse
    {
        [DataMember(Name = "detail")] public string Detail;
        [DataMember(Name = "platformId")] public string PlatformId;
        [DataMember(Name = "status")] public string Status;
    }

    #endregion

    #region Presence

    [JsonConverter(typeof(StringEnumConverter))]
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
        [DataMember] public string[] friendsId;
        [DataMember] public UserStatus[] availability;
        [DataMember] public string[] activity;
        [DataMember] public DateTime[] lastSeenAt;
    }

    [DataContract, Preserve]
    public class FriendsStatusNotif
    {
        [DataMember] public string userID;
        [DataMember] public UserStatus availability;
        [DataMember] public string activity;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract, Preserve]
    public class SetUserStatusRequest
    {
        [DataMember] public string availability;
        [DataMember] public string activity;
    }

    [DataContract, Preserve]
    public class OnlineFriends
    {
        [DataMember] public string[] onlineFriendsId;
    }

    [DataContract, Preserve]
    public class UserStatusNotif
    {
        [DataMember] public string userID;
        [DataMember] public UserStatus availability;
        [DataMember] public string activity;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract, Preserve]
    public class BulkUserStatusNotif
    {
        [DataMember] public UserStatusNotif[] data;
        [DataMember] public int online;
        [DataMember] public int busy;
        [DataMember] public int invisible;
        [DataMember] public int offline;
        [DataMember] public int away;
    }

    #endregion

    #region Block/Unblock

    [DataContract, Preserve]
    public class BlockPlayerRequest
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string blockedUserId;
    }

    [DataContract, Preserve]
    public class BlockPlayerResponse
    {
        [DataMember] public string blockedUserId;
    }

    [DataContract, Preserve]
    public class UnblockPlayerRequest
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string unblockedUserId;
    }

    [DataContract, Preserve]
    public class UnblockPlayerResponse
    {
        [DataMember] public string unblockedUserId;
    }

    [DataContract, Preserve]
    public class PlayerBlockedNotif
    {
        [DataMember] public string userId; //ID of the blocker
        [DataMember] public string blockedUserId; //ID of the blocked user
    }

    [DataContract, Preserve]
    public class PlayerUnblockedNotif
    {
        [DataMember] public string userId; //ID of user that lift the block
        [DataMember] public string unblockedUserId; // ID of the unblocked user
    }

    [DataContract, Preserve]
    public class BlockedData
    {
        [DataMember] public string blockedUserId;
        [DataMember] public DateTime blockedAt;
    }

    [DataContract, Preserve]
    public class BlockedList
    {
        [DataMember] public BlockedData[] data;
    }

    [DataContract, Preserve]
    public class BlockerData
    {
        [DataMember] public string userId;
        [DataMember] public DateTime blockedAt;
    }

    [DataContract, Preserve]
    public class BlockerList
    {
        [DataMember] public BlockerData[] data;
    }

    #endregion

    #region Session Attribute

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionAttributeName
    {
        profanity_filtering_level
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProfanityFilterLevel
    {
        all,
        mandatory,
        none
    }

    [DataContract, Preserve]
    public class SetSessionAttributeRequest
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string key;
        [DataMember] public string value;
    }

    public class GetSessionAttributeRequest
    {
        [DataMember] public string key;
    }

    public class GetSessionAttributeResponse
    {
        [DataMember] public string value;
    }

    public class ServerGetSessionAttributeResponse
    {
        [DataMember] public string key;
        [DataMember] public string value;
    }

    public class GetSessionAttributeAllResponse
    {
        [DataMember] public Dictionary<string, string> attributes;
    }

    public class ServerSetSessionAttributeRequest
    {
        [DataMember] public Dictionary<string, string> attributes;
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
        [DataMember] public string token;
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
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public BanType ban;
        [DataMember] public string endDate;
        [DataMember] public BanReason reason;
        [DataMember] public bool enable;
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
        [DataMember] public string destinationId;
        [DataMember] public string message;
    }

    #endregion

    #region SessionStorage

    [DataContract, Preserve]
    public class SessionStorageChangedNotification
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "sessionID")] public string SessionID;
        [DataMember(Name = "actorUserID")] public string ActorUserID;
        [DataMember(Name = "isLeader")] public bool IsLeader;
        [DataMember(Name = "storageChanges")] public JObject StorageChanges;
    }

    #endregion

    #region Region

    [DataContract, Preserve]
    public class ChangeRegionRequest
    {
        [DataMember(Name = "region")] public string Region;
    }
    #endregion
}