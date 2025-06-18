// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Server.Interface
{
    public interface IServerChallenge
    {
        /// <summary>
        /// Retrieve a list of challenges in the current namespace.
        /// </summary>
        /// <param name="optionalParameters"></param>
        /// <param name="callback">Result callback of retrieved challenges</param>
        public void GetChallenges(GetChallengesOptionalParamenters optionalParameters
            , ResultCallback<ChallengeResponse> callback);

        /// <summary>
        /// Retrieve a list of challenges in the current namespace.
        /// </summary>
        /// <param name="optionalParameters"></param>
        /// <param name="callback">Result callback of retrieved challenges</param>
        public void GetChallenges(ResultCallback<ChallengeResponse> callback);

        /// <summary>
        /// Create a new challenge in the current namespace.
        /// </summary>
        /// <param name="createChallengeRequest">Create challenge request model</param>
        /// <param name="callback">Result callback of the created challenge</param>
        public void CreateChallenge(CreateChallengeRequest createChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback);

        /// <summary>
        /// Retrieve a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to be retrieved</param>
        /// <param name="callback">Result callback of the retrieved challenge</param>
        public void GetChallenge(string challengeCode, ResultCallback<ChallengeResponseInfo> callback);

        /// <summary>
        /// Update a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to be updated</param>
        /// <param name="updateChallengeRequest">Update challenge request model</param>
        /// <param name="callback">Result callback of the updated challenge</param>
        public void UpdateChallenge(string challengeCode
            , UpdateChallengeRequest updateChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback);

        /// <summary>
        /// Delete a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to be updated</param>
        /// <param name="callback">Result callback if deletion is successful or not</param>
        public void DeleteChallenge(string challengeCode, ResultCallback callback);

        /// <summary>
        /// Retrieve a list of periods for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to retrieve periods for</param>
        /// <param name="callback">Result callback of periods for the specified challenge</param>
        /// <param name="limit">Number of periods to skip when returning period list (default: 0)</param>
        /// <param name="offset">Number of periods to include in the returned period list (default: 20)</param>
        public void GetChallengePeriods(string challengeCode
            , ResultCallback<ChallengePeriodResponse> callback
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Utility endpoint to execute randomize goal schedule for a specified challenge in the 
        /// current namespace that the AssignmentRule is set to RANDOMIZED and RandomizePerRotation 
        /// is set to true.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to randomize goals for</param>
        /// <param name="callback">Result callback of challenge that has its goals randomized</param>
        public void RandomizeChallengeGoals(string challengeCode
            , ResultCallback<RandomizedChallengeResponse[]> callback);

        /// <summary>
        /// Delete a specified TIED challenge along with its goals, schedules, and player progressions
        /// in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of TIED challenge to delete</param>
        /// <param name="callback">Result callback if deletion is successful or not</param>
        public void DeleteTiedChallenge(string challengeCode, ResultCallback callback);

        /// <summary>
        /// Retrieve a list of goals for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to retrieve goals for</param>
        /// <param name="callback">Result callback of retrieved goals</param>
        /// <param name="challengeSortBy">Determines the order in which the goals in the response are returned</param>
        /// <param name="offset">Number of goals to skip when returning goal list (default: 0)</param>
        /// <param name="limit">Number of goals to include in the returned goal list (default: 20)</param>
        public void GetChallengeGoals(string challengeCode
            , ResultCallback<GoalResponse> callback
            , ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20);

        /// <summary>
        /// Create a new goal for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to create a goal for</param>
        /// <param name="createChallengeGoalRequest">Create challenge goal request model</param>
        /// <param name="callback">Result callback of the created goal</param>
        public void CreateChallengeGoal(string challengeCode
            , CreateChallengeGoalRequest createChallengeGoalRequest
            , ResultCallback<GoalResponseInfo> callback);

        /// <summary>
        /// Retrieve a specified goal for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to retrieve a goal for</param>
        /// <param name="goalCode">String identifier of goal to retrieve</param>
        /// <param name="callback">Result callback of the retrieved goal</param>
        public void GetChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback<GoalResponseInfo> callback);

        /// <summary>
        /// Update a specified goal for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to update a goal for</param>
        /// <param name="goalCode">String identifier of goal to update</param>
        /// <param name="updateChallengeGoalRequest">Update challenge goal request model</param>
        /// <param name="callback">Result callback of the updated goal</param>
        public void UpdateChallengeGoal(string challengeCode
            , string goalCode
            , UpdateChallengeGoalRequest updateChallengeGoalRequest
            , ResultCallback<GoalResponseInfo> callback);

        /// <summary>
        /// Delete a specified goal for a specified challenge in the current namespace.
        /// </summary>
        /// <param name="challengeCode">String identifier of challenge to delete a goal for</param>
        /// <param name="goalCode">String identifier of goal to delete</param>
        /// <param name="callback">Result callback if deletion is successful or not</param>
        public void DeleteChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback callback);

        /// <summary>
        /// Send a request to attempt to evaluate many user's challenge progress
        /// </summary>
        /// <param name="userIds">List of the User IDs to be evaluated their challenge progress</param>
        /// <param name="callback">ResultCallback whether it success or failed</param>
        public void EvaluateChallengeProgress(string[] userIds
            , ResultCallback callback);

        /// <summary>
        /// Claim challenge rewards for multiple users.
        /// </summary>
        /// <param name="challengeBulkClaimRewardRequest">Bulk claim reward request model</param>
        /// <param name="callback">Result callback of each user's rewards</param>
        public void ClaimReward(ChallengeBulkClaimRewardRequest[] challengeBulkClaimRewardRequest
            , ResultCallback<ChallengeBulkClaimRewardResponse[]> callback);

        /// <summary>
        /// Claim challenge rewards for a single user.
        /// </summary>
        /// <param name="userId">User ID of user to claim rewards for</param>
        /// <param name="claimRewardRequest">Claim reward request model</param>
        /// <param name="callback">Result callback of the user's rewards</param>
        public void ClaimReward(string userId
            , ClaimRewardRequest claimRewardRequest
            , ResultCallback<UserReward[]> callback);

        /// <summary>
        /// Retrieve a list of a specified user's rewards in the current namespace.
        /// </summary>
        /// <param name="userId">User ID of user to retrieve rewards for</param>
        /// <param name="callback">Result callback of the user's rewards</param>
        /// <param name="challengeRewardStatus">Status of rewards to query/filter</param>
        /// <param name="challengeSortBy">Determines the order in which the rewards in the response are returned</param>
        /// <param name="offset">Number of rewards to skip when returning reward list (default: 0)</param>
        /// <param name="limit">Number of rewards to include in the returned reward list (default: 20)</param>
        public void GetUserRewards(string userId
            , ResultCallback<UserRewards> callback
            , ChallengeRewardStatus challengeRewardStatus = ChallengeRewardStatus.None
            , ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20);
    }
}