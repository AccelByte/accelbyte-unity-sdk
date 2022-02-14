// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using System.Collections;
using UnityEngine;

namespace AccelByte.Api
{
    class AccelByteWebSocket
    {
        private float pingDelay = 4000;
        private int totalTimeout = 60000;
        private int backoffDelay = 1000;
        private int maxDelay = 30000;
        private bool reconnectOnClose = false;

        private IWebSocket webSocket;
        private string webSocketUrl;
        private string authorizationToken;
        private string sessionId;
        private CoroutineRunner coroutineRunner;
        private Coroutine maintainConnectionCoroutine;
        private WsCloseCode closeCodeCurrent = WsCloseCode.NotSet;
        private ITokenGenerator tokenGenerator;

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
            coroutineRunner.Run(() =>
            {
                OnOpen?.Invoke();
            });
        }

        private void OnCloseReceived(ushort closeCode)
        {
            coroutineRunner.Run(() =>
            {
                if (!isReconnectable((WsCloseCode)closeCode))
                {
                    StopMaintainConnection();
                }

                OnClose?.Invoke(closeCode);
            });
        }

        private void OnErrorReceived(string errorMessage)
        {
            coroutineRunner.Run(() =>
            {
                OnError?.Invoke(errorMessage);
            });
        }

        private void OnMessageReceived(string message)
        {
            coroutineRunner.Run(() =>
            {
                OnMessage(message);
            });
        }

        private void OnTokenReceived(string token)
        {
             this.webSocket.Connect(this.webSocketUrl, this.authorizationToken, this.sessionId, token);
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

        public AccelByteWebSocket(IWebSocket webSocket, CoroutineRunner coroutineRunner)
        {
            this.closeCodeCurrent = WsCloseCode.NotSet;

            this.webSocket = webSocket;
            this.coroutineRunner = coroutineRunner;

            this.webSocket.OnOpen += OnOpenReceived;
            this.webSocket.OnMessage += OnMessageReceived;
            this.webSocket.OnError += OnErrorReceived;
            this.webSocket.OnClose += OnCloseReceived;
        }

        /// <summary>
        /// TokenGenerator is used for generate access token when connecting to lobby. 
        /// If token generator is not specified, no token will be used when connecting to lobby.
        /// For entitlement token verification, use EntitlementTokenGenerator class on the parameter.
        /// </summary>
        /// <param name="tokenGenerator"> Token generator for connecting lobby. </param>
        public void SetConnectionTokenGenerator(ITokenGenerator tokenGenerator)
        {
            if(maintainConnectionCoroutine != null)
            {
                AccelByteDebug.LogWarning("Can't set connection token generator! Lobby is already connected.");
                return;
            }

            this.tokenGenerator = tokenGenerator;
            this.tokenGenerator.TokenReceivedEvent += OnTokenReceived;
        }

        private bool isReconnectable(WsCloseCode code)
        {

            switch (code)
            {
                case WsCloseCode.Abnormal:
                case WsCloseCode.ServerError:
                case WsCloseCode.ServiceRestart:
                case WsCloseCode.TryAgainLater:
                case WsCloseCode.TlsHandshakeFailure: return true;
                default: return false;
            }
        }

        /// <summary>
        /// Establish websocket connection
        /// </summary>
        /// <param name="url">url of the websocket server</param>
        /// <param name="authorizationToken">user authorization token</param>
        /// <param name="sessionId">session id header</param>
        public void Connect(string url, string authorizationToken, string sessionId = "")
        {
            if(State == WsState.Connecting || State == WsState.Open)
            {
                AccelByteDebug.Log("[AccelByteWebSocket] is connecting or already connected");
                return;
            }

            if (this.tokenGenerator != null)
            {
                if (!this.tokenGenerator.IsValid())
                {
                    this.tokenGenerator.RequestToken();

                    return;
                }
            }

            webSocketUrl = url;
            this.authorizationToken = authorizationToken;
            webSocket.Connect(url, this.authorizationToken, sessionId, this.tokenGenerator?.Token);

            // check status after connect, only maintain connection when close code is reconnectable
            if (this.closeCodeCurrent == WsCloseCode.NotSet || isReconnectable(this.closeCodeCurrent))
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
        /// <param name="totalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="backoffDelay">Initial delay time</param>
        /// <param name="maxDelay">Maximum delay time</param>
        public void SetRetryParameters(int totalTimeout, int backoffDelay, int maxDelay)
        {
            if (maintainConnectionCoroutine != null)
            {
                AccelByteDebug.LogWarning("Can't change retry parameters! Lobby is already connected.");
                return;
            }

            this.totalTimeout = totalTimeout;
            this.backoffDelay = backoffDelay;
            this.maxDelay = maxDelay;
        }

        /// <summary>
        /// Send message to websocket server
        /// </summary>
        /// <param name="message">message to be sent</param>
        public void Send(string message)
        {
            webSocket.Send(message);
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
                        var timeout = TimeSpan.FromSeconds(totalTimeout / 1000f);

                        while (this.reconnectOnClose &&
                            this.webSocket.ReadyState == WsState.Closed &&
                            DateTime.Now - firstClosedTime < timeout)
                        {
                            // debug ws connection
#if DEBUG
                            AccelByteDebug.Log("[WS] Re-Connecting");
#endif
                            if (this.tokenGenerator == null)
                            {
                                this.webSocket.Connect(this.webSocketUrl, this.authorizationToken, this.sessionId);
                            }
                            else
                            {
                                this.tokenGenerator.RequestToken();
                            }

                            float randomizedDelay = (float)(nextDelay + ((rand.NextDouble() * 0.5) - 0.5));
#if DEBUG
                            AccelByteDebug.Log("[WS] Next reconnection in: " + randomizedDelay);
#endif
                            yield return new WaitForSeconds(randomizedDelay / 1000f);

                            nextDelay *= 2;

                            if (nextDelay > maxDelay)
                            {
                                nextDelay = maxDelay;
                            }
                        }

                        if (this.webSocket.ReadyState == WsState.Closed)
                        {
                            this.OnRetryAttemptFailed?.Invoke(this, EventArgs.Empty);

                            yield break;
                        }

                        break;
                }
            }
        }

        private void StartMaintainConnection()
        {
            if(maintainConnectionCoroutine != null)
            {
                StopMaintainConnection();
            }

            this.reconnectOnClose = true;
            this.maintainConnectionCoroutine = this.coroutineRunner.Run(MaintainConnection(backoffDelay, maxDelay, totalTimeout));
        }

        private void StopMaintainConnection()
        {
            if (maintainConnectionCoroutine == null)
                return;

            this.reconnectOnClose = false;
            coroutineRunner.Stop(this.maintainConnectionCoroutine);
            this.maintainConnectionCoroutine = null;
        }
    }
}
