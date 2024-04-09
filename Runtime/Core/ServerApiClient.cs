// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Reflection;
using AccelByte.Models;
using AccelByte.Server;
using AccelByte.Server.Interface;

namespace AccelByte.Core
{
    /// <summary>
    /// Holder of ServerApiBase children.
    /// <para>eg: ServerAchievementApi, ServerLobbyApi</para>
    /// </summary>
    public class ServerApiClient
    {
        #region Constructor
        public ServerOauthLoginSession session;
        public IHttpClient httpClient;
        public readonly CoroutineRunner coroutineRunner;

        private ServerConfig serverConfig;
        private OAuthConfig serverOAuthConfig;

        private Dictionary<string, WrapperBase> wrapperBaseCollection = new Dictionary<string, WrapperBase>();
        private AccelByteStatsDMetricExporterApi statsExporterApi;
        private DedicatedServer dedicatedServer;

        public ServerApiClient(ServerOauthLoginSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutinerunner) : this(inSession, inHttpClient, inCoroutinerunner, AccelByteSDK.GetServerConfig(), AccelByteSDK.GetServerOAuthConfig())
        {
        }

        public ServerApiClient(ServerOauthLoginSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , ServerConfig serverConfig
            , OAuthConfig oAuthConfig)
        {
            session = inSession;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
            this.serverConfig = serverConfig;
            this.serverOAuthConfig = oAuthConfig;
            sharedMemory = new ApiSharedMemory();
        }
        #endregion /Constructor

        public void Reset()
        {
            session.Reset();
        }

        public object[] getApiArgs() => new object[]
        {
            httpClient,
            serverConfig,
            session,
        };

        // Reflection cannot find `internal` scope modifiers without these bindings.
        private const BindingFlags reflectionBindingToFindInternal =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        private ApiSharedMemory sharedMemory;

        /// <summary>
        /// Get new ServerApiBase child class instance.
        /// </summary>
        /// <list type="bullet">
        /// <item><term>DedicatedServerManagerApi</term></item>
        /// <item><term>ServerDSHubApi</term></item>
        /// <item><term>ServerAchievementApi</term></item>
        /// <item><term>ServerCloudSaveApi</term></item>
        /// <item><term>ServerEcommerceApi</term></item>
        /// <item><term>ServerGameTelemetryApi</term></item>
        /// <item><term>ServerLobbyApi</term></item>
        /// <item><term>ServerSessionApi</term></item>
        /// <item><term>ServerMatchmakingApi</term></item>
        /// <item><term>ServerMatchmakingV2Api</term></item>
        /// <item><term>ServerQosManagerApi</term></item>
        /// <item><term>ServerSeasonPassApi</term></item>
        /// <item><term>ServerStatisticApi</term></item>
        /// <item><term>ServerUserAccountApi</term></item>
        /// </list>
        /// <typeparam name="TServerWrapper">ServerWrapperBase child - eg, "ServerGroup"</typeparam>
        /// <typeparam name="TServerApi">ServerApiBase child - eg, "ServerGroupApi"</typeparam>
        /// <returns></returns>
        public TServerWrapper GetServerApi<TServerWrapper, TServerApi>()
            where TServerWrapper : WrapperBase
            where TServerApi : ServerApiBase
        {
            // ####################################################################
            // Expected TApi constructor params: (IHttpClient, ServerConfig, ISession)
            // Expected TWrapper constructor params: (TServerApi, ISession, CoroutineRunner)
            // ####################################################################

            string currentWrapperName = typeof(TServerWrapper).GetTypeInfo().FullName;

            if (wrapperBaseCollection.ContainsKey(currentWrapperName))
            {
                return (TServerWrapper)wrapperBaseCollection[currentWrapperName];
            }

            object[] apiArgs = getApiArgs();

            // Create the TApi to pass as a param to the TCore constructor
            TServerApi api = (TServerApi)Activator.CreateInstance(
                typeof(TServerApi),
                bindingAttr: reflectionBindingToFindInternal,
                binder: null,
                args: apiArgs,
                culture: null);

            object[] wrapperArgs =
            {
                api,
                session,
                coroutineRunner,
            };

            // Return CoreBase child with consistently-known args, including an ApiBase type
            TServerWrapper newWrapperInstance = (TServerWrapper)Activator.CreateInstance(
                typeof(TServerWrapper),
                bindingAttr: reflectionBindingToFindInternal,
                binder: null,
                args: wrapperArgs,
                culture: null);

            TServerWrapper ServerWrapperInstance = newWrapperInstance;

            SetApi(ServerWrapperInstance);
            return ServerWrapperInstance;
        }

