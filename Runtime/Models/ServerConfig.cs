// Copyright (c) 2020-2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models {
    [DataContract]
    public class ServerConfig
    {
        [DataMember] public string Namespace { get; set; }
        [DataMember] public string BaseUrl { get; set; }
        [DataMember] public string IamServerUrl { get; set; }
        [DataMember] public string DSMControllerServerUrl { get; set; }
        [DataMember] public string StatisticServerUrl { get; set; }
        [DataMember] public string PlatformServerUrl { get; set; }
        [DataMember] public string QosManagerServerUrl { get; set; }
        [DataMember] public string GameTelemetryServerUrl { get; set; }
        [DataMember] public string AchievementServerUrl { get; set; }
        [DataMember] public string LobbyServerUrl { get; set; }
        [DataMember] public string CloudSaveServerUrl { get; set; }
        [DataMember] public string ClientId { get; set; }
        [DataMember] public string ClientSecret { get; set; }
        [DataMember] public string RedirectUri { get; set; }
        [DataMember] public string MatchmakingServerUrl { get; set; }
        [DataMember] public string SeasonPassServerUrl { get; set; }


        /// <summary>
        ///  Copy member values
        /// </summary>
        public ServerConfig ShallowCopy()
        {
            return (ServerConfig) MemberwiseClone();
        }

        /// <summary>
        ///  Assign missing config values.
        /// </summary>
        public void Expand()
        {
            if (this.BaseUrl == null) return;
            
            this.IamServerUrl = this.GetDefaultServerApiUrl(this.IamServerUrl, "/iam");

            this.DSMControllerServerUrl = this.GetDefaultServerApiUrl(this.DSMControllerServerUrl, "/dsmcontroller");

            this.PlatformServerUrl = this.GetDefaultServerApiUrl(this.PlatformServerUrl, "/platform");

            this.StatisticServerUrl = this.GetDefaultServerApiUrl(this.StatisticServerUrl, "/social");

            this.QosManagerServerUrl = this.GetDefaultServerApiUrl(this.QosManagerServerUrl, "/qosm");

            this.GameTelemetryServerUrl = this.GetDefaultServerApiUrl(this.GameTelemetryServerUrl, "/game-telemetry");

            this.AchievementServerUrl = this.GetDefaultServerApiUrl(this.AchievementServerUrl, "/achievement");

            this.LobbyServerUrl = this.GetDefaultServerApiUrl(this.LobbyServerUrl, "/lobby");

            this.CloudSaveServerUrl = this.GetDefaultServerApiUrl(this.CloudSaveServerUrl, "/cloudsave");

            this.MatchmakingServerUrl = this.GetDefaultServerApiUrl(this.MatchmakingServerUrl, "/matchmaking");

            this.SeasonPassServerUrl = this.GetDefaultServerApiUrl(this.SeasonPassServerUrl, "/seasonpass");
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

            if (this.DSMControllerServerUrl == httpBaseUrl + "/dsmcontroller") this.DSMControllerServerUrl = null;

            if (this.PlatformServerUrl == httpBaseUrl + "/platform") this.PlatformServerUrl = null;

            if (this.StatisticServerUrl == httpBaseUrl + "/statistic") this.StatisticServerUrl = null;

            if (this.QosManagerServerUrl == httpBaseUrl + "/qosm") this.QosManagerServerUrl = null;

            if (this.GameTelemetryServerUrl == httpBaseUrl + "/game-telemetry") this.GameTelemetryServerUrl = null;

            if (this.AchievementServerUrl == httpBaseUrl + "/achievement") this.AchievementServerUrl = null;

            if (this.LobbyServerUrl == httpBaseUrl + "/lobby") this.LobbyServerUrl = null;

            if (this.CloudSaveServerUrl == httpBaseUrl + "/cloudsave") this.CloudSaveServerUrl = null;

            if (this.MatchmakingServerUrl == httpBaseUrl + "/matchmaking") this.MatchmakingServerUrl = null;

            if (this.SeasonPassServerUrl == httpBaseUrl + "/seasonpass") this.SeasonPassServerUrl = null;
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace)) throw new System.Exception("Init AccelByte SDK failed, Server Namespace must not null or empty.");

            if (string.IsNullOrEmpty(this.ClientId)) throw new System.Exception("Init AccelByte SDK failed, Server Client ID must not null or empty.");

            if (string.IsNullOrEmpty(this.BaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Server Base URL must not null or empty.");
        }

        public bool IsRequiredFieldEmpty()
        {
            if (string.IsNullOrEmpty(this.Namespace)) return true;

            if (string.IsNullOrEmpty(this.ClientId)) return true;

            if (string.IsNullOrEmpty(this.BaseUrl)) return true;

            return false;
        }

        /// <summary>
        /// Set services URL.
        /// </summary>
        /// <param name="specificServerUrl">The specific URL, if empty will be replaced by baseUrl+defaultUrl.</param>
        /// <param name="defaultServerUrl">The default URL, will be used if specific URL is empty.</param>
        /// <returns></returns>
        private string GetDefaultServerApiUrl(string specificServerUrl, string defaultServerUrl)
        {
            if (string.IsNullOrEmpty(specificServerUrl))
            {
                return string.Format("{0}{1}", BaseUrl, defaultServerUrl);
            }

            return specificServerUrl;
        }
    }

    [DataContract]
    public class MultiServerConfigs
    {
        [DataMember] public ServerConfig Development { get; set; }
        [DataMember] public ServerConfig Certification { get; set; }
        [DataMember] public ServerConfig Production { get; set; }
        [DataMember] public ServerConfig Default { get; set; }
    }
}
