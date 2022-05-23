// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
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
        private static OAuthConfig oAuthConfig;
        private static Config config;
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
        #endregion /Modules with ApiBase

        private static bool initialized = false;
        private static SettingsEnvironment activeEnvironment = SettingsEnvironment.Default;
        internal static event Action configReset;
        public static event Action<SettingsEnvironment> environmentChanged;

        internal static OAuthConfig OAuthConfig
        {
            get
            {
                CheckPlugin();
                return oAuthConfig;
            }
        }

        public static Config Config
        {
            get
            {
                CheckPlugin();
                return config;
            }
        }

        static AccelBytePlugin()
        {
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    initialized = false;

                    ResetApis();
                }
            };
#endif
        }

        internal static void Initialize(Config inConfig = null, OAuthConfig inOAuthConfig = null)
        {
            ResetApis();
            initialized = true;
            string activePlatform = GetActivePlatform();

            if ( inConfig == null && inOAuthConfig == null)
            {
                RetrieveConfigFromJsonFile(activePlatform);
                if (oAuthConfig.IsRequiredFieldEmpty())
                {
                    RetrieveConfigFromJsonFile();
                }
            }
            else
            {
                config = inConfig;
                oAuthConfig = inOAuthConfig;
            }

            oAuthConfig.CheckRequiredField();
            oAuthConfig.Expand();
            config.CheckRequiredField();
            config.Expand();

            coroutineRunner = new CoroutineRunner();

            InitHttpClient();
            InitGameClient();
            InitUser();

            ServicePointManager.ServerCertificateValidationCallback = OnCertificateValidated;
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

        private static void RetrieveConfigFromJsonFile(string platform = "")
        {
            var oAuthFile = Resources.Load("AccelByteSDKOAuthConfig" + platform);
            var configFile = Resources.Load("AccelByteSDKConfig");

            if (oAuthFile == null)
            {
                oAuthFile = Resources.Load("AccelByteSDKOAuthConfig");
                if (oAuthFile == null)
                {
                    throw new Exception("'AccelByteSDKOAuthConfig.json' isn't found in the Project/Assets/Resources directory");
                }
            }

            if (configFile == null)
            {
                throw new Exception("'AccelByteSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeOAuthJsonText = ((TextAsset)oAuthFile).text;
            string wholeJsonText = ((TextAsset)configFile).text;

            MultiOAuthConfigs multiOAuthConfigs = wholeOAuthJsonText.ToObject<MultiOAuthConfigs>();    
            MultiConfigs multiConfigs = wholeJsonText.ToObject<MultiConfigs>();
            if(multiOAuthConfigs == null)
            {
                multiOAuthConfigs = new MultiOAuthConfigs();
            }
            if(multiConfigs == null)
            {
                multiConfigs = new MultiConfigs();
            }
            multiOAuthConfigs.Expand();
            multiConfigs.Expand();
      
            switch (activeEnvironment)
            {
                case SettingsEnvironment.Development:
                    AccelBytePlugin.oAuthConfig = multiOAuthConfigs.Development;
                    AccelBytePlugin.config = multiConfigs.Development; break;
                case SettingsEnvironment.Certification:
                    AccelBytePlugin.oAuthConfig = multiOAuthConfigs.Certification;
                    AccelBytePlugin.config = multiConfigs.Certification; break;
                case SettingsEnvironment.Production:
                    AccelBytePlugin.oAuthConfig = multiOAuthConfigs.Production;
                    AccelBytePlugin.config = multiConfigs.Production; break;
                case SettingsEnvironment.Default:
                default:
                    AccelBytePlugin.oAuthConfig = multiOAuthConfigs.Default;
                    AccelBytePlugin.config = multiConfigs.Default; break;
            }
        }

        private static void InitHttpClient()
        {
            httpClient = new AccelByteHttpClient();
            httpClient.SetCredentials(oAuthConfig.ClientId, oAuthConfig.ClientSecret);
            httpClient.SetBaseUri(new Uri(config.BaseUrl));
        }

        private static void InitGameClient()
        {
            gameClient = new GameClient(oAuthConfig, config, httpClient);
        }

        private static void InitUser()
        {
            var loginSession = new LoginSession(
                config.IamServerUrl,
                config.Namespace,
                config.RedirectUri,
                httpClient,
                coroutineRunner,
                config.UsePlayerPrefs);

            user = new User(
                new UserApi(
                    httpClient,
                    Config,
                    loginSession),
                loginSession,
                coroutineRunner);
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
                lobby = new Lobby(
                    config.LobbyServerUrl,
                    new WebSocket(),
                    new LobbyApi(
                        httpClient, 
                        Config, // baseUrl==LobbyServerUrl
                        session),
                    session,
                    config.Namespace,
                    coroutineRunner);

                configReset += () =>
                {
                    lobby = null;
                    lobby = new Lobby(
                        config.LobbyServerUrl,
                        new WebSocket(),
                        new LobbyApi(
                            httpClient,
                            Config, // baseUrl==LobbyServerUrl
                            session),
                        session,
                        config.Namespace,
                        coroutineRunner);
                };
            }

            return lobby;
        }

        public static CloudStorage GetCloudStorage()
        {
            if (cloudStorage == null)
            {
                CheckPlugin();
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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
                IUserSession session = GetUser().Session;
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

        public static Miscellaneous GetMiscellaneous()
        {
            if (miscellaneous == null)
            {
                CheckPlugin();
                IUserSession session = GetUser().Session;
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
            configReset = null;
            environmentChanged = null;
        }

        public static void SetEnvironment(SettingsEnvironment environment)
        {
            CheckPlugin();
            activeEnvironment = environment;
            string activePlatform = GetActivePlatform();
            RetrieveConfigFromJsonFile(activePlatform);
            if (config.IsRequiredFieldEmpty())
            {
                activeEnvironment = SettingsEnvironment.Default;
                RetrieveConfigFromJsonFile(activePlatform);
            }
            if (oAuthConfig.IsRequiredFieldEmpty())
            {
                RetrieveConfigFromJsonFile();
            }
            oAuthConfig.Expand();
            config.Expand();
            httpClient = null;
            user = null;
            gameClient = null;
            InitHttpClient();
            InitGameClient();
            InitUser();
            if (configReset != null) { configReset.Invoke(); }
            if (environmentChanged != null){ environmentChanged.Invoke(activeEnvironment); }
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

        internal static string GetActivePlatform()
        {
            string activePlatform;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxPlayer:
                    if (Resources.Load("AccelByteSDKOAuthConfig" + PlatformType.Steam.ToString()) != null)
                    {
                        activePlatform = PlatformType.Steam.ToString();
                    }
                    else if (Resources.Load("AccelByteSDKOAuthConfig" + PlatformType.EpicGames.ToString()) != null)
                    {
                        activePlatform = PlatformType.EpicGames.ToString();
                    }
                    else
                    {
                        activePlatform = "";
                    }
                    break;
                case RuntimePlatform.OSXPlayer:
                    activePlatform = PlatformType.Apple.ToString(); break;
                case RuntimePlatform.IPhonePlayer:
                    activePlatform = PlatformType.iOS.ToString(); break;
                case RuntimePlatform.Android:
                    activePlatform = PlatformType.Android.ToString(); break;
                case RuntimePlatform.PS4:
                    activePlatform = PlatformType.PS4.ToString(); break;
#if UNITY_2020_2_OR_NEWER
                case RuntimePlatform.PS5:
                    activePlatform = PlatformType.PS5.ToString(); break;
#endif
                case RuntimePlatform.XBOX360:
                case RuntimePlatform.XboxOne:
                    activePlatform = PlatformType.Live.ToString(); break;
                case RuntimePlatform.Switch:
                    activePlatform = PlatformType.Nintendo.ToString(); break;
#if UNITY_2019_3_OR_NEWER
                case RuntimePlatform.Stadia:
                    activePlatform = PlatformType.Stadia.ToString(); break;
#endif
                default:
                    activePlatform = ""; break;
            }
            return activePlatform;
        }
    }
}
