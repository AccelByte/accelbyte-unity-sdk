// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using UnityEngine;

namespace AccelByte.Core
{
    public static class AccelByteSDK
    {
        public static AccelByteEnvironment Environment
        {
            internal set;
            get;
        }

        public static ClientAnalyticsService ClientAnalytics
        {
            internal set;
            get;
        }
        internal static Models.OverrideConfigs OverrideConfigs;
    }
}
