// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    class LobbyWebsocketApi
    {
        #region public events declaration

        /// <summary>
        /// Raised when websocket successfully opened connection.
        /// </summary>
        public event OnOpenHandler OnOpen;

        /// <summary>
        /// Raised when websocket closed connection.
        /// </summary>
        public event OnCloseHandler OnClose;

        /// <summary>
        /// Raised when websocket received a message.
        /// </summary>
        public event OnMessageHandler OnMessage;

        /// <summary>
        /// Raised when websocket connection has an error.
        /// </summary>
        public event OnErrorHandler OnError;

        #endregion public events declaration
        
        #region public delegate declaration

        public delegate T NotificationPayloadModifier<T>(T payload) where T : class, new();
        
        #endregion

        #region private fields declaration

        private readonly string namespace_;
        private readonly object syncToken = new object();

        private readonly Dictionary<long, Action<ErrorCode, string>> responseCallbacks = new Dictionary<long, Action<ErrorCode, string>>();

        private readonly CoroutineRunner coroutineRunner;
        private readonly UserSession session;

        private long id;
        private string websocketUrl;
        private string channelSlug;
        private AccelByteWebSocket webSocket;
        private LobbySessionId lobbySessionId;

        private const string LOBBY_SESSION_ID_HEADER_NAME = "X-Ab-LobbySessionID";
        
        #endregion private fields declaration

        #region public properties

        public AccelByteWebSocket WebSocket => webSocket;

        #endregion

        #region constructor

        public LobbyWebsocketApi(CoroutineRunner inCoroutineRunner, string inWebsocketUrl, IWebSocket inWebsocket,
            UserSession inSession, string inNamespace)
        {
            Assert.IsNotNull(inCoroutineRunner);

            websocketUrl = inWebsocketUrl;
            namespace_ = inNamespace;

            session = inSession;
            coroutineRunner = inCoroutineRunner;

            lobbySessionId = new LobbySessionId();
            OverrideWebsocket(inWebsocket);
        }

        #endregion

        #region public methods

        public void OverrideWebsocket(IWebSocket inWebSocket
            , int inPingDelay = 4000
            , int inBackoffDelay = 1000
            , int inMaxDelay = 30000
            , int inTotalTimeout = 60000)
        {
            if (webSocket != null)
            {
                webSocket.OnOpen -= HandleOnOpen;
                webSocket.OnMessage -= HandleOnMessage;
                webSocket.OnClose -= HandleOnClose;
                webSocket.OnError -= HandleOnError;
            }
            
            webSocket = new AccelByteWebSocket(inWebSocket, coroutineRunner);
            webSocket.OnOpen += HandleOnOpen;
            webSocket.OnMessage += HandleOnMessage;
            webSocket.OnClose += HandleOnClose;
            webSocket.OnError += HandleOnError;

            webSocket.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay, inPingDelay);
        }

        /// <summary>
        /// Set lobbySessionID for current websocket connection
        /// </summary>
        public void SetSessionId(string lobbySessionID)
        {
            if(webSocket == null)
            {
                return;
            }

            webSocket.CustomHeaders[LOBBY_SESSION_ID_HEADER_NAME] = lobbySessionID;
        }

        /// <summary>
        /// Lobby connection status
        /// </summary>
        public bool IsConnected => webSocket?.IsConnected == true;

        /// <summary>
        /// Connect to lobby with current logged in user credentials.
        /// The token generator need to be set for connection with entitlement verification.
        /// </summary>
        public void Connect()
        {
            if (!session.IsValid())
            {
                throw new Exception("Cannot connect to websocket because user is not logged in.");
            }

            if (IsConnected)
            {
                AccelByteDebug.LogWarning("[Lobby] already connected");
                return;
            }
            
            if (webSocket.IsConnecting)
            {
                AccelByteDebug.LogWarning("[Lobby] lobby is connecting");
                return;
            }

            webSocket.Connect(websocketUrl, session.AuthorizationToken, lobbySessionId.lobbySessionID);
        }

        /// <summary>
        /// Change the delay parameters to maintain connection in the lobby before lobby connected.
        /// </summary>
        /// <param name="inTotalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="inBackoffDelay">Initial delay time</param>
        /// <param name="inMaxDelay">Maximum delay time</param>
        public void SetRetryParameters(
            int inTotalTimeout = 60000,
            int inBackoffDelay = 1000,
            int inMaxDelay = 30000)
        {
            webSocket?.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay);
        }

        /// <summary>
        /// TokenGenerator is used for generate access token when connecting to lobby. 
        /// If token generator is not specified, no token will be used when connecting to lobby.
        /// For entitlement token verification, use EntitlementTokenGenerator class on the parameter.
        /// </summary>
        /// <param name="tokenGenerator"> Token generator for connecting lobby. </param>
        public void SetConnectionTokenGenerator(ITokenGenerator tokenGenerator)
        {
            Assert.IsFalse(tokenGenerator == null, 
                "Can't set connection token generator! Token generator is null.");

            webSocket.SetConnectionTokenGenerator(tokenGenerator);
        }

        /// <summary>
        /// Disconnect from Lobby.
        /// </summary>
        public void Disconnect()
        {
            webSocket.Disconnect();

            channelSlug = null;
        }
        
        private void SendRequest<T, U>( MessageType requestType
            , T requestPayload
            , ResultCallback<U> callback )
            where T : class, new()
            where U : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);
            AwesomeFormat.WritePayload(writer, requestPayload);

            responseCallbacks[messageId] = (errorCode, response) =>
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

            webSocket.Send(writer.ToString());
        }

        private void SendRequest<T>( MessageType requestType
            , T requestPayload
            , ResultCallback callback )
            where T : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);
            AwesomeFormat.WritePayload(writer, requestPayload);

            responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result result = errorCode != ErrorCode.None 
                    ? Result.CreateError(errorCode) 
                    : Result.CreateOk();

                coroutineRunner.Run(() => callback.Try(result));
            };

            webSocket.Send(writer.ToString());
        }

        private void SendRequest<U>( MessageType requestType
            , ResultCallback<U> callback )
            where U : class, new()
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);

            responseCallbacks[messageId] = (errorCode, response) =>
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

            webSocket.Send(writer.ToString());
        }

        private void SendRequest( MessageType requestType
            , ResultCallback callback )
        {
            long messageId = GenerateId();
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, requestType, messageId);

            responseCallbacks[messageId] = (errorCode, response) =>
            {
                Result result = errorCode != ErrorCode.None 
                    ? Result.CreateError(errorCode) 
                    : Result.CreateOk();

                coroutineRunner.Run(() => callback.Try(result));
            };

            webSocket.Send(writer.ToString());
        }

        public void HandleNotification<T>(string message, ResultCallback<T> handler, NotificationPayloadModifier<T> modifier = null) where T : class, new()
        {
            Report.GetWebSocketNotification(message);

            if (handler == null)
            {
                return;
            }

            T payload;
            ErrorCode errorCode = AwesomeFormat.ReadPayload(message, out payload);
            if (modifier != null)
            {
                payload = modifier(payload);
            }

            if (errorCode != ErrorCode.None)
            {
                coroutineRunner.Run( ()=>handler(Result<T>.CreateError(errorCode)));
            }
            else
            {
                coroutineRunner.Run( ()=>handler(Result<T>.CreateOk(payload)));
            }
        }
        
        public void HandleUserStatusNotif(string message, ResultCallback<FriendsStatusNotif> handler)
        {
            HandleNotification(message, handler, payload =>
            {
                payload.activity = Uri.UnescapeDataString(payload.activity);
                
                return payload;
            });
        }
        
        public void DispatchNotification<T>(T notification, ResultCallback<T> handler)
            where T : class, new()
        {
            if (handler == null) return;

            coroutineRunner.Run(() => handler(Result<T>.CreateOk(notification)));
        }

        public void HandleResponse(long messageId, string message, ErrorCode errorCode)
        {
            Action<ErrorCode, string> handler;
            
            if (messageId != -1 && responseCallbacks.TryGetValue(messageId, out handler))
            {
                responseCallbacks.Remove(messageId);
                handler(errorCode, message);
            }
        }

        #region Party

        /// <summary>
        /// Get party information (leader, members, invitation token)
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains PartyInfo via callback when completed.
        /// </param>
        public void GetPartyInfo( ResultCallback<PartyInfo> callback )
        {
            SendRequest(MessageType.partyInfoRequest, callback);
        }

        /// <summary>
        /// Create a party and become a party leader for the party. 
        /// The newer function has different struct return callback and it has partyCode.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contain PartyInfo via callback when completed.
        /// </param>
        public void CreateParty( ResultCallback<PartyInfo> callback )
        {
            SendRequest(MessageType.partyCreateRequest, callback);
        }

        /// <summary>
        /// Create a party and become a party leader for the party.
        /// PartyCode is also provided to the party creator through the callback.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contain PartyCreateResponse via callback when completed.
        /// </param>
        public void CreateParty( ResultCallback<PartyCreateResponse> callback )
        {
            SendRequest(MessageType.partyCreateRequest, callback);
        }

        /// <summary>
        /// Leave a party a current logged in user is in.
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void LeaveParty( ResultCallback callback )
        {
            SendRequest(MessageType.partyLeaveRequest, callback);
        }

        /// <summary>
        /// Invite other user by userId. Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void InviteToParty( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.partyInviteRequest, 
                new PartyInviteRequest {friendID = userId}, callback);
        }

        /// <summary>
        /// Invite other user by userId with detailed model in response callback.
        /// Only party leader (creator) can invite other user.
        /// </summary>
        /// <param name="userId">User Id of a person to be invited to </param>
        /// <param name="callback">
        /// Returns a Result that contain PartyInviteResponse via callback when completed
        /// </param>
        public void InviteToPartyDetailedCallback( string userId
            , ResultCallback<PartyInviteResponse> callback )
        {
            SendRequest(MessageType.partyInviteRequest, 
                new PartyInviteRequest { friendID = userId }, callback);
        }

        /// <summary>
        /// Join a party by invitation from party leader.
        /// </summary>
        /// <param name="partyID">Party ID of the party to join to</param>
        /// <param name="invitationToken">Invitation token to join the party</param>
        /// <param name="callback">
        /// Returns a Result that contains PartyInfo via callback when completed
        /// </param>
        public void JoinParty( string partyID
            , string invitationToken
            , ResultCallback<PartyInfo> callback )
        {
            SendRequest(
                MessageType.partyJoinRequest,
                new PartyJoinRequest
                {
                    partyID = partyID,
                    invitationToken = invitationToken,
                },
                callback);
        }

        /// <summary>
        /// Kick a member out of our party. Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void KickPartyMember( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.partyKickRequest, 
                new PartyKickRequest {memberID = userId}, callback);
        }

        /// <summary>
        /// Kick a member out of our party with detailed model in response callback.
        /// Only a party leader can kick a party member.
        /// </summary>
        /// <param name="userId">User Id of the user to be kicked out of party</param>
        /// <param name="callback">
        /// Returns a Result that contains PartyKickResponse via callback when completed.
        /// </param>
        public void KickPartyMemberDetailedCallback( string userId
            , ResultCallback<KickResponse> callback )
        {
            SendRequest(MessageType.partyKickRequest, 
                new PartyKickRequest { memberID = userId }, callback);
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
        public void RejectPartyInvitation( string partyId
            , string invitationToken
            , ResultCallback<PartyRejectResponse> callback )
        {
            SendRequest(MessageType.partyRejectRequest, 
                new PartyRejectRequest
                {
                    invitationToken = invitationToken, 
                    partyID =  partyId,
                }, callback);
        }

        /// <summary>
        /// Generate party code for invitation
        /// </summary>
        /// <param name="callback">Return the party code that has been generated if success</param>
        public void GeneratePartyCode(ResultCallback<PartyGenerateCodeResponse> callback)
        {
            SendRequest(MessageType.partyGenerateCodeRequest, callback);
        }
        
        /// <summary>
        /// Get party code
        /// </summary>
        /// <param name="callback">Return the party code that has been generated previously if success</param>
        public void GetPartyCode(ResultCallback<PartyGetCodeResponse> callback)
        {
            SendRequest(MessageType.partyGetCodeRequest, callback);
        }
        
        /// <summary>
        /// Remove party invite code
        /// </summary>
        /// <param name="callback"></param>
        public void DeletePartyCode(ResultCallback callback)
        {
            SendRequest(MessageType.partyDeleteCodeRequest, callback);
        }

        /// <summary>
        /// Join to a party via party code
        /// </summary>
        /// <param name="callback"></param>
        public void JoinPartyViaCode(string invitationPartyCode, ResultCallback<PartyInfo> callback)
        {
            SendRequest(MessageType.partyJoinViaCodeRequest,
                new PartyJoinViaCodeRequest { partyCode = invitationPartyCode }, callback);
        }

        /// <summary> Promote member to be a party leader.</summary>
        /// <param name="userId">User ID that will be promoted as a party leader.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void PromotePartyLeader( string userId
            , ResultCallback<PartyPromoteLeaderResponse> callback )
        {
            SendRequest(MessageType.partyPromoteLeaderRequest, 
                new PartyPromoteLeaderRequest { newLeaderUserId = userId }, callback);
        }

        /// <summary>
        /// Send notification to party member 
        /// </summary>
        /// <param name="topic">Topic The topic of the request. Can use this as ID to know how to marshal the payload</param>
        /// <param name="payload">Payload The Payload of the request. Can be JSON string</param>
        /// <param name="callback">
        /// Returns a Result that contains PartySendNotifResponse via callback when completed.
        /// </param>
        public void SendNotificationToPartyMember(string topic, string payload, ResultCallback<PartySendNotifResponse> callback)
        {
            SendRequest(MessageType.partySendNotifRequest, 
                new PartySendNotifRequest { topic = topic, payload = payload}, callback);
        }

        #endregion Party

        #region Chat
        /// <summary>
        /// Send chat to other party members
        /// </summary>
        /// <param name="chatMessage">Message to send to party</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPartyChat( string chatMessage
            , ResultCallback callback )
        {
            SendRequest(MessageType.partyChatRequest, 
                new PartyChatRequest { payload = chatMessage }, callback);
        }

        /// <summary>
        /// Send personal chat to friend
        /// </summary>
        /// <param name="userId">Friend user id</param>
        /// <param name="chatMessage">Message to send to friend</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SendPersonalChat( string userId
            , string chatMessage
            , ResultCallback callback )
        {
            SendRequest(
                MessageType.personalChatRequest,
                new PersonalChatRequest { to = userId, payload = chatMessage },
                callback);
        }
        /// <summary>
        /// Send Join default global chat channel request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains ChatChannelSlug via callback when completed.
        /// </param>
        public void JoinDefaultChatChannel( ResultCallback<ChatChannelSlug> callback )
        {
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
        public void SendChannelChat( string chatMessage
            , ResultCallback callback )
        {
            if (string.IsNullOrEmpty(channelSlug))
            {
                callback.TryError(ErrorCode.InvalidRequest, 
                    "You're not in any chat channel.");
            }
            else
            {
                SendRequest(MessageType.sendChannelChatRequest, new ChannelChatRequest
                {
                    channelSlug = channelSlug,
                    payload = chatMessage,
                }, callback);
            }
        }
        #endregion Chat

        #region StatusAndPresence
        /// <summary>
        /// Set current user status and activity
        /// </summary>
        /// <param name="status">User status (Online, Available, Busy, Invisible)</param>
        /// <param name="activity">Describe an activity of the user, could be anything.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void SetUserStatus( UserStatus status
            , string activity
            , ResultCallback callback )
        {
            SendRequest(
                MessageType.setUserStatusRequest,
                new SetUserStatusRequest
                {
                    availability = status.ToString().ToLower(),
                    activity = Uri.EscapeDataString(activity), // Escape the string first for customizable string
                }, 
                callback);
        }

        /// <summary>
        /// Get a list of friends status (availability, activity, last seen at)
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends Status via callback when completed.
        /// </param>
        public void ListFriendsStatus( ResultCallback<FriendsStatus> callback )
        {
            SendRequest(MessageType.friendsStatusRequest, 
                (Result<FriendsStatus> result) =>
                {
                    if (result.IsError)
                    {
                        callback(result);
                        return;
                    }

                    int activityLength = result.Value.activity.Length;
                    for (int i = 0; i < activityLength; i++)
                    {
                        result.Value.activity[i] = Uri.UnescapeDataString(result.Value.activity[i]);
                    }

                    callback(result);
                });
        }
        #endregion StatusAndPresence

        #region LobbyNotification
        /// <summary>
        ///  Ask lobby to send all pending notification to user. Listen to OnNotification.
        /// </summary> 
        /// <param name="callback">Returns a Result via callback when completed.</param>
        [Obsolete("Lobby 2.4.0 and above dropped support for this function")]
        public void PullAsyncNotifications( ResultCallback callback )
        {
            SendRequest(MessageType.offlineNotificationRequest, callback);
        }
        #endregion LobbyNotification

        #region Friend
        /// <summary>
        /// Send request friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriend( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.requestFriendsRequest, 
                new Friend {friendId = userId}, callback);
        }

        /// <summary>
        /// Send request friend request using other profile's public id
        /// </summary>
        /// <param name="publicId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void RequestFriendByPublicId(string publicId
            , ResultCallback callback)
        {
            SendRequest(MessageType.requestFriendsByPublicIDRequest,
                new FriendByPublicId { friendPublicId = publicId }, callback);
        }

        /// <summary>
        /// Send unfriend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void Unfriend( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.unfriendRequest, 
                new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send list of outgoing friends request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void ListOutgoingFriends( ResultCallback<Friends> callback )
        {
            SendRequest(MessageType.listOutgoingFriendsRequest, callback);
        }

        /// <summary>
        /// Send cancel friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void CancelFriendRequest( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.cancelFriendsRequest, 
                new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send list of incoming friends request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void ListIncomingFriends( ResultCallback<Friends> callback )
        {
            SendRequest(MessageType.listIncomingFriendsRequest, callback);
        }

        /// <summary>
        /// Send accept friend request. 
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void AcceptFriend( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.acceptFriendsRequest, 
                new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send reject friend request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">Result of the function.</param>
        public void RejectFriend( string userId
            , ResultCallback callback )
        {
            SendRequest(MessageType.rejectFriendsRequest, 
                new Friend { friendId = userId }, callback);
        }

        /// <summary>
        /// Send load friends list request.
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains Friends via callback when completed.
        /// </param>
        public void LoadFriendsList( ResultCallback<Friends> callback )
        {
            SendRequest(MessageType.listOfFriendsRequest, callback);
        }

        /// <summary>
        /// Send get friendship status request.
        /// </summary>
        /// <param name="userId">Targeted user ID.</param>
        /// <param name="callback">
        /// Returns a Result that contains FriendshipStatus via callback when completed.
        /// </param>
        public void GetFriendshipStatus( string userId
            , ResultCallback<FriendshipStatus> callback )
        {
            SendRequest(MessageType.getFriendshipStatusRequest, 
                new Friend {friendId = userId}, callback);
        }
        #endregion Friend

        #region Matchmaking
        /// <summary>
        /// Send matchmaking start request.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        public void StartMatchmaking( string gameMode
            , MatchmakingOptionalParam param
            , ResultCallback<MatchmakingCode> callback )
        {
            string strLatencies = "";
            string jsonAttributeString = "";
            string userIdsCSV = "";
            string extraAttributesCSV = "";

            if (param != null)
            {
                if (param.latencies != null && param.latencies.Count > 0)
                {
                    strLatencies = "{" +
                    string.Join(",", param.latencies.Select(
                        pair => $@"""{pair.Key}"":{pair.Value}").ToArray()) +
                    "}";
                }

                if (param.tempPartyUserIds != null && param.tempPartyUserIds.Length > 0)
                    userIdsCSV = String.Join(",", param.tempPartyUserIds);

                if (param.extraAttributes != null && param.extraAttributes.Length > 0)
                    extraAttributesCSV = string.Join(",", param.extraAttributes);

                if (param.subGameModes != null && param.subGameModes.Length > 0)
                {
                    if (param.partyAttributes == null)
                        param.partyAttributes = new Dictionary<string, object>();

                    param.partyAttributes.Add("sub_game_mode", param.subGameModes);
                }

                if (param.newSessionOnly)
                {
                    if (param.partyAttributes == null)
                        param.partyAttributes = new Dictionary<string, object>();

                    param.partyAttributes.Add("new_session_only", "true");
                }

                if (param.partyAttributes != null)
                    jsonAttributeString = param.partyAttributes.ToJsonString();
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
                    extraAttributes = extraAttributesCSV,
                },
                callback);
        }
        
        /// <summary>
        /// Send a message to matchmaking service to indicate the user is ready for match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="callback"></param>
        public void ConfirmReadyForMatch( string matchId
            , ResultCallback callback )
        {
            SendRequest(MessageType.setReadyConsentRequest, 
                new ReadyConsentRequest { matchId = matchId }, callback);
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="callback">
        /// Result of the function with a cancel matchmaking status code.
        /// </param>
        public void CancelMatchmaking( string gameMode
            , ResultCallback<MatchmakingCode> callback )
        {
            SendRequest(
                MessageType.cancelMatchmakingRequest,
                new StartMatchmakingRequest { gameMode = gameMode },
                callback);
        }

        /// <summary>
        /// Send matchmaking cancel request.
        /// </summary>
        /// <param name="gameMode">Target matchmaking game mode</param>
        /// <param name="isTempParty">Is using temp party when starting matchmaking</param>
        /// <param name="callback">
        /// Result of the function with a cancel matchmaking status code.
        /// </param>
        public void CancelMatchmaking( string gameMode
            , bool isTempParty
            , ResultCallback<MatchmakingCode> callback )
        {
            SendRequest(
                MessageType.cancelMatchmakingRequest,
                new StartMatchmakingRequest 
                { 
                    gameMode = gameMode,
                    isTempParty = isTempParty,
                },
                callback);
        }

        /// <summary>
        /// Request Dedicated Custom Server
        /// </summary>
        /// <param name="request">Specification</param>
        /// <param name="callback"></param>
        public void RequestDS(CustomDsCreateRequest request)
        {
            SendRequest(MessageType.createDSRequest,
                request,
                r => { });
        }
        #endregion Matchmaking

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
        public void BlockPlayer( string userId
            , ResultCallback<BlockPlayerResponse> callback )
        {
            SendRequest(MessageType.blockPlayerRequest, new BlockPlayerRequest
            {
                userId = session.UserId,
                blockedUserId = userId,
                Namespace = namespace_,
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
        public void UnblockPlayer( string userId
            , ResultCallback<UnblockPlayerResponse> callback )
        {
            SendRequest(MessageType.unblockPlayerRequest, new UnblockPlayerRequest
            {
                userId = session.UserId,
                unblockedUserId = userId,
                Namespace = namespace_
            }, callback);
        }
        #endregion BlockUnblock
        
        #region ProfanityFilter
        public void SetProfanityFilterLevel( ProfanityFilterLevel level
            , ResultCallback callback )
        {
            SendRequest(MessageType.setSessionAttributeRequest, new SetSessionAttributeRequest
            {
                Namespace = namespace_,
                key = SessionAttributeName.profanity_filtering_level.ToString(),
                value = level.ToString()
            }, callback);
        }
        #endregion ProfanityFilter

        #region SessionAttribute
        public void SetSessionAttribute( string key
            , string value
            , ResultCallback callback )
        {
            SendRequest(MessageType.setSessionAttributeRequest, new SetSessionAttributeRequest
            {
                Namespace = namespace_,
                key = key,
                value = value
            }, callback);
        }

        public void GetSessionAttribute(string key, ResultCallback<GetSessionAttributeResponse> callback)
        {
            SendRequest(MessageType.getSessionAttributeRequest, new GetSessionAttributeRequest
            {
                key = key
            }, callback);
        }

        public void GetSessionAttributeAll( ResultCallback<GetSessionAttributeAllResponse> callback )
        {
            SendRequest(MessageType.getAllSessionAttributeRequest, callback);
        }
        #endregion SessionAttribute

        #region Signaling
        /// <summary>
        /// Send a signaling message to another user.
        /// </summary>
        /// <param name="userId">The recipient's user ID.</param>
        /// <param name="message">Signaling message to be sent.</param>
        public void SendSignalingMessage( string userId
            , string message )
        {
            SendRequest(MessageType.signalingP2PNotif, new SignalingP2P { destinationId = userId, message = message }, r => { });
        }
        #endregion Signaling

        #region Token
        public void RefreshToken( string newAccessToken
            , ResultCallback callback )
        {
            SendRequest(MessageType.refreshTokenRequest, new RefreshAccessTokenRequest
            {
                token = newAccessToken
            }, callback);
        }
        #endregion Token

        #endregion // public methods

        #region private methods

        private void HandleOnOpen()
        {
            this.OnOpen?.Invoke(); 
        }

        private void HandleOnClose(ushort closeCode)
        {
            this.OnClose?.Invoke(closeCode);
        }

        private void HandleOnMessage(string message)
        {
            this.OnMessage?.Invoke(message);
        }

        private void HandleOnError(string errorMsg)
        {
            this.OnError?.Invoke(errorMsg);
        }
        
        private long GenerateId()
        {
            lock (syncToken)
            {
                if (id < Int64.MaxValue)
                    id++;
                else
                    id = 0;
            }

            return id;
        }

        #endregion private methods
        
    }
}