// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Reflection;
using AccelByte.Models;
using AccelByte.Server;

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

        public ServerApiClient( ServerOauthLoginSession inSession
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutinerunner )
        {
            session = inSession;
            httpClient = inHttpClient;
            coroutineRunner = inCoroutinerunner;
        }
        #endregion /Constructor


        private static ServerConfig serverConfig => AccelByteServerPlugin.Config;
        
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

        /// <summary>
        /// Get new ServerApiBase child class instance.
        /// </summary>
        /// <list type="bullet">
        /// <item><term>DedicatedServerManagerApi</term></item>
        /// <item><term>ServerAchievementApi</term></item>
        /// <item><term>ServerCloudSaveApi</term></item>
        /// <item><term>ServerEcommerceApi</term></item>
        /// <item><term>ServerGameTelemetryApi</term></item>
        /// <item><term>ServerLobbyApi</term></item>
        /// <item><term>ServerMatchmakingApi</term></item>
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
            object newWrapperInstance = Activator.CreateInstance(
                typeof(TServerWrapper), 
                bindingAttr: reflectionBindingToFindInternal,
                binder: null,
                args: wrapperArgs,
                culture: null);
            
            return (TServerWrapper)newWrapperInstance;
        }

        internal void environmentChanged()
        {
            session = AccelByteServerPlugin.GetDedicatedServer().Session as ServerOauthLoginSession;
            httpClient.SetCredentials(serverConfig.ClientId, serverConfig.ClientSecret);
            httpClient.SetBaseUri(new Uri(serverConfig.BaseUrl));
        }

    }
}
