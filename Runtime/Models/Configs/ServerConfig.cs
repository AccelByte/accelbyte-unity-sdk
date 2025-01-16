// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models {
    [DataContract, Preserve]
    public class ServerConfig : IAccelByteConfig
    {
        private const int defaultCacheSize = 100;
        private const int defaultCacheLifeTime = 100;
        private const string defaultAMSServerUrl = "ws://127.0.0.1:5555/watchdog";
        private const int defaultAMSHeartbeatInterval = 15;
        private const bool defaultPredefinedEvent = false;
        private const string defaultStatsDUrl = "localhost";
        private const int defaultStatsDPort = 8125;
        private const int defaultStatsDMetricInterval = 60;
        [DataMember] public string Namespace;
        [DataMember] public string PublisherNamespace;
        [DataMember] public string BaseUrl;
        [DataMember] public string AgreementServerUrl;
        [DataMember] public string IamServerUrl;
        [DataMember] public string DSHubServerUrl;
        [DataMember] public string DSMControllerServerUrl;
        [DataMember] public string StatisticServerUrl;
        [DataMember] public string UGCServerUrl;
        [DataMember] public string PlatformServerUrl;
        [DataMember] public string QosManagerServerUrl;
        [DataMember] public string GameTelemetryServerUrl;
        [DataMember] public string AchievementServerUrl;
        [DataMember] public string LobbyServerUrl;
        [DataMember] public string SessionServerUrl;
        [DataMember] public string CloudSaveServerUrl;
        [DataMember] public string ChallengeServerUrl;
        [DataMember] public string InventoryServerUrl;
        [DataMember] public string ProfanityFilterServerUrl;
        [DataMember] public string RedirectUri;
        [DataMember] public string MatchmakingServerUrl;
        [DataMember] public string MatchmakingV2ServerUrl;
        [DataMember] public string SeasonPassServerUrl;
        [DataMember] public string BasicServerUrl;
        [DataMember] public string AMSServerUrl = defaultAMSServerUrl;
        [DataMember] public string WatchdogUrl;
        [DataMember] public int AMSHeartbeatInterval = defaultAMSHeartbeatInterval;
        [DataMember] public int Port;
        [DataMember] public int MaximumCacheSize = defaultCacheSize;
        [DataMember] public int MaximumCacheLifeTime = defaultCacheLifeTime;
        [DataMember] public bool EnablePreDefinedEvent = defaultPredefinedEvent;
        [DataMember] public string StatsDServerUrl;
        [DataMember] public int StatsDServerPort;
        [DataMember] public int StatsDMetricInterval;
        [DataMember] public string DsId;
        [DataMember] public bool EnableDebugLog = true;
        [DataMember] public bool EnhancedServiceLogging = false;
        [DataMember] public string DebugLogFilter = "Verbose";
        [DataMember] public int DSHubReconnectTotalTimeout = 60000;
        [DataMember] public int AMSReconnectTotalTimeout = 60000;
        [DataMember] public bool ServerUseAMS = true;
        public string DSHubServerWsUrl
        {
            get;
            private set;
        }

        private IDebugger logger;

        /// <summary>
        ///  Copy member values
        /// </summary>
        public ServerConfig ShallowCopy()
        {
            return (ServerConfig) MemberwiseClone();
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand(bool forceExpandServiceApiUrl = false)
        {
            if (this.BaseUrl == null)
            {
                return;
            }
            
            this.IamServerUrl = this.ExpanServiceApiUrl(this.IamServerUrl, "/iam", forceExpandServiceApiUrl);

            this.DSHubServerUrl = this.ExpanServiceApiUrl(this.DSHubServerUrl, "/dshub", forceExpandServiceApiUrl);

            if (!string.IsNullOrEmpty(DSHubServerUrl))
            {
                if(DSHubServerUrl.EndsWith("/"))
                {
                    DSHubServerUrl = DSHubServerUrl.Remove(DSHubServerUrl.Length - 1);
                }

                this.DSHubServerWsUrl = DSHubServerUrl.Replace("https://", "wss://");
                DSHubServerWsUrl += '/';
            }

            this.AgreementServerUrl = this.ExpanServiceApiUrl(this.AgreementServerUrl, "/agreement", forceExpandServiceApiUrl);

            this.DSMControllerServerUrl = this.ExpanServiceApiUrl(this.DSMControllerServerUrl, "/dsmcontroller", forceExpandServiceApiUrl);

            this.PlatformServerUrl = this.ExpanServiceApiUrl(this.PlatformServerUrl, "/platform", forceExpandServiceApiUrl);

            this.StatisticServerUrl = this.ExpanServiceApiUrl(this.StatisticServerUrl, "/social", forceExpandServiceApiUrl);

            this.UGCServerUrl = this.ExpanServiceApiUrl(this.UGCServerUrl, "/ugc", forceExpandServiceApiUrl);

            this.QosManagerServerUrl = this.ExpanServiceApiUrl(this.QosManagerServerUrl, "/qosm", forceExpandServiceApiUrl);

            this.GameTelemetryServerUrl = this.ExpanServiceApiUrl(this.GameTelemetryServerUrl, "/game-telemetry", forceExpandServiceApiUrl);

            this.AchievementServerUrl = this.ExpanServiceApiUrl(this.AchievementServerUrl, "/achievement", forceExpandServiceApiUrl);

            this.LobbyServerUrl = this.ExpanServiceApiUrl(this.LobbyServerUrl, "/lobby", forceExpandServiceApiUrl);
            
            this.SessionServerUrl = this.ExpanServiceApiUrl(this.SessionServerUrl, "/session", forceExpandServiceApiUrl);

            this.CloudSaveServerUrl = this.ExpanServiceApiUrl(this.CloudSaveServerUrl, "/cloudsave", forceExpandServiceApiUrl);

            this.ChallengeServerUrl = this.ExpanServiceApiUrl(this.ChallengeServerUrl, "/challenge", forceExpandServiceApiUrl);
            
            this.InventoryServerUrl = this.ExpanServiceApiUrl(this.InventoryServerUrl, "/inventory", forceExpandServiceApiUrl);

            this.ProfanityFilterServerUrl = this.ExpanServiceApiUrl(this.ProfanityFilterServerUrl, "/profanity-filter", forceExpandServiceApiUrl);

            this.MatchmakingServerUrl = this.ExpanServiceApiUrl(this.MatchmakingServerUrl, "/matchmaking", forceExpandServiceApiUrl);
            
            this.MatchmakingV2ServerUrl = this.ExpanServiceApiUrl(this.MatchmakingV2ServerUrl, "/match2", forceExpandServiceApiUrl);

            this.SeasonPassServerUrl = this.ExpanServiceApiUrl(this.SeasonPassServerUrl, "/seasonpass", forceExpandServiceApiUrl);
            
            this.BasicServerUrl = this.ExpanServiceApiUrl(this.BasicServerUrl, "/basic", forceExpandServiceApiUrl);

            if (MaximumCacheSize <= 0)
            {
                logger?.LogWarning($"Invalid maximum cache size: ${MaximumCacheSize}\n. Set to default value: {defaultCacheSize}");
                MaximumCacheSize = defaultCacheSize;
            }

            if (MaximumCacheLifeTime <= 0)
            {
                logger?.LogWarning($"Invalid maximum cache lifetime: ${MaximumCacheLifeTime}\n. Set to default value: {defaultCacheLifeTime}");
                MaximumCacheLifeTime = defaultCacheLifeTime;
            }
                
            this.StatsDServerUrl = string.IsNullOrEmpty(this.StatsDServerUrl) ? defaultStatsDUrl : this.StatsDServerUrl;
            this.StatsDServerPort = this.StatsDServerPort <= 0 ? defaultStatsDPort : this.StatsDServerPort;
            this.StatsDMetricInterval = this.StatsDMetricInterval <= 0 ? defaultStatsDMetricInterval : this.StatsDMetricInterval;
        }

        /// <summary>
        ///  Remove config values that can be derived from another value.
        /// </summary>
        public void Compact()
        {
            int index;
            // remove protocol
            if ((index = this.BaseUrl.IndexOf("://")) > 0) this.BaseUrl = this.BaseUrl.Substring(index + 3);

            if (this.BaseUrl == null) return;
            string httpBaseUrl = "https://" + this.BaseUrl;

            if (this.IamServerUrl == httpBaseUrl + "/iam") this.IamServerUrl = null;

            if (this.DSHubServerUrl == httpBaseUrl + "/dshub") this.DSMControllerServerUrl = null;

            if (this.AgreementServerUrl == httpBaseUrl + "/agreement") this.AgreementServerUrl = null;
            
            if (this.DSMControllerServerUrl == httpBaseUrl + "/dsmcontroller") this.DSMControllerServerUrl = null;

            if (this.PlatformServerUrl == httpBaseUrl + "/platform") this.PlatformServerUrl = null;

            if (this.StatisticServerUrl == httpBaseUrl + "/statistic") this.StatisticServerUrl = null;

            if (this.UGCServerUrl == httpBaseUrl + "/ugc") this.UGCServerUrl = null;

            if (this.QosManagerServerUrl == httpBaseUrl + "/qosm") this.QosManagerServerUrl = null;

            if (this.GameTelemetryServerUrl == httpBaseUrl + "/game-telemetry") this.GameTelemetryServerUrl = null;

            if (this.AchievementServerUrl == httpBaseUrl + "/achievement") this.AchievementServerUrl = null;

            if (this.LobbyServerUrl == httpBaseUrl + "/lobby") this.LobbyServerUrl = null;
            
            if (this.SessionServerUrl == httpBaseUrl + "/session") this.SessionServerUrl = null;

            if (this.CloudSaveServerUrl == httpBaseUrl + "/cloudsave") this.CloudSaveServerUrl = null;

            if (this.ChallengeServerUrl == httpBaseUrl + "/challenge") this.ChallengeServerUrl = null;

            if (this.InventoryServerUrl == httpBaseUrl + "/inventory") this.InventoryServerUrl = null;

            if (this.ProfanityFilterServerUrl == httpBaseUrl + "/profanity-filter") this.ProfanityFilterServerUrl = null;

            if (this.MatchmakingServerUrl == httpBaseUrl + "/matchmaking") this.MatchmakingServerUrl = null;
            
            if (this.MatchmakingV2ServerUrl == httpBaseUrl + "/match2") this.MatchmakingV2ServerUrl = null;

            if (this.SeasonPassServerUrl == httpBaseUrl + "/seasonpass") this.SeasonPassServerUrl = null;
            
            if(this.BasicServerUrl == httpBaseUrl + "/basic")
            {
                this.BasicServerUrl = null;
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace)) throw new System.Exception("Init AccelByte SDK failed, Server Namespace must not null or empty.");

            if (string.IsNullOrEmpty(this.BaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Server Base URL must not null or empty.");
        }

        public bool IsRequiredFieldEmpty()
        {
            if (string.IsNullOrEmpty(this.Namespace)) return true;

            if (string.IsNullOrEmpty(this.BaseUrl)) return true;

            return false;
        }

        /// <summary>
        /// Set services URL.
        /// </summary>
        /// <param name="specificServerUrl">The specific URL, if empty will be replaced by baseUrl+defaultUrl.</param>
        /// <param name="defaultServerUrl">The default URL, will be used if specific URL is empty.</param>
        /// <returns></returns>
        private string ExpanServiceApiUrl(string specificServerUrl, string defaultServerUrl, bool forceExpand)
        {
            if (string.IsNullOrEmpty(specificServerUrl) || forceExpand)
            {
                return string.Format("{0}{1}", BaseUrl, defaultServerUrl);
            }

            return specificServerUrl;
        }

        public void SanitizeBaseUrl()
        {
            string sanitizedUrl = string.Empty;
            try
            {
                sanitizedUrl = Utils.UrlUtils.SanitizeBaseUrl(this.BaseUrl);
            }
            catch (System.Exception ex)
            {
                logger?.LogWarning("Invalid Server Config BaseUrl: " + ex.Message);
            }
            this.BaseUrl = sanitizedUrl;
        }
    }

    [DataContract, Preserve]
    public class MultiServerConfigs : IAccelByteMultiConfigs
    {
        [DataMember] public ServerConfig Development;
        [DataMember] public ServerConfig Certification;
        [DataMember] public ServerConfig Production;
        [DataMember] public ServerConfig Sandbox;
        [DataMember] public ServerConfig Integration;
        [DataMember] public ServerConfig QA;
        [DataMember] public ServerConfig Default;

        public void Expand(bool forceExpandServiceApiUrl = false)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                ServerConfig envConfig = fieldInfo.GetValue(this) as ServerConfig;
                if (envConfig == null)
                {
                    envConfig = new ServerConfig();
                }
                envConfig.SanitizeBaseUrl();
                envConfig.Expand(forceExpandServiceApiUrl);

                fieldInfo.SetValue(this, envConfig);
            }
        }

        public void InitializeNullEnv()
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                var envConfig = fieldInfo.GetValue(this) as ServerConfig;
                if (envConfig == null)
                {
                    envConfig = new ServerConfig();
                    fieldInfo.SetValue(this, envConfig);
                }
            }
        }

        public ServerConfig GetConfigFromEnvironment(SettingsEnvironment targetEnvironment)
        {
            ServerConfig retval = null;
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    retval = fieldInfo.GetValue(this) as ServerConfig;
                    break;
                }
            }

            return retval;
        }

        public void SetConfigToEnv(ServerConfig newConfig, SettingsEnvironment targetEnvironment)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    fieldInfo.SetValue(this, newConfig);
                    break;
                }
            }
        }

        IAccelByteConfig IAccelByteMultiConfigs.GetConfigFromEnvironment(SettingsEnvironment targetEnvironment)
        {
            return GetConfigFromEnvironment(targetEnvironment);
        }

        void IAccelByteMultiConfigs.SetConfigToEnv(IAccelByteConfig newConfig, SettingsEnvironment targetEnvironment)
        {
            if (newConfig is ServerConfig)
            {
                SetConfigToEnv(newConfig as ServerConfig, targetEnvironment);
            }
        }
    }
}
