// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerCloudSave : WrapperBase
    {
        private readonly ServerCloudSaveApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        private PredefinedEventScheduler predefinedEventScheduler;

        private enum PredefinedGameRecordMode
        {
            Updated,
            Deleted
        }

        [JsonConverter(typeof(StringEnumConverter))]
        private enum PredefinedGameRecordStrategy
        {
            [EnumMember(Value = "APPEND")]
            Append,
            [EnumMember(Value = "REPLACE")]
            Replace,
        }

        [UnityEngine.Scripting.Preserve]
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
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventScheduler">Predefined event scheduler object reference</param>
        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;
        }

        /// <summary>
        /// </summary>
        /// <param name="inAi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal ServerCloudSave( ServerCloudSaveApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        private void SendPredefinedEvent(
            PredefinedGameRecordMode mode, 
            string key, 
            string strategy = null,
            string setBy = null, 
            Dictionary<string, object> values = null)
        {
            if (predefinedEventScheduler != null)
            {
                IAccelByteTelemetryPayload payload;

                switch (mode)
                {
                    case PredefinedGameRecordMode.Updated:
                        payload = new PredefinedGameRecordUpdatedPayload(key, setBy, Utils.JsonUtils.SerializeWithStringEnum(strategy), values);
                        break;
                    case PredefinedGameRecordMode.Deleted:
                        payload = new PredefinedGameRecordDeletedPayload(key);
                        break;
                    default:
                        return;
                }

                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
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

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

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

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

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

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

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

            SendPredefinedEvent(
                PredefinedGameRecordMode.Updated,
                key,
                Utils.JsonUtils.SerializeWithStringEnum(PredefinedGameRecordStrategy.Append),
                setBy.GetString(),
                recordRequest);

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

            SendPredefinedEvent(PredefinedGameRecordMode.Deleted, key);

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

            SendPredefinedEvent(
                PredefinedGameRecordMode.Updated, 
                key, 
                Utils.JsonUtils.SerializeWithStringEnum(PredefinedGameRecordStrategy.Replace), 
                setBy.GetString(), 
                recordRequest);

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

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">The key of the record</param>
        /// <param name="callback"></param>
        public void GetGameRecords(string key, ResultCallback<GameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetGameRecords(key, callback));
        }

        /// <summary>
        /// Create new admin game record or append the existing admin game record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void CreateAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.CreateAdminGameRecord(key, recordRequest, callback));
        }

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void QueryAdminGameRecordsByKey(string key
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.QueryAdminGameRecordsByKey(key, callback));
        }

        /// <summary>
        /// Retrieve list of records key by namespace
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="limit">The limit of the keys result.</param>
        /// <param name="offset">Distance between the beginning of the data list and a given point</param>
        public void QueryAdminGameRecordKeys(ResultCallback<GameRecordList> callback
            , int limit = 20
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.QueryAdminGameRecordKeys(callback, offset, limit));
        }

        /// <summary>
        /// Create new admin game record or replace the existing admin game record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.ReplaceAdminGameRecord(key, recordRequest, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteAdminGameRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.DeleteAdminGameRecord(key, callback));
        }

        /// <summary>
        /// Create new admin user record or append the existing admin user record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="userId">UserId of the record owner.</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void CreateAdminUserRecord(string key
            , string userId
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminUserRecord> callback)
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

            coroutineRunner.Run(api.CreateAdminUserRecord(key, userId, recordRequest, callback));
        }

        /// <summary>
        /// Query admin user records by Key.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="userId">UserId of the record owner.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void QueryAdminUserRecordsByKey(string key
            , string userId
            , ResultCallback<AdminUserRecord> callback)
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

            coroutineRunner.Run(api.QueryAdminUserRecordsByKey(key, userId, callback));
        }

        /// <summary>
        /// Query admin user record Keys.
        /// </summary>
        /// <param name="userId">UserId of the record owner.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="limit">The limit of the keys result.</param>
        /// <param name="offset">Distance between the beginning of the data list and a given point</param>
        public void QueryAdminUserRecordKeys(string userId
            , ResultCallback<PaginatedGetAdminUserRecordKeys> callback
            , int limit = 20
            , int offset = 0)
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

            coroutineRunner.Run(api.QueryAdminUserRecordKeys(userId, callback, limit, offset));
        }

        /// <summary>
        /// Replace admin user record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="userId">UserId of the record owner.</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceAdminUserRecord(string key
            , string userId
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminUserRecord> callback)
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

            coroutineRunner.Run(api.ReplaceAdminUserRecord(key, userId, recordRequest, callback));
        }

        public void DeleteAdminUserRecord(string key
            , string userId
            , ResultCallback callback)
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

            coroutineRunner.Run(api.DeleteAdminUserRecord(key, userId, callback));
        }
    }
}
