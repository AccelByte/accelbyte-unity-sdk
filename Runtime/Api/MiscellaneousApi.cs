// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api {
    internal class MiscellaneousApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;
        private readonly string @namespace;

        internal MiscellaneousApi(string @namespace ,string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + " is null");
            Assert.IsNotNull(httpClient, nameof(httpClient) + " is null");
            Assert.IsNotNull(@namespace, nameof(@namespace) + "Namespace parameter is null!");

            this.@namespace = @namespace;
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GetCurrentTime(ResultCallback<Time> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/misc/time")
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Time>();
            callback.Try(result);
        }
        
        public IEnumerator GetCountryGroups(ResultCallback<Country[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/misc/countries")
                .WithPathParam("namespace", @namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Country[]>();
            callback.Try(result);
        }
        public IEnumerator GetLanguages(ResultCallback<Dictionary<string,string>> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/misc/languages")
                .WithPathParam("namespace", @namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<Dictionary<string,string>>();
            callback.Try(result);
        }
        public IEnumerator GetTimeZones(ResultCallback<string[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", @namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<string[]>();
            callback.Try(result);
        }
    }
}