// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class ClientGameTelemetryApi : ApiBase
    {
        internal ClientGameTelemetryApi(IHttpClient inHttpClient, 
            Config inConfig, 
            ISession inSession) 
            : base(inHttpClient, inConfig, inConfig.GameTelemetryServerUrl, inSession)
        {
        }

        public void SendData(List<TelemetryBody> telemetryBodies
            , ResultCallback callback)
        {
            AccelByteGameTelemetryApi.SendProtectedEventV1(telemetryBodies,httpOperator, BaseUrl, AuthToken, callback);
        }

        internal void FetchServerTime(ref AccelByteTimeManager timeManager)
        {
            Assert.IsNotNull(timeManager, "Time manager can't be null");
            timeManager.FetchServerTime(httpOperator, Config.Namespace, Config.BasicServerUrl);
        }
    }
}