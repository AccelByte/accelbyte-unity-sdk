// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using UnityEngine.Assertions;
using WebSocketSharp;

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

        public bool IsConnected => _webSocket?.ReadyState is WebSocketState.Open;

        #endregion

        private readonly CoroutineRunner _coroutineRunner;
        private readonly ISession _session;

        private string _websocketUrl;
        private WebSocket _webSocket;

        public ServerDSHubWebsocketApi(CoroutineRunner inCoroutineRunner, string inWebsocketUrl, ISession inSession)
        {
            Assert.IsNotNull(inCoroutineRunner);

            _coroutineRunner = inCoroutineRunner;
            _websocketUrl = inWebsocketUrl.Replace("https://", "wss://");
            _session = inSession;
        }

        public void CreateWebsocket(string serverName)
        {
            _webSocket = new WebSocket(_websocketUrl, _session.AuthorizationToken);
            _webSocket.SslConfiguration.EnabledSslProtocols =
                System.Security.Authentication.SslProtocols.Default
                | System.Security.Authentication.SslProtocols.Tls11
                | System.Security.Authentication.SslProtocols.Tls12;
            _webSocket.CustomHeaders = new Dictionary<string, string>
            {
                { "X-Ab-ServerID", serverName }
            };

            _webSocket.OnOpen += (sender, ev) =>
            {
                OnOpen?.Invoke();
            };

            _webSocket.OnMessage += (sender, ev) =>
            {
                if (ev.RawData == null) return;

                OnMessage?.Invoke(ev.Data);
            };

            _webSocket.OnError += (sender, ev) => { OnError?.Invoke(ev.Message); };

            _webSocket.OnClose += (sender, ev) => { OnClose?.Invoke(ev.Code); };

            _webSocket.Connect();
        }

        public void Connect(string serverName)
        {
            if (!_session.IsValid())
            {
                throw new Exception("Cannot connect to websocket because server is not authenticated.");
            }

            try
            {
                if (_webSocket != null)
                {
                    switch (_webSocket.ReadyState)
                    {
                        case WebSocketState.Open:
                            AccelByteDebug.LogWarning("[Server DS Hub] Websocket is connected");
                            return;
                        case WebSocketState.Connecting:
                            AccelByteDebug.LogWarning("[Server DS Hub] Websocket is connecting");
                            return;
                        case WebSocketState.Closing:
                        case WebSocketState.Closed:
                            _webSocket = null;
                            break;
                        default:
                            throw new WebSocketInvalidStateException("[Server DS Hub] Websocket in invalid state.");
                    }
                }

                CreateWebsocket(serverName);
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("[Server DS Hub] Websocket failed to connect.", e);
            }
        }

        public void Disconnect(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            if (_webSocket.ReadyState is WebSocketState.Closing || _webSocket.ReadyState is WebSocketState.Closed)
            {
                return;
            }

            try
            {
                _webSocket.Close((ushort)code, reason);
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Failed to close the connection.", e);
            }

            _webSocket = null;
        }

        public void HandleNotification<T>(T payload, ResultCallback<T> handler) where T : class, new()
        {
            Report.GetServerWebSocketNotification(payload.ToJsonString());
            
            if (handler == null)
            {
                return;
            }

            _coroutineRunner.Run(() => handler(Result<T>.CreateOk(payload)));
        }
    }
}