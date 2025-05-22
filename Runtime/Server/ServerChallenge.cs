// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;
using System;

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkClaimRewardOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ClaimReward(challengeBulkClaimRewardRequest, optionalParameters, callback);
        }

        internal void ClaimReward(ChallengeBulkClaimRewardRequest[] challengeBulkClaimRewardRequest
            , BulkClaimRewardOptionalParameters optionalParameters
            , ResultCallback<ChallengeBulkClaimRewardResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ClaimReward(challengeBulkClaimRewardRequest, optionalParameters, callback);
        }

        public void ClaimReward(string userId, ClaimRewardRequest claimRewardRequest, ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ClaimRewardOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ClaimReward(userId, claimRewardRequest, optionalParameters, callback);
        }

        internal void ClaimReward(string userId
            , ClaimRewardRequest claimRewardRequest
            , ClaimRewardOptionalParameters optionalParameters
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ClaimReward(userId, claimRewardRequest, optionalParameters, callback);
        }

        public void CreateChallenge(CreateChallengeRequest createChallengeRequest, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateChallenge(createChallengeRequest, optionalParameters, callback);
        }

        internal void CreateChallenge(CreateChallengeRequest createChallengeRequest
            , CreateChallengeOptionalParameters optionalParameters
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateChallenge(createChallengeRequest, optionalParameters, callback);
        }

        public void CreateChallengeGoal(string challengeCode, CreateChallengeGoalRequest createChallengeGoalRequest, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateChallengeGoal(challengeCode, createChallengeGoalRequest, optionalParameters, callback);
        }

        internal void CreateChallengeGoal(string challengeCode
            , CreateChallengeGoalRequest createChallengeGoalRequest
            , CreateChallengeGoalOptionalParameters optionalParameters
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateChallengeGoal(challengeCode, createChallengeGoalRequest, optionalParameters, callback);
        }

        public void DeleteChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteChallenge(challengeCode, optionalParameters, callback);
        }

        internal void DeleteChallenge(string challengeCode
            , DeleteChallengeOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteChallenge(challengeCode, optionalParameters, callback);
        }

        public void DeleteChallengeGoal(string challengeCode, string goalCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        internal void DeleteChallengeGoal(string challengeCode
            , string goalCode
            , DeleteChallengeGoalOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        public void DeleteTiedChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteTiedChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteTiedChallenge(challengeCode, optionalParameters, callback);
        }

        internal void DeleteTiedChallenge(string challengeCode, DeleteTiedChallengeOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteTiedChallenge(challengeCode, optionalParameters, callback);
        }

        public void EvaluateChallengeProgress(string[] userIds
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new EvaluateChallengeProgressOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            EvaluateChallengeProgress(userIds, optionalParameters, callback);
        }

        internal void EvaluateChallengeProgress(string[] userIds
            , EvaluateChallengeProgressOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

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

            api.EvaluateChallengeProgress(requestBody, optionalParameters, callback);
        }

        public void GetChallenge(string challengeCode, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetChallenge(challengeCode, optionalParameters, callback);
        }

        internal void GetChallenge(string challengeCode, GetChallengeOptionalParameters optionalParameters, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenge(challengeCode, optionalParameters, callback);
        }

        public void GetChallengeGoal(string challengeCode, string goalCode, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        internal void GetChallengeGoal(string challengeCode, string goalCode, GetChallengeGoalOptionalParameters optionalParameters, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        public void GetChallengeGoals(string challengeCode, ResultCallback<GoalResponse> callback, ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeGoalsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                SortBy = challengeSortBy
            };

            GetChallengeGoals(challengeCode, optionalParameters, callback);
        }

        internal void GetChallengeGoals(string challengeCode
            , GetChallengeGoalsOptionalParameters optionalParameters
            , ResultCallback<GoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengeGoals(challengeCode, optionalParameters, callback);
        }

        public void GetChallengePeriods(string challengeCode, ResultCallback<ChallengePeriodResponse> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengePeriodsOptionalParameters()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset
            };

            GetChallengePeriods(challengeCode, optionalParameters, callback);
        }

        internal void GetChallengePeriods(string challengeCode
            , GetChallengePeriodsOptionalParameters optionalParameters
            , ResultCallback<ChallengePeriodResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallengePeriods(challengeCode, optionalParameters, callback);
        }

        [Obsolete("This interface is deprecated, and will be removed on AGS 2025.4. Please use GetChallenges(optionalParameters, callback).")]
        public void GetChallenges(ResultCallback<ChallengeResponse> callback, ChallengeStatus status = ChallengeStatus.None, ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengesOptionalParamenters()
            {
                Status = status,
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                SortBy = sortBy
            };

            GetChallenges(optionalParameters, callback);
        }

        public void GetChallenges(ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            GetChallenges(new GetChallengesOptionalParamenters()
            {
                Logger = SharedMemory?.Logger
            }, callback);
        }

        public void GetChallenges(GetChallengesOptionalParamenters optionalParameters
            , ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetChallenges(optionalParameters, callback);
        }

        public void GetUserRewards(string userId, ResultCallback<UserRewards> callback, ChallengeRewardStatus challengeRewardStatus = ChallengeRewardStatus.None, ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserRewardsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                RewardStatus = challengeRewardStatus,
                SortBy = challengeSortBy
            };

            GetUserRewards(userId, optionalParameters, callback);
        }

        internal void GetUserRewards(string userId
            , GetUserRewardsOptionalParameters optionalParameters
            , ResultCallback<UserRewards> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserRewards(userId, optionalParameters, callback);
        }

        public void RandomizeChallengeGoals(string challengeCode, ResultCallback<RandomizedChallengeResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new RandomizeChallengeGoalsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            RandomizeChallengeGoals(challengeCode, optionalParameters, callback);
        }

        internal void RandomizeChallengeGoals(string challengeCode
            , RandomizeChallengeGoalsOptionalParameters optionalParameters
            , ResultCallback<RandomizedChallengeResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RandomizeChallengeGoals(challengeCode, optionalParameters, callback);
        }

        public void UpdateChallenge(string challengeCode, UpdateChallengeRequest updateChallengeRequest, ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateChallenge(challengeCode, updateChallengeRequest, optionalParameters, callback);
        }

        internal void UpdateChallenge(string challengeCode
            , UpdateChallengeRequest updateChallengeRequest
            , UpdateChallengeOptionalParameters optionalParameters
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateChallenge(challengeCode, updateChallengeRequest, optionalParameters, callback);
        }

        public void UpdateChallengeGoal(string challengeCode, string goalCode, UpdateChallengeGoalRequest updateChallengeGoalRequest, ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateChallengeGoal(challengeCode, goalCode, updateChallengeGoalRequest, optionalParameters, callback);
        }

        internal void UpdateChallengeGoal(string challengeCode
            , string goalCode
            , UpdateChallengeGoalRequest updateChallengeGoalRequest
            , UpdateChallengeGoalOptionalParameters optionalParameters
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateChallengeGoal(challengeCode, goalCode, updateChallengeGoalRequest, optionalParameters, callback);
        }
    }
}