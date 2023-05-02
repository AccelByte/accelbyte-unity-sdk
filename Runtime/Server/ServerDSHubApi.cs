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
        internal ServerDSHubApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.DSHubServerUrl, session )
        {
        }

        public ServerConfig ServerConfig => base.serverConfig;
    }
}