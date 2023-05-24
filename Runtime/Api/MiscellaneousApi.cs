// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api 
{
    internal class MiscellaneousApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BasicServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal MiscellaneousApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BasicServerUrl, session )
        {
        }

        #region Public Methods

        public IEnumerator GetCurrentTime( ResultCallback<Time> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/misc/time")
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Time>();
            callback.Try(result);
        }
        
        public IEnumerator GetCountryGroups( ResultCallback<Country[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/misc/countries")
                .WithPathParam("namespace", Namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Country[]>();
            callback.Try(result);
        }

        public IEnumerator GetLanguages( ResultCallback<Dictionary<string,string>> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/misc/languages")
                .WithPathParam("namespace", Namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Dictionary<string,string>>();
            callback.Try(result);
        }

        public IEnumerator GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", Namespace_)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<string[]>();
            callback.Try(result);
        }

        #endregion
    }
}
