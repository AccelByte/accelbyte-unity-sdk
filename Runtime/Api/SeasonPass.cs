// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
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

            coroutineRunner.Run(api.GetCurrentSeason(language, callback));
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetUserSeason(session.UserId, seasonId, callback));
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

            coroutineRunner.Run(api.GetCurrentUserSeason(session.UserId, callback));
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

            coroutineRunner.Run(api.ClaimRewards(session.UserId, rewardRequest, callback));
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

            coroutineRunner.Run(api.BulkClaimRewards(session.UserId, callback));
        }
    }
}
