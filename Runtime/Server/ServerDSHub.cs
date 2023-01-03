// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerDSHub : WrapperBase
    {
        #region Events

        /// <summary>
        /// Event triggered when successfully connected to DSHub
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// Event triggered when disconnected from DSHub
        /// </summary>
        public event Action<WsCloseCode> OnDisconnected;

        /// <summary>
        /// Event triggered when this Server is claimed by a match
        /// </summary>
        public event ResultCallback<ServerClaimedNotification> MatchmakingV2ServerClaimed;

        /// <summary>
        /// Event triggered when server received a backfill proposal
        /// </summary>
        public event ResultCallback<MatchmakingV2BackfillProposalNotification> MatchmakingV2BackfillProposalReceived;

        #endregion

        #region Properties

        /// <summary>
        /// DSHub connection status
        /// </summary>
        public bool IsConnected => _serverDSHubWebsocketApi.IsConnected;

        #endregion

        private readonly ISession _session;
        private readonly CoroutineRunner _coroutineRunner;

        private readonly ServerDSHubApi _serverDSHubApi;
        private readonly ServerDSHubWebsocketApi _serverDSHubWebsocketApi;

        internal ServerDSHub(ServerDSHubApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "serverDSHubApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            _serverDSHubApi = inApi;
            _session = inSession;
            _coroutineRunner = inCoroutineRunner;

            _serverDSHubWebsocketApi =
                new ServerDSHubWebsocketApi(inCoroutineRunner, inApi.ServerConfig.DSHubServerUrl, inSession);

            _serverDSHubWebsocketApi.OnOpen += HandleOnOpen;
            _serverDSHubWebsocketApi.OnClose += HandleOnClose;
            _serverDSHubWebsocketApi.OnMessage += HandleOnMessage;
        }

        /// <summary>
        /// Connect to DSHub
        /// </summary>
        /// <param name="serverName">this server's  name</param>
        public void Connect(string serverName)
        {
            Report.GetFunctionLog(GetType().Name);

            _serverDSHubWebsocketApi.Connect(serverName);
        }

        /// <summary>
        /// Disconnect from DSHub
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(GetType().Name);

            _serverDSHubWebsocketApi.Disconnect();
        }


        private void HandleOnOpen()
        {
#if DEBUG
            AccelByteDebug.Log("[Server DS Hub] Websocket connection open");
#endif
            OnConnected?.Invoke();
        }

        private void HandleOnClose(ushort closeCode)
        {
#if DEBUG
            AccelByteDebug.Log("[Server DS Hub] Websocket connection close: " + closeCode);
#endif
            var code = (WsCloseCode)closeCode;

            OnDisconnected?.Invoke(code);
        }

        private void HandleOnMessage(string message)
        {
            Report.GetServerWebSocketResponse(message);

            var notification = JsonConvert.DeserializeObject<ServerDSHubWebsocketNotificationTopic>(message);

            switch (notification.topic)
            {
                case DsHubNotificationTopic.serverClaimed:
                    var serverClaimedNotification =
                        JsonConvert.DeserializeObject<ServerDSHubWebsocketNotification<ServerClaimedNotification>>(
                            message);
                    _serverDSHubWebsocketApi.HandleNotification(serverClaimedNotification.payload,
                        MatchmakingV2ServerClaimed);
                    break;
                case DsHubNotificationTopic.BACKFILL_PROPOSAL:
                    var backfillProposalNotification =
                        JsonConvert
                            .DeserializeObject<
                                ServerDSHubWebsocketNotification<MatchmakingV2BackfillProposalNotification>>(message);
                    _serverDSHubWebsocketApi.HandleNotification(backfillProposalNotification.payload,
                        MatchmakingV2BackfillProposalReceived);
                    break;
                case DsHubNotificationTopic.EMPTY:
                    break;
                default:
                    AccelByteDebug.Log(
                        $"Server DS Hub notification topic not supported: {notification.topic}");
                    break;
            }
        }
    }
}