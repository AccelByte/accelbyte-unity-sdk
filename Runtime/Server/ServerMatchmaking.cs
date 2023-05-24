// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using UnityEngine;
using AccelByte.Core;
using UnityEngine.Assertions;
using AccelByte.Models;
using System.Collections;

namespace AccelByte.Server
{
    public class ServerMatchmaking : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly ServerMatchmakingApi api;

        private Coroutine statusPollingCoroutine;
        private bool statusPollingActive = false;
        private ResultCallback<MatchmakingResult> statusPollingCallback;
        private string statusPollingMatchId;

        [UnityEngine.Scripting.Preserve]
        internal ServerMatchmaking( ServerMatchmakingApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner==null (@ constructor)");

            coroutineRunner = inCoroutineRunner;
            session = inSession;
            api = inApi;
        }

        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        /// <param name="session"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="coroutineRunner"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerMatchmaking( ServerMatchmakingApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Enqueue Game Server to Joinable Session Queue. 
        /// this will make this server joinable by other parties while already in a session.
        /// </summary>
        /// <param name="body">the session's data (get this data from QuerySessionStatus)</param>
        /// <param name="callback">the result of this operation</param>
        public void EnqueueJoinableSession( MatchmakingResult body
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.EnqueueJoinableSession(body, callback));
        }

        /// <summary>
        /// Dequeue Game Server from Joinable Session Queue.
        /// this will make this server not joinable to other parties while already in a session.
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">the result of this operation</param>
        public void DequeueJoinableSession( string matchId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            DequeueRequest body = new DequeueRequest(){match_id = matchId};

            coroutineRunner.Run(api.DequeueJoinableSession(body, callback));
        }

        /// <summary>
        /// Add a user to session data
        /// </summary>
        /// <param name="channelName">The Channel's Name.</param>
        /// <param name="matchId">match/session ID.</param>
        /// <param name="userId">user ID to be added.</param>
        /// <param name="callback">the result of this operation.</param>
        /// <param name="partyId">optional, the party ID of the user to be added. if not listed user will be added to a new party.</param>
        public void AddUserToSession( string channelName
            , string matchId
            , string userId
            , ResultCallback callback
            , string partyId = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.AddUserToSession(channelName, matchId, userId, partyId, callback));
        }

        /// <summary>
        /// Remove a user from session data
        /// </summary>
        /// <param name="channelName">The Channel's Name.</param>
        /// <param name="matchId">match/session ID.</param>
        /// <param name="userId">user ID to be removed.</param>
        /// <param name="callback">the result of this operation.</param>
        /// <param name="body">optional, the session's data</param>
        public void RemoveUserFromSession( string channelName
            , string matchId
            , string userId
            , ResultCallback callback
            , MatchmakingResult body = null )
        {
            Report.GetFunctionLog(GetType().Name);

            if(!session.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.RemoveUserFromSession(channelName, matchId, userId, body, callback));
        }

        /// <summary>
        /// Get the session's data status
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">Result of this operation, will return Session Data as MatchmakingResult class</param>
        public void QuerySessionStatus( string matchId
            , ResultCallback<MatchmakingResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                Debug.Log("Server session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.QuerySessionStatus(matchId, callback));
        }

        /// <summary>
        /// activate session data polling in a certain time interval.
        /// </summary>
        /// <param name="matchId">the match/session ID</param>
        /// <param name="callback">Result of this operation, will return Session Data as MatchmakingResult class</param>
        /// <param name="intervalSec">the interval of every session data get call in second</param>
        public void ActivateSessionStatusPolling( string matchId
            , ResultCallback<MatchmakingResult> callback
            , int intervalSec = 5 )
        {
            if(statusPollingActive == false)
            {
                statusPollingCallback = callback;
                statusPollingMatchId = matchId;
                statusPollingActive = true;
                statusPollingCoroutine = coroutineRunner.Run(SessionStatusPolling(intervalSec));
            }
        }

        private IEnumerator SessionStatusPolling( int intervalSec = 5 )
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