// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

#if !UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class HeartBeatApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="session"></param>
        public HeartBeatApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.BaseUrl, session)
        {
        }

        public IEnumerator SendHeartBeatEvent(Dictionary<string, object> heartBeatEvent
            , ResultCallback callback)
        {
            Assert.IsNotNull(heartBeatEvent, nameof(heartBeatEvent) + " is null.");

            var request = HttpRequestBuilder
                .CreatePost("https://heartbeat.accelbyte.io/add")
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(heartBeatEvent.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

    }
}
#endif