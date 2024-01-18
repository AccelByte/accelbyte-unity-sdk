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
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal UserProfiles( UserProfilesApi inApi
            , UserSession inSession
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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal UserProfiles( UserProfilesApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get (my) user profile / current logged in user.
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
        /// Get an user profile  
        /// </summary>
        /// <param name="userId">User Id value to create user profile.</param> 
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void GetUserProfile(string userId
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserProfile(userId, callback));
        }

        /// <summary>
        /// Create (my) user profile / current logged in user.
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

            coroutineRunner.Run(api.CreateUserProfile(createRequest, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb, PredefinedAnalyticsMode.Create);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Create an user profile 
        /// </summary>
        /// <param name="userId">User Id value to create user profile.</param>
        /// <param name="language">Language allowed format: en, en-US.</param>
        /// <param name="customAttributes">Custom attribues value to be included.</param>
        /// <param name="timezone">Timezone follows : IANA time zone, e.g. Asia/Shanghai.</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void CreateUserProfile(string userId
            , string language
            , Dictionary<string, object> customAttributes
            , string timezone
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var newUserProfile = new CreateUserProfileRequest()
            {
                language = language,
                customAttributes = customAttributes,
                timeZone = timezone
            };

            coroutineRunner.Run(api.CreateUserProfile(userId, newUserProfile, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb, PredefinedAnalyticsMode.Create);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Update some fields of (my) user profile / current logged in user. 
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
                api.UpdateUserProfile(updateRequest, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb, PredefinedAnalyticsMode.Update);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Update some fields of an user profile 
        /// </summary>
        /// <param name="userId">User Id value to create user profile.</param>
        /// <param name="language">Language allowed format: en, en-US.</param>
        /// <param name="customAttributes">Custom attribues value to be included.</param>
        /// <param name="timezone">Timezone follows : IANA time zone, e.g. Asia/Shanghai.</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed</param>
        public void UpdateUserProfile(string userId
            , string language
            , string timezone
            , Dictionary<string, object> customAttributes
            , string zipCode
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var newRequest = new UpdateUserProfileRequest()
            {
                language = language,
                timeZone = timezone,
                customAttributes = customAttributes,
                zipCode = zipCode
            };

            coroutineRunner.Run(api.UpdateUserProfile(userId, newRequest, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    SendPredefinedEvent(cb, PredefinedAnalyticsMode.Update);
                }
                HandleCallback(cb, callback);
            }));
        }

        /// <summary>
        /// Get user's own custom attribute profile information. If it doesn't exist, that will be an error
        /// </summary>
        /// <param name="callback">Returns a Result Json Object via callback when completed.</param>
        [Obsolete("This endpoint is not able to use since give a security issue for other player/user, use GetUserProfilePublicInfo instead")]
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

        /// <summary>
        /// Update user's own custom attribute profile information. If it doesn't exist, that will be an error
        /// </summary>
        /// <param name="updates">ProfileUpdateRequest Request object.</param>
        /// <param name="callback">Returns a Result Json Object via callback when completed.</param>
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

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

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
                        AccelByteDebug.LogError(
                            $"Unable to get Public User Profile Code:{result.Error.Code} Message:{result.Error.Message}");
                        callback.TryError(result.Error);
                    }
                    else
                    {
                        coroutineRunner.Run(ABUtilities.DownloadTexture2D(result.Value.avatarUrl, callback));
                    }
                });
        }

        /// <summary>
        /// Get user public profile info by public id/code.
        /// </summary>
        /// <param name="publicId">The publicId/public code of a user</param>
        /// <param name="callback">Returns a Result that contains UserProfile via callback when completed.</param>
        public void GetUserProfilePublicInfoByPublicId(string publicId, 
            ResultCallback<PublicUserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserProfilePublicInfoByPublicId(publicId, callback));
        }

        /// <summary>
        /// Generate an upload URL. It's valid for 10 minutes.
        /// </summary>
        /// <param name="folder">The name of folder where the file will be uploaded, must be between 1-256 characters, all characters allowed no whitespace</param>
        /// <param name="fileType">One of the these types: jpeg, jpg, png, bmp, gif, mp3, bin, webp</param>
        /// <param name="callback">Returns a Result that contains <see cref="PublicUserProfile"/> via callback when completed.</param>
        public void GenerateUploadURL(string folder
            , FileType fileType
            , ResultCallback<GenerateUploadURLResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GenerateUploadURL(folder, fileType, callback));
        }

        /// <summary>
        /// Generate an upload URL for user content. It's valid for 10 minutes.
        /// </summary>
        /// <param name="userId">UserID for the requested</param>
        /// <param name="fileType">One of the these types: jpeg, jpg, png, bmp, gif, mp3, bin, webp</param>
        /// <param name="callback">Returns a Result that contains <see cref="PublicUserProfile"/> via callback when completed.</param>
        /// <param name="category">Supported categories: default, reporting. Default value : default</param>
        public void GenerateUploadURLForUserContent(string userId
            , FileType fileType
            , ResultCallback<GenerateUploadURLResult> callback 
            , UploadCategory category = UploadCategory.DEFAULT)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GenerateUploadURLForUserContent(userId, fileType, callback, category));
        }
        
        /// <summary>
        /// Get user's own custom private attribute profile information. If it doesn't exist, that will be an error
        /// </summary>
        /// <param name="callback">Returns a Result Json Object via callback when completed.</param>
        public void GetPrivateCustomAttributes( ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetPrivateCustomAttributes(callback));
        }

        /// <summary>
        /// Update user's own private custom attribute profile information. If it doesn't exist, that will be an error
        /// </summary>
        /// <param name="updates">additional properties or Custom Attributes</param>
        /// <param name="callback">Returns a Result Json Object via callback when completed.</param>
        public void UpdatePrivateCustomAttributes( Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            coroutineRunner.Run(
                api.UpdatePrivateCustomAttributes(updates, callback));
        }

        #region PredefinedEvents

        private enum PredefinedAnalyticsMode
        {
            Create,
            Update
        }

        private IAccelByteTelemetryPayload CreatePayload(UserProfile result, PredefinedAnalyticsMode mode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (mode)
            {
                case PredefinedAnalyticsMode.Create:
                    payload = new PredefinedUserProfileCreatedPayload(result);
                    break;
                case PredefinedAnalyticsMode.Update:
                    payload = new PredefinedUserProfileUpdatedPayload(result);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent(Result<UserProfile> result, PredefinedAnalyticsMode mode)
        {
            IAccelByteTelemetryPayload payload = CreatePayload(result.Value, mode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            var predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        #endregion
    }
}
