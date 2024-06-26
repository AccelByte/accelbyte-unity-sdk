﻿// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class AccelByteClientRegistry
    {
        internal const string DefaultId = "default";
        private readonly Dictionary<string, ApiClient> clientApiInstances;

        private SettingsEnvironment environment;
        internal Config Config;
        internal OAuthConfig OAuthConfig;

        private PresenceBroadcastEventScheduler presenceEventScheduler;
        private PredefinedEventScheduler predefinedEventScheduler;
        private GameStandardAnalyticsClientService gameStandardAnalyticsService;
        internal AccelByteGameStandardEventCacheImp GameStandardCacheImp;

        private AccelByteNetworkConditioner networkConditioner;
        private AccelByteMessagingSystem messagingSystem;
        private AccelByteNotificationSender notificationSender;
        private AccelByteNotificationBuffer notificationBuffer;

        IHttpClient httpClient;

        private ApiSharedMemory sharedMemory;
        
        internal IHttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = CreateHtppClient();
                }
                return httpClient;
            }
        }

        private GameClient gameClient;
        internal GameClient GameClient
        {
            get
            {
                if (gameClient == null)
                {
                    gameClient = new GameClient(OAuthConfig, Config, HttpClient);
                }
                return gameClient;
            }
        }

        private List<ApiClient> loginUserClientApis;

        private readonly IHttpRequestSenderFactory requestSenderFactory;

        internal AccelByteClientRegistry(SettingsEnvironment environment, Config config, OAuthConfig oAuthConfig, IHttpRequestSenderFactory requestSenderFactory)
        {
            clientApiInstances = new Dictionary<string, ApiClient>();
            loginUserClientApis = new List<ApiClient>();
            this.requestSenderFactory = requestSenderFactory;
            Initialize(environment, config, oAuthConfig);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiClient GetApi(string id = DefaultId)
        {
            // If an existing instance exists here, return it.
            bool hasExistingInstance = clientApiInstances.ContainsKey(id);
            if (hasExistingInstance)
            {
                clientApiInstances.TryGetValue(id, out ApiClient apiBase);
                return apiBase;
            }

            ApiClient newApiClient = CreateNewApi();
            RegisterApi(id, newApiClient);

            return newApiClient;
        }

        public AccelByteNetworkConditioner GetNetworkConditioner()
        {
            return networkConditioner;
        }

        public AccelByteMessagingSystem GetMessagingSystem()
        {
            return messagingSystem;
        }

        public AccelByteNotificationSender GetNotificationSender()
        {
            return notificationSender;
        }

        public PresenceBroadcastEventScheduler GetPresenceBroadcastEvent()
        {
            return presenceEventScheduler;
        }

        public GameStandardAnalyticsClientService GetGameStandardEvents()
        {
            if (gameStandardAnalyticsService == null)
            {
                AnalyticsService analyticsApiWrapper = null;
                if (loginUserClientApis.Count > 0)
                {
                    analyticsApiWrapper = loginUserClientApis[0].GetAnalyticsService();
                }
                gameStandardAnalyticsService = new GameStandardAnalyticsClientService(analyticsApiWrapper, Config);
                LoadGameStandardAnalyticsCache(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString());
            }
            return gameStandardAnalyticsService;
        }

        internal PredefinedEventScheduler GetPredefinedEvents()
        {
            return predefinedEventScheduler;
        }

        private void Initialize(SettingsEnvironment environment, Config config, OAuthConfig oAuthConfig)
        {
            this.Config = config;
            this.OAuthConfig = oAuthConfig;
            this.environment = environment;

            InitializeNetworkConditioner();
            InitializeMessagingSystem();
            InitializeNotificationSender();
            InitializeAnalytics(config);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += OnCertificateValidated;

            sharedMemory = new ApiSharedMemory()
            {
                IdValidator = new Utils.AccelByteIdValidator(),
                PredefinedEventScheduler = predefinedEventScheduler,
                NetworkConditioner =  networkConditioner,
                MessagingSystem = messagingSystem,
                NotificationSender = notificationSender,
            };

            SendSDKInitializedEvent(AccelByteSDK.Version);
        }

        internal void Reset()
        {
            if (clientApiInstances.Count > 0)
            {
                foreach (var keyValue in clientApiInstances)
                {
                    keyValue.Value.Reset();
                }
                clientApiInstances.Clear();
            }

            ClearAnalytics();

            gameClient = null;
            httpClient = null;

            loginUserClientApis.Clear();

            System.Net.ServicePointManager.ServerCertificateValidationCallback -= OnCertificateValidated;
        }

        internal void ChangeEnvironment(SettingsEnvironment newEnvironment, Config config, OAuthConfig oAuthConfig)
        {
            List<string> registryKeysWithActiveHeartbeat = new List<string>();
            if (clientApiInstances.Count > 0)
            {
                const bool autoCreateHeartbeatWrapper = false;
                foreach (var keyValue in clientApiInstances)
                {
                    string registryKey = keyValue.Key;
                    ApiClient registryApiClient = keyValue.Value;

                    HeartBeat heartBeat = registryApiClient.GetHeartBeatService(autoCreateHeartbeatWrapper);
                    bool heartBeatActive = heartBeat != null && heartBeat.IsHeartBeatJobEnabled;
                    if (heartBeatActive)
                    {
                        registryKeysWithActiveHeartbeat.Add(registryKey);
                    }
                }
            }

            Reset();

            Initialize(newEnvironment, config, oAuthConfig);

            foreach (var registryKey in registryKeysWithActiveHeartbeat)
            {
                const bool enableHeartBeat = true;
                GetApi(registryKey).GetHeartBeatService().SetHeartBeatEnabled(enableHeartBeat);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="newApiClient"></param>
        /// <param name="id">Leave empty for "default"; duplicate key names will replace the previous entry</param>
        /// <returns>isSuccess</returns>
        internal bool RegisterApi(string id, ApiClient newApiClient)
        {
            Assert.IsNotNull(newApiClient, "!apiClient @ RegisterApiClient");
            if (string.IsNullOrEmpty(id))
            {
                AccelByteDebug.LogError("!key @ RegisterApiClient");
                return false;
            }

            if(clientApiInstances.ContainsKey(id))
            {
                RemoveApi(id);
            }

            newApiClient.SetSharedMemory(sharedMemory);
            InitializeHeartbeatData(ref newApiClient, Config, environment);

            newApiClient.GetUser().OnLoginSuccess += (loginToken) =>
            {
                loginUserClientApis.Add(newApiClient);
                if(loginUserClientApis.Count == 1)
                {
                    AttachAnalyticsSchedulerApi(newApiClient);
                }
            };

            newApiClient.GetUser().OnLogout += () =>
            {
                int index = loginUserClientApis.FindIndex(apiClient => apiClient == newApiClient);
                if (index >= 0)
                {
                    loginUserClientApis.RemoveAt(index);
                    const int firstLoginUserIndex = 0;
                    if (index == firstLoginUserIndex)
                    {
                        if (loginUserClientApis.Count == 0)
                        {
                            DetachAnalyticsSchedulerApi();
                        }
                        else
                        {
                            AttachAnalyticsSchedulerApi(loginUserClientApis[0]);
                        }
                    }
                }
            };

            clientApiInstances[id] = newApiClient;
            return true;
        }

        internal bool RemoveApi(string id)
        {
            bool success = false;
            if (clientApiInstances.ContainsKey(id))
            {
                ApiClient removedClientApi = clientApiInstances[id];
                removedClientApi.Reset();
                if(loginUserClientApis.Contains(removedClientApi))
                {
                    loginUserClientApis.Remove(removedClientApi);
                }
                success = clientApiInstances.Remove(id);
            }
            return success;
        }

        /// <summary>
        /// Creates (and returns) new ApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal ApiClient CreateNewApi()
        {
            CoroutineRunner coroutineRunner = new CoroutineRunner();

            IHttpClient httpClient = CreateHtppClient();

            string sessionCacheTableName = $"TokenCache/{environment}/TokenData";
            IAccelByteDataStorage dataStorage = new Core.AccelByteDataStorageBinaryFile(AccelByteSDK.FileStream);
            var session = new UserSession(
                    httpClient,
                    coroutineRunner,
                    Config.PublisherNamespace,
                    Config.UsePlayerPrefs,
                    sessionCacheTableName,
                    dataStorage);

            var newApiClient = new ApiClient(session, httpClient, coroutineRunner, Config, OAuthConfig);

            return newApiClient;
        }

        protected virtual IHttpClient CreateHtppClient()
        {
            IHttpRequestSender requestSender = requestSenderFactory.CreateHttpRequestSender();
            AccelByteHttpClient httpClient = new AccelByteHttpClient(requestSender);
            var cacheImplementation = new AccelByteLRUMemoryCacheImplementation<AccelByteCacheItem<AccelByteHttpCacheData>>(Config.MaximumCacheSize);
            httpClient.SetCacheImplementation(cacheImplementation, Config.MaximumCacheLifeTime);
            httpClient.SetCredentials(OAuthConfig.ClientId, OAuthConfig.ClientSecret);
            httpClient.SetBaseUri(new Uri(Config.BaseUrl));
            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.NamespaceHeaderKey, Config.Namespace);
            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.GameClientVersionHeaderKey, UnityEngine.Application.version);
            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.SdkVersionHeaderKey, AccelByteSDK.Version);
            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.FlightIdKey, AccelByteSDK.FlightId);

            return httpClient;
        }

        private void InitializeAnalytics(Config config)
        {
            const AnalyticsService analyticsApiWrapper = null;
            predefinedEventScheduler = CreatePredefinedEventScheduler(analyticsApiWrapper, config);
            presenceEventScheduler = CreatePresenceBroadcastEventScheduler(analyticsApiWrapper, config);
            if(gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.Initialize(analyticsApiWrapper, config);
                LoadGameStandardAnalyticsCache(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString());
            }

            string gameStandardCacheDirectory = GameStandardAnalyticsClientService.DefaultCacheDirectory;
            string gameStandardCacheFileName = GameStandardAnalyticsClientService.DefaultCacheFileName;
            string gameStandardCacheEncryptionKey = OAuthConfig.ClientId;
            if (GameStandardCacheImp == null)
            {
                GameStandardCacheImp = new AccelByteGameStandardEventCacheImp(
                    gameStandardCacheDirectory
                    , gameStandardCacheFileName
                    , gameStandardCacheEncryptionKey
                );
            }
            else
            {
                GameStandardCacheImp.UpdateKey(gameStandardCacheEncryptionKey);
            }
        }

        private void ClearAnalytics()
        {
            presenceEventScheduler.SetEventEnabled(false);
            predefinedEventScheduler.SetEventEnabled(false);

            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.StopScheduler();
                const bool cacheAsync = false;
                CacheGameStandardAnalytics(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString(), cacheAsync);
                gameStandardAnalyticsService.Scheduler.ClearTasks();
            }
        }

        private void AttachAnalyticsSchedulerApi(ApiClient apiClient)
        {
            AnalyticsService analyticsWrapper = apiClient.GetAnalyticsService();
            presenceEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            predefinedEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.SetAnalyticsWrapper(analyticsWrapper);
            }
        }

        private void DetachAnalyticsSchedulerApi()
        {
            const AnalyticsService analyticsWrapper = null;
            presenceEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            predefinedEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.SetAnalyticsWrapper(analyticsWrapper);
            }
        }

        private void CacheGameStandardAnalytics(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsClientService gameStandardService, string environment, bool async)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetSaveAsync(async);
                gameStandardService.CacheEvents(cacheImp, environment);
            }
        }

        private void LoadGameStandardAnalyticsCache(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsClientService gameStandardService, string environment)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetLoadAsync(true);
                gameStandardService.LoadCachedEvent(cacheImp, environment);
            }
        }

        internal void SendSDKInitializedEvent(string sdkVersion)
        {
            var sdkInitializedPayload = new PredefinedSDKInitializedPayload("", sdkVersion);
            var telemetryEvent = new AccelByteTelemetryEvent(sdkInitializedPayload);
            predefinedEventScheduler.SendEvent(telemetryEvent, null);
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

        private static void InitializeHeartbeatData(ref ApiClient apiClient, Config config, SettingsEnvironment environment)
        {
            HeartBeat heartBeatWrapper = apiClient.GetHeartBeatService();

            string publisherNamespace = config.PublisherNamespace;
            string customerName = !string.IsNullOrEmpty(config.CustomerName) ? config.CustomerName : config.PublisherNamespace;
            string gameName = config.Namespace;

            SettingsEnvironment env = environment;
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
            heartBeatWrapper.AddSendData(HeartBeat.CustomerNameKey, customerName);
            heartBeatWrapper.AddSendData(HeartBeat.PublisherNamespaceKey, publisherNamespace);
            heartBeatWrapper.AddSendData(HeartBeat.EnvironmentKey, envString);
            heartBeatWrapper.AddSendData(HeartBeat.GameNameKey, gameName);
        }

        private void InitializeNetworkConditioner()
        {
            networkConditioner = new AccelByteNetworkConditioner();
        }

        private void InitializeMessagingSystem()
        {
            messagingSystem = new AccelByteMessagingSystem();
        }

        private void InitializeNotificationSender()
        {
            notificationSender = new AccelByteNotificationSender(ref messagingSystem);
        }

        private void InitializeNotificationBuffer()
        {
            notificationBuffer = new AccelByteNotificationBuffer();
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
    }
}
