// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AccelByte.Api
{
    public class AccelByteWebSocket
    {
        internal const string LobbySessionIdHeaderName = "X-Ab-LobbySessionID";
        internal const string PlatformIdHeaderName = "X-Ab-Platform";
        internal const string PlatformUserIdHeaderName = "X-Ab-Platform-User-Id";

        protected int pingDelay = 4000;
        protected int totalTimeout = 60000;
        protected int backoffDelay = 1000;
        protected int maxDelay = 30000;

        protected IWebSocket webSocket;
        protected string webSocketUrl;
        protected string authorizationToken;
        
        protected WsCloseCode closeCodeCurrent = WsCloseCode.NotSet;
        protected ITokenGenerator tokenGenerator;
        private WebstocketMaintainer maintainer;

        public Dictionary<string, string> ReconnectCustomHeaders { get; set; }

        /// <summary>
        /// Raised prior websocket reconnect
        /// </summary>
        internal event Action OnPreReconnectAction;

        /// <summary>
        /// Raised when websocket connection succesfully connected
        /// </summary>
        public event OnOpenHandler OnOpen;

        /// <summary>
        /// Raised when websocket connection successfully closed
        /// </summary>
        public event OnCloseHandler OnClose;

        /// <summary>
        /// Raised when websocket received a message
        /// </summary>
        public event OnMessageHandler OnMessage;

        /// <summary>
        /// Raised when websocket received an error
        /// </summary>
        public event OnErrorHandler OnError;

        /// <summary>
        /// Raised when websocket failed an attempt to reconnect.
        /// </summary>
        public event EventHandler OnRetryAttemptFailed;

        private void OnOpenReceived()
        {
            OnOpen?.Invoke();
        }

        private void OnCloseReceived(ushort closeCode)
        {
            if (!IsReconnectable((WsCloseCode)closeCode))
            {
                StopMaintainConnection();
            }

            OnClose?.Invoke(closeCode);
        }

        private void OnErrorReceived(string errorMessage)
        {
            OnError?.Invoke(errorMessage);
        }

        private void OnMessageReceived(string message)
        {
            OnMessage?.Invoke(message);
        }

        private void OnTokenReceived(string token)
        {
             this.webSocket.Connect(this.webSocketUrl, this.authorizationToken, ReconnectCustomHeaders, token);
        }

        /// <summary>
        /// Get websocket connection state
        /// </summary>
        public WsState State
        {
            get
            {
                return webSocket.ReadyState;
            }
        }

        /// <summary>
        /// Check websocket is in open state
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return State == WsState.Open;
            }
        }

        /// <summary>
        /// Check websocket is in connecting state
        /// </summary>
        public bool IsConnecting
        {
            get
            {
                return State == WsState.Connecting;
            }
        }

        public AccelByteWebSocket(IWebSocket webSocket)
        {
            if(webSocket == null)
            {
                throw new System.InvalidOperationException("Assigning null websocket");
            }
            this.closeCodeCurrent = WsCloseCode.NotSet;

            this.webSocket = webSocket;

            this.webSocket.OnOpen += OnOpenReceived;
            this.webSocket.OnMessage += OnMessageReceived;
            this.webSocket.OnError += OnErrorReceived;
            this.webSocket.OnClose += OnCloseReceived;

            ReconnectCustomHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// TokenGenerator is used for generate access token when connecting to lobby. 
        /// If token generator is not specified, no token will be used when connecting to lobby.
        /// For entitlement token verification, use EntitlementTokenGenerator class on the parameter.
        /// </summary>
        /// <param name="tokenGenerator"> Token generator for connecting lobby. </param>
        public void SetConnectionTokenGenerator(ITokenGenerator tokenGenerator)
        {
            if(maintainer != null)
            {
                AccelByteDebug.LogWarning("Can't set connection token generator! Lobby is already connected.");
                return;
            }

            this.tokenGenerator = tokenGenerator;
            this.tokenGenerator.TokenReceivedEvent += OnTokenReceived;
        }

        private bool IsReconnectable(WsCloseCode code)
        {

            switch (code)
            {
                case WsCloseCode.Abnormal:
                case WsCloseCode.ServerError:
                case WsCloseCode.ServiceRestart:
                case WsCloseCode.TryAgainLater:
                case WsCloseCode.TlsHandshakeFailure:
                case WsCloseCode.ServerShuttingDown:  
                    return true;
                
                default: return false;
            }
        }

        /// <summary>
        /// Establish websocket connection
        /// </summary>
        /// <param name="url">url of the websocket server</param>
        /// <param name="authorizationToken">user authorization token</param>
        /// <param name="sessionId">Lobby's session id header</param>
        public void Connect(string url, string authorizationToken, string sessionId = "")
        {
            Dictionary<string, string> customHeader = new Dictionary<string, string>()
            {
                { LobbySessionIdHeaderName, sessionId }
            };
            
            Connect(url, authorizationToken, customHeader);
        }

        public void Connect(string url, string authorizationToken, Dictionary<string, string> customHeaders)
        {
            if(State == WsState.Connecting || State == WsState.Open)
            {
                AccelByteDebug.LogVerbose("[AccelByteWebSocket] is connecting or already connected");
                return;
            }

            if (tokenGenerator != null)
            {
                if (!tokenGenerator.IsValid())
                {
                    tokenGenerator.RequestToken();

                    return;
                }
            }

            webSocketUrl = url;
            this.authorizationToken = authorizationToken;

            if (tokenGenerator != null)
            {
                customHeaders["Entitlement"] = tokenGenerator.Token;
            }

            AccelByteDebug.LogVerbose($"Connecting websocket to {url}");
            webSocket.Connect(url, this.authorizationToken, customHeaders);

            // check status after connect, only maintain connection when close code is reconnectable
            if (this.closeCodeCurrent == WsCloseCode.NotSet || IsReconnectable(this.closeCodeCurrent))
            {
                StartMaintainConnection();
            }
        }

        /// <summary>
        /// Disconnect websocket
        /// </summary>
        public void Disconnect()
        {
            StopMaintainConnection();

            if (this.IsConnected || this.IsConnecting)
            {
                this.webSocket.Close();
            }
        }

        /// <summary>
        /// Change the delay parameters to maintain connection in the websocket.
        /// </summary>
        /// <param name="totalTimeout">Time limit until stop to re-attempt (ms)</param>
        /// <param name="backoffDelay">Initial delay time (ms)</param>
        /// <param name="maxDelay">Maximum delay time (ms)</param>
        public void SetRetryParameters(int totalTimeout, int backoffDelay, int maxDelay, int pingDelay = 4000)
        {
            if (maintainer != null)
            {
                AccelByteDebug.LogWarning("Can't change retry parameters! Lobby is already connected.");
                return;
            }

            this.totalTimeout = totalTimeout;
            this.backoffDelay = backoffDelay;
            this.maxDelay = maxDelay;
            this.pingDelay = pingDelay;
        }

        /// <summary>
        /// Send message to websocket server
        /// </summary>
        /// <param name="message">message to be sent</param>
        public void Send(string message)
        {
            webSocket.Send(message);
        }

        private void StartMaintainConnection()
        {
            if(maintainer != null)
            {
                StopMaintainConnection();
            }

            const bool reconnectOnClose = true;
            System.Action reconnectAction = () =>
            {
                if (this.tokenGenerator == null)
                {
                    webSocket.Connect(webSocketUrl, authorizationToken, ReconnectCustomHeaders);
                }
                else
                {
                    tokenGenerator.RequestToken();
                }
            };
            System.Action onReconnectFailed = () =>
            {
                maintainer = null;
                OnRetryAttemptFailed?.Invoke(this, EventArgs.Empty);
            };

            maintainer = new WebstocketMaintainer(ref webSocket, ref pingDelay, ref backoffDelay, ref maxDelay, ref totalTimeout, reconnectOnClose, OnPreReconnectAction, reconnectAction, onReconnectFailed);
        }

        private void StopMaintainConnection()
        {
            if (maintainer == null)
            {
                return;
            }

            maintainer.ShutDown();
            maintainer = null;
        }

        /// <summary>
        /// Retrying connection with exponential backoff if disconnected, ping if connected
        /// </summary>
        private class WebstocketMaintainer
        {
            private readonly bool reconnectOnClose;
            private Action setupReconnectAction;
            private Action reconnectAction;
            private Action onReconnectFailed;
            private readonly IWebSocket webSocket;
            private readonly int pingDelay;
            private readonly int backoffDelay;
            private readonly int maxDelay;
            private readonly int totalTimeout;
            bool isMaintaining = false;

            public WebstocketMaintainer(ref IWebSocket webSocket, ref int pingDelay, ref int backoffDelay, ref int maxDelay, ref int totalTimeout, bool reconnectOnClose, System.Action setupReconnectAction, System.Action reconnectAction, System.Action onReconnectFailed)
            {
                this.webSocket = webSocket;
                this.pingDelay = pingDelay;
                this.backoffDelay = backoffDelay;
                this.maxDelay = maxDelay;
                this.totalTimeout = totalTimeout;
                this.setupReconnectAction = setupReconnectAction;
                this.reconnectOnClose = reconnectOnClose;
                this.reconnectAction = reconnectAction;
                this.onReconnectFailed = onReconnectFailed;
                MaintainLoop();
            }

            ~WebstocketMaintainer()
            {
                ShutDown();
            }

            public void ShutDown()
            {
                reconnectAction = null;
                onReconnectFailed = null;
                isMaintaining = false;
            }

            async void MaintainLoop()
            {
                isMaintaining = true;
                while (isMaintaining)
                {
                    switch (webSocket.ReadyState)
                    {
                        case WsState.New:
                            break;
                        case WsState.Open:
                            webSocket.Ping();

                            await Task.Delay(pingDelay);

                            break;
                        case WsState.Connecting:
                            while (webSocket.ReadyState == WsState.Connecting)
                            {
                                await Task.Delay(1000);
                            }

                            break;
                        case WsState.Closing:
                            while (webSocket.ReadyState == WsState.Closing)
                            {
                                await Task.Delay(1000);
                            }

                            break;
                        case WsState.Closed:
                            System.Random rand = new System.Random();
                            int nextDelay = backoffDelay;
                            var firstClosedTime = DateTime.Now;
                            var timeout = TimeSpan.FromSeconds(totalTimeout / 1000f);

                            while (reconnectOnClose &&
                                webSocket.ReadyState == WsState.Closed &&
                                DateTime.Now - firstClosedTime < timeout)
                            {
#if DEBUG
                                AccelByteDebug.LogVerbose("[WS] Re-Connecting");
#endif
                                setupReconnectAction?.Invoke();

                                reconnectAction?.Invoke();

                                var randomizedDelay = Mathf.RoundToInt((float)(nextDelay + ((rand.NextDouble() * 0.5) - 0.5)));
#if DEBUG
                                AccelByteDebug.LogVerbose("[WS] Next reconnection in: " + randomizedDelay);
#endif
                                await Task.Delay(randomizedDelay);

                                nextDelay *= 2;

                                if (nextDelay > maxDelay)
                                {
                                    nextDelay = maxDelay;
                                }
                            }

                            if (reconnectOnClose && webSocket.ReadyState == WsState.Closed)
                            {
                                onReconnectFailed?.Invoke();
                                return;
                            }

                            break;
                    }
                }
            }
        }
    }
}
