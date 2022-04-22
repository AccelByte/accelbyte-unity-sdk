// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to connect to Lobby and access its services. 
    /// </summary>
    public class Lobby
    {
        /// <summary>
        /// Raised when lobby is connected
        /// </summary>
        public event Action Connected;

        /// <summary>
        /// Raised when lobby got message from server that it will disconnect
        /// </summary>
        public event ResultCallback<DisconnectNotif> Disconnecting;

        /// <summary>
        /// Raised when lobby is disconnected
        /// </summary>
        public event Action<WsCloseCode> Disconnected;

        /// <summary>
        /// Raised when a user is invited to party
        /// </summary>
        public event ResultCallback<PartyInvitation> InvitedToParty;

        /// <summary>
        /// Raised when some user joined our party
        /// </summary>
        public event ResultCallback<JoinNotification> JoinedParty;

        /// <summary>
        /// Raised when the user is kicked from party
        /// </summary>
        public event ResultCallback<KickNotification> KickedFromParty;

        /// <summary>
        /// Raised when the user leave from party
        /// </summary>
        public event ResultCallback<LeaveNotification> LeaveFromParty;

        /// <summary>
        /// Raised when a user reject party invitation
        /// </summary>
        public event ResultCallback<PartyRejectNotif> RejectedPartyInvitation;

        /// <summary>
        /// Raised when personal chat message received.
        /// </summary>
        public event ResultCallback<ChatMessage> PersonalChatReceived;

        /// <summary>
        /// Raised when party chat message received.
        /// </summary>
        public event ResultCallback<ChatMessage> PartyChatReceived;

        /// <summary>
        /// Raised when a notification (usually from the system or admin) is received.
        /// </summary>
        public event ResultCallback<Notification> OnNotification;

        /// <summary>
        /// Raised when friends status changed 
        /// </summary>
        public event ResultCallback<FriendsStatusNotif> FriendsStatusChanged;

        /// <summary>
        /// Raised when friend request accepted
        /// </summary>        
        public event ResultCallback<Friend> FriendRequestAccepted;

        /// <summary>
        /// Raised when there is an incoming friend request
        /// </summary>
        public event ResultCallback<Friend> OnIncomingFriendRequest;

        /// <summary>
        /// Raised when friend remove user from friendlist
        /// </summary>
        public event ResultCallback<Friend> OnUnfriend;

        /// <summary>
        /// Raised when friend request canceled
        /// </summary>
        public event ResultCallback<Acquaintance> FriendRequestCanceled;

        /// <summary>
        /// Raised when friend request rejected
        /// </summary>
        public event ResultCallback<Acquaintance> FriendRequestRejected;

        /// <summary>
        /// Raised when matchmaking process is completed.
        /// </summary>
        public event ResultCallback<MatchmakingNotif> MatchmakingCompleted;

        /// <summary>
        /// Raised when user from within the same party confirmed he/she is ready for match
        /// </summary>
        public event ResultCallback<ReadyForMatchConfirmation> ReadyForMatchConfirmed;

        /// <summary>
        /// Raised when DS process is updated.
        /// </summary>
        public event ResultCallback<DsNotif> DSUpdated;

        public event ResultCallback<RematchmakingNotification> RematchmakingNotif;
        
        /// <summary>
        /// Raised when there's an update in the party's storage.
        /// </summary>
        public event ResultCallback<PartyDataUpdateNotif> PartyDataUpdateNotif;

        /// <summary>
        /// Raised when channel chat message received.
        /// </summary>
        public event ResultCallback<ChannelChatMessage> ChannelChatReceived;

        /// <summary>
        /// Raised when player is blocked.
        /// </summary>
        public event ResultCallback<PlayerBlockedNotif> PlayerBlockedNotif;
        
        /// <summary>
        /// Raised when player is unblocked.
        /// </summary>
        public event ResultCallback<PlayerUnblockedNotif> PlayerUnblockedNotif;

        /// <summary>
        /// Raised when player is banned
        /// </summary>
        public event ResultCallback<UserBannedNotification> UserBannedNotification;

        /// <summary>
        /// Raised when player is unbanned
        /// </summary>
        public event ResultCallback<UserBannedNotification> UserUnbannedNotification;

        /// <summary>
        /// Raised when there is an incoming signaling notification
        /// </summary>
        public event ResultCallback<SignalingP2P> SignalingP2PNotification;

        /// <summary>
        /// Raised when lobby access token succesfully updated
        /// </summary>
        public event Action TokenRefreshed;

        private readonly int pingDelay;
        private int backoffDelay;
        private int maxDelay;
        private int totalTimeout;

        private readonly Dictionary<long, Action<ErrorCode, string>> responseCallbacks =
            new Dictionary<long, Action<ErrorCode, string>>();

        private readonly LobbyApi api;
        private readonly string @namespace;
        private readonly string websocketUrl;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        private readonly object syncToken = new object();
        private AccelByteWebSocket webSocket;
        private bool reconnectsOnBans;
        private long id;
        private LobbySessionId lobbySessionId;
        private string channelSlug = null;

        public event EventHandler OnRetryAttemptFailed;

        internal Lobby(string websocketUrl, IWebSocket webSocket, LobbyApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner,
            int pingDelay = 4000, int backoffDelay = 1000, int maxDelay = 30000, int totalTimeout = 60000)
        {
            Assert.IsNotNull(webSocket);
            Assert.IsNotNull(coroutineRunner);

            this.websocketUrl = websocketUrl;
            this.webSocket = new AccelByteWebSocket(webSocket, coroutineRunner);
            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
            this.pingDelay = pingDelay;
            this.backoffDelay = backoffDelay;
            this.maxDelay = maxDelay;
            this.totalTimeout = totalTimeout;
            this.reconnectsOnBans = false;
            this.lobbySessionId = new LobbySessionId();

            this.webSocket.OnOpen += HandleOnOpen;
            this.webSocket.OnMessage += HandleOnMessage;
            this.webSocket.OnClose += HandleOnClose;
        }

        /// <summary>
        /// Lobby connection status
        /// </summary>
        public bool IsConnected { get { return this.webSocket.IsConnected; } }

        /// <summary>
        /// Connect to lobby with current logged in user credentials.
        /// The token generator need to be set for connection with entitlement verification.
        /// </summary>
        public void Connect()
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                throw new Exception("Cannot connect to websocket because user is not logged in.");
            }

            if (this.IsConnected)
            {
                AccelByteDebug.LogWarning("[Lobby] already connected");
                return;
            }
            
            if (this.webSocket.IsConnecting)
            {
                AccelByteDebug.LogWarning("[Lobby] lobby is connecting");
                return;
            }

            this.webSocket.Connect(this.websocketUrl, this.session.AuthorizationToken, this.lobbySessionId.lobbySessionID);
        }

        /// <summary>
        /// Change the delay parameters to maintain connection in the lobby before lobby connected.
        /// </summary>
        /// <param name="totalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="backoffDelay">Initial delay time</param>
        /// <param name="maxDelay">Maximum delay time</param>
        public void SetRetryParameters(int totalTimeout = 60000, int backoffDelay = 1000, int maxDelay = 30000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            webSocket.SetRetryParameters(totalTimeout, backoffDelay, maxDelay);
        }

        /// <summary>
        /// TokenGenerator is used for generate access token when connecting to lobby. 
        /// If token generator is not specified, no token will be used when connecting to lobby.
        /// For entitlement token verification, use EntitlementTokenGenerator class on the parameter.
        /// </summary>
        /// <param name="tokenGenerator"> Token generator for connecting lobby. </param>
        public void SetConnectionTokenGenerator(ITokenGenerator tokenGenerator)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsFalse(tokenGenerator == null, "Can't set connection token generator! Token generator is null.");

            this.webSocket.SetConnectionTokenGenerator(tokenGenerator);
        }

        /// <summary>
        /// Disconnect from Lobby.
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.webSocket.Disconnect();

            this.channelSlug = null;
            LoginSession loginSession = session as LoginSession;
            if (loginSession != null)
            {
                loginSession.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }
        }

        /// <summary>
        /// Get party information (leader, members, invitation token)
        /// </summary>
        /// <param name="callback">Returns a Result that contains PartyInfo via callback when completed.</param>
        public void GetPartyInfo(ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyInfoRequest, callback);
        }

        /// <summary>
        /// Create a party and become a party leader for the party. 
        /// </summary>
        /// <param name="callback">Returns a Result that contain PartyInfo via callback when completed.</param>
        public void CreateParty(ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyCreateRequest, callback);
        }

        /// <summary>
        /// Leave a party a current logged in user is in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LeaveParty(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyLeaveRequest, callback);
        }

        /// <summary>
        /// Invite other user by userId. Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void InviteToParty(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyInviteRequest, new PartyInviteRequest {friendID = userId}, callback);
        }

        /// <summary>
        /// Invite other user by userId with detailed model in response callback. Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">Returns a Result that contain PartyInviteResponse via callback when completed</param>
        public void InviteToPartyDetailedCallback(string userId, ResultCallback<PartyInviteResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyInviteRequest, new PartyInviteRequest { friendID = userId }, callback);
        }

        /// <summary>
        /// Join a party by invitation from party leader.
        /// </summary>
        /// <param name="partyID">Party ID of the party to join to</param>
        /// <param name="invitationToken">Invitation token to join the party</param>
        /// <param name="callback">Returns a Result that contains PartyInfo via callback when completed</param>
        public void JoinParty(string partyID, string invitationToken, ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.partyJoinRequest,
                new PartyJoinRequest {partyID = partyID, invitationToken = invitationToken},
                callback);
        }

        /// <summary>
        /// Kick a member out of our party. Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void KickPartyMember(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyKickRequest, new PartyKickRequest {memberID = userId}, callback);
        }

        /// <summary>
        /// Kick a member out of our party with detailed model in response callback. Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">Returns a Result that contains PartyKickResponse via callback when completed.</param>
        public void KickPartyMemberDetailedCallback(string userId, ResultCallback<KickResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyKickRequest, new PartyKickRequest { memberID = userId }, callback);
        }

        /// <summary>
        /// Reject a party invitation.
        /// </summary>
        /// <param name="partyId">Party ID of an incoming party invitation that will be declined.</param>
        /// <param name="invitationToken">Invitation token of an incoming party invitation that will be declined.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RejectPartyInvitation(string partyId, string invitationToken, ResultCallback<PartyRejectResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyRejectRequest, new PartyRejectRequest{invitationToken = invitationToken, partyID =  partyId}, callback);
        }

        /// <summary>
        /// Promote member to be a party leader.
        /// </summary>
        /// <param name="userId">User ID that will be promoted as a party leader.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void PromotePartyLeader(string userId, ResultCallback<PartyPromoteLeaderResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyPromoteLeaderRequest, new PartyPromoteLeaderRequest { newLeaderUserId = userId }, callback);
        }

        /// <summary>
        /// Send chat to other party members
        /// </summary>
        /// <param name="chatMessage">Message to send to party</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPartyChat(string chatMessage, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.partyChatRequest, new PartyChatRequest {payload = chatMessage}, callback);
        }

        /// <summary>
        /// Send personal chat to friend
        /// </summary>
        /// <param name="userId">Friend user id</param>
        /// <param name="chatMessage">Message to send to friend</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPersonalChat(string userId, string chatMessage, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.personalChatRequest,
                new PersonalChatRequest {to = userId, payload = chatMessage},
                callback);
        }

        /// <summary>
        /// Set current user status and activity
        /// </summary>
        /// <param name="status">User status (Online, Available, Busy, Invisible)</param>
        /// <param name="activity">Describe an activity of the user, could be anything.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void SetUserStatus(UserStatus status, string activity, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.setUserStatusRequest,
                new SetUserStatusRequest {availability = (uint) status, activity = Uri.EscapeDataString(activity)},//Escape the string first for customizable string
                callback);
        }

        /// <summary>
        /// Get a list of friends status (availability, activity, last seen at)
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends Status via callback when completed.</param>
        public void ListFriendsStatus(ResultCallback<FriendsStatus> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.friendsStatusRequest, callback);
        }

        /// <summary>
        ///  Ask lobby to send all pending notification to user. Listen to OnNotification.
        /// </summary> 
        /// <param name="callback">Returns a Result via callback when completed.</param>
        [Obsolete("Lobby 2.4.0 and above dropped support for this function")]
        public void PullAsyncNotifications(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.offlineNotificationRequest, callback);
        }

        /// <summary>
        /// Send request friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriend(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.requestFriendsRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send request friend request in bulk.
        /// </summary>
        /// <param name="userIds">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void BulkRequestFriend(string[] userIds, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            BulkFriendsRequest otherUserIds= new BulkFriendsRequest{ friendIds = userIds };
            coroutineRunner.Run(
                this.api.BulkFriendRequest(
                    this.@namespace,
                    this.session.UserId,
                    otherUserIds,
                    this.session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Send unfriend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void Unfriend(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.unfriendRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send list of outgoing friends request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void ListOutgoingFriends(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.listOutgoingFriendsRequest, callback);
        }

        /// <summary>
        /// Send cancel friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void CancelFriendRequest(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.cancelFriendsRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send list of incoming friends request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void ListIncomingFriends(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.listIncomingFriendsRequest, callback);
        }

        /// <summary>
        /// Send accept friend request. 
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void AcceptFriend(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.acceptFriendsRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send reject friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void RejectFriend(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.rejectFriendsRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send load friends list request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void LoadFriendsList(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.listOfFriendsRequest, callback);
        }

        /// <summary>
        /// Send get friendship status request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result that contains FriendshipStatus via callback when completed.</param>
        public void GetFriendshipStatus(string userId, ResultCallback<FriendshipStatus> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.getFriendshipStatusRequest, new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send request get user presence in bulk.
        /// </summary>
        /// <param name="userIds">requested userIds</param>
        /// <param name="callback">Returns a Result that contains BulkUserStatusNotif via callback when completed.</param>
        public void BulkGetUserPresence(string[] userIds, ResultCallback<BulkUserStatusNotif> callback, bool countOnly = false)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                this.api.BulkGetUserPresence(
                    this.@namespace,
                    userIds,
                    this.session.AuthorizationToken,
                    callback,
                    countOnly));
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, "", null, null, null, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, serverName, null, null, null, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, string clientVersion,
            ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, serverName, clientVersion, null, null, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string clientVersion,
            Dictionary<string, int> latencies, ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, "", clientVersion, latencies, null, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string clientVersion,
            Dictionary<string, int> latencies, Dictionary<string, object> partyAttributes, ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, "", clientVersion, latencies, partyAttributes, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">UserIDs to form a temporary party with (include user who started the matchmaking). Temporary party will disband when matchmaking finishes.</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string clientVersion, Dictionary<string, int> latencies,
            Dictionary<string, object> partyAttributes, string[] tempPartyUserIds,
            ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, "", clientVersion, latencies, partyAttributes, tempPartyUserIds, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">UserIDs to form a temporary party with (include user who started the matchmaking). Temporary party will disband when matchmaking finishes.</param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string clientVersion, Dictionary<string, int> latencies,
            Dictionary<string, object> partyAttributes, string[] tempPartyUserIds, string[] extraAttributes,
            ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, "", clientVersion, latencies, partyAttributes, tempPartyUserIds, extraAttributes, callback);
        }


        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">Party attributes the matchmaker will do string equality check to matchmake with other party's party attribute</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, string clientVersion, Dictionary<string, object> partyAttributes, ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, serverName, clientVersion, null, partyAttributes, null, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">UserIDs to form a temporary party with (include user who started the matchmaking). Temporary party will disband when matchmaking finishes.</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, string clientVersion, 
            Dictionary<string, object> partyAttributes, string[] tempPartyUserIds, 
            ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, serverName, clientVersion, null, partyAttributes, tempPartyUserIds, null, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">UserIDs to form a temporary party with (include user who started the matchmaking). Temporary party will disband when matchmaking finishes.</param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, string clientVersion,
            Dictionary<string, object> partyAttributes, string[] tempPartyUserIds, string[] extraAttributes,
            ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(gameMode, serverName, clientVersion, null, partyAttributes, tempPartyUserIds, extraAttributes, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies"></param> Preferred latencies
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">UserIDs to form a temporary party with (include user who started the matchmaking). Temporary party will disband when matchmaking finishes.</param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(
            string gameMode,
            string serverName,
            string clientVersion,
            Dictionary<string, int> latencies,
            Dictionary<string, object> partyAttributes,
            string[] tempPartyUserIds,
            string[] extraAttributes,
            ResultCallback<MatchmakingCode> callback)
        {
            MatchmakingOptionalParam param = new MatchmakingOptionalParam
            {
                serverName = serverName,
                clientVersion = clientVersion,
                latencies = latencies,
                partyAttributes = partyAttributes,
                tempPartyUserIds = tempPartyUserIds,
                extraAttributes = extraAttributes
            };

            StartMatchmaking(gameMode, param, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        public void StartMatchmaking(string gameMode, MatchmakingOptionalParam param, ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            string strLatencies = "";
            string jsonAttributeString = "";
            string userIdsCSV = "";
            string extraAttributesCSV = "";

            if(param != null)
            {
                if(param.latencies != null && param.latencies.Count > 0)
                {
                    strLatencies = "{" +
                    string.Join(",", param.latencies.Select(pair => $@"""{pair.Key}"":{pair.Value}").ToArray()) +
                    "}";
                }

                if(param.tempPartyUserIds != null && param.tempPartyUserIds.Length > 0)
                {
                    userIdsCSV = String.Join(",", param.tempPartyUserIds);
                }

                if(param.extraAttributes != null && param.extraAttributes.Length > 0)
                {
                    extraAttributesCSV = string.Join(",", param.extraAttributes);
                }

                if(param.subGameModes != null && param.subGameModes.Length > 0)
                {
                    if (param.partyAttributes == null)
                        param.partyAttributes = new Dictionary<string, object>();

                    param.partyAttributes.Add("sub_game_mode", param.subGameModes);
                }

                if(param.newSessionOnly)
                {
                    if (param.partyAttributes == null)
                        param.partyAttributes = new Dictionary<string, object>();

                    param.partyAttributes.Add("new_session_only", "true");
                }

                if(param.partyAttributes != null)
                {
                    jsonAttributeString = param.partyAttributes.ToJsonString();
                }
            }

            SendRequest(
                MessageType.startMatchmakingRequest,
                new StartMatchmakingRequest
                {
                    gameMode = gameMode,
                    serverName = param?.serverName,
                    clientVersion = param?.clientVersion,
                    latencies = strLatencies,
                    partyAttributes = jsonAttributeString,
                    tempParty = userIdsCSV,
                    extraAttributes = extraAttributesCSV
                },
                callback);
        }
        
        /// <summary>
        /// Send a message to matchmaking service to indicate the user is ready for match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="callback"></param>
        public void ConfirmReadyForMatch(string matchId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.setReadyConsentRequest, new ReadyConsentRequest {matchId = matchId}, callback);
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">Result of the function with a cancel matchmaking status code.</param>
        public void CancelMatchmaking(string gameMode, ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.cancelMatchmakingRequest,
                new StartMatchmakingRequest {gameMode = gameMode},
                callback);
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="isTempParty">Is using temp party when starting matchmaking</param>
        /// <param name="callback">Result of the function with a cancel matchmaking status code.</param>
        public void CancelMatchmaking(string gameMode, bool isTempParty, 
            ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.cancelMatchmakingRequest,
                new StartMatchmakingRequest { 
                    gameMode = gameMode,
                    isTempParty = isTempParty
                },
                callback);
        }

        /// <summary>
        /// Send Join default global chat channel request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains ChatChannelSlug via callback when completed.</param>
        public void JoinDefaultChatChannel(ResultCallback<ChatChannelSlug> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest<ChatChannelSlug>(MessageType.joinDefaultChannelRequest, result => 
            {
                if (result.IsError)
                {
                    callback.TryError(result.Error);
                }
                else
                {
                    channelSlug = result.Value.channelSlug;
                    callback.Try(result);
                }
            });
        }

        /// <summary>
        /// Send a Chat Message to a Global Chat Channel.
        /// </summary>
        /// <param name="chatMessage">Message to send to the channel</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendChannelChat(string chatMessage, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (string.IsNullOrEmpty(channelSlug))
            {
                callback.TryError(ErrorCode.InvalidRequest, "You're not in any chat channel.");
            }
            else
            {
                SendRequest(MessageType.sendChannelChatRequest, new ChannelChatRequest
                {
                    channelSlug = channelSlug,
                    payload = chatMessage
                }, callback);
            }
        }

        /// <summary>
        /// Get party storage by party ID.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void GetPartyStorage(string partyId, ResultCallback<PartyDataUpdateNotif> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(
                this.api.GetPartyStorage(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    partyId,
                    callback));
        }

        /// <summary>
        /// Write party storage data to the targeted party ID.
        /// Beware:
        /// Object will not be write immediately, please take care of the original object until it written.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        /// <param name="payloadModifier">Function to modify the latest party data with your customized modifier.</param>
        /// <param name="retryAttempt">the number of retry to do when there is an error in writing to party storage (likely due to write conflicts)</param>
        public void WritePartyStorage(string partyId, ResultCallback<PartyDataUpdateNotif> callback, Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier, int retryAttempt = 1)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId), "Party ID should not be null.");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            WritePartyStorageRecursive(retryAttempt, partyId, callback, payloadModifier);
        }

        private void WritePartyStorageRecursive(int remainingAttempt, string partyId, ResultCallback<PartyDataUpdateNotif> callback, Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier)
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed, "Exhaust all retry attempt to modify party storage. Please try again."));
                return;
            }

            GetPartyStorage(partyId, getPartyStorageResult =>
            {
                if (getPartyStorageResult.IsError)
                {
                    callback.TryError(getPartyStorageResult.Error);
                }
                else
                {
                    getPartyStorageResult.Value.custom_attribute = payloadModifier(getPartyStorageResult.Value.custom_attribute);

                    var updateRequest = new PartyDataUpdateRequest();
                    updateRequest.custom_attribute = getPartyStorageResult.Value.custom_attribute;
                    updateRequest.updatedAt = getPartyStorageResult.Value.updatedAt;

                    coroutineRunner.Run(
                        this.api.WritePartyStorage(
                            this.@namespace,
                            this.session.AuthorizationToken,
                            updateRequest,
                            partyId,
                            callback,
                            () =>
                            {
                                WritePartyStorageRecursive(remainingAttempt - 1, partyId, callback, payloadModifier);
                            }));
                }
            });
        }

        /// <summary>
        /// Block the specified player from doing some action against current user.
        /// The specified player will be removed from current user's friend list too.
        /// 
        /// Actions that prevented to do each other:
        /// * add friend
        /// * direct chat
        /// * invite to party 
        /// * invite to group
        /// * matchmaking result as one alliance 
        ///
        /// Additional limitation:
        /// * blocked player cannot access blocker/current user's UserProfile.
        /// 
        /// </summary>
        /// <param name="userId">Blocked user's user ID</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void BlockPlayer(string userId, ResultCallback<BlockPlayerResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.blockPlayerRequest, new BlockPlayerRequest
            {
                userId = this.session.UserId,
                blockedUserId = userId,
                Namespace = @namespace
            }, callback);
        }


        /// <summary>
        /// Unblock the specified player and allow it to some action against current user again.
        /// 
        /// Allow each other to:
        /// * add friend
        /// * direct chat
        /// * invite to party 
        /// * invite to group
        /// * matchmaking result as one alliance 
        ///
        /// Additional limitation:
        /// * unblocked player can access blocker/current user's UserProfile.
        /// 
        /// </summary>
        /// <param name="userId">Unblocked user's user ID</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void UnblockPlayer(string userId, ResultCallback<UnblockPlayerResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.unblockPlayerRequest, new UnblockPlayerRequest
            {
                userId = this.session.UserId,
                unblockedUserId = userId,
                Namespace = @namespace
            }, callback);
        }

        
        public void GetListOfBlockedUser(ResultCallback<BlockedList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(
                this.api.GetListOfBlockedUser(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    this.session.UserId,
                    callback));
        }

        
        public void GetListOfBlocker(ResultCallback<BlockerList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            coroutineRunner.Run(
               this.api.GetListOfBlocker(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    this.session.UserId,
                    callback));
        }

        public void SetProfanityFilterLevel(ProfanityFilterLevel level, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(MessageType.setSessionAttributeRequest, new SetSessionAttributeRequest()
            {
                Namespace = this.@namespace,
                key = SessionAttributeName.profanity_filtering_level.ToString(),
                value = level.ToString()
            }, callback);
        }

        public void SetSessionAttribute(string key, string value, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            SendRequest(MessageType.setSessionAttributeRequest, new SetSessionAttributeRequest()
            {
                Namespace = this.@namespace,
                key = key,
                value = value
            }, callback);
        }

        public void GetSessionAttribute(string key, ResultCallback<GetSessionAttributeResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            SendRequest(MessageType.getSessionAttributeRequest, new GetSessionAttributeRequest()
            {
                key = key
            }, callback);
        }

        public void GetSessionAttributeAll(ResultCallback<GetSessionAttributeAllResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            SendRequest(MessageType.getAllSessionAttributeRequest, callback);
        }

        /// <summary>
        /// Send a signaling message to another user.
        /// </summary>
        /// <param name="userId">The recipient's user ID.</param>
        /// <param name="message">Signaling message to be sent.</param>
        public void SendSignalingMessage(string userId, string message)
        {
            SendRequest(MessageType.signalingP2PNotif, new SignalingP2P { destinationId = userId, message = message }, r => { });
        }

        private void RefreshToken(string newAccessToken, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            SendRequest(MessageType.refreshTokenRequest, new RefreshAccessTokenRequest()
            {
                token = newAccessToken
            }, callback);
        }

        private long GenerateId()
        {
            lock (this.syncToken)
            {
                if (this.id < Int64.MaxValue)
                {
                    this.id++;
                }
                else
                {
                    this.id = 0;
                }
            }

            return this.id;
        }

        private void SendRequest<T, U>(MessageType requestType, T requestPayload, ResultCallback<U> callback)
            where T : class, new() where U : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);
            AwesomeFormat.WritePayload(writer, requestPayload);

            this.responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result<U> result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result<U>.CreateError(errorCode);
                }
                else
                {
                    U responsePayload;
                    errorCode = AwesomeFormat.ReadPayload(response, out responsePayload);

                    result = errorCode != ErrorCode.None
                        ? Result<U>.CreateError(errorCode)
                        : Result<U>.CreateOk(responsePayload);
                }

                coroutineRunner.Run(() => callback.Try(result));
            };

            this.webSocket.Send(writer.ToString());
        }

        private void SendRequest<T>(MessageType requestType, T requestPayload, ResultCallback callback)
            where T : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);
            AwesomeFormat.WritePayload(writer, requestPayload);

            this.responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result result = errorCode != ErrorCode.None ? Result.CreateError(errorCode) : Result.CreateOk();

                coroutineRunner.Run(() => callback.Try(result));
            };

            this.webSocket.Send(writer.ToString());
        }

        private void SendRequest<U>(MessageType requestType, ResultCallback<U> callback) where U : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);

            this.responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result<U> result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result<U>.CreateError(errorCode);
                }
                else
                {
                    U responsePayload;
                    errorCode = AwesomeFormat.ReadPayload(response, out responsePayload);

                    result = errorCode != ErrorCode.None
                        ? Result<U>.CreateError(errorCode)
                        : Result<U>.CreateOk(responsePayload);
                }

                coroutineRunner.Run(() => callback.Try(result));
            };

            this.webSocket.Send(writer.ToString());
        }

        private void SendRequest(MessageType requestType, ResultCallback callback)
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);

            this.responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result result = errorCode != ErrorCode.None ? Result.CreateError(errorCode) : Result.CreateOk();

                coroutineRunner.Run(() => callback.Try(result));
            };

            this.webSocket.Send(writer.ToString());
        }

        private void HandleOnOpen()
        {
            // debug ws connection
#if DEBUG
            AccelByteDebug.Log("[WS] Connection open");
#endif
            Action handler = this.Connected;

            if (handler != null)
            {
                handler();
            }

            LoginSession loginSession = session as LoginSession;
            if (loginSession != null)
            {
                loginSession.RefreshTokenCallback += LoginSession_RefreshTokenCallback;
            }
        }

        private void LoginSession_RefreshTokenCallback(string accessToken)
        {
            RefreshToken(accessToken, result =>
            {
                TokenRefreshed?.Invoke();
                AccelByteDebug.Log($"Updating access token in lobby {(result.IsError ? $"Error with code {result.Error.Code}, message {result.Error.Message}" : "Success")}");
            });
        }

        private void HandleOnClose(ushort closecode)
        {
            // debug ws connection
#if DEBUG
            AccelByteDebug.Log("[WS] Connection close: " + closecode);
#endif
            var code = (WsCloseCode)closecode;

            LoginSession loginSession = session as LoginSession;
            if (loginSession != null)
            {
                loginSession.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }

            this.Disconnected?.Invoke(code);

            if (reconnectsOnBans)
            {
                reconnectsOnBans = false;
                coroutineRunner.Run(ReconnectOnBan());
            }
        }

        private IEnumerator ReconnectOnBan()
        {
            System.Random rand = new System.Random();
            float randomFloat = (float)(rand.NextDouble());
            yield return new WaitForSeconds((backoffDelay + ((randomFloat * 0.5f) - 0.5f)) / 1000);

            Connect();
        }
        private void HandleOnMessage(string message)
        {
            Report.GetWebSocketResponse(message);

            long messageId;
            MessageType messageType;
            ErrorCode errorCode = AwesomeFormat.ReadHeader(message, out messageType, out messageId);

            switch (messageType)
            {
            case MessageType.partyGetInvitedNotif:
                HandleNotification(message, this.InvitedToParty);

                break;
            case MessageType.partyJoinNotif:
                HandleNotification(message, this.JoinedParty);

                break;
            case MessageType.partyKickNotif:
                HandleNotification(message, this.KickedFromParty);

                break;
            case MessageType.partyLeaveNotif:
                HandleNotification(message, this.LeaveFromParty);

                break;
            case MessageType.personalChatNotif:
                HandleNotification(message, this.PersonalChatReceived);

                break;
            case MessageType.partyChatNotif:
                HandleNotification(message, this.PartyChatReceived);

                break;
            case MessageType.messageNotif:
                HandleNotification(message, this.OnNotification);

                break;
            case MessageType.userStatusNotif:
                HandleNotification(message, this.FriendsStatusChanged);

                break;
            case MessageType.matchmakingNotif:
                HandleNotification(message, this.MatchmakingCompleted);

                break;
            case MessageType.dsNotif:
                HandleNotification(message, this.DSUpdated);

                break;
            case MessageType.acceptFriendsNotif:
                HandleNotification(message, this.FriendRequestAccepted);

                break;
            case MessageType.requestFriendsNotif:
                HandleNotification(message, this.OnIncomingFriendRequest);

                break;
            case MessageType.unfriendNotif:
                HandleNotification(message, this.OnUnfriend);

                break;
            case MessageType.cancelFriendsNotif:
                HandleNotification(message, this.FriendRequestCanceled);

                break;
            case MessageType.rejectFriendsNotif:
                HandleNotification(message, this.FriendRequestRejected);

                break;
            case MessageType.setReadyConsentNotif:
                HandleNotification(message, this.ReadyForMatchConfirmed);

                break;
            case MessageType.rematchmakingNotif:
                HandleNotification(message, this.RematchmakingNotif);

                break;
            case MessageType.channelChatNotif:
                HandleNotification(message, this.ChannelChatReceived);

                break;
            case MessageType.connectNotif:
                AwesomeFormat.ReadPayload(message, out lobbySessionId);
                webSocket.SetSessionId(lobbySessionId.lobbySessionID);
                break;
            case MessageType.disconnectNotif:
                HandleNotification(message, this.Disconnecting);
                break;
            case MessageType.partyDataUpdateNotif:
                HandleNotification(message, this.PartyDataUpdateNotif);
                break;
            case MessageType.partyRejectNotif:
                HandleNotification(message, this.RejectedPartyInvitation);
                break;
            case MessageType.blockPlayerNotif:
                HandleNotification(message, this.PlayerBlockedNotif);
                break;
            case MessageType.unblockPlayerNotif:
                HandleNotification(message, this.PlayerUnblockedNotif);
                break;
            case MessageType.userBannedNotification:
                HandleNotification(message, this.UserBannedNotification);
                break;
            case MessageType.userUnbannedNotification:
                HandleNotification(message, this.UserUnbannedNotification);
                break;
            case MessageType.signalingP2PNotif:
                HandleNotification(message, this.SignalingP2PNotification);
                break;
            default:
                Action<ErrorCode, string> handler;

                if (messageId != -1 && this.responseCallbacks.TryGetValue(messageId, out handler))
                {
                    this.responseCallbacks.Remove(messageId);
                    handler(errorCode, message);
                }

                break;
            }

            if (messageType == MessageType.userBannedNotification 
                || messageType == MessageType.userUnbannedNotification)
            {
                coroutineRunner.Run(() =>
                {
                    HandleBanNotification();
                });
            }
        }

        private void HandleNotification<T>(string message, ResultCallback<T> handler) where T : class, new()
        {
            Report.GetWebSocketNotification(message);

            if (handler == null)
            {
                return;
            }

            T payload;
            ErrorCode errorCode = AwesomeFormat.ReadPayload(message, out payload);

            if (errorCode != ErrorCode.None)
            {
                coroutineRunner.Run( ()=>handler(Result<T>.CreateError(errorCode)));
            }
            else
            {
                coroutineRunner.Run( ()=>handler(Result<T>.CreateOk(payload)));
            }
        }

        private void HandleBanNotification()
        {
            Report.GetFunctionLog(this.GetType().Name);
            reconnectsOnBans = true;

            LoginSession loginSession = session as LoginSession;
            if (loginSession != null)
            {
                loginSession.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }

            this.api.OnBanNotificationReceived(this.session.AuthorizationToken,
            session =>
            {
                UpdateAuthToken(session);
            });
        }

        private void UpdateAuthToken(string session)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.session.AuthorizationToken = session;
        }
    }
}
