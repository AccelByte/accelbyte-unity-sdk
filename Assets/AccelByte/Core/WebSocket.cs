// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.


using System;
using System.Collections.Generic;
using AccelByte.Core;
#if !UNITY_WEBGL || UNITY_EDITOR
using WebSocketSharp;
#endif

namespace HybridWebSocket
{
#if UNITY_WEBGL && !UNITY_EDITOR
    internal static class JslibInterop
    {
        private delegate void InteropOnOpenHandler(uint instanceId);

        private delegate void InteropOnMessageHandler(uint instanceId, IntPtr messagePtr);

        private delegate void InteropOnErrorHandler(uint instanceId, IntPtr errorMsgPtr);

        private delegate void InteropOnCloseHandler(uint instanceId, ushort closeCode);

        private static Dictionary<uint, OnOpenHandler> onOpenHandlers =  new Dictionary<uint, OnOpenHandler>();
        private static Dictionary<uint, OnMessageHandler> onMessageHandlers = new Dictionary<uint, OnMessageHandler>();
        private static Dictionary<uint, OnErrorHandler> onErrorHandlers = new Dictionary<uint, OnErrorHandler>();
        private static Dictionary<uint, OnCloseHandler> onCloseHandlers = new Dictionary<uint, OnCloseHandler>();

        static JslibInterop()
        {
            JslibInterop.WsSetEventHandlers(
                JslibInterop.WsHandleOnOpen,
                JslibInterop.WsHandleOnMessage,
                JslibInterop.WsHandleOnError,
                JslibInterop.WsHandleOnClose);
        }

        [DllImport("__Internal")]
        private static extern uint WsSetEventHandlers(InteropOnOpenHandler onOpen, InteropOnMessageHandler onMessage,
            InteropOnErrorHandler onError, InteropOnCloseHandler onClose);

        [DllImport("__Internal")] public static extern uint WsCreate(string url, string protocols);

        [DllImport("__Internal")] public static extern uint WsOpen(uint objectId);

        [DllImport("__Internal")] public static extern uint WsClose(uint objectId, int code, string reason);

        [DllImport("__Internal")] public static extern uint WsSend(uint objectId, string message);

        [DllImport("__Internal")] public static extern uint WsDestroy(uint objectId);

        [DllImport("__Internal")] public static extern uint WsGetReadyState(uint objectId);
        
        public static void WsResetEvents(uint objectId)
        {
            JslibInterop.onOpenHandlers.Remove(objectId);
            JslibInterop.onMessageHandlers.Remove(objectId);
            JslibInterop.onErrorHandlers.Remove(objectId);
            JslibInterop.onCloseHandlers.Remove(objectId);
        }

        public static void WsAddOnOpen(uint objectId, OnOpenHandler callback)
        {
            if (!JslibInterop.onOpenHandlers.ContainsKey(objectId))
            {
                JslibInterop.onOpenHandlers[objectId] = null;
            }
            
            JslibInterop.onOpenHandlers[objectId] += callback;
        }

        public static void WsRemoveOnOpen(uint objectId, OnOpenHandler callback)
        {
            if (!JslibInterop.onOpenHandlers.ContainsKey(objectId)) return;
            
            JslibInterop.onOpenHandlers[objectId] -= callback;
        }

        public static void WsAddOnMessage(uint objectId, OnMessageHandler callback)
        {
            if (!JslibInterop.onMessageHandlers.ContainsKey(objectId))
            {
                JslibInterop.onMessageHandlers[objectId] = null;
            }

            JslibInterop.onMessageHandlers[objectId] += callback;
        }

        public static void WsRemoveOnMessage(uint objectId, OnMessageHandler callback)
        {
            if (!JslibInterop.onMessageHandlers.ContainsKey(objectId)) return;
            
            JslibInterop.onMessageHandlers[objectId] -= callback;
        }

        public static void WsAddOnError(uint objectId, OnErrorHandler callback)
        {
            if (!JslibInterop.onErrorHandlers.ContainsKey(objectId))
            {
                JslibInterop.onErrorHandlers[objectId] = null;
            }

            JslibInterop.onErrorHandlers[objectId] += callback;
        }

        public static void WsRemoveOnError(uint objectId, OnErrorHandler callback)
        {
            if (!JslibInterop.onErrorHandlers.ContainsKey(objectId)) return;

            JslibInterop.onErrorHandlers[objectId] -= callback;
        }

