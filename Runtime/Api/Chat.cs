﻿// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using HybridWebSocket;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AccelByte.Api
{   
    public class Chat : WrapperBase
    {
        #region constructor
        internal Chat(ChatApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            session = inSession;
            api = inApi;

            IWebSocket webSocket = new WebSocket();
            websocketApi = new ChatWebsocketApi(inCoroutineRunner
                , inSession
                , webSocket
                , api.GetConfig().ChatServerUrl
                , api.GetConfig().Namespace);

            websocketApi.OnOpen += HandleOnOpen;
            websocketApi.OnMessage += HandleOnMessage;
            websocketApi.OnClose += HandleOnClose;
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
            websocketApi.CreatePersonalTopic(otherUserId, callback);
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
            websocketApi.BlockUser(userId, callback);
        }
        
        /// <summary>
        /// Unblock user.
        /// </summary>
        /// <param name="userId">user ID to unblock.</param>
        /// <param name="callback">Function callback when operation is done.</param>
        public void UnblockUser(string userId, ResultCallback<BlockUnblockResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            websocketApi.UnblockUser(userId, callback);
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
            websocketApi.GetSystemMessagesStats(new GetSystemMessageStatsRequest(), callback);
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
        #endregion
        
        #region internal events
        internal event Action OnTokenRefreshed;
        #endregion

        #region private Members

        private ChatApi api;
        private UserSession session;
        private ChatWebsocketApi websocketApi;

        #endregion

        #region private methods
        
        private void HandleOnClose(ushort closecode)
        {
            Disconnected?.Invoke((WsCloseCode)closecode);
            websocketApi.SessionId = string.Empty;
        }
        
        private void HandleNotification<T>(string message, Action<T> eventHandler)
        {
            Report.GetWebSocketNotification(message);
            
            var payloadedMessage = JsonConvert.DeserializeObject<ChatWsMessage<T>>(message);
            if (payloadedMessage == null)
            {
                AccelByteDebug.Log("chat failed to deserialize notification\n" + message);
                return;
            }

            eventHandler?.Invoke(payloadedMessage.params_);
        }

        private void HandleOnMessage(string message)
        {
            AccelByteDebug.Log("Chat received ws message\n" + message);
            
            // deserialize json
            ChatWsMessage messageJsonObjectMethod = JsonConvert.DeserializeObject<ChatWsMessage>(message);
            if (messageJsonObjectMethod == null)
            {
                AccelByteDebug.Log("chat message deserialize message failed.");
                return;
            }

            // get event type from method field

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
                        AccelByteDebug.Log("Failed to deserialize connection payload");
                    }

                    Connected?.Invoke();
                    AccelByteDebug.Log("connected to chat service with session id " + websocketApi.SessionId);
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
                        AccelByteDebug.LogWarning("chat ws message method not supported! method is " 
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
            
            AccelByteDebug.Log("Connected to chat service");
        }

        private void OnRefreshTokenCallback_RefreshToken(string newToken)
        {
            websocketApi.RefreshToken(newToken, (result =>
            {
                if (result.IsError)
                {
                    AccelByteDebug.LogWarning($"Error sending new access token to chat service\n" +
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
    }
}