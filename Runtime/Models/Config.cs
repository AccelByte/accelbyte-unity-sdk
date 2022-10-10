// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using AccelByte.Api;

namespace AccelByte.Models
{
    /// <summary>
    /// Primarily used by the Editor for the config in the popup menu.
    /// <para>Looking for runtime settings? See static AccelBytePlugin.Config</para>
    /// </summary>
    [DataContract]
    public class Config
    {
        [DataMember] public string Namespace { get; set; }
        [DataMember] public bool UsePlayerPrefs { get; set; }
        [DataMember] public bool EnableDebugLog { get; set; }
        [DataMember] public string DebugLogFilter { get; set; }
        [DataMember] public string BaseUrl { get; set; }
        [DataMember] public string IamServerUrl { get; set; }
        [DataMember] public string PlatformServerUrl { get; set; }
        [DataMember] public string BasicServerUrl { get; set; }
        [DataMember] public string LobbyServerUrl { get; set; }
        [DataMember] public string CloudStorageServerUrl { get; set; }
        [DataMember] public string GameProfileServerUrl { get; set; }
        [DataMember] public string StatisticServerUrl { get; set; }
        [DataMember] public string QosManagerServerUrl { get; set; }
        [DataMember] public string AgreementServerUrl { get; set; }
        [DataMember] public string LeaderboardServerUrl { get; set; }
        [DataMember] public string CloudSaveServerUrl { get; set; }
        [DataMember] public string GameTelemetryServerUrl { get; set; }
        [DataMember] public string AchievementServerUrl { get; set; }
        [DataMember] public string UGCServerUrl { get; set; }
        [DataMember] public string ReportingServerUrl { get; set; }
        [DataMember] public string SeasonPassServerUrl { get; set; }
        [DataMember] public string SessionBrowserServerUrl { get; set; }
        [DataMember] public bool UseTurnManager { get; set; }
        [DataMember] public string TurnManagerServerUrl { get; set; }
        [DataMember] public string TurnServerHost { get; set; }
        [DataMember] public string TurnServerPort { get; set; }
        [DataMember] public string TurnServerPassword { get; set; }
        [DataMember] public string TurnServerSecret { get; set; }
        [DataMember] public string TurnServerUsername { get; set; }
        [DataMember] public string GroupServerUrl { get; set; }
        [DataMember] public string RedirectUri { get; set; }
        [DataMember] public string AppId { get; set; }
        [DataMember] public string PublisherNamespace { get; set; }

