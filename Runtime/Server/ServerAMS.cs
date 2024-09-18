// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AccelByte.Server
{
    public class ServerAMS
    {
        internal const string DefaultServerUrl = "ws://127.0.0.1:5555/watchdog";
        internal const int DefaulHeatbeatSeconds = 15;
        internal const string CommandLineAMSWatchdogUrlId = "-watchdogUrl";
        internal const string CommandLineAMSHeartbeatId = "-heartbeat";

        #region Events

        /// <summary>
        /// Raised when DS is connected to AMS
        /// </summary>
        public event Action OnOpen;

        /// <summary>
        /// Raised when DS got message from server that it will disconnect
        /// </summary>
        public event ResultCallback<DisconnectNotif> Disconnecting;

        /// <summary>
        /// Raised when DS is disconnected from AMS
        /// </summary>
        public event Action<WsCloseCode> Disconnected;

        // <summary>
        /// Raised when DS receive drain command
        /// </summary>
        public event Action OnDrainReceived;

        #endregion

        private ServerAMSWebsocketApi websocketApi;
        private readonly CoroutineRunner coroutineRunner;
        private Coroutine heartBeatCoroutine;
        private TimeSpan heartBeatInterval = TimeSpan.FromSeconds(15);
        private ApiSharedMemory sharedMemory;

        /// <summary>
        /// Event triggered each time a websocket reconnection attempt failed
        /// </summary>
        public event EventHandler OnRetryAttemptFailed
        {
            add => websocketApi.WebSocket.OnRetryAttemptFailed += value;
            remove => websocketApi.WebSocket.OnRetryAttemptFailed -= value;
        }

        /// <summary>
        /// AMS connection status
        /// </summary>
        public bool IsConnected => websocketApi.IsConnected;

        internal ServerAMS(ServerConfig config, CoroutineRunner inCoroutineRunner, ApiSharedMemory sharedMemory) : this(config.AMSServerUrl, config.AMSHeartbeatInterval, inCoroutineRunner, sharedMemory, config.AMSReconnectTotalTimeout)
        { 
        
        }

        internal ServerAMS(string serverUrl, int heartbeatIntervalSecond, CoroutineRunner inCoroutineRunner, ApiSharedMemory sharedMemory, int inWebsocketConnectionTimeoutMs = 60000)
        {
            Assert.IsNotNull(inCoroutineRunner);

            this.sharedMemory = sharedMemory;
            
            coroutineRunner = inCoroutineRunner;
            websocketApi = new ServerAMSWebsocketApi(inCoroutineRunner, serverUrl);
            websocketApi.SetSharedMemory(sharedMemory);
            
            heartBeatInterval = TimeSpan.FromSeconds(heartbeatIntervalSecond);
            websocketApi.OnOpen += HandleOnOpen;
            websocketApi.OnMessage += HandleOnMessage;
            websocketApi.OnClose += HandleOnClose;
        }

        /// <summary>
        /// Connect to AMS with provided ds id.
        /// The token generator need to be set for connection with entitlement verification.
        /// </summary>
        public virtual void Connect(string dsId)
        {
            Report.GetFunctionLog(GetType().Name);

            if (dsId == null || dsId.Length == 0)
            {
                sharedMemory?.Logger?.LogWarning("dsid not provided, not connecting to AMS");
            }
            else
            {
                websocketApi.Connect(dsId);
            }
        }

        /// <summary>
        /// Change the delay parameters to maintain connection to AMS.
        /// </summary>
        /// <param name="inTotalTimeout">Time limit until stop to re-attempt</param>
        /// <param name="inBackoffDelay">Initial delay time</param>
        /// <param name="inMaxDelay">Maximum delay time</param>
        public void SetRetryParameters(int inTotalTimeout = 60000
            , int inBackoffDelay = 1000
            , int inMaxDelay = 30000)
        {
            websocketApi.SetRetryParameters(inTotalTimeout, inBackoffDelay, inMaxDelay);
        }

        /// <summary>
        /// Disconnect from AMS.
        /// </summary>
        public virtual void Disconnect()
        {
            Report.GetFunctionLog(GetType().Name);

            StopHeartBeatScheduler();
            websocketApi.Disconnect();
        }

        /// <summary>
        /// Send ready message to AMS.
        /// </summary>
        public void SendReadyMessage()
        {
            sharedMemory?.Logger?.LogVerbose("Send ready to AMS");
            SendReadyMessageImplementation();
        }

        /// <summary>
        /// Set DS session timeout to new value. Calling this will refresh the timeout timer.
        /// </summary>
        /// <param name="newTimeout">New timeout value in seconds</param>
        public void SetDSTimeout(int newTimeout)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!IsConnected)
            {
                AccelByteDebug.LogWarning("Cannot set timeout, DS not connected to AMS yet.");
                return;
            }

            string sessionTimeoutMessage =
                string.Format("{{\"resetSessionTimeout\":{{\"newTimeout\":\"{0}\"}}}}", newTimeout);
            websocketApi.SendMessage(sessionTimeoutMessage);
        }

        /// <summary>
        /// Reset DS session timeout to default fleet setting. Calling this will refresh the timeout timer.
        /// </summary>
        public void ResetDSTimeout()
        {
            Report.GetFunctionLog(GetType().Name);

            if (!IsConnected)
            {
                AccelByteDebug.LogWarning("Cannot set timeout, DS not connected to AMS yet.");
                return;
            }

            string sessionTimeoutMessage =
                "{\"resetSessionTimeout\":{}}";
            websocketApi.SendMessage(sessionTimeoutMessage);
        }

        protected virtual void SendReadyMessageImplementation()
        {
            websocketApi.SendReadyMessage();
            StartHeartBeatScheduler();
        }

        #region protected/private methods
        protected void HandleOnOpen()
        {
            // debug ws connection
#if DEBUG
            sharedMemory?.Logger?.LogVerbose("[WS] Connection open");
#endif
            Action handler = OnOpen;

            if (handler != null)
            {
                handler();
            }
        }

        protected void HandleOnClose(ushort closecode)
        {
            // debug ws connection
#if DEBUG
            if (Enum.TryParse(closecode.ToString(), out WsCloseCode verboseCode))
            {
                sharedMemory?.Logger?.Log($"[WS Server AMS] Websocket connection close: {closecode} named {verboseCode.ToString()}");
            }
            else
            {
                sharedMemory?.Logger?.Log($"[WS Server AMS] Websocket connection close: {closecode}.");
            }
#endif
            var code = (WsCloseCode)closecode;
            Disconnected?.Invoke(code);
        }

        protected void HandleOnMessage(string message)
        {
            Report.GetWebSocketResponse(message);

            try
            {
                var command = JsonConvert.DeserializeObject<JObject>(message);
                if (command.ContainsKey("drain"))
                {
                    HandleDrain();
                    return;
                }

                Utils.IAccelByteWebsocketSerializer messageSerializer = new Utils.AwesomeFormat();
                messageSerializer.ReadHeader(message, out MessageType messageType, out long messageId);
                if (messageType == MessageType.disconnectNotif)
                {
                    ErrorCode errorCode = messageSerializer.ReadPayload(message, out DisconnectNotif payload);
                    HandleDisconnecting(payload, errorCode);
                    return;
                }
            }
            catch (Exception ex)
            {
                sharedMemory?.Logger?.LogWarning(ex.Message);
            }
        }

        protected void HandleDrain()
        {
            sharedMemory?.Logger?.LogWarning("Enter drain mode");
            OnDrainReceived?.Invoke();
        }

        protected void HandleDisconnecting(DisconnectNotif payload, ErrorCode errorCode)
        {
            if (errorCode != ErrorCode.None)
            {
                coroutineRunner.Run(() => Disconnecting(Result<DisconnectNotif>.CreateError(errorCode)));
            }
            else
            {
                coroutineRunner.Run(() => Disconnecting(Result<DisconnectNotif>.CreateOk(payload)));
            }
        }

        private IEnumerator RunPeriodicHeartBeat()
        {
            while (true)
            {
                yield return new WaitForSeconds((float)heartBeatInterval.TotalSeconds);
                websocketApi.SendHeartBeat();
            }
        }

        private void StartHeartBeatScheduler()
        {
            if (heartBeatCoroutine != null)
            {
                StopHeartBeatScheduler();
            }

            this.heartBeatCoroutine = this.coroutineRunner.Run(RunPeriodicHeartBeat());
        }

        private void StopHeartBeatScheduler()
        {
            if (heartBeatCoroutine == null)
            {
                return;
            }

            coroutineRunner.Stop(this.heartBeatCoroutine);
            this.heartBeatCoroutine = null;
        }

        #endregion
    }
}
