// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AccelByte.Core;
using AccelByte.Models;
using HybridWebSocket;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AccelByte.Api
{
    /// <summary>
    /// Equivalent to Unreal's FRegistry
    /// </summary>
    public static class AccelBytePlugin
    {
        private static bool initialized = false;
        internal static Action<AccelByteSettingsV2> OnSettingsUpdate;
        public static event Action<SettingsEnvironment> environmentChanged;

        internal static OAuthConfig OAuthConfig
        {
            get
            {
                OAuthConfig retval = AccelByteSDK.GetClientRegistry().OAuthConfig;
                return retval;
            }
        }

        public static Config Config
        {
            get
            {
                Config retval = AccelByteSDK.GetClientRegistry().Config;
                return retval;
            }
        }

        static AccelBytePlugin()
        {
            const string deprecatedWarning = "AccelBytePlugin is deprecated and will be removed on July release. Please migrate to AccelByteSDK.GetClientRegistry.\nGuideline how to migrate: https://docs.accelbyte.io/gaming-services/developers/sdk-tools/sdk-guides/multiple-registries/migrate-to-multiple-registry-unity/";
            UnityEngine.Debug.LogWarning(deprecatedWarning);

            if (!initialized)
            {
                Initialize();
            }
        }

        public static User GetUser()
        {
            User retval = AccelByteSDK.GetClientRegistry().GetApi().GetUser();
            return retval;
        }

        public static UserProfiles GetUserProfiles()
        {
            UserProfiles retval = AccelByteSDK.GetClientRegistry().GetApi().GetUserProfiles();
            return retval;
        }

        public static Categories GetCategories()
        {
            Categories retval = AccelByteSDK.GetClientRegistry().GetApi().GetCategories();
            return retval;
        }

        public static Items GetItems()
        {
            Items retval = AccelByteSDK.GetClientRegistry().GetApi().GetItems();
            return retval;
        }

        public static Currencies GetCurrencies()
        {
            Currencies retval = AccelByteSDK.GetClientRegistry().GetApi().GetCurrencies();
            return retval;
        }

        public static Orders GetOrders()
        {
            Orders retval = AccelByteSDK.GetClientRegistry().GetApi().GetOrders();
            return retval;
        }

        public static Reward GetReward()
        {
            Reward retval = AccelByteSDK.GetClientRegistry().GetApi().GetReward();
            return retval;
        }

        public static Wallet GetWallet()
        {
            Wallet retval = AccelByteSDK.GetClientRegistry().GetApi().GetWallet();
            return retval;
        }

        public static Lobby GetLobby()
        {
            Lobby retval = AccelByteSDK.GetClientRegistry().GetApi().GetLobby();
            return retval;
        }

        public static Session GetSession()
        {
            Session retval = AccelByteSDK.GetClientRegistry().GetApi().GetSession();
            return retval;
        }

        public static MatchmakingV2 GetMatchmaking()
        {
            MatchmakingV2 retval = AccelByteSDK.GetClientRegistry().GetApi().GetMatchmakingV2();
            return retval;
        }

        public static CloudStorage GetCloudStorage()
        {
            CloudStorage retval = AccelByteSDK.GetClientRegistry().GetApi().GetCloudStorage();
            return retval;
        }

        public static GameProfiles GetGameProfiles()
        {
            GameProfiles retval = AccelByteSDK.GetClientRegistry().GetApi().GetGameProfiles();
            return retval;
        }

        public static Entitlement GetEntitlement()
        {
            Entitlement retval = AccelByteSDK.GetClientRegistry().GetApi().GetEntitlement();
            return retval;
        }

        public static Fulfillment GetFulfillment()
        {
            Fulfillment retval = AccelByteSDK.GetClientRegistry().GetApi().GetFulfillment();
            return retval;
        }

        public static Statistic GetStatistic()
        {
            Statistic retval = AccelByteSDK.GetClientRegistry().GetApi().GetStatistic();
            return retval;
        }

        public static QosManager GetQos()
        {
            QosManager retval = AccelByteSDK.GetClientRegistry().GetApi().GetQos();
            return retval;
        }

        public static Agreement GetAgreement()
        {
            Agreement retval = AccelByteSDK.GetClientRegistry().GetApi().GetAgreement();
            return retval;
        }

        public static Leaderboard GetLeaderboard()
        {
            Leaderboard retval = AccelByteSDK.GetClientRegistry().GetApi().GetLeaderboard();
            return retval;
        }

        public static CloudSave GetCloudSave()
        {
            CloudSave retval = AccelByteSDK.GetClientRegistry().GetApi().GetCloudSave();
            return retval;
        }

        public static GameTelemetry GetGameTelemetry()
        {
            GameTelemetry retval = AccelByteSDK.GetClientRegistry().GetApi().GetGameTelemetry();
            return retval;
        }

        public static Achievement GetAchievement()
        {
            Achievement retval = AccelByteSDK.GetClientRegistry().GetApi().GetAchievement();
            return retval;
        }

        public static Group GetGroup()
        {
            Group retval = AccelByteSDK.GetClientRegistry().GetApi().GetGroup();
            return retval;
        }

        public static UGC GetUgc()
        {
            UGC retval = AccelByteSDK.GetClientRegistry().GetApi().GetUgc();
            return retval;
        }

        public static Reporting GetReporting()
        {
            Reporting retval = AccelByteSDK.GetClientRegistry().GetApi().GetReporting();
            return retval;
        }

        public static SeasonPass GetSeasonPass()
        {
            SeasonPass retval = AccelByteSDK.GetClientRegistry().GetApi().GetSeasonPass();
            return retval;
        }

        public static SessionBrowser GetSessionBrowser()
        {
            SessionBrowser retval = AccelByteSDK.GetClientRegistry().GetApi().GetSessionBrowser();
            return retval;
        }

        public static TurnManager GetTurnManager()
        {
            TurnManager retval = AccelByteSDK.GetClientRegistry().GetApi().GetTurnManager();
            return retval;
        }

        public static Miscellaneous GetMiscellaneous()
        {
            Miscellaneous retval = AccelByteSDK.GetClientRegistry().GetApi().GetMiscellaneous();
            return retval;
        }

        public static Gdpr GetGdpr()
        {
            Gdpr retval = AccelByteSDK.GetClientRegistry().GetApi().GetGdpr();
            return retval;
        }

        public static PresenceBroadcastEvent GetPresenceBroadcastEvent()
        {
            PresenceBroadcastEvent retval = AccelByteSDK.GetClientRegistry().GetApi().GetPresenceBroadcastEvent();
            return retval;
        }

        public static PresenceBroadcastEventScheduler GetPresenceBroadcastEventScheduler()
        {
            PresenceBroadcastEventScheduler retval = AccelByteSDK.GetClientRegistry().GetPresenceBroadcastEvent();
            return retval;
        }

        public static HeartBeat GetHeartBeat()
        {
            HeartBeat retval = AccelByteSDK.GetClientRegistry().GetApi().GetHeartBeatService();
            return retval;
        }

        public static StoreDisplay GetStoreDisplay()
        {
            StoreDisplay retval = AccelByteSDK.GetClientRegistry().GetApi().GetStoreDisplayService();
            return retval;
        }

        public static BinaryCloudSave GetBinaryCloudSave()
        {
            BinaryCloudSave retval = AccelByteSDK.GetClientRegistry().GetApi().GetBinaryCloudSave();
            return retval;
        }

        public static AnalyticsService GetAnalyticService()
        {
            AnalyticsService retval = AccelByteSDK.GetClientRegistry().GetApi().GetAnalyticsService();
            return retval;
        }

        public static GameStandardAnalyticsClientService GetGameStandardAnalyticsService()
        {
            GameStandardAnalyticsClientService retval = AccelByteSDK.GetClientRegistry().GetGameStandardEvents();
            return retval;
        }

        public static void ConfigureHttpApi<T>(params object[] args) where T : HttpApiBase
        {
            AccelByteSDK.GetClientRegistry().GameClient.ConfigureHttpApi<T>(args);
        }

        public static T GetHttpApi<T>() where T : HttpApiBase
        {
            return AccelByteSDK.GetClientRegistry().GameClient.GetHttpApi<T>();
        }

        #region Environment
        [Obsolete("Use AccelByteSDK.Environment.Set() to update environment target")]
        public static void SetEnvironment(SettingsEnvironment newEnvironment)
        {
            AccelByteSDK.Environment.Set(newEnvironment);
        }

        [Obsolete("Use AccelByteSDK.Environment.Current to get current environment target")]
        public static SettingsEnvironment GetEnvironment()
        {
            return AccelByteSDK.Environment.Current;
        }
        #endregion

        internal static void Reset()
        {
            AccelByteSDK.Environment.OnEnvironmentChanged -= environmentChanged;
            AccelByteSDKMain.OnSDKStopped -= Reset;
            initialized = false;
        }

        internal static void Initialize()
        {
            AccelByteSDK.Environment.OnEnvironmentChanged += environmentChanged;
            AccelByteSDKMain.OnSDKStopped += Reset;

            initialized = true;
        }

        public static ServiceVersion GetServiceVersion()
        {
            return AccelByteSDK.GetClientRegistry().GetApi().GetVersionService();
        }

        public static Version GetPluginVersion()
        {
            return new Version(AccelByteSDK.Version);
        }

        internal static UserSession CreateSession(Config newSdkConfig, IHttpClient httpClient, CoroutineRunner taskRunner, string environment, IFileStream accelByteFileStream)
        {
            string sessionCacheTableName = $"TokenCache/{environment}/TokenData";

            IAccelByteDataStorage dataStorage = new Core.AccelByteDataStorageBinaryFile(accelByteFileStream);

            var newSession = new UserSession(
                httpClient,
                taskRunner,
                newSdkConfig.PublisherNamespace,
                newSdkConfig.UsePlayerPrefs,
                sessionCacheTableName,
                dataStorage);

            return newSession;
        }

        internal static User CreateUser(Config newSdkConfig, UserSession userSession, CoroutineRunner taskRunner, IHttpClient httpClient)
        {
            var newUser = new User(
                new UserApi(
                    httpClient,
                    newSdkConfig,
                    userSession),
                userSession,
                taskRunner);

            return newUser;
        }

        internal static PresenceBroadcastEventScheduler CreatePresenceBroadcastEventScheduler(AnalyticsService analyticsService, Config config)
        {
            bool presenceBroadcastEventJobEnabled = config.EnablePresenceBroadcastEvent;
            var presenceInitialState = (PresenceBroadcastEventGameState)config.PresenceBroadcastEventGameState;
            string presenceInitialStateDescription = config.PresenceBroadcastEventGameStateDescription;
            string @namespace = config.Namespace;

            var newPresenceBroadcastEventScheduler = new PresenceBroadcastEventScheduler(analyticsService, @namespace, presenceInitialState, presenceInitialStateDescription);
            if (presenceBroadcastEventJobEnabled)
            {
                int presenceBroadcastIntervalInMs = Utils.TimeUtils.SecondsToMilliseconds(config.PresenceBroadcastEventInterval);
                newPresenceBroadcastEventScheduler.StartPresenceEvent(presenceBroadcastIntervalInMs);
            }
            return newPresenceBroadcastEventScheduler;
        }

        internal static PredefinedEventScheduler CreatePredefinedEventScheduler(AnalyticsService analyticsService, Config config)
        {
            var newPredefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
            newPredefinedEventScheduler.SetEventEnabled(config.EnablePreDefinedEvent);
            return newPredefinedEventScheduler;
        }

        public static void ClearEnvironmentChangedEvent()
        {

        }
    }
}
