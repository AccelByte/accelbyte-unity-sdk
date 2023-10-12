// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Leaderboard : WrapperBase
    {
        private readonly LeaderboardApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Leaderboard( LeaderboardApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Leaderboard( LeaderboardApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="timeFrame">The time frame of leaderboard</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        /// <param name="callback">
        /// Returns a Result that contains LeaderboardRankingResult via callback when completed
        /// </param>
        public void GetRankings( string leaderboardCode
            , LeaderboardTimeFrame timeFrame
            , int offset
            , int limit
            , ResultCallback<LeaderboardRankingResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(leaderboardCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!"));
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetRankings(
                    leaderboardCode,
                    timeFrame,
                    offset,
                    limit,
                    callback));
        }

        /// <summary>
        /// Get user's ranking from leaderboard.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRankingData via callback when completed
        /// </param>
        public void GetUserRanking( string userId
            , string leaderboardCode
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(leaderboardCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!"));
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
                api.GetUserRanking(
                    leaderboardCode,
                    userId,
                    callback));
        }

        /// <summary>
        /// Get user's ranking from leaderboard with additional key. The additional key will be
        /// suffixed to the userId to access multi level user ranking, such as character ranking.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="additionalKey">
        /// To identify multi level user ranking, such as character ranking
        /// </param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRankingData via callback when completed
        /// </param>
        public void GetUserRanking( string userId
            , string additionalKey
            , string leaderboardCode
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(leaderboardCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!"));
                return;
            }

            if (string.IsNullOrEmpty(additionalKey))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't query all time leaderboard ranking data! additionalKey paramater couldn't be empty"));
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
                api.GetUserRanking(
                    leaderboardCode,
                    string.Format("{0}_{1}", userId, additionalKey),
                    callback));
        }

        /// <summary>
        /// List all leaderboard by given namespace
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains LeaderboardPagedList via callback when completed</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetLeaderboardList( ResultCallback<LeaderboardPagedList> callback
            , int offset = 0
            , int limit = 0 )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLeaderboardList(
                    offset,
                    limit,
                    callback));
        }

        /// <summary>
        /// List all leaderboard by given namespace
        /// </summary>
        /// <param name="callback">Returns a Result that contains LeaderboardPagedListV3 via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetLeaderboardListV3(ResultCallback<LeaderboardPagedListV3> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLeaderboardListV3(offset, limit, callback));
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        public void GetRangkingsV3(string leaderboardCode, ResultCallback<LeaderboardRankingResult> callback,
            int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetRangkingsV3(leaderboardCode, offset, limit, callback));
        }

        /// <summary>
        /// Get leaderboard's ranking list for specific cycle
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="cycleId">The id of leaderboard cycle</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        public void GetRankingsByCycle(string leaderboardCode, string cycleId,
            ResultCallback<LeaderboardRankingResult> callback, int offset = 0, int limit = 20)
        {
            coroutineRunner.Run(api.GetRankingsByCycle(leaderboardCode, cycleId, offset, limit, callback));
        }
        
        /// <summary>
        /// Get user's ranking from leaderboard with additional key. The additional key will be
        /// suffixed to the userId to access multi level user ranking, such as character ranking. 
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        public void GetUserRankingV3( string userId
            , string leaderboardCode
            , ResultCallback<UserRankingDataV3> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(leaderboardCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!"));
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
                api.GetUserRankingV3(
                    leaderboardCode,
                    userId,
                    callback));
        }
    }
}
