// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.IO;
using AccelByte.Core;
using AccelByte.Models;
using WebSocketSharp;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to connect to Lobby and access its services. 
    /// </summary>
    public class Lobby
    {
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

        private const int PingDelay = 30000;
        
        private readonly User account;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;
        private readonly WebSocket webSocket;
        private readonly Dictionary<long, Action<ErrorCode, string>> responseCallbacks;
        private readonly object syncToken = new object();
        private bool Connected;
        
        private long id;

        internal Lobby(string url, User account, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            this.account = account;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
            this.responseCallbacks = new Dictionary<long, Action<ErrorCode, string>>();
            this.webSocket = new WebSocket(url + "/lobby/");
            this.webSocket.OnMessage += HandleMessages;
            this.webSocket.SslConfiguration.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Lobby connection status
        /// </summary>
        public bool IsConnected
        {
            get { return this.Connected; }
        }

        /// <summary>
        /// Connect to lobby with current logged in user credentials.
        /// </summary>
        public void Connect()
        {
            this.webSocket.CustomHeaders = new Dictionary<string, string>
                {{"Authorization", "Bearer " + this.account.AccessToken}};
            this.webSocket.Connect();
            this.Connected = true;
            this.taskDispatcher.Start(Ping());
        }

        /// <summary>
        /// Disconnect from Lobby.
        /// </summary>
        public void Disconnect()
        {
            this.webSocket.Close();
            this.Connected = false;
        }

        /// <summary>
        /// Get party information (leader, members, invitation token)
        /// </summary>
        /// <param name="callback">Returns a Result that contains PartyInfo via callback when completed.</param>
        public void GetPartyInfo(ResultCallback<PartyInfo> callback)
        {
            SendRequest(MessageType.partyInfoRequest, callback);
        }

        /// <summary>
        /// Create a party and become a party leader for the party. 
        /// </summary>
        /// <param name="callback">Returns a Result that contain PartyInfo via callback when completed.</param>
        public void CreateParty(ResultCallback<PartyInfo> callback)
        {
            SendRequest(MessageType.partyCreateRequest, callback);
        }

        /// <summary>
        /// Leave a party a current logged in user is in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LeaveParty(ResultCallback callback)
        {
            SendRequest(MessageType.partyLeaveRequest, callback);
        }

        /// <summary>
        /// Invite other user by userId. Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void InviteToParty(string userId, ResultCallback callback)
        {
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
            SendRequest(MessageType.partyJoinRequest,
                new PartyJoinRequest {partyID = partyID, invitationToken = invitationToken}, callback);
        }

        /// <summary>
        /// Kick a member out of our party. Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void KickPartyMember(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.partyKickRequest, new PartyKickRequest {memberID = userId}, callback);
        }

        /// <summary>
        /// Send chat to other party members
        /// </summary>
        /// <param name="chatMessage">Message to send to party</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPartyChat(string chatMessage, ResultCallback callback)
        {
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
            SendRequest(MessageType.personalChatRequest,
                new PersonalChatRequest {to = userId, payload = chatMessage}, callback);
        }

        /// <summary>
        /// Set current user status and activity
        /// </summary>
        /// <param name="status">User status (Online, Available, Busy, Invisible)</param>
        /// <param name="activity">Describe an activity of the user, could be anything.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void SetUserStatus(UserStatus status, string activity, ResultCallback callback)
        {
            SendRequest(
                MessageType.setUserStatusRequest,
                new SetUserStatusRequest
                {
                    availability = (uint) status,
                    activity = activity
                },
                callback);
        }

        /// <summary>
        /// Get a list of friends status (availability, activity, last seen at)
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends Status via callback when completed.</param>
        public void ListFriendsStatus(ResultCallback<FriendsStatus> callback)
        {
            SendRequest(MessageType.friendsStatusRequest, callback);
        }
        
        /// <summary>
        ///  Ask lobby to send all pending notification to user. Listen to OnNotification.
        /// </summary> 
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void PullAsyncNotifications(ResultCallback callback)
        {
            SendRequest(MessageType.offlineNotificationRequest, callback);
        }
        
        /// <summary>
        /// Send request friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriend(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.requestFriendsRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send unfriend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void Unfriend(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.unfriendRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send list of outgoing friends request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void ListOutgoingFriends(ResultCallback<Friends> callback)
        {
            SendRequest(MessageType.listOutgoingFriendsRequest, callback);
        }

        /// <summary>
        /// Send cancel friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void CancelFriendRequest(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.cancelFriendsRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send list of incoming friends request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void ListIncomingFriends(ResultCallback<Friends> callback)
        {
            SendRequest(MessageType.listIncomingFriendsRequest, callback);
        }

        /// <summary>
        /// Send accept friend request. 
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void AcceptFriend(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.acceptFriendsRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send reject friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void RejectFriend(string userId, ResultCallback callback)
        {
            SendRequest(MessageType.rejectFriendsRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send load friends list request.
        /// </summary>
        /// <param name="callback">Returns a Result that contains Friends via callback when completed.</param>
        public void LoadFriendsList(ResultCallback<Friends> callback)
        {
            SendRequest(MessageType.listOfFriendsRequest, callback);
        }

        /// <summary>
        /// Send get friendship status request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result that contains FriendshipStatus via callback when completed.</param>
        public void GetFriendshipStatus(string userId, ResultCallback<FriendshipStatus> callback)
        {
            SendRequest(MessageType.getFriendshipStatusRequest, new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">Result of the function with a start matchmaking status code.</param>
        public void StartMatchmaking(string gameMode, ResultCallback<MatchmakingCode> callback)
        {
            SendRequest(MessageType.startMatchmakingRequest,
                new GameMode { gameMode = gameMode }, callback);
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">Result of the function with a cancel matchmaking status code.</param>
        public void CancelMatchmaking(string gameMode, ResultCallback<MatchmakingCode> callback)
        {
            SendRequest(MessageType.cancelMatchmakingRequest,
                new GameMode { gameMode = gameMode }, callback);
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
            where T : class, new()
            where U : class, new()
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
                
                this.coroutineRunner.Run(() => callback(result));
            };
            
            this.webSocket.SendAsync(writer.ToString(), null);
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

                this.coroutineRunner.Run(() => callback(result));
            };
            
            this.webSocket.SendAsync(writer.ToString(), null);
        }

        private void SendRequest<U>(MessageType requestType, ResultCallback<U> callback)
            where U : class, new()
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

                this.coroutineRunner.Run(() => callback(result));
            };

            this.webSocket.SendAsync(writer.ToString(), null);
        }

        private void SendRequest(MessageType requestType, ResultCallback callback)
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);

            this.responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result result = errorCode != ErrorCode.None ? Result.CreateError(errorCode) : Result.CreateOk();

                this.coroutineRunner.Run(() => callback(result));
            };
            
            this.webSocket.SendAsync(writer.ToString(), null);
        }

        private void HandleMessages(object sender, MessageEventArgs eventArgs)
        {
            long messageId;
            MessageType messageType;
            ErrorCode errorCode = AwesomeFormat.ReadHeader(eventArgs.Data, out messageType, out messageId);

            switch (messageType)
            {
                case MessageType.partyGetInvitedNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.InvitedToParty);

                    break;
                case MessageType.partyJoinNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.JoinedParty);

                    break;
                case MessageType.partyKickNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.KickedFromParty);

                    break;
                case MessageType.personalChatNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.PersonalChatReceived);

                    break;
                case MessageType.partyChatNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.PartyChatReceived);

                    break;
                case MessageType.messageNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.OnNotification);

                    break;
                case MessageType.userStatusNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.FriendsStatusChanged);

                    break;
                case MessageType.matchmakingNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.MatchmakingCompleted);

                    break;
                case MessageType.acceptFriendsNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.FriendRequestAccepted);

                    break;
                case MessageType.requestFriendsNotif:
                    Lobby.HandleNotification(eventArgs.Data, this.OnIncomingFriendRequest);

                    break;
                case MessageType.connectNotif:
                    this.Connected = true;

                    break;
                default:
                    Action<ErrorCode, string> handler;

                    if (messageId != -1 && this.responseCallbacks.TryGetValue(messageId, out handler))
                    {
                        this.responseCallbacks.Remove(messageId);
                        handler(errorCode, eventArgs.Data);
                    }

                    break;
            }
        }

        private IEnumerator<ITask> Ping()
        {
            while (this.IsConnected)
            {
                yield return new DelayTask(Lobby.PingDelay);

                this.webSocket.Ping();
            }
        }

        private static void HandleNotification<T>(string message, ResultCallback<T> handler)
            where T : class, new()
        {
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
