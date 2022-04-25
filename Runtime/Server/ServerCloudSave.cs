// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerCloudSave
    {
        private readonly ServerCloudSaveApi api;
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerCloudSave(ServerCloudSaveApi api, IServerSession session, string namespace_, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct CloudSave manager; api is null!");
            Assert.IsNotNull(session, "Can not construct CloudSave manager; session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(namespace_), "Can not construct CloudSave manager; namespace parameter couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "Can not construct CloudSave manager; coroutineRunner is null!");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create and save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserRecord(string userId, string key, Dictionary<string, object> recordRequest, ResultCallback callback)
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
                this.api.SaveUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, recordRequest, callback, false));
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create and save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Indicate which party that could modify the player record</param>
        /// <param name="isPublic">Indicate whether the player record is a public record or not</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserRecord(string userId, string key, Dictionary<string, object> recordRequest, 
            RecordSetBy setBy, bool isPublic, ResultCallback callback)
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
                this.api.SaveUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, recordRequest, setBy, isPublic, callback));
        }

        /// <summary>
        /// Get a record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result that contains UserRecord via callback when completed</param>
        public void GetUserRecord(string userId, string key, ResultCallback<UserRecord> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, false, callback));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecord(string userId, string key, Dictionary<string, object> recordRequest, ResultCallback callback)
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
                this.api.ReplaceUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, recordRequest, callback, false));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Indicate which party that could modify the player record.</param>
        /// <param name="isPublic">Indicate whether the player record is a public record or not.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecord(string userId, string key, Dictionary<string, object> recordRequest,
            RecordSetBy setBy, bool isPublic, ResultCallback callback)
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
                this.api.ReplaceUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, recordRequest, setBy, isPublic, callback));
        }

        /// <summary>
        /// Delete a record under the given key in user-level.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteUserRecord(string userId, string key, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteUserRecord(this.namespace_, userId, this.session.AuthorizationToken, key, callback));
        }
    }
}
