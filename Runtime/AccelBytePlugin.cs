// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
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
        private static CoroutineRunner coroutineRunner;
        private static IHttpClient httpClient;

        private static GameClient gameClient;

        #region Modules with ApiBase
        private static User user;
        private static Categories categories;
        private static Items items;
        private static Currencies currencies;
        private static Orders orders;
        private static Wallet wallet;
        private static UserProfiles userProfiles;
        private static Lobby lobby;
        private static CloudStorage cloudStorage;
        private static GameProfiles gameProfiles;
        private static Entitlement entitlement;
        private static Fulfillment fulfillment;
        private static Statistic statistic;
        private static QosManager _qosManager;
        private static Agreement agreement;
        private static Leaderboard leaderboard;
        private static CloudSave cloudSave;
        private static GameTelemetry gameTelemetry;
        private static Achievement achievement;
        private static Group group;
        private static UGC ugc;
        private static Reporting reporting;
        private static SeasonPass seasonPass;
        private static Miscellaneous miscellaneous;
        private static Reward reward;
        private static SessionBrowser sessionBrowser;
        private static Session _session;
        private static MatchmakingV2 _matchmakingV2;
        private static TurnManager turnManager;
        private static ServiceVersion serviceVersion;
        private static HeartBeat heartBeat;
        private static StoreDisplay storeDisplay;
        private static BinaryCloudSave binaryCloudSave;
        private static PresenceBroadcastEvent presenceBroadcastEvent;
        private static PresenceBroadcastEventScheduler presenceBroadcastEventScheduler;
        private static AnalyticsService analyticsService;
        private static GameStandardAnalyticsClientService gameStandardAnalyticsService;
        private static Gdpr gdpr;
        #endregion /Modules with ApiBase

        private static bool initialized = false;
        internal static event Action configReset;
        internal static Action<AccelByteSettingsV2> OnSettingsUpdate;
        public static event Action<SettingsEnvironment> environmentChanged;
        private static IHttpRequestSender defaultHttpSender = null;

        private static PredefinedEventScheduler predefinedEventScheduler;
        private static AccelByteGameStandardEventCacheImp gameStandardCacheImp;

        private static UserSession activeSession;

        internal static OAuthConfig OAuthConfig
        {
            get
            {
                CheckPlugin();
                return settings.OAuthConfig;
            }
        }

        public static Config Config
        {
            get
            {
                CheckPlugin();
                return settings.SDKConfig;
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

        static AccelBytePlugin()
        {
            AccelByteSDKMain.OnSDKStopped += () =>
            {
                if (gameStandardAnalyticsService != null)
                {
                    gameStandardAnalyticsService.StopScheduler();

                    CacheGameStandardAnalytics(gameStandardCacheImp, gameStandardAnalyticsService, AccelByteSDK.Environment.Current.ToString(), false);
                }

                Reset();
            };
            OnSettingsUpdate += AccelByteDebug.Initialize;
        }

        internal static void Reset()
        {
            initialized = false;

            if (heartBeat != null)
            {
                heartBeat.SetHeartBeatEnabled(false);
                heartBeat = null;
            }

            if (presenceBroadcastEventScheduler != null)
            {
                presenceBroadcastEventScheduler.SetPresenceBroadcastEventEnabled(false);
                presenceBroadcastEventScheduler = null;
            }

            if (predefinedEventScheduler != null)
            {
                predefinedEventScheduler.SetEventEnabled(false);
                predefinedEventScheduler.Dispose();
                predefinedEventScheduler = null;
            }

            ResetApis();
        }

        internal static void Initialize()
        {
            Initialize(null, null);

#if TEMPORARY_ENABLE_COMPAT_CHECK
            ValidateCompatibility();
#endif
        }

        internal static void Initialize(Config inConfig, OAuthConfig inOAuthConfig)
        {
            ResetApis();

            var activeEnvironment = AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;

            AccelByteSettingsV2 newSettings;
            if (inConfig == null && inOAuthConfig == null)
            {
                string activePlatform = AccelByteSettingsV2.GetActivePlatform(false);
                newSettings = RetrieveConfigFromJsonFile(activePlatform, activeEnvironment);
            }
            else
            {
                newSettings = new AccelByteSettingsV2(inOAuthConfig, inConfig);
            }

            try
            {
                newSettings.SDKConfig.CheckRequiredField();
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

            coroutineRunner = new CoroutineRunner();

            httpClient = CreateHttpClient(settings.OAuthConfig, settings.SDKConfig);
            gameClient = CreateGameClient(settings.OAuthConfig, settings.SDKConfig, httpClient);
            user = CreateUser(settings.SDKConfig, coroutineRunner, httpClient);
            activeSession = user.Session;

            analyticsService = CreateAnalytics(settings.SDKConfig, httpClient, coroutineRunner, activeSession);
            predefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
            predefinedEventScheduler.SetEventEnabled(settings.SDKConfig.EnablePreDefinedEvent);

            gameStandardAnalyticsService = new GameStandardAnalyticsClientService(
                analyticsService,
                activeSession,
                coroutineRunner
            );

            string gameStandardCacheDirectory = GameStandardAnalyticsClientService.DefaultCacheDirectory;
            string gameStandardCacheFileName = GameStandardAnalyticsClientService.DefaultCacheFileName;
            gameStandardCacheImp = new AccelByteGameStandardEventCacheImp(gameStandardCacheDirectory, gameStandardCacheFileName, settings.OAuthConfig.ClientId);

            LoadGameStandardAnalyticsCache(gameStandardCacheImp, gameStandardAnalyticsService, AccelByteSDK.Environment.Current.ToString());

            HttpRequestBuilder.SetNamespace(settings.SDKConfig.Namespace);
            HttpRequestBuilder.SetGameClientVersion(Application.version);
            HttpRequestBuilder.SetSdkVersion(AccelByteSettingsV2.AccelByteSDKVersion);
            ServicePointManager.ServerCertificateValidationCallback = OnCertificateValidated;
            PredefinedGameStateCommand.GlobalGameStateCommand.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            AccelByteSDK.Environment.OnEnvironmentChangedV2 += UpdateEnvironment;
            AccelByteSDK.Environment.OnEnvironmentChanged += environmentChanged;

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
                        activeSession),
                    coroutineRunner);
            }

            return serviceVersion;
        }

        static bool ValidateCompatibility()
        {
            string matrixJsonText = Utils.AccelByteFileUtils.ReadTextFileFromResource(AccelByteSettingsV2.ServiceCompatibilityResourcePath());
            var result = Utils.ServiceVersionUtils.CheckServicesCompatibility(GetServiceVersion(), new AccelByteServiceVersion(matrixJsonText));
            return result;
        }

        public static Version GetPluginVersion()
        {
            return new Version(AccelByteSettingsV2.AccelByteSDKVersion);
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

        private static bool OnCertificateValidated(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);

                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }

            return isOk;
        }

        private static AccelByteSettingsV2 RetrieveConfigFromJsonFile(string platform, SettingsEnvironment environment)
        {
            var retval = new AccelByteSettingsV2(platform, environment, false);
            retval.OverrideClientSDKConfig(AccelByteSDK.OverrideConfigs.SDKConfigOverride.GetByEnvironment(environment));
            retval.OverrideOAuthConfig(AccelByteSDK.OverrideConfigs.OAuthConfigOverride.GetByEnvironment(environment));
            return retval;
        }

        private static AccelByteHttpClient CreateHttpClient(OAuthConfig newOAuthConfig, Config newSdkConfig)
        {
            var newHttpClient = new AccelByteHttpClient(DefaultHttpSender);
            var cacheImplementation = new AccelByteLRUMemoryCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>(newSdkConfig.MaximumCacheSize);
            newHttpClient.SetCacheImplementation(cacheImplementation, newSdkConfig.MaximumCacheLifeTime);
            newHttpClient.SetCredentials(newOAuthConfig.ClientId, newOAuthConfig.ClientSecret);
            newHttpClient.SetBaseUri(new Uri(newSdkConfig.BaseUrl));
            return newHttpClient;
        }

        private static void UpdateHttpClientSender(IHttpRequestSender newSender)
        {
            if (httpClient != null && httpClient is AccelByteHttpClient)
            {
                (httpClient as AccelByteHttpClient).SetSender(newSender);
            }
        }

        private static GameClient CreateGameClient(OAuthConfig newOAuthConfig, Config newSdkConfig, IHttpClient httpClient)
        {
            var newGameClient = new GameClient(newOAuthConfig, newSdkConfig, httpClient);
            return newGameClient;
        }

        private static User CreateUser(Config newSdkConfig, CoroutineRunner taskRunner, IHttpClient httpClient)
        {
            var userSession = new UserSession(
                httpClient,
                taskRunner,
                newSdkConfig.PublisherNamespace,
                newSdkConfig.UsePlayerPrefs);

            var newUser = new User(
                new UserApi(
                    httpClient,
                    newSdkConfig,
                    userSession),
                userSession,
                taskRunner);

            newUser.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            return newUser;
        }

        public static User GetUser()
        {
            CheckPlugin();
            return user;
        }

        public static UserProfiles GetUserProfiles()
        {
            if (userProfiles == null)
            {
                CheckPlugin();
                userProfiles = new UserProfiles(
                    new UserProfilesApi(
                        httpClient,
                        Config, // baseUrl==BasicServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    userProfiles = null;
                    userProfiles = new UserProfiles(
                        new UserProfilesApi(
                            httpClient,
                            Config, // baseUrl==BasicServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                    userProfiles.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };

                userProfiles.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            }

            return userProfiles;
        }

        public static Categories GetCategories()
        {
            if (categories == null)
            {
                CheckPlugin();
                categories = new Categories(
                    new CategoriesApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    categories = null;
                    categories = new Categories(
                        new CategoriesApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return categories;
        }

        public static Items GetItems()
        {
            if (items == null)
            {
                CheckPlugin();
                items = new Items(
                    new ItemsApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    items = null;
                    items = new Items(
                        new ItemsApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return items;
        }

        public static Currencies GetCurrencies()
        {
            if (currencies == null)
            {
                CheckPlugin();
                currencies = new Currencies(
                    new CurrenciesApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    currencies = null;
                    currencies = new Currencies(
                        new CurrenciesApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return currencies;
        }

        public static Orders GetOrders()
        {
            if (orders == null)
            {
                CheckPlugin();
                orders = new Orders(
                    new OrdersApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    orders = null;
                    orders = new Orders(
                        new OrdersApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                    orders.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };
            }

            orders.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            return orders;
        }

        public static Reward GetReward()
        {
            if (reward == null)
            {
                CheckPlugin();
                reward = new Reward(
                    new RewardApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    reward = null;
                    reward = new Reward(
                        new RewardApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return reward;
        }

        public static Wallet GetWallet()
        {
            if (wallet == null)
            {
                CheckPlugin();
                wallet = new Wallet(
                    new WalletApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    wallet = null;
                    wallet = new Wallet(
                        new WalletApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return wallet;
        }

        public static Lobby GetLobby()
        {
            if (lobby == null)
            {
                CheckPlugin();
                lobby = new Lobby(
                    new LobbyApi(
                        httpClient,
                        Config, // baseUrl==LobbyServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    lobby = null;
                    lobby = new Lobby(
                        new LobbyApi(
                            httpClient,
                            Config, // baseUrl==LobbyServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    lobby.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };

                lobby.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            }

            return lobby;
        }

        public static Session GetSession()
        {
            if (_session == null)
            {
                CheckPlugin();
                _session = new Session(
                    new SessionApi(
                        httpClient,
                        Config, // baseUrl==SessionServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    _session = null;
                    _session = new Session(
                        new SessionApi(
                            httpClient,
                            Config, // baseUrl==SessionServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    _session.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };

                _session.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            }

            return _session;
        }

        public static MatchmakingV2 GetMatchmaking()
        {
            if (_matchmakingV2 == null)
            {
                CheckPlugin();
                _matchmakingV2 = new MatchmakingV2(
                    new MatchmakingV2Api(
                        httpClient,
                        Config, // baseUrl==MatchmakingV2ServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    _matchmakingV2 = null;
                    _matchmakingV2 = new MatchmakingV2(
                        new MatchmakingV2Api(
                            httpClient,
                            Config, // baseUrl==MatchmakingV2ServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    _matchmakingV2.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };

                _matchmakingV2.SetPredefinedEventScheduler(ref predefinedEventScheduler);
            }

            return _matchmakingV2;
        }

        public static CloudStorage GetCloudStorage()
        {
            if (cloudStorage == null)
            {
                CheckPlugin();
                cloudStorage = new CloudStorage(
                    new CloudStorageApi(
                        httpClient,
                        Config,
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    cloudStorage = null;
                    cloudStorage = new CloudStorage(
                        new CloudStorageApi(
                            httpClient,
                            Config,
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return cloudStorage;
        }

        public static GameProfiles GetGameProfiles()
        {
            if (gameProfiles == null)
            {
                CheckPlugin();
                gameProfiles = new GameProfiles(
                    new GameProfilesApi(
                        httpClient,
                        Config, // baseUrl==GameProfileServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    gameProfiles = null;
                    gameProfiles = new GameProfiles(
                        new GameProfilesApi(
                            httpClient,
                            Config, // baseUrl==GameProfileServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return gameProfiles;
        }

        public static Entitlement GetEntitlement()
        {
            if (entitlement == null)
            {
                CheckPlugin();
                entitlement = new Entitlement(
                    new EntitlementApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    entitlement = null;
                    entitlement = new Entitlement(
                        new EntitlementApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return entitlement;
        }

        public static Fulfillment GetFulfillment()
        {
            if (fulfillment == null)
            {
                CheckPlugin();
                fulfillment = new Fulfillment(
                    new FulfillmentApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    fulfillment = null;
                    fulfillment = new Fulfillment(
                        new FulfillmentApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            fulfillment.SetPredefinedEventScheduler(ref predefinedEventScheduler);

            return fulfillment;
        }

        public static Statistic GetStatistic()
        {
            if (statistic == null)
            {
                CheckPlugin();
                statistic = new Statistic(
                    new StatisticApi(
                        httpClient,
                        Config, // baseUrl==StatisticServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    statistic = null;
                    statistic = new Statistic(
                        new StatisticApi(
                            httpClient,
                            Config, // baseUrl==StatisticServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return statistic;
        }

        public static QosManager GetQos()
        {
            if (_qosManager == null)
            {
                CheckPlugin();
                _qosManager = new QosManager(
                    new QosManagerApi(
                        httpClient,
                        Config, // baseUrl==QosManagerServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    _qosManager = null;
                    _qosManager = new QosManager(
                        new QosManagerApi(
                            httpClient,
                            Config, // baseUrl==QosManagerServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return _qosManager;
        }

        public static Agreement GetAgreement()
        {
            if (agreement == null)
            {
                CheckPlugin();
                agreement = new Agreement(
                    new AgreementApi(
                        httpClient,
                        Config, // baseUrl==AgreementServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    agreement = null;
                    agreement = new Agreement(
                        new AgreementApi(
                            httpClient,
                            Config, // baseUrl==AgreementServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return agreement;
        }

        public static Leaderboard GetLeaderboard()
        {
            if (leaderboard == null)
            {
                CheckPlugin();
                leaderboard = new Leaderboard(
                    new LeaderboardApi(
                        httpClient,
                        Config, // baseUrl==LeaderboardServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    leaderboard = null;
                    leaderboard = new Leaderboard(
                        new LeaderboardApi(
                            httpClient,
                            Config, // baseUrl==LeaderboardServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return leaderboard;
        }

        public static CloudSave GetCloudSave()
        {
            if (cloudSave == null)
            {
                CheckPlugin();
                cloudSave = new CloudSave(
                    new CloudSaveApi(
                        httpClient,
                        Config, // baseUrl==CloudSaveServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    cloudSave = null;
                    cloudSave = new CloudSave(
                        new CloudSaveApi(
                            httpClient,
                            Config, // baseUrl==CloudSaveServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return cloudSave;
        }

        public static GameTelemetry GetGameTelemetry()
        {
            if (gameTelemetry == null)
            {
                CheckPlugin();
                gameTelemetry = new GameTelemetry(
                    new GameTelemetryApi(
                        httpClient,
                        Config, // baseUrl==GameTelemetryServerUrl 
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    gameTelemetry = null;
                    gameTelemetry = new GameTelemetry(
                        new GameTelemetryApi(
                            httpClient,
                            Config, // baseUrl==GameTelemetryServerUrl 
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return gameTelemetry;
        }

        public static Achievement GetAchievement()
        {
            if (achievement == null)
            {
                CheckPlugin();
                achievement = new Achievement(
                    new AchievementApi(
                        httpClient,
                        Config, // baseUrl==AchievementServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                achievement.SetPredefinedEventScheduler(ref predefinedEventScheduler);

                configReset += () =>
                {
                    achievement = null;
                    achievement = new Achievement(
                        new AchievementApi(
                            httpClient,
                            Config, // baseUrl==AchievementServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    achievement.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };
            }

            return achievement;
        }

        public static Group GetGroup()
        {
            if (group == null)
            {
                CheckPlugin();
                group = new Group(
                    new GroupApi(
                        httpClient,
                        Config, // baseUrl==GroupServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                group.SetPredefinedEventScheduler(ref predefinedEventScheduler);

                configReset += () =>
                {
                    group = null;
                    group = new Group(
                        new GroupApi(
                            httpClient,
                            Config, // baseUrl==GroupServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    group.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };
            }

            return group;
        }

        public static UGC GetUgc()
        {
            if (ugc == null)
            {
                CheckPlugin();
                ugc = new UGC(
                    new UGCApi(
                        httpClient,
                        Config, // baseUrl==UGCServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    ugc = null;
                    ugc = new UGC(
                        new UGCApi(
                            httpClient,
                            Config, // baseUrl==UGCServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return ugc;
        }

        public static Reporting GetReporting()
        {
            if (reporting == null)
            {
                CheckPlugin();
                reporting = new Reporting(
                    new ReportingApi(
                        httpClient,
                        Config, // baseUrl==ReportingServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    reporting = null;
                    reporting = new Reporting(
                        new ReportingApi(
                            httpClient,
                            Config, // baseUrl==ReportingServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return reporting;
        }

        public static SeasonPass GetSeasonPass()
        {
            if (seasonPass == null)
            {
                CheckPlugin();
                seasonPass = new SeasonPass(
                    new SeasonPassApi(
                        httpClient,
                        Config, // baseUrl==SeasonPassServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                seasonPass.SetPredefinedEventScheduler(ref predefinedEventScheduler);

                configReset += () =>
                {
                    seasonPass = null;
                    seasonPass = new SeasonPass(
                        new SeasonPassApi(
                            httpClient,
                            Config, // baseUrl==SeasonPassServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    seasonPass.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };
            }

            return seasonPass;
        }

        public static SessionBrowser GetSessionBrowser()
        {
            if (sessionBrowser == null)
            {
                CheckPlugin();
                sessionBrowser = new SessionBrowser(
                    new SessionBrowserApi(
                        httpClient,
                        Config, // baseUrl==SessionBrowserServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                sessionBrowser.SetPredefinedEventScheduler(ref predefinedEventScheduler);

                configReset += () =>
                {
                    sessionBrowser = new SessionBrowser(
                        new SessionBrowserApi(
                            httpClient,
                            Config, // baseUrl==SessionBrowserServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);

                    seasonPass.SetPredefinedEventScheduler(ref predefinedEventScheduler);
                };
            }
            return sessionBrowser;
        }

        public static TurnManager GetTurnManager()
        {
            if (turnManager == null)
            {
                CheckPlugin();
                turnManager = new TurnManager(
                    new TurnManagerApi(
                        httpClient,
                        Config, // baseUrl==TurnManagerServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    turnManager = new TurnManager(
                        new TurnManagerApi(
                            httpClient,
                            Config, // baseUrl==TurnManagerServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }
            return turnManager;
        }

        public static Miscellaneous GetMiscellaneous()
        {
            if (miscellaneous == null)
            {
                CheckPlugin();
                miscellaneous = new Miscellaneous(
                    new MiscellaneousApi(
                        httpClient,
                        Config, // baseUrl==BasicServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    miscellaneous = null;
                    miscellaneous = new Miscellaneous(
                        new MiscellaneousApi(
                            httpClient,
                            Config, // baseUrl==BasicServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return miscellaneous;
        }

        public static Gdpr GetGdpr()
        {
            if (reporting == null)
            {
                CheckPlugin();
                gdpr = new Gdpr(
                    new GdprApi(
                        httpClient,
                        Config, // baseUrl==GdprServerUrl
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    gdpr = null;
                    gdpr = new Gdpr(
                        new GdprApi(
                            httpClient,
                            Config, // baseUrl==GdprServerUrl
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return gdpr;
        }

        public static PresenceBroadcastEvent GetPresenceBroadcastEvent()
        {
            if (presenceBroadcastEvent == null)
            {
                CheckPlugin();
                presenceBroadcastEvent = new PresenceBroadcastEvent(
                new PresenceBroadcastEventApi(
                    httpClient,
                    Config,
                    activeSession),
                activeSession,
                coroutineRunner);

                configReset += () =>
                {
                    presenceBroadcastEvent = null;
                    presenceBroadcastEvent = new PresenceBroadcastEvent(
                        new PresenceBroadcastEventApi(
                            httpClient,
                            Config,
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return presenceBroadcastEvent;
        }

        public static PresenceBroadcastEventScheduler GetPresenceBroadcastEventScheduler()
        {
            if (presenceBroadcastEventScheduler == null)
            {
                CheckPlugin();

                PresenceBroadcastEvent presenceBroadcastEvent = GetPresenceBroadcastEvent();
                presenceBroadcastEventScheduler = new PresenceBroadcastEventScheduler(presenceBroadcastEvent);

                configReset += () =>
                {
                    bool presenceBroadcastEventJobEnabled = false;
                    if (presenceBroadcastEventScheduler != null)
                    {
                        presenceBroadcastEventJobEnabled = presenceBroadcastEventScheduler.IsPresenceBroadcastEventJobEnabled;
                        presenceBroadcastEventScheduler.SetPresenceBroadcastEventEnabled(false);
                    }

                    presenceBroadcastEventScheduler = null;
                    presenceBroadcastEventScheduler = new PresenceBroadcastEventScheduler(presenceBroadcastEvent);

                    if (presenceBroadcastEventJobEnabled)
                    {
                        presenceBroadcastEventScheduler.SetPresenceBroadcastEventEnabled(true);
                    }
                };
            }

            return presenceBroadcastEventScheduler;
        }

        public static AnalyticsService GetAnalyticService()
        {
            CheckPlugin();
            return analyticsService;
        }

        public static GameStandardAnalyticsClientService GetGameStandardAnalyticsService()
        {
            CheckPlugin();
            return gameStandardAnalyticsService;
        }

        internal static void CacheGameStandardAnalytics(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsClientService gameStandardService, string environment, bool async)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetSaveAsync(async);
                gameStandardService.CacheEvents(cacheImp, environment);
            }
        }

        internal static void LoadGameStandardAnalyticsCache(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsClientService gameStandardService, string environment)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetLoadAsync(true);
                gameStandardService.LoadCachedEvent(cacheImp, environment);
            }
        }

        private static AnalyticsService CreateAnalytics(Config newSdkConfig, IHttpClient httpClient, CoroutineRunner coroutineRunner, UserSession userSession)
        {
            analyticsService = new AnalyticsService(
            new AnalyticsApi(
                httpClient,
                newSdkConfig,
                userSession),
            userSession,
            coroutineRunner);

            return analyticsService;
        }

        public static HeartBeat GetHeartBeat()
        {
            if (heartBeat == null)
            {
                CheckPlugin();
                heartBeat = new HeartBeat(
                    new HeartBeatApi(
                        httpClient,
                        Config,
                        activeSession));
                ProvideHeartbeatData(heartBeat);

                configReset += () =>
                {
                    bool isHeartBeatJobEnabled = false;
                    if (heartBeat != null)
                    {
                        isHeartBeatJobEnabled = heartBeat.IsHeartBeatJobEnabled;
                        heartBeat.SetHeartBeatEnabled(false);
                    }
                    heartBeat = null;
                    heartBeat = new HeartBeat(
                        new HeartBeatApi(
                            httpClient,
                            Config,
                            activeSession));
                    ProvideHeartbeatData(heartBeat);
                    if (isHeartBeatJobEnabled)
                    {
                        heartBeat.SetHeartBeatEnabled(true);
                    }
                };
            }

            return heartBeat;
        }

        public static StoreDisplay GetStoreDisplay()
        {
            if (storeDisplay == null)
            {
                CheckPlugin();
                storeDisplay = new StoreDisplay(
                    new StoreDisplayApi(
                        httpClient,
                        Config,
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    storeDisplay = null;
                    storeDisplay = new StoreDisplay(
                        new StoreDisplayApi(
                            httpClient,
                            Config,
                            activeSession),
                        activeSession,
                        coroutineRunner);
                };
            }

            return storeDisplay;
        }

        public static BinaryCloudSave GetBinaryCloudSave()
        {
            if (binaryCloudSave == null)
            {
                CheckPlugin();
                binaryCloudSave = new BinaryCloudSave(
                    new BinaryCloudSaveApi(
                        httpClient,
                        Config,
                        activeSession),
                    activeSession,
                    coroutineRunner);

                configReset += () =>
                {
                    binaryCloudSave = null;
                    binaryCloudSave = new BinaryCloudSave(
                    new BinaryCloudSaveApi(
                        httpClient,
                        Config,
                        activeSession),
                    activeSession,
                    coroutineRunner);
                };
            }

            return binaryCloudSave;
        }

        private static void ProvideHeartbeatData(HeartBeat targetHeartbeat)
        {
            string publisherNamespace = Config.PublisherNamespace;
            string customerName = !string.IsNullOrEmpty(Config.CustomerName) ? Config.CustomerName : Config.PublisherNamespace;
            string gameName = Config.Namespace;

            SettingsEnvironment env = AccelByteSDK.Environment != null ? AccelByteSDK.Environment.Current : SettingsEnvironment.Default;
            string envString = string.Empty;
            switch (env)
            {
                case SettingsEnvironment.Development:
                    envString = "dev";
                    break;
                case SettingsEnvironment.Certification:
                    envString = "cert";
                    break;
                case SettingsEnvironment.Production:
                    envString = "prod";
                    break;
                case SettingsEnvironment.Default:
                    envString = "default";
                    break;
            }
            targetHeartbeat.AddSendData(HeartBeat.CustomerNameKey, customerName);
            targetHeartbeat.AddSendData(HeartBeat.PublisherNamespaceKey, publisherNamespace);
            targetHeartbeat.AddSendData(HeartBeat.EnvironmentKey, envString);
            targetHeartbeat.AddSendData(HeartBeat.GameNameKey, gameName);
        }
        public static void ConfigureHttpApi<T>(params object[] args) where T : HttpApiBase
        {
            CheckPlugin();
            gameClient.ConfigureHttpApi<T>(args);
        }

        public static T GetHttpApi<T>() where T : HttpApiBase
        {
            CheckPlugin();
            return gameClient.GetHttpApi<T>();
        }

        private static void ResetApis()
        {
            categories = null;
            items = null;
            orders = null;
            wallet = null;
            userProfiles = null;
            lobby = null;
            cloudStorage = null;
            gameProfiles = null;
            entitlement = null;
            statistic = null;
            _qosManager = null;
            agreement = null;
            leaderboard = null;
            cloudSave = null;
            gameTelemetry = null;
            ugc = null;
            seasonPass = null;
            reward = null;
            sessionBrowser = null;
            turnManager = null;
            configReset = null;
            presenceBroadcastEvent = null;
            gameStandardAnalyticsService = null;
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

        private static void UpdateEnvironment(SettingsEnvironment previousEnvironment, SettingsEnvironment newEnvironment)
        {
            try
            {
                string activePlatform = AccelByteSettingsV2.GetActivePlatform(false);
                var newSettings = RetrieveConfigFromJsonFile(activePlatform, newEnvironment);
                if (settings.SDKConfig.IsRequiredFieldEmpty())
                {
                    newEnvironment = SettingsEnvironment.Default;
                    newSettings = RetrieveConfigFromJsonFile(activePlatform, newEnvironment);
                }
                if (settings.OAuthConfig.IsRequiredFieldEmpty())
                {
                    newSettings = RetrieveConfigFromJsonFile("", newEnvironment);
                }
                settings = newSettings;

                httpClient = null;
                user = null;
                gameClient = null;

                httpClient = CreateHttpClient(settings.OAuthConfig, settings.SDKConfig);
                gameClient = CreateGameClient(settings.OAuthConfig, settings.SDKConfig, httpClient);
                user = CreateUser(settings.SDKConfig, coroutineRunner, httpClient);

                activeSession = user.Session;

                analyticsService = CreateAnalytics(settings.SDKConfig, httpClient, coroutineRunner, activeSession);
                if (predefinedEventScheduler != null)
                {
                    predefinedEventScheduler.SetEventEnabled(false);
                    predefinedEventScheduler.Dispose();
                }
                predefinedEventScheduler = null;
                predefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
                predefinedEventScheduler.SetEventEnabled(settings.SDKConfig.EnablePreDefinedEvent);

                // Cache game standard events
                var oldGameStandardAnalytics = gameStandardAnalyticsService;
                oldGameStandardAnalytics.StopScheduler();

                const bool cacheGameStandardAsync = true;
                CacheGameStandardAnalytics(gameStandardCacheImp, oldGameStandardAnalytics, previousEnvironment.ToString(), cacheGameStandardAsync);

                gameStandardAnalyticsService = null;
                gameStandardAnalyticsService = new GameStandardAnalyticsClientService(
                    analyticsService,
                    activeSession,
                    coroutineRunner
                );
                gameStandardCacheImp.UpdateKey(settings.OAuthConfig.ClientId);
                LoadGameStandardAnalyticsCache(gameStandardCacheImp, gameStandardAnalyticsService, newEnvironment.ToString());

                HttpRequestBuilder.SetNamespace(settings.SDKConfig.Namespace);

                configReset?.Invoke();
            }
            catch (Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }
        }
        #endregion

        public static void ClearEnvironmentChangedEvent()
        {
            CheckPlugin();
        }
    }
}
