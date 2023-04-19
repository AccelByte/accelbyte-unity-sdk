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
        private static AccelByteSettingsV2 settings;
        private static ServerOauthLoginSession session;
        private static CoroutineRunner coroutineRunner;
        private static IHttpClient httpClient;
        private static TokenData accessToken;
        private static DedicatedServer server;
        private static ServerDSHub dsHub;
        private static ServerWatchdog watchdog;
        private static DedicatedServerManager dedicatedServerManager;
        private static ServerEcommerce ecommerce;
        private static ServerStatistic statistic;
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

        private static bool initialized = false;
        private static SettingsEnvironment activeEnvironment = SettingsEnvironment.Default;
        internal static event Action configReset;
        public static event Action<SettingsEnvironment> environmentChanged;
        private static IHttpRequestSender defaultHttpSender = new UnityHttpRequestSender();

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
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.ExitingEditMode)
                {
                    return;
                }

                initialized = false;
                if (watchdog != null)
                {
                    watchdog.Disconnect();
                }
                ResetApis();
            };
#endif
        }

        private static void Initialize()
        {
            Initialize(null, null);

            ValidateCompatibility();
        }

        internal static void Initialize(ServerConfig inConfig, OAuthConfig inOAuthConfig)
        {
            ResetApis();

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

            newSettings.OAuthConfig.CheckRequiredField();
            newSettings.ServerSdkConfig.CheckRequiredField();

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

        internal static ServerWatchdog CreateWatchdogConnection(string dsId, ServerConfig newSdkConfig)
        {
            return CreateWatchdogConnection(dsId, newSdkConfig.WatchdogServerUrl, newSdkConfig.WatchdogHeartbeatInterval);
        }

        internal static ServerWatchdog CreateWatchdogConnection(string dsId, string watchdogServerUrl, int watchdogHeartbeatIntervalSecond)
        {
            if (dsId == null)
            {
                AccelByteDebug.LogWarning("dsid not provided, not connecting to watchdog");
                return null;
            }

            var newWatchdog = new ServerWatchdog(
                watchdogServerUrl,
                watchdogHeartbeatIntervalSecond,
                coroutineRunner);
            newWatchdog.Connect(dsId);
            return newWatchdog;
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
            };

            return dsHub;
        }

        public static ServerWatchdog GetWatchdog()
        {
            CheckPlugin();
            return watchdog;
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

        public static void SetEnvironment(SettingsEnvironment environment)
        {
            CheckPlugin();
            activeEnvironment = environment;
            string activePlatform = AccelByteSettingsV2.GetActivePlatform(true);
            var newSettings = RetrieveConfigFromJsonFile(activePlatform, activeEnvironment);
            if (newSettings.ServerSdkConfig.IsRequiredFieldEmpty())
            {
                activeEnvironment = SettingsEnvironment.Default;
                newSettings = RetrieveConfigFromJsonFile(activePlatform, activeEnvironment);
            }
            if (newSettings.OAuthConfig.IsRequiredFieldEmpty())
            {
                newSettings = RetrieveConfigFromJsonFile("", activeEnvironment);
            }
            settings = newSettings;

            HttpRequestBuilder.SetNamespace(settings.ServerSdkConfig.Namespace);

            session = CreateServerSessionClient(settings.ServerSdkConfig, settings.OAuthConfig, httpClient, coroutineRunner);
            server = CreateDedicatedServerClient(session, coroutineRunner);
            if (configReset != null) 
            { 
                configReset.Invoke(); 
            }
            if (environmentChanged != null) 
            { 
                environmentChanged.Invoke(activeEnvironment); 
            }
        }

        private static void ResetApis()
        {
            accessToken = null;
            server = null;
            dsHub = null;
            watchdog = null;
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
            environmentChanged = null;
        }

        public static SettingsEnvironment GetEnvironment()
        {
            CheckPlugin();
            return activeEnvironment;
        }

        public static void ClearEnvironmentChangedEvent()
        {
            CheckPlugin();
            environmentChanged = null;
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
