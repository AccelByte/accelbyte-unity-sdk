// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
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

        [UnityEngine.Scripting.Preserve]
        internal ServerAchievement(ServerAchievementApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct server achievement; api is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct server achievement; coroutineRunner is null!");

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
        internal ServerAchievement(ServerAchievementApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this(inApi, inSession, inCoroutineRunner)
        {
        }

        /// <summary>
        /// Unlock specific achievement.
        /// </summary>
        /// <param name="userId">The id of the user who will receive achievement.</param>
        /// <param name="achievementCode">The achievement code which will be unlock.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void UnlockAchievement(string userId
            , string achievementCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (string.IsNullOrEmpty(achievementCode))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Can't unlock achievement; AchievementCode parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

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
