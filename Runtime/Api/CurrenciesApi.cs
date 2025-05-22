// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;

namespace AccelByte.Api
{
    internal class CurrenciesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal CurrenciesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        public IEnumerator GetCurrencyList( ResultCallback<CurrencyList[]> callback, CurrencyType currencyType = CurrencyType.NONE )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetCurrencyListOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                CurrencyType = currencyType
            };

            GetCurrencyList(optionalParameters, callback);

            yield return null;
        }

        internal void GetCurrencyList(GetCurrencyListOptionalParameters optionalParameters, ResultCallback<CurrencyList[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/currencies")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);
            if (optionalParameters?.CurrencyType != null && optionalParameters?.CurrencyType != CurrencyType.NONE)
            {
                builder.WithQueryParam("currencyType", optionalParameters.CurrencyType.ToString());
            }

            var request = builder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<CurrencyList[]>();
                callback.Try(result);
            });
        }
    }
}
