// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;

namespace AccelByte.Core
{
    public interface IAccelByteTelemetryEvent
    {
        Models.IAccelByteTelemetryPayload Payload
        {
            get;
        }

        [Obsolete("SDK will assign time stamps on telemetry Send(). Field will be removed on 3.80 release.")]
        System.DateTime CreatedTimestamp
        {
            get;
        }

        string EventName
        {
            get;
        }
    }
}
