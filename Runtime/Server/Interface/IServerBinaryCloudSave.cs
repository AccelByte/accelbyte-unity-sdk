// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Server.Interface
{
    public interface IServerBinaryCloudSave
    {
        /// <summary>
        /// Create game binary records.
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="fileType">File type of the binary</param>
        /// <param name="setBy">Metadata value</param>
        /// <param name="ttlConfig">
        /// The configuration to control the action taken if the record has expired.
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contains BinaryInfo via callback when completed.
        /// </param>
        public void CreateGameBinaryRecord(string key
            , FileType fileType
            , RecordSetBy setBy
            , TTLConfig ttlConfig
            , ResultCallback<BinaryInfo> callback);

        /// <summary>
        /// Delete a game binary record.
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeleteGameBinaryRecord(string key
            , ResultCallback callback);

        /// <summary>
        /// Get a game binary record by its key.
        /// </summary>
        /// <param name=key"> Key Key of the binary record</param>
        /// <param name=callback">
        /// Returns a Result that contains BinaryInfo via callback when completed.
        /// </param>
        public void GetGameBinaryRecord(string key
            , ResultCallback<GameBinaryRecord> callback);

        /// <summary>
        /// Bulk query game binary records.
        /// </summary>
        /// <param name="query">Query string that will be used to query the game's binary record keys</param>
        /// <param name="tags">Filter list by tags, max 5 tags per request</param>
        /// <param name="offset">Offset of the result</param>
        /// <param name="limit">Limit of the result</param>
        /// <param name=callback">
        /// Returns a Result that contains Paginated GameBinaryRecords via callback when completed.
        /// </param>
        public void QueryGameBinaryRecords(string query
            , ICollection<string> tags
            , int offset
            , int limit
            , ResultCallback<PaginatedGameBinaryRecords> callback);

        /// <summary>
        /// Request presigned URL to upload the binary record to s3.
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="fileType">Type of the binary</param>
        /// <param name=callback">
        /// Returns a Result that contains BinaryInfo via callback when completed.
        /// </param>
        public void RequestGameBinaryRecordPresignedUrl(string key
            , FileType fileType
            , ResultCallback<BinaryInfo> callback);

        /// <summary>
        /// Update a game binary record file by its key.
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="contentType">The specific type of the binary record created</param>
        /// <param name="fileLocation">Location of the uploaded binary file</param>
        /// <param name="callback">
        /// Returns a Result that contains updated GameBinaryRecords via callback when completed.
        /// </param>
        public void UpdateGameBinaryRecord(string key
            , FileType contentType
            , string fileLocation
            , ResultCallback<GameBinaryRecord> callback);

        /// <summary>
        /// Update a game binary record metadata by its key.
        /// </summary>
        /// <param name="key">Key of the binary record</param>
        /// <param name="setBy">The specific type of the binary record created</param>
        /// <param name="tags">Location of the uploaded binary file</param>
        /// <param name="ttlConfig">
        /// The configuration to control the action taken if the record has expired.
        /// </param>
        /// <param name="callback">
        /// Returns a Result that contains updated GameBinaryRecords via callback when completed.
        /// </param>
        public void UpdateGameBinaryRecordMetadata(string key
            , RecordSetBy setBy
            , ICollection<string> tags
            , TTLConfig ttlConfig
            , ResultCallback<GameBinaryRecord> callback);
    }
}