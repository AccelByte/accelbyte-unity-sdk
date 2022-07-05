// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerCloudSave : WrapperBase
    {
        private readonly ServerCloudSaveApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerCloudSave( ServerCloudSaveApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct CloudSave manager; api is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct CloudSave manager; coroutineRunner is null!");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inAi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal ServerCloudSave( ServerCloudSaveApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create and save
        /// the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.SaveUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic: false));
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create and save
        /// the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Indicate which party that could modify the player record</param>
        /// <param name="isPublic">Indicate whether the player record is a public record or not</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool isPublic
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
                
            }

            coroutineRunner.Run(
                api.SaveUserRecord(
                    userId,
                    key,
                    recordRequest,
                    setBy,
                    isPublic,
                    callback));
        }

        /// <summary>
        /// Get a record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRecord via callback when completed
        /// </param>
        public void GetUserRecord( string userId
            , string key
            , ResultCallback<UserRecord> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserRecord(userId, key, false, callback));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.ReplaceUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic:false));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Indicate which party that could modify the player record.</param>
        /// <param name="isPublic">Indicate whether the player record is a public record or not.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool isPublic
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                api.ReplaceUserRecord(
                    userId,
                    key,
                    recordRequest,
                    setBy,
                    isPublic,
                    callback));
        }

        /// <summary>
        /// Delete a record under the given key in user-level.
        /// </summary>
        /// <param name="userId">Targeted user ID</param>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteUserRecord( string userId
            , string key
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.DeleteUserRecord(
                userId,
                key, 
                callback));
        }


        /// <summary>
        /// Save a game record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Record set by.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.SaveGameRecord(
                key,
                recordRequest,
                setBy,
                callback));
        }

        /// <summary>
        /// Delete a game record under the given key.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGameRecord( string key
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.DeleteGameRecord(key, callback));
        }

        /// <summary>
        /// Replace a game record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="setBy">Record set by.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.ReplaceGameRecord(
                key, 
                recordRequest,
                setBy, 
                callback));
        }

        /// <summary>
        /// Replace a game record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="lastUpdated">Last updated</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param> 
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceGameRecordCheckLatest( string key
            , DateTime lastUpdated
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(key), "Key should not be null.");
            Assert.IsNotNull(recordRequest, "RecordRequest should not be null.");

            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ConcurrentReplaceRequest
            {
                
                updatedAt = lastUpdated, 
                value = recordRequest,
            };

            coroutineRunner.Run(api.ReplaceGameRecord(
                key, request, callback));
        }

        /// <summary>
        /// Get a record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="callback">
        /// <param name="query">list of GameRecordKey</param>
        /// <param name="offset">Offset The offset of GameRecord result. Default value is 0.</param>
        /// <param name="limit">limit The limit of GameRecord result. Default value is 20.</param>
        /// Returns a Result that contains UserRecord via callback when completed
        /// </param>
        public void RetrieveGameRecordsKey(ResultCallback<GameRecordList> callback
            , string query = "{}"
            , int offset = 0
            , int limit  = 20 )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(offset.ToString(), "Can't get query user record! offset parameter is null!");
            Assert.IsNotNull(limit.ToString(), "Can't get query user record! limit parameter is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RetrieveGameRecordsKey(callback, query, offset, limit));
        }
    }
}
