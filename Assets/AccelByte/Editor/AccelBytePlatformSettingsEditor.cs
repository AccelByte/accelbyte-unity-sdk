// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Models;

namespace AccelByte.Api
{
    using UnityEditor;
    using UnityEngine;

    public class AccelBytePlatformSettingsEditor : EditorWindow
    {
        private static AccelBytePlatformSettingsEditor _instance;
        public static AccelBytePlatformSettingsEditor Instance { get { return _instance; } }
        public Texture2D AccelByteLogo;
        public Config TemporarySetting;
        public Rect LogoRect;
        public LogType SelectedLogFilter;

        [MenuItem("AccelByte/Edit Settings")]
        public static void Edit()
        {
            // Get existing open window or if none, make a new one:
            if (_instance != null)
            {
                _instance.CloseFinal();
            }
            
            _instance = (AccelBytePlatformSettingsEditor)EditorWindow.GetWindow(typeof(AccelBytePlatformSettingsEditor));
            _instance.Initialize();
            _instance.Show();
        }

        public void Initialize()
        {
            titleContent = new GUIContent("AccelByte Configuration");
            AccelByteLogo = Resources.Load<Texture2D>("ab-logo");
            this.TemporarySetting = AccelByteSettings.Instance.CopyConfig();
            LogoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);
            if( !Enum.TryParse( this.TemporarySetting.DebugLogFilter, out SelectedLogFilter ) )
            {
                SelectedLogFilter = LogType.Log;
            }
        }

        public void CloseFinal()
        {
            Close();
            _instance = null;
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            LogoRect.x = (this.position.width - 300) / 2;
            GUI.DrawTexture(LogoRect, AccelByteLogo);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(100);

            if(AccelByteSettings.Instance.CompareConfig(TemporarySetting))
            {
                EditorGUILayout.HelpBox("All configs has been saved!", MessageType.Info, true);
            }
            else
            {
                EditorGUILayout.HelpBox("Unsaved changes", MessageType.Warning, true);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Namespace");
            TemporarySetting.Namespace =  EditorGUILayout.TextField(TemporarySetting.Namespace);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Publisher Namespace");
            TemporarySetting.PublisherNamespace = EditorGUILayout.TextField(TemporarySetting.PublisherNamespace);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use Session Management");
            TemporarySetting.UseSessionManagement = EditorGUILayout.Toggle(TemporarySetting.UseSessionManagement);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use PlayerPrefs");
            TemporarySetting.UsePlayerPrefs = EditorGUILayout.Toggle(TemporarySetting.UsePlayerPrefs);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enable Debug Log");
            TemporarySetting.EnableDebugLog = EditorGUILayout.Toggle(TemporarySetting.EnableDebugLog);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "Log Type Filter" );
            SelectedLogFilter = (LogType) EditorGUILayout.EnumPopup( SelectedLogFilter );
            TemporarySetting.DebugLogFilter = SelectedLogFilter.ToString();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Url");
            TemporarySetting.BaseUrl = EditorGUILayout.TextField(TemporarySetting.BaseUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Api Base Url");
            TemporarySetting.ApiBaseUrl = EditorGUILayout.TextField(TemporarySetting.ApiBaseUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Non Api Base Url");
            TemporarySetting.NonApiBaseUrl = EditorGUILayout.TextField(TemporarySetting.NonApiBaseUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Login Server Url");
            TemporarySetting.LoginServerUrl = EditorGUILayout.TextField(TemporarySetting.LoginServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("IAM Server Url");
            TemporarySetting.IamServerUrl = EditorGUILayout.TextField(TemporarySetting.IamServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform Server Url");
            TemporarySetting.PlatformServerUrl = EditorGUILayout.TextField(TemporarySetting.PlatformServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Basic Server Url");
            TemporarySetting.BasicServerUrl = EditorGUILayout.TextField(TemporarySetting.BasicServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Lobby Server Url");
            TemporarySetting.LobbyServerUrl = EditorGUILayout.TextField(TemporarySetting.LobbyServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Cloud Storage Server Url");
            TemporarySetting.CloudStorageServerUrl = EditorGUILayout.TextField(TemporarySetting.CloudStorageServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Game Profile Server Url");
            TemporarySetting.GameProfileServerUrl = EditorGUILayout.TextField(TemporarySetting.GameProfileServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Statistic Server Url");
            TemporarySetting.StatisticServerUrl = EditorGUILayout.TextField(TemporarySetting.StatisticServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Achievement Server Url");
            TemporarySetting.AchievementServerUrl = EditorGUILayout.TextField(TemporarySetting.AchievementServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("CloudSave Server Url");
            TemporarySetting.CloudSaveServerUrl = EditorGUILayout.TextField(TemporarySetting.CloudSaveServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Agreement Server Url");
            TemporarySetting.AgreementServerUrl = EditorGUILayout.TextField(TemporarySetting.AgreementServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Leaderboard Server Url");
            TemporarySetting.LeaderboardServerUrl = EditorGUILayout.TextField(TemporarySetting.LeaderboardServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Game Telemetry Server Url");
            TemporarySetting.GameTelemetryServerUrl = EditorGUILayout.TextField(TemporarySetting.GameTelemetryServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Group Server Url");
            TemporarySetting.GroupServerUrl = EditorGUILayout.TextField(TemporarySetting.GroupServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Client Id");
            TemporarySetting.ClientId = EditorGUILayout.TextField(TemporarySetting.ClientId);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Client Secret");
            TemporarySetting.ClientSecret = EditorGUILayout.PasswordField(TemporarySetting.ClientSecret);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Redirect Uri");
            TemporarySetting.RedirectUri = EditorGUILayout.TextField(TemporarySetting.RedirectUri);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("App Id");
            TemporarySetting.AppId = EditorGUILayout.TextField(TemporarySetting.AppId);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {                
                AccelByteSettings.Instance.UpdateConfig(TemporarySetting.ShallowCopy());            
                AccelByteSettings.Instance.Save();
            }

            EditorGUILayout.EndVertical();
#endif
        }
        
    }
}
