// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
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

        private enum PredefinedGameRecordMode
        {
            Updated,
            Deleted
        }

        [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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

        private void SendPredefinedEvent(
            PredefinedGameRecordMode mode, 
            string key, 
            string strategy = null,
            string setBy = null, 
            Dictionary<string, object> values = null)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler != null)
            {
                IAccelByteTelemetryPayload payload;

                switch (mode)
                {
                    case PredefinedGameRecordMode.Updated:
                        payload = new PredefinedGameRecordUpdatedPayload(key, setBy, Utils.JsonUtils.SerializeWithStringEnum(strategy), values);
                        break;
                    case PredefinedGameRecordMode.Deleted:
                        payload = new PredefinedGameRecordDeletedPayload(session.UserId, key);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SaveUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic: false);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! RecordRequest parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.SaveUserRecord(
                userId,
                key,
                recordRequest,
                setBy,
                isPublic,
                callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserRecord(userId, key, false, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! RecordRequest parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ReplaceUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic:false);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! RecordRequest parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            
            api.ReplaceUserRecord(
                userId,
                key,
                recordRequest,
                setBy,
                isPublic,
                callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteUserRecord(
                userId,
                key, 
                callback);
        }

        /// <summary>
        /// Save a game record.If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record to be formatted as JSON</param>
        /// <param name="optionalParams">Optional params to set metadata (Can be null)</param>
        /// <param name="callback">Returns a GameRecord result via callback when completed</param>
        public void SaveGameRecord(string key
            , Dictionary<string, object> recordRequest
            , GameRecordMetadataOptionalParams optionalParams
            , ResultCallback<GameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GameRecordMetadataOptionalParams();
            }

            SendPredefinedEvent(
                PredefinedGameRecordMode.Updated,
                key,
                Utils.JsonUtils.SerializeWithStringEnum(PredefinedGameRecordStrategy.Append),
                optionalParams.SetBy.GetString(),
                recordRequest);

            api.SaveGameRecord(
                key,
                recordRequest,
                optionalParams,
                callback);
        }

        /// <summary>
        /// Delete a game record under the given key.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGameRecord( string key
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            SendPredefinedEvent(PredefinedGameRecordMode.Deleted, key);

            api.DeleteGameRecord(key, callback);
        }

        /// <summary>
        /// Replace a game record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record to be formatted as JSON</param>
        /// <param name="optionalParams">Optional params to set metadata (Can be null)</param>
        /// <param name="callback">Returns a GameRecord result via callback when completed</param>
        public void ReplaceGameRecord(string key
            , Dictionary<string, object> recordRequest
            , GameRecordMetadataOptionalParams optionalParams
            , ResultCallback<GameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GameRecordMetadataOptionalParams();
            }

            SendPredefinedEvent(
                PredefinedGameRecordMode.Updated,
                key,
                Utils.JsonUtils.SerializeWithStringEnum(PredefinedGameRecordStrategy.Replace),
                optionalParams.SetBy.GetString(),
                recordRequest);

            api.ReplaceGameRecord(
                key,
                recordRequest,
                optionalParams,
                callback);
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

            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ConcurrentReplaceRequest
            {
                
                updatedAt = lastUpdated, 
                value = recordRequest,
            };

            api.ReplaceGameRecord(key, request, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RetrieveGameRecordsKey(callback, query, offset, limit);
        }

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">The key of the record</param>
        /// <param name="callback"></param>
        public void GetGameRecords(string key, ResultCallback<GameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);
            
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetGameRecords(key, callback);
        }

        /// <summary>
        /// Create new admin game record or append the existing admin game record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="optionalParams">Optional params to set metadata (Can be null)</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void CreateAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , AdminRecordMetadataOptionalParams optionalParams
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateAdminGameRecord(key, recordRequest, optionalParams, callback);
        }

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void QueryAdminGameRecordsByKey(string key
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryAdminGameRecordsByKey(key, callback);
        }

        /// <summary>
        /// Get an array of records by their keys in namespace-level.
        /// </summary>
        /// <param name="keys">Array of keys of record to get. Maximum value of 20.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void BulkQueryAdminGameRecordsByKey(string[] keys
            , ResultCallback<BulkAdminGameRecordResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkQueryAdminGameRecordsByKey(keys, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryAdminGameRecordKeys(callback, limit, offset);
        }

        /// <summary>
        /// Create new admin game record or replace the existing admin game record.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="optionalParams">Optional params to set metadata (Can be null)</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , AdminRecordMetadataOptionalParams optionalParams
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ReplaceAdminGameRecord(key, recordRequest, optionalParams, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteAdminGameRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteAdminGameRecord(key, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateAdminUserRecord(key, userId, recordRequest, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryAdminUserRecordsByKey(key, userId, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryAdminUserRecordKeys(userId, callback, limit, offset);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ReplaceAdminUserRecord(key, userId, recordRequest, callback);
        }

        public void DeleteAdminUserRecord(string key
            , string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteAdminUserRecord(key, userId, callback);
        }

        /// <summary>
        /// Delete the TTLConfig for a specific Admin Game Record by its key.
        /// </summary>
        /// <param name="key">Key of the admin game record.</param>
        /// <param name="callback">Returns a result via callback when completed.</param>
        public void DeleteAdminGameRecordTTLConfig(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteAdminGameRecordTTLConfig(key, callback);
        }

        /// <summary>
        /// Delete the TTLConfig for a specific Game Record by its key.
        /// </summary>
        /// <param name="key">Key of the game record.</param>
        /// <param name="callback">Returns a result via callback when completed.</param>
        public void DeleteGameRecordTTLConfig(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteGameRecordTTLConfig(key, callback);
        }
    }
}
