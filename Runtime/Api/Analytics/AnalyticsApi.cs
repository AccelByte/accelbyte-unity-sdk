// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Api
{
    public class AnalyticsApi : ApiBase
    {
        private readonly string gameTelemetryUrl;

        internal AnalyticsApi(IHttpClient inHttpClient, 
            Config inConfig, 
            ISession inSession) 
            : base(inHttpClient, inConfig, inConfig.GameTelemetryServerUrl, inSession)
        {
            gameTelemetryUrl = inConfig.GameTelemetryServerUrl + "/v1/protected/events";
        }

        public async void SendData(List<TelemetryBody> telemetryBodies
            , ResultCallback callback)
        {
            if (telemetryBodies == null)
            {
                AccelByteDebug.LogError(nameof(telemetryBodies) + " is null.");
                callback.TryError(ErrorCode.InvalidArgument);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(gameTelemetryUrl)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(telemetryBodies.ToUtf8Json())
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