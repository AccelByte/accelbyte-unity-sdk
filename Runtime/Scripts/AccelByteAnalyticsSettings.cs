// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
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

namespace AccelByte.Api
{
    /// <summary>
    /// Primarily used by the Editor for the config in the popup menu.
    /// <para>Looking for runtime settings? See static AccelBytePlugin.Config</para>
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class AccelByteAnalyticsSettings
    {
        private const string defaultOAuthFileName = "AccelByteAnalyticsOAuthConfig";
#if UNITY_EDITOR
        public static string AnalyticsOAuthFullPath(string platform)
        {
            string retval = System.IO.Path.Combine(AccelByteSettingsV2.GeneratedConfigsDirectoryFullPath(), $"{defaultOAuthFileName}{platform}.json");
            return retval;
        }
#endif

        public static string AnalyticsOAuthResourcePath(string platform)
        {
            string retval = System.IO.Path.Combine(AccelByteSettingsV2.GeneratedConfigsResourceDirectory, $"{defaultOAuthFileName}{platform}");
            return retval;
        }

        public Config LoadClientConfigFile(SettingsEnvironment environment)
        {
            IAccelByteMultiConfigs clientMultiConfig = AccelByteSettingsV2.LoadSDKConfigFile();
            if (clientMultiConfig == null)
            {
                throw new System.Exception($"Client config is not found.\nPlease save the config on AccelByte's configuration");
            }
            Config retval = (Config)clientMultiConfig.GetConfigFromEnvironment(environment);
            return retval;
        }

        public ServerConfig LoadServerConfigFile(SettingsEnvironment environment)
        {
            IAccelByteMultiConfigs clientMultiConfig = AccelByteSettingsV2.LoadSDKServerConfigFile();
            if (clientMultiConfig == null)
            {
                throw new System.Exception($"Server config is not found.");
            }
            ServerConfig retval = (ServerConfig)clientMultiConfig.GetConfigFromEnvironment(environment);
            return retval;
        }

        public OAuthConfig LoadOAuthFile(string platform, SettingsEnvironment environment, bool useDefaultPlatformOnNotFound)
        {
            MultiOAuthConfigs multiOAuthConfigs = LoadOAuthFile(platform, useDefaultPlatformOnNotFound);
            OAuthConfig retval = multiOAuthConfigs.GetConfigFromEnvironment(environment);
            return retval;
        }

        public MultiOAuthConfigs LoadOAuthFile(string platform, bool useDefaultPlatformOnNotFound)
        {
            MultiOAuthConfigs retval = LoadOAuthFile(platform);
            if (retval == null)
            {
                if (useDefaultPlatformOnNotFound)
                {
                    retval = LoadOAuthFile(string.Empty);
                    if (retval != null)
                    {
                        AccelByteDebug.Log($"Analytic OAuth {platform} config not found, using default OAuth config");
                    }
                }
            }

            if (retval == null)
            {
                throw new System.Exception($"Analytic OAuth config not found.\nPlease set analytics oAuth config on AccelByte's configuration");
            }

            return retval;
        }

        protected virtual MultiOAuthConfigs LoadOAuthFile(string targetPlatform)
        {
            MultiOAuthConfigs retval = null;

            string oAuthPath = AnalyticsOAuthResourcePath(targetPlatform);
            var oAuthFile = Resources.Load(oAuthPath);

            if (oAuthFile != null)
            {
                string wholeOAuthJsonText = ((TextAsset)oAuthFile).text;
                retval = wholeOAuthJsonText.ToObject<MultiOAuthConfigs>();
            }

            if (retval != null)
            {
                retval.Expand();
            }

            return retval;
        }
    }
}
