// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Server.Interface
{
    public interface IServerChallenge
    {
        /// <summary>
        /// Send a request to attempt to evaluate many user's challenge progress
        /// </summary>
        /// <param name="userIds">List of the User IDs to be evaluated their challenge progress</param>
        /// <param name="callback">ResultCallback whether it success or failed</param>
        public void EvaluateChallengeProgress(string[] userIds
            , ResultCallback callback);
    }
}