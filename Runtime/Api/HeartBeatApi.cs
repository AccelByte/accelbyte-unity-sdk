// Copyright (c) 2022-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class HeartBeatApi : ApiBase
    {
        public const string HeartBeatUrl = "https://heartbeat.accelbyte.io/add";
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

        public async void SendHeartBeatEvent(Dictionary<string, object> heartBeatEvent
            , ResultCallback callback)
        {
            Assert.IsNotNull(heartBeatEvent, nameof(heartBeatEvent) + " is null.");

            var request = HttpRequestBuilder
                .CreatePost(HeartBeatUrl)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(heartBeatEvent.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpSendResult sendResult = await HttpClient.SendRequestAsync(request);
            IHttpResponse response = sendResult.CallbackResponse;
            var result = response.TryParse();
            callback.Try(result);
        }
    }
}