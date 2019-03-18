// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AccelByte.Core;
using UnityEngine;

namespace AccelByte.Api
{
    public static class AccelBytePlugin
    {
        private static readonly Config config;
        private static readonly AsyncTaskDispatcher taskDispatcher;
        private static readonly CoroutineRunner coroutineRunner;
        private static readonly User user;

        private static Categories categories;
        private static Items items;
        private static Orders orders;
        private static Wallet wallet;
        private static Telemetry telemetry;
        private static UserProfiles userProfiles;
        private static Lobby lobby;
        private static CloudStorage cloudStorage;
        private static GameProfiles gameProfiles;
        private static Entitlements entitlements;

        public static Config Config
        {
            get { return AccelBytePlugin.config; }
        }

        static AccelBytePlugin()
        {
            var configFile = Resources.Load("AccelByteSDKConfig");

            if (configFile == null)
            {
                throw new Exception("'AccelByteSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeJsonText = ((TextAsset) configFile).text;
            
            AccelBytePlugin.config = SimpleJson.SimpleJson.DeserializeObject<Config>(wholeJsonText);
            AccelBytePlugin.config.Expand();
            AccelBytePlugin.taskDispatcher = new AsyncTaskDispatcher();
            AccelBytePlugin.coroutineRunner = new CoroutineRunner();
            var authApi = new AuthenticationApi(AccelBytePlugin.config.IamServerUrl);
            
            AccelBytePlugin.user = new User(
                authApi,
                new UserApi(AccelBytePlugin.config.IamServerUrl),
                AccelBytePlugin.config.Namespace,
                AccelBytePlugin.config.ClientId,
                AccelBytePlugin.config.ClientSecret,
                AccelBytePlugin.config.RedirectUri,
                AccelBytePlugin.taskDispatcher,
                AccelBytePlugin.coroutineRunner);

            ServicePointManager.ServerCertificateValidationCallback = AccelBytePlugin.OnCertificateValidated;
        }

        private static bool OnCertificateValidated(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
                    AccelBytePlugin.Config.PublisherNamespace,
                    new UserProfilesApi(AccelBytePlugin.config.BasicServerUrl),
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher, 
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.userProfiles;
        }

        public static Categories GetCategories()
        {
            if (AccelBytePlugin.categories == null)
            {
                AccelBytePlugin.categories = new Categories(
                    new CategoriesApi(AccelBytePlugin.config.PlatformServerUrl), 
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher, 
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.categories;
        }

        public static Items GetItems()
        {
            if (AccelBytePlugin.items == null)
            {
                AccelBytePlugin.items = new Items(
                    new ItemsApi(AccelBytePlugin.config.PlatformServerUrl), 
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher, 
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.items;
        }

        public static Orders GetOrders()
        {
            if (AccelBytePlugin.orders == null)
            {
                AccelBytePlugin.orders = new Orders(
                    new OrdersApi(AccelBytePlugin.config.PlatformServerUrl),
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.orders;
        }

        public static Wallet GetWallet()
        {
            if (AccelBytePlugin.wallet == null)
            {
                AccelBytePlugin.wallet = new Wallet(
                    AccelBytePlugin.config.Namespace,
                    new WalletApi(AccelBytePlugin.config.PlatformServerUrl),
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.wallet;
        }


        public static Telemetry GetTelemetry()
        {
            if (AccelBytePlugin.telemetry == null)
            {
                AccelBytePlugin.telemetry = new Telemetry(
                    new TelemetryApi(AccelBytePlugin.config.TelemetryServerUrl), 
                    AccelBytePlugin.user,
                    AccelBytePlugin.config.ClientId, 
                    AccelBytePlugin.taskDispatcher, 
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.telemetry;
        }

        public static Lobby GetLobby()
        {
            if (AccelBytePlugin.lobby == null)
            {
                AccelBytePlugin.lobby = new Lobby(AccelBytePlugin.config.LobbyServerUrl, 
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.lobby;
        }

        public static CloudStorage GetCloudStorage()
        {
            if (AccelBytePlugin.cloudStorage == null)
            {
                AccelBytePlugin.cloudStorage = new CloudStorage(
                    new CloudStorageApi(AccelBytePlugin.config.CloudStorageServerUrl), 
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher, 
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.cloudStorage;
        }

                public static GameProfiles GetGameProfiles()
                {
                    if (AccelBytePlugin.gameProfiles == null)
                    {
                        AccelBytePlugin.gameProfiles = new GameProfiles(
                            new GameProfilesApi(AccelBytePlugin.config.GameProfileServerUrl),
                            AccelBytePlugin.user,
                            AccelBytePlugin.taskDispatcher,
                            AccelBytePlugin.coroutineRunner);
                    }

                    return AccelBytePlugin.gameProfiles;
                }             

        public static Entitlements GetEntitlements()
        {
            if (AccelBytePlugin.entitlements == null)
            {
                AccelBytePlugin.entitlements = new Entitlements(
                    AccelBytePlugin.config.Namespace,
                    new EntitlementApi(AccelBytePlugin.config.PlatformServerUrl),
                    AccelBytePlugin.user,
                    AccelBytePlugin.taskDispatcher,
                    AccelBytePlugin.coroutineRunner);
            }

            return AccelBytePlugin.entitlements;
        }
    }
}
