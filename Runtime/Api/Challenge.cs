// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api.Interface;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;
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

        [Obsolete("This interface is deprecated, and will be removed on AGS 2025.4. Please use GetChallenges(optionalParameters, callback).")]
        public void GetChallenges(ResultCallback<ChallengeResponse> callback
            , ChallengeStatus status = ChallengeStatus.None
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            var optionalParams = new GetChallengesOptionalParamenters()
            {
                Limit = limit,
                Offset = offset,
                SortBy = sortBy,
                Status = status
            };

            GetChallenges(optionalParams, callback);
        }

        public void GetChallenges(GetChallengesOptionalParamenters optionalParameters
            , ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenges(optionalParameters, callback);
        }

        public void GetChallenges(ResultCallback<ChallengeResponse> callback)
        {
            GetChallenges(null, callback);
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

        public void ListScheduleByGoal(string challengeCode
            , string goalCode
            , ResultCallback<ChallengeListScheduleByGoalResponse> callback)
        {
            ListScheduleByGoal(challengeCode, goalCode, null, callback);
        }

        public void ListScheduleByGoal(string challengeCode
            , string goalCode
            , ChallengeListScheduleByGoalOptionalParameters optionalParams
            , ResultCallback<ChallengeListScheduleByGoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ListScheduleByGoal(challengeCode, goalCode, optionalParams , callback);
        }

        public void ListSchedules(string challengeCode
            , ResultCallback<ChallengeListSchedulesResponse> callback)
        {
            ListSchedules(challengeCode, null, callback);
        }

        public void ListSchedules(string challengeCode
            , ChallengeListSchedulesOptionalParameters optionalParams
            , ResultCallback<ChallengeListSchedulesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            api.ListSchedules(challengeCode, optionalParams, callback);
        }
    }
}