        /// <summary>
        ///  Copy member values
        /// </summary>
        public Config ShallowCopy()
        {
            return (Config) MemberwiseClone();
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
                   this.GameProfileServerUrl == anotherConfig.GameProfileServerUrl &&
                   this.StatisticServerUrl == anotherConfig.StatisticServerUrl &&
                   this.QosManagerServerUrl == anotherConfig.QosManagerServerUrl &&
                   this.AgreementServerUrl == anotherConfig.AgreementServerUrl &&
                   this.LeaderboardServerUrl == anotherConfig.LeaderboardServerUrl &&
                   this.CloudSaveServerUrl == anotherConfig.CloudSaveServerUrl &&
                   this.GameTelemetryServerUrl == anotherConfig.GameTelemetryServerUrl &&
                   this.AchievementServerUrl == anotherConfig.AchievementServerUrl &&
                   this.GroupServerUrl == anotherConfig.GroupServerUrl &&
                   this.UGCServerUrl == anotherConfig.UGCServerUrl &&
                   this.SeasonPassServerUrl == anotherConfig.SeasonPassServerUrl &&
                   this.SessionBrowserServerUrl == anotherConfig.SessionBrowserServerUrl &&
                   this.TurnManagerServerUrl == anotherConfig.TurnManagerServerUrl &&
                   this.UseTurnManager == anotherConfig.UseTurnManager &&
                   this.TurnServerHost == anotherConfig.TurnServerHost &&
                   this.TurnServerPort == anotherConfig.TurnServerPort &&
                   this.TurnServerUsername == anotherConfig.TurnServerUsername &&
                   this.TurnServerPassword == anotherConfig.TurnServerPassword &&
                   this.TurnServerSecret == anotherConfig.TurnServerSecret &&
                   this.RedirectUri == anotherConfig.RedirectUri &&
                   this.AppId == anotherConfig.AppId &&
                   this.PublisherNamespace == anotherConfig.PublisherNamespace;
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand()
        {
            if (this.BaseUrl != null)
            {
                int index;
                // remove protocol
                string baseUrl = this.BaseUrl;
                if ((index = baseUrl.IndexOf("://")) > 0) baseUrl = baseUrl.Substring(index + 3);

                string httpsBaseUrl = "https://" + baseUrl;
                string wssBaseUrl = "wss://" + baseUrl;

                this.IamServerUrl = GetDefaultApiUrl(this.IamServerUrl, "/iam");

                this.PlatformServerUrl = GetDefaultApiUrl(this.PlatformServerUrl, "/platform");

                this.BasicServerUrl = GetDefaultApiUrl(this.BasicServerUrl, "/basic");

                if (string.IsNullOrEmpty(this.LobbyServerUrl)) this.LobbyServerUrl = wssBaseUrl + "/lobby/";

                this.CloudStorageServerUrl = GetDefaultApiUrl(this.CloudStorageServerUrl, "/social");

                this.GameProfileServerUrl = GetDefaultApiUrl(this.GameProfileServerUrl, "/social");

                this.StatisticServerUrl = GetDefaultApiUrl(this.StatisticServerUrl, "/social");

                this.QosManagerServerUrl = GetDefaultApiUrl(this.QosManagerServerUrl, "/qosm");

                this.AgreementServerUrl = GetDefaultApiUrl(this.AgreementServerUrl, "/agreement");

                this.LeaderboardServerUrl = GetDefaultApiUrl(this.LeaderboardServerUrl, "/leaderboard");

                this.CloudSaveServerUrl = GetDefaultApiUrl(this.CloudSaveServerUrl, "/cloudsave");

                this.GameTelemetryServerUrl = GetDefaultApiUrl(this.GameTelemetryServerUrl, "/game-telemetry");

                this.AchievementServerUrl = GetDefaultApiUrl(this.AchievementServerUrl, "/achievement");

                this.GroupServerUrl = GetDefaultApiUrl(this.GroupServerUrl, "/group");

                this.UGCServerUrl = GetDefaultApiUrl(this.UGCServerUrl, "/ugc");

                this.ReportingServerUrl = GetDefaultApiUrl(this.ReportingServerUrl, "/reporting");

                this.SeasonPassServerUrl = GetDefaultApiUrl(this.SeasonPassServerUrl, "/seasonpass");
                
                this.SessionBrowserServerUrl = GetDefaultApiUrl(this.SessionBrowserServerUrl, "/sessionbrowser");
                
                this.TurnManagerServerUrl = GetDefaultApiUrl(this.TurnManagerServerUrl, "/turnmanager");

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

                if (this.GameTelemetryServerUrl == httpsBaseUrl + "/game-telemetry") this.GameTelemetryServerUrl = null;

                if (this.AchievementServerUrl == httpsBaseUrl + "/achievement") this.AchievementServerUrl = null;
                
                if (this.GroupServerUrl == httpsBaseUrl + "/group") this.GroupServerUrl = null;

                if (this.UGCServerUrl == httpsBaseUrl + "/ugc") this.UGCServerUrl = null;

                if (this.ReportingServerUrl == httpsBaseUrl + "/reporting") this.ReportingServerUrl = null;

                if (this.SeasonPassServerUrl == httpsBaseUrl + "/seasonpass") this.SeasonPassServerUrl = null;

                if (this.SessionBrowserServerUrl == httpsBaseUrl + "/sessionbrowser") this.SessionBrowserServerUrl = null;

                if (this.TurnManagerServerUrl == httpsBaseUrl + "/turnmanager") this.TurnManagerServerUrl = null;
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace)) throw new System.Exception("Init AccelByte SDK failed, Namespace must not null or empty.");

            if (string.IsNullOrEmpty(this.BaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Base URL must not null or empty.");

            if (string.IsNullOrEmpty(this.RedirectUri)) throw new System.Exception("Init AccelByte SDK failed, Redirect URI must not null or empty.");
        }

        public bool IsRequiredFieldEmpty()
        {
            if (string.IsNullOrEmpty(this.Namespace)) return true;

            if (string.IsNullOrEmpty(this.BaseUrl)) return true;

            if (string.IsNullOrEmpty(this.RedirectUri)) return true;

            return false;
        }

        /// <summary>
        /// Set services URL.
        /// </summary>
        /// <param name="specificServerUrl">The specific URL, if empty will be replaced by baseUrl+defaultUrl.</param>
        /// <param name="defaultServerUrl">The default URL, will be used if specific URL is empty.</param>
        /// <returns></returns>
        private string GetDefaultApiUrl(string specificServerUrl, string defaultServerUrl)
        {
            if(string.IsNullOrEmpty(specificServerUrl))
            {
                return $"{this.BaseUrl}{defaultServerUrl}";
            }

            return specificServerUrl;
        }
    }

    [DataContract]
    public class OAuthConfig
    {
        [DataMember] public string ClientId { get; set; }
        [DataMember] public string ClientSecret { get; set; }

        /// <summary>
        ///  Copy member values
        /// </summary>
        public OAuthConfig ShallowCopy()
        {
            return (OAuthConfig)MemberwiseClone();
        }

        public bool Compare(OAuthConfig anotherConfig)
        {
            return this.ClientId == anotherConfig.ClientId &&
                   this.ClientSecret == anotherConfig.ClientSecret;
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand()
        {
            if (this.ClientSecret == null)
            {
                this.ClientSecret = "";
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.ClientId)) throw new System.Exception("Init AccelByte SDK failed, Client ID must not null or empty.");
        }

        public bool IsRequiredFieldEmpty()
        {
            return (string.IsNullOrEmpty(this.ClientId));
        }
    }

    [DataContract]
    public class MultiConfigs
    {
        [DataMember] public Config Development { get; set; }
        [DataMember] public Config Certification { get; set; }
        [DataMember] public Config Production { get; set; }
        [DataMember] public Config Default { get; set; }

        public void Expand()
        {
            if(Development == null)
            {
                Development = new Config();
                Development.Expand();
            }
            if (Certification == null)
            {
                Certification = new Config();
                Certification.Expand();
            }
            if (Production == null)
            {
                Production = new Config();
                Production.Expand();
            }
            if (Default == null)
            {
                Default = new Config();
                Default.Expand();
            }
        }
    }

    [DataContract]
    public class MultiOAuthConfigs
    {
        [DataMember] public OAuthConfig Development { get; set; }
        [DataMember] public OAuthConfig Certification { get; set; }
        [DataMember] public OAuthConfig Production { get; set; }
        [DataMember] public OAuthConfig Default { get; set; }

        public void Expand()
        {
            if (Development == null)
            {
                Development = new OAuthConfig();
                Development.Expand();
            }
            if (Certification == null)
            {
                Certification = new OAuthConfig();
                Certification.Expand();
            }
            if (Production == null)
            {
                Production = new OAuthConfig();
                Production.Expand();
            }
            if (Default == null)
            {
                Default = new OAuthConfig();
                Default.Expand();
            }
        }
    }

    [DataContract]
    public class VersionJson
    {
        [DataMember] public string Version { get; set; }
    }
}
