// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Reflection;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Server;

namespace AccelByte.Core
{
    /// <summary>
    /// Holder of ApiBase children, ultimately stored in MultiRegistry. 
    /// <para>eg: GroupApi, AchievementApi</para>
    /// </summary>
    public class ApiClient
    {
        #region Constructor
        public UserSession session;
        public IHttpClient httpClient;
        public readonly CoroutineRunner coroutineRunner;
        public Dictionary<string, WrapperBase> wrapperBaseCollection = new Dictionary<string, WrapperBase>();

        public ApiClient( UserSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner )
        {
            session = inSession;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutineRunner;
        }
        #endregion /Constructor
        
        // Reflection cannot find `internal` scope modifiers without these bindings.
        private const BindingFlags reflectionBindingToFindInternal =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance;

        /// <summary>CURRENTLY UNUSED: Added for Unreal parity</summary>
        public bool UseSharedCredentials;
        
        private static OAuthConfig oAuthConfig => AccelBytePlugin.OAuthConfig;
        private static Config config => AccelBytePlugin.Config;
        
        public object[] getApiArgs() => new object[]
        {
            httpClient,
            config, 
            session,
        };
        
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
        public TWrapper GetApi<TWrapper, TApi>() 
            where TWrapper : WrapperBase 
            where TApi  : ApiBase // `Parent` supports both client and server
        {
            string currentWrapperName = typeof(TWrapper).GetTypeInfo().FullName;

            if (wrapperBaseCollection.ContainsKey(currentWrapperName))
            {
                return (TWrapper)wrapperBaseCollection[currentWrapperName];
            }

            // ####################################################################
            // Expected TApi constructor params: (IHttpClient, Config, ISession)
            // Expected TWrapper constructor params: (TApi, IUserSession, CoroutineRunner)
            // ####################################################################

            object[] apiArgs = getApiArgs();
            
            // Create the TApi to pass as a param to the TCore constructor
            TApi api = (TApi)Activator.CreateInstance(
                typeof(TApi), 
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
            object newWrapperInstance = Activator.CreateInstance(
                typeof(TWrapper), 
                bindingAttr: reflectionBindingToFindInternal,
                binder: null,
                args: wrapperArgs,
                culture: null);

            wrapperBaseCollection.Add(currentWrapperName, (WrapperBase)newWrapperInstance);
            return (TWrapper)wrapperBaseCollection[currentWrapperName];
        }


#region API_GETTER
        public User GetUser() { return GetApi<User, UserApi>(); }
        public UserProfiles GetUserProfiles() { return GetApi<UserProfiles, UserProfilesApi>(); }
        public Categories GetCategories() { return GetApi<Categories, CategoriesApi>(); }
        public Items GetItems() { return GetApi<Items, ItemsApi>(); }
        public Currencies GetCurrencies() { return GetApi<Currencies, CurrenciesApi>(); }
        public Orders GetOrders() { return GetApi<Orders, OrdersApi>(); }
        public Reward GetReward() { return GetApi<Reward, RewardApi>(); }
        public Wallet GetWallet() { return GetApi<Wallet, WalletApi>(); }
        public Lobby GetLobby() { return GetApi<Lobby, LobbyApi>(); }
        public CloudStorage GetCloudStorage() { return GetApi<CloudStorage, CloudStorageApi>(); }
        public GameProfiles GetGameProfiles() { return GetApi<GameProfiles, GameProfilesApi>(); }
        public Entitlement GetEntitlement() { return GetApi<Entitlement, EntitlementApi>(); }
        public Fulfillment GetFulfillment() { return GetApi<Fulfillment, FulfillmentApi>(); }
        public Statistic GetStatistic() { return GetApi<Statistic, StatisticApi>(); }
        public QosManager GetQos() { return GetApi<QosManager, QosManagerApi>(); }
        public Agreement GetAgreement() { return GetApi<Agreement, AgreementApi>(); }
        public Leaderboard GetLeaderboard() { return GetApi<Leaderboard, LeaderboardApi>(); }
        public CloudSave GetCloudSave() { return GetApi<CloudSave, CloudSaveApi>(); }
        public GameTelemetry GetGameTelemetry() { return GetApi<GameTelemetry, GameTelemetryApi>(); }
        public Achievement GetAchievement() { return GetApi<Achievement, AchievementApi>(); }
        public Group GetGroup() { return GetApi<Group, GroupApi>(); }
        public UGC GetUgc() { return GetApi<UGC, UGCApi>(); }
        public Reporting GetReporting() { return GetApi<Reporting, ReportingApi>(); }
        public SeasonPass GetSeasonPass() { return GetApi<SeasonPass, SeasonPassApi>(); }
        public SessionBrowser GetSessionBrowser() { return GetApi<SessionBrowser, SessionBrowserApi>(); }
        public TurnManager GetTurnManager() { return GetApi<TurnManager, TurnManagerApi>(); }
        public Miscellaneous GetMiscellaneous() { return GetApi<Miscellaneous, MiscellaneousApi>(); }
        public Session GetSession() { return GetApi<Session, SessionApi>(); }
        public MatchmakingV2 GetMatchmakingV2() { return GetApi<MatchmakingV2, MatchmakingV2Api>(); }
        public Chat GetChat() { return GetApi<Chat, ChatApi>(); }
        public PresenceBroadcastEvent GetPresenceBroadcastEvent() { return GetApi<PresenceBroadcastEvent, PresenceBroadcastEventApi>(); }
        public Gdpr GetGdpr() { return GetApi<Gdpr, GdprApi>(); }
        public AnalyticsService GetAnalyticsService() { return GetApi<AnalyticsService, AnalyticsApi>(); }
#endregion


        internal void environmentChanged()
        {
            session = AccelBytePlugin.GetUser().Session as UserSession;
            httpClient.SetCredentials(oAuthConfig.ClientId, oAuthConfig.ClientSecret);
            httpClient.SetBaseUri(new Uri(config.BaseUrl));
        }
    }
    // Class
} // Namespace
