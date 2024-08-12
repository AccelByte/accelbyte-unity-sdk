// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Diagnostics;

namespace AccelByte.Core
{
    internal static class TimeManagerBootstrap
    {
        internal static void Execute()
        {
        }

        public static void Stop()
        {
            if (AccelByteSDK.GlobalTimeManager != null)
            {
                AccelByteSDK.GlobalTimeManager.StopGameSession();
            }
        }
    }
}
