// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerGameTelemetryApi : ServerApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ServerGameTelemetryApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.GameTelemetryServerUrl, session)
        {
        }

        public void SendData(List<TelemetryBody> telemetryBodies
            , ResultCallback callback)
        {
            AccelByteGameTelemetryApi.SendProtectedEventV1(telemetryBodies, HttpOperator, BaseUrl, AuthToken, callback);
        }

        internal void FetchServerTime(ref AccelByteTimeManager timeManager)
        {
            Assert.IsNotNull(timeManager, "Time manager can't be null");
            timeManager.FetchServerTime(HttpOperator, ServerConfig.Namespace, ServerConfig.BasicServerUrl);
        }
    }
}