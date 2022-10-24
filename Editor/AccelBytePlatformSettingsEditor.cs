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
        private static AccelBytePlatformSettingsEditor _instance;
        public static AccelBytePlatformSettingsEditor Instance { get { return _instance; } }
        public Texture2D AccelByteLogo;
        public OAuthConfig TemporaryOAuth;
        public Config TemporarySetting;
        public int TemporaryEnvironmentSetting;
        public int TemporaryPlatformSetting;
        public string TemporarySDKVersion;
        public List<string> platformList;
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
            Debug.LogWarning(Application.platform.ToString());
            titleContent = new GUIContent("AccelByte Configuration");
            AccelByteLogo = Resources.Load<Texture2D>("ab-logo");
            this.TemporaryOAuth = AccelByteSettings.Instance.CopyOAuthConfig();
            this.TemporarySetting = AccelByteSettings.Instance.CopyConfig();
            this.TemporaryEnvironmentSetting = (int)AccelByteSettings.Instance.GetEditedEnvironment();
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
            this.TemporaryPlatformSetting = platformList.Count - 1;
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

            if (EditorApplication.isPlaying)
            {
                CloseFinal();
                return;
            }

            if (AccelByteSettings.Instance.CompareOAuthConfig(TemporaryOAuth) && AccelByteSettings.Instance.CompareConfig(TemporarySetting))
            {
                EditorGUILayout.HelpBox("All configs has been saved!", MessageType.Info, true);
            }
            else
            {
                EditorGUILayout.HelpBox("Unsaved changes", MessageType.Warning, true);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SDK Version");
            EditorGUILayout.LabelField(AccelByteSettings.Instance.AccelByteSDKVersion);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Environment");
            EditorGUI.BeginChangeCheck();
            TemporaryEnvironmentSetting = EditorGUILayout.Popup(TemporaryEnvironmentSetting, new string[] { "Development", "Certification", "Production", "Default" });
            if (EditorGUI.EndChangeCheck())
            {
                AccelByteSettings.Instance.SetEditedEnvironment((SettingsEnvironment)TemporaryEnvironmentSetting);
                TemporaryOAuth = AccelByteSettings.Instance.CopyOAuthConfig();
                TemporarySetting = AccelByteSettings.Instance.CopyConfig();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform");
            
            EditorGUI.BeginChangeCheck();
            TemporaryPlatformSetting = EditorGUILayout.Popup(TemporaryPlatformSetting, platformList.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                if(platformList[TemporaryPlatformSetting] == "Default")
                {
                    AccelByteSettings.Instance.SetEditedPlatform();
                }
                else
                {
                    AccelByteSettings.Instance.SetEditedPlatform(platformList[TemporaryPlatformSetting]);
                }
                TemporaryOAuth = AccelByteSettings.Instance.CopyOAuthConfig();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Client Id");
            TemporaryOAuth.ClientId = EditorGUILayout.TextField(TemporaryOAuth.ClientId);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Client Secret");
            TemporaryOAuth.ClientSecret = EditorGUILayout.PasswordField(TemporaryOAuth.ClientSecret);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Namespace");
            TemporarySetting.Namespace =  EditorGUILayout.TextField(TemporarySetting.Namespace);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Publisher Namespace");
            TemporarySetting.PublisherNamespace = EditorGUILayout.TextField(TemporarySetting.PublisherNamespace);
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
            EditorGUILayout.LabelField("Season Pass Server Url");
            TemporarySetting.SeasonPassServerUrl = EditorGUILayout.TextField(TemporarySetting.SeasonPassServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use TURN Manager");
            TemporarySetting.UseTurnManager = EditorGUILayout.Toggle(TemporarySetting.UseTurnManager);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Server Host");
            TemporarySetting.TurnServerHost = EditorGUILayout.TextField(TemporarySetting.TurnServerHost);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Server Port");
            TemporarySetting.TurnServerPort = EditorGUILayout.TextField(TemporarySetting.TurnServerPort);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Manager Server Url");
            TemporarySetting.TurnManagerServerUrl = EditorGUILayout.TextField(TemporarySetting.TurnManagerServerUrl);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Server Username");
            TemporarySetting.TurnServerUsername = EditorGUILayout.TextField(TemporarySetting.TurnServerUsername);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Server Secret");
            TemporarySetting.TurnServerSecret = EditorGUILayout.TextField(TemporarySetting.TurnServerSecret);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TURN Server Password");
            TemporarySetting.TurnServerPassword = EditorGUILayout.TextField(TemporarySetting.TurnServerPassword);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Session BrowserServer Url");
            TemporarySetting.SessionBrowserServerUrl = EditorGUILayout.TextField(TemporarySetting.SessionBrowserServerUrl);
            EditorGUILayout.EndHorizontal();            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Session Server Url");
            TemporarySetting.SessionServerUrl = EditorGUILayout.TextField(TemporarySetting.SessionServerUrl);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MatchmakingV2 Server Url");
            TemporarySetting.MatchmakingV2ServerUrl = EditorGUILayout.TextField(TemporarySetting.MatchmakingV2ServerUrl);
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
                AccelByteSettings.Instance.UpdateOAuthConfig(TemporaryOAuth.ShallowCopy());
                AccelByteSettings.Instance.UpdateConfig(TemporarySetting.ShallowCopy());
                AccelByteSettings.Instance.Save();
            }

            EditorGUILayout.EndVertical();
#endif
        }
        
    }
}
