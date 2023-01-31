// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class ServiceVersionApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param>
        /// <param name="session"></param>
        internal ServiceVersionApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.BaseUrl, session)
        {
        }

        #region Methods
        public IEnumerator GetVersionInfo(string serviceName
            , ResultCallback<ServiceVersionInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(serviceName, "Can't get service's version info! service name parameter is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + $"/{serviceName}/version")
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<ServiceVersionInfo>();
            callback.Try(result);
        }
        #endregion
    }
}
