// Copyright (c) 2019 - 2024 AccelByte Inc. All Rights Reserved.
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
    public class AccelByteClientSettingsEditor : EditorWindow
    {
        private static AccelByteClientSettingsEditor instance;
        public static AccelByteClientSettingsEditor Instance
        {
            get
            {
                return instance;
            }
        }
        private const string windowTitle = "AccelByte Client Config";
        private int temporaryEnvironmentSetting;
        private int temporaryPlatformSetting;
        private int temporaryPresenceBroadcastEventGameStateSetting;
        private string[] presenceBroadcastEventGameStateList;
        private Rect logoRect;

        private AccelByteAnalyticsSettings analyticsSettings;
        private MultiOAuthConfigs originalClientOAuthConfigs;
        private MultiOAuthConfigs originalAnalyticsOAuthConfigs;
        private MultiConfigs originalSdkConfigs;
        private OAuthConfig editedAnalyticsOAuthConfig;
        private OAuthConfig editedClientOAuthConfig;
        private Config editedSdkConfig;
        private Vector2 scrollPos;
        private Dictionary<string, bool> foldoutConfigStatus;
        private bool initialized;
        private bool generateServiceUrl = true;

        [MenuItem("AccelByte/Edit Client Settings")]
        public static void OpenWindow()
        {
            // Get existing open window or if none, make a new one:
            if (instance != null)
            {
                instance.CloseFinal();
            }

            instance = EditorWindow.GetWindow<AccelByteClientSettingsEditor>(windowTitle, true, System.Type.GetType("UnityEditor.ConsoleWindow,UnityEditor.dll"));
            instance.Show();
        }

        private void Initialize()
        {
            if (!initialized)
            {
                this.temporaryPlatformSetting = 0;

                temporaryEnvironmentSetting = 0;

                presenceBroadcastEventGameStateList = new string[]
                {
                    AccelByte.Utils.JsonUtils.SerializeWithStringEnum(
                        PresenceBroadcastEventGameState.OutOfGameplay),
                    AccelByte.Utils.JsonUtils.SerializeWithStringEnum(
                        PresenceBroadcastEventGameState.InGameplay),
                    AccelByte.Utils.JsonUtils.SerializeWithStringEnum(
                        PresenceBroadcastEventGameState.Store),
                };
                temporaryPresenceBroadcastEventGameStateSetting = 0;

                logoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);

                analyticsSettings = new AccelByteAnalyticsSettings();

                initialized = true;
            }

            if(originalSdkConfigs == null)
            {
                originalSdkConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
                if(originalSdkConfigs == null)
                {
                    originalSdkConfigs = new MultiConfigs();
                    originalSdkConfigs.InitializeNullEnv();
                }
            }
            if (originalClientOAuthConfigs == null)
            {
                string platformName = EditorCommon.GetPlatformName(EditorCommon.PlatformList, temporaryPlatformSetting);
                originalClientOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(platformName);
                if (originalClientOAuthConfigs == null)
                {
                    originalClientOAuthConfigs = new MultiOAuthConfigs();
                    originalClientOAuthConfigs.InitializeNullEnv();
                }
            }
            if (originalAnalyticsOAuthConfigs == null)
            {
                try
                {
                    string platformName = EditorCommon.GetPlatformName(EditorCommon.PlatformList, temporaryPlatformSetting);
                    originalAnalyticsOAuthConfigs = analyticsSettings.LoadOAuthFile(platformName, false);
                }
                catch (Exception)
                {

                }
                
                if (originalAnalyticsOAuthConfigs == null)
                {
                    originalAnalyticsOAuthConfigs = new MultiOAuthConfigs();
                    originalAnalyticsOAuthConfigs.InitializeNullEnv();
                }
            }

            SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
            if (editedSdkConfig == null)
            {
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, targetEnvironment);
                editedSdkConfig = originalSdkConfig != null ? originalSdkConfig.ShallowCopy() : new Config();
            }
            if (editedClientOAuthConfig == null)
            {
                var originalClientOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalClientOAuthConfigs, targetEnvironment);
                editedClientOAuthConfig = originalClientOAuthConfig != null ? originalClientOAuthConfig.ShallowCopy() : new OAuthConfig();
            }
            if (editedAnalyticsOAuthConfig == null)
            {
                var originalAnalyticsOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalAnalyticsOAuthConfigs, targetEnvironment);
                editedAnalyticsOAuthConfig = originalAnalyticsOAuthConfig != null ? originalAnalyticsOAuthConfig.ShallowCopy() : new OAuthConfig();
            }

            if(foldoutConfigStatus == null)
            {
                foldoutConfigStatus = new Dictionary<string, bool>();
            }
        }

        private void CloseFinal()
        {
            Close();
            instance = null;
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
                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, targetEnvironment);
                var originalClientOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalClientOAuthConfigs, targetEnvironment);
                var originalAnalyticsOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalAnalyticsOAuthConfigs, targetEnvironment);

                bool oAuthConfigIdentical = EditorCommon.CompareConfig(editedClientOAuthConfig, originalClientOAuthConfig);
                bool clientConfigIdentical = EditorCommon.CompareConfig(editedSdkConfig, originalSdkConfig);
                bool analyticsConfigIdentical = EditorCommon.CompareConfig(editedAnalyticsOAuthConfig, originalAnalyticsOAuthConfig);

                if (!oAuthConfigIdentical || !clientConfigIdentical || !analyticsConfigIdentical)
                {
                    EditorGUILayout.HelpBox("Unsaved changes", UnityEditor.MessageType.Warning, wide: true);
                }
                else
                {
                    EditorGUILayout.HelpBox("No changes detected", UnityEditor.MessageType.Info, wide: true);
                }
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, alwaysShowHorizontal:false, alwaysShowVertical:true);
            
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

                var originalSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, targetEnvironment);
                var originalClientOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalClientOAuthConfigs, targetEnvironment);
                var originalAnalyticsOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalAnalyticsOAuthConfigs, targetEnvironment);

                editedSdkConfig = originalSdkConfig != null ? originalSdkConfig.ShallowCopy() : new Config();
                editedClientOAuthConfig = originalClientOAuthConfig != null ? originalClientOAuthConfig.ShallowCopy() : new OAuthConfig();
                editedAnalyticsOAuthConfig = originalAnalyticsOAuthConfig != null ? originalAnalyticsOAuthConfig.ShallowCopy() : new OAuthConfig();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform");
            EditorGUI.BeginChangeCheck();
            temporaryPlatformSetting = EditorGUILayout.Popup(temporaryPlatformSetting, EditorCommon.PlatformList);
            if (EditorGUI.EndChangeCheck())
            {
                string targetPlatform = "";
                if (EditorCommon.PlatformList[temporaryPlatformSetting] != "Default")
                {
                    targetPlatform = EditorCommon.PlatformList[temporaryPlatformSetting];
                }
                SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);

                originalClientOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(targetPlatform);
                try
                {
                    originalAnalyticsOAuthConfigs = analyticsSettings.LoadOAuthFile(targetPlatform, false);
                }
                catch (Exception)
                {
                    originalAnalyticsOAuthConfigs = null;
                }

                editedClientOAuthConfig = originalClientOAuthConfigs != null ? AccelByteSettingsV2.GetOAuthByEnvironment(originalClientOAuthConfigs, targetEnvironment) : new OAuthConfig();
                editedAnalyticsOAuthConfig = originalClientOAuthConfigs != null ? AccelByteSettingsV2.GetOAuthByEnvironment(originalAnalyticsOAuthConfigs, targetEnvironment) : new OAuthConfig();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorCommon.CreateTextInput((newValue) => editedSdkConfig.BaseUrl = newValue, editedSdkConfig.BaseUrl, "Base Url", required: true);
            EditorCommon.CreateTextInput((newValue) => editedSdkConfig.RedirectUri = newValue, editedSdkConfig.RedirectUri, "Redirect Uri", required: true);
            EditorCommon.CreateTextInput((newValue) => editedSdkConfig.Namespace = newValue, editedSdkConfig.Namespace, "Namespace", required: true);
            EditorCommon.CreateTextInput((newValue) => editedSdkConfig.PublisherNamespace = newValue, editedSdkConfig.PublisherNamespace, "Publisher Namespace", required: true);
            EditorCommon.CreateTextInput((newValue) => editedClientOAuthConfig.ClientId = newValue, editedClientOAuthConfig.ClientId, "Client Id", required: true);
            EditorCommon.CreateTextInput((newValue) => editedClientOAuthConfig.ClientSecret = newValue, editedClientOAuthConfig.ClientSecret, "Client Secret");
            EditorCommon.CreateTextInput((newValue) => editedSdkConfig.AppId = newValue, editedSdkConfig.AppId, "App Id");

            if (EditorCommon.CreateFoldout("Analytics Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateTextInput((newValue) => editedAnalyticsOAuthConfig.ClientId = newValue, editedAnalyticsOAuthConfig.ClientId, "Analytics Client Id", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedAnalyticsOAuthConfig.ClientSecret = newValue, editedAnalyticsOAuthConfig.ClientSecret, "Analytics Client Secret", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Cache Configs", foldoutConfigStatus))
            {
                Action<double> onCacheSizeChanged = (newValue) =>
                {
                    if(newValue > 0)
                    {
                        editedSdkConfig.MaximumCacheSize = Mathf.FloorToInt((float)newValue);
                    }
                };
                EditorCommon.CreateNumberInput(onCacheSizeChanged, editedSdkConfig.MaximumCacheSize, "Cache Size", indentLevel: 1);

                Action<double> onCacheLifeTimeChanged = (newValue) =>
                {
                    if (newValue > 0)
                    {
                        editedSdkConfig.MaximumCacheLifeTime = Mathf.FloorToInt((float)newValue);
                    }
                };
                EditorCommon.CreateNumberInput(onCacheLifeTimeChanged, editedSdkConfig.MaximumCacheLifeTime, "Cache Life Time (Seconds)", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Other Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.CustomerName = newValue, editedSdkConfig.CustomerName, "Customer Name", indentLevel: 1);
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.UsePlayerPrefs = newValue, editedSdkConfig.UsePlayerPrefs, "Use PlayerPrefs", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Log Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnableDebugLog = newValue, editedSdkConfig.EnableDebugLog, "Enable Debug Log", indentLevel: 1);
                
                EditorGUILayout.BeginHorizontal();
                EditorCommon.AddIndent(depth: 1);
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

            if(EditorCommon.CreateFoldout("Service Url Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => generateServiceUrl = newValue, generateServiceUrl, "Auto Generate Service Url", indentLevel: 1);
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnableAmsServerQos = newValue, editedSdkConfig.EnableAmsServerQos, "Use AMS QoS Server Url", indentLevel: 1);

                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.IamServerUrl = newValue, editedSdkConfig.IamServerUrl, "IAM Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.PlatformServerUrl = newValue, editedSdkConfig.PlatformServerUrl, "Platform Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.BasicServerUrl = newValue, editedSdkConfig.BasicServerUrl, "Basic Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.LobbyServerUrl = newValue, editedSdkConfig.LobbyServerUrl, "Lobby Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.CloudStorageServerUrl = newValue, editedSdkConfig.CloudStorageServerUrl, "Cloud Storage Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.GameProfileServerUrl = newValue, editedSdkConfig.GameProfileServerUrl, "Game Profile Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.StatisticServerUrl = newValue, editedSdkConfig.StatisticServerUrl, "Statistic Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.AchievementServerUrl = newValue, editedSdkConfig.AchievementServerUrl, "Achievement Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.CloudSaveServerUrl = newValue, editedSdkConfig.CloudSaveServerUrl, "CloudSave Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.QosManagerServerUrl = newValue, editedSdkConfig.QosManagerServerUrl, "QoS Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.AgreementServerUrl = newValue, editedSdkConfig.AgreementServerUrl, "Agreement Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.LeaderboardServerUrl = newValue, editedSdkConfig.LeaderboardServerUrl, "Leaderboard Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.GameTelemetryServerUrl = newValue, editedSdkConfig.GameTelemetryServerUrl, "Game Telemetry Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.GroupServerUrl = newValue, editedSdkConfig.GroupServerUrl, "Group Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.SeasonPassServerUrl = newValue, editedSdkConfig.SeasonPassServerUrl, "Season Pass Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.SessionBrowserServerUrl = newValue, editedSdkConfig.SessionBrowserServerUrl, "Session BrowserServer Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.SessionServerUrl = newValue, editedSdkConfig.SessionServerUrl, "Session Server Url", false, generateServiceUrl, indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.MatchmakingV2ServerUrl = newValue, editedSdkConfig.MatchmakingV2ServerUrl, "MatchmakingV2 Server Url", false, generateServiceUrl, indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("TURN Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.UseTurnManager = newValue, editedSdkConfig.UseTurnManager, "Use TURN Manager", indentLevel: 1);
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnableAuthHandshake = newValue, editedSdkConfig.EnableAuthHandshake, "Use Secure Handshaking", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnServerHost = newValue, editedSdkConfig.TurnServerHost, "TURN Server Host", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnServerPort = newValue, editedSdkConfig.TurnServerPort, "TURN Server Port", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnManagerServerUrl = newValue, editedSdkConfig.TurnManagerServerUrl, "TURN Manager Server Url", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnServerUsername = newValue, editedSdkConfig.TurnServerUsername, "TURN Server Username", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnServerSecret = newValue, editedSdkConfig.TurnServerSecret, "TURN Server Secret", indentLevel: 1);
                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.TurnServerPassword = newValue, editedSdkConfig.TurnServerPassword, "TURN Server Password", indentLevel: 1);
                EditorCommon.CreateNumberInput((newvalue) => editedSdkConfig.PeerMonitorIntervalMs = (int)newvalue, editedSdkConfig.PeerMonitorIntervalMs, "Peer Monitor Interval in Milliseconds", indentLevel: 1);
                EditorCommon.CreateNumberInput((newvalue) => editedSdkConfig.PeerMonitorTimeoutMs = (int)newvalue, editedSdkConfig.PeerMonitorTimeoutMs, "Peer Monitor Timeout in Milliseconds", indentLevel: 1);
                EditorCommon.CreateNumberInput((newvalue) => editedSdkConfig.HostCheckTimeoutInSeconds = (int)newvalue, editedSdkConfig.HostCheckTimeoutInSeconds, "Host Check Timeout in Seconds", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Presence Broadcast Event Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnablePresenceBroadcastEvent = newValue, editedSdkConfig.EnablePresenceBroadcastEvent, "Enable Presence Broadcast Event", indentLevel: 1);
                EditorCommon.CreateNumberInput((newValue) => editedSdkConfig.PresenceBroadcastEventInterval = (int)newValue, editedSdkConfig.PresenceBroadcastEventInterval, "Set Interval In Seconds", indentLevel: 1);
                int minimumInternvalInSecond = Core.PresenceBroadcastEventScheduler.MiniumAllowedIntervalInlMs / 1000;
                if (editedSdkConfig.PresenceBroadcastEventInterval < minimumInternvalInSecond)
                {
                    editedSdkConfig.PresenceBroadcastEventInterval = minimumInternvalInSecond;
                }

                EditorGUILayout.BeginHorizontal();
                EditorCommon.AddIndent(depth: 1);
                EditorGUILayout.LabelField("Game State");
                EditorGUI.BeginChangeCheck();
                temporaryPresenceBroadcastEventGameStateSetting = EditorGUILayout.Popup(temporaryPresenceBroadcastEventGameStateSetting, presenceBroadcastEventGameStateList);
                if (EditorGUI.EndChangeCheck())
                {
                    int gameState = 0;
                    if (presenceBroadcastEventGameStateList[temporaryPresenceBroadcastEventGameStateSetting] !=
                        AccelByte.Utils.JsonUtils.SerializeWithStringEnum(
                        PresenceBroadcastEventGameState.OutOfGameplay))
                    {
                        gameState = temporaryPresenceBroadcastEventGameStateSetting;
                    }
                    editedSdkConfig.PresenceBroadcastEventGameState = gameState;
                }
                EditorGUILayout.LabelField("");
                EditorGUILayout.EndHorizontal();

                EditorCommon.CreateTextInput((newValue) => editedSdkConfig.PresenceBroadcastEventGameStateDescription = newValue, editedSdkConfig.PresenceBroadcastEventGameStateDescription, "Set Game State description", indentLevel: 1);
            }

            if (EditorCommon.CreateFoldout("Pre-Defined Event Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnablePreDefinedEvent = newValue, editedSdkConfig.EnablePreDefinedEvent, "Enable Pre-Defined Game Event", indentLevel: 1);
            }
            
            if (EditorCommon.CreateFoldout("Client Analytics Event Configs", foldoutConfigStatus))
            {
                EditorCommon.CreateToggleInput((newValue) => editedSdkConfig.EnableClientAnalyticsEvent = newValue, editedSdkConfig.EnableClientAnalyticsEvent, "Enable Client Analytics Event", indentLevel: 1);
                EditorCommon.CreateNumberInput((newValue) => editedSdkConfig.ClientAnalyticsEventInterval = newValue, editedSdkConfig.ClientAnalyticsEventInterval, "Set Interval In Seconds", indentLevel: 1);

                const float minimalInterval = Core.ClientAnalyticsEventScheduler.ClientAnalyticsMiniumAllowedIntervalInlMs / 1000f;
                if (editedSdkConfig.ClientAnalyticsEventInterval < minimalInterval)
                {
                    editedSdkConfig.ClientAnalyticsEventInterval = minimalInterval;
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                editedClientOAuthConfig.Expand();
                editedSdkConfig.SanitizeBaseUrl();
                editedSdkConfig.Expand(generateServiceUrl);

                SettingsEnvironment targetEnvironment = EditorCommon.GetEnvironment(EditorCommon.EnvironmentList, temporaryEnvironmentSetting);
                originalClientOAuthConfigs = AccelByteSettingsV2.SetOAuthByEnvironment(originalClientOAuthConfigs, editedClientOAuthConfig.ShallowCopy(), targetEnvironment);
                originalSdkConfigs = AccelByteSettingsV2.SetSDKConfigByEnvironment(originalSdkConfigs, editedSdkConfig.ShallowCopy(), targetEnvironment);
                originalAnalyticsOAuthConfigs = AccelByteSettingsV2.SetOAuthByEnvironment(originalAnalyticsOAuthConfigs, editedAnalyticsOAuthConfig.ShallowCopy(), targetEnvironment);

                string platformName = EditorCommon.GetPlatformName(EditorCommon.PlatformList, temporaryPlatformSetting);
                AccelByteSettingsV2.SaveConfig(originalClientOAuthConfigs, AccelByteSettingsV2.OAuthFullPath(platformName));
                AccelByteSettingsV2.SaveConfig(originalSdkConfigs, AccelByteSettingsV2.SDKConfigFullPath(false));
                AccelByteSettingsV2.SaveConfig(originalAnalyticsOAuthConfigs, AccelByteAnalyticsSettings.AnalyticsOAuthFullPath(platformName));

                Debug.Log("AccelByte Client Config Is Updated");
            }

            EditorGUILayout.EndVertical();
        }
    }
}
