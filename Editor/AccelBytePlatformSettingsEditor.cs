// Copyright (c) 2019-2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Models;

namespace AccelByte.Api
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class AccelBytePlatformSettingsEditor : EditorWindow
    {
        private static AccelBytePlatformSettingsEditor instance;
        public static AccelBytePlatformSettingsEditor Instance
        {
            get
            {
                return instance;
            }
        }
        private const string windowTitle = "AccelByte Configuration";
        private Texture2D accelByteLogo;
        private int temporaryEnvironmentSetting;
        private int temporaryPlatformSetting;
        private string[] environmentList;
        private string[] platformList;
        private Rect logoRect;

        private MultiOAuthConfigs originalOAuthConfigs;
        private MultiConfigs originalSdkConfigs;
        private OAuthConfig editedOAuthConfig;
        private Config editedSdkConfig;
        private Vector2 scrollPos;
        private bool showCacheConfigs;
        private bool showOtherConfigs;
        private bool showLogConfigs;
        private bool showServiceUrlConfigs;
        private bool showTURNconfigs;
        private GUIStyle requiredTextFieldGUIStyle;
        private bool initialized;

        [MenuItem("AccelByte/Edit Settings")]
        public static void Edit()
        {
            // Get existing open window or if none, make a new one:
            if (instance != null)
            {
                instance.CloseFinal();
            }

            instance = EditorWindow.GetWindow<AccelBytePlatformSettingsEditor>(windowTitle, true, System.Type.GetType("UnityEditor.ConsoleWindow,UnityEditor.dll"));
            instance.Show();
        }

        private void Initialize()
        {
            if (!initialized)
            {
                requiredTextFieldGUIStyle = new GUIStyle();
                requiredTextFieldGUIStyle.normal.textColor = Color.yellow;

                accelByteLogo = Resources.Load<Texture2D>("ab-logo");

                platformList = new string[]
                {
                    PlatformType.Steam.ToString(),
                    PlatformType.Apple.ToString(),
                    PlatformType.iOS.ToString(),
                    PlatformType.Android.ToString(),
                    PlatformType.PS4.ToString(),
                    PlatformType.PS5.ToString(),
                    PlatformType.Live.ToString(),
                    PlatformType.Nintendo.ToString(),
                    "Default"
                };
                this.temporaryPlatformSetting = platformList.Length - 1;

                environmentList = new string[]
                {
                    "Development",
                    "Certification",
                    "Production",
                    "Default"
                };
                temporaryEnvironmentSetting = environmentList.Length - 1;

                logoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);
                initialized = true;
            }

            if(originalSdkConfigs == null)
            {
                originalSdkConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
                if(originalSdkConfigs == null)
                {
                    originalSdkConfigs = new MultiConfigs();
                }
            }
            if (originalOAuthConfigs == null)
            {
                originalOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(GetPlatformName(platformList, temporaryPlatformSetting));
                if (originalOAuthConfigs == null)
                {
                    originalOAuthConfigs = new MultiOAuthConfigs();
                }
            }
            if (editedSdkConfig == null)
            {
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);
                editedSdkConfig = originalSdkConfig != null ? originalSdkConfig.ShallowCopy() : new Config();
            }
            if (editedOAuthConfig == null)
            {
                var originalOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);
                editedOAuthConfig = originalOAuthConfig != null ? originalOAuthConfig.ShallowCopy() : new OAuthConfig();
            }
        }

        private void CloseFinal()
        {
            Close();
            instance = null;
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            Initialize();

            logoRect.x = (this.position.width - 300) / 2;
            GUI.DrawTexture(logoRect, accelByteLogo);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(100);

            if (EditorApplication.isPlaying)
            {
                CloseFinal();
                return;
            }

            {
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);
                var originalOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);

                if (CompareOAuthConfig(editedOAuthConfig, originalOAuthConfig) && CompareConfig(editedSdkConfig, originalSdkConfig))
                {
                    EditorGUILayout.HelpBox("All configs has been saved!", MessageType.Info, true);
                }
                else
                {
                    EditorGUILayout.HelpBox("Unsaved changes", MessageType.Warning, true);
                }
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SDK Version");
            EditorGUILayout.LabelField(AccelByteSettingsV2.AccelByteSDKVersion);
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Environment");
            EditorGUI.BeginChangeCheck();
            temporaryEnvironmentSetting = EditorGUILayout.Popup(temporaryEnvironmentSetting, environmentList);
            if (EditorGUI.EndChangeCheck())
            {
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);
                var originalOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);

                editedSdkConfig = originalSdkConfig != null ? originalSdkConfig.ShallowCopy() : new Config();
                editedOAuthConfig = originalOAuthConfig != null ? originalOAuthConfig.ShallowCopy() : new OAuthConfig();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform");
            EditorGUI.BeginChangeCheck();
            temporaryPlatformSetting = EditorGUILayout.Popup(temporaryPlatformSetting, platformList);
            if (EditorGUI.EndChangeCheck())
            {
                string targetPlatform = "";
                if (platformList[temporaryPlatformSetting] != "Default")
                {
                    targetPlatform = platformList[temporaryPlatformSetting];
                }
                originalOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(targetPlatform);
                editedOAuthConfig = originalOAuthConfigs != null ? AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting) : new OAuthConfig();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            CreateTextInput((newValue) => editedSdkConfig.BaseUrl = newValue, editedSdkConfig.BaseUrl, "Base Url", true);
            CreateTextInput((newValue) => editedSdkConfig.RedirectUri = newValue, editedSdkConfig.RedirectUri, "Redirect Uri", true);
            CreateTextInput((newValue) => editedSdkConfig.Namespace = newValue, editedSdkConfig.Namespace, "Namespace", true);
            CreateTextInput((newValue) => editedSdkConfig.PublisherNamespace = newValue, editedSdkConfig.PublisherNamespace, "Publisher Namespace", true);
            CreateTextInput((newValue) => editedOAuthConfig.ClientId = newValue, editedOAuthConfig.ClientId, "Client Id", true);
            CreateTextInput((newValue) => editedOAuthConfig.ClientSecret = newValue, editedOAuthConfig.ClientSecret, "Client Secret");
            CreateTextInput((newValue) => editedSdkConfig.AppId = newValue, editedSdkConfig.AppId, "App Id");

            showCacheConfigs = EditorGUILayout.Foldout(showCacheConfigs, "Cache Configs");
            if (showCacheConfigs)
            {
                Action<double> onCacheSizeChanged = (newValue) =>
                {
                    if(newValue > 0)
                    {
                        editedSdkConfig.MaximumCacheSize = Mathf.FloorToInt((float)newValue);
                    }
                };
                CreateNumberInput(onCacheSizeChanged, editedSdkConfig.MaximumCacheSize, "Cache Size");

                Action<double> onCacheLifeTimeChanged = (newValue) =>
                {
                    if (newValue > 0)
                    {
                        editedSdkConfig.MaximumCacheLifeTime = Mathf.FloorToInt((float)newValue);
                    }
                };
                CreateNumberInput(onCacheLifeTimeChanged, editedSdkConfig.MaximumCacheLifeTime, "Cache Life Time (Seconds)");
            }

            showOtherConfigs = EditorGUILayout.Foldout(showOtherConfigs, "Other Configs");
            if (showOtherConfigs)
            {
                CreateTextInput((newValue) => editedSdkConfig.CustomerName = newValue, editedSdkConfig.CustomerName, "Customer Name");
                CreateToggleInput((newValue) => editedSdkConfig.UsePlayerPrefs = newValue, editedSdkConfig.UsePlayerPrefs, "Use PlayerPrefs");
            }

            showLogConfigs = EditorGUILayout.Foldout(showLogConfigs, "Log Configs");
            if (showLogConfigs)
            {
                CreateToggleInput((newValue) => editedSdkConfig.EnableDebugLog = newValue, editedSdkConfig.EnableDebugLog, "Enable Debug Log");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Log Type Filter");
                AccelByte.Core.AccelByteLogType currentLogFilter;
                if (!Enum.TryParse(editedSdkConfig.DebugLogFilter, out currentLogFilter))
                {
                    currentLogFilter = AccelByte.Core.AccelByteLogType.Verbose;
                }
                var newLogFilter = (AccelByte.Core.AccelByteLogType)EditorGUILayout.EnumPopup(currentLogFilter);
                editedSdkConfig.DebugLogFilter = newLogFilter.ToString();

                EditorGUILayout.LabelField("");
                EditorGUILayout.EndHorizontal();
            }

            showServiceUrlConfigs = EditorGUILayout.Foldout(showServiceUrlConfigs, "Service Url Configs");
            if(showServiceUrlConfigs)
            {
                CreateTextInput((newValue) => editedSdkConfig.IamServerUrl = newValue, editedSdkConfig.IamServerUrl, "IAM Server Url");
                CreateTextInput((newValue) => editedSdkConfig.PlatformServerUrl = newValue, editedSdkConfig.PlatformServerUrl, "Platform Server Url");
                CreateTextInput((newValue) => editedSdkConfig.BasicServerUrl = newValue, editedSdkConfig.BasicServerUrl, "Basic Server Url");
                CreateTextInput((newValue) => editedSdkConfig.LobbyServerUrl = newValue, editedSdkConfig.LobbyServerUrl, "Lobby Server Url");
                CreateTextInput((newValue) => editedSdkConfig.CloudStorageServerUrl = newValue, editedSdkConfig.CloudStorageServerUrl, "Cloud Storage Server Url");
                CreateTextInput((newValue) => editedSdkConfig.GameProfileServerUrl = newValue, editedSdkConfig.GameProfileServerUrl, "Game Profile Server Url");
                CreateTextInput((newValue) => editedSdkConfig.StatisticServerUrl = newValue, editedSdkConfig.StatisticServerUrl, "Statistic Server Url");
                CreateTextInput((newValue) => editedSdkConfig.AchievementServerUrl = newValue, editedSdkConfig.AchievementServerUrl, "Achievement Server Url");
                CreateTextInput((newValue) => editedSdkConfig.CloudSaveServerUrl = newValue, editedSdkConfig.CloudSaveServerUrl, "CloudSave Server Url");
                CreateTextInput((newValue) => editedSdkConfig.AgreementServerUrl = newValue, editedSdkConfig.AgreementServerUrl, "Agreement Server Url");
                CreateTextInput((newValue) => editedSdkConfig.LeaderboardServerUrl = newValue, editedSdkConfig.LeaderboardServerUrl, "Leaderboard Server Url");
                CreateTextInput((newValue) => editedSdkConfig.GameTelemetryServerUrl = newValue, editedSdkConfig.GameTelemetryServerUrl, "Game Telemetry Server Url");
                CreateTextInput((newValue) => editedSdkConfig.GroupServerUrl = newValue, editedSdkConfig.GroupServerUrl, "Group Server Url");
                CreateTextInput((newValue) => editedSdkConfig.SeasonPassServerUrl = newValue, editedSdkConfig.SeasonPassServerUrl, "Season Pass Server Url");
                CreateTextInput((newValue) => editedSdkConfig.SessionBrowserServerUrl = newValue, editedSdkConfig.SessionBrowserServerUrl, "Session BrowserServer Url");
                CreateTextInput((newValue) => editedSdkConfig.SessionServerUrl = newValue, editedSdkConfig.SessionServerUrl, "Session Server Url");
                CreateTextInput((newValue) => editedSdkConfig.MatchmakingV2ServerUrl = newValue, editedSdkConfig.MatchmakingV2ServerUrl, "MatchmakingV2 Server Url");
            }

            showTURNconfigs = EditorGUILayout.Foldout(showTURNconfigs, "TURN Configs");
            if (showTURNconfigs)
            {
                CreateToggleInput((newValue) => editedSdkConfig.UseTurnManager = newValue, editedSdkConfig.UseTurnManager, "Use TURN Manager");
                CreateToggleInput((newValue) => editedSdkConfig.EnableAuthHandshake = newValue, editedSdkConfig.EnableAuthHandshake, "Use Secure Handshaking");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerHost = newValue, editedSdkConfig.TurnServerHost, "TURN Server Host");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerPort = newValue, editedSdkConfig.TurnServerPort, "TURN Server Port");
                CreateTextInput((newValue) => editedSdkConfig.TurnManagerServerUrl = newValue, editedSdkConfig.TurnManagerServerUrl, "TURN Manager Server Url");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerUsername = newValue, editedSdkConfig.TurnServerUsername, "TURN Server Username");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerSecret = newValue, editedSdkConfig.TurnServerSecret, "TURN Server Secret");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerPassword = newValue, editedSdkConfig.TurnServerPassword, "TURN Server Password");
                CreateNumberInput((newvalue) => editedSdkConfig.PeerMonitorIntervalMs = (int)newvalue, editedSdkConfig.PeerMonitorIntervalMs, "Peer Monitor Interval in Milliseconds");
                CreateNumberInput((newvalue) => editedSdkConfig.PeerMonitorTimeoutMs = (int)newvalue, editedSdkConfig.PeerMonitorTimeoutMs, "Peer Monitor Timeout in Milliseconds");
                CreateNumberInput((newvalue) => editedSdkConfig.HostCheckTimeoutInSeconds = (int)newvalue, editedSdkConfig.HostCheckTimeoutInSeconds, "Host Check Timeout in Seconds");
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                editedOAuthConfig.Expand();
                editedSdkConfig.SanitizeBaseUrl();
                editedSdkConfig.Expand();

                originalOAuthConfigs = AccelByteSettingsV2.SetOAuthByEnvironment(originalOAuthConfigs, editedOAuthConfig, (SettingsEnvironment) temporaryEnvironmentSetting);
                originalSdkConfigs = AccelByteSettingsV2.SetSDKConfigByEnvironment(originalSdkConfigs, editedSdkConfig, (SettingsEnvironment)temporaryEnvironmentSetting);
                AccelByteSettingsV2.SaveConfig(originalOAuthConfigs, AccelByteSettingsV2.OAuthFullPath(GetPlatformName(platformList, temporaryPlatformSetting)));
                AccelByteSettingsV2.SaveConfig(originalSdkConfigs, AccelByteSettingsV2.SDKConfigFullPath(false));
            }

            EditorGUILayout.EndVertical();
