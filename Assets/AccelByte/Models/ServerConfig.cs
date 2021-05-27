// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models {
    [DataContract]
    public class ServerConfig
    {
        [DataMember] public string Namespace { get; set; }
        [DataMember] public string BaseUrl { get; set; }
        [DataMember] public string ApiBaseUrl { get; set; }
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
            if (this.BaseUrl != null)
            {
                this.IamServerUrl = GetDefaultServerApiUrl(this.IamServerUrl, "/iam");

                this.DSMControllerServerUrl = GetDefaultServerApiUrl(this.DSMControllerServerUrl, "/dsmcontroller");

                this.PlatformServerUrl = GetDefaultServerApiUrl(this.PlatformServerUrl, "/platform");

                this.StatisticServerUrl = GetDefaultServerApiUrl(this.StatisticServerUrl, "/social");

                this.QosManagerServerUrl = GetDefaultServerApiUrl(this.QosManagerServerUrl, "/qosm");

                this.GameTelemetryServerUrl = GetDefaultServerApiUrl(this.GameTelemetryServerUrl, "/game-telemetry");

                this.AchievementServerUrl = GetDefaultServerApiUrl(this.AchievementServerUrl, "/achievement");

                this.LobbyServerUrl = GetDefaultServerApiUrl(this.LobbyServerUrl, "/lobby");

                this.CloudSaveServerUrl = GetDefaultServerApiUrl(this.CloudSaveServerUrl, "/cloudsave");

                this.MatchmakingServerUrl = GetDefaultServerApiUrl(this.MatchmakingServerUrl, "/matchmaking");

            }
        }

        /// <summary>
        ///  Remove config values that can be derived from another value.
        /// </summary>
        public void Compact()
        {
            int index;
            // remove protocol
            if ((index = this.BaseUrl.IndexOf("://")) > 0) this.BaseUrl = this.BaseUrl.Substring(index + 3);

            if (this.BaseUrl != null)
            {
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

            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace)) throw new System.Exception("Init AccelByte SDK failed, Server Namespace must not null or empty.");

            if (string.IsNullOrEmpty(this.ClientId)) throw new System.Exception("Init AccelByte SDK failed, Server Client ID must not null or empty.");

            if (string.IsNullOrEmpty(this.BaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Server Base URL must not null or empty.");

            if (string.IsNullOrEmpty(this.ApiBaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Server API Base URL must not null or empty.");
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
}
