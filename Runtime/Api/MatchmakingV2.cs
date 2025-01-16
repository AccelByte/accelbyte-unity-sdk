// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class MatchmakingV2 : WrapperBase
    {
        internal MatchmakingV2Api Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        private Dictionary<string, PartySessionStorageReservedData> partyMemberSessionIds;
        internal readonly PartySessionStorageApi PartySessionStorageApi;
        private string partySessionId;

        internal Action<Result<MatchmakingV2MatchTicketStatus>> MatchmakingTicketPolled;

        public bool PartyMemberPastSessionRecordSyncEnabled
        {
            get;
            set;
        }

        [UnityEngine.Scripting.Preserve]
        internal MatchmakingV2(MatchmakingV2Api inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            Api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
            PartySessionStorageApi = new PartySessionStorageApi(inApi.HttpClient,
                inApi.Config,
                session);
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            SharedMemory?.MessagingSystem?.UnsubscribeToTopicV2(AccelByteMessagingTopic.OnJoinedPartyNotification, OnJoinedPartyEventHandler);
            SharedMemory?.MessagingSystem?.UnsubscribeToTopicV2(AccelByteMessagingTopic.OnCreatedPartyNotification, OnCreatedPartyEventHandler);
            SharedMemory?.MessagingSystem?.UnsubscribeToTopicV2(AccelByteMessagingTopic.OnLeftPartyNotification, OnLeftPartyEventHandler);
            SharedMemory?.MessagingSystem?.UnsubscribeToTopicV2(AccelByteMessagingTopic.OnJoinedGameSessionNotification, OnJoinedGameSessionEventHandler);

            base.SetSharedMemory(newSharedMemory);

            SharedMemory?.MessagingSystem?.SubscribeToTopicV2(AccelByteMessagingTopic.OnJoinedPartyNotification, OnJoinedPartyEventHandler);
            SharedMemory?.MessagingSystem?.SubscribeToTopicV2(AccelByteMessagingTopic.OnCreatedPartyNotification, OnCreatedPartyEventHandler);
            SharedMemory?.MessagingSystem?.SubscribeToTopicV2(AccelByteMessagingTopic.OnLeftPartyNotification, OnLeftPartyEventHandler);
            SharedMemory?.MessagingSystem?.SubscribeToTopicV2(AccelByteMessagingTopic.OnJoinedGameSessionNotification, OnJoinedGameSessionEventHandler);
        }

        /// <summary>
        /// Creates a new request for matchmaking
        /// </summary>
        /// <param name="matchPoolName">Name of target match pool</param>
        /// <param name="callback">
        /// Returns a Result that contain MatchmakingV2CreateTicketResponse via callback when completed.
        /// </param>
        public void CreateMatchmakingTicket(string matchPoolName,
            ResultCallback<MatchmakingV2CreateTicketResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            CreateMatchmakingTicket(matchPoolName, null, callback);
        }
        
        /// <summary>
        /// Creates a new request for matchmaking
        /// </summary>
        /// <param name="matchPoolName">Name of target match pool</param>
        /// <param name="optionalParams">Optional parameters in matchmaking ticket request.</param>
        /// <param name="callback">
        /// Returns a Result that contain MatchmakingV2CreateTicketResponse via callback when completed.
        /// </param>
        public void CreateMatchmakingTicket(string matchPoolName
            , MatchmakingV2CreateTicketRequestOptionalParams optionalParams,
            ResultCallback<MatchmakingV2CreateTicketResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (GetPartySessionStorageRequired(optionalParams))
            {
                GetPartySessionStorage(optionalParams.sessionId, cb =>
                {
                    if (!cb.IsError)
                    {
                        foreach (var memberSessionId in cb.Value.Reserved)
                        {
                            SharedMemory?.PastSessionRecordManager?.InsertPastSessionId(memberSessionId.Key, memberSessionId.Value.PastSessionIds); ;
                        }
                        partyMemberSessionIds = cb.Value.Reserved;
                    }
                    CreateMatchmakingTicketHandler(matchPoolName, optionalParams, callback);
                });
                return;
            }

            CreateMatchmakingTicketHandler(matchPoolName, optionalParams, callback);
        }
        
        /// <summary>
        /// Get details for a specific match ticket
        /// </summary>
        /// <param name="ticketId">Targeted matchmaking ticket id</param>
        /// <param name="callback">
        /// Returns a Result that contain MatchmakingV2MatchTicketStatus via callback when completed.
        /// </param>
        public void GetMatchmakingTicket(string ticketId
            , ResultCallback<MatchmakingV2MatchTicketStatus> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(ticketId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetTicketIdInvalidMessage(ticketId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.GetMatchmakingTicket(ticketId, callback));
        }
        
        /// <summary>
        /// Deletes an existing matchmaking ticket.
        /// </summary>
        /// <param name="ticketId">Targeted matchmaking ticket id</param>
        /// <param name="callback">
        /// Returns a Result via callback when completed.
        /// </param>
        public void DeleteMatchmakingTicket(string ticketId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(ticketId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetTicketIdInvalidMessage(ticketId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.DeleteMatchmakingTicket(ticketId, cb =>
                {
                    SendPredefinedEvent(ticketId, cb, callback);
                }));
        }

        /// <summary>
        /// Get matchmaking's match pool metrics
        /// </summary>
        /// <param name="matchPool">Name of the match pool</param>
        /// <param name="callback">Returns a Result that contain MatchmakingV2Metrics via callback when completed</param>
        public void GetMatchmakingMetrics(string matchPool, ResultCallback<MatchmakingV2Metrics> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.GetMatchmakingMetrics(matchPool, callback));
        }

        /// <summary>
        /// Get active matchmaking tickets for current user.
        /// </summary>
        /// <param name="callback">Returns a result that contain all active tickets of current user.</param>
        /// <param name="matchPool">Optional if filled it will return only ticket from specified matchpool</param>
        /// <param name="offset">The offset of the types and/or subtypes paging data result. Default value is 0.</param>
        /// <param name="limit">The limit of the types and subtypes results. Default value is 20.</param>
        public void GetUserMatchmakingTickets(ResultCallback<MatchmakingV2ActiveTickets> callback
            , string matchPool = "", int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.GetUserMatchmakingTickets(callback, matchPool, offset, limit));
        }

        /// <summary>
        /// Start polling for matchmaking ticket status when Enable Matchmaking Ticket Check is enabled on client config
        /// </summary>
        /// <param name="ticketId">String ID of ticket to poll status for.</param>
        /// <param name="matchPoolName">Matchpool name used in matchmaking</param>
        internal void StartMatchmakingTicketPoll(string ticketId, string matchPoolName)
        {
            coroutineRunner.Run(StartMatchmakingTicketPollAsync(ticketId, matchPoolName));
        }

        /// <summary>
        /// Start polling for matchmaking ticket status when Enable Matchmaking Ticket Check is enabled on client config
        /// </summary>
        /// <param name="ticketId">String ID of ticket to poll status for.</param>
        /// <param name="matchPoolName">Matchpool name used in matchmaking</param>
        internal IEnumerator StartMatchmakingTicketPollAsync(string ticketId, string matchPoolName)
        {
            if (!Api.Config.EnableMatchmakingTicketCheck)
            {
                yield break;
            }

            yield return new WaitForSeconds(Api.Config.MatchmakingTicketCheckInitialDelay / 1000f);

            bool shouldContinuePolling = true;
            while (shouldContinuePolling)
            {
                coroutineRunner.Run(
                    Api.GetMatchmakingTicket(ticketId, result =>
                    {
                        MatchmakingTicketPolled?.Invoke(result);

                        if (result.IsError)
                        {
                            if (result.Error.Code == ErrorCode.NotFound)
                            {
                                var expiredNotif = new MatchmakingV2TicketExpiredNotification()
                                {
                                    ticketId = ticketId,
                                    matchPool = matchPoolName,
                                    namespace_ = Api.Config.Namespace,
                                    createdAt = DateTime.UtcNow
                                };
                                var expiredNotifMessage = AccelByteNotificationSenderUtility.ComposeMMv2Notification(
                                    "OnMatchmakingTicketExpired"
                                    , expiredNotif.ToJsonString()
                                    , isEncoded: true);
                                SharedMemory.NotificationSender.SendLobbyNotification(expiredNotifMessage);

                                shouldContinuePolling = false;
                            }

                            return;
                        }

                        if (result.Value.matchFound)
                        {
                            SharedMemory.MessagingSystem
                                .SendMessage(AccelByteMessagingTopic.MatchFoundOnPoll, result.Value.sessionId);

                            shouldContinuePolling = false;
                            return;
                        }
                    }));

                if (shouldContinuePolling)
                {
                    yield return new WaitForSeconds(Api.Config.MatchmakingTicketCheckPollRate / 1000f);
                }
            }
        }

        private bool GetPartySessionStorageRequired(MatchmakingV2CreateTicketRequestOptionalParams optionalParams)
        {
            if (optionalParams == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(optionalParams.sessionId) 
                && (optionalParams.ExclusionType == MatchmakingV2ExclusionType.NPastSession
                || optionalParams.ExclusionType == MatchmakingV2ExclusionType.AllMemberCachedSession);
        }

        private void CreateMatchmakingTicketHandler(string matchPoolName
            , MatchmakingV2CreateTicketRequestOptionalParams optionalParams
            , ResultCallback<MatchmakingV2CreateTicketResponse> callback)
        {
            if (optionalParams != null)
            {
                optionalParams.ExcludedGameSessionIds = PopulatePastMemberSessionId(optionalParams);
            }


            coroutineRunner.Run(Api.CreateMatchmakingTicket(matchPoolName, optionalParams, cb =>
            {
                if (!cb.IsError)
                {
                    StartMatchmakingTicketPoll(cb.Value.matchTicketId, matchPoolName);
                    SharedMemory.MessagingSystem.SendMessage(AccelByteMessagingTopic.MatchmakingStarted, cb.Value.matchTicketId);
                }
                SendPredefinedEvent(matchPoolName, optionalParams, cb, callback);
            }));
        }

        private string[] PopulatePastMemberSessionId(MatchmakingV2CreateTicketRequestOptionalParams optionalParams)
        {

            if (optionalParams == null 
                || optionalParams.ExclusionType == MatchmakingV2ExclusionType.None)
            {
                return null;
            }

            if (optionalParams.ExclusionType == MatchmakingV2ExclusionType.ExplicitList)
            {
                return optionalParams.ExcludedGameSessionIds;
            }

            if (partyMemberSessionIds == null)
            {
                return null;
            }

            var populatedPastMemberSessionId = new List<string>();

            foreach (var pastMemberSessionId in partyMemberSessionIds.Values)
            {
                if (optionalParams.ExclusionType == MatchmakingV2ExclusionType.NPastSession)
                {
                    populatedPastMemberSessionId.AddRange(pastMemberSessionId.PastSessionIds.TakeLast(optionalParams.ExcludedPastSessionCount));
                }
                else
                {
                    populatedPastMemberSessionId.AddRange(pastMemberSessionId.PastSessionIds);
                }
            }
            return populatedPastMemberSessionId.ToArray();
        }

        #region PartySessionStorage
        internal void GetPartySessionStorage(string partyId
            , ResultCallback<GetPartySessionStorageResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId
                , Utils.AccelByteIdValidator.HypensRule.NoRule
                , Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId)
                , callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            PartySessionStorageApi.GetPartySessionStorage(partyId, callback);
        }

        internal void StorePersonalDataToReservedPartySessionStorage(string partyId
            , PartySessionStorageReservedData body
            , ResultCallback<PartySessionStorageReservedData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId
                , Utils.AccelByteIdValidator.HypensRule.NoRule
                , Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId)
                , callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            PartySessionStorageApi.StorePersonalDataToReservedPartySessionStorage(partyId, body, callback);
        }

        private void UpdatePartySessionStorageWithPastSessionInfo(string partyId)
        {
            if (partyId == null || !PartyMemberPastSessionRecordSyncEnabled)
            {
                return;
            }

            partySessionId = partyId;

            var pastSessionIds = SharedMemory?.PastSessionRecordManager?
                .GetPastSessionIds(session.UserId);

            if (pastSessionIds?.Count() > 0)
            {
                var payload = new PartySessionStorageReservedData(pastSessionIds);
                PartySessionStorageApi.StorePersonalDataToReservedPartySessionStorage(partyId
                    , payload
                    , null);
            }
        }

        private void OnJoinedPartyEventHandler(AccelByteMessage notif)
        {
            var partyNotificationUserJoined =
                JsonConvert.DeserializeObject<SessionV2PartyJoinedNotification>(notif.Message);

            if (partyNotificationUserJoined.members.Any(member => member.id == session.UserId))
            {
                UpdatePartySessionStorageWithPastSessionInfo(partyNotificationUserJoined.partyId);
            }
        }

        private void OnCreatedPartyEventHandler(AccelByteMessage notif)
        {
            var partyNotificationUserJoined =
                JsonConvert.DeserializeObject<PartyCreatedNotification>(notif.Message);
            
            if (partyNotificationUserJoined.CreatedBy == session.UserId)
            {
                UpdatePartySessionStorageWithPastSessionInfo(partyNotificationUserJoined.PartyID);
            }
        }

        private void OnLeftPartyEventHandler(AccelByteMessage notif)
        {
            if (notif.Message == session.UserId)
            {
                partySessionId = null;
            }
        }

        private void OnJoinedGameSessionEventHandler(AccelByteMessage notif)
        {
            if (notif.Message == session.UserId)
            {
                UpdatePartySessionStorageWithPastSessionInfo(partySessionId);
            }
        }
        #endregion

        #region PredefinedEvents

        IAccelByteTelemetryPayload CreatePayload(Result result, string matchTicketId)
        {
            string localUserId = session.UserId;

            IAccelByteTelemetryPayload payload = new PredefinedMatchmakingCanceledPayload(localUserId, matchTicketId);

            return payload;
        }

        IAccelByteTelemetryPayload CreatePayload<T>(Result<T> result, string matchPoolName, MatchmakingV2CreateTicketRequestOptionalParams optionalParams)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (typeof(T))
            {
                case Type createMatchmaking when createMatchmaking == typeof(MatchmakingV2CreateTicketResponse):
                    var createMatchmakingValue = result.Value as MatchmakingV2CreateTicketResponse;
                    payload = new PredefinedMatchmakingRequestedPayload(localUserId
                        , matchPoolName
                        , optionalParams?.sessionId
                        , optionalParams?.attributes
                        , createMatchmakingValue.matchTicketId
                        , createMatchmakingValue.queueTime);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent<T>(string matchPoolName, MatchmakingV2CreateTicketRequestOptionalParams optionalParams, Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                callback?.Try(result);
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, matchPoolName, optionalParams);
            var matchmakingEvent = new AccelByteTelemetryEvent(payload);

            predefinedEventScheduler.SendEvent(matchmakingEvent, null);
            callback?.Try(result);
        }

        private void SendPredefinedEvent(string matchTicketId, Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                callback?.Try(result);
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, matchTicketId);
            var matchmakingEvent = new AccelByteTelemetryEvent(payload);

            predefinedEventScheduler.SendEvent(matchmakingEvent, null);
            callback?.Try(result);
        }

        #endregion
    }
}