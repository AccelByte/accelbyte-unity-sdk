// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Server;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    /// <summary>
    /// Tracks multiple session instances for class children inheritted from ApiBase (or ServerApiBase)
    /// <para>
    /// Eg, for Xbox with multiple users signing in for a local session of 2+ players
    ///     (both signed into their own, individual accounts)
    /// </para>
    /// </summary>
    public static class MultiRegistry
    {
        #region Constructor
        // public static readonly bool UseSharedCredentials; // TODO: Why do we need this?
        private static readonly Dictionary<string, ApiClient> apiClientInstances;
        private static readonly Dictionary<string, ServerApiClient> serverApiClientInstances;

        static MultiRegistry()
        {
            apiClientInstances = new Dictionary<string, ApiClient>();
            serverApiClientInstances = new Dictionary<string, ServerApiClient>();
        }
        #endregion /Constructor

        
        #region Client
        private static Config config => AccelBytePlugin.Config;
        
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ApiClient GetApiClient(string key = "default")
        {
            // If an existing instance exists here, return it.
            bool hasExistingInstance = apiClientInstances.ContainsKey(key); 
            if (hasExistingInstance)
            {
                apiClientInstances.TryGetValue(key, out ApiClient apiBase);
                return apiBase;
            }
            
            ApiClient newApiClient = CreateNewApiClient(key);
            RegisterApiClient(key, newApiClient);

            if(key == "default")
            {
                AccelBytePlugin.configReset += newApiClient.environmentChanged;
            }

            return newApiClient;
        }

        /// <summary>
        /// Creates (and returns) new ApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ApiClient CreateNewApiClient(string key = "default")
        {
            CoroutineRunner coroutineRunner = new CoroutineRunner();

            AccelByteHttpClient httpClient = new AccelByteHttpClient();
            httpClient.SetCredentials( config.ClientId, config.ClientSecret );
            httpClient.SetBaseUri( new Uri( config.BaseUrl ) );

            LoginSession session;

            if (key == "default")
            {
                session = AccelBytePlugin.GetUser().Session as LoginSession;
            }
            else
            {
                session = new LoginSession(
                    config.IamServerUrl,
                    config.Namespace,
                    config.RedirectUri,
                    httpClient,
                    coroutineRunner,
                    config.UsePlayerPrefs );
            }

            return new ApiClient( session, httpClient, coroutineRunner );
        }

        /// <summary>
        /// </summary>
        /// <param name="serverApiClient"></param>
        /// <param name="key">Leave empty for "default"; duplicate key names will replace the previous entry</param>
        /// <returns>isSuccess</returns>
        public static bool RegisterApiClient(string key, ApiClient serverApiClient)
        {
            Assert.IsNotNull(serverApiClient, "!apiClient @ RegisterApiClient");
            if (string.IsNullOrEmpty(key))
            {
                UnityEngine.Debug.LogError("!key @ RegisterApiClient");
                return false;
            }
            
            apiClientInstances[key] = serverApiClient;
            return true;
        }
        #endregion /Client
        

        #region Server
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ServerApiClient GetServerApiClient(string key = "default")
        {
            // If an existing instance exists here, return it.
            bool hasExistingInstance = serverApiClientInstances.ContainsKey(key); 
            if (hasExistingInstance)
            {
                serverApiClientInstances.TryGetValue(key, out ServerApiClient serverApiBase);
                return serverApiBase;
            }
            
            ServerApiClient newServerApiClient = CreateNewServerApiClient(key);
            RegisterServerApiClient(key, newServerApiClient);

            if (key == "default")
            {
                AccelByteServerPlugin.configReset += newServerApiClient.environmentChanged;
            }

            return newServerApiClient;
        }

        /// <summary>
        /// Creates (and returns) new ServerApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ServerApiClient CreateNewServerApiClient(string key = "default")
        {
            CoroutineRunner coroutineRunner = new CoroutineRunner();

            AccelByteHttpClient httpClient = new AccelByteHttpClient();
            httpClient.SetCredentials( config.ClientId, config.ClientSecret );
            httpClient.SetBaseUri( new Uri( config.BaseUrl ) );

            ServerOauthLoginSession session;

            if( key == "default" )
            {
                session = AccelByteServerPlugin.GetDedicatedServer().Session as ServerOauthLoginSession;
            }
            else
            {
                session = new ServerOauthLoginSession(
                    config.IamServerUrl,
                    config.ClientId,
                    config.ClientSecret,
                    httpClient,
                    coroutineRunner );
            }

            return new ServerApiClient( session, httpClient, coroutineRunner );
        }

        /// <summary>
        /// Overrides existing session with same key name
        /// </summary>
        /// <param name="serverApiClient"></param>
        /// <param name="key">Leave empty for "default"</param>
        /// <returns>isSuccess</returns>
        public static bool RegisterServerApiClient(string key, ServerApiClient serverApiClient)
        {
            Assert.IsNotNull(serverApiClient, "!apiClient @ RegisterServerApiClient");
            if (string.IsNullOrEmpty(key))
            {
                UnityEngine.Debug.LogError("!key @ RegisterServerApiClient");
                return false;
            }
            
            serverApiClientInstances[key] = serverApiClient;
            return true;
        }
        #endregion /Server
    }
}
