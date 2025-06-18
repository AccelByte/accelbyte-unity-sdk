// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;

namespace AccelByte.Core
{
    internal class AccelByteSDKImplementator
    {
        public AccelByteEnvironment Environment
        {
            internal set;
            get;
        }

        public ClientAnalyticsService ClientAnalytics
        {
            get
            {
                if (clientAnalyticsInstance == null)
                {
                    clientAnalyticsInstance = new ClientAnalyticsService(SdkHttpSenderFactory, CoreHeartBeat, TimeManager, GetClientConfig(), Environment.Current);
                    clientAnalyticsInstance.StartAcceptEvent();
                }

                return clientAnalyticsInstance;
            }
        }

        private ClientAnalyticsService clientAnalyticsInstance;

        public string FlightId
        {
            get
            {
                return AccelByteSDKMain.FlightId;
            }
        }

        public string Version
        {
            get
            {
                return AccelByteSettingsV2.AccelByteSDKVersion;
            }
        }

        internal readonly Utils.AccelByteServiceTracker ServiceTracker;
        
        internal System.Action<IHttpClient> OnHttpClientCreated;
        internal Utils.SingularCallEvent<AccelByteClientRegistry> OnClientRegistryCreated;
        internal Utils.SingularCallEvent<Server.AccelByteServerRegistry> OnServerRegistryCreated;

        public AccelByteSDKImplementator()
        {
            Environment = new AccelByteEnvironment();
            Environment.OnEnvironmentChanged += ChangeInterfaceEnvironment;
            
            ServiceTracker = new Utils.AccelByteServiceTracker();
            var serviceLogger = new Utils.AccelByteServiceLogger();
            
            ServiceTracker.OnNewRequestSentEvent += serviceLogger.LogServiceActivity;
            ServiceTracker.OnNewResponseReceivedEvent += serviceLogger.LogServiceActivity;
            ServiceTracker.OnSendingWebsocketRequestEvent += serviceLogger.LogServiceActivity;
            ServiceTracker.OnReceivingWebsocketNotificationEvent += serviceLogger.LogServiceActivity;
        }

        public AccelByteClientRegistry GetClientRegistry()
        {
            if (ClientRegistry == null)
            {
                ClientRegistry = CreateClientRegistry(Environment.Current);
                OnClientRegistryCreated.Invoke(ClientRegistry);
            }
            return ClientRegistry;
        }

        public Server.AccelByteServerRegistry GetServerRegistry()
        {
            if (ServerRegistry == null)
            {
                ServerRegistry = CreateServerRegistry(Environment.Current);
                OnServerRegistryCreated.Invoke(ServerRegistry);
            }
            return ServerRegistry;
        }

        /// <summary>
        /// Get SDK config on client side
        /// </summary>
        public Models.Config GetClientConfig()
        {
            Models.Config retval = GetClientRegistry().Config;
            retval = retval.ShallowCopy();
            return retval;
        }

        /// <summary>
        /// Get oauth config on client side
        /// </summary>
        public Models.OAuthConfig GetClientOAuthConfig()
        {
            Models.OAuthConfig retval = GetClientRegistry().OAuthConfig;
            retval = retval.ShallowCopy();
            return retval;
        }

        /// <summary>
        /// Get SDK config on server side
        /// </summary>
        public Models.ServerConfig GetServerConfig()
        {
            Models.ServerConfig retval = GetServerRegistry().Config;
            retval = retval.ShallowCopy();
            return retval;
        }

        /// <summary>
        /// Get oauth config on server side
        /// </summary>
        public Models.OAuthConfig GetServerOAuthConfig()
        {
            Models.OAuthConfig retval = GetServerRegistry().OAuthConfig;
            retval = retval.ShallowCopy();
            return retval;
        }

        internal AccelByteClientRegistry CreateClientRegistry(Models.SettingsEnvironment environment)
        {
            const bool getServerConfig = false;
            AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(environment, OverrideConfigs, getServerConfig);
            IHttpRequestSenderFactory httpRequestSenderFactory = SdkHttpSenderFactory;
            var newClientRegistry = new AccelByteClientRegistry(
                environment, 
                settings.SDKConfig, 
                settings.OAuthConfig, 
                httpRequestSenderFactory, 
                TimeManager, 
                CoreHeartBeat, 
                FileStream, 
                ServiceTracker);
            
            return newClientRegistry;
        }

        internal Server.AccelByteServerRegistry CreateServerRegistry(Models.SettingsEnvironment environment)
        {
            const bool getServerConfig = true;
            AccelByteSettingsV2 settings = AccelByteSettingsV2.GetSettingsByEnv(environment, OverrideConfigs, getServerConfig);
            IHttpRequestSenderFactory httpRequestSenderFactory = SdkHttpSenderFactory;
            var newServerRegistry = new Server.AccelByteServerRegistry(
                environment
                , settings.ServerSdkConfig
                , settings.OAuthConfig
                , httpRequestSenderFactory
                , TimeManager
                , CoreHeartBeat
                , FileStream
                , ServiceTracker);
            
            return newServerRegistry;
        }

