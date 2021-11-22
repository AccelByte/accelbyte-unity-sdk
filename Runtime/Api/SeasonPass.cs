// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Season Pass service.
    /// </summary>
    public class SeasonPass
    {
        private readonly SeasonPassApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal SeasonPass(SeasonPassApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null. Construction is failed.");
            Assert.IsNotNull(session, "session parameter can not be null. Construction is failed.");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "namespace paramater can not be empty. Construction is failed.");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction is failed.");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get current active season.
        /// </summary>
        /// <param name="language">The language of the Season.</param>
        /// <param name="callback">Returns a Result that contains SeasonInfo via callback when completed.</param>
        public void GetCurrentSeason(string language, ResultCallback<SeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(this.api.GetCurrentSeason(this.@namespace, this.session.AuthorizationToken, language, callback));
        }

        /// <summary>
        /// Get user season data by SeasonId.
        /// </summary>
        /// <param name="seasonId">The Id of the Season.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void GetUserSeason(string seasonId, ResultCallback<UserSeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(this.api.GetUserSeason(this.@namespace, this.session.AuthorizationToken, this.session.UserId, seasonId, callback));
        }

        /// <summary>
        /// Get current active user season data.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void GetCurrentUserSeason(ResultCallback<UserSeasonInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(this.api.GetCurrentUserSeason(this.@namespace, this.session.AuthorizationToken, this.session.UserId, callback));
        }

        /// <summary>
        /// Claim Season Rewards.
        /// </summary>
        /// <param name="rewardRequest">Detail information for the Reward Request.</param> 
        /// <param name="callback">Returns a Result that contains SeasonClaimRewardResponse via callback when completed.</param>
        public void ClaimRewards(SeasonClaimRewardRequest rewardRequest, ResultCallback<SeasonClaimRewardResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(this.api.ClaimRewards(this.@namespace, this.session.AuthorizationToken, this.session.UserId, rewardRequest, callback));
        }

        /// <summary>
        /// Bulk claim season rewards.
        /// </summary>
        /// <param name="callback">Returns a Result that contains SeasonClaimRewardResponse via callback when completed.</param>
        public void BulkClaimRewards(ResultCallback<SeasonClaimRewardResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(this.api.BulkClaimRewards(this.@namespace, this.session.AuthorizationToken, this.session.UserId, callback));
        }


    }
}
