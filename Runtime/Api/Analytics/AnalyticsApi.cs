// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Api
{
    [Obsolete("Please use ClientGameTelemetryApi api, will be removed on AGS 3.81")]
    public class AnalyticsApi : ClientGameTelemetryApi
    {
        internal AnalyticsApi(IHttpClient inHttpClient, 
            Config inConfig, 
            ISession inSession) 
            : base(inHttpClient, inConfig, inSession)
        {
        }
    }
}