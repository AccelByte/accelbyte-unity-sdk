// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Text;
using AccelByte.Models;
using UnityEngine;
using Utf8Json;

namespace AccelByte.Api
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public sealed class AccelByteSettings : ScriptableObject
    {
        public static bool UsePlayerPrefs
        {
            get { return AccelByteSettings.Instance.config.UsePlayerPrefs; }
            set { AccelByteSettings.Instance.config.UsePlayerPrefs = value; }
        }

        public static bool EnableDebugLog
        {
            get { return AccelByteSettings.Instance.config.EnableDebugLog; }
            set { AccelByteSettings.Instance.config.EnableDebugLog = value; }
        }

        public static string DebugLogFilter
        {
            get { return AccelByteSettings.Instance.config.DebugLogFilter; }
            set { AccelByteSettings.Instance.config.DebugLogFilter = value; }
        }

        public static string Namespace
        {
            get { return AccelByteSettings.Instance.config.Namespace; }
            set { AccelByteSettings.Instance.config.Namespace = value; }
        }

        public static string BaseUrl
        {
            get { return AccelByteSettings.Instance.config.BaseUrl; }
            set { AccelByteSettings.Instance.config.BaseUrl = value; }
        }

        public static string IamServerUrl
        {
            get { return AccelByteSettings.Instance.config.IamServerUrl; }
            set { AccelByteSettings.Instance.config.IamServerUrl = value; }
        }

        public static string PlatformServerUrl
        {
            get { return AccelByteSettings.Instance.config.PlatformServerUrl; }
            set { AccelByteSettings.Instance.config.PlatformServerUrl = value; }
        }

        public static string BasicServerUrl
        {
            get { return AccelByteSettings.Instance.config.BasicServerUrl; }
            set { AccelByteSettings.Instance.config.BasicServerUrl = value; }
        }

        public static string LobbyServerUrl
        {
            get { return AccelByteSettings.Instance.config.LobbyServerUrl; }
            set { AccelByteSettings.Instance.config.LobbyServerUrl = value; }
        }

        public static string CloudStorageServerUrl
        {
            get { return AccelByteSettings.Instance.config.CloudStorageServerUrl; }
            set { AccelByteSettings.Instance.config.CloudStorageServerUrl = value; }
        }

        public static string GameProfileServerUrl
        {
            get { return AccelByteSettings.Instance.config.GameProfileServerUrl; }
            set { AccelByteSettings.Instance.config.GameProfileServerUrl = value; }
        }

        public static string StatisticServerUrl
        {
            get { return AccelByteSettings.Instance.config.StatisticServerUrl; }
            set { AccelByteSettings.Instance.config.StatisticServerUrl = value; }
        }

        public static string CloudSaveServerUrl
        {
            get { return AccelByteSettings.Instance.config.CloudSaveServerUrl; }
            set { AccelByteSettings.Instance.config.CloudSaveServerUrl = value; }
        }

        public static string AchievementServerUrl
        {
            get { return AccelByteSettings.Instance.config.AchievementServerUrl; }
            set { AccelByteSettings.Instance.config.AchievementServerUrl = value; }
        }

        public static string GroupServerUrl
        {
            get { return AccelByteSettings.Instance.config.GroupServerUrl; }
            set { AccelByteSettings.Instance.config.GroupServerUrl = value; }
        }
        
        public static string AgreementServerUrl
        {
            get { return AccelByteSettings.Instance.config.AgreementServerUrl; }
            set { AccelByteSettings.Instance.config.AgreementServerUrl = value; }
        }

        public static string LeaderboardServerUrl
        {
            get { return AccelByteSettings.Instance.config.LeaderboardServerUrl; }
            set { AccelByteSettings.Instance.config.LeaderboardServerUrl = value; }
        }

        public static string GameTelemetryServerUrl
        {
            get { return AccelByteSettings.Instance.config.GameTelemetryServerUrl; }
            set { AccelByteSettings.Instance.config.GameTelemetryServerUrl = value; }
        }

        public static string UGCServerUrl
        {
            get { return AccelByteSettings.Instance.config.UGCServerUrl; }
            set { AccelByteSettings.Instance.config.UGCServerUrl = value; }
        }

        public static string SeasonPassServerUrl
        {
            get { return AccelByteSettings.Instance.config.SeasonPassServerUrl; }
            set { AccelByteSettings.Instance.config.SeasonPassServerUrl = value; }
        }

        public static string ClientId
        {
            get { return AccelByteSettings.Instance.config.ClientId; }
            set { AccelByteSettings.Instance.config.ClientId = value; }
        }

        public static string ClientSecret
        {
            get { return AccelByteSettings.Instance.config.ClientSecret; }
            set { AccelByteSettings.Instance.config.ClientSecret = value; }
        }

        public static string RedirectUri
        {
            get { return AccelByteSettings.Instance.config.RedirectUri; }
            set { AccelByteSettings.Instance.config.RedirectUri = value; }
        }

        public static string AppId
        {
            get { return AccelByteSettings.Instance.config.AppId; }
            set { AccelByteSettings.Instance.config.AppId = value; }
        }

        public static string PublisherNamespace
        {
            get { return AccelByteSettings.instance.config.PublisherNamespace; }
            set { AccelByteSettings.instance.config.PublisherNamespace = value; }
        }

        private Config config;

        private static AccelByteSettings instance;

        public static AccelByteSettings Instance
        {
            get
            {
                if (AccelByteSettings.instance == null)
                {
                    AccelByteSettings.instance = Resources.Load<AccelByteSettings>("AccelByteSettings");

                    // This can happen if the developer never input their values into the Unity Editor
                    // and therefore never created the AccelByteSettings.asset file
                    // Use a dummy object with defaults for the getters so we don't have a null pointer exception
                    if (AccelByteSettings.instance == null)
                    {
                        AccelByteSettings.instance = ScriptableObject.CreateInstance<AccelByteSettings>();

#if UNITY_EDITOR
                        // Only in the editor should we save it to disk
                        string properPath = System.IO.Path.Combine(Application.dataPath, "Resources");

                        if (!System.IO.Directory.Exists(properPath))
                        {
                            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                        }

                        string fullPath = System.IO.Path.Combine(System.IO.Path.Combine("Assets", "Resources"),
                            "AccelByteSettings.asset");
                        UnityEditor.AssetDatabase.CreateAsset(AccelByteSettings.instance, fullPath);
#endif
                    }

                    Debug.Log("AccelByteSettings loaded");
                    AccelByteSettings.instance.Load();
                }

                return AccelByteSettings.instance;
            }
            set { AccelByteSettings.instance = value; }
        }

        public Config CopyConfig() { return this.config.ShallowCopy(); }
        public void UpdateConfig(Config newConfig) { this.config = newConfig; }
        public bool CompareConfig(Config newConfig) { return this.config.Compare(newConfig); }

        /// <summary>
        ///  Load configuration from AccelByteSDKConfig.json
        /// </summary>
        public void Load()
        {
            var configFile = Resources.Load("AccelByteSDKConfig");

            if (configFile == null)
            {
                this.config = new Config();
            }
            else
            {
                string wholeJsonText = ((TextAsset) configFile).text;
                this.config = JsonSerializer.Deserialize<Config>(wholeJsonText);
            }
        }

        /// <summary>
        ///  Save configuration to AccelByteSDKConfig.json
        /// </summary>
        public void Save()
        {
            // Only in the editor should we save it to disk
            string properPath = System.IO.Path.Combine(Application.dataPath, "Resources");

            if (!System.IO.Directory.Exists(properPath))
            {
                #if UNITY_EDITOR 
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                #endif
            }

            string fullPath =
                System.IO.Path.Combine(System.IO.Path.Combine("Assets", "Resources"), "AccelByteSDKConfig.json");

            byte[] notPrettyConfig = JsonSerializer.Serialize(this.config);
            string prettyConfig = JsonSerializer.PrettyPrint(notPrettyConfig);
            System.IO.File.WriteAllBytes(fullPath, Encoding.ASCII.GetBytes(prettyConfig));
        }
    }
}