// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide an API to access Challenge service.
    /// </summary>
    public class ServerChallenge : WrapperBase, IServerChallenge
    {
        private readonly ServerChallengeApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerChallenge(ServerChallengeApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(inApi, "Cannot construct Challenge manager; api is null!");

            api = inApi;
            session = inSession;
        }

        public void EvaluateChallengeProgress(string[] userIds
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (userIds == null || userIds.Length == 0)
            {
                callback.TryError(ErrorCode.InvalidRequest);
                return;
            }

            ChallengeEvaluatePlayerProgressionRequest requestBody = new ChallengeEvaluatePlayerProgressionRequest()
            {
                UserIds = userIds
            };

            api.EvaluateChallengeProgress(requestBody, callback);
        }
    }
}