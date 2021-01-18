// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine;
using AccelByte.Core;
using UnityEngine.Assertions;
using AccelByte.Models;
using System.Collections;

namespace AccelByte.Server
{
    public class ServerMatchmaking
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly IServerSession serverSession;
        private readonly string @namespace;
        private readonly ServerMatchmakingApi api;

        private Coroutine statusPollingCoroutine;
        private bool statusPollingActive = false;
        private ResultCallback<MatchmakingResult> statusPollingCallback;
        private string statusPollingMatchId;

        public ServerMatchmaking(ServerMatchmakingApi api, IServerSession serverSession, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(serverSession, "session parameter can not be null");
            Assert.IsNotNull(@namespace, "@namespace parameter can not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.coroutineRunner = coroutineRunner;
            this.serverSession = serverSession;
            this.@namespace = @namespace;
            this.api = api;
        }

        /// <summary>
        /// Enqueue Game Server to Joinable Session Queue. 
        /// this will make this server joinable by other parties while already in a session.
        /// </summary>
        /// <param name="body">the session's data (get this data from QuerySessionStatus)</param>
        /// <param name="callback">the result of this operation</param>
        public void EnqueueJoinableSession(MatchmakingResult body, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.serverSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.EnqueueJoinableSession(this.@namespace, serverSession.AuthorizationToken, body, callback));
        }

        /// <summary>
        /// Dequeue Game Server from Joinable Session Queue.
        /// this will make this server not joinable to other parties while already in a session.
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">the result of this operation</param>
        public void DequeueJoinableSession(string matchId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.serverSession.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            DequeueRequest body = new DequeueRequest(){match_id = matchId};

            coroutineRunner.Run(api.DequeueJoinableSession(this.@namespace, serverSession.AuthorizationToken, body, callback));
        }

        /// <summary>
        /// Add a user to session data
        /// </summary>
        /// <param name="channelName">The Channel's Name.</param>
        /// <param name="matchId">match/session ID.</param>
        /// <param name="userId">user ID to be added.</param>
        /// <param name="callback">the result of this operation.</param>
        /// <param name="partyId">optional, the party ID of the user to be added. if not listed user will be added to a new party.</param>
        public void AddUserToSession(string channelName, string matchId, string userId, ResultCallback callback, string partyId = "")
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.serverSession.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.AddUserToSession(this.@namespace, serverSession.AuthorizationToken, channelName, matchId, userId, partyId, callback));
        }

        /// <summary>
        /// Remove a user from session data
        /// </summary>
        /// <param name="channelName">The Channel's Name.</param>
        /// <param name="matchId">match/session ID.</param>
        /// <param name="userId">user ID to be removed.</param>
        /// <param name="callback">the result of this operation.</param>
        /// <param name="body">optional, the session's data</param>
        public void RemoveUserFromSession(string channelName, string matchId, string userId, ResultCallback callback, MatchmakingResult body = null)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if(!this.serverSession.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.RemoveUserFromSession(this.@namespace, serverSession.AuthorizationToken, channelName, matchId, userId, body, callback));
        }

        /// <summary>
        /// Get the session's data status
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">Result of this operation, will return Session Data as MatchmakingResult class</param>
        public void QuerySessionStatus(string matchId, ResultCallback<MatchmakingResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.serverSession.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(api.QuerySessionStatus(this.@namespace, serverSession.AuthorizationToken, matchId, callback));
        }

        /// <summary>
        /// activate session data polling in a certain time interval.
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">Result of this operation, will return Session Data as MatchmakingResult class</param>
        /// <param name="intervalSec">the interval of every session data get call in second</param>
        public void ActivateSessionStatusPolling(string matchId, ResultCallback<MatchmakingResult> callback, int intervalSec = 5)
        {
            if(statusPollingActive == false)
            {
                statusPollingCallback = callback;
                statusPollingMatchId = matchId;
                statusPollingActive = true;
                statusPollingCoroutine = coroutineRunner.Run(SessionStatusPolling(intervalSec));
            }
        }

        private IEnumerator SessionStatusPolling(int intervalSec = 5)
        {
            var waitTime = new WaitForSeconds(intervalSec);
            while(statusPollingActive)
            {
                QuerySessionStatus(statusPollingMatchId, statusPollingCallback);
                yield return waitTime;
            }
        }

        /// <summary>
        /// Deactivate session data polling
        /// </summary>
        public void DeactivateStatusPolling()
        {
            statusPollingActive = false;
            statusPollingCallback = null;
            if(statusPollingCoroutine != null)
            {
                coroutineRunner.Stop(statusPollingCoroutine);
                statusPollingCoroutine = null;
            }
        }
    }
}