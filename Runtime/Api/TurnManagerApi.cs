// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{

    public class TurnManagerApi : AccelByte.Core.ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        public TurnManagerApi(IHttpClient inHttpClient, Config inConfig, ISession inSession)
            : base(inHttpClient, inConfig, inConfig.TurnManagerServerUrl, inSession)
        {
        }

        public IEnumerator GetTurnServers(ResultCallback<TurnServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/turn")
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<TurnServerList>();
            callback.Try(result);
        }

        public IEnumerator GetTurnServerCredential(string region
            , string ip
            , int port
            , ResultCallback<TurnServerCredential> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(region, "Can't Get Turn Server Credential! region parameter is null");
            Assert.IsNotNull(ip, "Can't Get Turn Server Credential! ip parameter is null");
            Assert.IsTrue(port > 0 && port < 65536,
                "Can't Get Turn Server Credential! port is not within 1-65535");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/turn/secret/{region}/{ip}/{port}")
                .WithPathParam("region", region)
                .WithPathParam("ip", ip)
                .WithPathParam("port", port.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<TurnServerCredential>();
            callback.Try(result);
        }
    }

}