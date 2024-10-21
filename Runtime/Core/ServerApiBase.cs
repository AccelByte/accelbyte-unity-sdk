// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using JetBrains.Annotations;
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
        ///</summary>
        internal readonly ServerConfig ServerConfig;
        internal readonly HttpOperator HttpOperator;

        protected ServerApiBase( IHttpClient inHttpClient
            , ServerConfig inServerConfig
            , string inBaseUrl
            , ISession inSession
            , HttpOperator httpOperator = null)
            : base( inHttpClient, inSession, inBaseUrl )
        {
            Assert.IsNotNull(inServerConfig, $"Creating {GetType().Name} failed. " +
                "Parameter `_serverConfig` is null");
            ServerConfig = inServerConfig;
            this.HttpOperator = httpOperator != null ? httpOperator : HttpOperator.CreateDefault(inHttpClient);
        }
        #endregion /Constructor
        
        #region High-Level Shortcuts
        /// <summary>ServerConfig.Namespace wrapper</summary>
        internal string Namespace_ => ServerConfig.Namespace;
        #endregion /High-Level Shortcuts
    }
}
