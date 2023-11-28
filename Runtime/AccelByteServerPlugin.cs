// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AccelByte.Server
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class AccelByteServerPlugin
    {
        private static AccelByteSettingsV2 currentSettings;
        private static AccelByteSettingsV2 settings
        {
            get
            {
                return currentSettings;
            }

            set
            {
                currentSettings = value;
                OnSettingsUpdate?.Invoke(currentSettings);
            }
        }
        private static ServerOauthLoginSession session;
        private static CoroutineRunner coroutineRunner;
        private static IHttpClient httpClient;
        private static DedicatedServer server;
        private static ServerDSHub dsHub;
        private static ServerAMS ams;
        private static DedicatedServerManager dedicatedServerManager;
        private static ServerEcommerce ecommerce;
        private static ServerStatistic statistic;
        private static ServerUGC ugc;
        private static ServerQosManager _qosManager;
        private static ServerGameTelemetry gameTelemetry;
        private static ServerAchievement achievement;
        private static ServerLobby lobby;
        private static ServerSession _session;
        private static ServerCloudSave cloudSave;
        private static ServerMatchmaking matchmaking;
        private static ServerMatchmakingV2 _matchmakingV2;
        private static ServerUserAccount userAccount;
        private static ServerSeasonPass seasonPass;
        private static ServiceVersion serviceVersion;
        private static ServerAnalyticsService serverAnalyticsService;
        private static AccelByteStatsDMetricExporterApi statsDMetricExporter;

        private static bool initialized = false;
        internal static event Action configReset;
        internal static event Action<AccelByteSettingsV2> OnSettingsUpdate;
        public static event Action<SettingsEnvironment> environmentChanged;
        private static IHttpRequestSender defaultHttpSender = null;

        private static PredefinedEventScheduler predefinedEventScheduler = null;

        internal static OAuthConfig OAuthConfig
        {
            get
            {
                CheckPlugin();
                return settings.OAuthConfig;
            }
        }

        public static ServerConfig Config
        {
            get
            {
                CheckPlugin();
                return settings.ServerSdkConfig;
            }
        }

        internal static IHttpRequestSender DefaultHttpSender
        {
            get
            {
                if (defaultHttpSender == null)
                {
                    var httpSenderScheduler = new WebRequestSchedulerAsync();
                    defaultHttpSender = new UnityHttpRequestSender(httpSenderScheduler);
                }
                return defaultHttpSender;
            }
            set
            {
                defaultHttpSender = value;
                UpdateHttpClientSender(defaultHttpSender);
            }
        }

        static AccelByteServerPlugin()
        {
            AccelByteSDKMain.OnSDKStopped += () =>
            {
                initialized = false;
                if (ams != null)
                {
                    ams.Disconnect();
                }
                ResetApis();
            };

            OnSettingsUpdate += AccelByteDebug.Initialize;
        }

        private static void Initialize()
        {
            Initialize(null, null);

#if TEMPORARY_ENABLE_COMPAT_CHECK
            ValidateCompatibility();
#endif
        }

        internal static void Initialize(ServerConfig inConfig, OAuthConfig inOAuthConfig)
        {
            ResetApis();

            var activeEnvironment = AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;

            AccelByteSettingsV2 newSettings;
            if (inConfig == null && inOAuthConfig == null)
            {
                string activePlatform = AccelByteSettingsV2.GetActivePlatform(true);
                newSettings = RetrieveConfigFromJsonFile(activePlatform, activeEnvironment);
            }
            else
            {
                newSettings = new AccelByteSettingsV2(inOAuthConfig, inConfig);
            }

            try
            {
                newSettings.ServerSdkConfig.CheckRequiredField();
            }
            catch (Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }

            try
            {
                newSettings.OAuthConfig.CheckRequiredField();
            }
            catch (Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }

            settings = newSettings;

            HttpRequestBuilder.SetNamespace(settings.ServerSdkConfig.Namespace);
            HttpRequestBuilder.SetGameClientVersion(Application.version);
            HttpRequestBuilder.SetSdkVersion(AccelByteSettingsV2.AccelByteSDKVersion);

            coroutineRunner = new CoroutineRunner();
            var newHttpClient = new AccelByteHttpClient(DefaultHttpSender);
            var cacheImplementation = new AccelByteLRUMemoryCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>(newSettings.ServerSdkConfig.MaximumCacheSize);
            newHttpClient.SetCacheImplementation(cacheImplementation, newSettings.ServerSdkConfig.MaximumCacheLifeTime);
            httpClient = newHttpClient;

            session = CreateServerSessionClient(settings.ServerSdkConfig, settings.OAuthConfig, httpClient, coroutineRunner);
            server = CreateDedicatedServerClient(session, coroutineRunner);
            
            serverAnalyticsService = CreateServerAnalyticsService(settings.ServerSdkConfig, httpClient, coroutineRunner, server.Session);
            predefinedEventScheduler = new PredefinedEventScheduler(serverAnalyticsService);
            predefinedEventScheduler.SetEventEnabled(settings.ServerSdkConfig.EnablePreDefinedEvent);
            PredefinedGameStateCommand.GlobalGameStateCommand.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            if (AccelByteSDK.Environment != null)
            {
                AccelByteSDK.Environment.OnEnvironmentChanged += UpdateEnvironment;
                AccelByteSDK.Environment.OnEnvironmentChanged += environmentChanged;
            }

            initialized = true;
        }

        public static ServiceVersion GetServiceVersion()
        {
            if (serviceVersion == null)
            {
                CheckPlugin();
                serviceVersion = new ServiceVersion(
                    new ServiceVersionApi(
                        httpClient,
                        Config, // baseUrl==justBaseUrl
                        session),
                    coroutineRunner);
            }

            return serviceVersion;
        }

        static bool ValidateCompatibility()
        {
            string matrixJsonText = Utils.AccelByteFileUtils.ReadTextFileFromResource(AccelByteSettingsV2.ServiceCompatibilityResourcePath());
            var result = Utils.ServiceVersionUtils.CheckServicesCompatibility(GetServiceVersion(), new AccelByteServiceVersion(matrixJsonText)); ;
            return result;
        }

        public static Version GetPluginVersion()
        {
            return new Version(AccelByteSettingsV2.AccelByteSDKVersion);
        }

        private static AccelByteSettingsV2 RetrieveConfigFromJsonFile(string platform, SettingsEnvironment environment)
        {
            var retval = new AccelByteSettingsV2(platform, environment, true);
            retval.OverrideServerSDKConfig(AccelByteSDK.OverrideConfigs.SDKConfigOverride.GetByEnvironment(environment));
            retval.OverrideOAuthConfig(AccelByteSDK.OverrideConfigs.OAuthConfigOverride.GetByEnvironment(environment));
            return retval;
        }

        private static ServerOauthLoginSession CreateServerSessionClient(ServerConfig newSdkConcifg, OAuthConfig newOAuth, IHttpClient httpClient, CoroutineRunner taskRunner)
        {
            var newSession = new ServerOauthLoginSession(
                newSdkConcifg.IamServerUrl,
                newOAuth.ClientId,
                newOAuth.ClientSecret,
                httpClient,
                taskRunner);
            return newSession;
        }

        private static DedicatedServer CreateDedicatedServerClient(ServerOauthLoginSession newSession, CoroutineRunner taskRunner)
        {
            var newServer = new DedicatedServer(
                session,
                coroutineRunner);
            return newServer;
        }

        internal static ServerAMS CreateAMSConnection(string dsId, ServerConfig newSdkConfig)
        {
            return CreateAMSConnection(dsId, newSdkConfig.AMSServerUrl, newSdkConfig.AMSHeartbeatInterval);
        }

        internal static ServerAMS CreateAMSConnection(string dsId, string amsServerUrl, int amsHeartbeatIntervalSecond)
        {
            if (ams == null)
            {
                CheckPlugin();
            }

            if (dsId == null)
            {
                AccelByteDebug.LogWarning("dsid not provided, not connecting to ams");
                return null;
            }

            var newAMS = new ServerAMS(
                amsServerUrl,
                amsHeartbeatIntervalSecond,
                coroutineRunner);
            newAMS.Connect(dsId);

            ams = newAMS;
            return newAMS;
        }

        /// <summary>
        /// Check whether if this static class is need to be refreshed/re-init
        /// </summary>
        private static void CheckPlugin()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }
        }

        public static DedicatedServer GetDedicatedServer()
        {
            CheckPlugin();
            return server;
        }

        public static ServerDSHub GetDsHub()
        {
            if (dsHub != null)
            {
                return dsHub;
            }

            CheckPlugin();
            dsHub = new ServerDSHub(
                new ServerDSHubApi(
                    httpClient,
                    Config, // baseUrl==DSHubServerUrl 
                    session),
                session,
                coroutineRunner);

            dsHub.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            configReset += () =>
            {
                dsHub = null;
                dsHub = new ServerDSHub(
                    new ServerDSHubApi(
                        httpClient,
                        Config, // baseUrl==DSHubServerUrl 
                        session),
                    session,
                    coroutineRunner);

                dsHub.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            };

            return dsHub;
        }

        public static ServerAMS GetAMS()
        {
            if (ams != null)
            {
                return ams;
            }

            CheckPlugin();
            string dsId = string.IsNullOrEmpty(Config.DsId) ?
                Utils.CommandLineArgs.GetArg(DedicatedServer.CommandLineDsId) : Config.DsId;

            ams = CreateAMSConnection(dsId, Config);

            configReset += () =>
            {
                ams = null;
                string dsId = string.IsNullOrEmpty(Config.DsId) ?
                    Utils.CommandLineArgs.GetArg(DedicatedServer.CommandLineDsId) : Config.DsId;

                ams = CreateAMSConnection(dsId, Config);
            };

            return ams;
        }

        public static DedicatedServerManager GetDedicatedServerManager()
        {
            if (dedicatedServerManager != null)
            {
                return dedicatedServerManager;
            }

            CheckPlugin();
            dedicatedServerManager = new DedicatedServerManager(
                new DedicatedServerManagerApi(
                    httpClient,
                    Config, // baseUrl==DSMControllerServerUrl 
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                dedicatedServerManager = null;
                dedicatedServerManager = new DedicatedServerManager(
                new DedicatedServerManagerApi(
                    httpClient,
                    Config, // baseUrl==DSMControllerServerUrl 
                    session),
                session,
                coroutineRunner);
            };

            return dedicatedServerManager;
        }

        public static ServerEcommerce GetEcommerce()
        {
            if (ecommerce != null) return ecommerce;

            CheckPlugin();
            ecommerce = new ServerEcommerce(
                new ServerEcommerceApi(
                    httpClient,
                    Config, // baseUrl==PlatformServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                ecommerce = null;
                ecommerce = new ServerEcommerce(
                new ServerEcommerceApi(
                    httpClient,
                    Config, // baseUrl==PlatformServerUrl
                    session),
                session,
                coroutineRunner);
            };

            ecommerce.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            return ecommerce;
        }

        public static ServerStatistic GetStatistic()
        {
            if (statistic != null) return statistic;

            CheckPlugin();
            statistic = new ServerStatistic(
                new ServerStatisticApi(
                    httpClient,
                    Config, // baseUrl==StatisticServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                statistic = null;
                statistic = new ServerStatistic(
                new ServerStatisticApi(
                    httpClient,
                    Config, // baseUrl==StatisticServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return statistic;
        }

        public static ServerUGC GetUGC()
        {
            if (ugc != null) return ugc;

            CheckPlugin();
            ugc = new ServerUGC(
                new ServerUGCApi(
                    httpClient,
                    Config, // baseUrl==UGCServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                ugc = null;
                ugc = new ServerUGC(
                new ServerUGCApi(
                    httpClient,
                    Config, // baseUrl==UGCServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return ugc;
        }

        public static ServerQosManager GetQos()
        {
            if (_qosManager != null) return _qosManager;

            CheckPlugin();
            _qosManager = new ServerQosManager(
                new ServerQosManagerApi(
                    httpClient,
                    Config, // baseUrl==QosManagerServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                _qosManager = null;
                _qosManager = new ServerQosManager(
                new ServerQosManagerApi(
                    httpClient,
                    Config, // baseUrl==QosManagerServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return _qosManager;
        }

        public static ServerGameTelemetry GetGameTelemetry()
        {
            if (gameTelemetry != null) return gameTelemetry;

            CheckPlugin();
            gameTelemetry = new ServerGameTelemetry(
                new ServerGameTelemetryApi(
                    httpClient,
                    Config, // baseUrl==GameTelemetryServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                gameTelemetry = null;
                gameTelemetry = new ServerGameTelemetry(
                new ServerGameTelemetryApi(
                    httpClient,
                    Config, // baseUrl==GameTelemetryServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return gameTelemetry;
        }

        public static ServerAchievement GetAchievement()
        {
            if (achievement != null) return achievement;

            CheckPlugin();
            achievement = new ServerAchievement(
                new ServerAchievementApi(
                    httpClient,
                    Config, // baseUrl==AchievementServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                achievement = null;
                achievement = new ServerAchievement(
                new ServerAchievementApi(
                    httpClient,
                    Config, // baseUrl==AchievementServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return achievement;
        }

        public static ServerLobby GetLobby()
        {
            if (lobby != null) return lobby;

            CheckPlugin();
            lobby = new ServerLobby(
                new ServerLobbyApi(
                    httpClient,
                    Config, // baseUrl==LobbyServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                lobby = null;
                lobby = new ServerLobby(
                new ServerLobbyApi(
                    httpClient,
                    Config, // baseUrl==LobbyServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return lobby;
        }

        public static ServerSession GetSession()
        {
            if (_session != null) return _session;

            CheckPlugin();
            _session = new ServerSession(
                new ServerSessionApi(
                    httpClient,
                    Config, // baseUrl==SessionServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                _session = null;
                _session = new ServerSession(
                    new ServerSessionApi(
                        httpClient,
                        Config, // baseUrl==SessionServerUrl
                        session),
                    session,
                    coroutineRunner);
            };

            return _session;
        }

        public static ServerCloudSave GetCloudSave()
        {
            if (cloudSave != null) return cloudSave;

            CheckPlugin();
            cloudSave = new ServerCloudSave(
                new ServerCloudSaveApi(
                    httpClient,
                    Config, // baseUrl==CloudSaveServerUrl
                    session),
                session,
                coroutineRunner);

            configReset += () =>
            {
                cloudSave = null;
                cloudSave = new ServerCloudSave(
                new ServerCloudSaveApi(
                    httpClient,
                    Config, // baseUrl==CloudSaveServerUrl
                    session),
                session,
                coroutineRunner);
            };

            return cloudSave;
        }

        public static ServerMatchmaking GetMatchmaking()
        {
            CheckPlugin();

            if (matchmaking == null)
            {
                configReset += () =>
                {
                    matchmaking = null;
                    matchmaking = new ServerMatchmaking(
                    new ServerMatchmakingApi(
                        httpClient,
                        Config, // baseUrl==MatchmakingServerUrl
                        session),
                    session,
                    settings.ServerSdkConfig.Namespace,
                    coroutineRunner);
                };
            }

            return matchmaking ?? (matchmaking = new ServerMatchmaking(
                new ServerMatchmakingApi(
                    httpClient,
                    Config, // baseUrl==MatchmakingServerUrl
                    session),
                session,
                settings.ServerSdkConfig.Namespace,
                coroutineRunner));
        }

        public static ServerMatchmakingV2 GetMatchmakingV2()
        {
            if (_matchmakingV2 != null)
            {
                return _matchmakingV2;
            }

            CheckPlugin();
            _matchmakingV2 = new ServerMatchmakingV2(
                new ServerMatchmakingV2Api(
                    httpClient,
                    Config, // baseUrl==MatchmakingV2ServerUrl 
                    session),
                session,
                coroutineRunner);

            _matchmakingV2.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            configReset += () =>
            {
                _matchmakingV2 = null;
                _matchmakingV2 = new ServerMatchmakingV2(
                    new ServerMatchmakingV2Api(
                        httpClient,
                        Config, // baseUrl==MatchmakingV2ServerUrl 
                        session),
                    session,
                    coroutineRunner);

                _matchmakingV2.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            };

            return _matchmakingV2;
        }

        public static ServerUserAccount GetUserAccount()
        {
            CheckPlugin();

            if (userAccount == null)
            {
                configReset += () =>
                {
                    userAccount = null;
                    userAccount = new ServerUserAccount(
                    new ServerUserAccountApi(
                        httpClient,
                        Config, // baseUrl==BaseUrl
                        session),
                    session,
                    coroutineRunner);
                };
            }

            return userAccount ?? (userAccount = new ServerUserAccount(
                new ServerUserAccountApi(
                    httpClient,
                    Config, // baseUrl==BaseUrl
                    session),
                session,
                coroutineRunner));
        }

        public static ServerSeasonPass GetSeasonPass()
        {
            CheckPlugin();

            if (seasonPass == null)
            {
                configReset += () =>
                {
                    seasonPass = null;
                    seasonPass = new ServerSeasonPass(
                    new ServerSeasonPassApi(
                        httpClient,
                        Config, // baseUrl==BaseUrl
                        session),
                    session,
                    coroutineRunner);
                };
            }

            return seasonPass ?? (seasonPass = new ServerSeasonPass(
                new ServerSeasonPassApi(
                    httpClient,
                    Config, // baseUrl==BaseUrl
                    session),
                session,
                coroutineRunner));
        }

        public static ServerAnalyticsService GetServerAnalyticsService()
        {
            CheckPlugin();
            return serverAnalyticsService;
        }

        private static ServerAnalyticsService CreateServerAnalyticsService(ServerConfig newSdkConfig,
            IHttpClient httpClient,
            CoroutineRunner coroutineRunner,
            ISession IamSession)
        {
            serverAnalyticsService = new ServerAnalyticsService(
            new ServerAnalyticsApi(
                httpClient,
                newSdkConfig,
                IamSession),
            IamSession,
            coroutineRunner);

            return serverAnalyticsService;
        }

        public static AccelByteStatsDMetricExporterApi GetStatsDMetricExporter()
        {
            CheckPlugin();

            if (statsDMetricExporter == null)
            {
                configReset += () =>
                {
                    statsDMetricExporter = null;
                    statsDMetricExporter = new AccelByteStatsDMetricExporterApi(httpClient,
                        Config,
                        session);
                };
            }

            return statsDMetricExporter ?? (statsDMetricExporter = new AccelByteStatsDMetricExporterApi(
                httpClient,
                Config,
                session
                ));
        }

        #region Environment
        [Obsolete("Use AccelByteSDK.Environment.Set() to update environment target")]
        public static void SetEnvironment(SettingsEnvironment newEnvironment)
        {
            CheckPlugin();

            if (AccelByteSDK.Environment != null)
            {
                AccelByteSDK.Environment.Set(newEnvironment);
            }
            else
            {
                UpdateEnvironment(newEnvironment);
            }
        }

        [Obsolete("Use AccelByteSDK.Environment.Current to get current environment target")]
        public static SettingsEnvironment GetEnvironment()
        {
            return AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;
        }

        static void UpdateEnvironment(SettingsEnvironment newEnvironment)
        {
            try
            {
                string activePlatform = AccelByteSettingsV2.GetActivePlatform(true);
                var newSettings = RetrieveConfigFromJsonFile(activePlatform, newEnvironment);
                if (newSettings.ServerSdkConfig.IsRequiredFieldEmpty())
                {
                    newEnvironment = SettingsEnvironment.Default;
                    newSettings = RetrieveConfigFromJsonFile(activePlatform, newEnvironment);
                }
                if (newSettings.OAuthConfig.IsRequiredFieldEmpty())
                {
                    newSettings = RetrieveConfigFromJsonFile("", newEnvironment);
                }
                settings = newSettings;

                HttpRequestBuilder.SetNamespace(settings.ServerSdkConfig.Namespace);

                session = CreateServerSessionClient(settings.ServerSdkConfig, settings.OAuthConfig, httpClient, coroutineRunner);
                server = CreateDedicatedServerClient(session, coroutineRunner);

                serverAnalyticsService = CreateServerAnalyticsService(settings.ServerSdkConfig, httpClient, coroutineRunner, server.Session);
                if (predefinedEventScheduler != null)
                {
                    predefinedEventScheduler.SetEventEnabled(false);
                    predefinedEventScheduler.Dispose();
                }
                predefinedEventScheduler = null;
                predefinedEventScheduler = new PredefinedEventScheduler(serverAnalyticsService);
                predefinedEventScheduler.SetEventEnabled(settings.ServerSdkConfig.EnablePreDefinedEvent);

                configReset?.Invoke();
            }
            catch (Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }
        }
        #endregion

        private static void ResetApis()
        {
            server = null;
            dsHub = null;
            ams = null;
            dedicatedServerManager = null;
            ecommerce = null;
            statistic = null;
            _qosManager = null;
            gameTelemetry = null;
            achievement = null;
            lobby = null;
            cloudSave = null;
            seasonPass = null;
            configReset = null;
            serverAnalyticsService = null;
            statsDMetricExporter = null;

            if (predefinedEventScheduler != null)
            {
                predefinedEventScheduler.SetEventEnabled(false);
                predefinedEventScheduler.Dispose();
                predefinedEventScheduler = null;
            }
        }

        public static void ClearEnvironmentChangedEvent()
        {
            CheckPlugin();
        }

        private static void UpdateHttpClientSender(IHttpRequestSender newSender)
        {
            if (httpClient != null && httpClient is AccelByteHttpClient)
            {
                (httpClient as AccelByteHttpClient).SetSender(newSender);
            }
        }
    }
}
