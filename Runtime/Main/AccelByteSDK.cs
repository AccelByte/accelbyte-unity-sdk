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
		
        public static string FlightId
        {
            internal set;
            get;
        }

        public static string Version
        {
            get
            {
                return AccelByteSettingsV2.AccelByteSDKVersion;
            }
        }
		
        internal static Models.OverrideConfigs OverrideConfigs;
        private static AccelByteFileStream fileStream;
        internal static AccelByteFileStream FileStream
        {
            get
            {
                if(fileStream == null)
                {
                    fileStream = new AccelByteFileStream();
                    AccelByteSDKMain.OnGameUpdate += dt =>
                    {
                        fileStream.Pop();
                    };
                }
                return fileStream;
            }
        }
    }
}
