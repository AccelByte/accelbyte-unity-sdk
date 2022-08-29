// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Reward : WrapperBase
    {
        private readonly RewardApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Reward( RewardApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunnerParameter can not be null. Construction failed");

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
        internal Reward( RewardApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// To get reward from a specific reward code
        /// </summary>
        /// <param name="rewardCode"> reward code</param>
        /// <param name="callback"> callback when get the response</param>
        public void GetRewardByRewardCode( string rewardCode
            , ResultCallback<RewardInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetRewardByRewardCode(rewardCode, callback));
        }

        /// <summary>
        /// Get reward from a specific reward id
        /// </summary>
        /// <param name="rewardId"> reward id</param>
        /// <param name="callback"> callback when get the response</param>
        public void GetRewardByRewardId( string rewardId
            , ResultCallback<RewardInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetRewardByRewardId(rewardId, callback));
        }

        /// <summary>
        /// Query all rewards related to the event reward topic
        /// </summary>
        /// <param name="eventTopic"> event topic</param>
        /// <param name="sortBy"> sortBy: namespace, namespace:asc, namespace:desc, rewardCode, rewardCode:asc, rewardCode:desc</param>
        /// <param name="callback"> callback when get the response</param>
        /// <param name="offset"> offset of the reward</param>
        /// <param name="limit"> limit offset of the reward</param>
        public void QueryRewards( string eventTopic
            , RewardSortBy sortBy
            , ResultCallback<QueryRewardInfo> callback
            , int offset = 0
            , int limit = 20 )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryRewards(
                    eventTopic,
                    offset,
                    limit,
                    sortBy,
                    callback));
        }
    }
}