// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class QosManagerApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal QosManagerApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + "is null");
            Assert.IsNotNull(httpWorker, nameof(httpWorker) + "is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetQosServers(ResultCallback<QosServerList> callback)
        {
            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/qos")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);
            
            var result = response.TryParseJson<QosServerList>();
            callback.Try(result);
        }
    }
}