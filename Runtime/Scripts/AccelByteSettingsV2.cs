// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
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
    public class AccelByteSettingsV2
    {
        internal const string DefaultStaticConfigsResourceDirectory = "AccelByteSDK";
        internal const string DefaultGeneratedConfigsResourceDirectory = "";
        public static string AccelByteSDKVersion
        {
            get
            {
                if(sdkVersion == null)
                {
                    sdkVersion = LoadVersionFile();
                }
                return sdkVersion;
            }
        }

        public Config SDKConfig
        {
            get
            {
                return sdkConfig;
            }
        }

        public ServerConfig ServerSdkConfig
        {
            get
            {
                return serverSdkConfig;
            }
        }

        public OAuthConfig OAuthConfig
        {
            get
            {
                return oAuthConfig;
            }
        }

        private static string sdkVersion = null;

        private OAuthConfig oAuthConfig;
        private Config sdkConfig;
        private ServerConfig serverSdkConfig;

#if UNITY_EDITOR
        public static string StaticConfigsDirectoryFullPath()
        {
            var accelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettingsV2).Assembly);
            string retval = System.IO.Path.Combine(accelBytePackageInfo.resolvedPath, "Configs", "Resources", StaticConfigsResourceDirectory);
            return retval;
        }

        public static string GeneratedConfigsDirectoryFullPath()
        {
            string retval = System.IO.Path.Combine(Application.dataPath, "Resources", GeneratedConfigsResourceDirectory);
            return retval;
        }
#endif

        public static string GeneratedConfigsResourceDirectory
        {
            get;
            set;
        } = DefaultGeneratedConfigsResourceDirectory;

        public static string StaticConfigsResourceDirectory
        {
            get;
            set;
        } = DefaultStaticConfigsResourceDirectory;

#if UNITY_EDITOR
        public static string SDKConfigFullPath(bool isServer)
        {
            string fileName = isServer ? "AccelByteServerSDKConfig.json" : "AccelByteSDKConfig.json";
            string retval = System.IO.Path.Combine(GeneratedConfigsDirectoryFullPath(), fileName);
            return retval;
        }
#endif
        public static string OldSDKConfigResourcePath(bool isServer = false)
        {
            string fileName = isServer ? "AccelByteServerSDKConfig" : "AccelByteSDKConfig";
            return System.IO.Path.Combine(StaticConfigsResourceDirectory, fileName);
        }

        public static string SDKConfigResourcePath(bool isServer = false)
        {
            string fileName = isServer ? "AccelByteServerSDKConfig" : "AccelByteSDKConfig";
            return System.IO.Path.Combine(GeneratedConfigsResourceDirectory, fileName);
        }

#if UNITY_EDITOR
        public static string OAuthFullPath(string platform, bool isServer = false)
        {
            string fileName = isServer ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            string retval = System.IO.Path.Combine(GeneratedConfigsDirectoryFullPath(), $"{fileName}{platform}.json");
            return retval;
        }
#endif
        public static string OldOAuthResourcePath(string platform, bool isServer)
        {
            string fileName = isServer ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            return System.IO.Path.Combine(StaticConfigsResourceDirectory, $"{fileName}{platform}");
        }

        public static string OAuthResourcePath(string platform, bool isServer)
        {
            string fileName = isServer ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            return System.IO.Path.Combine(GeneratedConfigsResourceDirectory, $"{fileName}{platform}");
        }

#if UNITY_EDITOR
        public static string SDKVersionFullPath()
        {
            return System.IO.Path.Combine(GeneratedConfigsDirectoryFullPath(), "AccelByteSDKVersion.json");
        }
