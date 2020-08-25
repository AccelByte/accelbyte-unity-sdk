// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Leaderboard
    {
        private readonly string @namespace;
        private readonly LeaderboardApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Leaderboard(LeaderboardApi api, ISession session, string ns, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(ns), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = ns;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode"> The id of the leaderboard </param>
        /// <param name="timeFrame"> The time frame of leaderboard </param>
        /// <param name="offset"> Offset of the list that has been sliced based on Limit parameter (optional, default = 0) </param>
        /// <param name="limit"> The limit of item on page (optional) </param>
        /// <param name="callback"> Returns a Result that contains LeaderboardRankingResult via callback when completed </param>
        public void GetRankings(string leaderboardCode, LeaderboardTimeFrame timeFrame, int offset, int limit, ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(leaderboardCode, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetRankings(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    leaderboardCode,
                    timeFrame,
                    offset,
                    limit,
                    callback));
        }

        /// <summary>
        /// Get user's ranking from leaderboard.
        /// </summary>
        /// <param name="userId"> The id of the user </param>
        /// <param name="leaderboardCode"> The id of the leaderboard </param>
        /// <param name="callback"> Returns a Result that contains UserRankingData via callback when completed </param>
        public void GetUserRanking(string userId, string leaderboardCode, ResultCallback<UserRankingData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(leaderboardCode, "Can't query all time leaderboard ranking data! leaderboardCode parameter is null!");
            Assert.IsNotNull(userId, "Can't query all time leaderboard ranking data! userId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserRanking(
                    this.@namespace,
                    this.session.AuthorizationToken,
                    leaderboardCode,
                    userId,
                    callback));
        }
    }
}
