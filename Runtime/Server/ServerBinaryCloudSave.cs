// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using AccelByte.Server.Interface;
using System.Collections.Generic;

namespace AccelByte.Server
{
    public class ServerBinaryCloudSave : WrapperBase, IServerBinaryCloudSave
    {
        private readonly ServerBinaryCloudSaveApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerBinaryCloudSave(ServerBinaryCloudSaveApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct CloudSave manager; api is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct CloudSave manager; coroutineRunner is null!");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        public void CreateGameBinaryRecord(string key
            , FileType fileType
            , RecordSetBy setBy
            , TTLConfig ttlConfig
            , ResultCallback<BinaryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateGameBinaryRecord(key
                , fileType
                , setBy
                , ttlConfig
                , callback);
        }

        public void DeleteGameBinaryRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteGameBinaryRecord(key, callback);
        }

        public void GetGameBinaryRecord(string key
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetGameBinaryRecord(key, callback);
        }

        public void QueryGameBinaryRecords(string query
            , ICollection<string> tags
            , int offset
            , int limit
            , ResultCallback<PaginatedGameBinaryRecords> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryGameBinaryRecords(query, tags, offset , limit , callback);
        }

        public void RequestGameBinaryRecordPresignedUrl(string key
            , FileType fileType
            , ResultCallback<BinaryInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RequestGameBinaryRecordPresignedUrl(key, fileType , callback);
        }

        public void UpdateGameBinaryRecord(string key
            , FileType contentType
            , string fileLocation
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateGameBinaryRecord(key, contentType , fileLocation , callback);
        }

        public void UpdateGameBinaryRecordMetadata(string key
            , RecordSetBy setBy
            , ICollection<string> tags
            , TTLConfig config
            , ResultCallback<GameBinaryRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateGameBinaryRecordMetadata(key, setBy , tags , config , callback);
        }
    }
}