#endif

        public static string SDKVersionResourcePath()
        {
            return System.IO.Path.Combine(GeneratedConfigsResourceDirectory, "AccelByteSDKVersion");
        }

        public static string ServiceCompatibilityResourcePath()
        {
            return System.IO.Path.Combine(StaticConfigsResourceDirectory, "CompatibilityMap.json");
        }

        public static MultiOAuthConfigs LoadOAuthFile(string targetPlatform, bool isServerConfig = false)
        {
            MultiOAuthConfigs retval = null;
            UnityEngine.Object targetOAuthFile = null;
            bool moveConfigFile = false;

            string oldPath = OldOAuthResourcePath(targetPlatform, isServerConfig);
            string latestPath = OAuthResourcePath(targetPlatform, isServerConfig);
            string usedPath = string.Empty;

            var oldOAuthFile = Resources.Load(oldPath);
            var oAuthFile = Resources.Load(latestPath);

            if (oldOAuthFile != null && oAuthFile == null)
            {
                usedPath = oldPath;
                targetOAuthFile = oldOAuthFile;
                moveConfigFile = true;
            }
            else if (oAuthFile != null)
            {
                usedPath = latestPath;
                targetOAuthFile = oAuthFile;
            }

            if (string.IsNullOrEmpty(usedPath))
            {
                string platformName = targetPlatform;
                if (string.IsNullOrEmpty(platformName))
                {
                    platformName = "default";
                }
            }

            if (targetOAuthFile != null)
            {
                string wholeOAuthJsonText = ((TextAsset)targetOAuthFile).text;
                retval = wholeOAuthJsonText.ToObject<MultiOAuthConfigs>();
            }

            if (moveConfigFile)
            {
#if UNITY_EDITOR
                SaveConfig(retval, OAuthFullPath(targetPlatform, isServerConfig));
#endif
            }
            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        public static OAuthConfig GetOAuthByEnvironment(MultiOAuthConfigs multiOAuthConfigs, SettingsEnvironment environment)
        {
            if(multiOAuthConfigs == null)
            {
                return null;
            }
            OAuthConfig retval = multiOAuthConfigs.GetConfigFromEnvironment(environment);
            return retval;
        }

        public static MultiOAuthConfigs SetOAuthByEnvironment(MultiOAuthConfigs multiOAuthConfigs, OAuthConfig newOauthConfig, SettingsEnvironment environment)
        {
            MultiOAuthConfigs retval = multiOAuthConfigs;
            if(retval != null)
            {
                retval.SetConfigToEnv(newOauthConfig, environment);
            }

            return retval;
        }

        public static MultiConfigs LoadSDKConfigFile()
        {
            MultiConfigs retval = null;
            UnityEngine.Object targetConfigFile = null;
            bool moveConfigFile = false;

            var oldConfigFile = Resources.Load(OldSDKConfigResourcePath(false));
            var configFile = Resources.Load(SDKConfigResourcePath(false));
            if (oldConfigFile != null && configFile == null)
            {
                targetConfigFile = oldConfigFile;
                moveConfigFile = true;
            }
            else if (configFile != null)
            {
                targetConfigFile = configFile;
            }

            if (targetConfigFile != null)
            {
                string wholeJsonText = ((TextAsset)targetConfigFile).text;
                retval = wholeJsonText.ToObject<MultiConfigs>();
            }

            if (moveConfigFile)
            {
#if UNITY_EDITOR
                SaveConfig(retval, SDKConfigFullPath(false));
#endif
            }

            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        public static MultiServerConfigs LoadSDKServerConfigFile()
        {
            MultiServerConfigs retval = null;
            UnityEngine.Object targetConfigFile = null;
            bool moveConfigFile = false;

            var oldConfigFile = Resources.Load(OldSDKConfigResourcePath(true));
            var configFile = Resources.Load(SDKConfigResourcePath(true));
            if (oldConfigFile != null && configFile == null)
            {
                targetConfigFile = oldConfigFile;
                moveConfigFile = true;
            }
            else if (configFile != null)
            {
                targetConfigFile = configFile;
            }

            if (targetConfigFile != null)
            {
                string wholeJsonText = ((TextAsset)targetConfigFile).text;
                retval = wholeJsonText.ToObject<MultiServerConfigs>();
            }

            if (moveConfigFile)
            {
#if UNITY_EDITOR
                SaveConfig(retval, SDKConfigFullPath(true));
#endif
            }

            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        internal static AccelByteSettingsV2 GetSettingsByEnv(SettingsEnvironment environment, OverrideConfigs overrideConfigs, bool getServerConfig)
        {
            var targetEnvironment = environment;
            const bool isServer = false;
            string activePlatform = AccelByteSettingsV2.GetActivePlatform(isServer);
            var newSettings = GetSettingsByPlatformAndEnv(activePlatform, targetEnvironment, overrideConfigs, getServerConfig);
            bool configValid = true;
            if(getServerConfig && newSettings.ServerSdkConfig.IsRequiredFieldEmpty())
            {
                configValid = false;
            }
            else if (!getServerConfig && newSettings.SDKConfig.IsRequiredFieldEmpty())
            {
                configValid = false;
            }

            if(!configValid)
            {
                targetEnvironment = SettingsEnvironment.Default;
                newSettings = GetSettingsByPlatformAndEnv(activePlatform, targetEnvironment, overrideConfigs, getServerConfig);
            }

            if (newSettings.OAuthConfig.IsRequiredFieldEmpty())
            {
                newSettings = GetSettingsByPlatformAndEnv(string.Empty, targetEnvironment, overrideConfigs, getServerConfig);
            }
            return newSettings;
        }

        private static AccelByteSettingsV2 GetSettingsByPlatformAndEnv(string platform, SettingsEnvironment environment, OverrideConfigs overrideConfigs, bool getServerConfig)
        {
            var retval = new AccelByteSettingsV2(platform, environment, getServerConfig);
            SDKConfigArgs configArgs = overrideConfigs.SDKConfigOverride.GetByEnvironment(environment);
            OAuthConfig oAuthConfigArgs = overrideConfigs.OAuthConfigOverride.GetByEnvironment(environment);
            if (!getServerConfig)
            {
                retval.OverrideClientSDKConfig(configArgs);
            }
            else
            {
                retval.OverrideServerSDKConfig(configArgs);
            }
            retval.OverrideOAuthConfig(oAuthConfigArgs);
            return retval;
        }

        public static Config GetSDKConfigByEnvironment(MultiConfigs multiSDKConfigs, SettingsEnvironment environment)
        {
            if(multiSDKConfigs == null)
            {
                return null;
            }
            Config retval = multiSDKConfigs.GetConfigFromEnvironment(environment);
            return retval;
        }

        public static ServerConfig GetSDKConfigByEnvironment(MultiServerConfigs multiSDKConfigs, SettingsEnvironment environment)
        {
            if (multiSDKConfigs == null)
            {
                return null;
            }

            ServerConfig retval = multiSDKConfigs.GetConfigFromEnvironment(environment);
            return retval;
        }

        public static MultiConfigs SetSDKConfigByEnvironment(MultiConfigs multiSDKConfig, Config newSDKConfig, SettingsEnvironment environment)
        {
            var retval = multiSDKConfig;
            if(retval != null)
            {
                retval.SetConfigToEnv(newSDKConfig, environment);
            }
            return retval;
        }

        public static MultiServerConfigs SetSDKConfigByEnvironment(MultiServerConfigs multiSDKConfig, ServerConfig newSDKConfig, SettingsEnvironment environment)
        {
            var retval = multiSDKConfig;
            if (retval != null)
            {
                retval.SetConfigToEnv(newSDKConfig, environment);
            }
            return retval;
        }

        /// <summary>
        /// Get version from config directory
        /// </summary>
        public static string LoadVersionFile()
        {
            string retval = "";
            var versionFile = Resources.Load(SDKVersionResourcePath());

#if UNITY_EDITOR
            if (versionFile == null)
            {
                CopyAccelByteSDKPackageVersion(null);
            }
            versionFile = Resources.Load(SDKVersionResourcePath());
#endif

            if (versionFile != null)
            {
                string wholeJsonText = ((TextAsset)versionFile).text;
                var versionObject = wholeJsonText.ToObject<AccelByte.Models.VersionJson>();
                if (versionObject != null)
                {
                    retval = versionObject.Version;
                }
            }

            return retval;
        }

        /// <summary>
        /// Save configuration
        /// Only in the editor should we save it to disk
        /// </summary>
        public static void SaveConfig<T>(T savedConfig, string savePath)
        {
#if UNITY_EDITOR
            string configPath = GeneratedConfigsDirectoryFullPath();

            if (!System.IO.Directory.Exists(configPath))
            {
                System.IO.Directory.CreateDirectory(configPath);
            }

            if (savedConfig != null)
            {
                System.IO.File.WriteAllBytes(savePath, Encoding.ASCII.GetBytes(savedConfig.ToJsonString(Formatting.Indented)));
            }
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        private static void CopyAccelByteSDKPackageVersion(object obj)
        {
#if UNITY_EDITOR
            var accelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettingsV2).Assembly);
            var versionJsonAbs = System.IO.Path.Combine(accelBytePackageInfo.assetPath, "version.json");

            var versionAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(versionJsonAbs);
            if (versionAsset == null)
            {
                Debug.Log("version.json not found under Package AccelByte SDK directory");
                goto endfunction;
            }
            if (versionAsset.text == string.Empty || versionAsset.text == null)
            {
                Debug.Log("version.json for AccelByte SDK is empty");
                goto endfunction;
            }

            string versionTextRaw = versionAsset.text;
            var versionAsObject = versionTextRaw.ToObject<AccelByte.Models.VersionJson>();
            if (versionAsObject == null)
            {
                Debug.Log("Failed to deserialize version.json from AccelByte SDK Package");
                goto endfunction;
            }

            string configPath = GeneratedConfigsDirectoryFullPath();
            if (!System.IO.Directory.Exists(configPath))
            {
                System.IO.Directory.CreateDirectory(configPath);
            }

            string SDKVersionPath = SDKVersionFullPath();
            System.IO.File.WriteAllBytes(SDKVersionPath, Encoding.ASCII.GetBytes(versionTextRaw));
            UnityEditor.AssetDatabase.Refresh();

            endfunction:
            {
                return;
            }
#endif
        }

        public static string GetActivePlatform(bool isServer)
        {
            string activePlatform;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    if (Resources.Load(AccelByteSettingsV2.OAuthResourcePath(PlatformType.Steam.ToString(), isServer)) != null)
                    {
                        activePlatform = PlatformType.Steam.ToString();
                    }
                    else if (Resources.Load(AccelByteSettingsV2.OAuthResourcePath(PlatformType.EpicGames.ToString(), isServer)) != null)
                    {
                        activePlatform = PlatformType.EpicGames.ToString();
                    }
                    else if (Resources.Load(AccelByteSettingsV2.OAuthResourcePath(PlatformType.Android.ToString(), isServer)) != null)
                    {
                        activePlatform = PlatformType.Android.ToString();
                    }
                    else
                    {
                        activePlatform = "";
                    }
                    break;
                case RuntimePlatform.OSXPlayer:
                    activePlatform = PlatformType.Apple.ToString();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    activePlatform = PlatformType.iOS.ToString();
                    break;
                case RuntimePlatform.Android:
                    activePlatform = PlatformType.Android.ToString();
                    break;
                case RuntimePlatform.PS4:
                    activePlatform = PlatformType.PS4.ToString();
                    break;
#if UNITY_2020_2_OR_NEWER
                case RuntimePlatform.PS5:
                    activePlatform = PlatformType.PS5.ToString();
                    break;
#endif
                case RuntimePlatform.GameCoreXboxOne:
                case RuntimePlatform.GameCoreXboxSeries:
                    activePlatform = PlatformType.Live.ToString();
                    break;
                case RuntimePlatform.Switch:
                    activePlatform = PlatformType.Nintendo.ToString();
                    break; 
                default:
                    activePlatform = "";
                    break;
            }
            return activePlatform;
        }

        public AccelByteSettingsV2(string platform, SettingsEnvironment environment, bool isServer)
        {
            string serverFlagLog = isServer ? "" : "Server ";

            MultiOAuthConfigs multiOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(platform, isServer);
            bool platformOauthConfigNotFound = false;

            if (multiOAuthConfigs == null && !string.IsNullOrEmpty(platform))
            {
                platformOauthConfigNotFound = true;
                multiOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(string.Empty, isServer);
            }

            if (multiOAuthConfigs == null)
            {
                multiOAuthConfigs = new MultiOAuthConfigs();
                string onPlatformOAuthNotFoundAdditionalMessage = string.Empty;
                if (platformOauthConfigNotFound)
                {
                    onPlatformOAuthNotFoundAdditionalMessage = $"or {serverFlagLog}OAuth{platform}";
                }
                AccelByteDebug.LogWarning($"{serverFlagLog}OAuth config {onPlatformOAuthNotFoundAdditionalMessage} not found, using empty config");
            }

            IAccelByteMultiConfigs multiConfigs;
            if(!isServer)
            {
                multiConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
            }
            else
            {
                multiConfigs = AccelByteSettingsV2.LoadSDKServerConfigFile();
            }
            if (multiConfigs == null)
            {
                if(isServer)
                {
                    multiConfigs = new MultiServerConfigs();
                }
                else
                {
                    multiConfigs = new MultiConfigs();
                }
                AccelByteDebug.LogWarning($"{serverFlagLog}SDK Config not found, using empty config");
            }

            multiOAuthConfigs.Expand();
            multiConfigs.Expand(false);

            oAuthConfig = multiOAuthConfigs.GetConfigFromEnvironment(environment);
            if (isServer)
            {
                serverSdkConfig = (ServerConfig)multiConfigs.GetConfigFromEnvironment(environment);
            }
            else
            {
                sdkConfig = (Config)multiConfigs.GetConfigFromEnvironment(environment);
            }
        }

        public AccelByteSettingsV2(OAuthConfig newOAuthConfig, Config newSdkConfig)
        {
            oAuthConfig = newOAuthConfig;
            sdkConfig = newSdkConfig;

            if(oAuthConfig != null)
            {
                oAuthConfig.Expand();
            }
            if (sdkConfig != null)
            {
                sdkConfig.SanitizeBaseUrl();
                sdkConfig.Expand();
            }
        }

        public AccelByteSettingsV2(OAuthConfig newOAuthConfig, ServerConfig newSdkServerConfig)
        {
            oAuthConfig = newOAuthConfig;
            serverSdkConfig = newSdkServerConfig;

            if (oAuthConfig != null)
            {
                oAuthConfig.Expand();
            }
            if (serverSdkConfig != null)
            {
                serverSdkConfig.Expand();
            }
        }

        internal void OverrideClientSDKConfig(SDKConfigArgs overrideConfig)
        {
            if (sdkConfig != null && overrideConfig != null)
            {
                if (overrideConfig.OverrideServiceUrl == true && overrideConfig.BaseUrl != null)
                {
                    sdkConfig.BaseUrl = overrideConfig.BaseUrl;
                    sdkConfig.SanitizeBaseUrl();
                    sdkConfig.Expand(true);
                }
                sdkConfig = overrideConfig.CopyToConfig(sdkConfig);
            }
        }

        internal void OverrideServerSDKConfig(SDKConfigArgs overrideConfig)
        {
            if (serverSdkConfig != null && overrideConfig != null)
            {
                if (overrideConfig.OverrideServiceUrl == true && !string.IsNullOrEmpty(overrideConfig.BaseUrl))
                {
                    serverSdkConfig.BaseUrl = overrideConfig.BaseUrl;
                    serverSdkConfig.SanitizeBaseUrl();
                    serverSdkConfig.Expand(true);
                }
                serverSdkConfig = overrideConfig.CopyToConfig(serverSdkConfig);
            }
        }

        internal void OverrideOAuthConfig(OAuthConfig overrideConfig)
        {
            if (oAuthConfig != null && overrideConfig != null)
            {
                if (overrideConfig.ClientId != null)
                {
                    oAuthConfig.ClientId = overrideConfig.ClientId;
                }
                if (overrideConfig.ClientSecret != null)
                {
                    oAuthConfig.ClientSecret = overrideConfig.ClientSecret;
                }
                if (overrideConfig.GoogleWebClientId != null)
                {
                    oAuthConfig.GoogleWebClientId = overrideConfig.GoogleWebClientId;
                }
            }
        }
        public OAuthConfig CopyOAuthConfig() 
        { 
            return oAuthConfig != null ? oAuthConfig.ShallowCopy() : null; 
        }
        public Config CopyConfig() 
        {
            return sdkConfig != null ? sdkConfig.ShallowCopy() : null;
        }
        public void UpdateOAuthConfig(OAuthConfig newConfig) 
        { 
            this.oAuthConfig = newConfig;
        }
        public void UpdateConfig(Config newConfig) 
        {
            this.sdkConfig = newConfig;
        }
        public void UpdateServerConfig(ServerConfig newConfig)
        {
            this.serverSdkConfig = newConfig;
        }
    }
}
