// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System.Collections.Generic;
using System;

namespace Core.Websocket
{
    internal class NativeWebSocketWrapper : IWebSocket
    {
        private NativeWebSocket.WebSocket webSocket;
        internal NativeWebSocket.WebSocket WebSocketOverride;

        public event OnOpenHandler OnOpen;
        public event OnMessageHandler OnMessage;
        public event OnErrorHandler OnError;
        public event OnCloseHandler OnClose;

        private ApiSharedMemory sharedMemory;

        ~NativeWebSocketWrapper()
        {
            OnOpen = null;
            OnMessage = null;
            OnError = null;
            OnClose = null;
        }

        public WsState ReadyState
        {
            get
            {
                if (webSocket == null)
                {
                    return WsState.Closed;
                }

                switch (webSocket.State)
                {
                    case NativeWebSocket.WebSocketState.Open: return WsState.Open;
                    case NativeWebSocket.WebSocketState.Closed: return WsState.Closed;
                    case NativeWebSocket.WebSocketState.Closing: return WsState.Closing;
                    case NativeWebSocket.WebSocketState.Connecting: return WsState.Connecting;
                    default: throw new WebSocketInvalidStateException("Unrecognized websocket ready state.");
                }
            }
        }

        public void SetProxy(string url, string username, string password)
        {
            throw new NotSupportedException("Proxy is not supported");
        }

        public void Connect(string url, string protocols, string sessionId, string entitlementToken = null, ResultCallback callback = null)
        {
            Dictionary<string, string> customHeader = new Dictionary<string, string>()
            {
                { "X-Ab-LobbySessionID", sessionId }
            };

            if (entitlementToken != null)
            {
                customHeader.Add("Entitlement", entitlementToken);
            }

            Connect(url, protocols, customHeader, entitlementToken, callback);
        }

        public void Connect(string url, string protocols, Dictionary<string, string> customHeaders, string entitlementToken = null, ResultCallback callback = null)
        {
            try
            {
                webSocket = string.IsNullOrEmpty(protocols) ? new NativeWebSocket.WebSocket(url, customHeaders) : new NativeWebSocket.WebSocket(url, protocols, customHeaders);

                if (WebSocketOverride != null)
                {
                    webSocket = WebSocketOverride;
                }

                webSocket.OnOpen += () =>
                {
                    OnOpen?.Invoke();
                };

                webSocket.OnMessage += data =>
                {
                    string dataStr = System.Text.Encoding.UTF8.GetString(data);
                    
                    sharedMemory?.ServiceTracker?.OnReceivingWebsocketNotification(dataStr, sharedMemory?.Logger);
                    OnMessage?.Invoke(dataStr);
                };

                webSocket.OnError += errorMessage =>
                {
                    OnError?.Invoke(errorMessage);
                };

                webSocket.OnClose += code =>
                {
                    OnClose?.Invoke((ushort)code);
                };

                AccelByteSDKMain.AddGameUpdateListener(dt =>
                {
#if !UNITY_WEBGL || UNITY_EDITOR
                    webSocket.DispatchMessageQueue();
#endif
                });
            }
            catch (Exception e)
            {
                const string errorMessage = "Websocket cannot be created";
                callback?.TryError(new Error(ErrorCode.InvalidResponse, $"{errorMessage} : {e.Message}"));
                throw new WebSocketUnexpectedException("Websocket cannot be created.", e);
            }

            switch (webSocket.State)
            {
                case NativeWebSocket.WebSocketState.Open:
                        sharedMemory?.Logger?.Log($"Websocket connection to {url} already opened");
                        callback?.TryOk();
                        return;
                case NativeWebSocket.WebSocketState.Connecting:
                        sharedMemory?.Logger?.LogVerbose($"Websocket connection to {url} is connecting");
                        callback?.TryOk();
                        break;
                case NativeWebSocket.WebSocketState.Closing:
                    {
                        const string errorMessage = "WebSocket is closing";
                        callback?.TryError(new Error(ErrorCode.InvalidResponse, $"{errorMessage}."));
                        throw new WebSocketInvalidStateException($"{errorMessage}.");
                    }
                case NativeWebSocket.WebSocketState.Closed:
                default:
                    {
                        const string errorMessage = "Websocket failed to connect";
                        NativeWebSocket.WebSocketOpenEventHandler onConnectSuccess = () =>
                        {
                            callback?.TryOk();
                        };

                        NativeWebSocket.WebSocketCloseEventHandler onConnectClosed = result =>
                        {
                            callback?.TryError(new Error(ErrorCode.InvalidResponse, $"{errorMessage}. close code: {result}"));
                        };

                        NativeWebSocket.WebSocketErrorEventHandler onConnectError = result =>
                        {
                            callback?.TryError(new Error(ErrorCode.InvalidResponse, $"{errorMessage}. error message: {result}"));
                        };

                        try
                        {
                            webSocket.OnOpen += onConnectSuccess;
                            webSocket.OnClose += onConnectClosed;
                            webSocket.OnError += onConnectError;

                            webSocket.Connect();
                        }
                        catch (Exception e)
                        {
                            webSocket.OnOpen -= onConnectSuccess;
                            webSocket.OnClose -= onConnectClosed;
                            webSocket.OnError -= onConnectError;

                            callback?.TryError(new Error(ErrorCode.InvalidResponse, $"{errorMessage} : {e.Message}"));
                            throw new WebSocketUnexpectedException($"{errorMessage}.", e);
                        }
                        webSocket.OnOpen -= onConnectSuccess;
                        webSocket.OnClose -= onConnectClosed;
                        webSocket.OnError -= onConnectError;
                        break;
                    }
            }
        }

        public void Send(string message)
        {
            // Check state
            if (webSocket.State != NativeWebSocket.WebSocketState.Open)
            {
                throw new WebSocketInvalidStateException("Websocket is not open.");
            }

            try
            {
                sharedMemory?.ServiceTracker?.OnSendingWebsocketRequest(message, sharedMemory?.Logger);
                webSocket.SendText(message);
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Unexpected websocket exception.", e);
            }
        }

        public void Ping()
        {
            webSocket.SendText("");
        }

        public void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            if (webSocket.State == NativeWebSocket.WebSocketState.Closing ||
                webSocket.State == NativeWebSocket.WebSocketState.Closed)
            {
                return;
            }

            try
            {
                webSocket.Close();
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Failed to close the connection.", e);
            }
        }

        void IWebSocket.SetSharedMemory(ApiSharedMemory sharedMemory)
        {
            this.sharedMemory = sharedMemory;
        }
    }
}