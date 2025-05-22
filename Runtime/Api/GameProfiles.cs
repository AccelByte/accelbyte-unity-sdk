// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    [Obsolete("This interface is deprecated and will be removed on 2025.5.AGS. Please use Api.CloudSave instead")]
    public class GameProfiles : WrapperBase
    {
        private readonly GameProfilesApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal GameProfiles( GameProfilesApi inApi
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
        internal GameProfiles( GameProfilesApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get all profiles of specified users
        /// </summary>
        /// <param name="userIds">Id of some users to get</param>
        /// <param name="callback">Returns all profiles for specified users via callback when completed.</param>
        public void BatchGetGameProfiles( ICollection<string> userIds
            , ResultCallback<UserGameProfiles[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BatchGetGameProfiles(userIds, callback));
        }

        /// <summary>
        /// Get all profiles of current user
        /// </summary>
        /// <param name="callback">Returns all profiles of current user via callback when completed.</param>
        public void GetAllGameProfiles( ResultCallback<GameProfile[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAllGameProfiles(session.UserId, callback));
        }

        /// <summary>
        /// Create  new profile for current user
        /// </summary>
        /// <param name="gameProfile">The game profile that about to create</param>
        /// <param name="callback">Returns the created game profile via callback when completed.</param>
        public void CreateGameProfile( GameProfileRequest gameProfile
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateGameProfile(session.UserId, gameProfile, callback));
        }

        /// <summary>
        /// Get a profile of current user by the profile id
        /// </summary>
        /// <param name="profileId"> the id of profile that about to get</param>
        /// <param name="callback">Returns a profile of current user via callback when completed. </param>
        public void GetGameProfile( string profileId
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(profileId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetProfileIdInvalidMessage(profileId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameProfile(session.UserId, profileId, callback));
        }

        /// <summary>
        /// Update current user's game profile
        /// </summary>
        /// <param name="gameProfile">The game profile that about to update</param>
        /// <param name="callback">Returns updated game profile via callback when completed.</param>
        public void UpdateGameProfile( GameProfile gameProfile
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(gameProfile, gameProfile?.profileId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGameProfile(
                    session.UserId,
                    gameProfile.profileId,
                    gameProfile,
                    callback));
        }

        /// <summary>
        /// Update current user's game profile identified by the game profile id
        /// </summary>
        /// <param name="profileId">The id of game profile that about to update</param>
        /// <param name="gameProfile">The game profile that about to update</param>
        /// <param name="callback">Returns updated game profile via callback when completed.</param>
        public void UpdateGameProfile( string profileId
            , GameProfileRequest gameProfile
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(profileId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetProfileIdInvalidMessage(profileId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGameProfile(
                    session.UserId,
                    profileId,
                    gameProfile,
                    callback));
        }

        /// <summary>
        /// Delete current user's game profile identified by the game profile id
        /// </summary>
        /// <param name="profileId">The id of game profile that about to delete</param>
        /// <param name="callback">Returns boolean status via callback when completed.</param>
        public void DeleteGameProfile( string profileId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(profileId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetProfileIdInvalidMessage(profileId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGameProfile(
                    session.UserId,
                    profileId,
                    callback));
        }

        /// <summary>
        /// Get attribute of specified current user's game profile identified by the attribute name
        /// </summary>
        /// <param name="profileId">The game profile that about to get</param>
        /// <param name="attributeName">The attribute name that about to get</param>
        /// <param name="callback">Returns an attribute via callback when completed.</param>
        public void GetGameProfileAttribute( string profileId
            , string attributeName
            , ResultCallback<GameProfileAttribute> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(profileId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetProfileIdInvalidMessage(profileId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameProfileAtrribute(
                    session.UserId,
                    profileId,
                    attributeName,
                    callback));
        }

        /// <summary>
        /// Update an attribute of current user's game profile
        /// </summary>
        /// <param name="profileId">The id of game profile that about to update</param>
        /// <param name="attribute">The attribute of game profile that about to update</param>
        /// <param name="callback">Returns updated game profile via callback when completed.</param>
        public void UpdateGameProfileAttribute( string profileId
            , GameProfileAttribute attribute
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(profileId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetProfileIdInvalidMessage(profileId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGameProfileAtrribute(
                    session.UserId,
                    profileId,
                    attribute,
                    callback));
        }
    }
}