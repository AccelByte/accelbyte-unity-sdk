// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace AccelByte.Api
{
    public class ChatWebsocketApi : AccelByteWebsocketApi
    {
        #region private properties
        
        private WsMessageFragmentProcessor messageFragmentProcessor = new WsMessageFragmentProcessor();
        private Dictionary<string, Action<ErrorCode, string>> responseCallbacks = new Dictionary<string, Action<ErrorCode, string>>();

        private const string messageFragmentStart = "CaSr";
        private const string messageFragmentEnd = "CaEd";
        private const string JsonRpcVer = "2.0";

        private JsonSerializerSettings skipNullSerializer = new JsonSerializerSettings()
            { DefaultValueHandling = DefaultValueHandling.Ignore};
        
        #endregion

        #region constructor

        internal ChatWebsocketApi(CoroutineRunner inCoroutineRunner
            , UserSession inSession
            , IWebSocket inWebSocket
            , string inWebsocketUrl
            , string inNamespace) : base(inCoroutineRunner, inSession, inWebSocket, inWebsocketUrl, inNamespace)
        {
            SetOnMessageHandler(HandleOnMessage);
        }
        #endregion

        #region public method

        public bool IsConnected
        {
            get
            {
                return webSocket.IsConnected;
            }
        }

        public override void Connect()
        {
            Dictionary<string, string> customHeaders = new Dictionary<string, string>()
            {
                { "X-Ab-ChatSessionID", SessionId },
                { "X-Ab-RpcEnvelopeStart", messageFragmentStart },
                { "X-Ab-RpcEnvelopeEnd", messageFragmentEnd }
            };

            messageFragmentProcessor.envelopeEnd = messageFragmentEnd;
            messageFragmentProcessor.envelopeStart = messageFragmentStart;
            base.Connect(customHeaders);
        }

        public void HandleResponse(string messageId, Error error, string message)
        {
            if (responseCallbacks.ContainsKey(messageId))
            {
                var handler = responseCallbacks[messageId];

                responseCallbacks.Remove(messageId);
                ErrorCode errorCode = error == null ? ErrorCode.None : error.Code; 
                handler(errorCode, message);
            }
        }

        // Refresh token
        public void RefreshToken(string newToken, ResultCallback result)
        {
            RefreshTokenRequest request = new RefreshTokenRequest()
            {
                token = newToken
            };
            
            SendRequest(ChatMessageMethod.actionRefreshToken, request, result);
        }
        
        #region Chat topic
        public void CreatePersonalTopic(string otherUserId, ResultCallback<ChatActionTopicResponse> callback)
        {
            ChatActionTopicRequest request = new ChatActionTopicRequest()
            {
                Namespace = namespace_,
                type = ChatTopicType.personal.ToString().ToUpper(),
                members = new[]
                {
                    session.UserId,
                    otherUserId
                }
            };
            
            SendRequest(ChatMessageMethod.actionCreateTopic, request, callback);
        }
        #endregion
        
        #region query topic
        public void QueryGroupTopic(string keyword, ResultCallback<QueryTopicResponse> callback, int offset, int limit)
        {
            QueryTopicRequest request = new QueryTopicRequest()
            {
                Namespace = namespace_, keyword = keyword, offset = offset, limit = limit
            };
            
            SendRequest(ChatMessageMethod.actionQueryGroupTopic, request, callback);
        }
        
        public void QueryPersonalTopic(string keyword, ResultCallback<QueryTopicResponse> callback, int offset, int limit)
        {
            QueryTopicRequest request = new QueryTopicRequest()
            {
                Namespace = namespace_, keyword = keyword, offset = offset, limit = limit
            };
            
            SendRequest(ChatMessageMethod.actionQueryPersonalTopic, request, callback);
        }
        
        public void QueryTopic(string keyword, ResultCallback<QueryTopicResponse> callback, int offset, int limit)
        {
            QueryTopicRequest request = new QueryTopicRequest()
            {
                Namespace = namespace_, keyword = keyword, offset = offset, limit = limit
            };
            
            SendRequest(ChatMessageMethod.actionQueryTopic, request, callback);
        }
        
        public void QueryTopic(string topicId, ResultCallback<QueryTopicByIdResponse> callback)
        {
            QueryTopicByIdRequest request = new QueryTopicByIdRequest()
            {
                topicId = topicId
            };
            
            SendRequest(ChatMessageMethod.actionQueryTopicById, request, callback);
        }
        #endregion
        
        #region Send chat
        public void SendChatMessage(string topicId, string message, ResultCallback<SendChatResponse> callback)
        {
            SendChatRequest request = new SendChatRequest() { topicId = topicId, message = message };
            
            SendRequest(ChatMessageMethod.sendChat, request, callback);
        }
        #endregion

        #region query chat
        public void QueryChat(string topicId, ResultCallback<QueryChatResponse> callback, int limit, DateTime lastChatCreatedAt)
        {
            QueryChatRequest request = new QueryChatRequest()
            {
                topicId = topicId, limit = limit, lastChatCreatedAt = lastChatCreatedAt
            };
            
            SendRequest(ChatMessageMethod.queryChat, request, callback);
        }
        #endregion
        
        #region block / unblock
        
        public void BlockUser(string userId, ResultCallback<BlockUnblockResponse> callback)
        {
            BlockUnblockRequest request = new BlockUnblockRequest() { userId = userId};
            
            SendRequest(ChatMessageMethod.actionBlockUser, request, callback);
        }
        
        public void UnblockUser(string userId, ResultCallback<BlockUnblockResponse> callback)
        {
            BlockUnblockRequest request = new BlockUnblockRequest() { userId = userId};
            
            SendRequest(ChatMessageMethod.actionUnblockUser, request, callback);
        }
        
        
        #endregion
        
        #endregion
        
        #region protected methods
        protected override void HandleOnMessage(string message)
        {
            // process if message is fragmented, wait until received full message
            string completeMessage;
            bool isMessageComplete = messageFragmentProcessor.Process(message, out completeMessage);

            if (!isMessageComplete)
                return; // nothing further to do if message not complete
            
            base.HandleOnMessage(completeMessage);
        }
        #endregion
        
        #region private methods

        private string CreateRequestMessage<T>(string messageId, ChatMessageMethod method, T request)
        {
            ChatWsMessage<T> requestEnveloped = new ChatWsMessage<T>()
            {
                jsonrpc = JsonRpcVer,
                id = messageId,
                method = method,
                params_ = request
            };
            
            string requestJsonString = JsonConvert.SerializeObject(requestEnveloped, Formatting.None, 
                skipNullSerializer);
            
            return requestJsonString;
        }
        
        private string CreateRequestMessage(string messageId, ChatMessageMethod method)
        {
            ChatWsMessage requestEnveloped = new ChatWsMessage()
            {
                jsonrpc = JsonRpcVer,
                id = messageId,
                method = method
            };
            
            string requestJsonString = JsonConvert.SerializeObject(requestEnveloped, Formatting.None, 
                skipNullSerializer);
            
            return requestJsonString;
        }

        private void SendRequest<T, U>(ChatMessageMethod method, T request, ResultCallback<U> callback)
        {
            string messageId = GenerateMessageId().ToString();

            Action<ErrorCode, string> responseHandler = ((errorCode, message) =>
            {
                Result<U> result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result<U>.CreateError(errorCode);
                }
                else
                {
                    ChatWsMessageResponse<U> responsePayload = 
                        JsonConvert.DeserializeObject<ChatWsMessageResponse<U>>(message);
                    if (responsePayload == null)
                    {
                        AccelByteDebug.Log("Chat failed to deserialize response payload from message\n" + message);
                        return;
                    }
                    errorCode = responsePayload.error == null ? ErrorCode.None : responsePayload.error.Code;

                    result = errorCode != ErrorCode.None
                        ? Result<U>.CreateError(errorCode)
                        : Result<U>.CreateOk(responsePayload.result);
                }

                coroutineRunner.Run(() => callback.Try(result));
            });
            
            responseCallbacks[messageId] = responseHandler;
            
            SendMessage(CreateRequestMessage(messageId, method, request));
        }
        
        private void SendRequest<T>(ChatMessageMethod method, T request, ResultCallback callback)
        {
            string messageId = GenerateMessageId().ToString();

            Action<ErrorCode, string> responseHandler = ((errorCode, message) =>
            {
                Result result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result.CreateError(errorCode);
                }
                else
                {
                    ChatWsMessage responsePayload = JsonConvert.DeserializeObject<ChatWsMessage>(message);
                    if (responsePayload == null)
                    {
                        AccelByteDebug.Log("Chat failed to deserialize response payload from message\n" + message);
                        return;
                    }
                    errorCode = responsePayload.error == null ? ErrorCode.None : responsePayload.error.Code;

                    result = errorCode != ErrorCode.None
                        ? Result.CreateError(errorCode)
                        : Result.CreateOk();
                }

                coroutineRunner.Run(() => callback.Try(result));
            });
            
            responseCallbacks[messageId] = responseHandler;

            SendMessage(CreateRequestMessage(messageId, method, request));
        }
        
        private void SendRequest<U>(ChatMessageMethod method, ResultCallback<U> callback)
        {
            string messageId = GenerateMessageId().ToString();

            Action<ErrorCode, string> responseHandler = ((errorCode, message) =>
            {
                Result<U> result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result<U>.CreateError(errorCode);
                }
                else
                {
                    ChatWsMessageResponse<U> responsePayload = 
                        JsonConvert.DeserializeObject<ChatWsMessageResponse<U>>(message);
                    if (responsePayload == null)
                    {
                        AccelByteDebug.Log("Chat failed to deserialize response payload from message\n" + message);
                        return;
                    }
                    errorCode = responsePayload.error == null ? ErrorCode.None : responsePayload.error.Code;

                    result = errorCode != ErrorCode.None
                        ? Result<U>.CreateError(errorCode)
                        : Result<U>.CreateOk(responsePayload.result);
                }

                coroutineRunner.Run(() => callback.Try(result));
            });
            
            responseCallbacks[messageId] = responseHandler;
            
            SendMessage(CreateRequestMessage(messageId, method));
        }
        
        private void SendRequest(ChatMessageMethod method, ResultCallback callback)
        {
            string messageId = GenerateMessageId().ToString();

            Action<ErrorCode, string> responseHandler = ((errorCode, message) =>
            {
                Result result;

                if (errorCode != ErrorCode.None)
                {
                    result = Result.CreateError(errorCode);
                }
                else
                {
                    ChatWsMessage responsePayload = JsonConvert.DeserializeObject<ChatWsMessage>(message);
                    if (responsePayload == null)
                    {
                        AccelByteDebug.Log("Chat failed to deserialize response payload from message\n" + message);
                        return;
                    }
                    errorCode = responsePayload.error == null ? ErrorCode.None : responsePayload.error.Code;

                    result = errorCode != ErrorCode.None
                        ? Result.CreateError(errorCode)
                        : Result.CreateOk();
                }

                coroutineRunner.Run(() => callback.Try(result));
            });

            responseCallbacks[messageId] = responseHandler;
            
            SendMessage(CreateRequestMessage(messageId, method));
        }
        #endregion
    }
}