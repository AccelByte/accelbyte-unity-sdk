// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;

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

        public static AccelByteClientRegistry GetClientRegistry()
        {
            if (ClientRegistry == null)
            {
                ClientRegistry = CreateClientRegistry(Environment.Current);
            }
            return ClientRegistry;
        }

        public static Server.AccelByteServerRegistry GetServerRegistry()
        {
            if (ServerRegistry == null)
            {
                ServerRegistry = CreateServerRegistry(Environment.Current);
            }
            return ServerRegistry;
        }

        internal static AccelByteClientRegistry CreateClientRegistry(Models.SettingsEnvironment environment)
        {
            const bool getServerConfig = false;
            AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(environment, OverrideConfigs, getServerConfig);
            IHttpRequestSenderFactory httpRequestSenderFactory = SdkHttpSenderFactory;
            var newClientRegistry = new AccelByteClientRegistry(environment, settings.SDKConfig, settings.OAuthConfig, httpRequestSenderFactory);
            return newClientRegistry;
        }

        internal static Server.AccelByteServerRegistry CreateServerRegistry(Models.SettingsEnvironment environment)
        {
            const bool getServerConfig = true;
            AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(environment, OverrideConfigs, getServerConfig);
            IHttpRequestSenderFactory httpRequestSenderFactory = SdkHttpSenderFactory;
            var newClientRegistry = new Server.AccelByteServerRegistry(environment, settings.ServerSdkConfig, settings.OAuthConfig, httpRequestSenderFactory);
            return newClientRegistry;
        }

        internal static void ChangeInterfaceEnvironment(Models.SettingsEnvironment newEnvironment)
        {
            Models.Config clientConfig = null;
            Models.OAuthConfig clientOAuthConfig = null;
            Models.ServerConfig serverConfig = null;
            Models.OAuthConfig serverOAuthConfig = null;

            if (ClientRegistry != null)
            {
                const bool getServerConfig = false;
                AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(newEnvironment, OverrideConfigs, getServerConfig);
                clientConfig = settings.SDKConfig;
                clientOAuthConfig = settings.OAuthConfig;
            }

            if (ServerRegistry != null)
            {
                const bool getServerConfig = true;
                AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(newEnvironment, OverrideConfigs, getServerConfig);
                serverConfig = settings.ServerSdkConfig;
                serverOAuthConfig = settings.OAuthConfig;
            }

            ChangeInterfaceEnvironment(newEnvironment, clientConfig, clientOAuthConfig, serverConfig, serverOAuthConfig);
        }

        internal static void ChangeInterfaceEnvironment(Models.SettingsEnvironment newEnvironment, Models.Config clientConfig, Models.OAuthConfig clientOAuthConfig, Models.ServerConfig serverConfig, Models.OAuthConfig serverOAuthConfig)
        {
            if (ClientRegistry != null)
            {
                ClientRegistry.ChangeEnvironment(newEnvironment, clientConfig, clientOAuthConfig);
            }

            if (ServerRegistry != null)
            {
                ServerRegistry.ChangeEnvironment(newEnvironment, serverConfig, serverOAuthConfig);
            }
        }

        internal static void Reset()
        {
            if (ClientRegistry != null)
            {
                ClientRegistry.Reset();
                ClientRegistry = null;
            }

            if (ServerRegistry != null)
            {
                ServerRegistry.Reset();
                ServerRegistry = null;
            }

            if (sdkHttpSenderFactory != null)
            {
                sdkHttpSenderFactory.ResetHttpRequestSenders();
                sdkHttpSenderFactory = null;
            }
        }

        internal static AccelByteClientRegistry ClientRegistry;
        internal static Server.AccelByteServerRegistry ServerRegistry;
        internal static Models.OverrideConfigs OverrideConfigs;

        #region File Stream
        private static AccelByteFileStream fileStream;
        internal static AccelByteFileStream FileStream
        {
            get
            {
                if (fileStream == null)
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
        #endregion

        private static IHttpRequestSenderFactory sdkHttpSenderFactory;

        internal static IHttpRequestSenderFactory SdkHttpSenderFactory
        {
            get
            {
                if (sdkHttpSenderFactory == null)
                {
                    sdkHttpSenderFactory = new AccelByteSDKHttpRequestFactory();
                }
                return sdkHttpSenderFactory;
            }
            set
            {
                sdkHttpSenderFactory = value;
            }
        }

        private static AccelBytePlatformHandler platformHandler;

        public static AccelBytePlatformHandler PlatformHandler
        {
            get
            {
                if(platformHandler == null)
                {
                    platformHandler = new AccelBytePlatformHandler();
                }
                return platformHandler;
            }
        }
    }
}
