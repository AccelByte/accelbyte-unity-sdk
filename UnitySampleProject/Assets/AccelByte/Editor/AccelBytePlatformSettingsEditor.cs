// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Api
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Api;

    [CustomEditor(typeof(AccelByteSettings))]
    public class AccelBytePlatformSettingsEditor : Editor
    {
        private bool isAdditionalInfoExpanded;
        private Config expandedConfig;

        private void OnEnable() { this.isAdditionalInfoExpanded = false; }

        [MenuItem("AccelByte/Edit Settings")]
        public static void Edit() { Selection.activeObject = AccelByteSettings.Instance; }

        public override void OnInspectorGUI()
        {
#if UNITY_EDITOR
            // test
            List<GUIHelper.Worker> workers = new List<GUIHelper.Worker>();
            this.expandedConfig = AccelByteSettings.Instance.CopyConfig();
            this.expandedConfig.Expand();

            GUIContent publisherNamespaceLabel = new GUIContent("Publisher Namespace");

            if (AccelByteSettings.PublisherNamespace != null)
            {
                AccelByteSettings.PublisherNamespace =
                    MakeTextBox(publisherNamespaceLabel, AccelByteSettings.PublisherNamespace);
            }

            GUIContent namespaceLabel = new GUIContent("Namespace");
            AccelByteSettings.Namespace = MakeTextBox(namespaceLabel, AccelByteSettings.Namespace);

            GUIContent baseUrlLabel = new GUIContent("Base Url");

            if (AccelByteSettings.BaseUrl != null)
            {
                AccelByteSettings.BaseUrl = MakeTextBox(baseUrlLabel, AccelByteSettings.BaseUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(baseUrlLabel, this.expandedConfig.BaseUrl);
                });
            }

            GUIContent iamServerUrlLabel = new GUIContent("IAM Server Url");

            if (AccelByteSettings.IamServerUrl != null)
            {
                AccelByteSettings.IamServerUrl = MakeTextBox(iamServerUrlLabel, AccelByteSettings.IamServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(iamServerUrlLabel, this.expandedConfig.IamServerUrl);
                });
            }

            GUIContent platformServerUrlLabel = new GUIContent("Platform Server Url");

            if (AccelByteSettings.PlatformServerUrl != null)
            {
                AccelByteSettings.PlatformServerUrl =
                    MakeTextBox(platformServerUrlLabel, AccelByteSettings.PlatformServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(platformServerUrlLabel, this.expandedConfig.PlatformServerUrl);
                });
            }

            GUIContent basicServerUrlLabel = new GUIContent("Basic Server Url");

            if (AccelByteSettings.PublisherNamespace != null)
            {
                AccelByteSettings.BasicServerUrl = MakeTextBox(basicServerUrlLabel, AccelByteSettings.BasicServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(basicServerUrlLabel, this.expandedConfig.BasicServerUrl);
                });
            }

            GUIContent lobbyServerUrlLabel = new GUIContent("Lobby Server Url");

            if (AccelByteSettings.LobbyServerUrl != null)
            {
                AccelByteSettings.LobbyServerUrl = MakeTextBox(lobbyServerUrlLabel, AccelByteSettings.LobbyServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(lobbyServerUrlLabel, this.expandedConfig.LobbyServerUrl);
                });
            }

            GUIContent cloudStorageServerUrlLabel = new GUIContent("Cloud Storage Server Url");

            if (AccelByteSettings.CloudStorageServerUrl != null)
            {
                AccelByteSettings.CloudStorageServerUrl = MakeTextBox(cloudStorageServerUrlLabel,
                    AccelByteSettings.CloudStorageServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(cloudStorageServerUrlLabel, this.expandedConfig.CloudStorageServerUrl);
                });
            }

            GUIContent telemetryServerUrlLabel = new GUIContent("Telemetry Server Url");

            if (AccelByteSettings.TelemetryServerUrl != null)
            {
                AccelByteSettings.TelemetryServerUrl =
                    MakeTextBox(telemetryServerUrlLabel, AccelByteSettings.TelemetryServerUrl);
            }
            else
            {
                workers.Add(() =>
                {
                    MakeSelectableLabel(telemetryServerUrlLabel, this.expandedConfig.TelemetryServerUrl);
                });
            }

            GUIContent clientIdLabel = new GUIContent("Client Id");
            AccelByteSettings.ClientId = MakeTextBox(clientIdLabel, AccelByteSettings.ClientId);

            GUIContent clientSecretLabel = new GUIContent("Client Secret");
            AccelByteSettings.ClientSecret = MakePasswordBox(clientSecretLabel, AccelByteSettings.ClientSecret);

            GUIContent redirectUriLabel = new GUIContent("Redirect Uri");
            AccelByteSettings.RedirectUri = MakeTextBox(redirectUriLabel, AccelByteSettings.RedirectUri);

            this.isAdditionalInfoExpanded = EditorGUILayout.Foldout(this.isAdditionalInfoExpanded, "Additional Info");

            if (this.isAdditionalInfoExpanded)
            {
                foreach (var worker in workers)
                {
                    worker();
                }
            }

            if (GUILayout.Button("Save"))
            {
                AccelByteSettings.Instance.Save();
            }

            if (GUILayout.Button("Reload"))
            {
                AccelByteSettings.Instance.Load();
            }
#endif
        }

        private string MakeTextBox(GUIContent label, string variable)
        {
            return GUIHelper.MakeControlWithLabel(label, () =>
            {
                GUI.changed = false;
                var result = EditorGUILayout.TextField(variable);
                SetDirtyOnGUIChange();

                return result;
            });
        }

        private string MakePasswordBox(GUIContent label, string variable)
        {
            return GUIHelper.MakeControlWithLabel(label, () =>
            {
                GUI.changed = false;
                var result = EditorGUILayout.PasswordField(variable);
                SetDirtyOnGUIChange();

                return result;
            });
        }

        private bool MakeToggle(GUIContent label, bool variable)
        {
            return GUIHelper.MakeControlWithLabel(label, () =>
            {
                GUI.changed = false;
                var result = EditorGUILayout.Toggle(variable);
                SetDirtyOnGUIChange();

                return result;
            });
        }

        private string MakeSelectableLabel(GUIContent label, string variable)
        {
            return GUIHelper.MakeControlWithLabel(label, () =>
            {
                GUI.changed = false;
                EditorGUILayout.SelectableLabel(variable);
                SetDirtyOnGUIChange();

                return variable;
            });
        }

        private void SetDirtyOnGUIChange()
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(AccelByteSettings.Instance);
                GUI.changed = false;
            }
        }
    }
}