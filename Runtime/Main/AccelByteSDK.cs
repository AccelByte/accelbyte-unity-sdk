// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;

namespace AccelByte.Core
{
    public static class AccelByteSDK
    {
        private static AccelByteSDKImplementator implementation;

        internal static AccelByteSDKImplementator Implementation
        {
            get
            {
                if (implementation == null)
                {
                    implementation = new AccelByteSDKImplementator();
                }
                return implementation;
            }
            set
            {
                implementation = value;
            }
        }
        
        public static AccelByteEnvironment Environment
        {
            get
            {
                return Implementation.Environment;
            }
        }

        public static ClientAnalyticsService ClientAnalytics
        {
            get
            {
                return Implementation.ClientAnalytics;
            }
        }

        public static string FlightId
        {
            get
            {
                return AccelByteSDKMain.FlightId;
            }
        }

        public static string Version
        {
            get
            {
                return Implementation.Version;
            }
        }

        public static AccelByteClientRegistry GetClientRegistry()
        {
            return Implementation.GetClientRegistry();
        }

        public static Server.AccelByteServerRegistry GetServerRegistry()
        {
            return Implementation.GetServerRegistry();
        }

        /// <summary>
        /// Get SDK config on client side
        /// </summary>
        public static Models.Config GetClientConfig()
        {
            return Implementation.GetClientConfig();
        }

        /// <summary>
        /// Get oauth config on client side
        /// </summary>
        public static Models.OAuthConfig GetClientOAuthConfig()
        {
            return Implementation.GetClientOAuthConfig();
        }

        /// <summary>
        /// Get SDK config on server side
        /// </summary>
        public static Models.ServerConfig GetServerConfig()
        {
            return Implementation.GetServerConfig();
        }

        /// <summary>
        /// Get oauth config on server side
        /// </summary>
        public static Models.OAuthConfig GetServerOAuthConfig()
        {
            return Implementation.GetServerOAuthConfig();
        }
        
        internal static IFileStream FileStream
        {
            get
            {
                return Implementation.FileStream;
            }
        }

        public static IFileStreamFactory FileStreamFactory
        {
            get
            {
                return Implementation.FileStreamFactory;
            }
            set
            {
                Implementation.FileStreamFactory = value;
            }
        }
        
        public static AccelBytePlatformHandler PlatformHandler
        {
            get
            {
                return Implementation.PlatformHandler;
            }
        }

        public static AccelByteTimeManager TimeManager
        {
            get
            {
                return Implementation.TimeManager;
            }
        }
    }
}
