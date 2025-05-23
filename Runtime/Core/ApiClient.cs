﻿// Copyright (c) 2022 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Reflection;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Api.Interface;

namespace AccelByte.Core
{
    /// <summary>
    /// Holder of ApiBase children, ultimately stored in MultiRegistry. 
    /// <para>eg: GroupApi, AchievementApi</para>
    /// </summary>
    public class ApiClient
    {
        public UserSession session;
        public IHttpClient httpClient;
        public readonly CoroutineRunner coroutineRunner;
        public Dictionary<string, WrapperBase> wrapperBaseCollection = new Dictionary<string, WrapperBase>();

        // Reflection cannot find `internal` scope modifiers without these bindings.
        private const BindingFlags reflectionBindingToFindInternal =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        /// <summary>CURRENTLY UNUSED: Added for Unreal parity</summary>
        public bool UseSharedCredentials;

        private OAuthConfig oAuthConfig;
        internal Config Config;

        public object[] getApiArgs() => new object[]
        {
            httpClient,
            Config,
            session,
        };

        private ApiSharedMemory sharedMemory;

        #region Constructor
        public ApiClient( UserSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner ) : this(inSession, inHttpClient, inCoroutineRunner, AccelByteSDK.GetClientConfig(), AccelByteSDK.GetClientOAuthConfig())
        {
        }

        internal ApiClient(UserSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , Config clientConfig
            , OAuthConfig oAuthConfig)
        {
            session = inSession;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
            Config = clientConfig;
            this.oAuthConfig = oAuthConfig;
            sharedMemory = new ApiSharedMemory();
        }
        #endregion /Constructor

        public void Reset()
        {
            StopHeartBeat();

            const bool autoCreate = false;
            User user = GetUser(autoCreate);
            if (user != null)
            {
                user.OnLoginSuccess = null;
                user.OnLogout = null;
            }
            
            Api.Session sessionWrapper = GetSession(autoCreate);
            if (sessionWrapper != null)
            {
                sessionWrapper.Reset();
            }

            if (session != null)
            {
                session.ClearSession();
            }

            sharedMemory.MessagingSystem.UnsubscribeAll();
        }
        
        /// <summary>
        /// Get new CoreBase child class instance with dynamic params.
        /// <para>* Param types: IHttpClient, Config, ISession.</para> 
        /// <para>* TEMPORARY EXCEPTION: UserApi's ISession param is of type UserSession</para> 
        /// </summary>
        /// 
        /// <list type="bullet">
        /// <item><term>User</term></item>
        /// <item><term>Categories</term></item>
        /// <item><term>Items</term></item>
        /// <item><term>Currencies</term></item>
        /// <item><term>Orders</term></item>
        /// <item><term>Wallet</term></item>
        /// <item><term>UserProfiles</term></item>
        /// <item><term>Lobby</term></item>
        /// <item><term>CloudStorage</term></item>
        /// <item><term>GameProfiles</term></item>
        /// <item><term>Entitlement</term></item>
        /// <item><term>Fulfillment</term></item>
        /// <item><term>Statistic</term></item>
        /// <item><term>QosManager</term></item>
        /// <item><term>Agreement</term></item>
        /// <item><term>Leaderboard</term></item>
        /// <item><term>CloudSave</term></item>
        /// <item><term>GameTelemetry</term></item>
        /// <item><term>Achievement</term></item>
        /// <item><term>Group</term></item>
        /// <item><term>UGC</term></item>
        /// <item><term>Reporting</term></item>
        /// <item><term>SeasonPass</term></item>
        /// <item><term>Miscellaneous</term></item>
        /// <item><term>Reward</term></item> 
        /// <item><term>PresenceBroadcastEvent</term></item> 
        /// <item><term>PredefinedAnalyticsEvent</term></item> 
        /// </list>
        /// 
        /// <typeparam name="TWrapper">WrapperBase child - eg, "Group"</typeparam>
        /// <typeparam name="TApi">ApiBase child - eg, "GroupApi"</typeparam>
        /// <example>
        /// <code>
        /// ApiClient apiClient = MultiRegistry.GetApiClient(userData.emailAddress);
        /// User localUser = apiClient.GetApi≪User, UserApi≫();
        /// </code>
        /// </example>
        /// 
        /// <returns>Returns new ApiBase child instance.</returns>
        public TWrapper GetApi<TWrapper, TApi>(bool autoCreate = true) 
            where TWrapper : WrapperBase 
            where TApi  : ApiBase // `Parent` supports both client and server
        {
            TWrapper wrapper = null;
            string currentWrapperName = typeof(TWrapper).GetTypeInfo().FullName;

            if (wrapperBaseCollection.ContainsKey(currentWrapperName))
            {
                wrapper = (TWrapper)wrapperBaseCollection[currentWrapperName];
            }
            else if (autoCreate)
            {
                // ####################################################################
                // Expected TApi constructor params: (IHttpClient, Config, ISession)
                // Expected TWrapper constructor params: (TApi, IUserSession, CoroutineRunner)
                // ####################################################################

                object[] apiArgs = getApiArgs();

                // Create the TApi to pass as a param to the TCore constructor
                TApi api = (TApi)Activator.CreateInstance(
                    typeof(TApi)
                    , bindingAttr: reflectionBindingToFindInternal
                    , binder: null
                    , args: apiArgs
                    , culture: null
                );
                api.SetSharedMemory(sharedMemory);

                object[] wrapperArgs =
                {
                api,
                session,
                coroutineRunner,
                };

                // Return CoreBase child with consistently-known args, including an ApiBase type
                TWrapper newWrapperInstance = (TWrapper)Activator.CreateInstance(
                    typeof(TWrapper),
                    bindingAttr: reflectionBindingToFindInternal,
                    binder: null,
                    args: wrapperArgs,
                    culture: null);

                SetApi(newWrapperInstance);
                wrapper = newWrapperInstance;
            }
            return wrapper;
        }

