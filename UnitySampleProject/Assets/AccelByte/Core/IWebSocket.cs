// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core {
    public delegate void OnOpenHandler();

    public delegate void OnMessageHandler(string data);

    public delegate void OnErrorHandler(string errorMsg);

    public delegate void OnCloseHandler(ushort closeCode);

    public enum WsState { Connecting, Open, Closing, Closed }

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
        TlsHandshakeFailure = 1015
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
        void Connect(string url, string protocols);
        void Close(WsCloseCode code = WsCloseCode.Normal, string reason = null);
        void Send(string message);

        void Ping();
        WsState ReadyState { get; }
        event OnOpenHandler OnOpen;
        event OnMessageHandler OnMessage;
        event OnErrorHandler OnError;
        event OnCloseHandler OnClose;
    }
}