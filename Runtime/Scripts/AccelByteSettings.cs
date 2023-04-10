// Copyright (c) 2019 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Compilation;
using UnityEditor;
#endif

using Newtonsoft.Json;

namespace AccelByte.Api
{
    /// <summary>
    /// Primarily used by the Editor for the config in the popup menu.
    /// <para>Looking for runtime settings? See static AccelBytePlugin.Config</para>
    /// </summary>
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

        public static string TurnManagerServerUrl
        {
            get { return AccelByteSettings.Instance.config.TurnManagerServerUrl; }
            set { AccelByteSettings.Instance.config.TurnManagerServerUrl = value; }
        }

        public static bool UseTurnManager
        {
            get { return AccelByteSettings.Instance.config.UseTurnManager; }
            set { AccelByteSettings.Instance.config.UseTurnManager = value; }
        }

        public static string TurnServerHost
        {
            get { return AccelByteSettings.Instance.config.TurnServerHost; }
            set { AccelByteSettings.Instance.config.TurnServerHost = value; }
        }

        public static string TurnServerPort
        {
            get { return AccelByteSettings.Instance.config.TurnServerPort; }
            set { AccelByteSettings.Instance.config.TurnServerPort = value; }
        }
        
        public static string TurnServerUsername
        {
            get { return AccelByteSettings.Instance.config.TurnServerUsername; }
            set { AccelByteSettings.Instance.config.TurnServerUsername = value; }
        }
        
        public static string TurnServerSecret
        {
            get { return AccelByteSettings.Instance.config.TurnServerSecret; }
            set { AccelByteSettings.Instance.config.TurnServerSecret = value; }
        }
        
        public static string TurnServerPassword
        {
            get { return AccelByteSettings.Instance.config.TurnServerPassword; }
            set { AccelByteSettings.Instance.config.TurnServerPassword = value; }
        }

        public static bool EnableAuthHandshake
        {
            get { return AccelByteSettings.Instance.config.EnableAuthHandshake; }
            set { AccelByteSettings.Instance.config.EnableAuthHandshake = value; }
        }

        public static string ClientId
        {
            get { return AccelByteSettings.Instance.oAuthConfig.ClientId; }
            set { AccelByteSettings.Instance.oAuthConfig.ClientId = value; }
        }

        public static string ClientSecret
        {
            get { return AccelByteSettings.Instance.oAuthConfig.ClientSecret; }
            set { AccelByteSettings.Instance.oAuthConfig.ClientSecret = value; }
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

        private OAuthConfig oAuthConfig;
        private MultiOAuthConfigs multiOAuthConfigs;
        private Config config;
        private MultiConfigs multiConfigs;
        private SettingsEnvironment editedEnvironment = SettingsEnvironment.Default;
        private string editedPlatform = "";

        private const string AccelByteSDKVersionFilename = "AccelByteSDKVersion";
        public string AccelByteSDKVersion { get; private set; } = "";

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

                    AccelByteSettings.instance.Load();
                    AccelByteDebug.Log("AccelByteSettings loaded");
                }

                return AccelByteSettings.instance;
            }
            set { AccelByteSettings.instance = value; }
        }

        public OAuthConfig CopyOAuthConfig() 
        { 
            return oAuthConfig != null ? oAuthConfig.ShallowCopy() : null; 
        }
        public Config CopyConfig() 
        {
            return config != null ? config.ShallowCopy() : null;
        }
        public void UpdateOAuthConfig(OAuthConfig newConfig) { this.oAuthConfig = newConfig; }
        public void UpdateConfig(Config newConfig) { this.config = newConfig; }
        public bool CompareOAuthConfig(OAuthConfig newConfig) 
        { 
            if(newConfig == null || oAuthConfig == null)
            {
                return false;
            }
            return this.oAuthConfig.Compare(newConfig); 
        }
        public bool CompareConfig(Config newConfig)
        {
            if (newConfig == null || config == null)
            {
                return false;
            }
            return this.config.Compare(newConfig); 
        }
        public SettingsEnvironment GetEditedEnvironment() { return this.editedEnvironment; }
        public void SetEditedEnvironment(SettingsEnvironment environment) 
        { 
            this.editedEnvironment = environment;
            this.Load();
        }
        public void SetEditedPlatform(string platform = "")
        {
            this.editedPlatform = platform;
            this.Load();
        }

        /// <summary>
        ///  Load configuration from Resource directory
        /// </summary>
        public void Load()
        {
            this.multiOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(editedPlatform);
            this.multiConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
            this.AccelByteSDKVersion = AccelByteSettingsV2.LoadVersionFile();

            if (this.multiOAuthConfigs == null)
            {
                this.multiOAuthConfigs = new MultiOAuthConfigs();
            }

            if (this.multiConfigs == null)
            {
                this.multiConfigs = new MultiConfigs();
            }

            switch (editedEnvironment)
            {
                case SettingsEnvironment.Development:
                    this.oAuthConfig = multiOAuthConfigs.Development;
                    this.config = multiConfigs.Development; break;
                case SettingsEnvironment.Certification:
                    this.oAuthConfig = multiOAuthConfigs.Certification;
                    this.config = multiConfigs.Certification; break;
                case SettingsEnvironment.Production:
                    this.oAuthConfig = multiOAuthConfigs.Production;
                    this.config = multiConfigs.Production; break;
                case SettingsEnvironment.Default:
                default:
                    this.oAuthConfig = multiOAuthConfigs.Default;
                    this.config = multiConfigs.Default; break;
            }
        }

        /// <summary>
        ///  Save configuration to AccelByteSDKConfig.json
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            var configDir = AccelByteSettingsV2.GeneratedConfigsDirectoryFullPath();
            if (!System.IO.Directory.Exists(configDir))
            {
                System.IO.Directory.CreateDirectory(configDir);
            }

            string oAuthFullpath = AccelByteSettingsV2.OAuthFullPath(editedPlatform, false);
            string fullPath = AccelByteSettingsV2.SDKConfigFullPath(false);

            switch (editedEnvironment)
            {
                case SettingsEnvironment.Development:
                    multiOAuthConfigs.Development = this.oAuthConfig;
                    this.multiConfigs.Development = this.config; break;
                case SettingsEnvironment.Certification:
                    multiOAuthConfigs.Certification = this.oAuthConfig;
                    this.multiConfigs.Certification = this.config; break;
                case SettingsEnvironment.Production:
                    multiOAuthConfigs.Production = this.oAuthConfig;
                    this.multiConfigs.Production = this.config; break;
                case SettingsEnvironment.Default:
                default:
                    multiOAuthConfigs.Default = this.oAuthConfig;
                    this.multiConfigs.Default = this.config; break;
            }

            System.IO.File.WriteAllBytes(oAuthFullpath, Encoding.ASCII.GetBytes(this.multiOAuthConfigs.ToJsonString(Formatting.Indented)));
            System.IO.File.WriteAllBytes(fullPath, Encoding.ASCII.GetBytes(this.multiConfigs.ToJsonString(Formatting.Indented)));
#endif
        }
    }
}
