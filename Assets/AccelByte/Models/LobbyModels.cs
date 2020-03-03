// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
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
    }

    [DataContract]
    public class DisconnectNotif
    {
        [DataMember] public string message;
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
    public class ChatMesssage
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

    #endregion

    #region Matchmaking

    [DataContract]
    public class StartMatchmakingRequest
    {
        [DataMember] public string gameMode;
        [DataMember] public string serverName;
        [DataMember] public string clientVersion;
        [DataMember] public string latencies;
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
    
    #endregion

    #region Presence

    public enum UserStatus
    {
        Offline = 0,
        Availabe = 1,
        Busy = 2,
        Invisible = 3
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
    
    #endregion
}