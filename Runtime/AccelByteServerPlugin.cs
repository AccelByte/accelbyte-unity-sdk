// Copyright (c) 2020-2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEditor;
using UnityEngine;

namespace AccelByte.Server
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class AccelByteServerPlugin
    {
        private static ServerOauthLoginSession session;
        private static ServerConfig config;
        private static CoroutineRunner coroutineRunner;
        private static IHttpClient httpClient;
        private static TokenData accessToken;
        private static DedicatedServer server;
        private static DedicatedServerManager dedicatedServerManager;
        private static ServerEcommerce ecommerce;
        private static ServerStatistic statistic;
        private static ServerQosManager _qosManager;
        private static ServerGameTelemetry gameTelemetry;
        private static ServerAchievement achievement;
        private static ServerLobby lobby;
        private static ServerCloudSave cloudSave;
        private static ServerMatchmaking matchmaking;
        private static ServerUserAccount userAccount;
        private static ServerSeasonPass seasonPass;

        private static bool hasBeenInitialized = false;
        private static SettingsEnvironment activeEnvironment = SettingsEnvironment.Default;
        internal static event Action configReset;
        public static event Action<SettingsEnvironment> environmentChanged;

        public static ServerConfig Config
        {
            get
            {
                CheckPlugin();
                return config;
            }
        }

        static AccelByteServerPlugin()
        {
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.ExitingEditMode) return;

                hasBeenInitialized = false;
                accessToken = null;
                server = null;
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
            };
        }

        private static void Init()
        {
#endif
            RetrieveConfigFromJsonFile();
            config.CheckRequiredField();
            config.Expand();
            coroutineRunner = new CoroutineRunner();
            httpClient = new AccelByteHttpClient();
            InitServerSessionClient();  
            InitDedicatedServerClient();
        }

        private static void RetrieveConfigFromJsonFile()
        {
            var configFile = Resources.Load("AccelByteServerSDKConfig");

            if (configFile == null)
            {
                throw new Exception(
                    "'AccelByteServerSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeJsonText = ((TextAsset)configFile).text;

            MultiServerConfigs multiServerConfigs = wholeJsonText.ToObject<MultiServerConfigs>();
            switch (activeEnvironment)
            {
                case SettingsEnvironment.Development:
                    config = multiServerConfigs.Development; break;
                case SettingsEnvironment.Certification:
                    config = multiServerConfigs.Certification; break;
                case SettingsEnvironment.Production:
                    config = multiServerConfigs.Production; break;
                case SettingsEnvironment.Default:
                default:
                    config = multiServerConfigs.Default; break;
            }
        }

        private static void InitServerSessionClient()
        {
            session = new ServerOauthLoginSession(
                config.IamServerUrl,
                config.ClientId,
                config.ClientSecret,
                httpClient,
                coroutineRunner);
        }

        private static void InitDedicatedServerClient()
        {
            server = new DedicatedServer(
                session,
                coroutineRunner);
        }

        /// <summary>
        /// Check whether if this static class is need to be refreshed/re-init
        /// </summary>
        private static void CheckPlugin()
        {
#if UNITY_EDITOR
            if (hasBeenInitialized) return;
            
            hasBeenInitialized = true;
            Init();
#endif
        }

        public static DedicatedServer GetDedicatedServer()
        {
            CheckPlugin();
            return server;
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

            if(matchmaking == null)
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
                    config.Namespace,
                    coroutineRunner);
                };
            }

            return matchmaking ?? (matchmaking = new ServerMatchmaking(
                new ServerMatchmakingApi(
                    httpClient, 
                    Config, // baseUrl==MatchmakingServerUrl
                    session),
                session,
                config.Namespace,
                coroutineRunner));
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
            RetrieveConfigFromJsonFile();
            if (config.IsRequiredFieldEmpty())
            {
                activeEnvironment = SettingsEnvironment.Default;
                RetrieveConfigFromJsonFile();
            }
            config.Expand();
            session = null;
            server = null;
            InitServerSessionClient();
            InitDedicatedServerClient();
            if (configReset != null) { configReset.Invoke(); }
            if (environmentChanged != null) { environmentChanged.Invoke(activeEnvironment); }
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
    }
}
