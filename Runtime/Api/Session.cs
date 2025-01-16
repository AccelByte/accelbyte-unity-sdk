// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Api.Interface;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;

namespace AccelByte.Api
{
    public class Session : WrapperBase, IClientSession
    {
        private readonly SessionApi sessionApi;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;
        private System.Threading.CancellationTokenSource cts;

        [UnityEngine.Scripting.Preserve]
        internal Session(SessionApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inCoroutineRunner);

            sessionApi = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
            cts = new System.Threading.CancellationTokenSource();
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            SharedMemory?.MessagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.MatchFoundOnPoll, OnTicketPollMatchFound);
            base.SetSharedMemory(newSharedMemory);

            SharedMemory?.MessagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.MatchFoundOnPoll, OnTicketPollMatchFound);
        }

        ~Session()
        {
            Reset();
        }

        internal void Reset()
        {
            cts.Cancel();
        }

#region PartySession
        public void GetPartyDetails(string partyId, ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetPartyDetails(
                    partyId,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.UpdateParty(
                    partyId,
                    request,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PatchUpdateParty(
                    partyId,
                    request,
                    callback));
        }

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
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "User id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ResultCallback<InviteUserToPartyResponse> inviteUserCallback = (result) =>
            {
                if (result != null && !result.IsError)
                {
                    SendPredefinedEvent(PredefinedAnalyticsMode.PartySessionInvite, userId, partyId);
                }

                HandleCallback(result, callback);
            };
            sessionApi.InviteUserToParty(
                partyId,
                userId,
                inviteUserCallback);
        }        
        
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
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "Leader id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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

        public void JoinParty(string partyId, ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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

        public void LeaveParty(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.LeaveParty(
                    partyId,
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SharedMemory?.MessagingSystem?.SendMessage(AccelByteMessagingTopic.OnLeftPartyNotification, session.UserId);
                            SendPredefinedEvent(PredefinedAnalyticsMode.PartySessionLeft, session.UserId, partyId);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        public void RejectPartyInvitation(string partyId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(partyId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetPartyIdInvalidMessage(partyId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RejectPartyInvitation(
                    partyId,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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

        public void CreateParty(SessionV2PartySessionCreateRequest request,
            ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(request, "SessionV2PartySessionCreateRequest cannot be null.");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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

        public void GetUserParties(ResultCallback<PaginatedResponse<SessionV2PartySession>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetUserParties(
                    callback));
        }

        public void JoinPartyByCode(string code
            , ResultCallback<SessionV2PartySession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "code cannot be null");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GenerateNewPartyCode(partyId, callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RevokePartyCode(sessionId, callback));
        }

        public void CancelPartyInvitation(string partyId
            , string userId
            , ResultCallback callback)
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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            sessionApi.CancelPartyInvitation(partyId, userId, callback);
        }

#endregion

#region GameSession

        public void CreateGameSession(SessionV2GameSessionCreateRequest request
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(request, "SessionV2GameSessionCreateRequest cannot be null");
            Assert.IsNotNull(request.configurationName, "configurationName cannot be null");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.CreateGameSession(
                    request,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SharedMemory?.PastSessionRecordManager?
                                .InsertPastSessionId(session.UserId, cb.Value.id);
                            SharedMemory?.MessagingSystem?.SendMessage(AccelByteMessagingTopic.OnJoinedGameSessionNotification, session.UserId);
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionCreate);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.QueryGameSession(
                    request,
                    callback));
        }

        public void GetGameSessionDetailsByPodName(string podName
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(podName, "podName cannot be null");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetGameSessionDetailsByPodName(
                    podName,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetGameSessionDetailsBySessionId(
                    sessionId,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.DeleteGameSession(
                    sessionId,
                    callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PatchGameSession(
                    sessionId,
                    request,
                    callback));
        }

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
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "User id cannot be null or empty"));
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            ResultCallback<InviteUserToGameSessionResponse> inviteUserCallback = (result) =>
            {
                if (result != null && !result.IsError)
                {
                    SendPredefinedEvent(PredefinedAnalyticsMode.GameSessionInvite, userId, sessionId);
                }

                HandleCallback(result, callback);
            };
            sessionApi.InviteUserToGameSession(
                sessionId,
                userId,
                inviteUserCallback);
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinGameSession(
                    sessionId,
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SharedMemory.PastSessionRecordManager?
                                .InsertPastSessionId(session.UserId, cb.Value.id);
                            SharedMemory?.MessagingSystem?.SendMessage(AccelByteMessagingTopic.OnJoinedGameSessionNotification, session.UserId);
                            SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionJoin);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

        public void LeaveGameSession(string sessionId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.LeaveGameSession(
                    sessionId,
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendPredefinedEvent(PredefinedAnalyticsMode.GameSessionLeft, session.UserId, sessionId);
                        }
                        HandleCallback(cb, callback);
                    }));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.RejectGameSessionInvitation(
                    sessionId,
                    callback));
        }

        public void GetUserGameSessions(SessionV2StatusFilter? statusFilter, SessionV2AttributeOrderBy? orderBy,
            bool? sortDesc, ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GetUserGameSessions(
                    statusFilter,
                    orderBy,
                    sortDesc,
                    callback));
        }

        public void JoinGameSessionByCode(string code
            , ResultCallback<SessionV2GameSession> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(code, "code cannot be null");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.JoinGameSessionByCode(code, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SharedMemory?.PastSessionRecordManager?
                            .InsertPastSessionId(session.UserId, cb.Value.id);
                        SendPredefinedEvent(cb, PredefinedAnalyticsMode.GameSessionJoin);
                    }
                    HandleCallback(cb, callback);
                }));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.GenerateNewGameSessionCode(sessionId, callback));
        }

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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.PromoteUserToGameSessionLeader(sessionId, leaderId, callback));
        }

        private void OnTicketPollMatchFound(string sessionId)
        {
            coroutineRunner.Run(OnTicketPollMatchFoundAsync(sessionId));
        }

        private IEnumerator OnTicketPollMatchFoundAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                AccelByteDebug.LogError("Session ID from polled ticket is invalid");
                yield break;
            }

            bool isMatchFoundNotifSent = false;
            bool isDsStatusChangedNotifSent = false;
            bool shouldContinuePolling = true;
            while (shouldContinuePolling && !cts.IsCancellationRequested)
            {
                sessionApi.GetGameSessionDetailsBySessionIdInternal(sessionId, result =>
                {
                    if (result.IsError || cts.IsCancellationRequested)
                    {
                        shouldContinuePolling = false;
                        return;
                    }

                    if (!isMatchFoundNotifSent && result.Value.ticketIds != null)
                    {
                        var tickets = new MatchmakingV2Ticket[result.Value.ticketIds.Length];
                        for (int i = 0; i < result.Value.ticketIds.Length; i++)
                        {
                            tickets[i] = new MatchmakingV2Ticket() { ticketId = result.Value.ticketIds[i] };
                        }

                        var matchFoundNotif = new MatchmakingV2MatchFoundNotification()
                        {
                            id = result.Value.id,
                            teams = result.Value.teams,
                            matchPool = result.Value.matchPool,
                            tickets = tickets,
                            namespace_ = sessionApi.Config.Namespace,
                            createdAt = System.DateTime.UtcNow
                        };

                        var matchFoundNotifMessage = AccelByteNotificationSenderUtility.ComposeMMv2Notification(
                            "OnMatchFound"
                            , matchFoundNotif.ToJsonString()
                            , isEncoded: true);
                        SharedMemory.NotificationSender.SendLobbyNotification(matchFoundNotifMessage);

                        isMatchFoundNotifSent = true;
                    }

                    if (result.Value.dsInformation.StatusV2 == SessionV2DsStatus.ENDED)
                    {
                        shouldContinuePolling = false;
                        return;
                    }

                    if (result.Value.dsInformation.StatusV2 == SessionV2DsStatus.AVAILABLE ||
                        result.Value.dsInformation.StatusV2 == SessionV2DsStatus.FAILED_TO_REQUEST)
                    {
                        if (!isDsStatusChangedNotifSent)
                        {
                            var dsStatusChangedNotif = new SessionV2DsStatusUpdatedNotification()
                            {
                                session = result.Value, sessionId = result.Value.id, error = null
                            };
                            var dsStatusChangedNotifMessage =
                                AccelByteNotificationSenderUtility.ComposeMMv2Notification(
                                    "OnDSStatusChanged"
                                    , dsStatusChangedNotif.ToJsonString()
                                    , isEncoded: true);

                            SharedMemory.NotificationSender.SendLobbyNotification(dsStatusChangedNotifMessage);

                            isDsStatusChangedNotifSent = true;
                        }

                        if (result.Value.dsInformation.StatusV2 == SessionV2DsStatus.AVAILABLE)
                        {
                            var sessionInviteNotif = new SessionV2GameInvitationNotification()
                            {
                                sessionId = result.Value.id
                            };

                            var sessionInviteNotifMessage =
                                AccelByteNotificationSenderUtility.ComposeSessionNotification(
                                    "OnSessionInvited"
                                    , sessionInviteNotif.ToJsonString()
                                    , isEncoded: true);
                            SharedMemory.NotificationSender.SendLobbyNotification(sessionInviteNotifMessage);
                        }

                        shouldContinuePolling = false;
                        return;
                    }
                });

                if (shouldContinuePolling)
                {
                    yield return new WaitForSeconds(sessionApi.Config.MatchmakingTicketCheckPollRate / 1000f);
                }
            }
        }

#endregion

#region SessionStorage

        public void UpdateLeaderStorage(string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                sessionApi.UpdateLeaderStorage(sessionId, data, callback));
        }

        public void UpdateMemberStorage(string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
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

                case PredefinedAnalyticsMode.GameSessionLeft:
                    payload = new PredefinedGameSessionV2LeftPayload(userId, sessionId);
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
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }
        
        private void HandleCallback<T>(Result<T> result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.TryOk();
        }

#endregion

#region Player
        /// <summary>
        /// Get player attributes.
        /// </summary>
        public void GetPlayerAttributes(ResultCallback<PlayerAttributesResponseBody> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            sessionApi.GetPlayerAttributes(callback);
        }

        /// <summary>
        /// Set the behavior of player online session.
        /// </summary>
        public void StorePlayerAttributes(PlayerAttributesRequestBody requestBody, ResultCallback<PlayerAttributesResponseBody> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            sessionApi.StorePlayerAttributes(requestBody, callback);
        }

        /// <summary>
        /// Reset player attributes
        /// </summary>
        public void ResetPlayerAttributes(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            sessionApi.RemovePlayerAttributes(callback);
        }
#endregion
    }
}