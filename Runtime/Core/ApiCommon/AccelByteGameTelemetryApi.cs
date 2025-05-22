// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal static class AccelByteGameTelemetryApi
    {
        public static void SendProtectedEventV1(List<TelemetryBody> telemetryBodies, HttpOperator httpOperator, string gameTelemetryServerUrl, string authToken, ResultCallback callback)
        {
            if (telemetryBodies == null || telemetryBodies.Count == 0)
            {
                callback?.TryError(ErrorCode.InvalidArgument, errorMessage: $"{nameof(telemetryBodies)} is null or empty.");
                return;
            }
            
            string url = gameTelemetryServerUrl + "/v1/protected/events";
            
            var request = HttpRequestBuilder
                .CreatePost(url)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(telemetryBodies.ToUtf8Json())
                .WithBearerAuth(authToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            TelemetryBody telemetryBodyWithLogger = telemetryBodies.Find(telemetryBody => telemetryBody.Logger != null);

            AdditionalHttpParameters additionalHttpParameters = new AdditionalHttpParameters()
            {
                Logger = telemetryBodyWithLogger?.Logger
            };

            httpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public static void TryAssignTelemetryBodyClientTimestamps(ref TelemetryBody telemetryBody,
            ref AccelByteTimeManager timeManager)
        {
            if (telemetryBody != null && timeManager != null)
            {
                if (telemetryBody.CreatedElapsedTime == null)
                {
                    telemetryBody.SetTimeReference(timeManager.TimelapseSinceSessionStart.Elapsed);   
                }

                if (!telemetryBody.IsTimeStampSet && timeManager.GetCachedServerTime() != null)
                {
                    telemetryBody.ClientTimestamp = telemetryBody.CalculateClientTimestampFromServerTime(timeManager.GetCachedServerTime());
                }
            }
        }
    }
}