// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class Config
    {
        [DataMember] public string Namespace { get; set; }
        [DataMember] public bool UseSessionManagement { get; set; }
        [DataMember] public string BaseUrl { get; set; }
        [DataMember] public string LoginServerUrl { get; set; }
        [DataMember] public string IamServerUrl { get; set; }
        [DataMember] public string PlatformServerUrl { get; set; }
        [DataMember] public string BasicServerUrl { get; set; }
        [DataMember] public string LobbyServerUrl { get; set; }
        [DataMember] public string CloudStorageServerUrl { get; set; }
        [DataMember] public string TelemetryServerUrl { get; set; }
        [DataMember] public string GameProfileServerUrl { get; set; }
        [DataMember] public string StatisticServerUrl { get; set; }
        [DataMember] public string QosManagerServerUrl { get; set; }
        [DataMember] public string ClientId { get; set; }
        [DataMember] public string ClientSecret { get; set; }
        [DataMember] public string RedirectUri { get; set; }

        /// <summary>
        ///  Copy member values
        /// </summary>
        public Config ShallowCopy()
        {
            return (Config) MemberwiseClone();
        }

        public bool Compare(Config anotherConfig)
        {
            if (this.Namespace == anotherConfig.Namespace &&
                this.UseSessionManagement == anotherConfig.UseSessionManagement &&
                this.BaseUrl == anotherConfig.BaseUrl &&
                this.LoginServerUrl == anotherConfig.LoginServerUrl &&
                this.IamServerUrl == anotherConfig.IamServerUrl &&
                this.PlatformServerUrl == anotherConfig.PlatformServerUrl &&
                this.BasicServerUrl == anotherConfig.BasicServerUrl &&
                this.LobbyServerUrl == anotherConfig.LobbyServerUrl &&
                this.CloudStorageServerUrl == anotherConfig.CloudStorageServerUrl &&
                this.TelemetryServerUrl == anotherConfig.TelemetryServerUrl &&
                this.GameProfileServerUrl == anotherConfig.GameProfileServerUrl &&
                this.StatisticServerUrl == anotherConfig.StatisticServerUrl &&
                this.QosManagerServerUrl == anotherConfig.QosManagerServerUrl &&
                this.ClientId == anotherConfig.ClientId &&
                this.ClientSecret == anotherConfig.ClientSecret &&
                this.RedirectUri == anotherConfig.RedirectUri)
            {
                return true;
            }

            return false;
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

                string httpsBaseUrl = "https://" + this.BaseUrl;
                string wssBaseUrl = "wss://" + this.BaseUrl;

                if (this.LoginServerUrl == null) this.LoginServerUrl = httpsBaseUrl;

                if (this.IamServerUrl == null) this.IamServerUrl = httpsBaseUrl + "/iam";

                if (this.PlatformServerUrl == null) this.PlatformServerUrl = httpsBaseUrl + "/platform";

                if (this.BasicServerUrl == null) this.BasicServerUrl = httpsBaseUrl + "/basic";

                if (this.LobbyServerUrl == null) this.LobbyServerUrl = wssBaseUrl + "/lobby/";

                if (this.CloudStorageServerUrl == null) this.CloudStorageServerUrl = httpsBaseUrl + "/binary-store";

                if (this.TelemetryServerUrl == null) this.TelemetryServerUrl = httpsBaseUrl + "/telemetry";

                if (this.GameProfileServerUrl == null) this.GameProfileServerUrl = httpsBaseUrl + "/soc-profile";

                if (this.StatisticServerUrl == null) this.StatisticServerUrl = httpsBaseUrl + "/statistic";

                if (this.QosManagerServerUrl == null) this.QosManagerServerUrl = httpsBaseUrl + "/qosm";
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
                string httpsBaseUrl = "https://" + this.BaseUrl;
                string wssBaseUrl = "wss://" + this.BaseUrl;

                if (this.IamServerUrl == httpsBaseUrl + "/iam") this.IamServerUrl = null;

                if (this.PlatformServerUrl == httpsBaseUrl + "/platform") this.PlatformServerUrl = null;

                if (this.BasicServerUrl == httpsBaseUrl + "/basic") this.BasicServerUrl = null;

                if (this.LobbyServerUrl == wssBaseUrl + "/lobby/") this.LobbyServerUrl = null;

                if (this.CloudStorageServerUrl == httpsBaseUrl + "/binary-store") this.CloudStorageServerUrl = null;

                if (this.TelemetryServerUrl == httpsBaseUrl + "/telemetry") this.TelemetryServerUrl = null;

                if (this.GameProfileServerUrl == httpsBaseUrl + "/soc-profile") this.GameProfileServerUrl = null;

                if (this.StatisticServerUrl == httpsBaseUrl + "/statistic") this.StatisticServerUrl = null;

                if (this.QosManagerServerUrl == httpsBaseUrl + "/qosm") this.QosManagerServerUrl = null;
            }
        }
    }
}