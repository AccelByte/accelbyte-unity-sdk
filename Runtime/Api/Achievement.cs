// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendUserIdPredefinedEvent(session.UserId, EventMode.GetAll);
                        }
                        HandleCallback(cb, callback);
                    }, 
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

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAchievement(achievementCode, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendAchievementCodePredefinedEvent(achievementCode, EventMode.GetSpecific);
                    }
                    HandleCallback(cb, callback);
                }));
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendUserIdPredefinedEvent(session.UserId, EventMode.GetUserAchievement);
                        }
                        HandleCallback(cb, callback);
                    }, 
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UnlockAchievement(session.UserId, session.AuthorizationToken, achievementCode, cb =>
                {
                    if (cb != null && !cb.IsError)
                    {
                        SendAchievementCodePredefinedEvent(achievementCode, EventMode.Unlocked);
                    }
                    HandleCallback(cb, callback);
                }));
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendAchievementCodePredefinedEvent(achievementCode, EventMode.GlobalGet);
                        }
                        HandleCallback(cb, callback);
                    },
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendAchievementCodePredefinedEvent(achievementCode, EventMode.GlobalGetContributors);
                        }
                        HandleCallback(cb, callback);
                    },
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
                    cb =>
                    {
                        if (!cb.IsError && cb.Value != null)
                        {
                            SendAchievementCodePredefinedEvent(achievementCode, EventMode.GlobalGetContributed);
                        }
                        HandleCallback(cb, callback);
                    },
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
                    cb =>
                    {
                        if (cb != null && !cb.IsError)
                        {
                            SendAchievementCodePredefinedEvent(achievementCode, EventMode.GlobalClaimed);
                        }
                        HandleCallback(cb, callback);
                    }));
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
                api.GetTags(name, sortBy, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendNamePredefinedEvent(name);
                    }
                    HandleCallback(cb, callback);
                }, offset, limit));
        }

        #region PredefinedEvents
        private enum EventMode
        {
            Unlocked,
            GetAll,
            GetSpecific,
            GetUserAchievement,
            GlobalGet,
            GlobalGetContributors,
            GlobalGetContributed,
            GlobalClaimed,
            GetTags
        }

        private IAccelByteTelemetryPayload CreateAchievementCodePayload(string achievementCode, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (mode)
            {
                case EventMode.Unlocked:
                    payload = new PredefinedAchievementUnlockedPayload(achievementCode);
                    break;

                case EventMode.GetSpecific:
                    payload = new PredefinedAchievementGetSpecificPayload(achievementCode);
                    break;

                case EventMode.GlobalGet:
                    payload = new PredefinedGlobalAchievementGetPayload(achievementCode);
                    break;

                case EventMode.GlobalGetContributors:
                    payload = new PredefinedGlobalAchievementGetContributorsPayload(achievementCode);
                    break;

                case EventMode.GlobalGetContributed:
                    payload = new PredefinedGlobalAchievementGetContributedPayload(achievementCode);
                    break;

                case EventMode.GlobalClaimed:
                    payload = new PredefinedGlobalAchievementClaimedPayload(achievementCode);
                    break;
            }

            return payload;
        }

        private IAccelByteTelemetryPayload CreateUserIdPayload(string userId, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (mode)
            {
                case EventMode.GetUserAchievement:
                    payload = new PredefinedAchievementGetUserAchievementsPayload(userId);
                    break;

                case EventMode.GetAll:
                    payload = new PredefinedAchievementGetAllPayload(userId);
                    break;
            }

            return payload;
        }

        private void SendAchievementCodePredefinedEvent(string achievementCode, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = CreateAchievementCodePayload(achievementCode, mode);
            SendPredefinedEvent(payload);
        }

        private void SendUserIdPredefinedEvent(string userId, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = CreateUserIdPayload(userId, mode);
            SendPredefinedEvent(payload);
        }

        private void SendNamePredefinedEvent(string name)
        {
            IAccelByteTelemetryPayload payload = new PredefinedAchievementGetTagsPayload(name);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            if (payload == null)
            {
                return;
            }

            var predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
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

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        #endregion

    }
}
