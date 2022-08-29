// Copyright (c) 2020-2022 AccelByte Inc. All Rights Reserved.
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
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
            Assert.IsNotNull(leaderboardCode, 
                "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!");

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
            Assert.IsNotNull(leaderboardCode, 
                "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!");
            Assert.IsNotNull(userId, 
                "Can't query all time leaderboard ranking data! userId parameter is null!");

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
            Assert.IsNotNull(leaderboardCode, 
                "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!");
            Assert.IsNotNull(userId, 
                "Can't query all time leaderboard ranking data! userId parameter is null!");
            Assert.IsFalse(string.IsNullOrEmpty(additionalKey),
                "Can't query all time leaderboard ranking data! additionalKey paramater couldn't be empty");

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
    }
}
