// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
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

    public enum WsCloseCode
    {
        /* Do NOT use NotSet - it's only purpose is to indicate that the close code cannot be parsed. */
        NotSet = 0,
        Normal = 1000,
        Away = 1001,
        ProtocolError = 1002,
        UnsupportedData = 1003,
        Undefined = 1004,
        NoStatus = 1005,
        Abnormal = 1006,
        InvalidData = 1007,
        PolicyViolation = 1008,
        TooBig = 1009,
        MandatoryExtension = 1010,
        ServerError = 1011,
        ServiceRestart = 1012,
        TryAgainLater = 1013,
        BadGateway = 1014,
        TlsHandshakeFailure = 1015,
        ServerShuttingDown = 4000,
        AuthTokenRevoked = 4001,
        ServerClosed = 4002,
        ServerDisconnected = 4003
    }

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
        void Connect(string url, string protocols, Dictionary<string, string> customHeaders, string entitlementToken = null);
        void Connect(string url, string protocols, string sessionId, string entitlementToken = null);
        void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null);
        void Send(string message);

        void SetProxy(string url, string username, string password);

        void Ping();
        WsState ReadyState { get; }
        event OnOpenHandler OnOpen;
        event OnMessageHandler OnMessage;
        event OnErrorHandler OnError;
        event OnCloseHandler OnClose;
    }
}