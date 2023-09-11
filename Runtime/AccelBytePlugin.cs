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
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class AccelBytePlugin
    {
        private static AccelByteSettingsV2 settings;
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
        private static PresenceBroadcastEvent presenceBroadcastEvent;
        private static PresenceBroadcastEventController presenceBroadcastEventController;
        private static AnalyticsService analyticsService;
        private static Gdpr gdpr;
        #endregion /Modules with ApiBase

        private static bool initialized = false;
        internal static event Action configReset;
        public static event Action<SettingsEnvironment> environmentChanged;
        private static IHttpRequestSender defaultHttpSender = null;

        private static PredefinedEventScheduler predefinedEventScheduler;

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
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    Reset();
                }
            };
#endif
        }

        internal static void Reset()
        {
            initialized = false;

            ResetApis();

            if (heartBeat != null)
            {
                heartBeat.SetHeartBeatEnabled(false);
                heartBeat = null;
            }

            if (presenceBroadcastEventController != null)
            {
                presenceBroadcastEventController.SetPresenceBroadcastEventEnabled(false);
                presenceBroadcastEventController.Dispose();
                presenceBroadcastEventController = null;
            }

            if (predefinedEventScheduler != null)
            {
                predefinedEventScheduler.SetEventEnabled(false);
                predefinedEventScheduler.Dispose();
                predefinedEventScheduler = null;
            }
        }

        internal static void Initialize()
        {
            Initialize(null, null);

            ValidateCompatibility();
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

            AccelByteDebug.SetEnableLogging(settings.SDKConfig.EnableDebugLog);
            AccelByteLogType logTypeEnum;
            if (Enum.TryParse(settings.SDKConfig.DebugLogFilter, out logTypeEnum))
            {
                AccelByteDebug.SetFilterLogType(logTypeEnum);
            }
            else
            {
                AccelByteDebug.SetFilterLogType(AccelByteLogType.Verbose);
            }

            httpClient = CreateHttpClient(settings.OAuthConfig, settings.SDKConfig);
            gameClient = CreateGameClient(settings.OAuthConfig, settings.SDKConfig, httpClient);
            user = CreateUser(settings.SDKConfig, coroutineRunner, httpClient);
            
            analyticsService = CreateAnalytics(settings.SDKConfig, httpClient, coroutineRunner, user.Session);
            predefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
            predefinedEventScheduler.SetEventEnabled(settings.SDKConfig.EnablePreDefinedEvent);

            HttpRequestBuilder.SetNamespace(settings.SDKConfig.Namespace);
            HttpRequestBuilder.SetGameClientVersion(Application.version);
            HttpRequestBuilder.SetSdkVersion(AccelByteSettingsV2.AccelByteSDKVersion);
            ServicePointManager.ServerCertificateValidationCallback = OnCertificateValidated;
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
                UserSession session = GetUser().Session;
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
                UserSession session = GetUser().Session;
                userProfiles = new UserProfiles(
                    new UserProfilesApi(
                        httpClient,
                        Config, // baseUrl==BasicServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    userProfiles = null;
                    userProfiles = new UserProfiles(
                        new UserProfilesApi(
                            httpClient,
                            Config, // baseUrl==BasicServerUrl 
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return userProfiles;
        }

        public static Categories GetCategories()
        {
            if (categories == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                categories = new Categories(
                    new CategoriesApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    categories = null;
                    categories = new Categories(
                        new CategoriesApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                items = new Items(
                    new ItemsApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    items = null;
                    items = new Items(
                        new ItemsApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                currencies = new Currencies(
                    new CurrenciesApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    currencies = null;
                    currencies = new Currencies(
                        new CurrenciesApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                orders = new Orders(
                    new OrdersApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    orders = null;
                    orders = new Orders(
                        new OrdersApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return orders;
        }

        public static Reward GetReward()
        {
            if (reward == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                reward = new Reward(
                    new RewardApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    reward = null;
                    reward = new Reward(
                        new RewardApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                wallet = new Wallet(
                    new WalletApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    wallet = null;
                    wallet = new Wallet(
                        new WalletApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                lobby = new Lobby(
                    new LobbyApi(
                        httpClient,
                        Config, // baseUrl==LobbyServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    lobby = null;
                    lobby = new Lobby(
                        new LobbyApi(
                            httpClient,
                            Config, // baseUrl==LobbyServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return lobby;
        }

        public static Session GetSession()
        {
            if (_session == null)
            {
                CheckPlugin();
                ISession session = GetUser().Session;
                _session = new Session(
                    new SessionApi(
                        httpClient,
                        Config, // baseUrl==SessionServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    _session = null;
                    _session = new Session(
                        new SessionApi(
                            httpClient,
                            Config, // baseUrl==SessionServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return _session;
        }

        public static MatchmakingV2 GetMatchmaking()
        {
            if (_matchmakingV2 == null)
            {
                CheckPlugin();
                ISession session = GetUser().Session;
                _matchmakingV2 = new MatchmakingV2(
                    new MatchmakingV2Api(
                        httpClient,
                        Config, // baseUrl==MatchmakingV2ServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    _matchmakingV2 = null;
                    _matchmakingV2 = new MatchmakingV2(
                        new MatchmakingV2Api(
                            httpClient,
                            Config, // baseUrl==MatchmakingV2ServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return _matchmakingV2;
        }

        public static CloudStorage GetCloudStorage()
        {
            if (cloudStorage == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                cloudStorage = new CloudStorage(
                    new CloudStorageApi(
                        httpClient,
                        Config,
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    cloudStorage = null;
                    cloudStorage = new CloudStorage(
                        new CloudStorageApi(
                            httpClient,
                            Config,
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                gameProfiles = new GameProfiles(
                    new GameProfilesApi(
                        httpClient,
                        Config, // baseUrl==GameProfileServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    gameProfiles = null;
                    gameProfiles = new GameProfiles(
                        new GameProfilesApi(
                            httpClient,
                            Config, // baseUrl==GameProfileServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                entitlement = new Entitlement(
                    new EntitlementApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    entitlement = null;
                    entitlement = new Entitlement(
                        new EntitlementApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                fulfillment = new Fulfillment(
                    new FulfillmentApi(
                        httpClient,
                        Config, // baseUrl==PlatformServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    fulfillment = null;
                    fulfillment = new Fulfillment(
                        new FulfillmentApi(
                            httpClient,
                            Config, // baseUrl==PlatformServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return fulfillment;
        }

        public static Statistic GetStatistic()
        {
            if (statistic == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                statistic = new Statistic(
                    new StatisticApi(
                        httpClient,
                        Config, // baseUrl==StatisticServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    statistic = null;
                    statistic = new Statistic(
                        new StatisticApi(
                            httpClient,
                            Config, // baseUrl==StatisticServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                _qosManager = new QosManager(
                    new QosManagerApi(
                        httpClient,
                        Config, // baseUrl==QosManagerServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    _qosManager = null;
                    _qosManager = new QosManager(
                        new QosManagerApi(
                            httpClient,
                            Config, // baseUrl==QosManagerServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                agreement = new Agreement(
                    new AgreementApi(
                        httpClient,
                        Config, // baseUrl==AgreementServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    agreement = null;
                    agreement = new Agreement(
                        new AgreementApi(
                            httpClient,
                            Config, // baseUrl==AgreementServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                leaderboard = new Leaderboard(
                    new LeaderboardApi(
                        httpClient,
                        Config, // baseUrl==LeaderboardServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    leaderboard = null;
                    leaderboard = new Leaderboard(
                        new LeaderboardApi(
                            httpClient,
                            Config, // baseUrl==LeaderboardServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                cloudSave = new CloudSave(
                    new CloudSaveApi(
                        httpClient,
                        Config, // baseUrl==CloudSaveServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    cloudSave = null;
                    cloudSave = new CloudSave(
                        new CloudSaveApi(
                            httpClient,
                            Config, // baseUrl==CloudSaveServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                gameTelemetry = new GameTelemetry(
                    new GameTelemetryApi(
                        httpClient,
                        Config, // baseUrl==GameTelemetryServerUrl 
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    gameTelemetry = null;
                    gameTelemetry = new GameTelemetry(
                        new GameTelemetryApi(
                            httpClient,
                            Config, // baseUrl==GameTelemetryServerUrl 
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                achievement = new Achievement(
                    new AchievementApi(
                        httpClient,
                        Config, // baseUrl==AchievementServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    achievement = null;
                    achievement = new Achievement(
                        new AchievementApi(
                            httpClient,
                            Config, // baseUrl==AchievementServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return achievement;
        }

        public static Group GetGroup()
        {
            if (group == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                group = new Group(
                    new GroupApi(
                        httpClient,
                        Config, // baseUrl==GroupServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    group = null;
                    group = new Group(
                        new GroupApi(
                            httpClient,
                            Config, // baseUrl==GroupServerUrl
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return group;
        }

        public static UGC GetUgc()
        {
            if (ugc == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                ugc = new UGC(
                    new UGCApi(
                        httpClient,
                        Config, // baseUrl==UGCServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    ugc = null;
                    ugc = new UGC(
                        new UGCApi(
                            httpClient,
                            Config, // baseUrl==UGCServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                reporting = new Reporting(
                    new ReportingApi(
                        httpClient,
                        Config, // baseUrl==ReportingServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    reporting = null;
                    reporting = new Reporting(
                        new ReportingApi(
                            httpClient,
                            Config, // baseUrl==ReportingServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                seasonPass = new SeasonPass(
                    new SeasonPassApi(
                        httpClient,
                        Config, // baseUrl==SeasonPassServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    seasonPass = null;
                    seasonPass = new SeasonPass(
                        new SeasonPassApi(
                            httpClient,
                            Config, // baseUrl==SeasonPassServerUrl
                            session),
                        session,
                        coroutineRunner);
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
                        GetUser().Session),
                    user.Session,
                    coroutineRunner);
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
                        GetUser().Session),
                    user.Session,
                    coroutineRunner);
            }
            return turnManager;
        }

        public static Miscellaneous GetMiscellaneous()
        {
            if (miscellaneous == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                miscellaneous = new Miscellaneous(
                    new MiscellaneousApi(
                        httpClient,
                        Config, // baseUrl==BasicServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    miscellaneous = null;
                    miscellaneous = new Miscellaneous(
                        new MiscellaneousApi(
                            httpClient,
                            Config, // baseUrl==BasicServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                gdpr = new Gdpr(
                    new GdprApi(
                        httpClient,
                        Config, // baseUrl==GdprServerUrl
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    gdpr = null;
                    gdpr = new Gdpr(
                        new GdprApi(
                            httpClient,
                            Config, // baseUrl==GdprServerUrl
                            session),
                        session,
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
                UserSession session = GetUser().Session;
                presenceBroadcastEvent = new PresenceBroadcastEvent(
                new PresenceBroadcastEventApi(
                    httpClient,
                    Config,
                    session),
                session,
                coroutineRunner);

                configReset += () =>
                {
                    presenceBroadcastEvent = null;
                    presenceBroadcastEvent = new PresenceBroadcastEvent(
                        new PresenceBroadcastEventApi(
                            httpClient,
                            Config,
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return presenceBroadcastEvent;
        }

        public static PresenceBroadcastEventController GetPresenceBroadcastEventController()
        {
            if (presenceBroadcastEventController == null)
            {
                CheckPlugin();

                PresenceBroadcastEvent presenceBroadcastEvent = GetPresenceBroadcastEvent();
                presenceBroadcastEventController = new PresenceBroadcastEventController(presenceBroadcastEvent);

                configReset += () =>
                {
                    bool presenceBroadcastEventJobEnabled = false;
                    if (presenceBroadcastEventController != null)
                    {
                        presenceBroadcastEventController.SetPresenceBroadcastEventEnabled(false);
                    }

                    presenceBroadcastEventController = null;
                    presenceBroadcastEventController = new PresenceBroadcastEventController(presenceBroadcastEvent);

                    presenceBroadcastEventController.SetPresenceBroadcastEventEnabled(Config.EnablePresenceBroadcastEvent);
                };
            }

            return presenceBroadcastEventController;
        }

        public static AnalyticsService GetAnalyticService()
        {
            CheckPlugin();
            return analyticsService;
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
                UserSession session = GetUser().Session;
                heartBeat = new HeartBeat(
                    new HeartBeatApi(
                        httpClient,
                        Config,
                        session));
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
                            session));
                    ProvideHeartbeatData(heartBeat);
                    if (isHeartBeatJobEnabled)
                    {
                        heartBeat.SetHeartBeatEnabled(true);
                    }
                };
            }

            return heartBeat;
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
            presenceBroadcastEventController = null;
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

        private static void UpdateEnvironment(SettingsEnvironment newEnvironment)
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

                AccelByteDebug.SetEnableLogging(settings.SDKConfig.EnableDebugLog);
                AccelByteLogType logTypeEnum;
                if (Enum.TryParse(settings.SDKConfig.DebugLogFilter, out logTypeEnum))
                {
                    AccelByteDebug.SetFilterLogType(logTypeEnum);
                }
                else
                {
                    AccelByteDebug.SetFilterLogType(AccelByteLogType.Verbose);
                }

                httpClient = null;
                user = null;
                gameClient = null;

                httpClient = CreateHttpClient(settings.OAuthConfig, settings.SDKConfig);
                gameClient = CreateGameClient(settings.OAuthConfig, settings.SDKConfig, httpClient);
                user = CreateUser(settings.SDKConfig, coroutineRunner, httpClient);

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
            environmentChanged = null;
        }

        public static StoreDisplay GetStoreDisplay()
        {
            if (storeDisplay == null)
            {
                CheckPlugin();
                UserSession session = GetUser().Session;
                storeDisplay = new StoreDisplay(
                    new StoreDisplayApi(
                        httpClient,
                        Config,
                        session),
                    session,
                    coroutineRunner);

                configReset += () =>
                {
                    storeDisplay = null;
                    storeDisplay = new StoreDisplay(
                        new StoreDisplayApi(
                            httpClient,
                            Config,
                            session),
                        session,
                        coroutineRunner);
                };
            }

            return storeDisplay;
        }
    }
}
