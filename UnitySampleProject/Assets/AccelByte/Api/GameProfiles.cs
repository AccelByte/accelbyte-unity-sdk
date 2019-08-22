// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameProfiles
    {
        private readonly GameProfilesApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal GameProfiles(GameProfilesApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
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
        /// Get all profiles of specified users
        /// </summary>
        /// <param name="userIds">Id of some users to get</param>
        /// <param name="callback">Returns all profiles for specified users via callback when completed.</param>
        public void BatchGetGameProfiles(ICollection<string> userIds, ResultCallback<UserGameProfiles[]> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.BatchGetGameProfiles(this.@namespace, userIds, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get all profiles of current user
        /// </summary>
        /// <param name="callback">Returns all profiles of current user via callback when completed.</param>
        public void GetAllGameProfiles(ResultCallback<GameProfile[]> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetAllGameProfiles(this.@namespace, this.session.UserId, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Create  new profile for current user
        /// </summary>
        /// <param name="gameProfile">The game profile that about to create</param>
        /// <param name="callback">Returns the created game profile via callback when completed.</param>
        public void CreateGameProfile(GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateGameProfile(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    gameProfile,
                    callback));
        }

        /// <summary>
        /// Get a profile of current user by the profile id
        /// </summary>
        /// <param name="profileId"> the id of profile that about to get</param>
        /// <param name="callback">Returns a profile of current user via callback when completed. </param>
        public void GetGameProfile(string profileId, ResultCallback<GameProfile> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGameProfile(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    profileId,
                    callback));
        }

        /// <summary>
        /// Update current user's game profile
        /// </summary>
        /// <param name="gameProfile">The game profile that about to update</param>
        /// <param name="callback">Returns updated game profile via callback when completed.</param>
        public void UpdateGameProfile(GameProfile gameProfile, ResultCallback<GameProfile> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGameProfile(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
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
        public void UpdateGameProfile(string profileId, GameProfileRequest gameProfile,
            ResultCallback<GameProfile> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGameProfile(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    profileId,
                    gameProfile,
                    callback));
        }

        /// <summary>
        /// Delete current user's game profile identified by the game profile id
        /// </summary>
        /// <param name="profileId">The id of game profile that about to delete</param>
        /// <param name="callback">Returns boolean status via callback when completed.</param>
        public void DeleteGameProfile(string profileId, ResultCallback callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteGameProfile(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    profileId,
                    callback));
        }

        /// <summary>
        /// Get attribute of specified current user's game profile identified by the attribute name
        /// </summary>
        /// <param name="profileId">The game profile that about to get</param>
        /// <param name="attributeName">The attribute name that about to get</param>
        /// <param name="callback">Returns an attribute via callback when completed.</param>
        public void GetGameProfileAttribute(string profileId, string attributeName,
            ResultCallback<GameProfileAttribute> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGameProfileAtrribute(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
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
        public void UpdateGameProfileAttribute(string profileId, GameProfileAttribute attribute,
            ResultCallback<GameProfile> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateGameProfileAtrribute(
                    this.@namespace,
                    this.session.UserId,
                    this.session.AuthorizationToken,
                    profileId,
                    attribute,
                    callback));
        }
    }
}