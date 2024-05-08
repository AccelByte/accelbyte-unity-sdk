// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using System;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSession : WrapperBase
    {
        private readonly ServerSessionApi _sessionApi;
        private readonly ISession _session;
        private readonly CoroutineRunner _coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerSession(ServerSessionApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct Session manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct Session manager; inCoroutineRunner is null!");

            _sessionApi = inApi;
            _session = inSession;
            _coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get all game session details.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="callback">Returns a paging result of SessionV2GameSession via callback when completed.</param>
        /// <param name="configurationName">(Optional)Filter result by configuration name</param>
        /// <param name="dsPodName">(Optional) Filter result by DS pod name</param>
        /// <param name="fromTime">(Optional) Filter result by time when session is created</param>
        /// <param name="gameMode">(Optional) Filter result by game mode</param>
        /// <param name="isPersistent">(Optional) Filter result by is persistent flag</param>
        /// <param name="isSoftDeleted">(Optional) Filter result by is soft deleted flag</param>
        /// <param name="joinability">(Optional) Filter result by joinability</param>
        /// <param name="limit">(Optional) Number of results to be returned per page</param>
        /// <param name="matchPool">(Optional) Filter result by match pool name</param>
        /// <param name="memberId">(Optional) Filter result by member id</param>
        /// <param name="offset">(Optional) Number of results to offset from</param>
        /// <param name="order">(Optional) Order of results by asc or desc</param>
        /// <param name="orderBy">(Optional) Results ordered by</param>\
        /// <param name="sessionId">(Optional) Filter result by session id</param>
        /// <param name="statusV2">(Optional) Filter result by ds status</param>
        /// <param name="toTime">(Optional) Filter result by time when session is created</param>
        public void GetAllGameSessions(
            ResultCallback<SessionV2GameSessionPagingResponse> callback
            , SessionV2DsStatus statusV2 = SessionV2DsStatus.None
            , string sessionId = ""
            , string matchPool = ""
            , string gameMode = ""
            , SessionV2Joinability joinability = SessionV2Joinability.None
            , string memberId = ""
            , string configurationName = ""
            , DateTime fromTime = default
            , DateTime toTime = default
            , string dsPodName = ""
            , bool isSoftDeleted = false
            , bool isPersistent = false
            , SessionV2AttributeOrderBy orderBy = SessionV2AttributeOrderBy.createdAt
            , SessionV2AttributeOrder order = SessionV2AttributeOrder.Desc
            , int offset = 0
            , int limit = 20
        )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetAllGameSessions(
                    callback
                    , statusV2
                    , sessionId
                    , matchPool
                    , gameMode
                    , joinability
                    , memberId
                    , configurationName
                    , fromTime
                    , toTime
                    , dsPodName
                    , isSoftDeleted
                    , isPersistent
                    , orderBy
                    , order
                    , offset
                    , limit
                )
            );
        }

        /// <summary>
        /// Get game session detail by sessionId.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetGameSessionDetails(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetGameSessionDetails(
                    sessionId,
                    callback));
        }

        /// <summary>
        /// Delete a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result via callback when completed.
        /// </param>
        public void DeleteGameSession(string sessionId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.DeleteGameSession(
                    sessionId,
                    callback));
        }

        /// <summary>
        /// Patch updates a game session, only specified fields from game session data.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="request">Compiled request of updating game session details</param>
        /// <param name="callback">
        /// Returns a result of updated SessionV2GameSession via callback when completed.
        /// </param>
        public void UpdateGameSession(string sessionId, SessionV2GameSessionUpdateRequest request
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.UpdateGameSession(
                    sessionId,
                    request,
                    callback));
        }

        /// <summary>
        /// Inform session service that current DS session is ready to receive client travel.
        /// This is needed if the session template used for this game session enabled the dsManualSetReady flag.
        /// </summary>
        /// <param name="sessionId">ID of the game session that claimed the DS.</param>
        /// <param name="isDsSessionReady">Bool value indicating if the DS session is ready or not.</param>
        /// <param name="callback">Returns a result via callback when completed.</param>
        public void SendDSSessionReady(string sessionId, bool isDsSessionReady
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule
                , Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.SendDSSessionReady(
                    sessionId,
                    isDsSessionReady,
                    callback));
        }

        /// <summary>
        /// Get Member Active Session.
        /// </summary>
        /// <param name="userId">Targeted User ID</param>
        /// <param name="configurationName">Session configuration name</param>
        /// <param name="callback">Returns a result via callback when completed.</param>
        public void GetMemberActiveSession(string userId
            , string configurationName
            , ResultCallback<SessionV2MemberActiveSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule
                , Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _sessionApi.GetMemberActiveSession(userId, configurationName, callback);
        }

        /// <summary>
        /// Reconcile Max Active Session.
        /// </summary>
        /// <param name="userId">Targeted User ID</param>
        /// <param name="configurationName">Session configuration name</param>
        /// <param name="callback">Returns a result via callback when completed.</param>
        public void ReconcileMaxActiveSession(string userId
            , string configurationName
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule
                , Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _sessionApi.ReconcileMaxActiveSession(userId, configurationName, callback);
        }
    }
}