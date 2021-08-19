// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class CurrenciesApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal CurrenciesApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetCurrencyList(string @namespace, string accessToken, ResultCallback<CurrencyList[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get Currency List! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get Currency List! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/currencies")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<CurrencyList[]>();

            callback.Try(result);
        }
    }
}
