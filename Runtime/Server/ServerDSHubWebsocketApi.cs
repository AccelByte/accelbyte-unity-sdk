// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Api;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerDSHubWebsocketApi
    {
        #region Events

        public event OnOpenHandler OnOpen;

        public event OnCloseHandler OnClose;

        public event OnMessageHandler OnMessage;

        public event OnErrorHandler OnError;

        #endregion

        #region Properties

        public bool IsConnected => GetCurrentWebsocket()?.State is WsState.Open;

        #endregion

        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;

        private readonly string websocketUrl;
        private AccelByteWebSocket webSocket;

        public ServerDSHubWebsocketApi(CoroutineRunner inCoroutineRunner, string inWebsocketUrl, ISession inSession)
        {
            Assert.IsNotNull(inCoroutineRunner);

            coroutineRunner = inCoroutineRunner;
            websocketUrl = inWebsocketUrl.Replace("https://", "wss://");
            session = inSession;
        }

        private void CreateWebsocket(string serverName)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {session.AuthorizationToken}"},
                { "X-Ab-ServerID", serverName }
            };

            SetCurrentWebsocket(new HybridWebSocket.WebSocket());

            GetCurrentWebsocket().OnOpen += () =>
            {
                OnOpen?.Invoke();
            };

            GetCurrentWebsocket().OnClose += code =>
            {
                OnClose?.Invoke((ushort)code);
            };

            GetCurrentWebsocket().OnError += msg =>
            {
                OnError?.Invoke(msg);
            };

            GetCurrentWebsocket().OnMessage += data =>
            {
                OnMessage?.Invoke(data);
            };

            GetCurrentWebsocket().Connect(websocketUrl, "", headers);
        }

        public void Connect(string serverName)
        {
            if (!session.IsValid())
            {
                throw new Exception("Cannot connect to websocket because server is not authenticated.");
            }

            try
            {
                if (GetCurrentWebsocket() != null)
                {
                    switch (GetCurrentWebsocket().State)
                    {
                        case WsState.Open:
                            AccelByteDebug.LogWarning("[Server DS Hub] Websocket is connected");
                            return;
                        case WsState.Connecting:
                            AccelByteDebug.LogWarning("[Server DS Hub] Websocket is connecting");
                            return;
                        case WsState.Closing:
                        case WsState.Closed:
                            SetCurrentWebsocket(null);
                            break;
                        default:
                            throw new NativeWebSocket.WebSocketInvalidStateException("[Server DS Hub] Websocket in invalid state.");
                    }
                }

                CreateWebsocket(serverName);
            }
            catch (Exception e)
            {
                throw new NativeWebSocket.WebSocketUnexpectedException("[Server DS Hub] Websocket failed to connect.", e);
            }
        }

        public void Disconnect(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            if (GetCurrentWebsocket().State is WsState.Closing || GetCurrentWebsocket().State is WsState.Closed)
            {
                return;
            }

            try
            {
                GetCurrentWebsocket().Disconnect();
            }
            catch (Exception e)
            {
                throw new NativeWebSocket.WebSocketUnexpectedException("Failed to close the connection.", e);
            }

            SetCurrentWebsocket(null);
        }

        public void HandleNotification<T>(T payload, ResultCallback<T> handler) where T : class, new()
        {
            Report.GetServerWebSocketNotification(payload.ToJsonString());
            
            if (handler == null)
            {
                return;
            }

            coroutineRunner.Run(() => handler(Result<T>.CreateOk(payload)));
        }

        internal virtual void AddOnOpenHandlerListener(OnOpenHandler handleOnOpen)
        {
            OnOpen += handleOnOpen;
        }

        internal virtual void RemoveOnOpenHandlerListener(OnOpenHandler handleOnOpen)
        {
            OnOpen -= handleOnOpen;
        }

        internal virtual void AddOnCloseHandlerListener(OnCloseHandler handleOnClose)
        {
            OnClose += handleOnClose;
        }

        internal virtual void RemoveOnCloseHandlerListener(OnCloseHandler handleOnClose)
        {
            OnClose -= handleOnClose;
        }

        internal virtual void AddOnMessageHandlerListener(OnMessageHandler handleOnMessage)
        {
            OnMessage += handleOnMessage;
        }

        internal virtual void RemoveOnMessageHandlerListener(OnMessageHandler handleOnMessage)
        {
            OnMessage -= handleOnMessage;
        }

        internal virtual void AddOnErrorHandlerListener(OnErrorHandler handleOnError)
        {
            OnError += handleOnError;
        }

        internal virtual void RemoveOnErrorHandlerListener(OnErrorHandler handleOnError)
        {
            OnError -= handleOnError;
        }

        internal virtual AccelByteWebSocket GetCurrentWebsocket()
        {
            return this.webSocket;
        }

        internal virtual void SetCurrentWebsocket(IWebSocket inWs)
        {
            if (inWs != null)
            {
                this.webSocket = new AccelByteWebSocket(inWs);
            }
            else
            {
                this.webSocket = null;
            }
        }
    }
}