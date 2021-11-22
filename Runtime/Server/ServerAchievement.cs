// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerAchievement
    {
        private readonly ServerAchievementApi api;
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerAchievement(ServerAchievementApi api, IServerSession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct server achievement; api is null!");
            Assert.IsNotNull(session, "Can not construct server achievement; session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can not construct server achievement; ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "Can not construct server achievement; coroutineRunner is null!");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Unlock specific achievement.
        /// </summary>
        /// <param name="achievementCode">The id of the user who will receive achievement.</param>
        /// <param name="achievementCode">The achievement code which will be unlock.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void UnlockAchievement(string userId, string achievementCode, ResultCallback callback)
        {
            Assert.IsNotNull(userId, "Can't unlock achievement; UserId parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't unlock achievement; AchievementCode parameter is null!");

            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UnlockAchievement(this.namespace_, userId, this.session.AuthorizationToken, achievementCode, callback));
        }
    }
}
