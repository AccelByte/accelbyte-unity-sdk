// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;

namespace AccelByte.Core {
    public delegate void OnOpenHandler();

    public delegate void OnMessageHandler(string data);

    public delegate void OnErrorHandler(string errorMsg);

    public delegate void OnCloseHandler(ushort closeCode);

    public enum WsState { New, Connecting, Open, Closing, Closed }

    public abstract class WebSocketException : Exception
    {
        protected WebSocketException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }

    public class WebSocketUnexpectedException : WebSocketException
    {
        public WebSocketUnexpectedException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }

    public class WebSocketInvalidStateException : WebSocketException
    {
        public WebSocketInvalidStateException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }

    public interface IWebSocket
    {
        void Connect(string url, string protocols, Dictionary<string, string> customHeaders, string entitlementToken = null, ResultCallback callback = null);
        void Connect(string url, string protocols, Dictionary<string, string> customHeaders,
            Models.WebsocketConnectOptionalParameters optionalParameters, string entitlementToken = null,
            ResultCallback callback = null);
        void Connect(string url, string protocols, string sessionId, string entitlementToken = null, ResultCallback callback = null);
        void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null);
        void Send(string message);
        void SetProxy(string url, string username, string password);
        void Ping();
        WsState ReadyState { get; }
        event OnOpenHandler OnOpen;
        event OnMessageHandler OnMessage;
        event OnErrorHandler OnError;
        event OnCloseHandler OnClose;
        internal void SetSharedMemory(ApiSharedMemory sharedMemory);
    }
}