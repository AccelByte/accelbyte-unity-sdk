// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class MatchmakingV2 : WrapperBase
    {
        private readonly MatchmakingV2Api matchmakingV2Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        
        internal Action<Result<MatchmakingV2MatchTicketStatus>> MatchmakingTicketPolled;

        [UnityEngine.Scripting.Preserve]
        internal MatchmakingV2(MatchmakingV2Api inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            matchmakingV2Api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.CreateMatchmakingTicket(matchPoolName, optionalParams, cb => 
                {
                    if(!cb.IsError)
                    {
                        SharedMemory.MessagingSystem.SendMessage(AccelByteMessagingTopic.MatchmakingStarted, cb.Value.matchTicketId);
                        StartMatchmakingTicketPoll(cb.Value.matchTicketId, matchPoolName);
                    }
                    SendPredefinedEvent(matchPoolName, optionalParams, cb, callback);
                }));
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.GetMatchmakingTicket(ticketId, callback));
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.DeleteMatchmakingTicket(ticketId, cb =>
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.GetMatchmakingMetrics(matchPool, callback));
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.GetUserMatchmakingTickets(callback, matchPool, offset, limit));
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
            if (!matchmakingV2Api.Config.EnableMatchmakingTicketCheck)
            {
                yield break;
            }

            yield return new WaitForSeconds(matchmakingV2Api.Config.MatchmakingTicketCheckInitialDelay / 1000f);

            bool shouldContinuePolling = true;
            while (shouldContinuePolling)
            {
                coroutineRunner.Run(
                    matchmakingV2Api.GetMatchmakingTicket(ticketId, result =>
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
                                    namespace_ = matchmakingV2Api.Config.Namespace,
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
                    yield return new WaitForSeconds(matchmakingV2Api.Config.MatchmakingTicketCheckPollRate / 1000f);
                }
            }
        }

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
                callback.TryError(result.Error);
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                callback.Try(result);
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, matchPoolName, optionalParams);
            var matchmakingEvent = new AccelByteTelemetryEvent(payload);

            predefinedEventScheduler.SendEvent(matchmakingEvent, null);
            callback.Try(result);
        }

        private void SendPredefinedEvent(string matchTicketId, Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                callback.Try(result);
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, matchTicketId);
            var matchmakingEvent = new AccelByteTelemetryEvent(payload);

            predefinedEventScheduler.SendEvent(matchmakingEvent, null);
            callback.Try(result);
        }

        #endregion
    }
}