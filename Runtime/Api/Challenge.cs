// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api.Interface;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access Challenge service.
    /// </summary>
    public class Challenge : WrapperBase, IClientChallenge
    {
        private readonly ChallengeApi api;
        private readonly UserSession session;

        [UnityEngine.Scripting.Preserve]
        internal Challenge(ChallengeApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");

            api = inApi;
            session = inSession;
        }

        public void GetChallenges(ResultCallback<ChallengeResponse> callback
            , ChallengeStatus status = ChallengeStatus.None
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenges(sortBy, status, offset, limit, callback);
        }

        public void GetScheduledChallengeGoals(string challengeCode
            , ResultCallback<GoalResponse> callback
            , string[] tags = null
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetScheduledChallengeGoals(challengeCode, tags, offset, limit, callback);
        }

        public void GetChallengeProgress(string challengeCode
            , string goalCode
            , ResultCallback<GoalProgressionResponse> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeProgress(challengeCode, goalCode, offset, limit, callback);
        }

        public void GetRewards(ChallengeRewardStatus status            
            , ResultCallback<UserRewards> callback
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetRewards(status
                , sortBy
                , offset
                , limit
                , callback);
        }

        public void ClaimReward(string[] rewardIDs
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(rewardIDs);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ClaimRewardRequest body = new ClaimRewardRequest()
            {
                RewardIDs = rewardIDs
            };

            api.ClaimReward(body
                , callback);
        }

        public void GetChallengeProgress(string challengeCode
            , int rotationIndex
            , ResultCallback<GoalProgressionResponse> callback
            , string goalCode = ""
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeProgress(challengeCode
                , rotationIndex
                , callback
                , goalCode
                , offset
                , limit);
        }

        public void EvaluateChallengeProgress(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.EvaluateChallengeProgress(callback);
        }
    }
}