        public void SetApi<TWrapper>(TWrapper newWrapper) where TWrapper : WrapperBase
        {
            string currentWrapperName = typeof(TWrapper).GetTypeInfo().FullName;
            newWrapper.SetSharedMemory(sharedMemory);
            wrapperBaseCollection[currentWrapperName] = newWrapper;
        }

        #region API_GETTER
        public DedicatedServer GetDedicatedServer()
        {
            if (dedicatedServer == null)
            {
                dedicatedServer = new DedicatedServer(session, coroutineRunner);
            }
            return dedicatedServer;
        }
        public DedicatedServerManager GetDedicatedServerManager() { return GetServerApi<DedicatedServerManager, DedicatedServerManagerApi>(); }
        public ServerEcommerce GetEcommerce() { return GetServerApi<ServerEcommerce, ServerEcommerceApi>(); }
        public ServerStatistic GetStatistic() { return GetServerApi<ServerStatistic, ServerStatisticApi>(); }
        public ServerUGC GetUGC() { return GetServerApi<ServerUGC, ServerUGCApi>(); }
        public ServerQosManager GetQos() { return GetServerApi<ServerQosManager, ServerQosManagerApi>(); }
        public ServerGameTelemetry GetGameTelemetry() { return GetServerApi<ServerGameTelemetry, ServerGameTelemetryApi>(); }
        public ServerAchievement GetAchievement() { return GetServerApi<ServerAchievement, ServerAchievementApi>(); }
        public ServerLobby GetLobby() { return GetServerApi<ServerLobby, ServerLobbyApi>(); }
        public ServerCloudSave GetCloudSave() { return GetServerApi<ServerCloudSave, ServerCloudSaveApi>(); }
        public IServerChallenge GetChallenge() { return GetServerApi<ServerChallenge, ServerChallengeApi>(); }
        public ServerMatchmaking GetMatchmaking() { return GetServerApi<ServerMatchmaking, ServerMatchmakingApi>(); }
        public ServerUserAccount GetUserAccount() { return GetServerApi<ServerUserAccount, ServerUserAccountApi>(); }
        public ServerSeasonPass GetSeasonPass() { return GetServerApi<ServerSeasonPass, ServerSeasonPassApi>(); }
        public ServerDSHub GetDsHub() { return GetServerApi<ServerDSHub, ServerDSHubApi>(); }
        public ServerSession GetSession() { return GetServerApi<ServerSession, ServerSessionApi>(); }
        public ServerMatchmakingV2 GetMatchmakingV2() { return GetServerApi<ServerMatchmakingV2, ServerMatchmakingV2Api>(); }
        public ServerAnalyticsService GetAnalyticsService() { return GetServerApi<ServerAnalyticsService, ServerAnalyticsApi>(); }
        public ServiceVersion GetVersionService() { return GetServerApi<ServiceVersion, ServiceVersionApi>(); }

        public AccelByteStatsDMetricExporterApi GetStatsMetricExporterService()
        {
            if (statsExporterApi == null)
            {
                statsExporterApi = new AccelByteStatsDMetricExporterApi(httpClient, serverConfig, session);
            }
            return statsExporterApi;
        }
        #endregion

        internal void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            sharedMemory = newSharedMemory;
        }
        
        internal void environmentChanged()
        {
            session = AccelByteSDK.GetServerRegistry().GetApi().session;
            httpClient.SetCredentials(serverOAuthConfig.ClientId, serverOAuthConfig.ClientSecret);
            httpClient.SetBaseUri(new Uri(serverConfig.BaseUrl));
        }
    }
}
