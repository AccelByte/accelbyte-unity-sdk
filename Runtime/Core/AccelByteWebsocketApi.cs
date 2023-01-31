// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;

namespace Core
{
    public class AccelByteWebsocketApi
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
        
        #region internal properties
        public string SessionId { get; set; }
        #endregion

        #endregion public events declaration
        
        #region private properties
        protected string websocketUrl;
        protected UserSession session;
        protected CoroutineRunner coroutineRunner;
        protected AccelByteWebSocket webSocket;
        protected string namespace_;
        
        protected readonly object syncToken = new object();
        private long messageId;
        #endregion
        
        #region constructor
        internal AccelByteWebsocketApi(CoroutineRunner inCoroutineRunner
            , UserSession inSession
            , IWebSocket inWebSocket
            , string inWebsocketUrl
            , string inNamespace)
        {
            coroutineRunner = inCoroutineRunner;
            session = inSession;
            websocketUrl = inWebsocketUrl;
            namespace_ = inNamespace;

            OverrideWebsocket(inWebSocket);
        }
        #endregion
        
        #region public method
        
        public virtual void OverrideWebsocket(IWebSocket inWebSocket
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

        public virtual void Connect()
        {
            webSocket.Connect(websocketUrl, session.AuthorizationToken);
        }

        public virtual void Connect(Dictionary<string, string> customHeader)
        {
            webSocket.Connect(websocketUrl, session.AuthorizationToken, customHeader);
        }

        public virtual void Disconnect()
        {
            webSocket.Disconnect();
        }

        public virtual void SendMessage(string message)
        {
            webSocket.Send(message);
        }
        #endregion
        
        #region protected methods

        protected void SetOnMessageHandler(OnMessageHandler OnMessage)
        {
            this.OnMessage = OnMessage;
        }

        protected virtual void HandleOnOpen()
        {
            coroutineRunner.Run( () => this.OnOpen?.Invoke());
        }

        protected virtual void HandleOnClose(ushort closeCode)
        {
            coroutineRunner.Run( () => this.OnClose?.Invoke(closeCode));
        }

        protected virtual void HandleOnMessage(string message)
        {
            coroutineRunner.Run( () => this.OnMessage?.Invoke(message));
        }

        protected virtual void HandleOnError(string errorMsg)
        {
            coroutineRunner.Run( () => this.OnError?.Invoke(errorMsg));
        }
        
        protected virtual long GenerateMessageId()
        {
            lock (syncToken)
            {
                if (messageId < Int64.MaxValue)
                    messageId++;
                else
                    messageId = 0;
            }

            return messageId;
        }

        #endregion private methods
    }
}