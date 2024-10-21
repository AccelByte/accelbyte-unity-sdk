// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AccelByte.Api.Interface
{
    public interface IClientSession
    {
        #region PartySession

        /// <summary>
        /// Get Party details with partyId.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void GetPartyDetails(string partyId, ResultCallback<SessionV2PartySession> callback);

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
            ResultCallback<SessionV2PartySession> callback);

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
            ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Invite a user to a party.
        /// </summary>
        /// <param name="partyId">Targeted party ID</param>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void InviteUserToParty(string partyId, string userId,
            ResultCallback callback);

        /// <summary>
        /// Promotes a party member to be a party leader. Only leader can promote a new leader.
        /// </summary>
        /// <param name="partyId">Targeted party ID</param>
        /// <param name="leaderId">Targeted user ID of new leader</param>
        /// <param name="callback">Returns a Result that contain SessionV2PartySession via callback when completed</param>
        public void PromoteUserToPartyLeader(string partyId, string leaderId,
            ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Join a party.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">
        /// Returns a Result that contain SessionV2PartySession via callback when completed.
        /// </param>
        public void JoinParty(string partyId, ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Leave a Party.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void LeaveParty(string partyId, ResultCallback callback);

        /// <summary>
        /// Reject a party invitation.
        /// </summary>
        /// <param name="partyId">Targeted party ID.</param>
        /// <param name="callback">Returns a Result with status code</param>
        public void RejectPartyInvitation(string partyId, ResultCallback callback);

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
            ResultCallback<SessionV2PartySessionKickResponse> callback);

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
            ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Get all parties of a user(invoker).
        /// </summary>
        /// <param name="callback">
        /// Returns a paginated result of SessionV2PartySession via callback when completed.
        /// </param>
        public void GetUserParties(ResultCallback<PaginatedResponse<SessionV2PartySession>> callback);

        /// <summary>
        /// Join a party using a code.
        /// </summary>
        /// <param name="code">Party code.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void JoinPartyByCode(string code
            , ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Generate new code for a party.
        /// </summary>
        /// <param name="partyId">Party ID.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void GenerateNewPartyCode(string partyId
            , ResultCallback<SessionV2PartySession> callback);

        /// <summary>
        /// Revoke party code.
        /// </summary>
        /// <param name="sessionId">Party ID.</param>
        /// <param name="callback">Returns a result of SessionV2PartySession via callback when completed.</param>
        public void RevokePartyCode(string sessionId
            , ResultCallback callback);

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
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Query Game Session
        /// By default, API will return a list of available game sessions
        /// </summary>
        /// <param name="request">List of attributes to filter from available sessions</param>
        /// <param name="callback">
        /// Returns a paginated result of SessionV2GameSession via callback when completed.
        /// </param>
        public void QueryGameSession(Dictionary<string, object> request
            , ResultCallback<PaginatedResponse<SessionV2GameSession>> callback);

        /// <summary>
        /// Get game session detail by podName.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="podName">Targeted game session's podName</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetGameSessionDetailsByPodName(string podName
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Get game session detail by sessionId.
        /// Session service has several DSInformation status to track DS request to DSMC
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void GetGameSessionDetailsBySessionId(string sessionId
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Delete a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result via callback when completed.
        /// </param>
        public void DeleteGameSession(string sessionId
            , ResultCallback callback);

        /// <summary>
        /// Patch updates a game session, only specified fields from game session data.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="request">Compiled request of updating game session details</param>
        /// <param name="callback">
        /// Returns a result of updated SessionV2GameSession via callback when completed.
        /// </param>
        public void PatchGameSession(string sessionId, SessionV2GameSessionUpdateRequest request
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Invite a user to a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="userId">Targeted user's userId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void InviteUserToGameSession(string sessionId, string userId
            , ResultCallback callback);

        /// <summary>
        /// Join a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void JoinGameSession(string sessionId
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Leave a game session.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result of SessionV2GameSession via callback when completed.
        /// </param>
        public void LeaveGameSession(string sessionId
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Reject a game session invitation.
        /// </summary>
        /// <param name="sessionId">Targeted game session's sessionId</param>
        /// <param name="callback">
        /// Returns a result via callback when completed.
        /// </param>
        public void RejectGameSessionInvitation(string sessionId
            , ResultCallback callback);

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
            bool? sortDesc, ResultCallback<PaginatedResponse<SessionV2GameSession>> callback);

        /// <summary>
        /// Join a game session using a code.
        /// </summary>
        /// <param name="code">Game session code.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void JoinGameSessionByCode(string code
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Generate new code for a game session.
        /// </summary>
        /// <param name="sessionId">Game session ID.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void GenerateNewGameSessionCode(string sessionId
            , ResultCallback<SessionV2GameSession> callback);

        /// <summary>
        /// Revoke game session code.
        /// </summary>
        /// <param name="sessionId">Game session ID.</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void RevokeGameSessionCode(string sessionId
            , ResultCallback callback);

        /// <summary>
        /// Promote user to be a game session leader.
        /// </summary>
        /// <param name="sessionId">Game session ID.</param>
        /// <param name="leaderId">Targeted user ID of new leader</param>
        /// <param name="callback">Returns a result of SessionV2GameSession via callback when completed.</param>
        public void PromoteUserToGameSessionLeader(string sessionId, string leaderId,
            ResultCallback<SessionV2GameSession> callback);

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
        public void UpdateLeaderStorage(string sessionId, JObject data, ResultCallback<JObject> callback);

        /// <summary>
        /// Update current user's session member storage data.
        /// This will overwrite this user's storage data, if updating also provide latest user's storage.
        /// To clear current user's storage data update with empty jsonObject.
        /// </summary>
        /// <param name="sessionId">The game or party session id.</param>
        /// <param name="data">Data to update leader storage.</param>
        /// <param name="callback">Callback will be called when completed.</param>
        public void UpdateMemberStorage(string sessionId, JObject data, ResultCallback<JObject> callback);

        #endregion
    }
}
