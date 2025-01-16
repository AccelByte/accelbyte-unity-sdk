// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Utils;
using System;
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
        internal const string GameClientVersionHeaderName = "Game-Client-Version";
        internal const string SDKVersionHeaderName = "AccelByte-SDK-Version";

        internal int PingDelay = 4000;
        internal int TotalTimeout = 60000;
        internal int BackoffDelay = 1000;
        internal int MaxDelay = 30000;

        internal IWebSocket WebSocketImp;
        protected string webSocketUrl;
        protected string authorizationToken;
        
        protected WsCloseCode closeCodeCurrent = WsCloseCode.NotSet;
        protected ITokenGenerator tokenGenerator;
        private WebstocketMaintainer maintainer;
        private IDebugger logger;

        public Dictionary<string, string> ReconnectCustomHeaders { get; set; }

        /// <summary>
        /// Raised prior websocket reconnect
        /// </summary>
        internal event Action OnPreReconnectAction;

        /// <summary>
        /// Raised when websocket connection successfully connected
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
             this.WebSocketImp.Connect(this.webSocketUrl, this.authorizationToken, ReconnectCustomHeaders, token);
        }

        /// <summary>
        /// Get websocket connection state
        /// </summary>
        public virtual WsState State
        {
            get
            {
                return WebSocketImp.ReadyState;
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

        public AccelByteWebSocket(IWebSocket webSocketImp, int connectionTimeoutMs = 60000)
        {
            if(webSocketImp == null)
            {
                throw new System.InvalidOperationException("Assigning null websocket");
            }
            this.closeCodeCurrent = WsCloseCode.NotSet;

            this.WebSocketImp = webSocketImp;

            this.WebSocketImp.OnOpen += OnOpenReceived;
            this.WebSocketImp.OnMessage += OnMessageReceived;
            this.WebSocketImp.OnError += OnErrorReceived;
            this.WebSocketImp.OnClose += OnCloseReceived;

            TotalTimeout = connectionTimeoutMs;

            ReconnectCustomHeaders = new Dictionary<string, string>();
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
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
                logger?.LogWarning("Can't set connection token generator! Lobby is already connected.");
                return;
            }

            this.tokenGenerator = tokenGenerator;
            this.tokenGenerator.TokenReceivedEvent += OnTokenReceived;
        }

        private bool IsReconnectable(WsCloseCode code)
        {
            return code <= WsCloseCode.ServerShuttingDown;
        }

        private Dictionary<string, string> ApplyAdditionalData(Dictionary<string, string> header)
        {
            var retval = new Dictionary<string, string>(header);
            retval[SDKVersionHeaderName] = AccelByteSDK.Version;
#if !UNITY_SERVER
            retval[GameClientVersionHeaderName] = Application.version;
#endif
            return retval;
        }

        /// <summary>
        /// Establish websocket connection
        /// </summary>
        /// <param name="url">url of the websocket server</param>
        /// <param name="authorizationToken">user authorization token</param>
        /// <param name="sessionId">Lobby's session id header</param>
        /// <param name="callback">A callback that will be triggered when error happens during connect</param>
        public virtual void Connect(string url, string authorizationToken, string sessionId = "", ResultCallback callback = null)
        {
            Dictionary<string, string> customHeader = new Dictionary<string, string>()
            {
                { LobbySessionIdHeaderName, sessionId }
            };
            
            Connect(url, authorizationToken, customHeader, callback);
        }

        /// <summary>
        /// Establish websocket connection through assigned IWebsocket
        /// </summary>
        /// <param name="url">url of the websocket server</param>
        /// <param name="authorizationToken">user authorization token</param>
        /// <param name="customHeaders">Additional header to be sent</param>
        /// <param name="callback">a callback that will be triggered when Connect is done</param>
        public virtual void Connect(string url, string authorizationToken, Dictionary<string, string> customHeaders, ResultCallback callback = null)
        {
            if(State == WsState.Connecting || State == WsState.Open)
            {
                logger?.LogVerbose("[AccelByteWebSocket] is connecting or already connected");
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

            customHeaders = ApplyAdditionalData(customHeaders);

            OnRetryAttemptFailed += (sender, e) =>
            {
                callback?.TryError(new Error(ErrorCode.InvalidResponse, "Reconnect attempts failed"));
            };

            logger?.LogVerbose($"Connecting websocket to {url}");
            WebSocketImp.Connect(url: url, 
                protocols: this.authorizationToken, 
                customHeaders: customHeaders, 
                entitlementToken: string.Empty,
                callback: result =>
                {
                    if (!result.IsError)
                    {
                        callback?.TryOk();
                    }
                });

            // check status after connect, only maintain connection when close code is reconnectable
            if (this.closeCodeCurrent == WsCloseCode.NotSet || IsReconnectable(this.closeCodeCurrent))
            {
                StartMaintainConnection();
            }
        }

        /// <summary>
        /// Disconnect websocket
        /// </summary>
        public virtual void Disconnect()
        {
            StopMaintainConnection();

            if (this.IsConnected || this.IsConnecting)
            {
                this.WebSocketImp.Close();
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
                logger?.LogWarning("Can't change retry parameters! Lobby is already connected.");
                return;
            }

            this.TotalTimeout = totalTimeout;
            this.BackoffDelay = backoffDelay;
            this.MaxDelay = maxDelay;
            this.PingDelay = pingDelay;
        }

        /// <summary>
        /// Send message to websocket server
        /// </summary>
        /// <param name="message">message to be sent</param>
        public virtual async void Send(string message)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var maintainerBehaviour = GetOrCreateMaintainerBehaviour();
            maintainerBehaviour.StartCoroutine(
                RetryBackoffUtils.RunCoroutine(WebsocketSend(message), logger: logger));
#else
            await RetryBackoffUtils.Run<int>(() => WebsocketSend(message), logger: logger);
#endif
        }

        private Task<int> WebsocketSend(string message)
        {
            int errorCode = 0;
            try
            {
                WebSocketImp.Send(message);
            }
            catch (Exception e)
            {
                logger?.LogWarning($"Sending failed with error : {e.Message}");
                errorCode = 1;
            }
            return Task.FromResult(errorCode);
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
                ReconnectCustomHeaders = ApplyAdditionalData(ReconnectCustomHeaders);

                if (this.tokenGenerator == null)
                {
                    WebSocketImp.Connect(webSocketUrl, authorizationToken, ReconnectCustomHeaders);
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

            maintainer = new WebstocketMaintainer(ref WebSocketImp, ref PingDelay, ref BackoffDelay, ref MaxDelay, ref TotalTimeout, reconnectOnClose, logger, OnPreReconnectAction, reconnectAction, onReconnectFailed);
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

        private static DummyBehaviour GetOrCreateMaintainerBehaviour()
        {
            GameObject sdkGameObject = AccelByteGameObject.GetOrCreateGameObject();

            var maintainerBehaviour = sdkGameObject.GetComponent<DummyBehaviour>();
            if (maintainerBehaviour == null)
            {
                maintainerBehaviour = sdkGameObject.AddComponent<DummyBehaviour>();
            }

            return maintainerBehaviour;
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
            private int currentRetryAttempt;
            private int maxReconnectRetries;
            private IDebugger logger;

            public WebstocketMaintainer(ref IWebSocket webSocket, ref int pingDelay, ref int backoffDelay, ref int maxDelay, ref int totalTimeout, bool reconnectOnClose, IDebugger logger, System.Action setupReconnectAction, System.Action reconnectAction, System.Action onReconnectFailed, int maxReconnectRetries = 5)
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
                this.maxReconnectRetries = maxReconnectRetries;
                this.logger = logger;
                currentRetryAttempt = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
                var maintainerBehaviour = GetOrCreateMaintainerBehaviour();
                maintainerBehaviour.StartCoroutine(MaintainLoop());
#else
                MaintainLoop();
#endif
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

#if UNITY_WEBGL && !UNITY_EDITOR
            System.Collections.IEnumerator MaintainLoop()
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

                            yield return new WaitForSeconds(pingDelay / 1000f);

                            break;
                        case WsState.Connecting:
                            while (webSocket.ReadyState == WsState.Connecting)
                            {
                                yield return new WaitForSeconds(1);
                            }

                            break;
                        case WsState.Closing:
                            while (webSocket.ReadyState == WsState.Closing)
                            {
                                yield return new WaitForSeconds(1);
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
                                logger?.LogVerbose("[WS] Re-Connecting");
#endif
                                setupReconnectAction?.Invoke();

                                reconnectAction?.Invoke();

                                var randomizedDelay = Mathf.RoundToInt((float)(nextDelay + ((rand.NextDouble() * 0.5) - 0.5)));
#if DEBUG
                                logger?.LogVerbose("[WS] Next reconnection in: " + randomizedDelay);
#endif
                                yield return new WaitForSeconds(randomizedDelay / 1000f);

                                nextDelay *= 2;

                                if (nextDelay > maxDelay)
                                {
                                    nextDelay = maxDelay;
                                }

                                currentRetryAttempt++;
#if DEBUG
                                logger?.LogVerbose("[WS] Current retry attempt: " + currentRetryAttempt);
#endif
                            }

                            if ((reconnectOnClose && webSocket.ReadyState == WsState.Closed) || currentRetryAttempt >= maxReconnectRetries)
                            {
                                onReconnectFailed?.Invoke();
                                yield break;
                            }

                            break;
                    }
                }
            }
#else
            async void MaintainLoop()
            {
                const int pollInterval = 1000;

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
                                await Task.Delay(pollInterval);
                            }

                            break;
                        case WsState.Closing:
                            while (webSocket.ReadyState == WsState.Closing)
                            {
                                await Task.Delay(pollInterval);
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
                                logger?.LogVerbose("[WS] Re-Connecting");
#endif
                                setupReconnectAction?.Invoke();

                                reconnectAction?.Invoke();

                                var randomizedDelay = Mathf.RoundToInt((float)(nextDelay + ((rand.NextDouble() * 0.5) - 0.5)));
#if DEBUG
                                logger?.LogVerbose("[WS] Next reconnection in: " + randomizedDelay);
#endif
                                await Task.Delay(randomizedDelay);

                                nextDelay *= 2;

                                if (nextDelay > maxDelay)
                                {
                                    nextDelay = maxDelay;
                                }

                                currentRetryAttempt++;
#if DEBUG
                                logger?.LogVerbose("[WS] Current retry attempt: " + currentRetryAttempt);
#endif
                            }

                            if ((reconnectOnClose && webSocket.ReadyState == WsState.Closed) || currentRetryAttempt >= maxReconnectRetries)
                            {
                                onReconnectFailed?.Invoke();
                                return;
                            }

                            break;
                    }
                }
            }
#endif
        }
    }
}
