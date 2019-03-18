// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide information of entitlements owned by the user
    /// </summary>
    public class Entitlements
    {
        private readonly string @namespace;
        private readonly EntitlementApi api;
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal Entitlements(string @namespace, EntitlementApi api, User user, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can't construct Entitlement! api is null!");
            Assert.IsNotNull(@namespace, "Can't construct Entitlement! namespace parameter is null!");
            Assert.IsNotNull(user, "Can't construct Entitlement! user parameter is null!");
            Assert.IsNotNull(taskDispatcher, "taskDispatcher must not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner must not be null");

            this.api = api;
            this.@namespace = @namespace;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get list of entitlements owned by a user
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void GetUserEntitlements(int page, int size, ResultCallback<PagedEntitlements> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetUserEntitlements(
                        this.@namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        page,
                        size,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<PagedEntitlements>) result)),
                    this.user));
        }
    }
}