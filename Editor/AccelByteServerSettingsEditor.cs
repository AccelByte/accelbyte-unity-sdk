// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Models;
using UnityEditor;
using UnityEngine;

namespace AccelByte.Editor
{
	public class AccelByteServerSettingsEditor : EditorWindow
    {
        private static AccelByteServerSettingsEditor instance;
        public static AccelByteServerSettingsEditor Instance
        {
            get
            {
                return instance;
            }
        }
        private const string windowTitle = "AccelByte Server Config";
        private int temporaryEnvironmentSetting;
        private int temporaryPlatformSetting;
        private Rect logoRect;

        private MultiOAuthConfigs originalServerOAuthConfigs;
        private MultiServerConfigs originalServerConfigs;
        private OAuthConfig editedServerOAuthConfig;
        private ServerConfig editedServerConfig;

        private Vector2 scrollPos;
        private Dictionary<string, bool> foldoutConfigStatus;
        private bool initialized;
        private bool generateServiceUrl = true;

        [MenuItem("AccelByte/Edit Server Settings")]
        public static void OpenWindow()
        {
            // Get existing open window or if none, make a new one:
            if (instance != null)
            {
                instance.CloseFinal();
            }

            instance = EditorWindow.GetWindow<AccelByteServerSettingsEditor>(windowTitle, true, System.Type.GetType("UnityEditor.ConsoleWindow,UnityEditor.dll"));
            instance.Show();
        }

        private void CloseFinal()
        {
            Close();
            instance = null;
        }

        private void Initialize()
        {
            if (!initialized)
            {
                temporaryPlatformSetting = 0;
                temporaryEnvironmentSetting = 0;
                logoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);
                initialized = true;
            }

            if (originalServerConfigs == null)
            {
                originalServerConfigs = AccelByteSettingsV2.LoadSDKServerConfigFile();
                if (originalServerConfigs == null)
                {
                    originalServerConfigs = new MultiServerConfigs();
                    originalServerConfigs.InitializeNullEnv();
                }
            }

            if (originalServerOAuthConfigs == null)
            {
                originalServerOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(string.Empty, isServerConfig: true);
                if (originalServerOAuthConfigs == null)
                {
                    originalServerOAuthConfigs = new MultiOAuthConfigs();
                    originalServerOAuthConfigs.InitializeNullEnv();
                }
            }

            SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
            if (editedServerConfig == null)
            {
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalServerConfigs, targetEnvironment);
                editedServerConfig = originalSdkConfig != null ? originalSdkConfig.ShallowCopy() : new ServerConfig();
            }
            if (editedServerOAuthConfig == null)
            {
                var originalClientOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalServerOAuthConfigs, targetEnvironment);
                editedServerOAuthConfig = originalClientOAuthConfig != null ? originalClientOAuthConfig.ShallowCopy() : new OAuthConfig();
            }

            if (foldoutConfigStatus == null)
            {
                foldoutConfigStatus = new Dictionary<string, bool>();
            }
        }

        private void OnGUI()
        {
            Initialize();

            logoRect.x = (this.position.width - 300) / 2;
            GUI.DrawTexture(logoRect, EditorCommon.AccelByteLogo);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(100);
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Editor Deactivated On Runtime", UnityEditor.MessageType.Info, wide: true);
                EditorGUILayout.EndVertical();
                return;
            }

            {
                SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalServerConfigs, targetEnvironment);
                var originalClientOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalServerOAuthConfigs, targetEnvironment);

                bool oAuthConfigIdentical = EditorCommon.CompareConfig(editedServerOAuthConfig, originalClientOAuthConfig);
                bool serverConfigIdentical = EditorCommon.CompareConfig(editedServerConfig, originalSdkConfig);

                if (!oAuthConfigIdentical || !serverConfigIdentical)
                {
                    EditorGUILayout.HelpBox("Unsaved changes", UnityEditor.MessageType.Warning, wide: true);
                }
                else
                {
                    EditorGUILayout.HelpBox("No changes detected", UnityEditor.MessageType.Info, wide: true);
                }
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, alwaysShowHorizontal: false, alwaysShowVertical: true);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SDK Version");
            EditorGUILayout.LabelField(AccelByteSettingsV2.AccelByteSDKVersion);
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Environment");
            EditorGUI.BeginChangeCheck();
            temporaryEnvironmentSetting = EditorGUILayout.Popup(temporaryEnvironmentSetting, EditorCommon.EnvironmentList);
            if (EditorGUI.EndChangeCheck())
            {
                SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
                var originalServerConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalServerConfigs, targetEnvironment);
                var originalServerOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalServerOAuthConfigs, targetEnvironment);

                editedServerConfig = originalServerConfig != null ? originalServerConfig.ShallowCopy() : new ServerConfig();
                editedServerOAuthConfig = originalServerOAuthConfig != null ? originalServerOAuthConfig.ShallowCopy() : new OAuthConfig();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorCommon.CreateTextInput((newValue) => editedServerConfig.BaseUrl = newValue, editedServerConfig.BaseUrl, "Base Url", required: true);
            EditorCommon.CreateTextInput((newValue) => editedServerConfig.RedirectUri = newValue, editedServerConfig.RedirectUri, "Redirect Uri", required: true);
            EditorCommon.CreateTextInput((newValue) => editedServerConfig.Namespace = newValue, editedServerConfig.Namespace, "Namespace Id", required: true);
            EditorCommon.CreateTextInput((newValue) => editedServerOAuthConfig.ClientId = newValue, editedServerOAuthConfig.ClientId, "Client Id", required: true);
            EditorCommon.CreateTextInput((newValue) => editedServerOAuthConfig.ClientSecret = newValue, editedServerOAuthConfig.ClientSecret, "Client Secret");

            #region Log
            if (EditorCommon.CreateFoldout("Log Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedServerConfig.EnableDebugLog = newValue, editedServerConfig.EnableDebugLog, "Enable Debug Log", indentLevel:1);

                EditorGUILayout.BeginHorizontal();
                EditorCommon.AddIndent(depth:1);
                EditorGUILayout.LabelField("Log Type Filter");
                AccelByte.Core.AccelByteLogType currentLogFilter;
                if (!Enum.TryParse(editedServerConfig.DebugLogFilter, out currentLogFilter))
                {
                    currentLogFilter = AccelByte.Core.AccelByteLogType.Verbose;
                }
                var newLogFilter = (AccelByte.Core.AccelByteLogType)EditorGUILayout.EnumPopup(currentLogFilter);
                editedServerConfig.DebugLogFilter = newLogFilter.ToString();

                EditorGUILayout.LabelField("");
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region URL
            if (EditorCommon.CreateFoldout("Service Url Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => generateServiceUrl = newValue, generateServiceUrl, "Auto Generate Service Url", indentLevel: 1);

                EditorCommon.CreateTextInput((newValue) => editedServerConfig.IamServerUrl = newValue, editedServerConfig.IamServerUrl, "IAM Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput(
                    (newValue) => editedServerConfig.AMSServerUrl = newValue
                    , defaultValue: editedServerConfig.AMSServerUrl
                    , fieldLabel: "AMS Server Url"
                    , required: false
                    , @readonly: generateServiceUrl
                    , indentLevel: 1
                );
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.DSHubServerUrl = newValue, editedServerConfig.DSHubServerUrl, "DS Hub Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.DSMControllerServerUrl = newValue, editedServerConfig.DSMControllerServerUrl, "DSM Controller Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.StatisticServerUrl = newValue, editedServerConfig.StatisticServerUrl, "Statistic Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.UGCServerUrl = newValue, editedServerConfig.UGCServerUrl, "UGC Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.PlatformServerUrl = newValue, editedServerConfig.PlatformServerUrl, "Platform Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.QosManagerServerUrl = newValue, editedServerConfig.QosManagerServerUrl, "QoS Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.GameTelemetryServerUrl = newValue, editedServerConfig.GameTelemetryServerUrl, "Game Telemetry Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.AchievementServerUrl = newValue, editedServerConfig.AchievementServerUrl, "Achievement Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.LobbyServerUrl = newValue, editedServerConfig.LobbyServerUrl, "Lobby Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.SessionServerUrl = newValue, editedServerConfig.SessionServerUrl, "Session Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.CloudSaveServerUrl = newValue, editedServerConfig.CloudSaveServerUrl, "CloudSave Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.MatchmakingV2ServerUrl = newValue, editedServerConfig.MatchmakingV2ServerUrl, "MatchmakingV2 Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.SeasonPassServerUrl = newValue, editedServerConfig.SeasonPassServerUrl, "Season Pass Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.WatchdogUrl = newValue, editedServerConfig.WatchdogUrl, "Watchdog Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedServerConfig.StatsDServerUrl = newValue, editedServerConfig.StatsDServerUrl, "Stat DS URL", false, generateServiceUrl, indentLevel: 1);
            }
            #endregion

            if (EditorCommon.CreateFoldout("Server Port", foldoutConfigStatus))
            {
                EditorCommon.CreateNumberInput((int newValue) => editedServerConfig.StatsDServerPort = newValue, editedServerConfig.StatsDServerPort, "Stat DS Port", false, indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Server DMetric", foldoutConfigStatus))
            {
                EditorCommon.CreateNumberInput((int newValue) => editedServerConfig.StatsDMetricInterval = newValue, editedServerConfig.StatsDMetricInterval, "DMetric Collection Interval (Seconds)", false, indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Pre-Defined Event Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedServerConfig.EnablePreDefinedEvent = newValue, editedServerConfig.EnablePreDefinedEvent, "Enable Pre-Defined Game Event", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Cache Configs", foldoutConfigStatus))
            {
                Action<double> onCacheSizeChanged = (newValue) =>
                {
                    if (newValue > 0)
                    {
                        editedServerConfig.MaximumCacheSize = Mathf.FloorToInt((float)newValue);
                    }
                };
                EditorCommon.CreateNumberInput(onCacheSizeChanged, editedServerConfig.MaximumCacheSize, "Cache Size", indentLevel: 1);

                Action<double> onCacheLifeTimeChanged = (newValue) =>
                {
                    if (newValue > 0)
                    {
                        editedServerConfig.MaximumCacheLifeTime = Mathf.FloorToInt((float)newValue);
                    }
                };
                EditorCommon.CreateNumberInput(onCacheLifeTimeChanged, editedServerConfig.MaximumCacheLifeTime, "Cache Life Time (Seconds)", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Websocket Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateNumberInput((newValue) => editedServerConfig.DSHubReconnectTotalTimeout = 
                    (int)newValue, editedServerConfig.DSHubReconnectTotalTimeout, "DS Hub Reconnect Total Timeout (ms)"
                    , indentLevel: 1);
                EditorCommon.CreateNumberInput((newValue) => editedServerConfig.AMSReconnectTotalTimeout = 
                    (int)newValue, editedServerConfig.AMSReconnectTotalTimeout, "AMS Reconnect Total Timeout (ms)"
                    , indentLevel: 1);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                editedServerOAuthConfig.Expand();
                editedServerConfig.SanitizeBaseUrl();
                editedServerConfig.Expand(generateServiceUrl);

                SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
                originalServerOAuthConfigs = AccelByteSettingsV2.SetOAuthByEnvironment(originalServerOAuthConfigs, editedServerOAuthConfig.ShallowCopy(), targetEnvironment);
                originalServerConfigs = AccelByteSettingsV2.SetSDKConfigByEnvironment(originalServerConfigs, editedServerConfig.ShallowCopy(), targetEnvironment);

                string platformName = EditorCommon.GetPlatformName(EditorCommon.PlatformList, temporaryPlatformSetting);
                AccelByteSettingsV2.SaveConfig(originalServerOAuthConfigs, AccelByteSettingsV2.OAuthFullPath(string.Empty, isServer: true));
                AccelByteSettingsV2.SaveConfig(originalServerConfigs, AccelByteSettingsV2.SDKConfigFullPath(isServer:true));

                Debug.Log("AccelByte Server Config Is Updated");
            }

            EditorGUILayout.EndVertical();
        }
    }
}
