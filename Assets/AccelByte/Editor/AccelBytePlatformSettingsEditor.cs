// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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

        [MenuItem("AccelByte/Edit Settings")]
        public static void Edit()
        {
            if (_instance == null)
            {
                // Get existing open window or if none, make a new one:
                _instance = (AccelBytePlatformSettingsEditor)EditorWindow.GetWindow(typeof(AccelBytePlatformSettingsEditor));
                _instance.Initialize();
                _instance.Show();
            }
            else
            {
                _instance.CloseFinal();
            }
        }

        public void Initialize()
        {
            titleContent = new GUIContent("AccelByte Configuration");
            AccelByteLogo = Resources.Load<Texture2D>("ab-logo");
            this.TemporarySetting = AccelByteSettings.Instance.CopyConfig();
            LogoRect = new Rect((this.position.width - 300) / 2, 10, 300, 86);
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
            EditorGUILayout.LabelField("Use Session Management");
            TemporarySetting.UseSessionManagement = EditorGUILayout.Toggle(TemporarySetting.UseSessionManagement);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Base Url");
            TemporarySetting.BaseUrl = EditorGUILayout.TextField(TemporarySetting.BaseUrl);
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
            EditorGUILayout.LabelField("Telemetry Server Url");
            TemporarySetting.TelemetryServerUrl = EditorGUILayout.TextField(TemporarySetting.TelemetryServerUrl);
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
            EditorGUILayout.LabelField("CloudSave Server Url");
            TemporarySetting.CloudSaveServerUrl = EditorGUILayout.TextField(TemporarySetting.CloudSaveServerUrl);
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