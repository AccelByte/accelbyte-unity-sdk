// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerMiscellaneous : WrapperBase, IServerMiscellaneousWrapper
    {
        private readonly ServerMiscellaneousApi api;
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession activeSession;

        [UnityEngine.Scripting.Preserve]
        internal ServerMiscellaneous( ServerMiscellaneousApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            api = inApi;
            coroutineRunner = inCoroutineRunner;
            activeSession = inSession;
        }

        public void GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            api.GetCurrentTime(
                (Result<Time> result) =>
                {
                    callback?.Try(result);
                });
        }

        public void GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.GetLanguages(callback);
        }
        
        public void GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.GetTimeZones(callback);
        }
        
        public void ListCountryGroups(ResultCallback<CountryGroup[]> callback)
        {
            ListCountryGroups(null, callback);
        }

        public void ListCountryGroups(string groupCode, ResultCallback<CountryGroup[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.ListCountryGroups(groupCode, callback);
        }
        
        public void AddCountryGroup(CountryGroup newCountryGroupData, ResultCallback<CountryGroup> callback = null)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.AddCountryGroup(newCountryGroupData, callback);
        }
        
        public void UpdateCountryGroup(string groupCode, CountryGroup newCountryGroupData, ResultCallback<CountryGroup> callback = null)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.UpdateCountryGroup(groupCode, newCountryGroupData, callback);
        }
        
        public void DeleteCountryGroup(string groupCode = null, ResultCallback callback = null)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!activeSession.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            api.DeleteCountryGroup(groupCode, callback);
        }
    }
}