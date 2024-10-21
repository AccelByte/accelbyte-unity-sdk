// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public class ServerDSHubApi: ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==DSHubServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerDSHubApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.DSHubServerUrl, session )
        {
        }

        public ServerConfig ServerConfig => base.ServerConfig;
    }
}