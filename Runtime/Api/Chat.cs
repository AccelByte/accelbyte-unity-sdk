// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using HybridWebSocket;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{   
    public class Chat : WrapperBase
    {
        #region private Members

        private ChatApi api;
        private UserSession session;
        private ChatWebsocketApi websocketApi;
        private readonly CoroutineRunner coroutineRunner;

        #endregion

        #region constructor
        [UnityEngine.Scripting.Preserve]
        internal Chat(ChatApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner) : this(inApi, inSession, inCoroutineRunner, null)
        {
            
        }

        [UnityEngine.Scripting.Preserve]
        internal Chat(ChatApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner
            , ChatWebsocketApi websocketApi)
        {
            session = inSession;
            api = inApi;
            coroutineRunner = inCoroutineRunner;

            if (websocketApi == null)
            {
                IWebSocket webSocket = new WebSocket();
                var newChatWebsocket = new ChatWebsocketApi(inCoroutineRunner
                    , inSession
                    , webSocket
                    , api.GetConfig().ChatServerWsUrl
                    , api.GetConfig().Namespace);
                SetWebsocketApi(newChatWebsocket);
            }
            else
            {
                SetWebsocketApi(websocketApi);
            }
        }
        #endregion

        #region public events
        /// <summary>
        /// Event triggered when websocket connection successfully connected.
        /// </summary>
        public event Action Connected;
        
        /// <summary>
        /// Event triggered when a websocket connection is disconnected.
        /// </summary>
        public event Action<WsCloseCode> Disconnected;

        /// <summary>
        /// Event triggered when user is added to a chat topic.
        /// </summary>
        public event Action<EventAddRemoveFromTopic> AddedToTopic;
        
        /// <summary>
        /// Event triggered when user is added to a chat topic.
        /// </summary>
        public event Action<EventAddRemoveFromTopic> RemovedFromTopic;

        /// <summary>
        /// Event triggered when a chat topic is deleted.
        /// </summary>
        public event Action<EventTopicUpdated> TopicDeleted;

        /// <summary>
        /// Event triggered when a new chat message is received.
        /// </summary>
        public event Action<EventNewChat> NewChatMessage;

        /// <summary>
        /// Event triggered when user is banned from chatting
        /// when ban is CHAT_ALL, then user also will not receive new chat events
        /// </summary>
        public event Action<UserBannedNotification> UserChatBanned;

        /// <summary>
        /// Event triggered when user is unbanned from chatting
        /// </summary>
        public event Action<UserBannedNotification> UserChatUnbanned;

        /// <summary>
        /// Event triggered when user receive new system message
        /// </summary>
        public event Action<SystemMessageNotif> NewSystemMessage;

        /// <summary>
        /// Event triggered when user muted from group chat
        /// </summary>
        public event Action<ChatMutedNotif> ChatMuted;

        /// <summary>
        /// Event triggered when user unmuted from group chat
        /// </summary>
        public event Action<ChatUnmutedNotif> ChatUnmuted;

        #endregion

        #region public properties
        
        /// <summary>
        /// Session Id of the connection to chat service.
        /// </summary>
        public string SessionId
        {
            get
            {
                if (websocketApi == null)
                {
                    return string.Empty;
                }
                
                return websocketApi.SessionId;
            }
        }

        public bool IsConnected
        {
            get
            {
                return websocketApi.IsConnected;
            }
        }
        
        #endregion
        
        #region public methods
        
        /// <summary>
        /// Establish a websocket connection to chat service.
        /// </summary>
        public void Connect()
        {
            Report.GetFunctionLog(GetType().Name);
            
            websocketApi.Connect();
        }

        /// <summary>
        /// Disconnect websocket connection from chat service.
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.Disconnect();
        }

        /// <summary>
        /// Send a new access token to be used in chat service.
        /// </summary>
        /// <param name="newToken">The new token to be used.</param>
        /// <param name="callback">Result of this operation.</param>
        public void RefreshToken(string newToken, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.RefreshToken(newToken, callback);
        }

        /// <summary>
        /// Create a private topic with other user.
        /// </summary>
        /// <param name="otherUserId">other user we want to create a topic with.</param>
        /// <param name="callback">Result of this operation.</param>
        public void CreatePersonalTopic(string otherUserId, ResultCallback<ChatActionTopicResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(otherUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(otherUserId), callback))
            {
                return;
            }

            websocketApi.CreatePersonalTopic(otherUserId, cb =>
            {
                SendCreateTopicPredefinedEvent(otherUserId, cb);
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Send chat message to a topic.
        /// </summary>
        /// <param name="topicId">ID of the topic.</param>
        /// <param name="message">Chat message to send.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void SendChatMessage(string topicId, string message, ResultCallback<SendChatResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.SendChatMessage(topicId, message, callback);
        }

        /// <summary>
        /// Query group topics that user is currently a part of.
        /// </summary>
        /// <param name="keyword">Topic name keyword to filter.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="offset">Page offset based on limit number, start from 0.
        /// (ex. offset 1 with limit 20 will give item number 21-40)</param>
        /// <param name="limit">Count limit of item returned in one request.</param>
        public void QueryGroupTopic(string keyword,
            ResultCallback<QueryTopicResponse> callback,
            int offset = 0,
            int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QueryGroupTopic(keyword, callback, offset, limit);
        }

        /// <summary>
        /// Query personal topics that user is currently a part of.
        /// </summary>
        /// <param name="keyword">Topic name keyword to filter.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="offset">Page offset based on limit number, start from 0.
        /// (ex. offset 1 with limit 20 will give item number 21-40)</param>
        /// <param name="limit">Count limit of item returned in one request.</param>
        public void QueryPersonalTopic(string keyword,
            ResultCallback<QueryTopicResponse> callback,
            int offset = 0,
            int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QueryPersonalTopic(keyword, callback, offset, limit);
        }

        /// <summary>
        /// Query group and personal topics that user is currently a part of.
        /// </summary>
        /// <param name="keyword">Topic name keyword to filter.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="offset">Page offset based on limit number, start from 0.
        /// (ex. offset 1 with limit 20 will give item number 21-40)</param>
        /// <param name="limit">Count limit of item returned in one request.</param>
        public void QueryTopic(string keyword, 
            ResultCallback<QueryTopicResponse> callback, 
            int offset = 0,
            int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QueryTopic(keyword, callback, offset, limit);
        }

        /// <summary>
        /// Query group and personal topic by topic ID.
        /// </summary>
        /// <param name="topicId">Topic ID to search.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void QueryTopic(string topicId, ResultCallback<QueryTopicByIdResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QueryTopic(topicId, callback);
        }

        /// <summary>
        /// Query chat messages within a topic.
        /// </summary>
        /// <param name="topicId">Topic ID to query.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="limit">Max returned data count.</param>
        /// <param name="lastChatCreatedAt">Oldest message date to fetch.</param>
        public void QueryChat(string topicId, 
            ResultCallback<QueryChatResponse> callback, 
            int limit = 20, 
            DateTime lastChatCreatedAt = default(DateTime))
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QueryChat(topicId, callback, limit, lastChatCreatedAt);
        }

        /// <summary>
        /// Block user.
        /// </summary>
        /// <param name="userId">user ID to block.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void BlockUser(string userId, ResultCallback<BlockUnblockResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            websocketApi.BlockUser(userId, cb =>
            {
                SendPredefinedEvent(EventMode.BlockUser, cb);
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Unblock user.
        /// </summary>
        /// <param name="userId">user ID to unblock.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void UnblockUser(string userId, ResultCallback<BlockUnblockResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            websocketApi.UnblockUser(userId, cb =>
            {
                SendPredefinedEvent(EventMode.UnblockUser, cb);
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Query system messages in user's system inbox
        /// </summary>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="request">Optional request parameter</param>
        public void QuerySystemMessage(ResultCallback<QuerySystemMessagesResponse> callback
            , QuerySystemMessageRequest request = default)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.QuerySystemMessage(callback, request);
        }

        /// <summary>
        /// Update system message(s) in user system inbox
        /// </summary>
        /// <param name="actionUpdateSystemMessages">Array of ActionUpdateSystemMessage containing action to mark read/unread and keep system messages.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void UpdateSystemMessages(HashSet<ActionUpdateSystemMessage> actionUpdateSystemMessages, ResultCallback<UpdateSystemMessagesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.UpdateSystemMessages(actionUpdateSystemMessages, callback);
        }

        /// <summary>
        /// Delete system message(s) in user system inbox
        /// </summary>
        /// <param name="messageIds">Array of system message id</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void DeleteSystemMessages(HashSet<string> messageIds, ResultCallback<DeleteSystemMessagesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.DeleteSystemMessages(messageIds, callback);
        }

        /// <summary>
        /// Get system messages stats
        /// </summary>
        /// <param name="callback">Function callback when operation is done.</param>
        /// <param name="request">Optional request parameter</param>
        public void GetSystemMessagesStats(ResultCallback<GetSystemMessageStatsResponse> callback
            , GetSystemMessageStatsRequest request = default)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.GetSystemMessagesStats(request, callback);
        }

        /// <summary>
        /// Delete a message from group chat (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="chatId">Id of message to delete.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void DeleteGroupChat(string groupId, string chatId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (!ValidateAccelByteId(chatId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChatIdInvalidMessage(chatId), callback))
            {
                return;
            }

            coroutineRunner.Run(api.DeleteGroupChat(groupId, chatId, cb =>
            {
                SendModeratorPredefinedEvent(groupId, chatId, EventMode.ModDeleteGroupChat, cb);
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Mute user from group chat (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="userId">Id of the user to be muted.</param>
        /// <param name="durationInSeconds">Duration of mute in seconds.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void MuteGroupUserChat(string groupId, string userId, int durationInSeconds, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            MuteGroupChatRequest req = new MuteGroupChatRequest
            {
                UserId = userId,
                Duration = durationInSeconds
            };

            coroutineRunner.Run(api.MuteGroupUserChat(groupId, req, cb =>
            {
                SendModeratorPredefinedEvent(groupId, userId, EventMode.ModMuteUser, cb);
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Unmute user from group chat (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="userId">Id of the user to be muted.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void UnmuteGroupUserChat(string groupId, string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            UnmuteGroupChatRequest req = new UnmuteGroupChatRequest { UserId = userId };

            coroutineRunner.Run(api.UnmuteGroupUserChat(groupId, req, cb =>
            {
                SendModeratorPredefinedEvent(groupId, userId, EventMode.ModUnmuteUser, cb);
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Get chat snapshot (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="chatId">Id of message.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void GetGroupChatSnapshot(string groupId, string chatId, ResultCallback<ChatSnapshotResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (!ValidateAccelByteId(chatId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetChatIdInvalidMessage(chatId), callback))
            {
                return;
            }

            coroutineRunner.Run(api.GetGroupChatSnapshot(groupId, chatId, callback));
        }

        /// <summary>
        /// Ban users from group chat (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="userIds">List of user ids to ban.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void BanGroupUserChat(string groupId, List<string> userIds, ResultCallback<BanGroupChatResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            BanGroupChatRequest req = new BanGroupChatRequest
            {
                UserIds = userIds
            };

            coroutineRunner.Run(api.BanGroupUserChat(groupId, req, cb =>
            {
                SendModeratorPredefinedEvent(groupId, userIds, EventMode.ModBanUser, cb);
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Unban users from group chat (used by group moderator).
        /// </summary>
        /// <param name="groupId">Id of group from group service.</param>
        /// <param name="userIds">List of user ids to ban.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void UnbanGroupUserChat(string groupId, List<string> userIds, ResultCallback<UnbanGroupChatResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            UnbanGroupChatRequest req = new UnbanGroupChatRequest
            {
                UserIds = userIds
            };

            coroutineRunner.Run(api.UnbanGroupUserChat(groupId, req, cb =>
            {
                SendModeratorPredefinedEvent(groupId, userIds, EventMode.ModUnbanUser, cb);
                HandleCallback(cb, callback);
            }));
        }
        
        #endregion
        
        #region internal methods
        /// <summary>
        /// Establish a websocket connection via proxy url.
        /// usually so 3rd party web debugging proxy tool (ex. fiddler) can see the message.
        /// </summary>
        /// <param name="proxyUrl"></param>
        internal void Connect(string proxyUrl)
        {
            var ws = new WebSocket();
            ws.SetProxy("http://" + proxyUrl, "", "");
            websocketApi.OverrideWebsocket(ws);
            
            Connect();
        }

        internal void SetWebsocketApi(ChatWebsocketApi newWebsocketApi)
        {
            if (newWebsocketApi == null)
            {
                return;
            }

            if (this.websocketApi != null)
            {
                this.websocketApi.OnOpen -= HandleOnOpen;
                this.websocketApi.OnMessage -= HandleOnMessage;
                this.websocketApi.OnClose -= HandleOnClose;
                this.websocketApi = null;
            }

            this.websocketApi = newWebsocketApi;
            this.websocketApi.OnOpen += HandleOnOpen;
            this.websocketApi.OnMessage += HandleOnMessage;
            this.websocketApi.OnClose += HandleOnClose;
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            this.websocketApi.SetSharedMemory(newSharedMemory);
        } 

        #endregion
        
        #region internal events
        internal event Action OnTokenRefreshed;
        #endregion

        #region private methods
        
        private void HandleOnClose(ushort closecode)
        {
#if DEBUG
            if (Enum.TryParse(closecode.ToString(), out WsCloseCode verboseCode))
            {
                SharedMemory?.Logger?.Log($"[Ws Chat] Websocket connection close: {closecode} named {verboseCode.ToString()}");
            }
            else
            {
                SharedMemory?.Logger?.Log($"[Ws Chat] Websocket connection close: {closecode}. Please refers https://demo.accelbyte.io/chat/v1/messages for more info");
            }
#endif

            Disconnected?.Invoke((WsCloseCode)closecode);
            websocketApi.SessionId = string.Empty;
        }
        
        private void HandleNotification<T>(string message, Action<T> eventHandler)
        {
            Report.GetWebSocketNotification(message);
            
            var payloadedMessage = JsonConvert.DeserializeObject<ChatWsMessage<T>>(message);
            if (payloadedMessage == null)
            {
                SharedMemory?.Logger?.Log("chat failed to deserialize notification\n" + message);
                return;
            }

            eventHandler?.Invoke(payloadedMessage.params_);
        }

        private void HandleOnMessage(string message)
        {
            SharedMemory?.Logger?.Log("Chat received ws message\n" + message);
            
            // deserialize json
            ChatWsMessage messageJsonObjectMethod = JsonConvert.DeserializeObject<ChatWsMessage>(message);
            if (messageJsonObjectMethod == null)
            {
                SharedMemory?.Logger?.Log("chat message deserialize message failed.");
                return;
            }

            string chatMessageMethod = messageJsonObjectMethod.method.ToString();
            if (SharedMemory.NetworkConditioner != null 
                && SharedMemory.NetworkConditioner.CalculateFailRate(chatMessageMethod))
            {
                SharedMemory?.Logger?.Log($"[AccelByteNetworkConditioner] Dropped chat message method {chatMessageMethod}.");
                return;
            }

            // switch case between handle type
            switch (messageJsonObjectMethod.method)
            {
                case ChatMessageMethod.eventConnected:
                    var messageWithPayload = JsonConvert.DeserializeObject<ChatWsMessage<EventConnected>>(message);

                    if (messageWithPayload != null)
                    {
                        websocketApi.SessionId = messageWithPayload.params_.sessionId;
                    }
                    else
                    {
                        SharedMemory?.Logger?.Log("Failed to deserialize connection payload");
                    }

                    Connected?.Invoke();
                    SharedMemory?.Logger?.Log("connected to chat service with session id " + websocketApi.SessionId);
                    break;
                case ChatMessageMethod.actionRefreshToken:
                    OnTokenRefreshed?.Invoke();
                    break;
                case ChatMessageMethod.eventTopicDeleted:
                    HandleNotification(message, TopicDeleted);
                    break;
                case ChatMessageMethod.eventAddedToTopic:
                    HandleNotification(message, AddedToTopic);
                    break;
                case ChatMessageMethod.eventRemovedFromTopic:
                    HandleNotification(message, RemovedFromTopic);
                    break;
                case ChatMessageMethod.eventNewChat:
                    HandleNotification(message, NewChatMessage);
                    break;
                case ChatMessageMethod.eventBanChat:
                    HandleBanNotification();
                    HandleNotification(message, UserChatBanned);
                    break;
                case ChatMessageMethod.eventUnbanChat:
                    HandleUnbanNotification();
                    HandleNotification(message, UserChatUnbanned);
                    break;
                case ChatMessageMethod.eventNewSystemMessage:
                    HandleNotification(message, NewSystemMessage);
                    break;
                case ChatMessageMethod.eventUserMuted:
                    HandleNotification(message, ChatMuted);
                    break;
                case ChatMessageMethod.eventUserUnmuted:
                    HandleNotification(message, ChatUnmuted);
                    break;
                default:
                    bool IsResponse = messageJsonObjectMethod.method.ToString().ToLower().StartsWith("action")
                                      || messageJsonObjectMethod.method == ChatMessageMethod.sendChat
                                      || messageJsonObjectMethod.method == ChatMessageMethod.readChat
                                      || messageJsonObjectMethod.method == ChatMessageMethod.queryChat;
                    if (IsResponse)
                    {
                        // Handle Response
                        websocketApi.HandleResponse(messageJsonObjectMethod.id, 
                            messageJsonObjectMethod.error, message);
                    }
                    else
                    {
                        SharedMemory?.Logger?.LogWarning("chat ws message method not supported! method is " 
                            + messageJsonObjectMethod.method);
                    }
                    break;
            }
        }

        private void HandleOnOpen()
        {
            // subscribe to refresh token callback to send new token to chat service.
            if (session != null)
            {
                session.RefreshTokenCallback += OnRefreshTokenCallback_RefreshToken;
            }
            
            SharedMemory?.Logger?.Log("Connected to chat service");
        }

        private void OnRefreshTokenCallback_RefreshToken(string newToken)
        {
            websocketApi.RefreshToken(newToken, (result =>
            {
                if (result.IsError)
                {
                    SharedMemory?.Logger?.LogWarning($"Error sending new access token to chat service\n" +
                                              $"code {result.Error.Code} with message {result.Error.Message}");
                }
            }));
        }
        
        private void HandleBanNotification()
        {

        }

        private void HandleUnbanNotification()
        {
            if (session != null)
            {
                session.RefreshTokenCallback -= OnRefreshTokenCallback_RefreshToken;
            }

            api.OnBanNotificationReceived((newAuthToken) =>
            {
                session.AuthorizationToken = newAuthToken;
                if (session != null)
                {
                    session.RefreshTokenCallback += OnRefreshTokenCallback_RefreshToken;
                }
            });
        }

        #endregion

        #region PredefinedEvents

        bool isAnalyticsConnected = false;

        private void ConnectPredefinedAnalyticsToEvents()
        {
            if (isAnalyticsConnected)
            {
                return;
            }

            Connected += SendConnectedPredefinedEvent;
            Disconnected += (result) =>
            {
                SendPredefinedEvent(result);
            };
            AddedToTopic += (result) =>
            {
                SendPredefinedEvent(result, EventMode.AddUserToTopic);
            };
            RemovedFromTopic += (result) =>
            {
                SendPredefinedEvent(result, EventMode.RemoveUserFromTopic);
            };

            isAnalyticsConnected = true;
        }

        internal enum EventMode
        {
            None,
            AddUserToTopic,
            RemoveUserFromTopic,
            BlockUser,
            UnblockUser,
            ModMuteUser,
            ModUnmuteUser,
            ModBanUser,
            ModUnbanUser,
            ModDeleteGroupChat
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T result, EventMode eventMode = EventMode.None)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (typeof(T))
            {
                case Type closeCodeDelegate when closeCodeDelegate == typeof(WsCloseCode):
                    Enum statusCode = result as Enum;
                    var statusCodeEnum = (WsCloseCode)statusCode;

                    payload = new PredefinedChatV2DisconnectedPayload(localUserId, statusCodeEnum);

                    break;

                case Type addRemoveFromTopicDelegate when addRemoveFromTopicDelegate == typeof(EventAddRemoveFromTopic):
                    var addRemoveFromTopic = result as EventAddRemoveFromTopic;

                    if (eventMode == EventMode.AddUserToTopic)
                    {
                        payload = new PredefinedChatV2TopicUserAddedPayload(addRemoveFromTopic.senderId, addRemoveFromTopic.topicId);
                    }
                    else if (eventMode == EventMode.RemoveUserFromTopic)
                    {
                        payload = new PredefinedChatV2TopicUserRemovedPayload(addRemoveFromTopic.senderId, addRemoveFromTopic.topicId);
                    }

                    break;

                case Type updateTopicDelegate when updateTopicDelegate == typeof(EventTopicUpdated):
                    var updateTopic = result as EventTopicUpdated;

                    payload = new PredefinedChatV2TopicDeletedPayload(updateTopic.senderId, updateTopic.topicId);

                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(Result<T> result, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (typeof(T))
            {
                case Type blockUnblockUserDelegate when blockUnblockUserDelegate == typeof(BlockUnblockResponse):
                    var blockUnblockResponse = result.Value as BlockUnblockResponse;

                    if (eventMode == EventMode.BlockUser)
                    {
                        payload = new PredefinedChatV2UserBlockedPayload(localUserId, blockUnblockResponse.userId);
                    }
                    else if (eventMode == EventMode.UnblockUser)
                    {
                        payload = new PredefinedChatV2UserUnblockedPayload(localUserId, blockUnblockResponse.userId);
                    }

                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreateConnectedPayload()
        {
            string localUserId = session.UserId;
            IAccelByteTelemetryPayload payload = new PredefinedChatV2ConnectedPayload(localUserId);

            return payload;
        }

        private IAccelByteTelemetryPayload CreateTopicCreatedPayload(string targetUserId)
        {
            string localUserId = session.UserId;
            IAccelByteTelemetryPayload payload = new PredefinedChatV2CreateTopicPayload(localUserId, targetUserId);

            return payload;
        }

        private IAccelByteTelemetryPayload CreateModeratorPayload(string groupId, string targetId, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (eventMode)
            {
                case EventMode.ModMuteUser:
                    payload = new PredefinedChatV2ModeratorMutedPayload(groupId, localUserId, targetId);
                    break;

                case EventMode.ModUnmuteUser:
                    payload = new PredefinedChatV2ModeratorUnmutedPayload(groupId, localUserId, targetId);
                    break;

                case EventMode.ModBanUser:
                    payload = new PredefinedChatV2ModeratorBannedPayload(groupId, localUserId, targetId);
                    break;

                case EventMode.ModUnbanUser:
                    payload = new PredefinedChatV2ModeratorUnbannedPayload(groupId, localUserId, targetId);
                    break;

                case EventMode.ModDeleteGroupChat:
                    payload = new PredefinedChatV2ModeratorDeletedPayload(groupId, localUserId, targetId);
                    break;
            }

            return payload;
        }

        internal void SendPredefinedEvent<T>(T result, EventMode eventMode = EventMode.None)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, eventMode);

            if (payload == null)
            {
                return;
            }

            var chatEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(chatEvent, null);
        }

        private void SendPredefinedEvent<T>(EventMode eventMode, Result<T> result)
        {
            if (result.IsError)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, eventMode);

            if (payload == null)
            {
                return;
            }

            var chatEvent = new AccelByteTelemetryEvent(payload);

            predefinedEventScheduler.SendEvent(chatEvent, null);
        }

        internal void SendConnectedPredefinedEvent()
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreateConnectedPayload();

            if (payload == null)
            {
                return;
            }

            var chatEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(chatEvent, null);
        }

        private void SendCreateTopicPredefinedEvent(string targetId, Result<ChatActionTopicResponse> result)
        {
            if (result.IsError)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreateTopicCreatedPayload(targetId);

            if (payload == null)
            {
                return;
            }

            var chatEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(chatEvent, null);
        }

        private void SendModeratorPredefinedEvent(string groupId, string targetId, EventMode eventMode, Result result)
        {
            if (result.IsError)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreateModeratorPayload(groupId, targetId, eventMode);

            if (payload == null)
            {
                return;
            }

            var chatEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(chatEvent, null);
        }

        private void SendModeratorPredefinedEvent<T>(string groupId, List<string> targetIds, EventMode eventMode, Result<T> result)
        {
            if (result.IsError)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            foreach(string targetId in targetIds)
            {
                IAccelByteTelemetryPayload payload = CreateModeratorPayload(groupId, targetId, eventMode);

                if (payload == null)
                {
                    continue;
                }

                var chatEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(chatEvent, null);
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