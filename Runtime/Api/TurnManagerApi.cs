// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft;

namespace AccelByte.Api
{

    public class TurnManagerApi : AccelByte.Core.ApiBase
    {

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

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<TurnServerList>();
            callback.Try(result);
        }
    }

}