        internal void ChangeInterfaceEnvironment(Models.SettingsEnvironment newEnvironment)
        {
            Models.Config clientConfig = null;
            Models.OAuthConfig clientOAuthConfig = null;
            Models.ServerConfig serverConfig = null;
            Models.OAuthConfig serverOAuthConfig = null;

            if (ClientRegistry != null || clientAnalyticsInstance != null)
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

        internal void ChangeInterfaceEnvironment(Models.SettingsEnvironment newEnvironment, Models.Config clientConfig, Models.OAuthConfig clientOAuthConfig, Models.ServerConfig serverConfig, Models.OAuthConfig serverOAuthConfig)
        {
            if (ClientRegistry != null)
            {
                ClientRegistry.ChangeEnvironment(newEnvironment, clientConfig, clientOAuthConfig);
            }

            if (ServerRegistry != null)
            {
                ServerRegistry.ChangeEnvironment(newEnvironment, serverConfig, serverOAuthConfig);
            }

            if (clientAnalyticsInstance != null)
            {
                clientAnalyticsInstance.StopAcceptEvent();
                clientAnalyticsInstance.DisposeScheduler();
                
                clientAnalyticsInstance = new ClientAnalyticsService(SdkHttpSenderFactory, CoreHeartBeat, TimeManager, clientConfig, newEnvironment);
                clientAnalyticsInstance.StartAcceptEvent();
            }
        }

        internal void Reset()
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

            if (clientAnalyticsInstance != null)
            {
                clientAnalyticsInstance.StopAcceptEvent();
                clientAnalyticsInstance.DisposeScheduler();
                clientAnalyticsInstance = null;
            }

            if (sdkHttpSenderFactory != null)
            {
                sdkHttpSenderFactory.ResetHttpRequestSenders();
                sdkHttpSenderFactory = null;
            }

            if (coreHeartBeat != null)
            {
                coreHeartBeat.Reset();
                coreHeartBeat = null;
            }

            AccelByteDebug.Reset();
        }

        internal void DisposeFileStream()
        {
            if(fileStream != null)
            {
                fileStream.Dispose();
            }
        }

        internal AccelByteClientRegistry ClientRegistry;
        internal Server.AccelByteServerRegistry ServerRegistry;
        internal Models.OverrideConfigs OverrideConfigs;

        #region File Stream
        private IFileStream fileStream;
        internal IFileStream FileStream
        {
            get
            {
                if (fileStream == null)
                {
                    fileStream = CreateFileStream();
                }
                return fileStream;
            }
        }
        public IFileStreamFactory FileStreamFactory;

        internal IFileStream CreateFileStream()
        {
            IFileStreamFactory fileStreamFactory = FileStreamFactory;
            if(fileStreamFactory == null)
            {
                fileStreamFactory = new AccelByteFileStreamFactory();
            }
            var createdFileStream = fileStreamFactory.CreateFileStream();
            AccelByteSDKMain.OnGameUpdate += dt =>
            {
                createdFileStream.Pop();
            };
            return createdFileStream;
        }
        #endregion

        private IHttpRequestSenderFactory sdkHttpSenderFactory;

        internal IHttpRequestSenderFactory SdkHttpSenderFactory
        {
            get
            {
                if (sdkHttpSenderFactory == null)
                {
                    var defaultHttpSender = new AccelByteSDKHttpRequestFactory(CoreHeartBeat);
                    defaultHttpSender.OnWebRequestSchedulerCreated = ServiceTracker.OnNewWebRequestSchedulerCreated;
                    sdkHttpSenderFactory = defaultHttpSender;
                }
                return sdkHttpSenderFactory;
            }
            set
            {
                sdkHttpSenderFactory = value;
            }
        }

        private AccelBytePlatformHandler platformHandler;

        public AccelBytePlatformHandler PlatformHandler
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

        public AccelByteTimeManager TimeManager
        {
            get
            {
                if (GlobalTimeManager == null)
                {
                    GlobalTimeManager = new AccelByteTimeManager();
                }

                return GlobalTimeManager;
            }
            internal set
            {
                GlobalTimeManager = value;
            }
        }
        
        internal AccelByteTimeManager GlobalTimeManager;

        internal CoreHeartBeat CoreHeartBeat
        {
            get
            {
                if (coreHeartBeat == null)
                {
                    coreHeartBeat = new CoreHeartBeat();
                }

                return coreHeartBeat;
            }
        }
        private CoreHeartBeat coreHeartBeat;
    }
}
