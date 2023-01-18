// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json.Linq;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Session : WrapperBase
    {
        private readonly SessionApi _sessionApi;
        private readonly ISession _session;
        private readonly CoroutineRunner _coroutineRunner;

        internal Session(SessionApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            _sessionApi = inApi;
            _session = inSession;
            _coroutineRunner = inCoroutineRunner;
        }

        #region PartySession

        /// <summary>
        /// Get Party details with partyId.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void GetPartyDetails(string partyId, ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetPartyDetails(
                    partyId,
                    callback));
        }

        /// <summary>
        /// Update(Overwrite) all fields of a party.
        /// Note: Join type can only be updated by the party's leader.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="request">
        /// Compiled request with the updating details of the party session
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void UpdateParty(string partyId, SessionV2PartySessionUpdateRequest request,
            ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");
            Assert.IsNotNull(request, "SessionV2PartySessionUpdateRequest cannot be null");


            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.UpdateParty(
                    partyId,
                    request,
                    callback));
        }

        /// <summary>
        /// Update(Partially) specified field(s) of a party.
        /// Note: Join type can only be updated by the party's leader.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="request">
        /// Compiled request with the updating details of the party session
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void PatchUpdateParty(string partyId, SessionV2PartySessionUpdateRequest request,
            ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");
            Assert.IsNotNull(request, "SessionV2PartySessionUpdateRequest cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.PatchUpdateParty(
                    partyId,
                    request,
                    callback));
        }

        /// <summary>
        /// Invite a user to a party.
        /// </summary>
        /// <param name="partyId">Targeted party ID</param>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void InviteUserToParty(string partyId, string userId,
            ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");
            Assert.IsFalse(string.IsNullOrEmpty(userId),
                "Party ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.InviteUserToParty(
                    partyId,
                    userId,
                    callback));
        }        
        
        /// <summary>
        /// Promotes a party member to be a party leader. Only leader can promote a new leader.
        /// </summary>
        /// <param name="partyId">Targeted party ID</param>
        /// <param name="leaderId">Targeted user ID of new leader</param>
        /// <param name="callback">Returns a Result that contain SessionV2PartySession via callback when completed</param>
        public void PromoteUserToPartyLeader(string partyId, string leaderId,
            ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");
            Assert.IsFalse(string.IsNullOrEmpty(leaderId),
                "Leader ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.PromoteUserToPartyLeader(
                    partyId,
                    leaderId,
                    callback));
        }

        /// <summary>
        /// Join a party.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void JoinParty(string partyId, ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.JoinParty(
                    partyId,
                    callback));
        }

        /// <summary>
        /// Leave a Party.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void LeaveParty(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.LeaveParty(
                    partyId,
                    callback));
        }

        /// <summary>
        /// Reject a party invitation.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void RejectPartyInvitation(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.RejectPartyInvitation(
                    partyId,
                    callback));
        }

        /// <summary>
        /// Kick a player from a party.
        /// Requires invoker to be the party leader.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="userId">Targeted user's ID.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySessionKickResponse via callback when completed.
        /// </param>
        public void KickUserFromParty(string partyId, string userId,
            ResultCallback<SessionV2PartySessionKickResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "Party ID cannot be null.");
            Assert.IsFalse(string.IsNullOrEmpty(partyId),
                "User ID cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.KickUserFromParty(
                    partyId,
                    userId,
                    callback));
        }

        /// <summary>
        /// Create a Party and assign invoker as leader.
        /// </summary>
        /// <param name="request">
        /// Compiled request with details of the creating party session
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void CreateParty(SessionV2PartySessionCreateRequest request,
            ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(request, "SessionV2PartySessionCreateRequest cannot be null.");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.CreateParty(
                    request,
                    callback));
        }

        /// <summary>
        /// Get all parties of a user(invoker).
        /// </summary>
        /// <param name="callback">
        /// Returns a paginated result of SessionV2PartySession via callback when completed.
        /// </param>
        public void GetUserParties(ResultCallback<PaginatedResponse<SessionV2PartySession>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetUserParties(
                    callback));
        }

        #endregion

        #region GameSession

        /// <summary>
        /// Create Game Session
        /// </summary>
        /// <param name="request">Details of creating game session.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2GameSession via callback when completed.
        /// </param>
        public void CreateGameSession(SessionV2GameSessionCreateRequest request
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(request, "SessionV2GameSessionCreateRequest cannot be null");
            Assert.IsNotNull(request.configurationName, "configurationName cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.CreateGameSession(
                    request,
                    callback));
        }

        /// <summary>
        /// Query Game Session
        /// By default, API will return a list of available game sessions
        /// </summary>
        /// <param name="request">List of attributes to filter from available sessions</param>
        /// <param name="callback">
        /// Returns a paginated result of SessionV2GameSession via callback when completed.
        /// </param>
        public void QueryGameSession(Dictionary<string, object> request
            , ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (request == null)
            {
                request = new Dictionary<string, object>();
            }

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.QueryGameSession(
                    request,
                    callback));
        }

        /// <summary>
        /// Get game session detail by podName.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="podName">Targeted game session's podName</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetGameSessionDetailsByPodName(string podName
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(podName, "podName cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetGameSessionDetailsByPodName(
                    podName,
                    callback));
        }

        /// <summary>
        /// Get game session detail by sessionId.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetGameSessionDetailsBySessionId(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetGameSessionDetailsBySessionId(
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
            Assert.IsNotNull(sessionId, "sessionId cannot be null");

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
        public void PatchGameSession(string sessionId, SessionV2GameSessionUpdateRequest request
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");
            Assert.IsNotNull(request, "SessionV2GameSessionUpdateRequest cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.PatchGameSession(
                    sessionId,
                    request,
                    callback));
        }

        /// <summary>
        /// Invite a user to a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="sessionId">Targeted user's userId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void InviteUserToGameSession(string sessionId, string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");
            Assert.IsNotNull(userId, "userId cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.InviteUserToGameSession(
                    sessionId,
                    userId,
                    callback));
        }

        /// <summary>
        /// Join a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void JoinGameSession(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.JoinGameSession(
                    sessionId,
                    callback));
        }

        /// <summary>
        /// Leave a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void LeaveGameSession(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.LeaveGameSession(
                    sessionId,
                    callback));
        }

        /// <summary>
        /// Reject a game session invitation.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result via callback when completed.
        /// </param>
        public void RejectGameSessionInvitation(string sessionId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "sessionId cannot be null");

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.RejectGameSessionInvitation(
                    sessionId,
                    callback));
        }

        /// <summary>
        /// Query user's game sessions.
        /// By default, API will return a list of user's active game sessions (INVITED,JOINED,CONNECTED).
        /// </summary>
        /// <param name="statusFilter">Game session status to filter. Supported status: INVITED, JOINED, CONNECTED</param>
        /// <param name="orderBy">Order result by specific attribute. Supported: createdAt (default), updatedAt</param>
        /// <param name="sortDesc">
        /// Is descending order of the result. Default is true. Ascending order if false.
        /// </param>
        /// <param name="callback">
        ///  Returns a paginated result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetUserGameSessions(SessionV2StatusFilter? statusFilter, SessionV2AttributeOrderBy? orderBy,
            bool? sortDesc, ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                _sessionApi.GetUserGameSessions(
                    statusFilter,
                    orderBy,
                    sortDesc,
                    callback));
        }

        #endregion
    }
}