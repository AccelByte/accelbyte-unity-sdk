// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json;
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

        /// <summary>
        /// Event triggered when game session ended.
        /// </summary>
        public event ResultCallback<SessionEndedNotification> GameSessionV2Ended;

        /// <summary>
        /// Event triggered when game session member changed.
        /// </summary>
        public event ResultCallback<SessionV2GameSession> GameSessionV2MemberChanged;

        #endregion

        #region Properties

        /// <summary>
        /// DSHub connection status
        /// </summary>
        public bool IsConnected => serverDSHubWebsocketApi.IsConnected;

        #endregion

        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        private readonly ServerDSHubApi serverDSHubApi;
        private readonly ServerDSHubWebsocketApi serverDSHubWebsocketApi;

        [UnityEngine.Scripting.Preserve]
        internal ServerDSHub(ServerDSHubApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "serverDSHubApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            serverDSHubApi = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;

            serverDSHubWebsocketApi =
                new ServerDSHubWebsocketApi(inCoroutineRunner, inApi.ServerConfig.DSHubServerUrl, inSession);

            serverDSHubWebsocketApi.OnOpen += HandleOnOpen;
            serverDSHubWebsocketApi.OnClose += HandleOnClose;
            serverDSHubWebsocketApi.OnMessage += HandleOnMessage;
        }

        /// <summary>
        /// Connect to DSHub
        /// </summary>
        /// <param name="serverName">this server's  name</param>
        public void Connect(string serverName)
        {
            Report.GetFunctionLog(GetType().Name);

            serverDSHubWebsocketApi.Connect(serverName);
        }

        /// <summary>
        /// Disconnect from DSHub
        /// </summary>
        public void Disconnect()
        {
            Report.GetFunctionLog(GetType().Name);

            serverDSHubWebsocketApi.Disconnect();
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
                    serverDSHubWebsocketApi.HandleNotification(serverClaimedNotification.payload,
                        MatchmakingV2ServerClaimed);
                    break;
                case DsHubNotificationTopic.BACKFILL_PROPOSAL:
                    var backfillProposalNotification =
                        JsonConvert
                            .DeserializeObject<
                                ServerDSHubWebsocketNotification<MatchmakingV2BackfillProposalNotification>>(message);
                    serverDSHubWebsocketApi.HandleNotification(backfillProposalNotification.payload,
                        MatchmakingV2BackfillProposalReceived);
                    break;
                case DsHubNotificationTopic.SessionEnded:
                    var sessionEndedNotification =
                        JsonConvert.DeserializeObject <ServerDSHubWebsocketNotification<SessionEndedNotification>>(message);
                    serverDSHubWebsocketApi.HandleNotification(sessionEndedNotification.payload, GameSessionV2Ended);
                    break;
                case DsHubNotificationTopic.SessionMemberChanged:
                    var sessionMemberChangedNotification =
                        JsonConvert.DeserializeObject<ServerDSHubWebsocketNotification<SessionV2GameSession>>(message);
                    serverDSHubWebsocketApi.HandleNotification(sessionMemberChangedNotification.payload, GameSessionV2MemberChanged);
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