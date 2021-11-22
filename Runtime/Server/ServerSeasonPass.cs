// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSeasonPass
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerSeasonPassApi api;
        private readonly IServerSession session;
        private readonly string namespace_;

        internal ServerSeasonPass(ServerSeasonPassApi api, IServerSession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, nameof(api) + " is null.");
            Assert.IsNotNull(coroutineRunner, nameof(coroutineRunner) + " is null.");
            Assert.IsNotNull(session, "Can not construct Season Pass Manager; session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can not construct Season Pass Manager; ns paramater couldn't be empty");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Grant exp to user by UserId. After reaching to the next level, currentExp will be reset to 0, remainder will be added.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="exp">Total of the exp will be granted to user.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GrantExpToUser(string userId, int exp, ResultCallback<UserSeasonInfoWithoutReward> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(userId, "Can't Grant Exp; UserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GrantExpToUser(this.namespace_, this.session.AuthorizationToken, userId, exp, callback));
        }

        /// <summary>
        /// Get current user season progression, season only located in non-publisher namespace.
        /// </summary>
        /// <param name="userId">The User ID to check user season progression.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GetCurrentUserSeasonProgression(string userId, ResultCallback<UserSeasonInfoWithoutReward> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(userId, "Can't check user progression; UserId parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetCurrentUserSeasonProgression(this.namespace_, this.session.AuthorizationToken, userId, callback));
        }

    }
}