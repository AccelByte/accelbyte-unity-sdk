// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class UserProfiles : WrapperBase
    {
        private readonly UserProfilesApi api;
        private readonly IUserSession session;
        private readonly CoroutineRunner coroutineRunner;

        internal UserProfiles( UserProfilesApi inApi
            , IUserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal UserProfiles( UserProfilesApi inApi
            , IUserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get user profile for current logged in user.
        /// </summary>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed.</param>
        public void GetUserProfile( ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserProfile(callback));
        }

        /// <summary>
        /// Create user profile for current logged in user.
        /// </summary>
        /// <param name="createRequest">User profile details to create user profile.</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void CreateUserProfile( CreateUserProfileRequest createRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateUserProfile(createRequest, callback));
        }

        /// <summary>
        /// Update some fields of user profile for current logged in user. 
        /// </summary>
        /// <param name="updateRequest">User profile details to update user profile</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void UpdateUserProfile( UpdateUserProfileRequest updateRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateUserProfile(updateRequest, callback));
        }

        public void GetCustomAttributes( ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetCustomAttributes(session.UserId, callback));
        }

        public void UpdateCustomAttributes( Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            coroutineRunner.Run(
                api.UpdateCustomAttributes(session.UserId, updates, callback));
        }

        /// <summary>
        /// Get the <see cref="PublicUserProfile"/> for a specified user.
        /// </summary>
        /// <param name="userId">UserID for the profile requested</param>
        /// <param name="callback">Returns a Result that contains <see cref="PublicUserProfile"/> via callback when completed.</param>
        public void GetPublicUserProfile( string userId
            , ResultCallback<PublicUserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserProfilePublicInfo(userId, callback));
        }
        /// <summary>
        /// Request the Avatar of the given UserProfile
        /// </summary>
        /// <param name="userID">The UserID of a public Profile</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        public void GetUserAvatar( string userID
            , ResultCallback<Texture2D> callback )
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
                        coroutineRunner.Run(ABUtilities.DownloadTexture2D(result.Value.avatarUrl, callback));
                    }
                });
        }
    }
}
