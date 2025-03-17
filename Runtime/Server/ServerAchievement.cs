// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerAchievement : WrapperBase
    {
        internal readonly ServerAchievementApi Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerAchievement(ServerAchievementApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct server achievement; api is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct server achievement; coroutineRunner is null!");

            Api = inApi;
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (string.IsNullOrEmpty(achievementCode))
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "Can't unlock achievement; AchievementCode parameter is null!"));
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.UnlockAchievement(userId, session.AuthorizationToken, achievementCode, callback));
        }

        /// <summary>
        /// Unlock multiple achievements via achievementCode for the current user.
        /// </summary>
        /// <param name="userId">User id of user to unlock achievements for.</param>
        /// <param name="achievementCodes">Array of achievement codes to be unlocked.</param>
        /// <param name="callback">Returns a Result via callback that contains an array of BulkUnlockAchievementResponse when completed.</param>
        public void BulkUnlockAchievement(string userId
            , string[] achievementCodes
            , ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            Api.BulkUnlockAchievement(userId, achievementCodes, callback);
        }
    }
}
