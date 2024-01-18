// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Session : WrapperBase
    {
        private readonly SessionApi sessionApi;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Session(SessionApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            sessionApi = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetPartyDetails(
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            Assert.IsNotNull(request, "SessionV2PartySessionUpdateRequest cannot be null");


            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.UpdateParty(
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            Assert.IsNotNull(request, "SessionV2PartySessionUpdateRequest cannot be null");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PatchUpdateParty(
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if(string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "User id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.InviteUserToParty(
                    partyId,
                    userId,
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendPredefinedEvent(PredefinedAnalyticsMode.PartySessionInvite, userId, partyId);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (string.IsNullOrEmpty(leaderId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Leader id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PromoteUserToPartyLeader(
                    partyId,
                    leaderId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.PartySessionUserPromoted);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinParty(
                    partyId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.PartySessionJoin);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Leave a Party.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void LeaveParty(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.LeaveParty(
                    partyId,
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendPredefinedEvent(PredefinedAnalyticsMode.PartySessionLeft, session.UserId, partyId);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Reject a party invitation.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void RejectPartyInvitation(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RejectPartyInvitation(
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

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            coroutineRunner.Run(
                sessionApi.KickUserFromParty(
                    partyId,
                    userId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.PartySessionKicked, userId);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
                
            coroutineRunner.Run(
                sessionApi.CreateParty(
                    request,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.PartySessionCreate);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetUserParties(
                    callback));
        }

        /// <summary>
        /// Join a party using a code.
        /// </summary>
        /// <param name="code">Party code.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void JoinPartyByCode(string code
            , ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "code cannot be null");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinPartyByCode(code, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb, PredefinedAnalyticsMode.PartySessionJoin);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Generate new code for a party.
        /// </summary>
        /// <param name="partyId">Party ID.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void GenerateNewPartyCode(string partyId
            , ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GenerateNewPartyCode(partyId, callback));
        }

        /// <summary>
        /// Revoke party code.
        /// </summary>
        /// <param name="sessionId">Party ID.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void RevokePartyCode(string sessionId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RevokePartyCode(sessionId, callback));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.CreateGameSession(
                    request,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionCreate);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.QueryGameSession(
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetGameSessionDetailsByPodName(
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

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetGameSessionDetailsBySessionId(
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.DeleteGameSession(
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
            Assert.IsNotNull(request, "SessionV2GameSessionUpdateRequest cannot be null");

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PatchGameSession(
                    sessionId,
                    request,
                    callback));
        }

        /// <summary>
        /// Invite a user to a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="userId">Targeted user's userId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void InviteUserToGameSession(string sessionId, string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "User id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.InviteUserToGameSession(
                    sessionId,
                    userId,
                    cb =>
                    {
                        if (!cb.IsError)
                        {
                            SendPredefinedEvent(PredefinedAnalyticsMode.GameSessionInvite, userId, sessionId);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinGameSession(
                    sessionId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionJoin);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.LeaveGameSession(
                    sessionId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionLeft);
                        }
                        HandleCallback(cb, callback);
                    }));
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

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RejectGameSessionInvitation(
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetUserGameSessions(
                    statusFilter,
                    orderBy,
                    sortDesc,
                    callback));
        }

        /// <summary>
        /// Join a game session using a code.
        /// </summary>
        /// <param name="code">Game session code.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void JoinGameSessionByCode(string code
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "code cannot be null");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinGameSessionByCode(code, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionJoin);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Generate new code for a game session.
        /// </summary>
        /// <param name="sessionId">Game session ID.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void GenerateNewGameSessionCode(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GenerateNewGameSessionCode(sessionId, callback));
        }

        /// <summary>
        /// Revoke game session code.
        /// </summary>
        /// <param name="sessionId">Game session ID.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void RevokeGameSessionCode(string sessionId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RevokeGameSessionCode(sessionId, callback));
        }

        public void PromoteUserToGameSessionLeader(string sessionId, string leaderId,
            ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PromoteUserToGameSessionLeader(sessionId, leaderId, callback));
        }

        #endregion

        #region SessionStorage

        /// <summary>
        /// Update leader's session storage data, can only be updated by current session leader.
        /// This will overwrite leader storage data, if updating also provide latest leader storage.
        /// To clear current leader storage data update with empty JObject.
        /// </summary>
        /// <param name="sessionId">The game or party session id.</param>
        /// <param name="data">Data to update leader storage.</param>
        /// <param name="callback">Callback will be called when completed.</param>
        public void UpdateLeaderStorage(string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.UpdateLeaderStorage(sessionId, data, callback));
        }

        /// <summary>
        /// Update current user's session member storage data.
        /// This will overwrite this user's storage data, if updating also provide latest user's storage.
        /// To clear current user's storage data update with empty jsonObject.
        /// </summary>
        /// <param name="sessionId">The game or party session id.</param>
        /// <param name="data">Data to update leader storage.</param>
        /// <param name="callback">Callback will be called when completed.</param>
        public void UpdateMemberStorage(string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.UpdateMemberStorage(session.UserId, sessionId, data, callback));
        }

        #endregion

        #region PredefinedEvents
        private enum PredefinedAnalyticsMode
        {
            GameSessionCreate,
            GameSessionInvite,
            GameSessionJoin,
            GameSessionLeft,
            PartySessionCreate,
            PartySessionInvite,
            PartySessionJoin,
            PartySessionLeft,
            PartySessionKicked,
            PartySessionUserPromoted
        }

        private IAccelByteTelemetryPayload CreatePredefinedPayload<T>(Result<T> apiCallResult, PredefinedAnalyticsMode mode, string targetUserId = null)
        {
            IAccelByteTelemetryPayload payload;
            string userId = (string.IsNullOrEmpty(targetUserId)) ? session.UserId : targetUserId;

            switch (mode)
            {
                case PredefinedAnalyticsMode.GameSessionCreate:
                    var sessionCreateResult = apiCallResult as Result<SessionV2GameSession>;
                    payload = new PredefinedGameSessionV2CreatedPayload(userId, sessionCreateResult.Value.id);
                    return payload;

                case PredefinedAnalyticsMode.GameSessionJoin:
                    var sessionJoinResult = apiCallResult as Result<SessionV2GameSession>;
                    payload = new PredefinedGameSessionV2JoinedPayload(userId, sessionJoinResult.Value.id);
                    return payload;

                case PredefinedAnalyticsMode.GameSessionLeft:
                    var sessionLeftResult = apiCallResult as Result<SessionV2GameSession>;
                    payload = new PredefinedGameSessionV2LeftPayload(userId, sessionLeftResult.Value.id);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionCreate:
                    var partyCreateResult = apiCallResult as Result<SessionV2PartySession>;
                    payload = new PredefinedPartySessionV2CreatedPayload(userId, partyCreateResult.Value.id);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionJoin:
                    var partyJoinResult = apiCallResult as Result<SessionV2PartySession>;
                    payload = new PredefinedPartySessionV2JoinedPayload(userId, partyJoinResult.Value.id);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionKicked:
                    var partyKickedResult = apiCallResult as Result<SessionV2PartySessionKickResponse>;
                    payload = new PredefinedPartySessionV2KickedPayload(userId, partyKickedResult.Value.partyId);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionUserPromoted:
                    var userPromotedResult = apiCallResult as Result<SessionV2PartySession>;
                    payload = new PredefinedPartySessionV2LeaderPromotedPayload(userPromotedResult.Value.leaderId, userPromotedResult.Value.id);
                    return payload;

                default:
                    return null;
            }
        }

        private IAccelByteTelemetryPayload CreatePredefinedPayload(PredefinedAnalyticsMode mode, string userId, string sessionId)
        {
            IAccelByteTelemetryPayload payload;

            switch (mode)
            {
                case PredefinedAnalyticsMode.GameSessionInvite:
                    payload = new PredefinedGameSessionV2InvitedPayload(userId, sessionId);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionInvite:
                    payload = new PredefinedPartySessionV2InvitedPayload(userId, sessionId);
                    return payload;

                case PredefinedAnalyticsMode.PartySessionLeft:
                    payload = new PredefinedPartySessionV2LeftPayload(userId, sessionId);
                    return payload;

                default:
                    return null;
            }
        }

        private void SendPredefinedEvent<T>(Result<T> result, PredefinedAnalyticsMode mode)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload(result, mode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent<T>(Result<T> result, PredefinedAnalyticsMode mode, string userId)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload(result, mode, userId);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(PredefinedAnalyticsMode mode, string userId, string sessionId)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload(mode, userId, sessionId);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            var predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        #endregion
    }
}