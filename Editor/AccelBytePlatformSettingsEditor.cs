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
        private Texture2D accelByteLogo;
        private int temporaryEnvironmentSetting;
        private int temporaryPlatformSetting;
        private List<string> platformList;
        private Rect logoRect;

        private MultiOAuthConfigs originalOAuthConfigs;
        private MultiConfigs originalSdkConfigs;
        private OAuthConfig editedOAuthConfig;
        private Config editedSdkConfig;
        private Vector2 scrollPos;
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
            
            instance = (AccelBytePlatformSettingsEditor)EditorWindow.GetWindow(typeof(AccelBytePlatformSettingsEditor));
            instance.Show();
        }

        private void Initialize()
        {
            if (!initialized)
            {
                requiredTextFieldGUIStyle = new GUIStyle();
                requiredTextFieldGUIStyle.normal.textColor = Color.yellow;

                titleContent = new GUIContent("AccelByte Configuration");
                accelByteLogo = Resources.Load<Texture2D>("ab-logo");
                platformList = new List<string>();
                platformList.Add(PlatformType.Steam.ToString());
                platformList.Add(PlatformType.EpicGames.ToString());
                platformList.Add(PlatformType.Apple.ToString());
                platformList.Add(PlatformType.iOS.ToString());
                platformList.Add(PlatformType.Android.ToString());
                platformList.Add(PlatformType.PS4.ToString());
                platformList.Add(PlatformType.PS5.ToString());
                platformList.Add(PlatformType.Live.ToString());
                platformList.Add(PlatformType.Nintendo.ToString());
                platformList.Add(PlatformType.Stadia.ToString());
                platformList.Add("Default");
                this.temporaryPlatformSetting = platformList.Count - 1;

                logoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);
                initialized = true;
            }

            if(originalSdkConfigs == null)
            {
                originalSdkConfigs = AccelByteSettingsV2.LoadSDKConfigFile();
            }
            if (originalOAuthConfigs == null)
            {
                originalOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(GetPlatformName(platformList, temporaryPlatformSetting));
            }
            if (editedSdkConfig == null)
            {
                editedSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, (SettingsEnvironment)temporaryEnvironmentSetting).ShallowCopy();
            }
            if (editedOAuthConfig == null)
            {
                editedOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting).ShallowCopy();
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

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SDK Version");
            EditorGUILayout.LabelField(AccelByteSettingsV2.AccelByteSDKVersion);
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Environment");
            EditorGUI.BeginChangeCheck();
            temporaryEnvironmentSetting = EditorGUILayout.Popup(temporaryEnvironmentSetting, new string[] { "Development", "Certification", "Production", "Default" });
            if (EditorGUI.EndChangeCheck())
            {
                editedSdkConfig = AccelByteSettingsV2.GetSDKConfigByEnvironment(originalSdkConfigs, (SettingsEnvironment)temporaryEnvironmentSetting).ShallowCopy();
                editedOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting).ShallowCopy();
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform");
            EditorGUI.BeginChangeCheck();
            temporaryPlatformSetting = EditorGUILayout.Popup(temporaryPlatformSetting, platformList.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                string targetPlatform = "";
                if (platformList[temporaryPlatformSetting] != "Default")
                {
                    targetPlatform = platformList[temporaryPlatformSetting];
                }
                originalOAuthConfigs = AccelByteSettingsV2.LoadOAuthFile(targetPlatform);
                editedOAuthConfig = AccelByteSettingsV2.GetOAuthByEnvironment(originalOAuthConfigs, (SettingsEnvironment)temporaryEnvironmentSetting);
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();

            CreateTextInput((newValue) => editedSdkConfig.AppId = newValue, editedSdkConfig.AppId, "App Id", true);
            CreateTextInput((newValue) => editedSdkConfig.BaseUrl = newValue, editedSdkConfig.BaseUrl, "Base Url", true);
            CreateTextInput((newValue) => editedSdkConfig.RedirectUri = newValue, editedSdkConfig.RedirectUri, "Redirect Uri", true);
            CreateTextInput((newValue) => editedSdkConfig.Namespace = newValue, editedSdkConfig.Namespace, "Namespace", true);
            CreateTextInput((newValue) => editedSdkConfig.PublisherNamespace = newValue, editedSdkConfig.PublisherNamespace, "Publisher Namespace", true);
            CreateTextInput((newValue) => editedOAuthConfig.ClientId = newValue, editedOAuthConfig.ClientId, "Client Id", true);
            CreateTextInput((newValue) => editedOAuthConfig.ClientSecret = newValue, editedOAuthConfig.ClientSecret, "Client Secret", true);

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
                CreateTextInput((newValue) => editedSdkConfig.TurnServerHost = newValue, editedSdkConfig.TurnServerHost, "TURN Server Host");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerPort = newValue, editedSdkConfig.TurnServerPort, "TURN Server Port");
                CreateTextInput((newValue) => editedSdkConfig.TurnManagerServerUrl = newValue, editedSdkConfig.TurnManagerServerUrl, "TURN Manager Server Url");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerUsername = newValue, editedSdkConfig.TurnServerUsername, "TURN Server Username");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerSecret = newValue, editedSdkConfig.TurnServerSecret, "TURN Server Secret");
                CreateTextInput((newValue) => editedSdkConfig.TurnServerPassword = newValue, editedSdkConfig.TurnServerPassword, "TURN Server Password");
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

        private string GetPlatformName(List<string> platformList, int index)
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
