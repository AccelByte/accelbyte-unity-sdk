// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSeasonPass : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerSeasonPassApi api;
        private readonly ISession session;

        internal ServerSeasonPass( ServerSeasonPassApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull( inApi, nameof( inApi ) + " is null.");
            Assert.IsNotNull( inCoroutineRunner, nameof( inCoroutineRunner ) + " is null.");

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
        internal ServerSeasonPass( ServerSeasonPassApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Grant exp to user by UserId. After reaching to the next level, currentExp will be reset to 0, remainder will be added.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="exp">Total of the exp will be granted to user.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GrantExpToUser( string userId
            , int exp
            , ResultCallback<UserSeasonInfoWithoutReward> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userId, "Can't Grant Exp; UserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GrantExpToUser(userId, exp, callback));
        }

        /// <summary>
        /// Get current user season progression, season only located in non-publisher namespace.
        /// </summary>
        /// <param name="userId">The User ID to check user season progression.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GetCurrentUserSeasonProgression( string userId
            , ResultCallback<UserSeasonInfoWithoutReward> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userId, "Can't check user progression; UserId parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetCurrentUserSeasonProgression(userId, callback));
        }

    }
}