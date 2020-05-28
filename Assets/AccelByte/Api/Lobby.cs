// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
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
        public event Action Connected;

        /// <summary>
        /// Raised when lobby got message from server that it will disconnect
        /// </summary>
        public event ResultCallback<DisconnectNotif> Disconnecting;

        /// <summary>
        /// Raised when lobby is disconnected
        /// </summary>
        public event Action Disconnected;

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
        /// Raised when personal chat message received.
        /// </summary>
        public event ResultCallback<ChatMesssage> PersonalChatReceived;

        /// <summary>
        /// Raised when party chat message received.
        /// </summary>
        public event ResultCallback<ChatMesssage> PartyChatReceived;

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

        private readonly int pingDelay;
        private readonly int backoffDelay;
        private readonly int maxDelay;
        private readonly int totalTimeout;

        private readonly Dictionary<long, Action<ErrorCode, string>> responseCallbacks =
            new Dictionary<long, Action<ErrorCode, string>>();
        
        private readonly string websocketUrl;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        private readonly object syncToken = new object();
        private readonly IWebSocket webSocket;
        private bool reconnectsOnClose;
        private long id;
        private Coroutine maintainConnectionCoroutine;

        public event EventHandler OnRetryAttemptFailed;

        internal Lobby(string websocketUrl, IWebSocket webSocket, ISession session, CoroutineRunner coroutineRunner,
            int pingDelay = 4000, int backoffDelay = 1000, int maxDelay = 30000, int totalTimeout = 60000)
        {
            Assert.IsNotNull(webSocket);
            Assert.IsNotNull(coroutineRunner);

            this.websocketUrl = websocketUrl;
            this.webSocket = webSocket;
            this.session = session;
            this.coroutineRunner = coroutineRunner;
            this.pingDelay = pingDelay;
            this.backoffDelay = backoffDelay;
            this.maxDelay = maxDelay;
            this.totalTimeout = totalTimeout;
            this.reconnectsOnClose = false;

            this.webSocket.OnOpen += HandleOnOpen;
            this.webSocket.OnMessage += HandleOnMessage;
            this.webSocket.OnClose += HandleOnClose;
        }

        /// <summary>
        /// Lobby connection status
        /// </summary>
        public bool IsConnected { get { return this.webSocket.ReadyState == WsState.Open; } }

        /// <summary>
        /// Connect to lobby with current logged in user credentials.
        /// </summary>
        public void Connect()
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                throw new Exception("Cannot connect to websocket because user is not logged in.");
            }

            this.webSocket.Connect(this.websocketUrl, this.session.AuthorizationToken);
            StartMaintainConnection();
        }

        private void StartMaintainConnection()
        {
            this.reconnectsOnClose = true;
            this.maintainConnectionCoroutine = this.coroutineRunner.Run(
                MaintainConnection(this.backoffDelay, this.maxDelay, this.totalTimeout));
        }

        private void StopMaintainConnection()
        {
            this.reconnectsOnClose = false;

            if (this.maintainConnectionCoroutine == null) return;

            this.coroutineRunner.Stop(this.maintainConnectionCoroutine);
            this.maintainConnectionCoroutine = null;
        }

        /// <summary>
        /// Retrying connection with exponential backoff if disconnected, ping if connected
        /// </summary>
        /// <param name="backoffDelay">Initial delay time</param>
        /// <param name="maxDelay">Maximum delay time</param>
        /// <param name="totalTimeout">Time limit until stop to re-attempt</param>
        /// <returns></returns>
        private IEnumerator MaintainConnection(int backoffDelay, int maxDelay, int totalTimeout)
        {
            while (true)
            {
                switch (this.webSocket.ReadyState)
                {
                case WsState.Open:
                    this.webSocket.Ping();

                    yield return new WaitForSeconds(this.pingDelay / 1000f);

                    break;
                case WsState.Connecting:
                    while (this.webSocket.ReadyState == WsState.Connecting)
                    {
                        yield return new WaitForSeconds(1f);
                    }

                    break;
                case WsState.Closing:
                    while (this.webSocket.ReadyState == WsState.Closing)
                    {
                        yield return new WaitForSeconds(1f);
                    }

                    break;
                case WsState.Closed:
                    System.Random rand = new System.Random();
                    int nextDelay = backoffDelay;
                    var firstClosedTime = DateTime.Now;

                    while (this.reconnectsOnClose &&
                        this.webSocket.ReadyState == WsState.Closed &&
                        DateTime.Now - firstClosedTime < TimeSpan.FromSeconds(totalTimeout))
                    {
                        this.webSocket.Connect(this.websocketUrl, this.session.AuthorizationToken);
                        float randomizedDelay = (float) (nextDelay + ((rand.NextDouble() * 0.5) - 0.5));

                        yield return new WaitForSeconds(randomizedDelay / 1000f);

                        nextDelay *= 2;

                        if (nextDelay > maxDelay)
                        {
                            nextDelay = maxDelay;
                        }
                    }

                    if (this.webSocket.ReadyState == WsState.Closed)
                    {
                        RaiseOnRetryAttemptFailed();

                        yield break;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Disconnect from Lobby.
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(this.GetType().Name);
            StopMaintainConnection();

            if (this.webSocket.ReadyState == WsState.Open || this.webSocket.ReadyState == WsState.Connecting)
            {
                this.webSocket.Close();
            }
        }

        // Invoker for OnRetryAttemptFailed
        protected virtual void RaiseOnRetryAttemptFailed()
        {
            this.OnRetryAttemptFailed?.Invoke(this, EventArgs.Empty);
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
                new SetUserStatusRequest {availability = (uint) status, activity = activity},
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
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.startMatchmakingRequest,
                new StartMatchmakingRequest {gameMode = gameMode},
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.startMatchmakingRequest,
                new StartMatchmakingRequest {gameMode = gameMode, serverName = serverName},
                callback);
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
            Report.GetFunctionLog(this.GetType().Name);
            SendRequest(
                MessageType.startMatchmakingRequest,
                new StartMatchmakingRequest
                {
                    gameMode = gameMode, serverName = serverName, clientVersion = clientVersion
                },
                callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="serverName">Server name to do match in Local DS</param>
        /// <param name="clientVersion">Game client version to ensure match with the same version</param>
        /// <param name="latencies">Server latencies based on regions</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, string serverName, string clientVersion,
            Dictionary<string, int> latencies, ResultCallback<MatchmakingCode> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            string strLatencies = "{" +
                string.Join(",", latencies.Select(pair => $@"""{pair.Key}"":{pair.Value}").ToArray()) +
                "}";

            SendRequest(
                MessageType.startMatchmakingRequest,
                new StartMatchmakingRequest
                {
                    gameMode = gameMode,
                    serverName = serverName,
                    clientVersion = clientVersion,
                    latencies = strLatencies
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

                this.coroutineRunner.Run(() => callback.Try(result));
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

                this.coroutineRunner.Run(() => callback.Try(result));
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

                this.coroutineRunner.Run(() => callback.Try(result));
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

                this.coroutineRunner.Run(() => callback.Try(result));
            };

            this.webSocket.Send(writer.ToString());
        }

        private void HandleOnOpen()
        {
            this.coroutineRunner.Run(
                () =>
                {
                    Action handler = this.Connected;

                    if (handler != null)
                    {
                        handler();
                    }
                });
        }

        private void HandleOnClose(ushort closecode)
        {
            this.coroutineRunner.Run(
                () =>
                {
                    StopMaintainConnection();

                    Action handler = this.Disconnected;

                    if (handler != null)
                    {
                        handler();
                    }

                    ;
                });
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
                Lobby.HandleNotification(message, this.InvitedToParty);

                break;
            case MessageType.partyJoinNotif:
                Lobby.HandleNotification(message, this.JoinedParty);

                break;
            case MessageType.partyKickNotif:
                Lobby.HandleNotification(message, this.KickedFromParty);

                break;
            case MessageType.partyLeaveNotif:
                Lobby.HandleNotification(message, this.LeaveFromParty);

                break;
            case MessageType.personalChatNotif:
                Lobby.HandleNotification(message, this.PersonalChatReceived);

                break;
            case MessageType.partyChatNotif:
                Lobby.HandleNotification(message, this.PartyChatReceived);

                break;
            case MessageType.messageNotif:
                Lobby.HandleNotification(message, this.OnNotification);

                break;
            case MessageType.userStatusNotif:
                Lobby.HandleNotification(message, this.FriendsStatusChanged);

                break;
            case MessageType.matchmakingNotif:
                Lobby.HandleNotification(message, this.MatchmakingCompleted);

                break;
            case MessageType.dsNotif:
                Lobby.HandleNotification(message, this.DSUpdated);

                break;
            case MessageType.acceptFriendsNotif:
                Lobby.HandleNotification(message, this.FriendRequestAccepted);

                break;
            case MessageType.requestFriendsNotif:
                Lobby.HandleNotification(message, this.OnIncomingFriendRequest);

                break;
            case MessageType.setReadyConsentNotif:
                Lobby.HandleNotification(message, this.ReadyForMatchConfirmed);

                break;
            case MessageType.rematchmakingNotif:
                Lobby.HandleNotification(message, this.RematchmakingNotif);

                break;
            case MessageType.connectNotif: break;
            case MessageType.disconnectNotif:
                Lobby.HandleNotification(message, this.Disconnecting);

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
        }

        private static void HandleNotification<T>(string message, ResultCallback<T> handler) where T : class, new()
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
                handler(Result<T>.CreateError(errorCode));
            }
            else
            {
                handler(Result<T>.CreateOk(payload));
            }
        }
    }
}