        public static void WsAddOnClose(uint objectId, OnCloseHandler callback)
        {
            if (!JslibInterop.onCloseHandlers.ContainsKey(objectId))
            {
                JslibInterop.onCloseHandlers[objectId] = null;
            }

            JslibInterop.onCloseHandlers[objectId] += callback;
        }

        public static void WsRemoveOnClose(uint objectId, OnCloseHandler callback)
        {
            if (!JslibInterop.onCloseHandlers.ContainsKey(objectId)) return;

            JslibInterop.onCloseHandlers[objectId] -= callback;
        }

        [MonoPInvokeCallback(typeof(InteropOnOpenHandler))]
        private static void WsHandleOnOpen(uint objectId)
        {
            OnOpenHandler handler;

            if (!JslibInterop.onOpenHandlers.TryGetValue(objectId, out handler)) return;

            if (handler == null) return;
            
            handler();                    
        }

        [MonoPInvokeCallback(typeof(InteropOnMessageHandler))]
        private static void WsHandleOnMessage(uint objectId, IntPtr messagePtr)
        {
            OnMessageHandler handler;

            if (!JslibInterop.onMessageHandlers.TryGetValue(objectId, out handler)) return;

            if (handler == null) return;

            handler(Marshal.PtrToStringAuto(messagePtr));                    
        }

        [MonoPInvokeCallback(typeof(InteropOnErrorHandler))]
        private static void WsHandleOnError(uint objectId, IntPtr errorPtr)
        {
            OnErrorHandler handler;

            if (!JslibInterop.onErrorHandlers.TryGetValue(objectId, out handler)) return;
            
            if (handler == null) return;

            handler(Marshal.PtrToStringAuto(errorPtr));                    
        }

        [MonoPInvokeCallback(typeof(InteropOnCloseHandler))]
        private static void WsHandleOnClose(uint objectId, ushort closeCode)
        {
            OnCloseHandler handler;

            if (!JslibInterop.onCloseHandlers.TryGetValue(objectId, out handler)) return;

            if (handler == null) return;
            
            handler(closeCode);                    
        }
    }

    internal class WebSocket : IWebSocket
    {
        private uint? objectId;

        ~WebSocket()
        {
            if (this.objectId.HasValue)
            {
                JslibInterop.WsResetEvents(this.objectId.Value);
                JslibInterop.WsDestroy(this.objectId.Value);
            }
            
            this.objectId = null;
        }

        public WsState ReadyState 
        { 
            get
            {
                if (!this.objectId.HasValue)
                {
                    return WsState.Closed;
                }
                
                uint state = JslibInterop.WsGetReadyState(this.objectId.Value);

                switch (state)
                {
                case 0:
                    return WsState.Connecting;

                case 1:
                    return WsState.Open;

                case 2:
                    return WsState.Closing;

                case 3:
                    return WsState.Closed;

                default:
                    throw new WebSocketInvalidStateException("Unrecognized websocket ready state.");
                }
            }
        }

        public void Connect(string url, string protocols, string sessionId)
        {
            this.objectId = JslibInterop.WsCreate(url, protocols);
            JslibInterop.WsOpen(this.objectId.Value);
        }

        public void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            if (this.objectId.HasValue)
            {
                JslibInterop.WsClose(this.objectId.Value, (int) code, reason);
            }
        }

        public void Send(string message)
        {
            if (this.objectId.HasValue)
            {
                JslibInterop.WsSend(this.objectId.Value, message);
            }
        }

        public void Ping()
        {
            if (this.objectId.HasValue)
            {
                JslibInterop.WsSend(this.objectId.Value, "");
            }
        }

        public event OnOpenHandler OnOpen
        {
            add
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsAddOnOpen(this.objectId.Value, value);
                }
            }
            remove
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsRemoveOnOpen(this.objectId.Value, value);
                }
            }
        }

        public event OnMessageHandler OnMessage
        {
            add
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsAddOnMessage(this.objectId.Value, value);
                }
            }
            
            remove
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsRemoveOnMessage(this.objectId.Value, value);
                }
            }
        }

        public event OnErrorHandler OnError
        {
            add
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsAddOnError(this.objectId.Value, value);
                }
            }
            
            remove
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsRemoveOnError(this.objectId.Value, value);
                }
            }
        }

        public event OnCloseHandler OnClose
        {
            add
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsAddOnClose(this.objectId.Value, value);
                }
            }
            
            remove
            {
                if (this.objectId.HasValue)
                {
                    JslibInterop.WsRemoveOnClose(this.objectId.Value, value);
                }
            }
        }
    }
