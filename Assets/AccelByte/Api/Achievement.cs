// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Achievement service.
    /// </summary>
    public class Achievement
    {
        private readonly AchievementApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly string @namespace;

        internal Achievement(AchievementApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Query all achievements in the related namespace.
        /// </summary>
        /// <param name="language">The language to display the appropiate achievement's name and description. If it is empty, it will use the its default language.
        /// If the achievement does not have the expected language, it will use its dafault languge.</param>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedPublicAchievement via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>/// 
        public void QueryAchievements(string language, AchievementSortBy sortBy, ResultCallback<PaginatedPublicAchievement> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.QueryAchievements(this.@namespace, this.session.AuthorizationToken, language, sortBy, callback, offset, limit));
        }

        /// <summary>
        /// Get an specific achievement information.
        /// </summary>
        /// <param name="achievementCode">The code of the expected achievement.</param>
        /// <param name="callback">Returns a Result that contains MultiLanguageAchievement via callback when completed.</param>
        public void GetAchievement(string achievementCode, ResultCallback<MultiLanguageAchievement> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(achievementCode, "Can't get achievement; AchievementCode parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetAchievement(this.@namespace, this.session.AuthorizationToken, achievementCode, callback));
        }

        /// <summary>
        /// Query user's achievements. Include achieved and in-progress.
        /// </summary>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedUserAchievement via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
        /// <param name="PreferUnlocked">True if the configuration to display unlocked achievements first active, the list order should display unlocked achievements first on top of locked achievements, and false otherwise. Default value is true.</param>
        public void QueryUserAchievements(AchievementSortBy sortBy, ResultCallback<PaginatedUserAchievement> callback, int offset = 0, int limit = 20, bool PreferUnlocked = true)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.QueryUserAchievements(this.@namespace, this.session.UserId, this.session.AuthorizationToken, sortBy, callback, offset, limit, PreferUnlocked));
        }

        /// <summary>
        /// Unlock specific achievement.
        /// </summary>
        /// <param name="achievementCode">The achievement code which will be unlock.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void UnlockAchievement(string achievementCode, ResultCallback callback)
        {
            Assert.IsNotNull(achievementCode, "Can't unlock achievement; AchievementCode parameter is null!");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UnlockAchievement(this.@namespace, this.session.UserId, this.session.AuthorizationToken, achievementCode, callback));
        }
    }
}
