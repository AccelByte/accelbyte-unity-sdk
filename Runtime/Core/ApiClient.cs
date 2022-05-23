// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Reflection;
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
        public LoginSession session;
        public IHttpClient httpClient;
        public readonly CoroutineRunner coroutineRunner;

        public ApiClient( LoginSession inSession
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
        /// <para>* TEMPORARY EXCEPTION: UserApi's ISession param is of type LoginSession</para> 
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
            
            return (TWrapper)newWrapperInstance;
        }

        internal void environmentChanged()
        {
            session = AccelBytePlugin.GetUser().Session as LoginSession;
            httpClient.SetCredentials(config.ClientId, config.ClientSecret);
            httpClient.SetBaseUri(new Uri(config.BaseUrl));
        }

    } // Class
} // Namespace
