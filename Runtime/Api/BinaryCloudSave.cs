// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class BinaryCloudSave : WrapperBase
    {
        private readonly BinaryCloudSaveApi binaryCloudSaveApi;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal BinaryCloudSave(BinaryCloudSaveApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            binaryCloudSaveApi = inApi;
            Assert.IsNotNull(inSession, "session==null (@ constructor)");
            session = inSession;
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Save a namespace-level user binary.
        /// If the binary doesn't exist, it will create the binary save, 
        /// if already exists, it will append to the existing one.
        /// </summary>
        /// <param name="key">Key of the binary record.</param>
        /// <param name="fileType">File type of the binary 
        /// (supported file types are jpeg, jpg, png, bmp, gif, mp3, webp, and bin).</param>
        /// <param name="callback">Returns a Result that contains UserBinaryRecord via callback when completed.</param>
        /// <param name="isPublic">Whether to save the binary as a public or private record. (Optional)</param>
        public void SaveUserBinaryRecord(string key
            , string fileType
            , ResultCallback<SaveBinaryRecordResponse> callback
            , bool isPublic = false)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.SaveUserBinaryRecord(key, fileType, isPublic, callback);
        }

        /// <summary>
        /// Get current user's binary record
        /// </summary>
        /// <param name="key">Key of the binary record.</param>
        /// <param name="callback">Returns a Result that contains UserBinaryRecord via callback when completed.</param>
        public void GetCurrentUserBinaryRecord(string key
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.GetCurrentUserBinaryRecord(key, callback);
        }

        /// <summary>
        /// Get a user's public binary record by its key and the owner's UserId
        /// </summary>
        /// <param name="key">Key of the binary record.</param>
        /// <param name="userId">ID of the user who owns the binary record.</param>
        /// <param name="callback">Returns a Result that contains UserBinaryRecord via callback when completed.</param>
        public void GetPublicUserBinaryRecord(string key
            , string userId
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.GetPublicUserBinaryRecord(key, userId, callback);
        }

        /// <summary>
        /// Bulk get current user's binary records by their keys
        /// </summary>
        /// <param name="keys">List of the keys of the binary records.</param>
        /// <param name="callback">Returns a Result that contains ListUserBinaryRecords via callback when completed.</param>
        public void BulkGetCurrentUserBinaryRecords(string[] keys
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkGetCurrentUserBinaryRecords(keys, callback);
        }

        /// <summary>
        /// Bulk get multiple public binary records from a single user by their keys
        /// </summary>
        /// <param name="keys">List of the keys of the binary records.</param>
        /// <param name="userId">ID of the user who owns the binary records.</param>
        /// <param name="callback">Returns a Result that contains ListUserBinaryRecords via callback when completed.</param>
        public void BulkGetPublicUserBinaryRecords(string[] keys
            , string userId
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkGetPublicUserBinaryRecords(keys, userId, callback);
        }

        /// <summary>
        /// Bulk get public binary records with the same key from multiple users
        /// </summary>
        /// <param name="key">Key of the binary records</param>
        /// <param name="userIds">UserId list of the binary record owner(s)</param>
        /// <param name="callback">Returns a Result that contains ListUserBinaryRecords via callback when completed</param>
        public void BulkGetPublicUserBinaryRecords(string key
            , string[] userIds
            , ResultCallback<ListUserBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkGetPublicUserBinaryRecords(key, userIds, callback);
        }

        /// <summary>
        /// Bulk query current user's binary records
        /// </summary>
        /// <param name="query">String that will be used to query the user's keys</param>
        /// <param name="callback">Returns a Result that contains PaginatedUserBinaryRecords 
        /// via callback when completed</param>
        /// <param name="offset">The offset of the binary records. Default value is 0</param>
        /// <param name="limit">The limit of the binary records. Default value is 0</param>
        public void BulkQueryCurrentUserBinaryRecords(string query
            , ResultCallback<PaginatedUserBinaryRecords> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkQueryCurrentUserBinaryRecords(query, callback, offset, limit);
        }

        /// <summary>
        /// Bulk query all of a user's public binary records
        /// </summary>
        /// <param name="userId">ID of the user who owns the binary records</param>
        /// <param name="callback">Returns a Result that contains PaginatedUserBinaryRecords 
        /// via callback when completed</param>
        /// <param name="offset">The offset of the binary records. Default value is 0</param>
        /// <param name="limit">The limit of the binary records. Default value is 0</param>
        public void BulkQueryPublicUserBinaryRecords(string userId
            , ResultCallback<PaginatedUserBinaryRecords> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkQueryPublicUserBinaryRecords(userId, callback, offset, limit);
        }

        /// <summary>
        /// Update current user's binary record to point to an uploaded file
        /// </summary>
        /// <param name="userId">ID of the user who owns the binary record</param>
        /// <param name="key">Key of the binary record that will be updated</param>
        /// <param name="contentType">Content type of the uploaded file,
        /// taken from the callback after requesting a presigned upload URL using RequestUserBinaryRecordPresignedUrl</param>
        /// <param name="fileLocation">Location of the uploaded file, 
        /// taken from the callback after requesting a presigned upload URL using RequestUserBinaryRecordPresignedUrl</param>
        /// <param name="callback">Returns a Result that contains UserBinaryRecord via callback when completed</param>
        public void UpdateUserBinaryRecordFile(string key
            , string contentType
            , string fileLocation
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.UpdateUserBinaryRecordFile(key, contentType, fileLocation, callback);
        }

        /// <summary>
        /// Update current user's binary record's metadata
        /// </summary>
        /// <param name="userId">ID of the user who owns the binary record</param>
        /// <param name="key">Key of the binary record that will be updated</param>
        /// <param name="isPublic">Whether to update the binary record to become a public or private record</param>
        /// <param name="callback">Returns a Result that contains UserBinaryRecord via callback when completed</param>
        public void UpdateUserBinaryRecordMetadata(string key
            , bool isPublic
            , ResultCallback<UserBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.UpdateUserBinaryRecordMetadata(key, isPublic, callback);
        }

        /// <summary>
        /// Delete current user's binary record under the given key
        /// </summary>
        /// <param name="key">Key of the binary record that will be deleted</param``>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteUserBinaryRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.DeleteUserBinaryRecord(key, callback);
        }

        /// <summary>
        /// Request a presigned url to upload current user's binary record file
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="fileType">File type of the binary 
        /// (supported file types are jpeg, jpg, png, bmp, gif, mp3, webp, and bin)</param>
        /// <param name="callback">Returns a Result that contains BinaryInfo via callback when completed</param>
        public void RequestUserBinaryRecordPresignedUrl(string key
            , string fileType
            , ResultCallback<RequestUserBinaryRecordPresignedUrlResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.RequestUserBinaryRecordPresignedUrl(key, fileType, callback);
        }

        /// <summary>
        /// Get a game binary record by its key
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="callback">Returns a Result that contains GameBinaryRecord via callback when completed</param>
        public void GetGameBinaryRecord(string key
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.GetGameBinaryRecord(key, callback);
        }

        /// <summary>
        /// Bulk get game binary records by their keys
        /// </summary>
        /// <param name="keys">List of keys of the binary records</param>
        /// <param name="callback">Returns a Result that contains ListGameBinaryRecords via callback when completed</param>
        public void BulkGetGameBinaryRecords(string[] keys
            , ResultCallback<ListGameBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkGetGameBinaryRecords(keys, callback);
        }

        /// <summary>
        /// Bulk get game binary records using a query
        /// </summary>
        /// <param name="query">String that will be used to query the game's binary record keys</param>
        /// <param name="callback">Returns a Result that contains PaginatedGameBinaryRecords 
        /// via callback when completed</param>
        /// <param name="offset">The offset of the binary records. Default value is 0</param>
        /// <param name="limit">The limit of the binary records. Default value is 0</param>
        public void BulkQueryGameBinaryRecords(string query
            , ResultCallback<PaginatedGameBinaryRecords> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            binaryCloudSaveApi.BulkQueryGameBinaryRecords(query, callback, offset, limit);
        }
    }
}