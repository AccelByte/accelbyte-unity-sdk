// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Server
{
    internal class ServerAnalyticsApi : ServerApiBase
    {
        private string gameTelemetryUrl;

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==GameTelemetryServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerAnalyticsApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.GameTelemetryServerUrl, session)
        {
            gameTelemetryUrl = config.GameTelemetryServerUrl + "/v1/protected/events";
        }

        public async void SendPredefinedEvent(List<TelemetryBody> predefinedEvents
            , ResultCallback callback)
        {
            if (predefinedEvents == null)
            {
                AccelByteDebug.LogError(nameof(predefinedEvents) + " is null.");
                callback.TryError(ErrorCode.InvalidArgument);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(gameTelemetryUrl)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(predefinedEvents.ToUtf8Json())
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