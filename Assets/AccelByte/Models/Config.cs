﻿// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
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
        [DataMember] public bool UsePlayerPrefs { get; set; }
        [DataMember] public bool EnableDebugLog { get; set; }
        [DataMember] public string DebugLogFilter { get; set; }
        [DataMember] public string BaseUrl { get; set; }
        [DataMember] public string ApiBaseUrl { get; set; }
        [DataMember] public string NonApiBaseUrl { get; set; }
        [DataMember] public string LoginServerUrl { get; set; }
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
        [DataMember] public string ClientId { get; set; }
        [DataMember] public string ClientSecret { get; set; }
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
            if (this.Namespace == anotherConfig.Namespace &&
                this.UseSessionManagement == anotherConfig.UseSessionManagement &&
                this.UsePlayerPrefs == anotherConfig.UsePlayerPrefs &&
                this.EnableDebugLog == anotherConfig.EnableDebugLog &&
                this.DebugLogFilter == anotherConfig.DebugLogFilter &&
                this.BaseUrl == anotherConfig.BaseUrl &&
                this.ApiBaseUrl == anotherConfig.ApiBaseUrl &&
                this.NonApiBaseUrl == anotherConfig.NonApiBaseUrl &&
                this.LoginServerUrl == anotherConfig.LoginServerUrl &&
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
                this.ClientId == anotherConfig.ClientId &&
                this.ClientSecret == anotherConfig.ClientSecret &&
                this.RedirectUri == anotherConfig.RedirectUri &&
                this.AppId == anotherConfig.AppId &&
                this.PublisherNamespace == anotherConfig.PublisherNamespace)
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
                string baseUrl = this.BaseUrl;
                if ((index = baseUrl.IndexOf("://")) > 0) baseUrl = baseUrl.Substring(index + 3);
                if ((index = this.NonApiBaseUrl.IndexOf("://")) > 0) this.NonApiBaseUrl = this.NonApiBaseUrl.Substring(index + 3);
                if ((index = this.ApiBaseUrl.IndexOf("://")) > 0) this.ApiBaseUrl = this.ApiBaseUrl.Substring(index + 3);

                string httpsBaseUrl = "https://" + baseUrl;
                string wssBaseUrl = "wss://" + baseUrl;

                if (this.ClientSecret == null) this.ClientSecret = "";

                if (string.IsNullOrEmpty(this.LoginServerUrl)) this.LoginServerUrl = httpsBaseUrl;

                if (string.IsNullOrEmpty(this.IamServerUrl)) this.IamServerUrl = httpsBaseUrl + "/iam";

                if (string.IsNullOrEmpty(this.PlatformServerUrl)) this.PlatformServerUrl = httpsBaseUrl + "/platform";

                if (string.IsNullOrEmpty(this.BasicServerUrl)) this.BasicServerUrl = httpsBaseUrl + "/basic";

                if (string.IsNullOrEmpty(this.LobbyServerUrl)) this.LobbyServerUrl = wssBaseUrl + "/lobby/";

                if (string.IsNullOrEmpty(this.CloudStorageServerUrl)) this.CloudStorageServerUrl = httpsBaseUrl + "/binary-store";

                if (string.IsNullOrEmpty(this.GameProfileServerUrl)) this.GameProfileServerUrl = httpsBaseUrl + "/soc-profile";

                if (string.IsNullOrEmpty(this.StatisticServerUrl)) this.StatisticServerUrl = httpsBaseUrl + "/statistic";

                if (string.IsNullOrEmpty(this.QosManagerServerUrl)) this.QosManagerServerUrl = httpsBaseUrl + "/qosm";

                if (string.IsNullOrEmpty(this.AgreementServerUrl)) this.AgreementServerUrl = httpsBaseUrl + "/agreement";

                if (string.IsNullOrEmpty(this.LeaderboardServerUrl)) this.LeaderboardServerUrl = httpsBaseUrl + "/leaderboard";
                
                if (string.IsNullOrEmpty(this.CloudSaveServerUrl)) this.CloudSaveServerUrl = httpsBaseUrl + "/cloudsave";

                if (string.IsNullOrEmpty(this.GameTelemetryServerUrl)) this.GameTelemetryServerUrl = httpsBaseUrl + "/game-telemetry";

                if (string.IsNullOrEmpty(this.AchievementServerUrl)) this.AchievementServerUrl = httpsBaseUrl + "/achievement";
                
                if (string.IsNullOrEmpty(this.GroupServerUrl)) this.GroupServerUrl = httpsBaseUrl + "/group";
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
            if ((index = this.NonApiBaseUrl.IndexOf("://")) > 0) this.NonApiBaseUrl = this.NonApiBaseUrl.Substring(index + 3);
            if ((index = this.ApiBaseUrl.IndexOf("://")) > 0) this.ApiBaseUrl = this.ApiBaseUrl.Substring(index + 3);

            if (baseUrl != null)
            {
                string httpsBaseUrl = "https://" + baseUrl;
                string wssBaseUrl = "wss://" + baseUrl;

                if (this.IamServerUrl == httpsBaseUrl + "/iam") this.IamServerUrl = null;

                if (this.PlatformServerUrl == httpsBaseUrl + "/platform") this.PlatformServerUrl = null;

                if (this.BasicServerUrl == httpsBaseUrl + "/basic") this.BasicServerUrl = null;

                if (this.LobbyServerUrl == wssBaseUrl + "/lobby/") this.LobbyServerUrl = null;

                if (this.CloudStorageServerUrl == httpsBaseUrl + "/binary-store") this.CloudStorageServerUrl = null;

                if (this.GameProfileServerUrl == httpsBaseUrl + "/soc-profile") this.GameProfileServerUrl = null;

                if (this.StatisticServerUrl == httpsBaseUrl + "/statistic") this.StatisticServerUrl = null;

                if (this.QosManagerServerUrl == httpsBaseUrl + "/qosm") this.QosManagerServerUrl = null;

                if (this.AgreementServerUrl == httpsBaseUrl + "/agreement") this.AgreementServerUrl = null;

                if (this.LeaderboardServerUrl == httpsBaseUrl + "/leaderboard") this.LeaderboardServerUrl = null;
                
                if (this.CloudSaveServerUrl == httpsBaseUrl + "/cloudsave") this.CloudSaveServerUrl = null;

                if (this.GameTelemetryServerUrl == httpsBaseUrl + "/game-telemetry") this.GameTelemetryServerUrl = null;

                if (this.AchievementServerUrl == httpsBaseUrl + "/achievement") this.AchievementServerUrl = null;
                
                if (this.GroupServerUrl == httpsBaseUrl + "/group") this.GroupServerUrl = null;
            }
        }

        /// <summary>
        /// Check required config field.
        /// </summary>
        public void CheckRequiredField()
        {
            if (string.IsNullOrEmpty(this.Namespace)) throw new System.Exception("Init AccelByte SDK failed, Namespace must not null or empty.");

            if (string.IsNullOrEmpty(this.ClientId)) throw new System.Exception("Init AccelByte SDK failed, Client ID must not null or empty.");

            if (string.IsNullOrEmpty(this.BaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Base URL must not null or empty.");

            if (string.IsNullOrEmpty(this.ApiBaseUrl)) throw new System.Exception("Init AccelByte SDK failed, API Base URL must not null or empty.");

            if (string.IsNullOrEmpty(this.NonApiBaseUrl)) throw new System.Exception("Init AccelByte SDK failed, Non-API URL must not null or empty.");
        }
    }
}
