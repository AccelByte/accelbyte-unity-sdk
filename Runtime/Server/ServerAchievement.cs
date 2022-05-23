// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerAchievement : WrapperBase
    {
        private readonly ServerAchievementApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerAchievement( ServerAchievementApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull( inApi, "Cannot construct server achievement; api is null!");
            Assert.IsNotNull( inCoroutineRunner, "Cannot construct server achievement; coroutineRunner is null!");

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
        internal ServerAchievement( ServerAchievementApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Unlock specific achievement.
        /// </summary>
        /// <param name="userId">The id of the user who will receive achievement.</param>
        /// <param name="achievementCode">The achievement code which will be unlock.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void UnlockAchievement( string userId
            , string achievementCode
            , ResultCallback callback )
        {
            Assert.IsNotNull(userId, "Can't unlock achievement; UserId parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't unlock achievement; AchievementCode parameter is null!");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UnlockAchievement(userId, session.AuthorizationToken, achievementCode, callback));
        }
    }
}
