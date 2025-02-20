// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Random = System.Random;
using Core.Websocket;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to connect to Lobby and access its services.
    /// </summary>
    public class Lobby : WrapperBase
    {
        #region PublicEvents
        
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
        /// SessionV2 - Raised when a party session is updated
        /// </summary>
        public event ResultCallback<SessionV2PartySessionUpdatedNotification> SessionV2PartyUpdated;

        /// <summary>
        /// SessionV2 - Raised when a user is being invited to a party
        /// </summary>
        public event ResultCallback<SessionV2PartyInvitationNotification> SessionV2InvitedUserToParty;

        /// <summary>
        /// SessionV2 - Raised when a user joined a party
        /// </summary>
        public event ResultCallback<SessionV2PartyJoinedNotification> SessionV2UserJoinedParty;

        /// <summary>
        /// SessionV2 - Raised when a party's members changed
        /// </summary>
        public event ResultCallback<SessionV2PartyMembersChangedNotification> SessionV2PartyMemberChanged;

        /// <summary>
        /// SessionV2 - Raised when a user rejected party invitation
        /// </summary>
        public event ResultCallback<SessionV2PartyInvitationRejectedNotification> SessionV2UserRejectedPartyInvitation;

        /// <summary>
        /// SessionV2 - Raised when a leader is canceling an invitation
        /// </summary>
        public event ResultCallback<SessionV2PartyInvitationCancelledNotification> SessionV2PartyInvitationCancelled;

        /// <summary>
        /// SessionV2 - Raised when a user is kicked from party
        /// </summary>
        public event ResultCallback<SessionV2PartyUserKickedNotification> SessionV2UserKickedFromParty;

        /// <summary>
        /// SessionV2 - Raised when a user is being invited to a game session
        /// </summary>
        public event ResultCallback<SessionV2GameInvitationNotification> SessionV2InvitedUserToGameSession;

        /// <summary>
        /// SessionV2 - Raised when a user joined a game session
        /// </summary>
        public event ResultCallback<SessionV2GameJoinedNotification> SessionV2UserJoinedGameSession;

        /// <summary>
        /// SessionV2 - Raised when a game session's members changed
        /// </summary>
        public event ResultCallback<SessionV2GameMembersChangedNotification> SessionV2GameSessionMemberChanged;

        /// <summary>
        /// SessionV2 - Raised when a user is kicked from game session
        /// </summary>
        public event ResultCallback<SessionV2GameUserKickedNotification> SessionV2UserKickedFromGameSession;

        /// <summary>
        /// SessionV2 - Raised when a user rejected game session invitation
        /// </summary>
        public event ResultCallback<SessionV2GameInvitationRejectedNotification>
            SessionV2UserRejectedGameSessionInvitation;

        /// <summary>
        /// SessionV2 - Raised when a game session is updated
        /// </summary>
        public event ResultCallback<SessionV2GameSessionUpdatedNotification> SessionV2GameSessionUpdated;

        /// <summary>
        /// SessionV2 - Raised when a game session is ended by the DS.
        /// </summary>
        public event ResultCallback<SessionV2GameSessionEndedNotification> SessionV2GameSessionEnded;

        /// <summary>
        /// SessionV2 - Raised when status of DS changed
        /// </summary>
        public event ResultCallback<SessionV2DsStatusUpdatedNotification> SessionV2DsStatusChanged;

        /// <summary>
        /// SessionV2 - Raised when session storage changed
        /// </summary>
        public event ResultCallback<SessionStorageChangedNotification> SessionV2StorageChanged;

        /// <summary>
        /// MatchmakingV2 - Raised when match is found
        /// </summary>
        public event ResultCallback<MatchmakingV2MatchFoundNotification> MatchmakingV2MatchFound;

        /// <summary>
        /// MatchmakingV2 - Raised when matchmaking started
        /// </summary>
        public event ResultCallback<MatchmakingV2MatchmakingStartedNotification> MatchmakingV2MatchmakingStarted;

        /// <summary>
        /// MatchmakingV2 - Raised when matchmaking is canceled
        /// </summary>
        public event ResultCallback<MatchmakingV2MatchmakingCanceledNotification> MatchmakingV2MatchmakingCanceled;

        /// <summary>
        /// MatchmakingV2 - Raised when matchmaking ticket expired
        /// </summary>
        public event ResultCallback<MatchmakingV2TicketExpiredNotification> MatchmakingV2TicketExpired;

        /// <summary>
        /// Raised when personal chat message received.
        /// </summary>
        public event ResultCallback<ChatMessage> PersonalChatReceived;

        /// <summary>
        /// Raised when party chat message received.
        /// </summary>
        public event ResultCallback<ChatMessage> PartyChatReceived;
        
        /// <summary>
        /// Triggered when party successfully created
        /// </summary>
        public event ResultCallback<PartyCreatedNotification> PartyCreatedNotification;

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
        /// Raised when DS process is updated.
        /// </summary>
        public event ResultCallback<DsNotif> DSUpdated;

        /// <summary>
        /// Raised when there's an update in the party's storage.
        /// </summary>
        public event ResultCallback<PartyDataUpdateNotif> PartyDataUpdateNotif;
        
        /// <summary>
        /// Raised when other party member send party notification.
        /// </summary>
        public event ResultCallback<PartyNotif> PartyNotif;

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
        /// Raised when a general error notification is sent from lobby
        /// </summary>
        public event ResultCallback ErrorNotification;

        /// <summary>
        /// Raised when lobby access token succesfully updated
        /// </summary>
        public event Action TokenRefreshed;

        #endregion PublicEvents

        private LobbyWebsocketApi websocketApi;
        
        private readonly LobbyApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        private AccelByteMessagingSystem messagingSystem;
        private AccelByteNotificationBuffer notificationBuffer;

        private NotificationQueue notificationQueue;
        private AccelByteHeartBeat ticker;
        private const int tickerInterval = 20;

        private AccelByteHeartBeat connectRetryTicker;
        private const int maxRetryAttempts = 3;
        private const int retryInterval = 20 * 1000;
        private int retryAttempts = 0;

        private int backoffDelay;
        
        private bool reconnectsOnBans;

        private Utils.IAccelByteWebsocketSerializer websocketSerializer;

        private HashSet<string> processedMatchmakingMessages = new HashSet<string>();

        internal string UserRegion;

        /// <summary>
        /// Event triggered each time a websocket reconnection attempt failed
        /// </summary>
        public event EventHandler OnRetryAttemptFailed
        {
            add => websocketApi.WebSocket.OnRetryAttemptFailed += value;
            remove => websocketApi.WebSocket.OnRetryAttemptFailed -= value;
        }

        [UnityEngine.Scripting.Preserve]
        internal Lobby(LobbyApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;

            reconnectsOnBans = false;

            websocketSerializer = new Utils.AwesomeFormat();

            IWebSocket webSocket = new NativeWebSocketWrapper();
            websocketApi = new LobbyWebsocketApi(coroutineRunner, api.GetConfig().LobbyServerUrl, webSocket, session, api.GetConfig().Namespace);
            
            websocketApi.OnOpen += HandleOnOpen;
            websocketApi.OnMessage += message =>
            {
                HandleOnMessage(message, false);
            };
            websocketApi.OnClose += HandleOnClose;

            notificationBuffer = new AccelByteNotificationBuffer(inApi?.SharedMemory?.Logger);

            connectRetryTicker = new AccelByteHeartBeat(retryInterval, SharedMemory?.Logger);

            notificationQueue = new NotificationQueue();
            notificationQueue.RefreshQueue();
            ticker = new AccelByteHeartBeat(tickerInterval);
            ticker.OnHeartbeatTrigger += Tick;

            OverrideWebsocket(webSocket);
        }

        /// <summary>
        /// Override current websocket instance, and set new retry parameters
        /// </summary>
        /// <param name="inWebSocket">IWebsocket instance to use</param>
        /// <param name="inPingDelay">delay for each connection ping</param>
        /// <param name="inBackoffDelay">first backoff delay duration for a retry connection attempt</param>
        /// <param name="inMaxDelay">max delay for each retry connection attempt</param>
        /// <param name="inTotalTimeout"></param>
        public void OverrideWebsocket(IWebSocket inWebSocket
            , int inPingDelay = 4000
            , int inBackoffDelay = 1000
            , int inMaxDelay = 30000
            , int inTotalTimeout = 60000)
        {
            backoffDelay = inBackoffDelay;
            websocketApi.OverrideWebsocket(inWebSocket, inPingDelay, inMaxDelay, inTotalTimeout);
            websocketApi.SetLogger(SharedMemory?.Logger);
            inWebSocket?.SetSharedMemory(SharedMemory);
        }

        /// <summary>
        /// Lobby connection status
        /// </summary>
        public bool IsConnected => websocketApi.IsConnected;

        /// <summary>
        /// Connect to lobby with current logged in user credentials.
        /// The token generator need to be set for connection with entitlement verification.
        /// </summary>
        public void Connect()
        {
            Report.GetFunctionLog(GetType().Name);

            connectRetryTicker.OnHeartbeatTrigger = null;
            ClearRetryConnectDelegate();

            websocketApi.Connect();

            retryAttempts = 0;
            connectRetryTicker.OnHeartbeatTrigger += RetryConnect;
            connectRetryTicker.Start(isImmediateTrigger: false);
        }

        /// <summary>
        /// Change the delay parameters to maintain connection in the lobby before lobby connected.
        /// </summary>
        /// <param name="inTotalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="inBackoffDelay">Initial delay time</param>
        /// <param name="inMaxDelay">Maximum delay time</param>
        public void SetRetryParameters(int inTotalTimeout = 60000
            , int inBackoffDelay = 1000
            , int inMaxDelay = 30000)
        {
            websocketApi.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay);
        }

        /// <summary>
        /// TokenGenerator is used for generate access token when connecting to lobby. 
        /// If token generator is not specified, no token will be used when connecting to lobby.
        /// For entitlement token verification, use EntitlementTokenGenerator class on the parameter.
        /// </summary>
        /// <param name="tokenGenerator"> Token generator for connecting lobby. </param>
        public void SetConnectionTokenGenerator(ITokenGenerator tokenGenerator)
        {
            websocketApi.SetConnectionTokenGenerator(tokenGenerator);
        }

        /// <summary>
        /// Disconnect from Lobby.
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(GetType().Name);

            websocketApi.Disconnect();

            if (session != null)
            {
                session.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }
        }

        /// <summary>
        /// Change user region
        /// </summary>
        /// <param name="region">New region</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ChangeUserRegion(string region, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ChangeUserRegion(region, result =>
            {
                if (result.IsError)
                {
                    callback.TryError(result.Error);
                    return;
                }

                UserRegion = region;
                callback.TryOk();
            });
        }

        /// <summary>
        /// Get list of notifications from user from specified time.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="startTime">Start time to filter notifications from</param>
        /// <param name="endTime">End time to filter notifications to</param>
        /// <param name="offset">Offset of result array value. Default value is 0.</param>
        /// <param name="limit">Offset of result array value. Default value is 25.</param>
        public void GetNotifications(ResultCallback<GetUserNotificationsResponse> callback
            , DateTime startTime = default
            , DateTime endTime = default
            , int offset = 0
            , int limit = 25)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetNotifications(callback, startTime, endTime, offset, limit);
        }

        #region Party

        /// <summary>
        /// Get party information (leader, members, invitation token)
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains PartyInfo via callback when completed.
        /// </param>
        public void GetPartyInfo(ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GetPartyInfo(callback);
        }

        /// <summary>
        /// Create a party and become a party leader for the party. 
        /// The newer function has different struct return callback and it has partyCode.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contain PartyInfo via callback when completed.
        /// </param>
        public void CreateParty(ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.CreateParty(cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Created, session.UserId, cb.Value.partyID);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Create a party and become a party leader for the party.
        /// PartyCode is also provided to the party creator through the callback.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contain PartyCreateResponse via callback when completed.
        /// </param>
        public void CreateParty(ResultCallback<PartyCreateResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.CreateParty(cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Created, session.UserId, cb.Value.partyID);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Leave a party a current logged in user is in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LeaveParty(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.LeaveParty(cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Left, session.UserId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Invite other user by userId. Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void InviteToParty(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.InviteToParty(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Invited, userId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Invite other user by userId with detailed model in response callback.
        /// Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">
        /// Returns a Result that contain PartyInviteResponse via callback when completed
        /// </param>
        public void InviteToPartyDetailedCallback(string userId
            , ResultCallback<PartyInviteResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.InviteToPartyDetailedCallback(userId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Invited, userId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Join a party by invitation from party leader.
        /// </summary>
        /// <param name="partyID">Party ID of the party to join to</param>
        /// <param name="invitationToken">Invitation token to join the party</param>
        /// <param name="callback">
        /// Returns a Result that contains PartyInfo via callback when completed
        /// </param>
        public void JoinParty(string partyID
            , string invitationToken
            , ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.JoinParty(partyID, invitationToken, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Joined, session.UserId, cb.Value.partyID);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Kick a member out of our party. Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void KickPartyMember(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.KickPartyMember(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Kicked, userId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Kick a member out of our party with detailed model in response callback.
        /// Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">
        /// Returns a Result that contains PartyKickResponse via callback when completed.
        /// </param>
        public void KickPartyMemberDetailedCallback(string userId
            , ResultCallback<KickResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.KickPartyMemberDetailedCallback(userId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedPartyV1Event(EventMode.PartyV1Kicked, userId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Reject a party invitation.
        /// </summary>
        /// <param name="partyId">
        /// Party ID of an incoming party invitation that will be declined.
        /// </param>
        /// <param name="invitationToken">
        /// Invitation token of an incoming party invitation that will be declined.
        /// </param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RejectPartyInvitation(string partyId
            , string invitationToken
            , ResultCallback<PartyRejectResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RejectPartyInvitation(partyId, invitationToken, callback);
        }

        /// <summary>
        /// Generate party code for invitation
        /// </summary>
        /// <param name="callback">Return the party code that has been generated if success</param>
        public void GeneratePartyCode(ResultCallback<PartyGenerateCodeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GeneratePartyCode(callback);
        }

        /// <summary>
        /// Get party code
        /// </summary>
        /// <param name="callback">Return the party code that has been generated previously if success</param>
        public void GetPartyCode(ResultCallback<PartyGetCodeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GetPartyCode(callback);
        }

        /// <summary>
        /// Remove party invite code
        /// </summary>
        /// <param name="callback"></param>
        public void DeletePartyCode(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.DeletePartyCode(callback);
        }

        /// <summary>
        /// Join to a party via party code
        /// </summary>
        /// <param name="callback"></param>
        public void JoinPartyViaCode(string invitationPartyCode, ResultCallback<PartyInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.JoinPartyViaCode(invitationPartyCode, callback);
        }

        /// <summary> Promote member to be a party leader.</summary>
        /// <param name="userId">User ID that will be promoted as a party leader.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void PromotePartyLeader(string userId
            , ResultCallback<PartyPromoteLeaderResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.PromotePartyLeader(userId, callback);
        }

        /// <summary>
        /// Set party member size limit
        /// </summary>
        /// <param name="partyId">designated party id</param>
        /// <param name="limit">member limit</param>
        /// <param name="callback">returns a Result via callback when completed</param>
        public void SetPartySizeLimit(string partyId, int limit, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            coroutineRunner.Run(api.SetPartySizeLimit(partyId, limit, callback));
        }

        /// <summary>
        /// Send notification to party member 
        /// </summary>
        /// <param name="topic">Topic The topic of the request. Can use this as ID to know how to marshal the payload</param>
        /// <param name="payload">Payload The Payload of the request. Can be JSON string</param>
        /// <param name="callback">
        /// Returns a Result that contains PartySendNotifResponse via callback when completed.
        /// </param>
        public void SendNotificationToPartyMember(string topic, string payload, ResultCallback<PartySendNotifResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendNotificationToPartyMember(topic, payload, callback);
        }
        #endregion Party

        #region Chat
        /// <summary>
        /// Send chat to other party members
        /// </summary>
        /// <param name="chatMessage">Message to send to party</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPartyChat(string chatMessage
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendPartyChat(chatMessage, callback);
        }

        /// <summary>
        /// Send personal chat to friend
        /// </summary>
        /// <param name="userId">Friend user id</param>
        /// <param name="chatMessage">Message to send to friend</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPersonalChat(string userId
            , string chatMessage
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendPersonalChat(userId, chatMessage, callback);
        }

        /// <summary>
        /// Send Join default global chat channel request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains ChatChannelSlug via callback when completed.
        /// </param>
        public void JoinDefaultChatChannel( ResultCallback<ChatChannelSlug> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.JoinDefaultChatChannel(callback);
        }

        /// <summary>
        /// Send a Chat Message to a Global Chat Channel.
        /// </summary>
        /// <param name="chatMessage">Message to send to the channel</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendChannelChat( string chatMessage
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendChannelChat(chatMessage, callback);
        }
        #endregion Chat

        #region StatusAndPresence
        /// <summary>
        /// Set current user status and activity
        /// </summary>
        /// <param name="status">User status (Online, Available, Busy, Invisible)</param>
        /// <param name="activity">Describe an activity of the user, could be anything.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void SetUserStatus(UserStatus status
            , string activity
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SetUserStatus(status, activity, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(status.ToString(), EventMode.UserStatusUpdated);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Get a list of friends status (availability, activity, last seen at)
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends Status via callback when completed.
        /// </param>
        public void ListFriendsStatus(ResultCallback<FriendsStatus> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ListFriendsStatus(callback);
        }
        #endregion StatusAndPresence

        #region LobbyNotification
        /// <summary>
        ///  Ask lobby to send all pending notification to user. Listen to OnNotification.
        /// </summary> 
        /// <param name="callback">Returns a Result via callback when completed.</param>
        [Obsolete("Lobby 2.4.0 and above dropped support for this function")]
        public void PullAsyncNotifications(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.PullAsyncNotifications(callback);
        }
        #endregion LobbyNotification

        #region Friend
        /// <summary>
        /// Send request friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriend(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RequestFriend(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.FriendRequestSent);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send request friend request using other profile's public id
        /// </summary>
        /// <param name="publicId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriendByPublicId(string publicId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RequestFriendByPublicId(publicId, callback);
        }

        /// <summary>
        /// Add other users as friends automatically, other users don't need to accept it.
        /// This is used for syncing other platform friends to AccelByte friend.
        /// </summary>
        /// <param name="userIds">Targeted user IDs.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void BulkRequestFriend(string[] userIds
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            BulkFriendsRequest otherUserIds = new BulkFriendsRequest { friendIds = userIds };
            coroutineRunner.Run(
                api.BulkFriendRequest(
                    session.UserId,
                    otherUserIds,
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendPredefinedSocialRequestEvent(userIds, EventMode.FriendRequestSent);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Send unfriend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void Unfriend(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.Unfriend(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.FriendUnfriended);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send list of outgoing friends request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void ListOutgoingFriends(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ListOutgoingFriends(callback);
        }
        
        /// <summary>
        /// Send list of outgoing friends request with added timestamp.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains FriendsWithTimestamp via callback when completed.
        /// </param>
        public void ListOutgoingFriendsWithTime(ResultCallback<FriendsWithTimestamp> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ListOutgoingFriendsWithTime(callback);
        }

        /// <summary>
        /// Send cancel friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void CancelFriendRequest(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.CancelFriendRequest(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.FriendRequestCancelled);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send list of incoming friends request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void ListIncomingFriends(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ListIncomingFriends(callback);
        }
        
        /// <summary>
        /// Send list of incoming friends request with added timestamp
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains FriendsWithTimestamp via callback when completed.
        /// </param>
        public void ListIncomingFriendsWithTime(ResultCallback<FriendsWithTimestamp> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ListIncomingFriendsWithTime(callback);
        }

        /// <summary>
        /// Send accept friend request. 
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void AcceptFriend(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.AcceptFriend(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.FriendRequestAccepted);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send reject friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void RejectFriend(string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RejectFriend(userId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.FriendRequestRejected);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send load friends list request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void LoadFriendsList(ResultCallback<Friends> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.LoadFriendsList(callback);
        }

        /// <summary>
        /// Send get friendship status request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">
        /// Returns a Result that contains FriendshipStatus via callback when completed.
        /// </param>
        public void GetFriendshipStatus(string userId
            , ResultCallback<FriendshipStatus> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GetFriendshipStatus(userId, callback);
        }

        /// <summary>
        /// Send request get user presence in bulk.
        /// </summary>
        /// <param name="userIds">requested userIds</param>
        /// <param name="callback">
        /// Returns a Result that contains BulkUserStatusNotif via callback when completed.
        /// </param>
        public void BulkGetUserPresence(string[] userIds
            , ResultCallback<BulkUserStatusNotif> callback
            , bool countOnly = false)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkGetUserPresence(
                    userIds,
                    callback,
                    countOnly));
        }

        /// <summary>
        /// Sync platform specific friend list to AccelByte friend list.
        /// This will automatically add platform specific friend list to AccelByte friend list if the friends already linked
        /// their account to AccelByte service.
        /// </summary>
        /// <param name="request">The request containing details of platform information.</param>
        /// <param name="callback">Returns a Result that contain response details.</param>
        public void SyncThirdPartyFriends(SyncThirdPartyFriendsRequest request
            , ResultCallback<SyncThirdPartyFriendsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.SyncThirdPartyFriends(request, callback));
        }
        #endregion Friend

        #region Matchmaking
        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">
        /// Result of the function with a start matchmaking status code.
        /// </param>
        public void StartMatchmaking(string gameMode
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                "",
                null,
                null,
                null,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                serverName,
                null,
                null,
                null,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , string clientVersion
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                serverName,
                clientVersion,
                null,
                null,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string clientVersion
            , Dictionary<string, int> latencies
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                "",
                clientVersion,
                latencies,
                null,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string clientVersion
            , Dictionary<string, int> latencies
            , Dictionary<string, object> partyAttributes
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                "",
                clientVersion,
                latencies,
                partyAttributes,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">
        /// UserIDs to form a temporary party with (include user who started the matchmaking).
        /// Temporary party will disband when matchmaking finishes.
        /// </param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string clientVersion
            , Dictionary<string, int> latencies
            , Dictionary<string, object> partyAttributes
            , string[] tempPartyUserIds
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                "",
                clientVersion,
                latencies,
                partyAttributes,
                tempPartyUserIds,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">
        /// UserIDs to form a temporary party with (include user who started the matchmaking).
        /// Temporary party will disband when matchmaking finishes.
        /// </param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string clientVersion
            , Dictionary<string, int> latencies
            , Dictionary<string, object> partyAttributes
            , string[] tempPartyUserIds
            , string[] extraAttributes
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                "",
                clientVersion,
                latencies,
                partyAttributes,
                tempPartyUserIds,
                extraAttributes,
                callback);
        }


        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">
        /// Party attributes the matchmaker will do string equality check to
        /// matchmake with other party's party attribute
        /// </param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , string clientVersion
            , Dictionary<string, object> partyAttributes
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                serverName,
                clientVersion,
                null,
                partyAttributes,
                null,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">
        /// UserIDs to form a temporary party with (include user who started the matchmaking).
        /// Temporary party will disband when matchmaking finishes.
        /// </param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , string clientVersion
            , Dictionary<string, object> partyAttributes
            , string[] tempPartyUserIds
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                serverName,
                clientVersion,
                null,
                partyAttributes,
                tempPartyUserIds,
                null,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">
        /// UserIDs to form a temporary party with (include user who started the matchmaking).
        /// Temporary party will disband when matchmaking finishes.</param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule
        /// </param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , string clientVersion
            , Dictionary<string, object> partyAttributes
            , string[] tempPartyUserIds
            , string[] extraAttributes
            , ResultCallback<MatchmakingCode> callback)
        {
            StartMatchmaking(
                gameMode,
                serverName,
                clientVersion,
                null,
                partyAttributes,
                tempPartyUserIds,
                extraAttributes,
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies"></param> Preferred latencies
        /// <param name="partyAttributes">Matchmaker will match party with the same party attributes</param>
        /// <param name="tempPartyUserIds">
        /// UserIDs to form a temporary party with (include user who started the matchmaking).
        /// Temporary party will disband when matchmaking finishes.
        /// </param>
        /// <param name="extraAttributes">Custom attributes defined in game mode's matching/flexing rule</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode
            , string serverName
            , string clientVersion
            , Dictionary<string, int> latencies
            , Dictionary<string, object> partyAttributes
            , string[] tempPartyUserIds
            , string[] extraAttributes
            , ResultCallback<MatchmakingCode> callback)
        {
            MatchmakingOptionalParam param = new MatchmakingOptionalParam
            {
                serverName = serverName,
                clientVersion = clientVersion,
                latencies = latencies,
                partyAttributes = partyAttributes,
                tempPartyUserIds = tempPartyUserIds,
                extraAttributes = extraAttributes,
            };

            StartMatchmaking(gameMode, param, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        public void StartMatchmaking(string gameMode
            , MatchmakingOptionalParam param
            , ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.StartMatchmaking(gameMode, param, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedMatchmakingV1StartedEvent(session.UserId, gameMode, param.serverName, param.clientVersion,
                        param.latencies, param.partyAttributes);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send a message to matchmaking service to indicate the user is ready for match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="callback"></param>
        public void ConfirmReadyForMatch(string matchId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.ConfirmReadyForMatch(matchId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedMatchmakingV1ReadyEvent(session.UserId, matchId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Reject match that was found.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="callback"></param>
        public void RejectMatch(string matchId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RejectMatch(matchId, cb =>
            {
                if (cb != null && !cb.IsError)
                {
                    SendPredefinedMatchmakingV1RejectEvent(session.UserId, matchId);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">
        /// Result of the function with a cancel matchmaking status code.
        /// </param>
        public void CancelMatchmaking(string gameMode
            , ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.CancelMatchmaking(gameMode, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedMatchmakingV1CancelEvent(session.UserId, gameMode);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="isTempParty">Is using temp party when starting matchmaking</param>
        /// <param name="callback">
        /// Result of the function with a cancel matchmaking status code.
        /// </param>
        public void CancelMatchmaking(string gameMode
            , bool isTempParty
            , ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.CancelMatchmaking(gameMode, isTempParty, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedMatchmakingV1CancelEvent(session.UserId, gameMode, isTempParty);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Request Dedicated Custom Server
        /// </summary>
        /// <param name="request">Specification</param>
        /// <param name="callback"></param>
        public void RequestDS(CustomDsCreateRequest request)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RequestDS(request);
        }
        #endregion Matchmaking
        
        #region PartyStorage
        /// <summary>Get party storage by party ID.</summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void GetPartyStorage(string partyId
            , ResultCallback<PartyDataUpdateNotif> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID should not be null.");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetPartyStorage(
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
        /// <param name="payloadModifier">
        /// Function to modify the latest party data with your customized modifier.
        /// </param>
        /// <param name="retryAttempt">
        /// the number of retry to do when there is an error in writing to party storage
        /// (likely due to write conflicts).
        /// </param>
        public void WritePartyStorage(string partyId
            , ResultCallback<PartyDataUpdateNotif> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier
            , int retryAttempt = 1)
        {
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID should not be null.");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            WritePartyStorageRecursive(retryAttempt, partyId, callback, payloadModifier);
        }

        private void WritePartyStorageRecursive(int remainingAttempt
            , string partyId
            , ResultCallback<PartyDataUpdateNotif> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier)
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed,
                    "Exhaust all retry attempt to modify party storage. Please try again."));
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
                    getPartyStorageResult.Value.custom_attribute = payloadModifier(
                        getPartyStorageResult.Value.custom_attribute);

                    var updateRequest = new PartyDataUpdateRequest();
                    updateRequest.custom_attribute = getPartyStorageResult.Value.custom_attribute;
                    updateRequest.updatedAt = getPartyStorageResult.Value.updatedAt;

                    coroutineRunner.Run(
                        api.WritePartyStorage(
                            updateRequest,
                            partyId,
                            callback,
                            () =>
                            {
                                WritePartyStorageRecursive(
                                    remainingAttempt - 1,
                                    partyId,
                                    callback,
                                    payloadModifier);
                            }));
                }
            });
        }
        #endregion PartyStorage

        #region BlockUnblock
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
        public void BlockPlayer(string userId
            , ResultCallback<BlockPlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.BlockPlayer(userId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.UserBlocked);
                }
                HandleCallback(cb, callback);
            });
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
        public void UnblockPlayer(string userId
            , ResultCallback<UnblockPlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.UnblockPlayer(userId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedSocialRequestEvent(userId, EventMode.UserUnblocked);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// get current logged in user's blocked list.
        /// </summary>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void GetListOfBlockedUser(ResultCallback<BlockedList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListOfBlockedUser(
                    session.UserId,
                    callback));
        }


        /// <summary>
        /// Get users that currently blocking the user logged in.
        /// </summary>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void GetListOfBlocker(ResultCallback<BlockerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListOfBlocker(
                    session.UserId,
                    callback));
        }
        #endregion BlockUnblock

        #region ProfanityFilter
        /// <summary>
        /// Set chat profanity filter level
        /// </summary>
        /// <param name="level">level of profanity filter to apply</param>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void SetProfanityFilterLevel( ProfanityFilterLevel level
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SetProfanityFilterLevel(level, callback);
        }
        #endregion ProfanityFilter

        #region SessionAttribute
        /// <summary>
        /// Set user's lobby session attributes.
        /// </summary>
        /// <param name="key">Attribute key</param>
        /// <param name="value">Attribute value</param>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void SetSessionAttribute( string key
            , string value
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            websocketApi.SetSessionAttribute(key, value, callback);
        }

        /// <summary>
        /// Get user's lobby session attributes.
        /// </summary>
        /// <param name="key">Attribute key</param>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void GetSessionAttribute(string key, ResultCallback<GetSessionAttributeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            websocketApi.GetSessionAttribute(key, callback);
        }

        /// <summary>
        /// Get all of the user's lobby session attributes.
        /// </summary>
        /// <param name="callback">Returns a result via callback when complete</param>
        public void GetSessionAttributeAll(ResultCallback<GetSessionAttributeAllResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GetSessionAttributeAll(callback);
        }
        #endregion SessionAttribute

        #region Signaling
        /// <summary>
        /// Send a signaling message to another user.
        /// </summary>
        /// <param name="userId">The recipient's user ID.</param>
        /// <param name="message">Signaling message to be sent.</param>
        public void SendSignalingMessage(string userId
            , string message)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendSignalingMessage(userId, message);
        }
        #endregion Signaling

        #region Token
        /// <summary>
        /// Refresh the access token to be used in lobby.
        /// </summary>
        /// <param name="newAccessToken">new access token to user in lobby</param>
        /// <param name="callback">Returns a result via callback when complete</param>
        private void RefreshToken( string newAccessToken
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RefreshToken(newAccessToken, callback);
        }
        #endregion Token

        private void HandleOnOpen()
        {
            ClearRetryConnectDelegate();

#if DEBUG
            // debug ws connection
            SharedMemory?.Logger?.LogVerbose("[WS] Connection open");
#endif
            Connected?.Invoke();

            if (session != null)
            {
                session.RefreshTokenCallback += LoginSession_RefreshTokenCallback;
            }

            messagingSystem?.SendMessage(AccelByteMessagingTopic.LobbyConnected);
            messagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.NotificationSenderLobby, OnNotificationSenderMessageReceivedHandle);
        }

        private void LoginSession_RefreshTokenCallback(string accessToken)
        {
            RefreshToken(accessToken, result =>
            {
                TokenRefreshed?.Invoke();

                string resultMsg = result.IsError
                    ? $"Error with code {result.Error.Code}, message {result.Error.Message}"
                    : "Success";
                SharedMemory?.Logger?.Log($"Updating access token in lobby {resultMsg}");
            });
        }

        private void HandleOnClose(ushort closecode)
        {
            // debug ws connection
#if DEBUG
            if (Enum.TryParse(closecode.ToString(), out WsCloseCode verboseCode))
            {
                SharedMemory?.Logger?.LogVerbose($"[WS Lobby] Connection close: {closecode} named {verboseCode.ToString()}");
            }
            else
            {
                SharedMemory?.Logger?.LogVerbose($"[WS Lobby] Connection close: {closecode}. Please refers https://demo.accelbyte.io/lobby/v1/messages for more info");
            }
#endif
            var code = (WsCloseCode)closecode;

            if (session != null)
            {
                session.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }

            Disconnected?.Invoke(code);

            messagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.NotificationSenderLobby, OnNotificationSenderMessageReceivedHandle);

            if (reconnectsOnBans)
            {
                reconnectsOnBans = false;
                coroutineRunner.Run(ReconnectOnBan());
            }

            ticker.Stop();
        }

        private IEnumerator ReconnectOnBan()
        {
            Random rand = new Random();
            float randomFloat = (float)(rand.NextDouble());

            yield return new WaitForSeconds((backoffDelay +
                                             ((randomFloat * 0.5f) - 0.5f)) / 1000);

            Connect();
        }

        private void SendBufferedNotifications()
        {
            var bufferedNotifications = notificationBuffer.GetSortedBuffer();
            messagingSystem?.SendMessage(AccelByteMessagingTopic.NotificationBufferSent, bufferedNotifications.ToJsonString());
            notificationBuffer.Clear();

            foreach (var notification in bufferedNotifications)
            {
                string lobbyMessage = null;

                switch (Enum.Parse(typeof(MessageType), notification.Type))
                {
                    case MessageType.connectNotif:
                        lobbyMessage = 
                            AccelByteNotificationSenderUtility.ComposeConnectedNotification(
                                notification.LobbySessionId
                                , notification.LoginType
                                , notification.ReconnectFromCode);
                        break;
                    case MessageType.messageNotif:
                        lobbyMessage =
                            AccelByteNotificationSenderUtility.ComposeMMv2Notification(
                                notification.Topic
                                , notification.Payload);
                        break;
                    case MessageType.messageSessionNotif:
                        lobbyMessage =
                            AccelByteNotificationSenderUtility.ComposeSessionNotification(
                                notification.Topic
                                , notification.Payload);
                        break;
                }

                if (string.IsNullOrEmpty(lobbyMessage))
                {
                    continue;
                }

                HandleOnMessage(lobbyMessage, true);
            }
        }

        private bool TryBufferNotification(string parsedJsonString)
        {
            if (notificationBuffer == null)
            {
                return false;
            }

            UserNotification receivedNotification = null;
            try
            {
                receivedNotification = parsedJsonString.ToObject<UserNotification>();
            }
            catch (Exception)
            {
                return false;
            }

            if (receivedNotification == null)
            {
                return false;
            }

            if (notificationBuffer.IsBuffering)
            {
                return notificationBuffer.TryAddBuffer(receivedNotification);
            }

            if (notificationBuffer.TryAddBuffer(receivedNotification))
            {
                SharedMemory?.Logger?.LogWarning($"Missing notification(s) detected, received:\n" +
                    $"{parsedJsonString}");

                GetNotifications(OnGetMissingNotification);

                return true;
            }

            return false;
        }

        private void OnGetMissingNotification(Result<GetUserNotificationsResponse> missingNotifications)
        {
            if (!missingNotifications.IsError)
            {
                notificationBuffer.TryAddMissingNotifications(missingNotifications.Value.Notifications);
            }

            SendBufferedNotifications();
        }

        private void HandleOnMessage(string message, bool isSkipConditioner = false)
        {
            long messageId;
            MessageType messageType;
            websocketSerializer.ReadHeader(message, out messageType, out messageId);

            string lobbyNotification = messageType.ToString();
            if (!isSkipConditioner
                && SharedMemory.NetworkConditioner != null 
                && SharedMemory.NetworkConditioner.CalculateFailRate(lobbyNotification))
            {
                SharedMemory?.Logger?.Log($"[AccelByteNetworkConditioner] Dropped lobby notification {lobbyNotification}.");
                return;
            }

            notificationQueue.Enqueue(message);
            if (!ticker.IsHeartBeatJobRunning)
            {
                ticker.Start();
            }
        }

        private void HandleBanNotification()
        {
            reconnectsOnBans = true;
        }

        private void HandleUnbanNotification()
        {
            reconnectsOnBans = false;

            if (session != null)
            {
                session.RefreshTokenCallback -= LoginSession_RefreshTokenCallback;
            }

            api.OnBanNotificationReceived((newAccessToken) => 
            { 
                UpdateAuthToken(newAccessToken);
                if (session != null)
                {
                    session.RefreshTokenCallback += LoginSession_RefreshTokenCallback;
                }
            });
        }

        private void UpdateAuthToken(string newSessionAuthToken)
        {
            Report.GetFunctionLog(GetType().Name);
            session.AuthorizationToken = newSessionAuthToken;
        }

        private void HandleMultiplayerV2Notification(string message, MultiplayerV2NotifType notificationType)
        {
            // Report.GetProtobufNotification(message);

            Notification notification;
            ErrorCode errorCode = websocketSerializer.ReadPayload(message, out notification);

            if (errorCode != ErrorCode.None)
            {
                SharedMemory?.Logger?.LogWarning($"Error {errorCode}: Failed to read payload of MultiplayerV2 notification");
                return;
            }

            var notificationPayload = notification.payload.Trim();

            // Pad the payload with "=" if length is not divisible by 4
            if (notificationPayload.Length % 4 != 0)
            {
                notificationPayload = notification.payload.PadRight(
                    notificationPayload.Length + 4 - notificationPayload.Length % 4, '=');
            }

            byte[] payloadBytes;
            try
            {
                payloadBytes = Convert.FromBase64String(notificationPayload);
            }
            catch (Exception)
            {
                SharedMemory?.Logger?.LogWarning(
                    $"Error {ErrorCode.ErrorFromException}: Failed to decode MultiplayerV2 notification from Base64: {notificationPayload}");
                return;
            }

            // var notificationEventEnvelope = Accelbyte.Proto.Session.NotificationEventEnvelope.Parser.ParseFrom(payloadString);
            var jsonString = Encoding.UTF8.GetString(payloadBytes);

            switch (notificationType)
            {
                case MultiplayerV2NotifType.OnPartyInvited:
                    var partyNotificationUserInvited =
                        JsonConvert.DeserializeObject<SessionV2PartyInvitationNotification>(jsonString);
                    websocketApi.DispatchNotification(partyNotificationUserInvited,
                        SessionV2InvitedUserToParty);
                    break;
                case MultiplayerV2NotifType.OnPartyJoined:
                    var partyNotificationUserJoined =
                        JsonConvert.DeserializeObject<SessionV2PartyJoinedNotification>(jsonString);
                    websocketApi.DispatchNotification(partyNotificationUserJoined,
                        SessionV2UserJoinedParty);
                    messagingSystem?.SendMessage(AccelByteMessagingTopic.OnJoinedPartyNotification, jsonString);
                    break;
                case MultiplayerV2NotifType.OnPartyMembersChanged:
                    var partyNotificationMembersChanged =
                        JsonConvert.DeserializeObject<SessionV2PartyMembersChangedNotification>(jsonString);
                    websocketApi.DispatchNotification(partyNotificationMembersChanged, SessionV2PartyMemberChanged);
                    break;
                case MultiplayerV2NotifType.OnPartyRejected:
                    var partyNotificationUserReject =
                        JsonConvert.DeserializeObject<SessionV2PartyInvitationRejectedNotification>(jsonString);
                    websocketApi.DispatchNotification(partyNotificationUserReject,
                        SessionV2UserRejectedPartyInvitation);
                    break;
                case MultiplayerV2NotifType.OnPartyKicked:
                    var partyNotificationUserKicked =
                        JsonConvert.DeserializeObject<SessionV2PartyUserKickedNotification>(jsonString);
                    websocketApi.DispatchNotification(partyNotificationUserKicked,
                        SessionV2UserKickedFromParty);
                    break;
                case MultiplayerV2NotifType.OnPartyUpdated:
                    var partySession =
                        JsonConvert.DeserializeObject<SessionV2PartySessionUpdatedNotification>(jsonString);
                    websocketApi.DispatchNotification(partySession,
                        SessionV2PartyUpdated);
                    break;
                case MultiplayerV2NotifType.OnPartyCancelled:
                    var cancelledPartySession = 
                        JsonConvert.DeserializeObject<SessionV2PartyInvitationCancelledNotification>(jsonString);
                    websocketApi.DispatchNotification(cancelledPartySession,
                        SessionV2PartyInvitationCancelled);
                    break;
                case MultiplayerV2NotifType.OnSessionInvited:
                    var gameSessionNotificationUserInvited =
                        JsonConvert.DeserializeObject<SessionV2GameInvitationNotification>(jsonString);
                    if (processedMatchmakingMessages.Add($"sessionInvited_{gameSessionNotificationUserInvited.sessionId}"))
                    {
                        websocketApi.DispatchNotification(gameSessionNotificationUserInvited,
                            SessionV2InvitedUserToGameSession);
                    }
                    break;
                case MultiplayerV2NotifType.OnSessionJoined:
                    var gameSessionNotificationUserJoined =
                        JsonConvert.DeserializeObject<SessionV2GameJoinedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSessionNotificationUserJoined,
                        SessionV2UserJoinedGameSession);
                    break;
                case MultiplayerV2NotifType.OnSessionKicked:
                    var gameSessionNotificationUserKicked =
                        JsonConvert.DeserializeObject<SessionV2GameUserKickedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSessionNotificationUserKicked,
                        SessionV2UserKickedFromGameSession);
                    break;
                case MultiplayerV2NotifType.OnSessionRejected:
                    var gameSessionNotificationUserReject =
                        JsonConvert.DeserializeObject<SessionV2GameInvitationRejectedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSessionNotificationUserReject,
                        SessionV2UserRejectedGameSessionInvitation);
                    break;
                case MultiplayerV2NotifType.OnSessionMembersChanged:
                    var gameSessionNotificationMembersChanged =
                        JsonConvert.DeserializeObject<SessionV2GameMembersChangedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSessionNotificationMembersChanged,
                        SessionV2GameSessionMemberChanged);
                    break;
                case MultiplayerV2NotifType.OnGameSessionUpdated:
                    var gameSession =
                        JsonConvert.DeserializeObject<SessionV2GameSessionUpdatedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSession,
                        SessionV2GameSessionUpdated);
                    break;
                case MultiplayerV2NotifType.OnSessionEnded:
                    var gameSessionEndedNotification =
                        JsonConvert.DeserializeObject<SessionV2GameSessionEndedNotification>(jsonString);
                    websocketApi.DispatchNotification(gameSessionEndedNotification,
                        SessionV2GameSessionEnded);
                    break;
                case MultiplayerV2NotifType.OnDSStatusChanged:
                    var dSStatusChangedNotification =
                        JsonConvert.DeserializeObject<SessionV2DsStatusUpdatedNotification>(jsonString);
                    if (processedMatchmakingMessages.Add($"dsStatusChanged_" +
                        $"{dSStatusChangedNotification.sessionId}_" +
                        $"{dSStatusChangedNotification.session.dsInformation.StatusV2}"))
                    {
                        websocketApi.DispatchNotification(dSStatusChangedNotification,
                            SessionV2DsStatusChanged);
                    }
                    break;
                case MultiplayerV2NotifType.OnMatchFound:
                    var matchFoundNotification =
                        JsonConvert.DeserializeObject<MatchmakingV2MatchFoundNotification>(jsonString);
                    if (processedMatchmakingMessages.Add($"matchFound_{matchFoundNotification.id}"))
                    {
                        websocketApi.DispatchNotification(matchFoundNotification,
                            MatchmakingV2MatchFound);
                    }
                    break;
                case MultiplayerV2NotifType.OnMatchmakingStarted:
                    var matchmakingStartedNotification =
                        JsonConvert.DeserializeObject<MatchmakingV2MatchmakingStartedNotification>(jsonString);
                    OnMatchmakingStartedHandle(matchmakingStartedNotification.ticketId);
                    websocketApi.DispatchNotification(matchmakingStartedNotification,
                        MatchmakingV2MatchmakingStarted);
                    break;
                case MultiplayerV2NotifType.OnMatchmakingTicketCanceled:
                    var matchmakingCanceledNotification =
                        JsonConvert.DeserializeObject<MatchmakingV2MatchmakingCanceledNotification>(jsonString);
                    websocketApi.DispatchNotification(matchmakingCanceledNotification,
                        MatchmakingV2MatchmakingCanceled);
                    break;
                case MultiplayerV2NotifType.OnMatchmakingTicketExpired:
                    var ticketExpiredNotification =
                        JsonConvert.DeserializeObject<MatchmakingV2TicketExpiredNotification>(jsonString);
                    if (processedMatchmakingMessages.Add($"ticketExpired_{ticketExpiredNotification.ticketId}"))
                    {
                        websocketApi.DispatchNotification(ticketExpiredNotification,
                            MatchmakingV2TicketExpired);
                    }
                    break;
                case MultiplayerV2NotifType.OnSessionStorageChanged:
                    var sessionStorageChangedNotification =
                        JsonConvert.DeserializeObject<SessionStorageChangedNotification>(jsonString);
                    websocketApi.DispatchNotification(sessionStorageChangedNotification, SessionV2StorageChanged);
                    break;
                case MultiplayerV2NotifType.OnPartyCreated:
                    var partyCreatedNotification = 
                        JsonConvert.DeserializeObject<PartyCreatedNotification>(jsonString);
                    websocketApi.DispatchNotification(partyCreatedNotification,
                        PartyCreatedNotification);
                    messagingSystem?.SendMessage(AccelByteMessagingTopic.OnCreatedPartyNotification, jsonString);
                    break;
                default:
                    SharedMemory?.Logger?.LogWarning($"MultiplayerV2 notification type {notificationType} not supported");
                    return;
            }
        }

        private void Tick()
        {
            if (notificationQueue.IsLocked || notificationQueue.IsEmpty())
            {
                return;
            }

            string message;
            if (!notificationQueue.Dequeue(out message))
            {
                return;
            }

            long messageId;
            MessageType messageType;
            ErrorCode errorCode = websocketSerializer.ReadHeader(message, out messageType, out messageId);

            websocketSerializer.ReadPayload(message, out Notification userNotif);

            if (TryBufferNotification(userNotif.ToJsonString()))
            {
                return;
            }

            notificationQueue.LockQueue();

            switch (messageType)
            {
                case MessageType.messageSessionNotif:
                    websocketSerializer.ReadPayload(message, out Notification sessionNotification);
                    if (!Enum.TryParse<MultiplayerV2NotifType>(sessionNotification.topic, true,
                            out MultiplayerV2NotifType sessionV2NotificationType))
                    {
                        SharedMemory?.Logger?.LogWarning(
                            $"SessionV2 notification topic not recognized: {sessionNotification.topic}");
                        break;
                    }
                    HandleMultiplayerV2Notification(message, sessionV2NotificationType);
                    break;
                case MessageType.partyGetInvitedNotif:
                    websocketApi.HandleNotification(message, InvitedToParty);
                    break;
                case MessageType.partyJoinNotif:
                    websocketApi.HandleNotification(message, JoinedParty);
                    break;
                case MessageType.partyKickNotif:
                    websocketApi.HandleNotification(message, KickedFromParty);
                    break;
                case MessageType.partyLeaveNotif:
                    websocketApi.HandleNotification(message, LeaveFromParty);
                    break;
                case MessageType.personalChatNotif:
                    websocketApi.HandleNotification(message, PersonalChatReceived);
                    break;
                case MessageType.partyChatNotif:
                    websocketApi.HandleNotification(message, PartyChatReceived);
                    break;
                case MessageType.messageNotif:
                    websocketSerializer.ReadPayload(message, out Notification notification);
                    if (Enum.TryParse<MultiplayerV2NotifType>(notification.topic, true,
                            out MultiplayerV2NotifType matchmakingV2NotificationType))
                    {
                        HandleMultiplayerV2Notification(message, matchmakingV2NotificationType);
                    }
                    else
                    {
                        websocketApi.HandleNotification(message, OnNotification);
                    }
                    break;
                case MessageType.userStatusNotif:
                    websocketApi.HandleUserStatusNotif(message, FriendsStatusChanged);
                    break;
                case MessageType.dsNotif:
                    websocketApi.HandleNotification(message, DSUpdated);
                    break;
                case MessageType.acceptFriendsNotif:
                    websocketApi.HandleNotification(message, FriendRequestAccepted);
                    break;
                case MessageType.requestFriendsNotif:
                    websocketApi.HandleNotification(message, OnIncomingFriendRequest);
                    break;
                case MessageType.unfriendNotif:
                    websocketApi.HandleNotification(message, OnUnfriend);
                    break;
                case MessageType.cancelFriendsNotif:
                    websocketApi.HandleNotification(message, FriendRequestCanceled);
                    break;
                case MessageType.rejectFriendsNotif:
                    websocketApi.HandleNotification(message, FriendRequestRejected);
                    break;
                case MessageType.channelChatNotif:
                    websocketApi.HandleNotification(message, ChannelChatReceived);
                    break;
                case MessageType.connectNotif:
                    websocketSerializer.ReadPayload(message, out LobbySessionId lobbySessionId);
                    websocketApi.AddReconnectCustomHeader(AccelByteWebSocket.LobbySessionIdHeaderName, lobbySessionId.lobbySessionID);
                    break;
                case MessageType.disconnectNotif:
                    websocketApi.HandleNotification(message, Disconnecting);
                    break;
                case MessageType.partyDataUpdateNotif:
                    websocketApi.HandleNotification(message, PartyDataUpdateNotif);
                    break;
                case MessageType.partyNotif:
                    websocketApi.HandleNotification(message, PartyNotif);
                    break;
                case MessageType.partyRejectNotif:
                    websocketApi.HandleNotification(message, RejectedPartyInvitation);
                    break;
                case MessageType.blockPlayerNotif:
                    websocketApi.HandleNotification(message, PlayerBlockedNotif);
                    break;
                case MessageType.unblockPlayerNotif:
                    websocketApi.HandleNotification(message, PlayerUnblockedNotif);
                    break;
                case MessageType.userBannedNotification:
                    websocketApi.HandleNotification(message, UserBannedNotification);
                    HandleBanNotification();
                    break;
                case MessageType.userUnbannedNotification:
                    websocketApi.HandleNotification(message, UserUnbannedNotification);
                    HandleUnbanNotification();
                    break;
                case MessageType.signalingP2PNotif:
                    websocketApi.HandleNotification(message, this.SignalingP2PNotification);
                    break;
                case MessageType.errorNotif:
                    websocketSerializer.ReadPayload(message, out ErrorNotif notif);
                    if (string.IsNullOrEmpty(notif.requestType))
                    {
                        if (ErrorNotification != null)
                        {
                            ErrorNotification(Result.CreateError((ErrorCode)notif.code, notif.message));
                        }
                    }
                    else
                    {
                        websocketApi.HandleResponse(long.Parse(notif.id), message, (ErrorCode)notif.code);
                    }
                    break;
                default:
                    websocketApi.HandleResponse(messageId, message, errorCode);
                    break;
            }

            notificationQueue.UnlockQueue();
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            messagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.QosRegionLatenciesUpdated, OnReceivedQosLatenciesUpdatedHandle);
            messagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.MatchmakingStarted, OnMatchmakingStartedHandle);
            messagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.NotificationSenderLobby, OnNotificationSenderMessageReceivedHandle);
            
            base.SetSharedMemory(newSharedMemory);

            messagingSystem = newSharedMemory.MessagingSystem;
            messagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.QosRegionLatenciesUpdated, OnReceivedQosLatenciesUpdatedHandle);
            messagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.MatchmakingStarted, OnMatchmakingStartedHandle);
            messagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.NotificationSenderLobby, OnNotificationSenderMessageReceivedHandle);
            notificationBuffer?.SetLogger(SharedMemory?.Logger);
            
            websocketApi?.SetLogger(SharedMemory?.Logger);
            websocketApi?.WebSocket?.WebSocketImp?.SetSharedMemory(newSharedMemory);
        }

        private void RetryConnect()
        {
            if (websocketApi.WebSocket.IsConnected)
            {
                ClearRetryConnectDelegate();
                return;
            }

            if (websocketApi.WebSocket.IsConnecting)
            {
                return;
            }

            if (retryAttempts >= maxRetryAttempts)
            {
                SharedMemory?.Logger?.LogWarning("Failed to connect to lobby websocket service");
                ClearRetryConnectDelegate();
                retryAttempts = 0;
                return;
            }

            retryAttempts++;
            SharedMemory?.Logger?.LogWarning($"Lobby retrying to connect ({retryAttempts}/{maxRetryAttempts})");
            websocketApi.Connect();
        }

        private void ClearRetryConnectDelegate()
        {
            connectRetryTicker.OnHeartbeatTrigger = null;
            connectRetryTicker.Stop();
        }

        private void OnReceivedQosLatenciesUpdatedHandle(string payload)
        {
            if (!IsConnected || string.IsNullOrEmpty(payload))
            {
                return;
            }

            ChangeUserRegion(payload, null);
        }

        private void OnMatchmakingStartedHandle(string payload)
        {
            processedMatchmakingMessages.Clear();
        }

        private void OnNotificationSenderMessageReceivedHandle(string payload)
        {
            if (!IsConnected || string.IsNullOrEmpty(payload))
            {
                return;
            }

            HandleOnMessage(payload, true);
        }

        #region PredefinedEvents

        bool isAnalyticsConnected = false;

        private void ConnectPredefinedAnalyticsToEvents()
        {
            if (isAnalyticsConnected)
            {
                return;
            }

            Connected += SendPredefinedConnectedEvent;
            Disconnected += SendPredefinedEvent;
            MatchmakingV2MatchmakingStarted += SendPredefinedEvent;

            isAnalyticsConnected = true;
        }

        private enum EventMode
        {
            LobbyConnected,
            LobbyDisconnected,
            FriendRequestSent,
            FriendRequestCancelled,
            FriendRequestAccepted,
            FriendRequestRejected,
            FriendUnfriended,
            UserBlocked,
            UserUnblocked,
            UserStatusUpdated,
            PartyV1Created,
            PartyV1Invited,
            PartyV1Joined,
            PartyV1Left,
            PartyV1Kicked
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T result)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (typeof(T))
            {
                case Type disconnectType when disconnectType == typeof(WsCloseCode):
                    Enum statusCodeEnum = result as Enum;
                    WsCloseCode statusCode = (WsCloseCode)statusCodeEnum;
                    payload = new PredefinedLobbyDisconnectedPayload(localUserId, statusCode);
                    break;

                case Type partyInfoType when partyInfoType == typeof(PartyInfo):
                    var partyInfo = result as PartyInfo;
                    payload = new PredefinedPartyV1CreatedPayload(localUserId, partyInfo.partyID);
                    break;

                case Type partyCreateResponseType when partyCreateResponseType == typeof(PartyCreateResponse):
                    var partyCreateResponse = result as PartyCreateResponse;
                    payload = new PredefinedPartyV1CreatedPayload(localUserId, partyCreateResponse.partyID);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(Result<T> result)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (typeof(T))
            {
                case Type matchmakingStarted when matchmakingStarted == typeof(MatchmakingV2MatchmakingStartedNotification):
                    var matchmakingStartedValue = result.Value as MatchmakingV2MatchmakingStartedNotification;
                    payload = new PredefinedMatchmakingStartedPayload(localUserId
                        , matchmakingStartedValue.matchPool
                        , matchmakingStartedValue.partyId
                        , matchmakingStartedValue.ticketId);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreatePartyV1Payload(string userId, string partyId, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (eventMode)
            {
                case EventMode.PartyV1Created:
                    payload = new PredefinedPartyV1CreatedPayload(userId, partyId);
                    break;

                case EventMode.PartyV1Invited:
                    payload = new PredefinedPartyV1InvitedPayload(userId, partyId);
                    break;

                case EventMode.PartyV1Joined:
                    payload = new PredefinedPartyV1JoinedPayload(userId, partyId);
                    break;

                case EventMode.PartyV1Kicked:
                    payload = new PredefinedPartyV1KickedPayload(userId, partyId);
                    break;

                case EventMode.PartyV1Left:
                    payload = new PredefinedPartyV1LeftPayload(userId, partyId);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreateSocialRequestPayload(string targetId, EventMode eventMode)
        {
            string localUserId = session.UserId;
            IAccelByteTelemetryPayload payload = null;

            switch (eventMode)
            {
                case EventMode.FriendRequestSent:
                    payload = new PredefinedFriendRequestSentPayload(localUserId, targetId);
                    break;

                case EventMode.FriendRequestCancelled:
                    payload = new PredefinedFriendRequestCancelledPayload(localUserId, targetId);
                    break;

                case EventMode.FriendRequestAccepted:
                    payload = new PredefinedFriendRequestAcceptedPayload(targetId, localUserId);
                    break;

                case EventMode.FriendRequestRejected:
                    payload = new PredefinedFriendRequestRejectedPayload(targetId, localUserId);
                    break;

                case EventMode.FriendUnfriended:
                    payload = new PredefinedFriendUnfriendedPayload(localUserId, targetId);
                    break;

                case EventMode.UserBlocked:
                    payload = new PredefinedUserBlockedPayload(localUserId, targetId);
                    break;

                case EventMode.UserUnblocked:
                    payload = new PredefinedUserUnblockedPayload(localUserId, targetId);
                    break;

                case EventMode.UserStatusUpdated:
                    payload = new PredefinedUserPresenceStatusUpdatedPayload(localUserId, targetId);
                    break;
            }

            return payload;
        }

        internal void SendPredefinedConnectedEvent()
        {
            string localUserId = session.UserId;
            IAccelByteTelemetryPayload payload = new PredefinedLobbyConnectedPayload(localUserId);

            SendPredefinedEvent(payload);
        }

        internal void SendPredefinedEvent<T>(T result)
        {   
            IAccelByteTelemetryPayload payload = CreatePayload(result);

            SendPredefinedEvent(payload);
        }

        internal void SendPredefinedEvent<T>(Result<T> result)
        {
            if (result.IsError)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null || payload == null)
            {
                return;
            }

            var predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
        }

        private void SendPredefinedMatchmakingV1StartedEvent(string userId, string gameMode, string serverName,
            string clientVersion, Dictionary<string, int> latencies, Dictionary<string, object> partyAttribute)
        {
            IAccelByteTelemetryPayload payload = new PredefinedMatchmakingV1StartedPayload(userId, gameMode, serverName,
                clientVersion, latencies, partyAttribute);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedMatchmakingV1ReadyEvent(string userId, string matchId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedMatchmakingV1ReadyConsentPayload(userId, matchId);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedMatchmakingV1RejectEvent(string userId, string matchId)
        {
            IAccelByteTelemetryPayload payload = new PredefinedMatchmakingV1RejectMatchPayload(userId, matchId);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedMatchmakingV1CancelEvent(string userId, string gameMode, bool isTempParty = false)
        {
            IAccelByteTelemetryPayload payload = new PredefinedMatchmakingV1CanceledPayload(userId, gameMode, isTempParty);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedPartyV1Event(EventMode eventMode, string userId, string partyId = null)
        {
            IAccelByteTelemetryPayload payload = CreatePartyV1Payload(userId, partyId, eventMode);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedSocialRequestEvent(string targetId, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = CreateSocialRequestPayload(targetId, eventMode);

            SendPredefinedEvent(payload);
        }

        private void SendPredefinedSocialRequestEvent(string[] targetIds, EventMode eventMode)
        {
            foreach (string targetId in targetIds)
            {
                IAccelByteTelemetryPayload payload = CreateSocialRequestPayload(targetId, eventMode);

                SendPredefinedEvent(payload);
            }
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            {
                if (result.IsError)
                {
                    callback.TryError(result.Error);
                    return;
                }

                callback.Try(result);
            }
        }

        #endregion
    }
}
