// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    #region General
    
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
        startMatchmakingRequest,
        startMatchmakingResponse,
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
    public class PartyInfo
    {
        [DataMember] public string partyID;
        [DataMember] public string leaderID;
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
    public class JoinNotification
    {
        [DataMember] public string userID;
    }

    [DataContract]
    public class KickNotification
    {
        [DataMember] public string leaderID;
        [DataMember] public string userID;
        [DataMember] public string partyID;
    }

    [DataContract]
    public class LeaveNotification
    {
        [DataMember] public string leaderID;
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
        [DataMember] public string userID;
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
    }

    [DataContract]
    public class MatchmakingNotif
    {
        [DataMember] public string status;
        [DataMember] public string matchId;
    }

    [DataContract]
    public class DsNotif
    {
        [DataMember] public string status;
        [DataMember] public string matchId;
        [DataMember] public string podName;
        [DataMember] public string ip;
        [DataMember] public int port;
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
    
    #endregion

    #region Friends

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
    public class FriendsStatus
    {
        [DataMember] public string[] friendsId;
        [DataMember] public string[] availability;
        [DataMember] public string[] activity;
        [DataMember] public DateTime[] lastSeenAt;
    }

    [DataContract]
    public class BulkFriendsRequest
    {
        [DataMember] public string[] friendIds { get; set; }
    }

    #endregion

    #region Presence

    public enum UserStatus
    {
        Offline = 0,
        Availabe = 1,
        Busy = 2,
        Invisible = 3
    }

    public enum GeneralUserStatus
    {
        offline,
        online,
        busy,
        invisible
    }
    
    [DataContract]
    public class FriendsStatusNotif
    {
        [DataMember] public string userID;
        [DataMember] public string availability;
        [DataMember] public string activity;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract]
    class SetUserStatusRequest
    {
        [DataMember] public uint availability;
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
        [DataMember] public GeneralUserStatus availability;
        [DataMember] public string activity;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public DateTime lastSeenAt;
    }

    [DataContract]
    public class BulkUserStatusNotif
    {
        [DataMember] public UserStatusNotif[] data;
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
}