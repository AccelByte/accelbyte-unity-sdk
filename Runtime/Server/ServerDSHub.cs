// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
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

            podName = serverName;
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

        #region PredefinedEvents

        internal string podName;
        internal Dictionary<string, SessionV2GameSession> cachedSessions = new Dictionary<string, SessionV2GameSession>();
        private PredefinedEventScheduler predefinedEventScheduler;
        private bool isAnalyticsConnected = false;

        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;

            ConnectPredefinedAnalyticsToEvents();
        }

        private void ConnectPredefinedAnalyticsToEvents()
        {
            if (isAnalyticsConnected)
            {
                return;
            }

            OnConnected += SendPredefinedEvent;
            OnDisconnected += SendPredefinedEvent;
            GameSessionV2MemberChanged += SendPredefinedEvent;
            MatchmakingV2ServerClaimed += SendPredefinedEvent;
            MatchmakingV2BackfillProposalReceived += SendPredefinedEvent;
            GameSessionV2Ended += endedNotif =>
            {
                if (endedNotif.IsError)
                {
                    return;
                }

                if (cachedSessions.ContainsKey(endedNotif.Value.SessionId))
                {
                    cachedSessions.Remove(endedNotif.Value.SessionId);
                }
            };

            isAnalyticsConnected = true;
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T result)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (typeof(T))
            {
                case Type statusCodeType when statusCodeType == typeof(WsCloseCode):
                    Enum statusCode = result as Enum;
                    var statusCodeEnum = (WsCloseCode)statusCode;

                    payload = new PredefinedDSDisconnectedPayload(podName, statusCodeEnum);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(Result<T> result)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (typeof(T))
            {
                case Type gameSessionType when gameSessionType == typeof(SessionV2GameSession):
                    var gameSession = result.Value as SessionV2GameSession;
                    SendSessionChangedPredefinedEvent(gameSession);

                    if (!cachedSessions.ContainsKey(gameSession.id))
                    {
                        cachedSessions.Add(gameSession.id, gameSession);
                        payload = new PredefinedDSClientJoinedPayload(podName, gameSession.members[0].id);
                    }
                    else if (cachedSessions[gameSession.id].members.Length < gameSession.members.Length)
                    {
                        SessionV2MemberData joinedMember = null;

                        foreach (var member in gameSession.members)
                        {
                            var newMember = Array.Find(cachedSessions[gameSession.id].members, sessionMember =>
                            {
                                return sessionMember.id == member.id;
                            });

                            if (newMember == null)
                            {
                                joinedMember = member;
                                payload = new PredefinedDSClientJoinedPayload(podName, joinedMember.id);
                                break;
                            }
                        }
                    }
                    else if (gameSession.members.Length < cachedSessions[gameSession.id].members.Length)
                    {
                        SessionV2MemberData joinedMember = null;

                        foreach (var member in cachedSessions[gameSession.id].members)
                        {
                            var newMember = Array.Find(gameSession.members, sessionMember =>
                            {
                                return sessionMember.id == member.id;
                            });

                            if (newMember == null)
                            {
                                joinedMember = member;
                                payload = new PredefinedDSClientLeftPayload(podName, joinedMember.id);
                                break;
                            }
                        }
                    }

                    cachedSessions[gameSession.id] = gameSession;
                    break;

                case Type serverClaimedType when serverClaimedType == typeof(ServerClaimedNotification):
                    var serverClaimed = result.Value as ServerClaimedNotification;

                    payload = new PredefinedDSClaimedPayload(podName, serverClaimed.sessionId);
                    break;

                case Type backfillNotifType when backfillNotifType == typeof(MatchmakingV2BackfillProposalNotification):
                    var backfillNotif = result.Value as MatchmakingV2BackfillProposalNotification;

                    payload = new PredefinedDSBackfillReceivedPayload(podName, backfillNotif);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreatePayload()
        {
            IAccelByteTelemetryPayload payload = new PredefinedDSConnectedPayload(podName);

            return payload;
        }

        internal void SendPredefinedEvent<T>(T result)
        {
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result);

            if (payload == null)
            {
                return;
            }

            var dsHubEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(dsHubEvent, null);
        }

        internal void SendPredefinedEvent<T>(Result<T> result)
        {
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result);

            if (payload == null)
            {
                return;
            }

            var dsHubEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(dsHubEvent, null);
        }

        internal void SendPredefinedEvent()
        {
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload();

            if (payload == null)
            {
                return;
            }

            var dsHubEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(dsHubEvent, null);
        }

        private void SendSessionChangedPredefinedEvent(SessionV2GameSession session)
        {
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = new PredefinedDSMemberChangedPayload(podName, session);

            var dsHubEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(dsHubEvent, null);
        }

        #endregion
    }
}