// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public class ServerProfanityFilter : WrapperBase, Interface.IServerProfanityFilter
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
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkCreateProfanityWords(bulkCreateRequest, callback);
        }

        public void CreateProfanityWord(string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

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

            api.CreateProfanityWord(word, falseNegatives, falsePositives, callback);
        }

        public void DeleteProfanityWord(string id, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.DeleteProfanityWord(id, callback);
        }

        public void GetProfanityWordGroups(ResultCallback<ProfanityWordGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParams = new GetProfanityWordGroupsOptionalParameters();

            GetProfanityWordGroups(optionalParams, callback);
        }

        public void GetProfanityWordGroups(GetProfanityWordGroupsOptionalParameters optionalParameters
            , ResultCallback<ProfanityWordGroupResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

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

            var optionalParams = new QueryProfanityWordsOptionalParameters();

            QueryProfanityWords(optionalParams, callback);
        }

        public void QueryProfanityWords(QueryProfanityWordsOptionalParameters optionalParams
            , ResultCallback<QueryProfanityWordsResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name, logger: SharedMemory?.Logger);

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

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateProfanityWord(id, word, falseNegatives, falsePositives, callback);
        }
    }
}