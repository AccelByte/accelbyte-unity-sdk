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
                int index;
                // remove protocol 
                if ((index = this.BaseUrl.IndexOf("://")) > 0) this.BaseUrl = this.BaseUrl.Substring(index + 3);

                string httpBaseUrl = "https://" + this.BaseUrl;

                if (this.IamServerUrl == null) this.IamServerUrl = httpBaseUrl + "/iam";

                if (this.DSMControllerServerUrl == null) this.DSMControllerServerUrl = httpBaseUrl + "/dsmcontroller";

                if (this.PlatformServerUrl == null) this.PlatformServerUrl = httpBaseUrl + "/platform";

                if (this.StatisticServerUrl == null) this.StatisticServerUrl = httpBaseUrl + "/statistic";

                if (this.QosManagerServerUrl == null) this.QosManagerServerUrl = httpBaseUrl + "/qosm";

                if (this.GameTelemetryServerUrl == null) this.GameTelemetryServerUrl = httpBaseUrl + "/game-telemetry";

                if (this.AchievementServerUrl == null) this.AchievementServerUrl = httpBaseUrl + "/achievement";

                if (this.LobbyServerUrl == null) this.LobbyServerUrl = httpBaseUrl + "/lobby";

                if (this.CloudSaveServerUrl == null) this.CloudSaveServerUrl = httpBaseUrl + "/cloudsave";
                
                if (this.MatchmakingServerUrl == null) this.MatchmakingServerUrl = httpBaseUrl + "/matchmaking";

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
    }
}
