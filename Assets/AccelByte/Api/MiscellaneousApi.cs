// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api {
    internal class MiscellaneousApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal MiscellaneousApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + " is null");
            Assert.IsNotNull(httpClient, nameof(httpClient) + " is null");

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
    }
}