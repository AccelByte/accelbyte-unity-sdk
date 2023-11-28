// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
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
    }
}