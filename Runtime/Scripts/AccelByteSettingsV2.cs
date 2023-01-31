// Copyright (c) 2019 - 2023 AccelByte Inc. All Rights Reserved.
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
        internal const string DefaultConfigsResourceDirectory = "AccelByteSDK";
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
        public static string ConfigsDirectoryFullPath()
        {
            var accelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettingsV2).Assembly);
            string retval = System.IO.Path.Combine(accelBytePackageInfo.resolvedPath, "Configs", "Resources", ConfigsResourceDirectory);
            return retval;
        }
#endif

        public static string ConfigsResourceDirectory
        {
            get;
            set;
        } = DefaultConfigsResourceDirectory;

#if UNITY_EDITOR
        public static string SDKConfigFullPath(bool isServer)
        {
            string fileName = isServer ? "AccelByteServerSDKConfig.json" : "AccelByteSDKConfig.json";
            string retval = System.IO.Path.Combine(ConfigsDirectoryFullPath(), fileName);
            return retval;
        }
#endif

        public static string SDKConfigResourcePath(bool isServer = false)
        {
            string fileName = isServer ? "AccelByteServerSDKConfig" : "AccelByteSDKConfig";
            return System.IO.Path.Combine(ConfigsResourceDirectory, fileName);
        }

#if UNITY_EDITOR
        public static string OAuthFullPath(string platform, bool isServer = false)
        {
            string fileName = isServer ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            string retval = System.IO.Path.Combine(ConfigsDirectoryFullPath(), $"{fileName}{platform}.json");
            return retval;
        }
#endif

        public static string OAuthResourcePath(string platform, bool isServer)
        {
            string fileName = isServer ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            return System.IO.Path.Combine(ConfigsResourceDirectory, $"{fileName}{platform}");
        }

#if UNITY_EDITOR
        public static string SDKVersionFullPath()
        {
            return System.IO.Path.Combine(ConfigsDirectoryFullPath(), "AccelByteSDKVersion.json");
        }