        public void SetApi<TWrapper>(TWrapper newWrapper) where TWrapper : WrapperBase
        {
            string currentWrapperName = typeof(TWrapper).GetTypeInfo().FullName;
            newWrapper.SetSharedMemory(sharedMemory);
            wrapperBaseCollection[currentWrapperName] = newWrapper;
        }

        #region API_GETTER
        public User GetUser(bool autoCreate = true)
        {
            User retval = GetApi<User, UserApi>(autoCreate);
            return retval;
        }
        public UserProfiles GetUserProfiles() { return GetApi<UserProfiles, UserProfilesApi>(); }
        public Categories GetCategories() { return GetApi<Categories, CategoriesApi>(); }
        public IClientChallenge GetChallenge() { return GetApi<Challenge, ChallengeApi>(); }
        public Items GetItems() { return GetApi<Items, ItemsApi>(); }
        public IClientInventory GetInventory() { return GetApi<Inventory, InventoryApi>();  }
        public Currencies GetCurrencies() { return GetApi<Currencies, CurrenciesApi>(); }
        public Orders GetOrders() { return GetApi<Orders, OrdersApi>(); }
        public Reward GetReward() { return GetApi<Reward, RewardApi>(); }
        public Wallet GetWallet() { return GetApi<Wallet, WalletApi>(); }
        public Lobby GetLobby() { return GetApi<Lobby, LobbyApi>(); }
        [Obsolete("This interface is deprecated and will be removed on 2025.5.AGS. Please use GetBinaryCloudSave() instead")]
        public CloudStorage GetCloudStorage() { return GetApi<CloudStorage, CloudStorageApi>(); }
        [Obsolete("This interface is deprecated and will be removed on 2025.5.AGS. Please use GetCloudSave() instead")]
        public GameProfiles GetGameProfiles() { return GetApi<GameProfiles, GameProfilesApi>(); }
        public Entitlement GetEntitlement() { return GetApi<Entitlement, EntitlementApi>(); }
        public Fulfillment GetFulfillment() { return GetApi<Fulfillment, FulfillmentApi>(); }
        public Statistic GetStatistic() { return GetApi<Statistic, StatisticApi>(); }
        public QosManager GetQos() { return GetApi<QosManager, QosManagerApi>(); }
        public Agreement GetAgreement() { return GetApi<Agreement, AgreementApi>(); }
        public Leaderboard GetLeaderboard() { return GetApi<Leaderboard, LeaderboardApi>(); }
        public CloudSave GetCloudSave() { return GetApi<CloudSave, CloudSaveApi>(); }
        public GameTelemetry GetGameTelemetry() { return GetApi<GameTelemetry, ClientGameTelemetryApi>(); }
        public Achievement GetAchievement() { return GetApi<Achievement, AchievementApi>(); }
        public Group GetGroup() { return GetApi<Group, GroupApi>(); }
        public UGC GetUgc() { return GetApi<UGC, UGCApi>(); }
        public Reporting GetReporting() { return GetApi<Reporting, ReportingApi>(); }
        public SeasonPass GetSeasonPass() { return GetApi<SeasonPass, SeasonPassApi>(); }
        public TurnManager GetTurnManager() { return GetApi<TurnManager, TurnManagerApi>(); }
        public Miscellaneous GetMiscellaneous() { return GetApi<Miscellaneous, ClientMiscellaneousApi>(); }

        public Session GetSession(bool autoCreate = true)
        {
            Session retval = GetApi<Session, SessionApi>(autoCreate);
            return retval;
        }
        public MatchmakingV2 GetMatchmakingV2() { return GetApi<MatchmakingV2, MatchmakingV2Api>(); }
        public Chat GetChat() { return GetApi<Chat, ChatApi>(); }
        public Gdpr GetGdpr() { return GetApi<Gdpr, GdprApi>(); }
        public AnalyticsService GetAnalyticsService() { return GetApi<AnalyticsService, ClientGameTelemetryApi>(); }
        public BinaryCloudSave GetBinaryCloudSave() { return GetApi<BinaryCloudSave, BinaryCloudSaveApi>(); }
        public Api.ServiceVersion GetVersionService() { return GetApi<Api.ServiceVersion, Api.ServiceVersionApi>(); }
        public HeartBeat GetHeartBeatService(bool autoCreate = true)
        {
            HeartBeat retval = GetApi<HeartBeat, HeartBeatApi>(autoCreate);
            return retval;
        }
        public StoreDisplay GetStoreDisplayService() { return GetApi<StoreDisplay, StoreDisplayApi>(); }
        #endregion

        private void StopHeartBeat()
        {
            const bool setActive = false;
            const bool autoCreate = false;
            HeartBeat heartbeatService = GetHeartBeatService(autoCreate);
            if (heartbeatService != null && heartbeatService.IsHeartBeatJobEnabled)
            {
                heartbeatService.SetHeartBeatEnabled(setActive);
            }
        }

        internal void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            sharedMemory = newSharedMemory;
        }
    }
    // Class
} // Namespace
