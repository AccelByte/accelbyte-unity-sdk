// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using HybridWebSocket;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AccelByte.Api
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class AccelBytePlugin
    {
#if UNITY_EDITOR
        private static Config config;
        private static CoroutineRunner coroutineRunner;
        private static UnityHttpWorker httpWorker;
        private static User user; 
#else
        private static readonly Config config;
        private static readonly CoroutineRunner coroutineRunner;
        private static readonly UnityHttpWorker httpWorker;
        private static readonly User user;
#endif
        private static Categories categories;
        private static Items items;
        private static Orders orders;
        private static Wallet wallet;
        private static UserProfiles userProfiles;
        private static Lobby lobby;
        private static CloudStorage cloudStorage;
        private static GameProfiles gameProfiles;
        private static Entitlement entitlement;
        private static Statistic statistic;
        private static Qos qos;
        private static Agreement agreement;
        private static Leaderboard leaderboard;
        private static CloudSave cloudSave;
        private static GameTelemetry gameTelemetry;
        private static Achievement achievement;

        public static Config Config { get { return AccelBytePlugin.config; } }

        static AccelBytePlugin()
        {
#if UNITY_EDITOR // Handle an unexpected behaviour if Domain Reload (experimental) is disabled
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Init();
                }
            };
        }

        private static void Init()
        {
#endif
#if (UNITY_WEBGL || UNITY_PS4 || UNITY_XBOXONE || UNITY_SWITCH) && !UNITY_EDITOR
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
            qos = null;
            agreement = null;
            leaderboard = null;
            cloudSave = null;
            gameTelemetry = null;

            var configFile = Resources.Load("AccelByteSDKConfig");

            if (configFile == null)
            {
                throw new Exception("'AccelByteSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeJsonText = ((TextAsset) configFile).text;

            AccelBytePlugin.config = wholeJsonText.ToObject<Config>();
            AccelBytePlugin.config.Expand();
            AccelBytePlugin.coroutineRunner = new CoroutineRunner();
            AccelBytePlugin.httpWorker = new UnityHttpWorker();
            ILoginSession loginSession = new LoginSession(
                    AccelBytePlugin.config.LoginServerUrl,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.config.ClientId,
                    AccelBytePlugin.config.ClientSecret,
                    AccelBytePlugin.config.RedirectUri,
                    AccelBytePlugin.httpWorker,
                    AccelBytePlugin.coroutineRunner, AccelBytePlugin.config.UseSessionManagement);


            AccelBytePlugin.user = new User(
                loginSession,
                new UserAccount(
                    AccelBytePlugin.config.IamServerUrl,
                    AccelBytePlugin.config.Namespace,
                    loginSession,
                    AccelBytePlugin.httpWorker),
                AccelBytePlugin.coroutineRunner,
                AccelBytePlugin.config.UseSessionManagement);

            ServicePointManager.ServerCertificateValidationCallback = AccelBytePlugin.OnCertificateValidated;
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
                        bool chainIsValid = chain.Build((X509Certificate2) certificate);

                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }

            return isOk;
        }

        public static User GetUser()
        {
            return AccelBytePlugin.user;
        }

        public static UserProfiles GetUserProfiles()
        {
            if (AccelBytePlugin.userProfiles == null)
            {
                AccelBytePlugin.userProfiles = new UserProfiles(
                    new UserProfilesApi(AccelBytePlugin.config.BasicServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.userProfiles;
        }

        public static Categories GetCategories()
        {
            if (AccelBytePlugin.categories == null)
            {
                AccelBytePlugin.categories = new Categories(
                    new CategoriesApi(AccelBytePlugin.config.PlatformServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.categories;
        }

        public static Items GetItems()
        {
            if (AccelBytePlugin.items == null)
            {
                AccelBytePlugin.items = new Items(
                    new ItemsApi(AccelBytePlugin.config.PlatformServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.items;
        }

        public static Orders GetOrders()
        {
            if (AccelBytePlugin.orders == null)
            {
                AccelBytePlugin.orders = new Orders(
                    new OrdersApi(AccelBytePlugin.config.PlatformServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.orders;
        }

        public static Wallet GetWallet()
        {
            if (AccelBytePlugin.wallet == null)
            {
                AccelBytePlugin.wallet = new Wallet(
                    new WalletApi(AccelBytePlugin.config.PlatformServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.wallet;
        }

        public static Lobby GetLobby()
        {
            if (AccelBytePlugin.lobby == null)
            {
                AccelBytePlugin.lobby = new Lobby(
                    AccelBytePlugin.config.LobbyServerUrl,
                    new WebSocket(),
                    new LobbyApi(AccelBytePlugin.config.BaseUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.lobby;
        }

        public static CloudStorage GetCloudStorage()
        {
            if (AccelBytePlugin.cloudStorage == null)
            {
                AccelBytePlugin.cloudStorage = new CloudStorage(
                    new CloudStorageApi(AccelBytePlugin.config.CloudStorageServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.cloudStorage;
        }

        public static GameProfiles GetGameProfiles()
        {
            if (AccelBytePlugin.gameProfiles == null)
            {
                AccelBytePlugin.gameProfiles = new GameProfiles(
                    new GameProfilesApi(AccelBytePlugin.config.GameProfileServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.gameProfiles;
        }
        public static Entitlement GetEntitlement()
        {
            if (AccelBytePlugin.entitlement == null)
            {
                AccelBytePlugin.entitlement = new Entitlement(
                    new EntitlementApi(AccelBytePlugin.config.PlatformServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.entitlement;
        }

        public static Statistic GetStatistic()
        {
            if (AccelBytePlugin.statistic == null)
            {
                AccelBytePlugin.statistic = new Statistic(
                    new StatisticApi(AccelBytePlugin.config.StatisticServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.statistic;
        }

        public static Qos GetQos()
        {
            if (AccelBytePlugin.qos == null)
            {
                AccelBytePlugin.qos = new Qos(
                    new QosManagerApi(AccelBytePlugin.config.QosManagerServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.qos;
        }

        public static Agreement GetAgreement()
        {
            if (AccelBytePlugin.agreement == null)
            {
                AccelBytePlugin.agreement = new Agreement(
                    new AgreementApi(AccelBytePlugin.Config.AgreementServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.agreement;
        }

        public static Leaderboard GetLeaderboard()
        {
            if (AccelBytePlugin.leaderboard == null)
            {
                AccelBytePlugin.leaderboard = new Leaderboard(
                    new LeaderboardApi(AccelBytePlugin.Config.LeaderboardServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.leaderboard;
        }
        
        public static CloudSave GetCloudSave()
        {
            if (AccelBytePlugin.cloudSave == null)
            {
                AccelBytePlugin.cloudSave = new CloudSave(
                    new CloudSaveApi(AccelBytePlugin.config.CloudSaveServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.cloudSave;
        }
        
        public static GameTelemetry GetGameTelemetry()
        {
            if (AccelBytePlugin.gameTelemetry == null)
            {
                AccelBytePlugin.gameTelemetry = new GameTelemetry(
                    new GameTelemetryApi(AccelBytePlugin.config.GameTelemetryServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.gameTelemetry;
        }

        public static Achievement GetAchievement()
        {
            if (AccelBytePlugin.achievement == null)
            {
                AccelBytePlugin.achievement = new Achievement(
                    new AchievementApi(AccelBytePlugin.config.AchievementServerUrl, AccelBytePlugin.httpWorker),
                    AccelBytePlugin.user.Session,
                    AccelBytePlugin.config.Namespace,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.achievement;
        }
    }
}