// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Api 
{
    internal class ClientMiscellaneousApi : ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ClientMiscellaneousApi( IHttpClient httpClient
            , Config config
            , ISession session) 
            : base( httpClient, config, config.BasicServerUrl, session )
        {
        }
        
        [UnityEngine.Scripting.Preserve]
        internal ClientMiscellaneousApi( IHttpClient httpClient
            , Config config
            , ISession session
            , HttpOperator httpOperator) 
            : base( httpClient, config, config.BasicServerUrl, session, httpOperator)
        {
        }

        public void GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetCurrentTime(
                httpOperator: httpOperator
                , @namespace: Namespace_
                , baseUrl: BaseUrl
                , callback);
        }
        
        public void GetCountryGroups( ResultCallback<Country[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetCountryGroups(
                httpOperator: httpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: false
                , authToken: null
                , callback);
        }

        public void GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetLanguages(
                httpOperator: httpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: false
                , authToken: null
                , callback);
        }

        public void GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            AccelByteMiscellaneousApi.GetTimeZones(
                httpOperator: httpOperator
                , baseUrl: BaseUrl
                , @namespace: Namespace_
                , adminEndpoint: false
                , authToken: null
                , callback);
        }
    }
}
