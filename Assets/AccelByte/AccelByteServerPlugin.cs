// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
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
#if UNITY_EDITOR
        private static ServerOauthLoginSession session;
        private static ServerConfig config;
        private static CoroutineRunner coroutineRunner;
        private static UnityHttpWorker httpWorker;
#else
        private static readonly ServerOauthLoginSession session;
        private static readonly ServerConfig config;
        private static readonly CoroutineRunner coroutineRunner;
        private static readonly UnityHttpWorker httpWorker;
#endif
        private static TokenData accessToken;
        private static DedicatedServer server;
        private static DedicatedServerManager dedicatedServerManager;
        private static ServerEcommerce ecommerce;
        private static ServerStatistic statistic;
        private static ServerQos qos;
        private static ServerGameTelemetry gameTelemetry;
        private static ServerAchievement achievement;
        private static ServerLobby lobby;
        private static ServerCloudSave cloudSave;
        private static ServerMatchmaking matchmaking;
        private static ServerUserAccount userAccount;

        private static bool hasBeenInitialized = false;

        public static ServerConfig Config
        {
            get
            {
                CheckPlugin();
                return AccelByteServerPlugin.config;
            }
        }

        static AccelByteServerPlugin()
        {
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    hasBeenInitialized = false;
            
                    accessToken = null;
                    server = null;
                    dedicatedServerManager = null;
                    ecommerce = null;
                    statistic = null;
                    qos = null;
                    gameTelemetry = null;
                    achievement = null;
                    lobby = null;
                    cloudSave = null;
                }
            };
        }

        private static void Init()
        {
#endif
#if (UNITY_WEBGL || ENABLE_IL2CPP) && !UNITY_EDITOR
            Utf8Json.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
                new [] {
                    Utf8Json.Formatters.PrimitiveObjectFormatter.Default
                },
                new[] {
                    Utf8Json.Resolvers.GeneratedResolver.Instance,
                    Utf8Json.Resolvers.BuiltinResolver.Instance,
                    Utf8Json.Resolvers.EnumResolver.Default,
                    // for unity
                    Utf8Json.Unity.UnityResolver.Instance
                }
            );
#endif

            var configFile = Resources.Load("AccelByteServerSDKConfig");

            if (configFile == null)
            {
                throw new Exception(
                    "'AccelByteServerSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeJsonText = ((TextAsset) configFile).text;

            AccelByteServerPlugin.config = wholeJsonText.ToObject<ServerConfig>();
            AccelByteServerPlugin.config.CheckRequiredField();
            AccelByteServerPlugin.config.Expand();
            AccelByteServerPlugin.coroutineRunner = new CoroutineRunner();
            AccelByteServerPlugin.httpWorker = new UnityHttpWorker();

            AccelByteServerPlugin.session = new ServerOauthLoginSession(
                AccelByteServerPlugin.config.IamServerUrl,
                AccelByteServerPlugin.config.ClientId,
                AccelByteServerPlugin.config.ClientSecret,
                AccelByteServerPlugin.httpWorker,
                AccelByteServerPlugin.coroutineRunner);

            AccelByteServerPlugin.server = new DedicatedServer(AccelByteServerPlugin.session, 
            AccelByteServerPlugin.coroutineRunner);
        }

        /// <summary>
        /// Check whether if this static class is need to be refreshed/re-init
        /// </summary>
        private static void CheckPlugin()
        {
#if UNITY_EDITOR
            if (!hasBeenInitialized)
            {
                hasBeenInitialized = true;
                Init();
            }
#endif
        }

        public static DedicatedServer GetDedicatedServer()
        {
            CheckPlugin();
            return AccelByteServerPlugin.server;
        }

        public static DedicatedServerManager GetDedicatedServerManager()
        {
            if (AccelByteServerPlugin.dedicatedServerManager == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.dedicatedServerManager = new DedicatedServerManager(
                    new DedicatedServerManagerApi(
                        AccelByteServerPlugin.config.DSMControllerServerUrl,
                        AccelByteServerPlugin.config.Namespace,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.dedicatedServerManager;
        }
        
        public static ServerEcommerce GetEcommerce()
        {
            if (AccelByteServerPlugin.ecommerce == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.ecommerce = new ServerEcommerce(
                    new ServerEcommerceApi(
                        AccelByteServerPlugin.config.PlatformServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.ecommerce;
        }
        
        public static ServerStatistic GetStatistic()
        {
            if (AccelByteServerPlugin.statistic == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.statistic = new ServerStatistic(
                    new ServerStatisticApi(
                        AccelByteServerPlugin.config.StatisticServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.statistic;
        }

        public static ServerQos GetQos()
        {
            if (AccelByteServerPlugin.qos == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.qos = new ServerQos(
                    new ServerQosManagerApi(
                        AccelByteServerPlugin.config.QosManagerServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.qos;
        }

        public static ServerGameTelemetry GetGameTelemetry()
        {
            if (AccelByteServerPlugin.gameTelemetry == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.gameTelemetry = new ServerGameTelemetry(
                    new ServerGameTelemetryApi(
                        AccelByteServerPlugin.config.GameTelemetryServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.gameTelemetry;
        }

        public static ServerAchievement GetAchievement()
        {
            if (AccelByteServerPlugin.achievement == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.achievement = new ServerAchievement(
                    new ServerAchievementApi(
                        AccelByteServerPlugin.config.AchievementServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.achievement;
        }

        public static ServerLobby GetLobby()
        {
            if (AccelByteServerPlugin.lobby == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.lobby = new ServerLobby(
                    new ServerLobbyApi(
                        AccelByteServerPlugin.config.LobbyServerUrl,
                        AccelByteServerPlugin.httpWorker), 
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.lobby;
        }

        public static ServerCloudSave GetCloudSave()
        {
            if (AccelByteServerPlugin.cloudSave == null)
            {
                CheckPlugin();
                AccelByteServerPlugin.cloudSave = new ServerCloudSave(
                    new ServerCloudSaveApi(
                        AccelByteServerPlugin.config.CloudSaveServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.cloudSave;
        }

        public static ServerMatchmaking GetMatchmaking()
        {
            CheckPlugin();

            if (AccelByteServerPlugin.matchmaking == null)
            {
                AccelByteServerPlugin.matchmaking = new ServerMatchmaking(
                    new ServerMatchmakingApi(
                        AccelByteServerPlugin.config.MatchmakingServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.matchmaking;
        }

        public static ServerUserAccount GetUserAccount()
        {
            CheckPlugin();

            if (AccelByteServerPlugin.userAccount == null)
            {
                AccelByteServerPlugin.userAccount = new ServerUserAccount(
                    new ServerUserAccountApi(
                        AccelByteServerPlugin.config.BaseUrl,
                        AccelByteServerPlugin.config.ApiBaseUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.userAccount;
        }
    }
}
