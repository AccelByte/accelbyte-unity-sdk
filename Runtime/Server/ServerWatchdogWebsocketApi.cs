// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using UnityEngine.Assertions;
using HybridWebSocket;
using AccelByte.Api;

namespace AccelByte.Server
{
    class ServerWatchdogWebsocketApi
    {
        #region Events

        public event OnOpenHandler OnOpen;
        public event OnCloseHandler OnClose;
        public event OnMessageHandler OnMessage;
        public event OnErrorHandler OnError;

        #endregion

        #region properties

        public AccelByteWebSocket WebSocket => webSocket;
        public bool IsConnected => webSocket?.IsConnected == true;

        #endregion

        private AccelByteWebSocket webSocket;
        private readonly CoroutineRunner coroutineRunner;

        private string watchdogUrl;
        private string dsId = string.Empty;

        public ServerWatchdogWebsocketApi(CoroutineRunner inCoroutineRunner, string inWatchdogUrl)
        {
            Assert.IsNotNull(inCoroutineRunner);

            coroutineRunner = inCoroutineRunner;
            watchdogUrl = inWatchdogUrl;
            IWebSocket webSocket = new WebSocket();
            OverrideWebsocket(webSocket);
        }

        public void OverrideWebsocket(IWebSocket inWebSocket
            , int inPingDelay = 4000
            , int inBackoffDelay = 1000
            , int inMaxDelay = 30000
            , int inTotalTimeout = 60000)
        {
            webSocket = new AccelByteWebSocket(inWebSocket);
            webSocket.OnOpen += HandleOnOpen;
            webSocket.OnMessage += HandleOnMessage;
            webSocket.OnClose += HandleOnClose;
            webSocket.OnError += HandleOnError;

            webSocket.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay, inPingDelay);
        }

        public void Connect(string id)
        {
            dsId = id;
            AccelByteDebug.Log(string.Format("Connecting to watchdog with id: {0}", dsId));

            if (IsConnected)
            {
                AccelByteDebug.Log("[Watchdog] already connected");
                return;
            }

            if (webSocket.IsConnecting)
            {
                AccelByteDebug.Log("[Watchdog] connecting to watchdog");
                return;
            }

            webSocket.Connect(watchdogUrl, string.Empty);
        }

        /// <summary>
        /// Change the delay parameters to maintain connection to Watchdog.
        /// </summary>
        /// <param name="inTotalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="inBackoffDelay">Initial delay time</param>
        /// <param name="inMaxDelay">Maximum delay time</param>
        public void SetRetryParameters(
            int inTotalTimeout = 60000,
            int inBackoffDelay = 1000,
            int inMaxDelay = 30000)
        {
            webSocket?.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay);
        }

        public void Disconnect(WsCloseCode code = WsCloseCode.Normal, string reason = null)
        {
            webSocket.Disconnect();
        }

        public void SendReadyMessage()
        {
            if(webSocket.IsConnected)
            {
                webSocket.Send(string.Format("{{\"ready\":{{\"dsid\":\"{0}\"}}}}", dsId));
            }
        }

        public void SendHeartBeat()
        {
            if (webSocket.IsConnected)
            {
                webSocket.Send("{\"heartbeat\":{}}");
            }
        }

        public void SendMessage(string msg)
        {
            webSocket.Send(msg);
        }

        #region private methods

        private void HandleOnOpen()
        {
            this.OnOpen?.Invoke();
        }

        private void HandleOnClose(ushort closeCode)
        {
            this.OnClose?.Invoke(closeCode);
        }

        private void HandleOnMessage(string message)
        {
            this.OnMessage?.Invoke(message);
        }

        private void HandleOnError(string errorMsg)
        {
            this.OnError?.Invoke(errorMsg);
        }

        #endregion private methods
    }
}
