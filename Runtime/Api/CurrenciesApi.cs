// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class CurrenciesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        internal CurrenciesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator GetCurrencyList( ResultCallback<CurrencyList[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Get Currency List! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Get Currency List! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/currencies")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<CurrencyList[]>();
            callback.Try(result);
        }

    }
}
