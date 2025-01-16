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

        string EventName
        {
            get;
        }
    }
}