#else

    internal class WebSocket : IWebSocket
    {
        private WebSocketSharp.WebSocket webSocket;
        private bool IsProxySet = false;
        private string ProxyUrl;
        private string ProxyUsername;
        private string ProxyPassword;

        public event OnOpenHandler OnOpen;
        public event OnMessageHandler OnMessage;
        public event OnErrorHandler OnError;
        public event OnCloseHandler OnClose;

        ~WebSocket()
        {
            this.OnOpen = null;
            this.OnMessage = null;
            this.OnError = null;
            this.OnClose = null;
        }

        public WsState ReadyState
        {
            get
            {
                if (this.webSocket == null)
                {
                    return WsState.Closed;
                }
                
                switch (this.webSocket.ReadyState)
                {
                    case WebSocketState.Open: return WsState.Open;
                    case WebSocketState.Closed: return WsState.Closed;
                    case WebSocketState.Closing: return WsState.Closing;
                    case WebSocketState.Connecting: return WsState.Connecting;
                    default: throw new WebSocketInvalidStateException("Unrecognized websocket ready state.");
                }
            }
        }

        public void SetProxy(string url, string username, string password)
        {
            this.ProxyUrl = url;
            this.ProxyUsername = username;
            this.ProxyPassword = password;
            this.IsProxySet = true;
        }

        public void Connect(string url, string protocols, string sessionId, string entitlementToken)
        {
            try
            {
                this.webSocket = new WebSocketSharp.WebSocket(url, protocols);
                this.webSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Default 
                    | System.Security.Authentication.SslProtocols.Tls11 
                    | System.Security.Authentication.SslProtocols.Tls12;

                this.webSocket.CustomHeaders = string.IsNullOrEmpty(entitlementToken) ?
                    new Dictionary<string, string>
                    {
                        { "X-Ab-LobbySessionID" , sessionId}
                    } :
                    new Dictionary<string, string>
                    {
                        { "X-Ab-LobbySessionID" , sessionId},
                        { "Entitlement" , entitlementToken}
                    };

                if (IsProxySet)
                {
                    this.webSocket.SetProxy(this.ProxyUrl, this.ProxyUsername, this.ProxyPassword);
                }

                this.webSocket.OnOpen += (sender, ev) =>
                {
                    OnOpenHandler handler = this.OnOpen;

                    if (handler != null)
                    {
                        handler();
                    }
                };

                this.webSocket.OnMessage += (sender, ev) =>
                {
                    if (ev.RawData == null) return;

                    OnMessageHandler handler = this.OnMessage;

                    if (handler != null)
                    {
                        handler(ev.Data);
                    }
                };

                this.webSocket.OnError += (sender, ev) =>
                {
                    OnErrorHandler handler = this.OnError;

                    if (handler != null)
                    {
                        handler(ev.Message);
                    }
                };

                this.webSocket.OnClose += (sender, ev) =>
                {
                    OnCloseHandler handler = this.OnClose;

                    if (handler != null)
                    {
                        handler(ev.Code);
                    }

                    OnMessageHandler messageHandler = this.OnMessage;

                    if (messageHandler != null)
                    {
                        messageHandler(ev.Reason);
                    }
                };
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Websocket cannot be created.", e);
            }

            switch (this.webSocket.ReadyState)
            {
            case WebSocketState.Open: return;
            case WebSocketState.Closing: throw new WebSocketInvalidStateException("WebSocket is closing.");
            case WebSocketState.Closed: break;
            default:

                try
                {
                    this.webSocket.Connect();
                }
                catch (Exception e)
                {
                    throw new WebSocketUnexpectedException("Websocket failed to connect.", e);
                }

                break;
            }
        }

        public void Send(string message)
        {
            // Check state
            if (this.webSocket.ReadyState != WebSocketState.Open)
                throw new WebSocketInvalidStateException("Websocket is not open.");

            try
            {
                Report.GetWebSocketRequest(message);
                this.webSocket.SendAsync(message, null);
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Unexpected websocket exception.", e);
            }
        }

        public void Ping()
        {
            this.webSocket.SendAsync("", null);
        }

        public void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            if (this.webSocket.ReadyState == WebSocketState.Closing ||
                this.webSocket.ReadyState == WebSocketState.Closed)
            {
                return;
            }

            try
            {
                this.webSocket.Close((ushort) code, reason);
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Failed to close the connection.", e);
            }
        }
    }
#endif
}