#endif

        public static string SDKVersionResourcePath()
        {
            return System.IO.Path.Combine(ConfigsResourceDirectory, "AccelByteSDKVersion");
        }

        public static MultiOAuthConfigs LoadOAuthFile(string targetPlatform, bool isServerConfig = false)
        {
            var retval = new MultiOAuthConfigs();
            UnityEngine.Object targetOAuthFile = null;
            bool moveConfigFile = false;
            var oldOAuthFileName = isServerConfig ? "AccelByteServerSDKOAuthConfig" : "AccelByteSDKOAuthConfig";
            var oldOAuthFile = Resources.Load($"{oldOAuthFileName}{targetPlatform}");
            var oAuthFile = Resources.Load(OAuthResourcePath(targetPlatform, isServerConfig));

            if (oldOAuthFile != null && oAuthFile == null)
            {
                targetOAuthFile = oldOAuthFile;
                moveConfigFile = true;
            }
            else if (oAuthFile != null)
            {
                targetOAuthFile = oAuthFile;
            }

            if (targetOAuthFile != null)
            {
                string wholeOAuthJsonText = ((TextAsset)targetOAuthFile).text;
                retval = wholeOAuthJsonText.ToObject<MultiOAuthConfigs>();
            }

#if UNITY_EDITOR
            var projectRootDir = System.IO.Directory.GetCurrentDirectory();
            var oldOAuthFileFullPath = System.IO.Path.Combine(projectRootDir, "Assets", "Resources", $"{oldOAuthFileName}{targetPlatform}.json");
            if (oldOAuthFile != null && System.IO.File.Exists(oldOAuthFileFullPath))
            {
                System.IO.File.Delete(oldOAuthFileFullPath);
            }
            if (moveConfigFile)
            {
                SaveConfig(retval, OAuthFullPath(targetPlatform, isServerConfig));
            }
#endif
            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        public static OAuthConfig GetOAuthByEnvironment(MultiOAuthConfigs multiOAuthConfigs, SettingsEnvironment environment)
        {
            OAuthConfig retval = null;
            switch (environment)
            {
                case SettingsEnvironment.Development:
                    retval = multiOAuthConfigs.Development;
                    break;
                case SettingsEnvironment.Certification:
                    retval = multiOAuthConfigs.Certification;
                    break;
                case SettingsEnvironment.Production:
                    retval = multiOAuthConfigs.Production;
                    break;
                case SettingsEnvironment.Default:
                default:
                    retval = multiOAuthConfigs.Default;
                    break;
            }

            return retval;
        }

        public static MultiOAuthConfigs SetOAuthByEnvironment(MultiOAuthConfigs multiOAuthConfigs, OAuthConfig newOauthConfig, SettingsEnvironment environment)
        {
            MultiOAuthConfigs retval = multiOAuthConfigs;
            switch (environment)
            {
                case SettingsEnvironment.Development:
                    retval.Development = newOauthConfig;
                    break;
                case SettingsEnvironment.Certification:
                    retval.Certification = newOauthConfig;
                    break;
                case SettingsEnvironment.Production:
                    retval.Production = newOauthConfig;
                    break;
                case SettingsEnvironment.Default:
                default:
                    retval.Default = newOauthConfig;
                    break;
            }

            return retval;
        }

        public static MultiConfigs LoadSDKConfigFile()
        {
            var retval = new MultiConfigs();
            UnityEngine.Object targetConfigFile = null;
            bool moveConfigFile = false;

            var oldConfigFileName = "AccelByteSDKConfig";
            var oldConfigFile = Resources.Load(oldConfigFileName);
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

#if UNITY_EDITOR
            var projectRootDir = System.IO.Directory.GetCurrentDirectory();

            var oldConfigFileFullPath = System.IO.Path.Combine(projectRootDir, "Assets", "Resources", $"{oldConfigFileName}.json");
            if (oldConfigFile != null && System.IO.File.Exists(oldConfigFileFullPath))
            {
                System.IO.File.Delete(oldConfigFileFullPath);
            }
            if (moveConfigFile)
            {
                SaveConfig(retval, SDKConfigFullPath(false));
            }
#endif
            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        public static MultiServerConfigs LoadSDKServerConfigFile()
        {
            var retval = new MultiServerConfigs();
            UnityEngine.Object targetConfigFile = null;
            bool moveConfigFile = false;

            var oldConfigFileName = "AccelByteServerSDKConfig";
            var oldConfigFile = Resources.Load(oldConfigFileName);
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

#if UNITY_EDITOR
            var projectRootDir = System.IO.Directory.GetCurrentDirectory();

            var oldConfigFileFullPath = System.IO.Path.Combine(projectRootDir, "Assets", "Resources", $"{oldConfigFileName}.json");
            if (oldConfigFile != null && System.IO.File.Exists(oldConfigFileFullPath))
            {
                System.IO.File.Delete(oldConfigFileFullPath);
            }
            if (moveConfigFile)
            {
                SaveConfig(retval, SDKConfigFullPath(true));
            }
#endif
            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }

        public static Config GetSDKConfigByEnvironment(MultiConfigs multiSDKConfigs, SettingsEnvironment environment)
        {
            Config retval = null;
            switch (environment)
            {
                case SettingsEnvironment.Development:
                    retval = multiSDKConfigs.Development;
                    break;
                case SettingsEnvironment.Certification:
                    retval = multiSDKConfigs.Certification;
                    break;
                case SettingsEnvironment.Production:
                    retval = multiSDKConfigs.Production;
                    break;
                case SettingsEnvironment.Default:
                default:
                    retval = multiSDKConfigs.Default;
                    break;
            }

            return retval;
        }

        public static MultiConfigs SetSDKConfigByEnvironment(MultiConfigs multiSDKConfig, Config newSDKConfig, SettingsEnvironment environment)
        {
            var retval = multiSDKConfig;
            switch (environment)
            {
                case SettingsEnvironment.Development:
                    retval.Development = newSDKConfig;
                    break;
                case SettingsEnvironment.Certification:
                    retval.Certification = newSDKConfig;
                    break;
                case SettingsEnvironment.Production:
                    retval.Production = newSDKConfig;
                    break;
                case SettingsEnvironment.Default:
                default:
                    retval.Default = newSDKConfig;
                    break;
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
            string configPath = ConfigsDirectoryFullPath();

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

            string configPath = ConfigsDirectoryFullPath();
            if (!System.IO.Directory.Exists(configPath))
            {
                System.IO.Directory.CreateDirectory(configPath);
            }

            string SDKVersionPath = SDKVersionFullPath();
            System.IO.File.WriteAllBytes(SDKVersionPath, Encoding.ASCII.GetBytes(versionTextRaw));

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
                case RuntimePlatform.LinuxPlayer:
                    if (Resources.Load(AccelByteSettingsV2.OAuthResourcePath(PlatformType.Steam.ToString(), isServer)) != null)
                    {
                        activePlatform = PlatformType.Steam.ToString();
                    }
                    else if (Resources.Load(AccelByteSettingsV2.OAuthResourcePath(PlatformType.EpicGames.ToString(), isServer)) != null)
                    {
                        activePlatform = PlatformType.EpicGames.ToString();
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
                case RuntimePlatform.XBOX360:
                case RuntimePlatform.XboxOne:
                    activePlatform = PlatformType.Live.ToString();
                    break;
                case RuntimePlatform.Switch:
                    activePlatform = PlatformType.Nintendo.ToString();
                    break;
#if UNITY_2019_3_OR_NEWER
                case RuntimePlatform.Stadia:
                    activePlatform = PlatformType.Stadia.ToString();
                    break;
#endif
                default:
                    activePlatform = "";
                    break;
            }
            return activePlatform;
        }

        public AccelByteSettingsV2(string platform, SettingsEnvironment environment, bool isServer)
        {
            MultiOAuthConfigs multiOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(platform, isServer);
            string serverFlagLog = isServer ? "" : "Server ";
            if (multiOAuthConfigs == null)
            {
                multiOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile("", isServer);
                if (multiOAuthConfigs != null)
                {
                    AccelByteDebug.Log($"{serverFlagLog}OAuth {platform} config not found, using default OAuth config");
                }
            }

            dynamic multiConfigs;
            if(!isServer)
            {
                multiConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
            }
            else
            {
                multiConfigs = AccelByteSettingsV2.LoadSDKServerConfigFile();
            }

            if (multiOAuthConfigs == null)
            {
                multiOAuthConfigs = new MultiOAuthConfigs();
                AccelByteDebug.LogWarning($"{serverFlagLog}OAuth config not found, using empty config");
            }
            if (multiConfigs == null)
            {
                multiConfigs = new MultiConfigs();
                AccelByteDebug.LogWarning($"{serverFlagLog}SDK Config not found, using empty config");
            }

            multiOAuthConfigs.Expand();
            multiConfigs.Expand();

            switch (environment)
            {
                case SettingsEnvironment.Development:
                    oAuthConfig = multiOAuthConfigs.Development;
                    if(isServer)
                    {
                        serverSdkConfig = multiConfigs.Development;
                    }
                    else
                    {
                        sdkConfig = multiConfigs.Development;
                    }
                    break;
                case SettingsEnvironment.Certification:
                    oAuthConfig = multiOAuthConfigs.Certification;
                    if (isServer)
                    {
                        serverSdkConfig = multiConfigs.Certification;
                    }
                    else
                    {
                        sdkConfig = multiConfigs.Certification;
                    }
                    break;
                case SettingsEnvironment.Production:
                    oAuthConfig = multiOAuthConfigs.Production;
                    if (isServer)
                    {
                        serverSdkConfig = multiConfigs.Production;
                    }
                    else
                    {
                        sdkConfig = multiConfigs.Production;
                    }
                    break;
                case SettingsEnvironment.Default:
                default:
                    oAuthConfig = multiOAuthConfigs.Default;
                    if (isServer)
                    {
                        serverSdkConfig = multiConfigs.Default;
                    }
                    else
                    {
                        sdkConfig = multiConfigs.Default;
                    }
                    break;
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
