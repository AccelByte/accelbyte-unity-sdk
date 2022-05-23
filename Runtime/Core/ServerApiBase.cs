// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    /// <summary>
    /// Inherit this abstract class to set common Server Api functionality.
    /// - Used for Server Api modules, such as "ServerLobbyApi".
    /// </summary>
    public abstract class ServerApiBase : ApiBaseParent
    {
        #region Constructor
        /// <summary>
        /// Contains namespace, baseUrl's
        /// TODO: Do we want this private, then just make protected Getters() for Config.x?
        ///</summary>
        protected readonly ServerConfig ServerConfig;
        
        protected ServerApiBase( IHttpClient inHttpClient
            , ServerConfig inServerConfig
            , string inBaseUrl
            , ISession inSession )
            : base( inHttpClient, inSession, inBaseUrl )
        {
            Assert.IsNotNull(inServerConfig, $"Creating {GetType().Name} failed. " +
                "Parameter `_serverConfig` is null");
            ServerConfig = inServerConfig;
        }
        #endregion /Constructor

        
        #region High-Level Shortcuts
        /// <summary>ServerConfig.Namespace wrapper</summary>
        protected string Namespace_ => ServerConfig.Namespace;
        #endregion /High-Level Shortcuts
    }
}
