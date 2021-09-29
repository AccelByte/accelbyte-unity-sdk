// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Reward
    {
        private readonly RewardApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Reward(RewardApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns parameter can't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunnerParameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// To get reward from a specific reward code
        /// </summary>
        /// <param name="rewardCode"> reward code</param>
        /// <param name="callback"> callback when get the response</param>
        public void GetRewardByRewardCode(string rewardCode, ResultCallback<RewardInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetRewardByRewardCode(
                    (@namespace == "") ? this.@namespace : @namespace,
                    this.session.AuthorizationToken,
                    rewardCode,
                    callback));
        }

        /// <summary>
        /// Get reward from a specific reward id
        /// </summary>
        /// <param name="rewardId"> reward id</param>
        /// <param name="callback"> callback when get the response</param>
        public void GetRewardByRewardId(string rewardId, ResultCallback<RewardInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetRewardByRewardId(
                    (@namespace == "") ? this.@namespace : @namespace,
                    this.session.AuthorizationToken,
                    rewardId,
                    callback));
        }

        /// <summary>
        /// Query all rewards related to the event reward topic
        /// </summary>
        /// <param name="eventTopic"> event topic</param>
        /// <param name="sortBy"> sortBy: namespace, namespace:asc, namespace:desc, rewardCode, rewardCode:asc, rewardCode:desc</param>
        /// <param name="callback"> callback when get the response</param>
        /// <param name="offset"> offset of the reward</param>
        /// <param name="limit"> limit offset of the reward</param>
        public void QueryRewards(string eventTopic, RewardSortBy sortBy, ResultCallback<QueryRewardInfo> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.QueryRewards(
                    (@namespace == "") ? this.@namespace : @namespace,
                    this.session.AuthorizationToken,
                    eventTopic,
                    offset,
                    limit,
                    sortBy,
                    callback));
        }
    }
}