#endif
        }

        private void CreateNumberInput(Action<double> setter, double defaultValue, string fieldLabel, bool required = false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.DoubleField(defaultValue);
            setter?.Invoke(newValue);

            string requiredText = "";
            if (required)
            {
                requiredText = "Required";
            }
            EditorGUILayout.LabelField(requiredText, requiredTextFieldGUIStyle);

            EditorGUILayout.EndHorizontal();
        }

        private void CreateTextInput(Action<string> setter, string defaultValue, string fieldLabel, bool required = false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.TextField(defaultValue);
            setter?.Invoke(newValue);

            string requiredText = "";
            if(required && string.IsNullOrEmpty(newValue))
            {
                requiredText = "Required";
            }
            EditorGUILayout.LabelField(requiredText, requiredTextFieldGUIStyle);

            EditorGUILayout.EndHorizontal();
        }

        private void CreateToggleInput(Action<bool> setter, bool defaultValue, string fieldLabel)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.Toggle(defaultValue);
            setter?.Invoke(newValue);

            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();
        }

        private bool CompareOAuthConfig(OAuthConfig firstConfig, OAuthConfig secondConfig)
        {
            if (firstConfig == null || secondConfig == null)
            {
                return false;
            }
            return firstConfig.Compare(secondConfig);
        }

        private bool CompareConfig(Config firstConfig, Config secondConfig)
        {
            if (firstConfig == null || secondConfig == null)
            {
                return false;
            }
            return firstConfig.Compare(secondConfig);
        }

        private string GetPlatformName(string[] platformList, int index)
        {
            string targetPlatform = "";
            if (platformList[index] != "Default")
            {
                targetPlatform = platformList[index];
            }
            return targetPlatform;
        }
    }
}
