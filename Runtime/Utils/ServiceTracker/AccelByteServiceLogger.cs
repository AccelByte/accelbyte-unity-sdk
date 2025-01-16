// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Utils
{
    internal class AccelByteServiceLogger
    {
        public void LogServiceActivity(ServiceLog log, IDebugger logger)
        {
            logger?.LogEnhancedService($"LogAccelByteServiceLoggingEvent: <--{log.ToJsonString()}-->");
        }
    }
}