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

        public event Action OnConnected;

        public event Action<WsCloseCode> OnDisconnected;

        public event ResultCallback<ServerClaimedNotification> MatchmakingV2ServerClaimed;

        public event ResultCallback<MatchmakingV2BackfillProposalNotification> MatchmakingV2BackfillProposalReceived;

        #endregion

        #region Properties

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


        public void Connect(string serverName)
        {
            Report.GetFunctionLog(GetType().Name);

            _serverDSHubWebsocketApi.Connect(serverName);
        }

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
                default:
                    Debug.LogError(
                        $"Error {ErrorCode.ErrorFromException}: Server DS Hub notification topic not supported: {notification.topic}");
                    break;
            }
        }
    }
}