// Copyright (c) 2020-2021 AccelByte Inc. All Rights Reserved.
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
        private static IHttpClient httpClient;
#else
        private static readonly ServerOauthLoginSession session;
        private static readonly ServerConfig config;
        private static readonly CoroutineRunner coroutineRunner;
        private static readonly IHttpClient httpClient;
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
        private static ServerSeasonPass seasonPass;

        private static bool hasBeenInitialized = false;

        public static ServerConfig Config
        {
            get
            {
                AccelByteServerPlugin.CheckPlugin();
                return AccelByteServerPlugin.config;
            }
        }

        static AccelByteServerPlugin()
        {
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.ExitingEditMode) return;

                AccelByteServerPlugin.hasBeenInitialized = false;
                AccelByteServerPlugin.accessToken = null;
                AccelByteServerPlugin.server = null;
                AccelByteServerPlugin.dedicatedServerManager = null;
                AccelByteServerPlugin.ecommerce = null;
                AccelByteServerPlugin.statistic = null;
                AccelByteServerPlugin.qos = null;
                AccelByteServerPlugin.gameTelemetry = null;
                AccelByteServerPlugin.achievement = null;
                AccelByteServerPlugin.lobby = null;
                AccelByteServerPlugin.cloudSave = null;
                AccelByteServerPlugin.seasonPass = null;
            };
        }

        private static void Init()
        {
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
            AccelByteServerPlugin.httpClient = new AccelByteHttpClient();

            AccelByteServerPlugin.session = new ServerOauthLoginSession(
                AccelByteServerPlugin.config.IamServerUrl,
                AccelByteServerPlugin.config.ClientId,
                AccelByteServerPlugin.config.ClientSecret,
                AccelByteServerPlugin.httpClient,
                AccelByteServerPlugin.coroutineRunner);

            AccelByteServerPlugin.server = new DedicatedServer(
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.coroutineRunner);
        }

        /// <summary>
        /// Check whether if this static class is need to be refreshed/re-init
        /// </summary>
        private static void CheckPlugin()
        {
#if UNITY_EDITOR
            if (AccelByteServerPlugin.hasBeenInitialized) return;
            
            AccelByteServerPlugin.hasBeenInitialized = true;
            AccelByteServerPlugin.Init();
#endif
        }

        public static DedicatedServer GetDedicatedServer()
        {
            AccelByteServerPlugin.CheckPlugin();
            return AccelByteServerPlugin.server;
        }

        public static DedicatedServerManager GetDedicatedServerManager()
        {
            if (AccelByteServerPlugin.dedicatedServerManager != null)
            {
                return AccelByteServerPlugin.dedicatedServerManager;
            }
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.dedicatedServerManager = new DedicatedServerManager(
                new DedicatedServerManagerApi(
                    AccelByteServerPlugin.config.DSMControllerServerUrl,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.dedicatedServerManager;
        }
        
        public static ServerEcommerce GetEcommerce()
        {
            if (AccelByteServerPlugin.ecommerce != null) return AccelByteServerPlugin.ecommerce;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.ecommerce = new ServerEcommerce(
                new ServerEcommerceApi(
                    AccelByteServerPlugin.config.PlatformServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.ecommerce;
        }
        
        public static ServerStatistic GetStatistic()
        {
            if (AccelByteServerPlugin.statistic != null) return AccelByteServerPlugin.statistic;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.statistic = new ServerStatistic(
                new ServerStatisticApi(
                    AccelByteServerPlugin.config.StatisticServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.statistic;
        }

        public static ServerQos GetQos()
        {
            if (AccelByteServerPlugin.qos != null) return AccelByteServerPlugin.qos;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.qos = new ServerQos(
                new ServerQosManagerApi(
                    AccelByteServerPlugin.config.QosManagerServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.qos;
        }

        public static ServerGameTelemetry GetGameTelemetry()
        {
            if (AccelByteServerPlugin.gameTelemetry != null) return AccelByteServerPlugin.gameTelemetry;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.gameTelemetry = new ServerGameTelemetry(
                new ServerGameTelemetryApi(
                    AccelByteServerPlugin.config.GameTelemetryServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.gameTelemetry;
        }

        public static ServerAchievement GetAchievement()
        {
            if (AccelByteServerPlugin.achievement != null) return AccelByteServerPlugin.achievement;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.achievement = new ServerAchievement(
                new ServerAchievementApi(
                    AccelByteServerPlugin.config.AchievementServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.achievement;
        }

        public static ServerLobby GetLobby()
        {
            if (AccelByteServerPlugin.lobby != null) return AccelByteServerPlugin.lobby;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.lobby = new ServerLobby(
                new ServerLobbyApi(
                    AccelByteServerPlugin.config.LobbyServerUrl,
                    AccelByteServerPlugin.httpClient), 
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.lobby;
        }

        public static ServerCloudSave GetCloudSave()
        {
            if (AccelByteServerPlugin.cloudSave != null) return AccelByteServerPlugin.cloudSave;
            
            AccelByteServerPlugin.CheckPlugin();
            AccelByteServerPlugin.cloudSave = new ServerCloudSave(
                new ServerCloudSaveApi(
                    AccelByteServerPlugin.config.CloudSaveServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner);

            return AccelByteServerPlugin.cloudSave;
        }

        public static ServerMatchmaking GetMatchmaking()
        {
            AccelByteServerPlugin.CheckPlugin();

            return AccelByteServerPlugin.matchmaking ?? (AccelByteServerPlugin.matchmaking = new ServerMatchmaking(
                new ServerMatchmakingApi(
                    AccelByteServerPlugin.config.MatchmakingServerUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner));
        }

        public static ServerUserAccount GetUserAccount()
        {
            AccelByteServerPlugin.CheckPlugin();

            return AccelByteServerPlugin.userAccount ?? (AccelByteServerPlugin.userAccount = new ServerUserAccount(
                new ServerUserAccountApi(
                    AccelByteServerPlugin.config.BaseUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner));
        }

        public static ServerSeasonPass GetSeasonPass()
        {
            AccelByteServerPlugin.CheckPlugin();

            return AccelByteServerPlugin.seasonPass ?? (AccelByteServerPlugin.seasonPass = new ServerSeasonPass(
                new ServerSeasonPassApi(
                    AccelByteServerPlugin.config.BaseUrl,
                    AccelByteServerPlugin.httpClient),
                AccelByteServerPlugin.session,
                AccelByteServerPlugin.config.Namespace,
                AccelByteServerPlugin.coroutineRunner));
        }
    }
}
