// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Achievement service.
    /// </summary>
    public class Achievement : WrapperBase
    {
        private readonly AchievementApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly UserSession session;
        
        internal Achievement( AchievementApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner==null (@ constructor)");

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
        internal Achievement( AchievementApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this(inApi, inSession, inCoroutineRunner) // Curry this obsolete data to the new overload ->
        {
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
        public void QueryAchievements( string language
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicAchievement> callback
            , int offset = 0
            , int limit = 20 
            , TagQueryBuilder tagBuilder = null
            )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var tags = tagBuilder is null ? string.Empty : tagBuilder.Build();

            coroutineRunner.Run(
                api.QueryAchievements(
                    language, 
                    tags,
                    sortBy, 
                    callback, 
                    offset, 
                    limit));
        }

        /// <summary>
        /// Get an specific achievement information.
        /// </summary>
        /// <param name="achievementCode">The code of the expected achievement.</param>
        /// <param name="callback">Returns a Result that contains MultiLanguageAchievement via callback when completed.</param>
        public void GetAchievement( string achievementCode
            , ResultCallback<MultiLanguageAchievement> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(achievementCode, "Can't get achievement; AchievementCode parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAchievement(achievementCode, callback));
        }

        /// <summary>
        /// Query user's achievements. Include achieved and in-progress.
        /// </summary>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedUserAchievement via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
        /// <param name="PreferUnlocked">True if the configuration to display unlocked achievements first active, the list order should display unlocked achievements first on top of locked achievements, and false otherwise. Default value is true.</param>
        public void QueryUserAchievements( AchievementSortBy sortBy
            , ResultCallback<PaginatedUserAchievement> callback
            , int offset = 0
            , int limit = 20
            , bool PreferUnlocked = true 
            , TagQueryBuilder tagBuilder = null
            )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var tags = tagBuilder is null ? string.Empty : tagBuilder.Build();

            coroutineRunner.Run(
                api.QueryUserAchievements(
                    session.UserId,
                    tags,
                    sortBy, 
                    callback, 
                    offset, 
                    limit, 
                    PreferUnlocked));
        }

        /// <summary>
        /// Unlock specific achievement.
        /// </summary>
        /// <param name="achievementCode">The achievement code which will be unlock.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void UnlockAchievement( string achievementCode
            , ResultCallback callback )
        {
            Assert.IsNotNull(achievementCode, "Can't unlock achievement; AchievementCode parameter is null!");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UnlockAchievement(session.UserId, session.AuthorizationToken, achievementCode, callback));
        }

        public void GetTags(string name
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicTag> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
            }
            
            coroutineRunner.Run(
                api.GetTags(name, sortBy, callback, offset, limit));
        }
    }
}
