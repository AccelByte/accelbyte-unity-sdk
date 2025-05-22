// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using UnityEngine.Assertions;
using HybridWebSocket;
using AccelByte.Api;
using AccelByte.Models;

namespace AccelByte.Server
{
    class ServerAMSWebsocketApi
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

        private string amsWatchdogUrl;
        private string dsId = string.Empty;

        private ApiSharedMemory sharedMemory;

        public ServerAMSWebsocketApi(CoroutineRunner inCoroutineRunner, string inAMSWatchdogUrl, int inWebsocketConnectionTimeoutMs = 60000)
        {
            Assert.IsNotNull(inCoroutineRunner);

            coroutineRunner = inCoroutineRunner;
            amsWatchdogUrl = inAMSWatchdogUrl;
            IWebSocket webSocket = new WebSocket();
            OverrideWebsocket(webSocket, inWebsocketConnectionTimeoutMs);
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
            webSocket.SetLogger(sharedMemory?.Logger);
        }

        public void Connect(string id)
        {
            Connect(id, new WebsocketConnectOptionalParameters()
            {
                Logger = sharedMemory?.Logger
            });
        }
        
        internal void Connect(string id, WebsocketConnectOptionalParameters optionalParameters)
        {
            var targetLogger = optionalParameters != null && optionalParameters.Logger != null ? optionalParameters.Logger : sharedMemory?.Logger;
            
            dsId = id;
            targetLogger?.Log(string.Format("Connecting to AMS with id: {0}", dsId));

            if (IsConnected)
            {
                targetLogger?.Log("[AMS] already connected");
                return;
            }

            if (webSocket.IsConnecting)
            {
                targetLogger?.Log("[AMS] connecting to AMS");
                return;
            }

            webSocket.Connect(amsWatchdogUrl, authorizationToken: string.Empty, optionalParameters: optionalParameters);
        }

        /// <summary>
        /// Change the delay parameters to maintain connection to AMS.
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

        internal void SetSharedMemory(ApiSharedMemory sharedMem)
        {
            this.sharedMemory = sharedMem;
            webSocket?.SetLogger(sharedMem?.Logger);
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
