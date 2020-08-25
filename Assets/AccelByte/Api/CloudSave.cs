// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access CloudSave service.
    /// </summary>
    public class CloudSave
    {
        private readonly CloudSaveApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly string @namespace;

        internal CloudSave(CloudSaveApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
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
        /// Save a user-level record. If the record doesn't exist, it will create and save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="isPublic">True if you want the record can be accessed by other user</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserRecord(string key, Dictionary<string, object> recordRequest, bool isPublic, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SaveUserRecord(this.@namespace, this.session.UserId, this.session.AuthorizationToken, key, recordRequest, isPublic, callback));
        }

        /// <summary>
        /// Get a record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result that contains UserRecord via callback when completed</param>
        public void GetUserRecord(string key, ResultCallback<UserRecord> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserRecord(this.@namespace, this.session.UserId, this.session.AuthorizationToken, key, false, callback));
        }

        /// <summary>
        /// Get other user's public record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="userId">UserId of the record owner</param>
        /// <param name="callback">Returns a Result that contains UserRecord via callback when completed</param>
        public void GetPublicUserRecord(string key, string userId, ResultCallback<UserRecord> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserRecord(this.@namespace, userId, this.session.AuthorizationToken, key, true, callback));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="isPublic">True if you want the record can be accessed by other user</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecord(string key, Dictionary<string, object> recordRequest, bool isPublic, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! RecordRequest parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ReplaceUserRecord(this.@namespace, this.session.UserId, this.session.AuthorizationToken, key, recordRequest, isPublic, callback));
        }

        /// <summary>
        /// Delete a record under the given key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteUserRecord(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteUserRecord(this.@namespace, this.session.UserId, this.session.AuthorizationToken, key, callback));
        }

        /// <summary>
        /// Save a namespace-level record. If the record doesn't exist, it will create and save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveGameRecord(string key, Dictionary<string, object> recordRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't save game record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save game record! RecordRequest parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SaveGameRecord(this.@namespace, this.session.AuthorizationToken, key, recordRequest, callback));
        }

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result that contains GameRecord via callback when completed</param>
        public void GetGameRecord(string key, ResultCallback<GameRecord> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't get game record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetGameRecord(this.@namespace, this.session.AuthorizationToken, key, callback));
        }

        /// <summary>
        /// Replace a record in namespace-level. If the record doesn't exist, it will create and save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceGameRecord(string key, Dictionary<string, object> recordRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't replace game record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace game record! RecordRequest parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ReplaceGameRecord(this.@namespace, this.session.AuthorizationToken, key, recordRequest, callback));
        }

        /// <summary>
        /// Delete a record under the given key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGameRecord(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't delete game record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteGameRecord(this.@namespace, this.session.AuthorizationToken, key, callback));
        }
    }
}