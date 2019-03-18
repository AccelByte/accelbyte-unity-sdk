// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public class UserProfiles
    {
        private readonly string @namespace;
        private readonly UserProfilesApi api;
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal UserProfiles(string @namespace, UserProfilesApi api, User user, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            this.api = api;
            this.@namespace = @namespace;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get user profile for current logged in user.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed.</param>
        public void GetUserProfile(ResultCallback<UserProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetUserProfile(this.@namespace, this.user.AccessToken, result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserProfile>) result)),
                    null));
        }

        /// <summary>
        /// Create user profile for current logged in user.
        /// </summary>
        /// <param name="createRequest">User profile details to create user profile.</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void CreateUserProfile(CreateUserProfileRequest createRequest, ResultCallback<UserProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.CreateUserProfile(
                        this.@namespace,
                        this.user.AccessToken,
                        createRequest,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserProfile>) result)),
                    null));
        }

        /// <summary>
        /// Update some fields of user profile for current logged in user. 
        /// </summary>
        /// <param name="updateRequest">User profile details to update user profile</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void UpdateUserProfile(UpdateUserProfileRequest updateRequest, ResultCallback<UserProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateUserProfile(
                        this.@namespace,
                        this.user.AccessToken,
                        updateRequest,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserProfile>) result)),
                    null));
        }
    }
}