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
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal Entitlements(EntitlementApi api, ISession session, string ns, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(ns), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = ns;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get list of entitlements owned by a user
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void GetUserEntitlements(int offset, int limit, ResultCallback<EntitlementPagingSlicedResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserEntitlements(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    offset,
                    limit,
                    callback));
        }
    }
}
