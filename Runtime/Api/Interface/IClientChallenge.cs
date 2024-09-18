// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api.Interface
{
    public interface IClientChallenge
    {
        /// <summary>
        /// Send a request to get all challenges.
        /// </summary>
        /// <param name="callback">Api Result Callback</param>
        /// <param name="status">Determines how the returned challenge list should be filtered</param>
        /// <param name="sortBy">Determines the returned sort order of the challenges</param>
        /// <param name="offset">Number of challenges to skip when returning the challenge list, defaults to 0</param>
        /// <param name="limit">Number of challenges that should be included in the challenge list, defaults to 20</param>
        public void GetChallenges(ResultCallback<ChallengeResponse> callback
            , ChallengeStatus status = ChallengeStatus.None
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Send a request to get all scheduled goals for a specific challenge
        /// </summary>
        /// <param name="challengeCode">String code for the challenge to check scheduled goals for</param>
        /// <param name="callback">Api Result Callback</param>
        /// <param name="tags">Array of tag strings used to filter resulting goals list</param>
        /// <param name="offset">Number of scheduled goals to skip when returning goals list, defaults to 0</param>
        /// <param name="limit">Number of scheduled goals that should be included in the goals list, defaults to 20</param>
        public void GetScheduledChallengeGoals(string challengeCode
            , ResultCallback<GoalResponse> callback
            , string[] tags = null
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Send a request to get current progress for a specific challenge
        /// </summary>
        /// <param name="challengeCode">String code for the challenge to check progress for</param>
        /// <param name="goalCode">String code for the specific challenge goal to get progress for</param>
        /// <param name="callback">Api Result Callback</param>
        /// <param name="offset">Number of scheduled goals to skip when returning goals list, defaults to 0</param>
        /// <param name="limit">Number of scheduled goals that should be included in the goals list, defaults to 20</param>
        public void GetChallengeProgress(string challengeCode
            , string goalCode
            , ResultCallback<GoalProgressionResponse> callback
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Send a request to get progress for a specific challenge in a previous rotation cycle.
        /// </summary>
        /// <param name="challengeCode">String code for the challenge to check progress for</param>
        /// <param name="rotationIndex">Index for rotation, treat rotation as an array sorted by the start time of a rotation in descending manner. 
        /// 0 indicates current or latest active rotation. Increment the value to refer to specific past rotation</param>
        /// <param name="callback">Api Result Callback</param>
        /// <param name="goalCode">String code for the specific challenge goal to get progress for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public void GetChallengeProgress(string challengeCode
            , int rotationIndex
            , ResultCallback<GoalProgressionResponse> callback
            , string goalCode = ""
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Send a request to get status of all challenge rewards
        /// </summary>
        /// <param name="status">Determines what specific rewards should be included in the response</param>
        /// <param name="callback">Api Result Callback</param>
        /// <param name="sortBy">Determines the order in which the rewards in the response are returned</param>
        /// <param name="offset">Number of rewards to skip when returning reward list, defaults to 0</param>
        /// <param name="limit">Number of rewards to include when returning reward list, defaults to 20</param>
        public void GetRewards(ChallengeRewardStatus status
            , ResultCallback<UserRewards> callback
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Send a request to claim a single reward
        /// </summary>
        /// <param name="rewardIDs">List of rewardID to be claimed</param>
        /// <param name="callback">Api Result Callback</param>
        public void ClaimReward(string[] rewardIDs
            , ResultCallback<UserReward[]> callback);

        /// <summary>
        /// Send a request to attempt to evaluate the current user's challenge progress
        /// </summary>
        /// <param name="callback">ResultCallback if operation is successful or not</param>
        public void EvaluateChallengeProgress(ResultCallback callback);
    }
}