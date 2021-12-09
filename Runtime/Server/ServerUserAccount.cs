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
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly ServerUserAccountApi api;

        internal ServerUserAccount(ServerUserAccountApi api, IServerSession session, string namespace_, 
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, nameof(api) + " is null.");
            Assert.IsNotNull(session, "Can not construct User Account manager; session parameter can not be null");
            Assert.IsFalse(
                string.IsNullOrEmpty(namespace_),
                "Can not construct User Account manager; ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, nameof(coroutineRunner) + " is null.");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get User Data by Authorization Token
        /// </summary>
        /// <param name="userAuthToken">User's authorization token</param>
        /// <param name="callback">Returns a UserData via callback when completed.</param>
        public void GetUserData(string userAuthToken, ResultCallback<UserData> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(userAuthToken), "parameter " + nameof(userAuthToken) + " is null");
            this.coroutineRunner.Run(api.GetUserData(userAuthToken, callback));
        }

        /// <summary>
        /// This function will search user by their third party Display Name. 
        /// The query will be used to find the user with the most approximate display name.
        /// </summary>
        /// <param name="platformDisplayName">Targeted user's third party by Display Name.</param>
        /// <param name="platformType">The platform type want to use to search user</param>
        /// <param name="callback">Returns a PagedUserOtherPlatformInfo via callback when completed.</param>
        /// <param name="limit">The limit of the users data result. Default value is 20.</param>
        /// <param name="offset">The offset of the users data result. Default value is 0.</param>
        public void SearchUserOtherPlatformDisplayName(string platformDisplayName, PlatformType platformType, 
            ResultCallback<PagedUserOtherPlatformInfo> callback, int limit = 20, int offset = 0)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchUserOtherPlatformDisplayName(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    platformDisplayName,
                    platformType,
                    callback,
                    limit,
                    offset));
        }

        /// <summary>
        /// This function will search user by their third party Platform User ID.
        /// </summary>
        /// <param name="platformUserId">Targeted user's third party user id.</param>
        /// <param name="platformType">The platform type want to use to search user</param>
        /// <param name="callback">Returns a UserOtherPlatformInfo via callback when completed.</param>
        public void SearchUserOtherPlatformUserId(string platformUserId, PlatformType platformType, ResultCallback<UserOtherPlatformInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SearchUserOtherPlatformUserId(
                    this.namespace_,
                    this.session.AuthorizationToken,
                    platformUserId,
                    platformType,
                    callback));
        }
    }
}