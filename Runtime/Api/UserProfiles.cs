// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class UserProfiles
    {
        private readonly UserProfilesApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal UserProfiles(UserProfilesApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(session, "session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get user profile for current logged in user.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed.</param>
        public void GetUserProfile(ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserProfile(this.@namespace, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Create user profile for current logged in user.
        /// </summary>
        /// <param name="createRequest">User profile details to create user profile.</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void CreateUserProfile(CreateUserProfileRequest createRequest, ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateUserProfile(this.@namespace, this.session.AuthorizationToken, createRequest, callback));
        }

        /// <summary>
        /// Update some fields of user profile for current logged in user. 
        /// </summary>
        /// <param name="updateRequest">User profile details to update user profile</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void UpdateUserProfile(UpdateUserProfileRequest updateRequest, ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateUserProfile(this.@namespace, this.session.AuthorizationToken, updateRequest, callback));
        }

        public void GetCustomAttributes(ResultCallback<Dictionary<string, object>> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetCustomAttributes(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    callback));
        }

        public void UpdateCustomAttributes(Dictionary<string, object> updates,
            ResultCallback<Dictionary<string, object>> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            
            this.coroutineRunner.Run(
                this.api.UpdateCustomAttributes(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    updates,
                    callback));
        }

        /// <summary>
        /// Get the <see cref="PublicUserProfile"/> for a specified user.
        /// </summary>
        /// <param name="userId">UserID for the profile requested</param>
        /// <param name="callback">Returns a Result that contains <see cref="PublicUserProfile"/> via callback when completed.</param>
        public void GetPublicUserProfile(string userId, ResultCallback<PublicUserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserProfilePublicInfo(this.@namespace, userId, this.session.AuthorizationToken, callback));
        }
        /// <summary>
        /// Request the Avatar of the given UserProfile
        /// </summary>
        /// <param name="userID">The UserID of a public Profile</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        public void GetUserAvatar(string userID, ResultCallback<Texture2D> callback)
        {
            GetPublicUserProfile(userID,
                result =>
                {
                    if (result.IsError)
                    {
                        Debug.LogError(
                            $"Unable to get Public User Profile Code:{result.Error.Code} Message:{result.Error.Message}");
                        callback.TryError(result.Error);
                    }
                    else
                    {
                        this.coroutineRunner.Run(ABUtilities.DownloadTexture2D(result.Value.avatarUrl, callback));
                    }
                });
        }
    }
}
