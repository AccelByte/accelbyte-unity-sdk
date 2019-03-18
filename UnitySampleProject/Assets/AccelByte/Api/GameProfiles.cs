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
        private readonly User user;
        private readonly AsyncTaskDispatcher taskDispatcher;
        private readonly CoroutineRunner coroutineRunner;

        internal GameProfiles(GameProfilesApi api, User user, AsyncTaskDispatcher taskDispatcher,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct GameProfile manager; api is null!");
            Assert.IsNotNull(user, "Can not construct GameProfile manager; userAccount is null!");
            Assert.IsNotNull(taskDispatcher, "Can not construct GameProfile manager; taskDispatcher is null!");
            Assert.IsNotNull(coroutineRunner, "Can not construct GameProfile manager; coroutineRunner is null!");

            this.api = api;
            this.user = user;
            this.taskDispatcher = taskDispatcher;
            this.coroutineRunner = coroutineRunner;
        }

        public void BatchGetGameProfiles(ICollection<string> userIds, ResultCallback<UserGameProfiles[]> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<UserGameProfiles[]>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.BatchGetGameProfiles(
                        this.user.Namespace,
                        userIds,
                        this.user.AccessToken,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<UserGameProfiles[]>) result)),
                    this.user));
        }

        public void GetAllGameProfiles(ResultCallback<GameProfile[]> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile[]>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetAllGameProfiles(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile[]>) result)),
                    this.user));
        }

        public void CreateGameProfile(GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.CreateGameProfile(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        gameProfile,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile>) result)),
                    this.user));
        }

        public void GetGameProfile(string profileId, ResultCallback<GameProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetGameProfile(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        profileId,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile>) result)),
                    this.user));
        }

        public void UpdateGameProfile(GameProfile gameProfile, ResultCallback<GameProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateGameProfile(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        gameProfile.profileId,
                        gameProfile,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile>) result)),
                    this.user));
        }

        public void UpdateGameProfile(string profileId, GameProfileRequest gameProfile,
            ResultCallback<GameProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateGameProfile(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        profileId,
                        gameProfile,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile>) result)),
                    this.user));
        }

        public void DeleteGameProfile(string profileId, ResultCallback callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.DeleteGameProfile(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        profileId,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result) result)),
                    this.user));
        }

        public void GetGameProfileAttribute(string profileId, string attributeName,
            ResultCallback<GameProfileAttribute> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(
                    Result<GameProfileAttribute>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.GetGameProfileAtrribute(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        profileId,
                        attributeName,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfileAttribute>) result)),
                    this.user));
        }

        public void UpdateGameProfileAttribute(string profileId, GameProfileAttribute attribute,
            ResultCallback<GameProfile> callback)
        {
            if (!this.user.IsLoggedIn)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.IsNotLoggedIn, "User is not logged in"));

                return;
            }

            this.taskDispatcher.Start(
                Task.Retry(
                    cb => this.api.UpdateGameProfileAtrribute(
                        this.user.Namespace,
                        this.user.UserId,
                        this.user.AccessToken,
                        profileId,
                        attribute,
                        result => cb(result)),
                    result => this.coroutineRunner.Run(() => callback((Result<GameProfile>) result)),
                    this.user));
        }
    }
}