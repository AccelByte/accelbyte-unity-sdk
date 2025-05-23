﻿// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class AccelByteServerRegistry
    {
        internal const string DefaultId = "default";
        private readonly Dictionary<string, ServerApiClient> serverApiClientInstances;

        private SettingsEnvironment environment;
        internal ServerConfig Config;
        internal OAuthConfig OAuthConfig;

        private PredefinedEventScheduler predefinedEventScheduler;
        private GameStandardAnalyticsServerService gameStandardAnalyticsService;
        internal AccelByteGameStandardEventCacheImp GameStandardCacheImp;
        private ServerAMS ams;

        IHttpClient httpClient;
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

        private List<ServerApiClient> loginUserClientApis;

        private readonly IHttpRequestSenderFactory requestSenderFactory;

        private ApiSharedMemory sharedMemory;

        private AccelByteTimeManager timeManager;
        private IFileStream fileStream;
        private readonly CoreHeartBeat coreHeartBeat;

        internal AccelByteTimeManager TimeManager
        {
            get
            {
                if (timeManager == null)
                {
                    timeManager = new AccelByteTimeManager();
                }

                return timeManager;
            }
            set
            {
                timeManager = value;
                sharedMemory.TimeManager = timeManager;
            }
        }

        public IDebugger Logger
        {
            get;
            private set;
        }
        
        private Utils.AccelByteServiceTracker serviceTracker;

        internal AccelByteServerRegistry(
            SettingsEnvironment environment
            , ServerConfig config
            , OAuthConfig oAuthConfig
            , IHttpRequestSenderFactory requestSenderFactory
            , AccelByteTimeManager timeManager
            , CoreHeartBeat coreHeartBeat
            , IFileStream fileStream
            , Utils.AccelByteServiceTracker serviceTracker)
        {
            serverApiClientInstances = new Dictionary<string, ServerApiClient>();
            loginUserClientApis = new List<ServerApiClient>();
            this.requestSenderFactory = requestSenderFactory;
            this.timeManager = timeManager;
            this.Logger = new AccelByteDebuggerV2();
            this.coreHeartBeat = coreHeartBeat;
            this.fileStream = fileStream;
            this.serviceTracker = serviceTracker;
            
            Initialize(environment, config, oAuthConfig);
            UpdateServerTime(ref this.timeManager, CreateHtppClient(), ref Config);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServerApiClient GetApi(string id = DefaultId)
        {
            // If an existing instance exists here, return it.
            bool hasExistingInstance = serverApiClientInstances.ContainsKey(id);
            if (hasExistingInstance)
            {
                serverApiClientInstances.TryGetValue(id, out ServerApiClient apiBase);
                return apiBase;
            }

            ServerApiClient newApiClient = CreateNewApi();
            RegisterApi(id, newApiClient);

            return newApiClient;
        }

        public ServerAMS GetAMS(bool autoCreate = true, bool autoConnect = true)
        {
            if (ams == null && autoCreate)
            {
                var coroutineRunner = new CoroutineRunner();
                ams = CreateAMSConnection(Config, coroutineRunner, autoConnect);
            }
            return ams;
        }

        public GameStandardAnalyticsServerService GetGameStandardEvents()
        {
            if (gameStandardAnalyticsService == null)
            {
                ServerAnalyticsService analyticsApiWrapper = null;
                if (loginUserClientApis.Count > 0)
                {
                    analyticsApiWrapper = loginUserClientApis[0].GetAnalyticsService();
                }
                gameStandardAnalyticsService = new GameStandardAnalyticsServerService(analyticsApiWrapper, Config);
                gameStandardAnalyticsService.Scheduler.SetSharedMemory(ref sharedMemory);
                LoadGameStandardAnalyticsCache(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString());
            }
            return gameStandardAnalyticsService;
        }

        internal void SetAMS(ServerAMS newAMS)
        {
            ams = newAMS;
        }

        internal PredefinedEventScheduler GetPredefinedEvents()
        {
            return predefinedEventScheduler;
        }

        internal void SetLogger(IDebugger newLogger)
        {
            if (sharedMemory != null)
            {
                Logger = newLogger;
                sharedMemory.Logger = newLogger;
                SetupCurrentLogger(Config);
            }
        }

        private void Initialize(SettingsEnvironment environment, ServerConfig config, OAuthConfig oAuthConfig)
        {
            Config = config;
            OAuthConfig = oAuthConfig;
            this.environment = environment;

            if (config != null)
            {
                SetupCurrentLogger(config);
            }

            sharedMemory = new ApiSharedMemory();
            sharedMemory.Logger = Logger;
            sharedMemory.CoreHeartBeat = coreHeartBeat;
            sharedMemory.ServiceTracker = serviceTracker;

            InitializeAnalytics(config);

            sharedMemory.PredefinedEventScheduler = predefinedEventScheduler;
            sharedMemory.IdValidator = new Utils.AccelByteIdValidator();
            sharedMemory.TimeManager = this.TimeManager;
            sharedMemory.TimeManager?.SetLogger(Logger);

            SendSDKInitializedEvent(AccelByteSDK.Version);
        }

        internal void Reset()
        {
            if (serverApiClientInstances.Count > 0)
            {
                foreach (var keyValue in serverApiClientInstances)
                {
                    keyValue.Value.Reset();
                }
                serverApiClientInstances.Clear();
            }

            if(ams != null)
            {
                ams.Disconnect();
            }

            ClearAnalytics();

            httpClient = null;

            loginUserClientApis.Clear();
        }

        internal void ChangeEnvironment(SettingsEnvironment newEnvironment, ServerConfig config, OAuthConfig oAuthConfig)
        {
            Reset();
            Initialize(newEnvironment, config, oAuthConfig);

            if(ams != null)
            {
                ams.Disconnect();
                ams = null;

                var coroutineRunner = new CoroutineRunner();
                ams = CreateAMSConnection(config, coroutineRunner, autoConnect: true);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="newApiServer"></param>
        /// <param name="id">Leave empty for "default"; duplicate key names will replace the previous entry</param>
        /// <returns>isSuccess</returns>
        internal bool RegisterApi(string id, ServerApiClient newApiServer)
        {
            Assert.IsNotNull(newApiServer, "!apiClient @ RegisterServerApi");
            if (string.IsNullOrEmpty(id))
            {
                Logger?.LogError("!id @ RegisterServerApi");
                return false;
            }

            if(serverApiClientInstances.ContainsKey(id))
            {
                RemoveApi(id);
            }

            newApiServer.SetSharedMemory(sharedMemory);

            newApiServer.session.OnLoginSuccess += () =>
            {
                loginUserClientApis.Add(newApiServer);
                if(loginUserClientApis.Count == 1)
                {
                    AttachAnalyticsSchedulerApi(newApiServer);
                }
            };

            newApiServer.session.OnLogoutSuccess += () =>
            {
                int index = loginUserClientApis.FindIndex(apiClient => apiClient == newApiServer);
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

            serverApiClientInstances[id] = newApiServer;
            
            return true;
        }

        internal bool RemoveApi(string id)
        {
            bool success = false;
            if(serverApiClientInstances.ContainsKey(id))
            {
                ServerApiClient removedServerApi = serverApiClientInstances[id];
                removedServerApi.Reset();
                if(loginUserClientApis.Contains(removedServerApi))
                {
                    loginUserClientApis.Remove(removedServerApi);
                }
                success = serverApiClientInstances.Remove(id);
            }
            return success;
        }

        /// <summary>
        /// Creates (and returns) new ApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal ServerApiClient CreateNewApi()
        {
            CoroutineRunner coroutineRunner = new CoroutineRunner();

            IHttpClient httpClient = CreateHtppClient();

            var session = new ServerOauthLoginSession(
                Config.IamServerUrl,
                OAuthConfig.ClientId,
                OAuthConfig.ClientSecret,
                httpClient,
                coroutineRunner);
            session.SetSharedMemory(sharedMemory);

            var newApiServer = new ServerApiClient(session, httpClient, coroutineRunner, Config, OAuthConfig);

            return newApiServer;
        }

        protected virtual IHttpClient CreateHtppClient()
        {
            IHttpRequestSender requestSender = requestSenderFactory.CreateHttpRequestSender();
            requestSender.SetLogger(Logger);
            AccelByteHttpClient httpClient = new AccelByteHttpClient(requestSender, Logger);
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

        private void InitializeAnalytics(ServerConfig config)
        {
            const ServerAnalyticsService analyticsApiWrapper = null;
            predefinedEventScheduler = CreatePredefinedEventScheduler(analyticsApiWrapper, config);
            predefinedEventScheduler.SetSharedMemory(ref sharedMemory);
            
            if(gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.Initialize(analyticsApiWrapper, config);
                LoadGameStandardAnalyticsCache(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString());
            }
            
            string gameStandardCacheFileName = GameStandardAnalyticsServerService.DefaultCacheFileName;
            string gameStandardCacheEncryptionKey = OAuthConfig.ClientId;
            string gameStandardCacheTableName = $"GameStandardCache/{gameStandardCacheFileName}";
            if (GameStandardCacheImp == null)
            {
                IAccelByteDataStorage gameStandardStorage = new AccelByteDataStorageBinaryFile(this.fileStream);
                GameStandardCacheImp = new AccelByteGameStandardEventCacheImp(
                    gameStandardCacheTableName
                    , gameStandardStorage
                    , gameStandardCacheEncryptionKey
                );
            }
            else
            {
                GameStandardCacheImp.UpdateKey(gameStandardCacheEncryptionKey);
            }
            GameStandardCacheImp.SetLogger(Logger);
        }

        private void ClearAnalytics()
        {
            predefinedEventScheduler.SetEventEnabled(false);

            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.StopScheduler();
                const bool cacheAsync = false;
                CacheGameStandardAnalytics(GameStandardCacheImp, gameStandardAnalyticsService, environment.ToString(), cacheAsync);
                gameStandardAnalyticsService.Scheduler.ClearTasks();
            }
        }

        private void AttachAnalyticsSchedulerApi(ServerApiClient apiServer)
        {
            ServerAnalyticsService analyticsWrapper = apiServer.GetAnalyticsService();
            predefinedEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.SetAnalyticsWrapper(analyticsWrapper);
            }
        }

        private void DetachAnalyticsSchedulerApi()
        {
            const ServerAnalyticsService analyticsWrapper = null;
            predefinedEventScheduler.SetAnalyticsApiWrapper(analyticsWrapper);
            if (gameStandardAnalyticsService != null)
            {
                gameStandardAnalyticsService.SetAnalyticsWrapper(analyticsWrapper);
            }
        }

        private void CacheGameStandardAnalytics(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsServerService gameStandardService, string environment, bool async)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetSaveAsync(async);
                gameStandardService.CacheEvents(cacheImp, environment);
            }
        }

        internal void SendSDKInitializedEvent(string sdkVersion)
        {
            var sdkInitializedPayload = new PredefinedSDKInitializedPayload("", sdkVersion);
            var telemetryEvent = new AccelByteTelemetryEvent(sdkInitializedPayload);
            predefinedEventScheduler.SendEvent(telemetryEvent, null);
        }

        private void LoadGameStandardAnalyticsCache(AccelByteGameStandardEventCacheImp cacheImp, GameStandardAnalyticsServerService gameStandardService, string environment)
        {
            if (cacheImp != null && gameStandardService != null)
            {
                cacheImp.SetLoadAsync(true);
                gameStandardService.LoadCachedEvent(cacheImp, environment);
            }
        }

        internal ServerAMS CreateAMSConnection(ServerConfig config, CoroutineRunner coroutineRunner, bool autoConnect = true)
        {
            if (config.DsId == null)
            {
                Logger?.LogWarning("dsid not provided, not connecting to ams");
                return null;
            }

            var newAMS = ConstructAMS(config, coroutineRunner);
            if (autoConnect)
            {
                newAMS.Connect(config.DsId);
            }

            return newAMS;
        }

        protected virtual ServerAMS ConstructAMS(ServerConfig config, CoroutineRunner coroutineRunner)
        {
            var newAMS = new ServerAMS(
                config,
                coroutineRunner,
                sharedMemory);
            return newAMS;
        }

        internal static PredefinedEventScheduler CreatePredefinedEventScheduler(ServerAnalyticsService analyticsService, ServerConfig config)
        {
            var newPredefinedEventScheduler = new PredefinedEventScheduler(analyticsService);
            newPredefinedEventScheduler.SetEventEnabled(config.EnablePreDefinedEvent);
            return newPredefinedEventScheduler;
        }

        private void UpdateServerTime(ref AccelByteTimeManager timeManager, IHttpClient httpClient, ref ServerConfig config)
        {
            if (httpClient != null && timeManager != null && config != null && timeManager.GetCachedServerTime() == null)
            {
                TimeManager.FetchServerTime(httpClient, config.Namespace, config.BasicServerUrl);
            }
        }

        private void SetupCurrentLogger(ServerConfig config)
        {
            Logger?.SetEnableLogging(config.EnableDebugLog);
            Logger?.SetEnableEnhancedLogging(config.EnhancedServiceLogging);
            if (Enum.TryParse(config.DebugLogFilter, out AccelByteLogType logTypeEnum))
            {
                Logger?.SetFilterLogType(logTypeEnum);
            }
            AccelByteDebug.Initialize(config.EnableDebugLog, config.DebugLogFilter);
        }
    }
}
