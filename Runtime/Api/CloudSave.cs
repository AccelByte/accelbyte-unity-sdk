// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide an API to access CloudSave service.
    /// </summary>
    public class CloudSave : WrapperBase
    {
        private readonly CloudSaveApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;

        private const int userIdsRequestLimit = 20;

        [UnityEngine.Scripting.Preserve]
        internal CloudSave( CloudSaveApi inApi
            , ISession inSession
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
        internal CloudSave( CloudSaveApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
        : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create
        /// and save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="isPublic">True if you want the record can be accessed by other user</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [Obsolete("This method will be deprecated in future, please use SaveUserRecord(string key, Dictionary<string, object> recordRequest, ResultCallback callback, bool isPublic = false)")]
        public void SaveUserRecord( string key
            , Dictionary<string, object> recordRequest
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
                    session.UserId,
                    key,
                    recordRequest,
                    callback,
                    isPublic));
        }

        /// <summary>
        /// Save a user-level record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="isPublic">True if you want the record can be accessed by other user. Default is false</param>
        public void SaveUserRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic = false)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                api.SaveUserRecord(
                    session.UserId,
                    key,
                    recordRequest,
                    callback,
                    isPublic));
        }

        /// <summary>
        /// Get a record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRecord via callback when completed
        /// </param>
        public void GetUserRecord( string key
            , ResultCallback<UserRecord> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserRecord(
                    session.UserId,
                    key,
                    callback,
                    isPublic:false));
        }

        /// <summary>
        /// Get other user's public record (arbitrary JSON data) by its key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="userId">UserId of the record owner</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRecord via callback when completed
        /// </param>
        public void GetPublicUserRecord( string key
            , string userId
            , ResultCallback<UserRecord> callback )
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
                api.GetUserRecord(
                    userId,
                    key,
                    cb =>
                    {
                        SendMultiParameterPredefinedEvent(EventMode.PublicPlayerGetRecord, userId, key);
                        HandleCallback(cb, callback);
                    },
                    isPublic:true));
        }
        
        #region ReplaceUserRecord Overloads
        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="isPublic">True if you want the record can be accessed by other user</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        [Obsolete("This method will be deprecated in future, please use " +
            "ReplaceUserRecord(string key, Dictionary<string, object> recordRequest, " +
            "ResultCallback callback, bool isPublic = false)")]
        public void ReplaceUserRecord( string key
            , Dictionary<string, object> recordRequest
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
                    session.UserId,
                    key,
                    recordRequest,
                    callback,
                    isPublic));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>

        public void ReplaceUserRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic = false)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            coroutineRunner.Run(
                api.ReplaceUserRecord(
                    session.UserId,
                    key,
                    recordRequest,
                    cb =>
                    {
                        SendPredefinedEvent(key, EventMode.PublicReplaceUserRecord);
                        HandleCallback(cb, callback);
                    },
                    isPublic));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="setPublic">
        /// Using metadata, True if you want the record can be accessed by other user. Default is false
        /// </param>
        public void ReplaceUserRecord( string key
            , bool setPublic
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ReplaceUserRecord(
                    session.UserId,
                    key,
                    recordRequest,
                    RecordSetBy.CLIENT,
                    setPublic,
                    cb =>
                    {
                        SendPredefinedEvent(key, EventMode.PublicReplaceUserRecord);
                        HandleCallback(cb, callback);
                    }));
        }
        #endregion /ReplaceUserRecord Overloads

        #region ReplaceUserRecordCheckLatest WithoutResponse
        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one,
        /// but will failed if lastUpdated is not up-to-date.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="lastUpdated">
        /// last time the record is updated. Retrieve it from GetGameRecord.
        /// </param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecordCheckLatest( string key
            , DateTime lastUpdated
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
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

            coroutineRunner.Run(
                api.ReplaceUserRecord(
                    session.UserId,
                    key, 
                    request,
                    cb =>
                    {
                        SendPredefinedEvent(key, EventMode.PublicReplaceUserRecord);
                        HandleCallback(cb, callback);
                    }));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// Beware:
        /// Function will try to get the latest value, put it in the custom modifier and
        /// request to replace the record. will retry it again when the record is updated
        /// by other user, until exhaust all the attempt.
        /// </summary>
        /// <param name="tryAttempt"> Attempt to try to replace the game record.</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="payloadModifier">
        /// Function to modify the latest record value with your customized modifier.
        /// </param>
        public void ReplaceUserRecordCheckLatest( int tryAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ReplaceUserRecordRecursive(
                tryAttempt,
                key,
                recordRequest,
                callback,
                payloadModifier);
        }

        private void ReplaceUserRecordRecursive( int remainingAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed, 
                    "Exhaust all retry attempt to modify game record. Please try again."));
                return;
            }

            GetUserRecord(key, getUserRecordResult =>
            {
                var updateRequest = new ConcurrentReplaceRequest();
                if (getUserRecordResult.IsError)
                {
                    if (getUserRecordResult.Error.Code == ErrorCode.PlayerRecordNotFound)
                    {
                        updateRequest.value = recordRequest;
                        updateRequest.updatedAt = DateTime.Now;

                        coroutineRunner.Run(
                            api.ReplaceUserRecord(
                                session.UserId,
                                key,
                                updateRequest,
                                callback,
                                () =>
                                {
                                    ReplaceUserRecordRecursive(
                                        remainingAttempt - 1,
                                        key, 
                                        recordRequest, 
                                        callback, 
                                        payloadModifier);
                                }));
                    }
                    else
                    {
                        callback.TryError(getUserRecordResult.Error);
                    }
                }
                else
                {
                    getUserRecordResult.Value.value = payloadModifier(getUserRecordResult.Value.value);

                    updateRequest.value = getUserRecordResult.Value.value;
                    updateRequest.updatedAt = getUserRecordResult.Value.updated_at;

                    coroutineRunner.Run(
                        api.ReplaceUserRecord(
                            session.UserId,
                            key,
                            updateRequest,
                            callback,
                            () =>
                            {
                                ReplaceUserRecordRecursive(
                                    remainingAttempt - 1, 
                                    key, 
                                    recordRequest, 
                                    callback,
                                    payloadModifier);
                            }));
                }
            });
        }
        #endregion /ReplaceUserRecordCheckLatest WithoutResponse

        #region ReplaceUserRecordCheckLatest WithResponse
        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one,
        /// but will failed if lastUpdated is not up-to-date.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="lastUpdated">
        /// last time the record is updated. Retrieve it from GetGameRecord.
        /// </param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceUserRecordCheckLatestWithResponse( string key
            , DateTime lastUpdated
            , Dictionary<string, object> recordRequest
            , ResultCallback<ConcurrentReplaceUserRecordResponse> callback )
        {
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

            coroutineRunner.Run(
                api.ReplaceUserRecord(
                    session.UserId,
                    key, 
                    request,
                    callback));
        }

        /// <summary>
        /// Replace a record in user-level. If the record doesn't exist, it will create and
        /// save the record. If already exists, it will replace the existing one.
        /// Beware:
        /// Function will try to get the latest value, put it in the custom modifier and
        /// request to replace the record. will retry it again when the record is updated
        /// by other user, until exhaust all the attempt.
        /// </summary>
        /// <param name="tryAttempt"> Attempt to try to replace the game record.</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="payloadModifier">
        /// Function to modify the latest record value with your customized modifier.
        /// </param>
        public void ReplaceUserRecordCheckLatestWithResponse( int tryAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<ConcurrentReplaceUserRecordResponse> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ReplaceUserRecordRecursive(
                tryAttempt,
                key,
                recordRequest,
                callback,
                payloadModifier);
        }

        private void ReplaceUserRecordRecursive( int remainingAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<ConcurrentReplaceUserRecordResponse> callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed, 
                    "Exhaust all retry attempt to modify game record. Please try again."));
                return;
            }

            GetUserRecord(key, getUserRecordResult =>
            {
                var updateRequest = new ConcurrentReplaceRequest();
                if (getUserRecordResult.IsError)
                {
                    if (getUserRecordResult.Error.Code == ErrorCode.PlayerRecordNotFound)
                    {
                        updateRequest.value = recordRequest;
                        updateRequest.updatedAt = DateTime.Now;

                        coroutineRunner.Run(
                            api.ReplaceUserRecord(
                                session.UserId,
                                key,
                                updateRequest,
                                callback,
                                () =>
                                {
                                    ReplaceUserRecordRecursive(
                                        remainingAttempt - 1,
                                        key, 
                                        recordRequest, 
                                        callback, 
                                        payloadModifier);
                                }));
                    }
                    else
                    {
                        callback.TryError(getUserRecordResult.Error);
                    }
                }
                else
                {
                    getUserRecordResult.Value.value = payloadModifier(getUserRecordResult.Value.value);

                    updateRequest.value = getUserRecordResult.Value.value;
                    updateRequest.updatedAt = getUserRecordResult.Value.updated_at;

                    coroutineRunner.Run(
                        api.ReplaceUserRecord(
                            session.UserId,
                            key,
                            updateRequest,
                            callback,
                            () =>
                            {
                                ReplaceUserRecordRecursive(
                                    remainingAttempt - 1, 
                                    key, 
                                    recordRequest, 
                                    callback,
                                    payloadModifier);
                            }));
                }
            });
        }
        #endregion /ReplaceUserRecordCheckLatest WithResponse

        /// <summary>
        /// Delete a record under the given key in user-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteUserRecord( string key
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteUserRecord(session.UserId, key, callback));
        }

        /// <summary>
        /// Save a namespace-level record. If the record doesn't exist, it will create and
        /// save the record, if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SaveGameRecord(key, recordRequest, cb =>
                {
                    SendPredefinedEvent(key, EventMode.GameSaveRecord);
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Get a record by its key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">
        /// Returns a Result that contains GameRecord via callback when completed
        /// </param>
        public void GetGameRecord( string key
            , ResultCallback<GameRecord> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameRecord(key, cb =>
                {
                    SendPredefinedEvent(key, EventMode.GameGetRecord);
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Replace a record in namespace-level. If the record doesn't exist, it will
        /// create and save the record. If already exists, it will replace the existing one.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ReplaceGameRecord(key, recordRequest, callback));
        }

        /// <summary>
        /// Replace a record in namespace-level. If the record doesn't exist, it will
        /// create and save the record. If already exists, it will replace the existing
        /// one, but will failed if lastUpdated is not up-to-date.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="lastUpdated">
        /// last time the record is updated. Retrieve it from GetGameRecord.
        /// </param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ReplaceGameRecordCheckLatest( string key
            , DateTime lastUpdated
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ConcurrentReplaceRequest request = new ConcurrentReplaceRequest
            {
                updatedAt = lastUpdated,
                value = recordRequest
            };

            coroutineRunner.Run(
                api.ReplaceGameRecord(
                    key,
                    request,
                    callback));
        }

        /// <summary>
        /// Replace a record in namespace-level. If the record doesn't exist, it will
        /// create and save the record. If already exists, it will replace the existing one.
        /// Beware:
        /// Function will try to get the latest value, put it in the custom modifier and
        /// request to replace the record. will retry it again when the record is
        /// updated by other user, until exhaust all the attempt.
        /// </summary>
        /// <param name="tryAttempt"> Attempt to try to replace the game record.</param>
        /// <param name="key">Key of record</param>
        /// <param name="recordRequest">The request of the record with JSON formatted.</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="payloadModifier">
        /// Function to modify the latest record value with your customized modifier.
        /// </param>
        public void ReplaceGameRecordCheckLatest( int tryAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            ReplaceGameRecordRecursive(
                tryAttempt,
                key,
                recordRequest,
                callback,
                payloadModifier);
        }

        private void ReplaceGameRecordRecursive( int remainingAttempt
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , Func<Dictionary<string, object>, Dictionary<string, object>> payloadModifier )
        {
            if (remainingAttempt <= 0)
            {
                callback.TryError(new Error(ErrorCode.PreconditionFailed, 
                    "Exhaust all retry attempt to modify game record. Please try again."));
                return;
            }

            GetGameRecord(key, getGameRecordResult =>
            {
                var updateRequest = new ConcurrentReplaceRequest();
                if (getGameRecordResult.IsError)
                {
                    if (getGameRecordResult.Error.Code == ErrorCode.GameRecordNotFound)
                    {
                        updateRequest.value = recordRequest;
                        updateRequest.updatedAt = DateTime.Now;

                        coroutineRunner.Run(
                            api.ReplaceGameRecord(
                                key,
                                updateRequest,
                                callback,
                                () =>
                                {
                                    ReplaceGameRecordRecursive(
                                        remainingAttempt - 1, 
                                        key, 
                                        recordRequest, 
                                        callback, 
                                        payloadModifier);
                                }));
                    }
                    else
                    {
                        callback.TryError(getGameRecordResult.Error);
                    }
                }
                else
                {
                    getGameRecordResult.Value.value = payloadModifier(getGameRecordResult.Value.value);

                    updateRequest.value = getGameRecordResult.Value.value;
                    updateRequest.updatedAt = getGameRecordResult.Value.updated_at;

                    coroutineRunner.Run(
                        api.ReplaceGameRecord(
                            key,
                            updateRequest,
                            callback,
                            () =>
                            {
                                ReplaceGameRecordRecursive(
                                    remainingAttempt - 1,
                                    key, 
                                    recordRequest, 
                                    callback, 
                                    payloadModifier);
                            }));
                }
            });
        }

        /// <summary>
        /// Delete a record under the given key in namespace-level.
        /// </summary>
        /// <param name="key">Key of record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGameRecord( string key
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteGameRecord(key, cb =>
                {
                    SendPredefinedEvent(key, EventMode.GameDeletedRecord);
                    HandleCallback(cb, callback);
                }));
        }
        
        /// <summary>
        /// Bulk get user records by keys.
        /// </summary>
        /// <param name="keys">Keys of records</param>
        /// <param name="callback">Returns a Result that contains UserRecords via callback when completed</param>
        public void BulkGetUserRecords( string[] keys
            , ResultCallback<UserRecords> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            BulkGetRecordsByKeyRequest bulkGetRecordsByKeyRequest = new BulkGetRecordsByKeyRequest
            {
                keys = keys
            };
            
            coroutineRunner.Run(
                api.BulkGetUserRecords(bulkGetRecordsByKeyRequest, cb =>
                {
                    SendPredefinedEvent(keys, EventMode.PlayerGetRecords);
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Bulk get public user records by multiple userIds and keys.
        /// </summary>
        /// <param name="key">Keys of records</param>
        /// <param name="userIds">Collection of AccelByte Ids</param>
        /// <param name="callback">Returns a result that contains UserRecords</param>
        public void BulkGetPublicUserRecord(string key
            , string[] userIds
            , ResultCallback<UserRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (userIds.Length <= 0 || userIds.Length > userIdsRequestLimit)
            {
                callback.TryError(ErrorCode.InvalidRequest);
                return;
            }

            BulkGetPublicUserRecordsByUserIdsRequest request = new BulkGetPublicUserRecordsByUserIdsRequest()
            {
                UserIds = userIds
            };

            coroutineRunner.Run(
                api.BulkGetPublicUserRecords(key, request, callback));
        }

        /// <summary>
        /// Bulk get game records by keys.
        /// </summary>
        /// <param name="keys">Keys of records</param>
        /// <param name="callback">Returns a Result that contains GameRecords via callback when completed</param>
        public void BulkGetGameRecords( string[] keys
            , ResultCallback<GameRecords> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            BulkGetRecordsByKeyRequest bulkGetRecordsByKeyRequest = new BulkGetRecordsByKeyRequest
            {
                keys = keys
            };
            
            coroutineRunner.Run(
                api.BulkGetGameRecords(bulkGetRecordsByKeyRequest, cb =>
                {
                    SendPredefinedEvent(keys, EventMode.GameGetRecords);
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Bulk get other user public record keys.
        /// </summary>
        /// <param name="userId">UserId of the record owner</param>
        /// <param name="callback">Returns a Result that contains GameRecords via callback when completed</param>
        /// <param name="offset">The offset of the public record keys. Default value is 0.</param>
        /// <param name="limit">The limit of the public record keys. Default value is 20.</param>
        public void BulkGetOtherPlayerPublicRecordKeys(string userId
            , ResultCallback<PaginatedBulkGetPublicUserRecordKeys> callback
            , int offset = 0
            , int limit = 20)
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
                api.BulkGetOtherPlayerPublicRecordKeys(userId, 
                    cb =>
                    {
                        SendPredefinedEvent(userId, EventMode.PublicPlayerGetOtherUserKeys);
                        HandleCallback(cb, callback);
                    }, 
                    offset, 
                    limit));
        }

        /// <summary>
        /// Bulk get other user public records by keys.
        /// </summary>
        /// <param name="userId">UserId of the record owner</param>
        /// <param name="data">Data struct to list Key of record</param>
        /// <param name="callback">Returns a Result that contains GameRecords via callback when completed</param>
        public void BulkGetOtherPlayerPublicRecords(string userId
            , BulkGetRecordsByKeyRequest data
            , ResultCallback<UserRecords> callback)
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
                api.BulkGetOtherPlayerPublicRecords(userId, data, cb =>
                {
                    SendMultiParameterPredefinedEvent(EventMode.PublicPlayerGetOtherUserRecords, userId, data.keys);
                    HandleCallback(cb, callback);
                }));
        }

        #region PredefinedEvents
        private enum EventMode
        {
            PlayerGetRecords,
            PublicPlayerGetRecord,
            PublicReplaceUserRecord,
            PublicPlayerGetOtherUserKeys,
            PublicPlayerGetOtherUserRecords,
            GameSaveRecord,
            GameGetRecord,
            GameDeletedRecord,
            GameGetRecords
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T value, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;
            string localUserId = session.UserId;

            switch (eventMode)
            {
                case EventMode.PlayerGetRecords:
                    var playerGetRecordsResult = value as string[];
                    payload = new PredefinedPlayerRecordGetRecordsPayload(localUserId, playerGetRecordsResult);
                    break;

                case EventMode.PublicPlayerGetRecord:
                    var publicPlayerGetRecordResult = value as object[];
                    payload = new PredefinedPublicPlayerRecordGetRecordPayload((string)publicPlayerGetRecordResult[0], (string)publicPlayerGetRecordResult[1]);
                    break;

                case EventMode.PublicReplaceUserRecord:
                    var publicPlayerReplaceUserRecordResult = value as string;
                    payload = new PredefinedPublicPlayerRecordUpdatedPayload(localUserId, publicPlayerReplaceUserRecordResult);
                    break;

                case EventMode.PublicPlayerGetOtherUserKeys:
                    var publicPlayerGetOtherUserKeysResult = value as string;
                    payload = new PredefinedPublicPlayerRecordGetOtherUserKeysPayload(publicPlayerGetOtherUserKeysResult);
                    break;

                case EventMode.PublicPlayerGetOtherUserRecords:
                    var publicPlayerGetOtherUserRecordsResult = value as object[];
                    var userId = publicPlayerGetOtherUserRecordsResult[0];
                    payload = new PredefinedPublicPlayerRecordGetOtherUserRecordsPayload((string)userId, (string[])publicPlayerGetOtherUserRecordsResult[1]);
                    break;

                case EventMode.GameSaveRecord:
                    var gameSaveRecordResult = value as string;
                    payload = new PredefinedGameRecordCreatedPayload(localUserId, gameSaveRecordResult);
                    break;

                case EventMode.GameGetRecord:
                    var gameGetRecordResult = value as string;
                    payload = new PredefinedGameRecordGetRecordPayload(localUserId, gameGetRecordResult);
                    break;

                case EventMode.GameDeletedRecord:
                    var gameDeletedRecordResult = value as string;
                    payload = new PredefinedGameRecordDeletedPayload(localUserId, gameDeletedRecordResult);
                    break;

                case EventMode.GameGetRecords:
                    var gameGetRecordsResult = value as string[];
                    payload = new PredefinedGameRecordGetRecordsPayload(localUserId, gameGetRecordsResult);
                    break;
            }

            return payload;
        }

        private void SendMultiParameterPredefinedEvent(EventMode eventMode, params object[] eventParameters)
        {
            var payload = CreatePayload(eventParameters, eventMode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent<T>(T value, EventMode eventMode)
        {
            var payload = CreatePayload(value, eventMode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            if (payload == null)
            {
                return;
            }

            AccelByteTelemetryEvent rewardEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(rewardEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        #endregion
    }
}
