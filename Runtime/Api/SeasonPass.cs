// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Season Pass service.
    /// </summary>
    public class SeasonPass : WrapperBase
    {
        private readonly SeasonPassApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal SeasonPass( SeasonPassApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null. Construction is failed.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null. Construction is failed.");

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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal SeasonPass( SeasonPassApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get current active season.
        /// </summary>
        /// <param name="language">The language of the Season.</param>
        /// <param name="callback">Returns a Result that contains SeasonInfo via callback when completed.</param>
        public void GetCurrentSeason(string language, ResultCallback<SeasonInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetCurrentSeason(language, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, PredefinedAnalyticsMode.GetCurrentSeason);
                }
                HandleCallback(cb, callback);
            })); 
        }

        /// <summary>
        /// Get user season data by SeasonId.
        /// </summary>
        /// <param name="seasonId">The Id of the Season.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void GetUserSeason( string seasonId
            , ResultCallback<UserSeasonInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(seasonId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSeasonIdInvalidMessage(seasonId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetUserSeason(session.UserId, seasonId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, PredefinedAnalyticsMode.GetUserSeason);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Get current active user season data.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void GetCurrentUserSeason( ResultCallback<UserSeasonInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetCurrentUserSeason(session.UserId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, PredefinedAnalyticsMode.GetCurrentUserSeason);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Claim Season Rewards.
        /// </summary>
        /// <param name="rewardRequest">Detail information for the Reward Request.</param> 
        /// <param name="callback">Returns a Result that contains SeasonClaimRewardResponse via callback when completed.</param>
        public void ClaimRewards( SeasonClaimRewardRequest rewardRequest
            , ResultCallback<SeasonClaimRewardResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.ClaimRewards(session.UserId, rewardRequest, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(rewardRequest, PredefinedAnalyticsMode.ClaimReward);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Bulk claim season rewards.
        /// </summary>
        /// <param name="callback">Returns a Result that contains SeasonClaimRewardResponse via callback when completed.</param>
        public void BulkClaimRewards( ResultCallback<SeasonClaimRewardResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.BulkClaimRewards(session.UserId, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb.Value, PredefinedAnalyticsMode.BulkClaimReward);
                }
                HandleCallback(cb, callback);
            }));
        }

        private enum PredefinedAnalyticsMode
        {
            ClaimReward,
            BulkClaimReward,
            GetCurrentSeason,
            GetUserSeason,
            GetCurrentUserSeason
        }

        private IAccelByteTelemetryPayload CreatePredefinedPayload<T>(T result, PredefinedAnalyticsMode mode)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (mode)
            {
                case PredefinedAnalyticsMode.ClaimReward:
                    var claimRewardResult = result as SeasonClaimRewardRequest;
                    payload = new PredefinedSeasonPassClaimRewardPayload(localUserId, claimRewardResult.passCode, 
                        claimRewardResult.tierIndex, claimRewardResult.rewardCode);
                    break;

                case PredefinedAnalyticsMode.BulkClaimReward:
                    payload = new PredefinedSeasonPassBulkClaimRewardPayload(localUserId);
                    break;

                case PredefinedAnalyticsMode.GetCurrentSeason:
                    var getCurrentSeasonResult = result as SeasonInfo;
                    payload = new PredefinedSeasonPassGetCurrentSeasonPayload(localUserId, getCurrentSeasonResult.language);
                    break;

                case PredefinedAnalyticsMode.GetUserSeason:
                    var getUserSeasonResult = result as UserSeasonInfo;
                    payload = new PredefinedSeasonPassGetUserSeasonPayload(localUserId, 
                        getUserSeasonResult.seasonId);
                    break;

                case PredefinedAnalyticsMode.GetCurrentUserSeason:
                    payload = new PredefinedSeasonPassGetCurrentUserSeasonPayload(localUserId);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent<T>(T result, PredefinedAnalyticsMode mode)
        {
            IAccelByteTelemetryPayload payload = CreatePredefinedPayload(result, mode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            var seasonPassEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(seasonPassEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }
    }
}
