﻿// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using AccelByte.Api;
using AccelByte.Core;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    /// <summary>
    /// Primarily used by the Editor for the config in the popup menu.
    /// <para>Looking for runtime settings? See static AccelBytePlugin.Config</para>
    /// </summary>
    [DataContract, Preserve]
    public class Config : IAccelByteConfig
    {
        private const int defaultCacheSize = 100;
        private const int defaultCacheLifeTime = 100;
        private const bool defaultPresenceBroadcastEvent = true;
        private const int defaultPresenceBroadcastEvetntIntervalInSecond = 600;
        private const int defaultPresenceBroadcastEventGameState = 0;
        private const bool defaultPredefinedEvent = false;
        private const bool defaultGameTelemetryCacheEnabled = true;
        private const bool defaultGameTelemetryAutoSend = true;
        private const int defaultClientAnalyticsEventIntervalInSecond = 10;

        [DataMember] public string Namespace = "";
        [DataMember] public bool UsePlayerPrefs = false;
        [DataMember] public bool EnableDebugLog = true;
        [DataMember] public bool EnhancedServiceLogging = false;
        [DataMember] public string DebugLogFilter = "Verbose";
        [DataMember] public bool RandomizeDeviceId = false;
        [DataMember] public string BaseUrl = "";
        [DataMember] public string IamServerUrl = "";
        [DataMember] public string PlatformServerUrl = "";
        [DataMember] public string BasicServerUrl = "";
        [DataMember] public string ChallengeServerUrl = "";
        [DataMember] public string LobbyServerUrl = "";
        [DataMember] public string CloudStorageServerUrl = "";
        [DataMember] public string GameProfileServerUrl = "";
        [DataMember] public string StatisticServerUrl = "";
        [DataMember] public string QosManagerServerUrl = "";
        [DataMember] public string AgreementServerUrl = "";
        [DataMember] public string LeaderboardServerUrl = "";
        [DataMember] public string InventoryServerUrl = "";
        [DataMember] public string CloudSaveServerUrl = "";
        [DataMember] public string GameTelemetryServerUrl = "";
        [DataMember] public string AchievementServerUrl = "";
        [DataMember] public string UGCServerUrl = "";
        [DataMember] public string ReportingServerUrl = "";
        [DataMember] public string SeasonPassServerUrl = "";
        [DataMember] public string SessionServerUrl = "";
        [DataMember] public string MatchmakingV2ServerUrl = "";
        [DataMember] public bool UseTurnManager = true;
        [DataMember] public string TurnManagerServerUrl = "";
        [DataMember] public string LoginQueueServerUrl = "";
        [DataMember] public string TurnServerHost = "";
        [DataMember] public string TurnServerPort = "";
        [DataMember] public string TurnServerPassword = "";
        [DataMember] public string TurnServerSecret = "";
        [DataMember] public string TurnServerUsername = "";
        [DataMember] public int PeerMonitorIntervalMs = 200;
        [DataMember] public int PeerMonitorTimeoutMs = 2000;
        [DataMember] public int HostCheckTimeoutInSeconds = 60;
        [DataMember] public string GroupServerUrl = "";
        [DataMember] public string ChatServerWsUrl = "";
        [DataMember] public string ChatServerUrl = "";
        [DataMember] public string GdprServerUrl = "";
        [DataMember] public string RedirectUri = "";
        [DataMember] public string AppId = "";
        [DataMember] public string PublisherNamespace = "";
        [DataMember] public string CustomerName = "";
        [DataMember] public bool EnableAuthHandshake;
        [DataMember] public int MaximumCacheSize = defaultCacheSize;
        [DataMember] public int MaximumCacheLifeTime = defaultCacheLifeTime;
        [DataMember] public bool EnablePresenceBroadcastEvent = defaultPresenceBroadcastEvent;
        [DataMember] public int PresenceBroadcastEventInterval = defaultPresenceBroadcastEvetntIntervalInSecond;
        [DataMember] public int PresenceBroadcastEventGameState = defaultPresenceBroadcastEventGameState;
        [DataMember] public string PresenceBroadcastEventGameStateDescription = "";
        [DataMember] public bool EnablePreDefinedEvent = defaultPredefinedEvent;
        [DataMember] public bool EnableClientAnalyticsEvent = false;
        [DataMember] public float ClientAnalyticsEventInterval = defaultClientAnalyticsEventIntervalInSecond;
        [DataMember] public bool GameTelemetryCacheEnabled = defaultGameTelemetryCacheEnabled;
        [DataMember] public bool EnableGameTelemetryStartupAutoSend = defaultGameTelemetryAutoSend;
        [DataMember] public bool EnableAmsServerQos = false;
        [DataMember] public bool EnableMatchmakingTicketCheck = true;
        [DataMember] public int MatchmakingTicketCheckPollRate = 5000;
        [DataMember] public int MatchmakingTicketCheckInitialDelay = 30000;

        private IDebugger logger;
        /// <summary>
        ///  Copy member values
        /// </summary>
        public Config ShallowCopy()
        {
            return (Config) MemberwiseClone();
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }

        public bool Compare(Config anotherConfig)
        {
            return this.Namespace == anotherConfig.Namespace &&
                   this.UsePlayerPrefs == anotherConfig.UsePlayerPrefs &&
                   this.EnableDebugLog == anotherConfig.EnableDebugLog &&
                   this.DebugLogFilter == anotherConfig.DebugLogFilter &&
                   this.BaseUrl == anotherConfig.BaseUrl &&
                   this.IamServerUrl == anotherConfig.IamServerUrl &&
                   this.PlatformServerUrl == anotherConfig.PlatformServerUrl &&
                   this.BasicServerUrl == anotherConfig.BasicServerUrl &&
                   this.LobbyServerUrl == anotherConfig.LobbyServerUrl &&
                   this.CloudStorageServerUrl == anotherConfig.CloudStorageServerUrl &&
                   this.ChallengeServerUrl == anotherConfig.ChallengeServerUrl &&
                   this.GameProfileServerUrl == anotherConfig.GameProfileServerUrl &&
                   this.StatisticServerUrl == anotherConfig.StatisticServerUrl &&
                   this.QosManagerServerUrl == anotherConfig.QosManagerServerUrl &&
                   this.AgreementServerUrl == anotherConfig.AgreementServerUrl &&
                   this.LeaderboardServerUrl == anotherConfig.LeaderboardServerUrl &&
                   this.CloudSaveServerUrl == anotherConfig.CloudSaveServerUrl &&
                   this.InventoryServerUrl == anotherConfig.InventoryServerUrl &&
                   this.GameTelemetryServerUrl == anotherConfig.GameTelemetryServerUrl &&
                   this.AchievementServerUrl == anotherConfig.AchievementServerUrl &&
                   this.GroupServerUrl == anotherConfig.GroupServerUrl &&
                   this.UGCServerUrl == anotherConfig.UGCServerUrl &&
                   this.SeasonPassServerUrl == anotherConfig.SeasonPassServerUrl &&
                   this.SessionServerUrl == anotherConfig.SessionServerUrl &&
                   this.MatchmakingV2ServerUrl == anotherConfig.MatchmakingV2ServerUrl &&
                   this.TurnManagerServerUrl == anotherConfig.TurnManagerServerUrl &&
                   this.LoginQueueServerUrl == anotherConfig.LoginQueueServerUrl &&
                   this.UseTurnManager == anotherConfig.UseTurnManager &&
                   this.TurnServerHost == anotherConfig.TurnServerHost &&
                   this.TurnServerPort == anotherConfig.TurnServerPort &&
                   this.TurnServerUsername == anotherConfig.TurnServerUsername &&
                   this.TurnServerPassword == anotherConfig.TurnServerPassword &&
                   this.TurnServerSecret == anotherConfig.TurnServerSecret &&
                   this.ChatServerWsUrl == anotherConfig.ChatServerWsUrl &&
                   this.ChatServerUrl == anotherConfig.ChatServerUrl &&
                   this.RedirectUri == anotherConfig.RedirectUri &&
                   this.AppId == anotherConfig.AppId &&
                   this.PublisherNamespace == anotherConfig.PublisherNamespace &&
                   this.CustomerName == anotherConfig.CustomerName &&
                   this.EnableAuthHandshake == anotherConfig.EnableAuthHandshake &&
                   this.MaximumCacheSize == anotherConfig.MaximumCacheSize &&
                   this.MaximumCacheLifeTime == anotherConfig.MaximumCacheLifeTime &&
                   this.PeerMonitorIntervalMs == anotherConfig.PeerMonitorIntervalMs &&
                   this.PeerMonitorTimeoutMs == anotherConfig.PeerMonitorTimeoutMs &&
                   this.HostCheckTimeoutInSeconds == anotherConfig.HostCheckTimeoutInSeconds &&
                   this.EnablePresenceBroadcastEvent == anotherConfig.EnablePresenceBroadcastEvent &&
                   this.GameTelemetryCacheEnabled == anotherConfig.GameTelemetryCacheEnabled &&
                   this.EnableGameTelemetryStartupAutoSend == anotherConfig.EnableGameTelemetryStartupAutoSend &&
                   this.PresenceBroadcastEventInterval == anotherConfig.PresenceBroadcastEventInterval &&
                   this.PresenceBroadcastEventGameState == anotherConfig.PresenceBroadcastEventGameState &&
                   this.PresenceBroadcastEventGameStateDescription == anotherConfig.PresenceBroadcastEventGameStateDescription &&
                   this.EnablePreDefinedEvent == anotherConfig.EnablePreDefinedEvent &&
                   this.EnableMatchmakingTicketCheck == anotherConfig.EnableMatchmakingTicketCheck &&
                   this.MatchmakingTicketCheckPollRate == anotherConfig.MatchmakingTicketCheckPollRate &&
                   this.MatchmakingTicketCheckInitialDelay == anotherConfig.MatchmakingTicketCheckInitialDelay;
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand(bool forceExpandServiceApiUrl = false)
        {
            if(Namespace == null)
            {
                Namespace = "";
            }
            if (string.IsNullOrEmpty(DebugLogFilter))
            {
                DebugLogFilter = "Verbose";
            }
            if (BaseUrl == null)
            {
                BaseUrl = "";
            }
            if (IamServerUrl == null)
            {
                IamServerUrl = "";
            }
            if (PlatformServerUrl == null)
            {
                PlatformServerUrl = "";
            }
            if (BasicServerUrl == null)
            {
                BasicServerUrl = "";
            }
            if (LobbyServerUrl == null)
            {
                LobbyServerUrl = "";
            }
            if (CloudStorageServerUrl == null)
            {
                CloudStorageServerUrl = "";
            }
            if (GameProfileServerUrl == null)
            {
                GameProfileServerUrl = "";
            }
            if (StatisticServerUrl == null)
            {
                StatisticServerUrl = "";
            }
            if (QosManagerServerUrl == null)
            {
                QosManagerServerUrl = "";
            }
            if (AgreementServerUrl == null)
            {
                AgreementServerUrl = "";
            }
            if (LeaderboardServerUrl == null)
            {
                LeaderboardServerUrl = "";
            }
            if (CloudSaveServerUrl == null)
            {
                CloudSaveServerUrl = "";
            }
            if (GameTelemetryServerUrl == null)
            {
                GameTelemetryServerUrl = "";
            }
            if (UGCServerUrl == null)
            {
                UGCServerUrl = "";
            }
            if (ReportingServerUrl == null)
            {
                ReportingServerUrl = "";
            }
            if (SeasonPassServerUrl == null)
            {
                SeasonPassServerUrl = "";
            }
            if (SessionServerUrl == null)
            {
                SessionServerUrl = "";
            }
            if (MatchmakingV2ServerUrl == null)
            {
                MatchmakingV2ServerUrl = "";
            }
            if (TurnManagerServerUrl == null)
            {
                TurnManagerServerUrl = "";
            }
            if (LoginQueueServerUrl == null)
            {
                LoginQueueServerUrl = "";
            }
            if (TurnServerHost == null)
            {
                TurnServerHost = "";
            }
            if (TurnServerPort == null)
            {
                TurnServerPort = "";
            }
            if (TurnServerPassword == null)
            {
                TurnServerPassword = "";
            }
            if (TurnServerSecret == null)
            {
                TurnServerSecret = "";
            }
            if (TurnServerUsername == null)
            {
                TurnServerUsername = "";
            }
            if (GroupServerUrl == null)
            {
                GroupServerUrl = "";
            }
            if (ChatServerWsUrl == null)
            {
                ChatServerWsUrl = "";
            }
            if (ChatServerUrl == null)
            {
                ChatServerUrl = "";
            }
            if (ChallengeServerUrl == null)
            {
                ChallengeServerUrl = "";
            }
            if (InventoryServerUrl == null)
            {
                InventoryServerUrl = "";
            }
            if (RedirectUri == null)
            {
                RedirectUri = "";
            }
            if (AppId == null)
            {
                AppId = "";
            }
            if (PublisherNamespace == null)
            {
                PublisherNamespace = "";
            }
            if (CustomerName == null)
            {
                CustomerName = "";
            }

            if(MaximumCacheSize <= 0)
            {
                logger?.LogWarning($"Invalid maximum cache size: ${MaximumCacheSize}\n. Set to default value: {defaultCacheSize}");
                MaximumCacheSize = defaultCacheSize;
            }

            if (MaximumCacheLifeTime <= 0)
            {
                logger?.LogWarning($"Invalid maximum cache lifetime: ${MaximumCacheLifeTime}\n. Set to default value: {defaultCacheLifeTime}");
                MaximumCacheLifeTime = defaultCacheLifeTime;
            }

            if (!string.IsNullOrEmpty(this.BaseUrl))
            {
                int index;
                // remove protocol
                string baseUrl = this.BaseUrl;
                if ((index = baseUrl.IndexOf("://")) > 0)
                {
                    baseUrl = baseUrl.Substring(index + 3);
                }

                string wssBaseUrl = "wss://" + baseUrl;

                this.IamServerUrl = ExpandServiceApiUrl(this.IamServerUrl, "/iam", forceExpandServiceApiUrl);

                this.PlatformServerUrl = ExpandServiceApiUrl(this.PlatformServerUrl, "/platform", forceExpandServiceApiUrl);

                this.BasicServerUrl = ExpandServiceApiUrl(this.BasicServerUrl, "/basic", forceExpandServiceApiUrl);

                if (string.IsNullOrEmpty(this.LobbyServerUrl) || forceExpandServiceApiUrl)
                {
                    this.LobbyServerUrl = wssBaseUrl + "/lobby/";
                }

                this.CloudStorageServerUrl = ExpandServiceApiUrl(this.CloudStorageServerUrl, "/social", forceExpandServiceApiUrl);

                this.GameProfileServerUrl = ExpandServiceApiUrl(this.GameProfileServerUrl, "/social", forceExpandServiceApiUrl);

                this.StatisticServerUrl = ExpandServiceApiUrl(this.StatisticServerUrl, "/social", forceExpandServiceApiUrl);

                string qosUrl = EnableAmsServerQos ? "/ams-qosm" : "/qosm";
                this.QosManagerServerUrl = ExpandServiceApiUrl(this.QosManagerServerUrl, qosUrl, forceExpandServiceApiUrl);

                this.AgreementServerUrl = ExpandServiceApiUrl(this.AgreementServerUrl, "/agreement", forceExpandServiceApiUrl);

                this.LeaderboardServerUrl = ExpandServiceApiUrl(this.LeaderboardServerUrl, "/leaderboard", forceExpandServiceApiUrl);

                this.CloudSaveServerUrl = ExpandServiceApiUrl(this.CloudSaveServerUrl, "/cloudsave", forceExpandServiceApiUrl);

                this.GameTelemetryServerUrl = ExpandServiceApiUrl(this.GameTelemetryServerUrl, "/game-telemetry", forceExpandServiceApiUrl);

                this.AchievementServerUrl = ExpandServiceApiUrl(this.AchievementServerUrl, "/achievement", forceExpandServiceApiUrl);

                this.GroupServerUrl = ExpandServiceApiUrl(this.GroupServerUrl, "/group", forceExpandServiceApiUrl);

                this.UGCServerUrl = ExpandServiceApiUrl(this.UGCServerUrl, "/ugc", forceExpandServiceApiUrl);

                this.ReportingServerUrl = ExpandServiceApiUrl(this.ReportingServerUrl, "/reporting", forceExpandServiceApiUrl);

                this.SeasonPassServerUrl = ExpandServiceApiUrl(this.SeasonPassServerUrl, "/seasonpass", forceExpandServiceApiUrl);

                this.SessionServerUrl = ExpandServiceApiUrl(this.SessionServerUrl, "/session", forceExpandServiceApiUrl);

                this.MatchmakingV2ServerUrl = ExpandServiceApiUrl(this.MatchmakingV2ServerUrl, "/match2", forceExpandServiceApiUrl);

                this.TurnManagerServerUrl = ExpandServiceApiUrl(this.TurnManagerServerUrl, "/turnmanager", forceExpandServiceApiUrl);

                this.ChatServerUrl = ExpandServiceApiUrl(this.ChatServerUrl, "/chat", forceExpandServiceApiUrl);
                
                this.ChallengeServerUrl = ExpandServiceApiUrl(this.ChallengeServerUrl, "/challenge", forceExpandServiceApiUrl);

                this.InventoryServerUrl = ExpandServiceApiUrl(this.InventoryServerUrl, "/inventory", forceExpandServiceApiUrl);

                this.LoginQueueServerUrl = ExpandServiceApiUrl(this.LoginQueueServerUrl, "/login-queue", forceExpandServiceApiUrl);

                if (string.IsNullOrEmpty(this.ChatServerWsUrl) || forceExpandServiceApiUrl)
                {
                    this.ChatServerWsUrl = wssBaseUrl + "/chat/";
                }

                this.GdprServerUrl = ExpandServiceApiUrl(this.GdprServerUrl, "/gdpr", forceExpandServiceApiUrl);
            }
        }

        /// <summary>
        ///  Remove config values that can be derived from another value.
        /// </summary>
        public void Compact()
        {
            int index;
            // remove protocol
            string baseUrl = this.BaseUrl;
            if ((index = baseUrl.IndexOf("://")) > 0) baseUrl = baseUrl.Substring(index + 3);

            if (baseUrl != null)
            {
                string httpsBaseUrl = "https://" + baseUrl;
                string wssBaseUrl = "wss://" + baseUrl;

                if (this.IamServerUrl == httpsBaseUrl + "/iam") this.IamServerUrl = null;

                if (this.PlatformServerUrl == httpsBaseUrl + "/platform") this.PlatformServerUrl = null;

                if (this.BasicServerUrl == httpsBaseUrl + "/basic") this.BasicServerUrl = null;

                if (this.LobbyServerUrl == wssBaseUrl + "/lobby/") this.LobbyServerUrl = null;

                if (this.CloudStorageServerUrl == httpsBaseUrl + "/social") this.CloudStorageServerUrl = null;

                if (this.GameProfileServerUrl == httpsBaseUrl + "/social") this.GameProfileServerUrl = null;

                if (this.StatisticServerUrl == httpsBaseUrl + "/social") this.StatisticServerUrl = null;

                if (this.QosManagerServerUrl == httpsBaseUrl + "/qosm") this.QosManagerServerUrl = null;

                if (this.AgreementServerUrl == httpsBaseUrl + "/agreement") this.AgreementServerUrl = null;

                if (this.LeaderboardServerUrl == httpsBaseUrl + "/leaderboard") this.LeaderboardServerUrl = null;

                if (this.CloudSaveServerUrl == httpsBaseUrl + "/cloudsave") this.CloudSaveServerUrl = null;
                
                if (this.ChallengeServerUrl == httpsBaseUrl + "/challenge") this.ChallengeServerUrl = null;

                if (this.InventoryServerUrl == httpsBaseUrl + "/inventory") this.InventoryServerUrl = null;

                if (this.GameTelemetryServerUrl == httpsBaseUrl + "/game-telemetry") this.GameTelemetryServerUrl = null;

                if (this.AchievementServerUrl == httpsBaseUrl + "/achievement") this.AchievementServerUrl = null;

                if (this.GroupServerUrl == httpsBaseUrl + "/group") this.GroupServerUrl = null;

                if (this.UGCServerUrl == httpsBaseUrl + "/ugc") this.UGCServerUrl = null;

                if (this.ReportingServerUrl == httpsBaseUrl + "/reporting") this.ReportingServerUrl = null;

                if (this.SeasonPassServerUrl == httpsBaseUrl + "/seasonpass") this.SeasonPassServerUrl = null;

                if (this.SessionServerUrl == httpsBaseUrl + "/session") this.SessionServerUrl = null;

                if (this.MatchmakingV2ServerUrl == httpsBaseUrl + "/match2") this.MatchmakingV2ServerUrl = null;

                if (this.TurnManagerServerUrl == httpsBaseUrl + "/turnmanager") this.TurnManagerServerUrl = null;

                if (this.ChatServerWsUrl == wssBaseUrl + "/chat/") this.ChatServerWsUrl = null;

                if (this.ChatServerUrl == httpsBaseUrl + "/chat") this.ChatServerUrl = null;

                if (this.LoginQueueServerUrl == httpsBaseUrl + "/login-queue")
                {
                    this.LoginQueueServerUrl = null;
                }
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace))
            {
                throw new System.Exception("Init AccelByte SDK failed, Namespace must not null or empty.");
            }
            if (string.IsNullOrEmpty(this.BaseUrl))
            {
                throw new System.Exception("Init AccelByte SDK failed, Base URL must not null or empty.");
            }
            if (string.IsNullOrEmpty(this.RedirectUri))
            {
                throw new System.Exception("Init AccelByte SDK failed, Redirect URI must not null or empty.");
            }
        }

        public bool IsRequiredFieldEmpty()
        {
            System.Collections.Generic.List<string> checkedStringFields = new System.Collections.Generic.List<string>()
            {
                Namespace,
                BaseUrl,
                RedirectUri
            };
            var retval = checkedStringFields.Exists((field) => string.IsNullOrEmpty(field));
            return retval;
        }

        /// <summary>
        /// Set services URL.
        /// </summary>
        /// <param name="specificServerUrl">The specific URL, if empty will be replaced by baseUrl+defaultUrl.</param>
        /// <param name="defaultServerUrl">The default URL, will be used if specific URL is empty.</param>
        /// <returns></returns>
        private string ExpandServiceApiUrl(string specificServerUrl, string defaultServerUrl, bool forceExpand)
        {
            if(string.IsNullOrEmpty(specificServerUrl) || forceExpand)
            {
                return $"{this.BaseUrl}{defaultServerUrl}";
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
                logger?.LogWarning("Invalid Client Config BaseUrl: " + ex.Message);
            }
            this.BaseUrl = sanitizedUrl;
        }
    }

    [DataContract, Preserve]
    public class MultiConfigs : IAccelByteMultiConfigs
    {
        [DataMember] public Config Development;
        [DataMember] public Config Certification;
        [DataMember] public Config Production;
        [DataMember] public Config Sandbox;
        [DataMember] public Config Integration;
        [DataMember] public Config QA;
        [DataMember] public Config Default;

        public void Expand(bool forceExpandServiceUrl = false)
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                Config envConfig = fieldInfo.GetValue(this) as Config;
                if (envConfig == null)
                {
                    envConfig = new Config();
                }
                envConfig.SanitizeBaseUrl();
                envConfig.Expand(forceExpandServiceUrl);

                fieldInfo.SetValue(this, envConfig);
            }
        }

        public void InitializeNullEnv()
        {
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                var envConfig = fieldInfo.GetValue(this) as Config;
                if (envConfig == null)
                {
                    envConfig = new Config();
                    fieldInfo.SetValue(this, envConfig);
                }
            }
        }

        public Config GetConfigFromEnvironment(SettingsEnvironment targetEnvironment)
        {
            Config retval = null;
            System.Reflection.FieldInfo[] fieldInfos = GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == targetEnvironment.ToString())
                {
                    retval = fieldInfo.GetValue(this) as Config;
                    break;
                }
            }

            return retval;
        }

        public void SetConfigToEnv(Config newConfig, SettingsEnvironment targetEnvironment)
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
            if (newConfig is Config)
            {
                SetConfigToEnv(newConfig as Config, targetEnvironment);
            }
        }
    }

    [DataContract, Preserve]
    public class VersionJson
    {
        [DataMember(Name = "version")] public string Version;
    }
}
