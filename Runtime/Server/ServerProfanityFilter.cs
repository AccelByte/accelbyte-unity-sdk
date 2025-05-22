// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;
using UnityEditor;

namespace AccelByte.Server
{
    public class ServerProfanityFilter : WrapperBase, IServerProfanityFilter
    {
        private readonly ServerProfanityFilterApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerProfanityFilter(ServerProfanityFilterApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(inApi, "Cannot construct ProfanityFilter manager, api is null!");

            api = inApi;
            session = inSession;
        }

        public void BulkCreateProfanityWords(CreateProfanityWordRequest[] bulkCreateRequest, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);
            
            BulkCreateProfanityWords(bulkCreateRequest, null, callback);
        }

        internal void BulkCreateProfanityWords(
            CreateProfanityWordRequest[] bulkCreateRequest
            , BulkCreateProfanityWordsOptionalParameters optionalParams
            , ResultCallback callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkCreateProfanityWords(bulkCreateRequest, optionalParams, callback);
        }

        public void CreateProfanityWord(string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            CreateProfanityWord(word, falseNegatives, falsePositives, null, callback);
        }

        internal void CreateProfanityWord(
            string word
            , string[] falseNegatives
            , string[] falsePositives
            , CreateProfanityWordOptionalParameters optionalParams
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (falseNegatives == null)
            {
                falseNegatives = new string[0];
            }
            if (falsePositives == null)
            {
                falsePositives = new string[0];
            }

            api.CreateProfanityWord(word, falseNegatives, falsePositives, optionalParams, callback);
        }

        public void DeleteProfanityWord(string id, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);
            
            DeleteProfanityWord(id, null, callback);
        }

        internal void DeleteProfanityWord(string id, DeleteProfanityWordOptionalParameters optionalParams, ResultCallback callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteProfanityWord(id, optionalParams, callback);
        }

        public void GetProfanityWordGroups(ResultCallback<ProfanityWordGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            GetProfanityWordGroups(null, callback);
        }

        public void GetProfanityWordGroups(GetProfanityWordGroupsOptionalParameters optionalParameters
            , ResultCallback<ProfanityWordGroupResponse> callback)
        {
            IDebugger activeLogger = optionalParameters != null && optionalParameters.Logger != null ? optionalParameters.Logger : SharedMemory?.Logger;
            Report.GetFunctionLog(this.GetType().Name, logger: activeLogger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetProfanityWordGroups(optionalParameters, callback);
        }

        public void QueryProfanityWords(ResultCallback<QueryProfanityWordsResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            QueryProfanityWords(null, callback);
        }

        public void QueryProfanityWords(QueryProfanityWordsOptionalParameters optionalParams
            , ResultCallback<QueryProfanityWordsResponse> callback)
        {
            IDebugger activeLogger = optionalParams != null && optionalParams.Logger != null ? optionalParams.Logger : SharedMemory?.Logger;
            Report.GetFunctionLog(this.GetType().Name, logger: activeLogger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.QueryProfanityWords(optionalParams, callback);
        }

        public void UpdateProfanityWord(string id
            , string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            UpdateProfanityWord(id, word, falseNegatives, falsePositives, null, callback);
        }

        internal void UpdateProfanityWord(
            string id
            , string word
            , string[] falseNegatives
            , string[] falsePositives
            , UpdateProfanityWordOptionalParameters optionalParams
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateProfanityWord(id, word, falseNegatives, falsePositives, optionalParams, callback);
        }
    }
}