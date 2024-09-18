// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide an API to access Challenge service.
    /// </summary>
    public class ServerChallenge : WrapperBase, IServerChallenge
    {
        private readonly ServerChallengeApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerChallenge(ServerChallengeApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(inApi, "Cannot construct Challenge manager; api is null!");

            api = inApi;
            session = inSession;
        }

        public void ClaimReward(ChallengeBulkClaimRewardRequest[] challengeBulkClaimRewardRequest, ResultCallback<ChallengeBulkClaimRewardResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ClaimReward(challengeBulkClaimRewardRequest, callback);
        }

        public void ClaimReward(string userId, ClaimRewardRequest claimRewardRequest, ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ClaimReward(userId, claimRewardRequest, callback);
        }

        public void CreateChallenge(CreateChallengeRequest createChallengeRequest, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateChallenge(createChallengeRequest, callback);
        }

        public void CreateChallengeGoal(string challengeCode, CreateChallengeGoalRequest createChallengeGoalRequest, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateChallengeGoal(challengeCode, createChallengeGoalRequest, callback);
        }

        public void DeleteChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteChallenge(challengeCode, callback);
        }

        public void DeleteChallengeGoal(string challengeCode, string goalCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteChallengeGoal(challengeCode, goalCode, callback);
        }

        public void DeleteTiedChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteTiedChallenge(challengeCode, callback);
        }

        public void EvaluateChallengeProgress(string[] userIds
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (userIds == null || userIds.Length == 0)
            {
                callback?.TryError(ErrorCode.InvalidRequest);
                return;
            }

            ChallengeEvaluatePlayerProgressionRequest requestBody = new ChallengeEvaluatePlayerProgressionRequest()
            {
                UserIds = userIds
            };

            api.EvaluateChallengeProgress(requestBody, callback);
        }

        public void GetChallenge(string challengeCode, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenge(challengeCode, callback);
        }

        public void GetChallengeGoal(string challengeCode, string goalCode, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeGoal(challengeCode, goalCode, callback);
        }

        public void GetChallengeGoals(string challengeCode, ResultCallback<GoalResponse> callback, ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeGoals(challengeCode, callback, challengeSortBy, offset, limit);
        }

        public void GetChallengePeriods(string challengeCode, ResultCallback<ChallengePeriodResponse> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengePeriods(challengeCode, callback, offset, limit);
        }

        public void GetChallenges(ResultCallback<ChallengeResponse> callback, ChallengeStatus status = ChallengeStatus.None, ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenges(callback, status, sortBy, offset, limit);
        }

        public void GetUserRewards(string userId, ResultCallback<UserRewards> callback, ChallengeRewardStatus challengeRewardStatus = ChallengeRewardStatus.None, ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserRewards(userId, callback, challengeRewardStatus, challengeSortBy, offset, limit);
        }

        public void RandomizeChallengeGoals(string challengeCode, ResultCallback<ChallengeResponseInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RandomizeChallengeGoals(challengeCode, callback);
        }

        public void UpdateChallenge(string challengeCode, UpdateChallengeRequest updateChallengeRequest, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateChallenge(challengeCode, updateChallengeRequest, callback);
        }

        public void UpdateChallengeGoal(string challengeCode, string goalCode, UpdateChallengeGoalRequest updateChallengeGoalRequest, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateChallengeGoal(challengeCode, goalCode, updateChallengeGoalRequest, callback);
        }
    }
}