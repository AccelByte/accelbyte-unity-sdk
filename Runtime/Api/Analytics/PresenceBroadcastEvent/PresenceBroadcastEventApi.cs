// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace AccelByte.Api
{
    [Obsolete("Duplicated implementation. Please use ClientGameTelemetryApi to send telemetry data. Will be removed on AGS 3.81")]
    public class PresenceBroadcastEventApi : ApiBase
    {
        private string presenceBroadcastEventUrl;

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        public PresenceBroadcastEventApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.BaseUrl, session)
        {
            presenceBroadcastEventUrl = config.GameTelemetryServerUrl + "/v1/protected/events";
        }

        public async void SendPresenceBroadcastEvent(TelemetryBody pbeEvent
            , ResultCallback callback)
        {
            if (pbeEvent == null)
            {
                Assert.IsNotNull(pbeEvent, nameof(pbeEvent) + " is null.");
                callback.TryError(ErrorCode.InvalidArgument);
            }

            var request = HttpRequestBuilder
                .CreatePost(presenceBroadcastEventUrl)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(pbeEvent.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpSendResult sendResult = await HttpClient.SendRequestAsync(request);
            IHttpResponse response = sendResult.CallbackResponse;
            var result = response.TryParse();
            callback.Try(result);
        }

        public async void SendPresenceBroadcastEvent(List<TelemetryBody> pbeEvents
            , ResultCallback callback)
        {
            if (pbeEvents.Count < 1)
            {
                Assert.IsNotNull(pbeEvents, nameof(pbeEvents) + " is null.");
                callback.TryError(ErrorCode.InvalidArgument);
            }

            var request = HttpRequestBuilder
                .CreatePost(presenceBroadcastEventUrl)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(pbeEvents.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpSendResult sendResult = await HttpClient.SendRequestAsync(request);
            IHttpResponse response = sendResult.CallbackResponse;
            var result = response.TryParse();
            callback.Try(result);
        }
    }
}
