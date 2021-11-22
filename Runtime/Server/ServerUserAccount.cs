// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUserAccount
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerUserAccountApi api;

        internal ServerUserAccount(ServerUserAccountApi api, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, nameof(api) + " is null.");
            Assert.IsNotNull(coroutineRunner, nameof(coroutineRunner) + " is null.");
            this.api = api;
            this.coroutineRunner = coroutineRunner;
        }

        public void GetUserData(string userAuthToken, ResultCallback<UserData> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(userAuthToken), "parameter " + nameof(userAuthToken) + " is null");
            this.coroutineRunner.Run(api.GetUserData(userAuthToken, callback));
        }
    }
}