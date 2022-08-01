// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    #region General

    [JsonConverter( typeof( StringEnumConverter ) )]
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
        cancelFriendsRequest,
        cancelFriendsResponse,
        listIncomingFriendsRequest,
        listIncomingFriendsResponse,
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
        signalingP2PNotif
    }

    [DataContract]
    public class DisconnectNotif
    {
        [DataMember] public string message;
    }

    [DataContract]
    public class LobbySessionId
    {
        [DataMember] public string lobbySessionID;
    }

    #endregion

    #region Notification

    [DataContract]
    public class Notification
    {
        [DataMember] public string id;
        [DataMember] public string from;
        [DataMember] public string to;
        [DataMember] public string topic;
        [DataMember] public string payload;
        [DataMember] public string sentAt;
    }
    
    #endregion

    #region Personal Chat

    [DataContract]
    public class ChatMessage
    {
        [DataMember] public string id;
        [DataMember] public string from;
        [DataMember] public string to;
        [DataMember] public string payload;
        [DataMember] public string receivedAt;
    }

    [DataContract]
    public class PersonalChatRequest
    {
        [DataMember] public string to;
        [DataMember] public string payload;
    }

    #endregion

    #region Global Chat

    [DataContract]
    public class ChatChannelSlug
    {
        [DataMember] public string channelSlug;
    }

    [DataContract]
    public class ChannelChatRequest
    {
        [DataMember] public string channelSlug;
        [DataMember] public string payload;
    }

    [DataContract]
    public class ChannelChatMessage
    {
        [DataMember] public string from;
        [DataMember] public string channelSlug;
        [DataMember] public string payload;
        [DataMember] public DateTime sentAt;
    }

    #endregion

    #region Party

    [DataContract]
    public class PartyCreateResponse : PartyInfo
    {
        [DataMember] public string partyCode; // Can be used by another player to join the recently created party
    }

    [DataContract]
    public class PartyInfo
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string[] members;
        [DataMember] public string[] invitees;
        [DataMember] public string invitationToken;
    }

    [DataContract]
    public class PartyDataUpdateNotif
    {
        [DataMember] public long updatedAt;
        [DataMember] public string partyId;
        [DataMember] public string leader;
        [DataMember] public string[] members;
        [DataMember] public string[] invitees;
        [DataMember] public Dictionary<string, object> custom_attribute;
    }

    [DataContract]
    public class PartyDataUpdateRequest
    {
        [DataMember] public long updatedAt;
        [DataMember] public Dictionary<string, object> custom_attribute;
    }

    [DataContract]
    public class SetPartySizeLimitRequest
    {
        [DataMember] public int limit;
    }
    
    [DataContract]
    public class ActivePartiesData
    {
        [DataMember] public PartyDataUpdateNotif[] data;
        [DataMember] public Paging paging;
    }

    [DataContract]
    public class PartyInviteRequest
    {
        [DataMember] public string friendID;
    }

    [DataContract]
    public class PartyInviteResponse
    {
        [DataMember] public string inviteeID;
    }

    [DataContract]
    public class PartyInvitation
    {
        [DataMember] public string from;
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract]
    public class PartyChatRequest
    {
        [DataMember] public string payload;
    }

    [DataContract]
    public class PartyJoinRequest
    {
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract]
    public class PartyKickRequest
    {
        [DataMember] public string memberID;
    }

    [DataContract]
    public class KickResponse
    {
        [DataMember] public string userID;
    }

    [DataContract]
    public class JoinNotification
    {
        [DataMember] public string userID;
    }

    [DataContract]
    public class KickNotification
    {
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
        [DataMember] public string partyID;
    }

    [DataContract]
    public class LeaveNotification
    {
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
    }

    [DataContract]
    public class PartyRejectRequest
    {
        [DataMember] public string partyID;
        [DataMember] public string invitationToken;
    }

    [DataContract]
    public class PartyRejectResponse
    {
        [DataMember] public string partyID;
    }

    [DataContract]
    public class PartyRejectNotif
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string userID;
    }

    [DataContract]
    public class PartyPromoteLeaderRequest
    {
        [DataMember] public string newLeaderUserId;
    }

    [DataContract]
    public class PartyPromoteLeaderResponse
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
        [DataMember] public string leader;
        [DataMember] public string members;
        [DataMember] public string invitees;
        [DataMember] public string invitationToken;
    }

    [DataContract]
    public class PartyGenerateCodeResponse
    {
        [DataMember] public string partyCode;
    }

    [DataContract]
    public class PartyGetCodeResponse
    {
        [DataMember] public string partyCode;
    }

    [DataContract]
    public class PartyJoinViaCodeRequest
    {
        [DataMember] public string partyCode;
    }
    #endregion

    #region Matchmaking

    [DataContract]
    public class StartMatchmakingRequest
    {
        [DataMember] public string gameMode;
        [DataMember] public string serverName;
        [DataMember] public string clientVersion;
        [DataMember] public string latencies;
        [DataMember] public string partyAttributes;
        [DataMember] public string tempParty;           // used to store userIds to form temp party with (please include the matchmaking starter's userIds)
        [DataMember] public bool isTempParty;           // used for cancel matchmaking when using temp party
        [DataMember] public string extraAttributes;
    }

    [DataContract]
    public class MatchmakingNotifAllies
    {
        [DataMember] public MatchingAlly[] data;
    }

    [DataContract]
    public class MatchmakingNotif
    {
        [DataMember] public string status;
        [DataMember] public string matchId;
        [DataMember] public string gameMode;
        [DataMember] public string deployment;
        [DataMember] public string clientVersion;
        [DataMember] public bool joinable;
        [DataMember] public MatchmakingNotifAllies matchingAllies;

    }

    [DataContract]
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

    [DataContract]
    public class MatchmakingCode
    {
        [DataMember] public int code;
    }
    
    [DataContract]
    public class ReadyConsentRequest
    {
        [DataMember] public string matchId;
    }

    [DataContract]
    public class ReadyForMatchConfirmation
    {
        [DataMember] public string matchId;
        [DataMember] public string userId;
    }

    [DataContract]
    public class RematchmakingNotification
    {
        [DataMember] public int banDuration;
    }

    [DataContract]
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

    [DataContract]
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

    [JsonConverter( typeof( StringEnumConverter ) )]
    [DataContract]
    public enum RelationshipStatusCode
    {
        [EnumMember] NotFriend = 0,
        [EnumMember] Outgoing = 1,
        [EnumMember] Incoming = 2,
        [EnumMember] Friend = 3
    }

    [DataContract]
    public class FriendshipStatus
    {
        [DataMember] public RelationshipStatusCode friendshipStatus;
    }

    [DataContract]
    public class Friends
    {
        [DataMember] public string[] friendsId;
    }

    [DataContract]
    public class Friend
    {
        [DataMember] public string friendId;
    }

    [DataContract]
    public class FriendByPublicId
    {
        [DataMember] public string friendPublicId;
    }

    [DataContract]
    public class Acquaintance
    {
        [DataMember] public string userId;
    }

    [DataContract]
    public class BulkFriendsRequest
    {
        [DataMember] public string[] friendIds { get; set; }
    }

    #endregion

    #region Presence

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum UserStatus
    {
        Offline,
        Online,
        Busy,
        Invisible
    }

    [DataContract]
    public class FriendsStatus
    {
        [DataMember] public string[] friendsId;
        [DataMember] public UserStatus[] availability;
        [DataMember] public string[] activity;
        [DataMember] public DateTime[] lastSeenAt;
    }
    
    [DataContract]
    public class FriendsStatusNotif
    {
        [DataMember] public string userID;
        [DataMember] public UserStatus availability;
        [DataMember] public string activity;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract]
    public class SetUserStatusRequest
    {
        [DataMember] public string availability;
        [DataMember] public string activity;
    }

    [DataContract]
    public class OnlineFriends
    {
        [DataMember] public string[] onlineFriendsId;
    }
    
    [DataContract]
    public class UserStatusNotif
    {
        [DataMember] public string userID;
        [DataMember] public UserStatus availability;
        [DataMember] public string activity;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract]
    public class BulkUserStatusNotif
    {
        [DataMember] public UserStatusNotif[] data;
        [DataMember] public int online;
        [DataMember] public int busy;
        [DataMember] public int invisible;
        [DataMember] public int offline;
    }
    #endregion

    #region Block/Unblock

    [DataContract]
    public class BlockPlayerRequest
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string blockedUserId;
    }
    
    [DataContract]
    public class BlockPlayerResponse
    {
        [DataMember] public string blockedUserId;
    }
    
    [DataContract]
    public class UnblockPlayerRequest
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string unblockedUserId;
    }
    
    [DataContract]
    public class UnblockPlayerResponse
    {
        [DataMember] public string unblockedUserId;
    }
    
    [DataContract]
    public class PlayerBlockedNotif
    {
        [DataMember] public string userId; //ID of the blocker
        [DataMember] public string blockedUserId; //ID of the blocked user
    }
    
    [DataContract]
    public class PlayerUnblockedNotif
    {
        [DataMember] public string userId; //ID of user that lift the block
        [DataMember] public string unblockedUserId; // ID of the unblocked user
    }

    [DataContract]
    public class BlockedData
    {
        [DataMember] public string blockedUserId;
        [DataMember] public DateTime blockedAt;
    }

    [DataContract]
    public class BlockedList
    {
        [DataMember] public BlockedData[] data;
    }

    [DataContract]
    public class BlockerData
    {
        [DataMember] public string userId;
        [DataMember] public DateTime blockedAt;
    }

    [DataContract]
    public class BlockerList
    {
        [DataMember] public BlockerData[] data;
    }

    #endregion

    #region Session Attribute

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SessionAttributeName
    {
        profanity_filtering_level
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ProfanityFilterLevel
    {
        all,
        mandatory,
        none
    }

    [DataContract]
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

    #endregion

    #region Refresh Access Token

    [DataContract]
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
    [DataContract]
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
    [DataContract]
    public class SignalingP2P
    {
        [DataMember] public string destinationId;
        [DataMember] public string message;
    }

    #endregion
}