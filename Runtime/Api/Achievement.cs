﻿// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
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

        [UnityEngine.Scripting.Preserve]
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
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
        /// <param name="tagBuilder">A query expression consists of tags to query the achievement from.</param>
        /// <param name="isGlobal">True if the configuration to display global achievements.</param>/// 
        public void QueryAchievements( string language
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicAchievement> callback
            , int offset = 0
            , int limit = 20
            , TagQueryBuilder tagBuilder = null
            , bool isGlobal = false
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
                    limit,
                    isGlobal));
        }

        /// <summary>
        /// Get an specific achievement information.
        /// </summary>
        /// <param name="achievementCode">The code of the expected achievement.</param>
        /// <param name="callback">Returns a Result that contains MultiLanguageAchievement via callback when completed.</param>
        public void GetAchievement(string achievementCode
            , ResultCallback<MultiLanguageAchievement> callback)
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
        /// <param name="preferUnlocked">True if the configuration to display unlocked achievements first active, the list order should display unlocked achievements first on top of locked achievements, and false otherwise. Default value is true.</param>
        /// <param name="tagBuilder">A query expression consists of tags to query the achievement from.</param>
        public void QueryUserAchievements( AchievementSortBy sortBy
            , ResultCallback<PaginatedUserAchievement> callback
            , int offset = 0
            , int limit = 20
            , bool preferUnlocked = true 
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
                   preferUnlocked));
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

        /// <summary>
        /// Query the progress list of global achievements. Include achieved and in-progress.
        /// </summary>
        /// <param name="achievementCode">The code of the expected global achievement.</param>
        /// <param name="achievementStatus">Achievement status for the achievements result.</param>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedUserGlobalAchievement via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
        /// <param name="tagBuilder">A query expression consists of tags to query the achievement from.</param>
        public void QueryGlobalAchievements(string achievementCode
            , GlobalAchievementStatus achievementStatus
            , GlobalAchievementListSortBy sortBy
            , ResultCallback<PaginatedUserGlobalAchievement> callback
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
                api.QueryGlobalAchievements(
                    achievementCode,
                    achievementStatus,
                    sortBy,
                    callback,
                    offset,
                    limit,
                    tags));
        }

        /// <summary>
        /// Query the list of contributors for a global achievement.
        /// </summary>
        /// <param name="achievementCode">The code of the expected global achievement.</param>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedGlobalAchievementContributors via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
        public void QueryGlobalAchievementContributors(string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementContributors> callback
            , int offset = 0
            , int limit = 20
            )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryGlobalAchievementContributors(
                    achievementCode,
                    sortBy,
                    callback,
                    offset,
                    limit));
        }

        /// <summary>
        /// Query the list of global achievements that have been contributed by the user.
        /// </summary>
        /// <param name="achievementCode">The code of the expected achievement.</param>
        /// <param name="sortBy">Sorting method for the achievements result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedGlobalAchievementUserContributed via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
        public void QueryGlobalAchievementUserContributed(string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementUserContributed> callback
            , int offset = 0
            , int limit = 20
            )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.QueryGlobalAchievementUserContributed( 
                    session.UserId,
                    achievementCode,
                    sortBy,
                    callback,
                    offset,
                    limit));
        }

        /// <summary>
        /// Claim specific global achievement.
        /// </summary>
        /// <param name="achievementCode">The code of the expected global achievement.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void ClaimGlobalAchievement(string achievementCode
            , ResultCallback callback
            )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ClaimGlobalAchievement(
                    session.UserId, 
                    session.AuthorizationToken, 
                    achievementCode, 
                    callback));
        }

        /// <summary>
        /// Query all tags for achievements.
        /// </summary>
        /// <param name="name">The name of the expected tag</param>
        /// <param name="sortBy">Sorting method for the achievement tags result.</param>
        /// <param name="callback">Returns a Result that contains PaginatedPublicTag via callback when completed.</param>
        /// <param name="offset">The offset of the achievement result. Default value is 0.</param>
        /// <param name="limit">The limit of the achievement result. Default value is 20.</param>
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
                return;
            }
            
            coroutineRunner.Run(
                api.GetTags(name, sortBy, callback, offset, limit));
        }
    }
}
