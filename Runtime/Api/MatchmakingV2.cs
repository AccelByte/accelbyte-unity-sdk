// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class MatchmakingV2 : WrapperBase
    {
        private readonly MatchmakingV2Api matchmakingV2Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

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
        /// <param name="optionalParams">Optional parameters in matchmaking ticket request.</param>
        /// <param name="callback">
        /// Returns a Result that contain MatchmakingV2CreateTicketResponse via callback when completed.
        /// </param>
        public void CreateMatchmakingTicket(string matchPoolName
            , [CanBeNull] MatchmakingV2CreateTicketRequestOptionalParams optionalParams,
            ResultCallback<MatchmakingV2CreateTicketResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(matchPoolName, nameof(matchPoolName) + " cannot be null");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.CreateMatchmakingTicket(matchPoolName, optionalParams, callback));
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
            Assert.IsNotNull(ticketId, nameof(ticketId) + " cannot be null");

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
            Assert.IsNotNull(ticketId, nameof(ticketId) + " cannot be null");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                matchmakingV2Api.DeleteMatchmakingTicket(ticketId, callback));
        }

        /// <summary>
        /// Get matchmaking's match pool metrics
        /// </summary>
        /// <param name="matchPool">Name of the match pool</param>
        /// <param name="callback">Returns a Result that contain MatchmakingV2Metrics via callback when completed</param>
        public void GetMatchmakingMetrics(string matchPool, ResultCallback<MatchmakingV2Metrics> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(matchPool), nameof(matchPool) + " cannot be null or empty");
            
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
    }
}