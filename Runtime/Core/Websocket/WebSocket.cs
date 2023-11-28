// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;

#if UNITY_WEBGL && !UNITY_EDITOR
using AOT;
using System.Runtime.InteropServices;
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

        public void Connect(string url, string protocols, string sessionId, string entitlementToken = null)
        {
            Dictionary<string, string> customHeaders = null;
            Connect(url, protocols, customHeaders, entitlementToken);
        }

        public void Connect(string url, string protocols, Dictionary<string, string> customHeaders, string entitlementToken = null)
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

        public void SetProxy(string url, string username, string password)
        {

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
        private NativeWebSocket.WebSocket webSocket;

        public event OnOpenHandler OnOpen;
        public event OnMessageHandler OnMessage;
        public event OnErrorHandler OnError;
        public event OnCloseHandler OnClose;

        ~WebSocket()
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

        public void Connect(string url, string protocols, string sessionId, string entitlementToken = null)
        {
            Dictionary<string, string> customHeader = new Dictionary<string, string>()
            {
                { "X-Ab-LobbySessionID", sessionId }
            };
            
            if (entitlementToken != null)
            {
                customHeader.Add("Entitlement", entitlementToken);
            }
            
            Connect(url, protocols, customHeader);
        }

        public void Connect(string url, string protocols, Dictionary<string, string> customHeaders, string entitlementToken = null)
        {
            try
            {
                webSocket = string.IsNullOrEmpty(protocols) ? new NativeWebSocket.WebSocket(url, customHeaders) : new NativeWebSocket.WebSocket(url, protocols, customHeaders);

                webSocket.OnOpen += () =>
                {
                    OnOpen?.Invoke();
                };

                webSocket.OnMessage += data =>
                {
                    string dataStr = System.Text.Encoding.UTF8.GetString(data);
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

                AccelByteSDKMain.OnGameUpdate += _ =>
                {
                    webSocket.DispatchMessageQueue();
                };
            }
            catch (Exception e)
            {
                throw new WebSocketUnexpectedException("Websocket cannot be created.", e);
            }

            switch (webSocket.State)
            {
            case NativeWebSocket.WebSocketState.Open:
                AccelByteDebug.Log($"Websocket connection to {url} already opened");
                return;
            case NativeWebSocket.WebSocketState.Connecting:
                AccelByteDebug.LogWarning($"Websocket connection to {url} is connecting");
                break;
            case NativeWebSocket.WebSocketState.Closing: throw new WebSocketInvalidStateException("WebSocket is closing.");
            case NativeWebSocket.WebSocketState.Closed:
            default:
                try
                {
                    webSocket.Connect();
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
            if (webSocket.State != NativeWebSocket.WebSocketState.Open)
                throw new WebSocketInvalidStateException("Websocket is not open.");

            try
            {
                Report.GetWebSocketRequest(message);
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
    }
#endif
}