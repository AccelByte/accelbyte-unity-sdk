// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AccelByte.Editor
{
    internal static class EditorCommon
    {
        private static GUIStyle requiredTextFieldGUIStyle;
        private static Texture2D accelByteLogo;

        private static string[] environmentList;
        internal static string[] EnvironmentList
        {
            get
            {
                if (environmentList == null)
                {
                    List<string> retval = Enum.GetValues(typeof(SettingsEnvironment))
                        .Cast<SettingsEnvironment>()
                        .Where(envEnum => envEnum != SettingsEnvironment.Default)
                        .Select(envEnum => envEnum.ToString())
                        .ToList();
                    retval.Insert(index: 0, SettingsEnvironment.Default.ToString());
                    environmentList = retval.ToArray();
                }
                return environmentList;
            }
        }

        internal static string[] PlatformList = new string[]
        {
            "Default",
            AccelByte.Models.PlatformType.Steam.ToString(),
            AccelByte.Models.PlatformType.Apple.ToString(),
            AccelByte.Models.PlatformType.iOS.ToString(),
            AccelByte.Models.PlatformType.Android.ToString(),
            AccelByte.Models.PlatformType.PS4.ToString(),
            AccelByte.Models.PlatformType.PS5.ToString(),
            AccelByte.Models.PlatformType.Live.ToString(),
            AccelByte.Models.PlatformType.Nintendo.ToString()
        };

        internal static GUIStyle RequiredTextFieldGUIStyle
        {
            get
            {
                if(requiredTextFieldGUIStyle == null)
                {
                    requiredTextFieldGUIStyle = new GUIStyle();
                    requiredTextFieldGUIStyle.normal.textColor = Color.yellow;
                }
                return requiredTextFieldGUIStyle;
            }
        }

        internal static Texture2D AccelByteLogo
        {
            get
            {
                if (accelByteLogo == null)
                {
                    accelByteLogo = Resources.Load<Texture2D>("ab-logo");
                }
                return accelByteLogo;
            }
        }

        internal static void CreateNumberInput(Action<double> setter, double defaultValue, string fieldLabel, bool required = false, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.DoubleField(defaultValue);
            setter?.Invoke(newValue);

            string requiredText = "";
            if (required)
            {
                requiredText = "Required";
            }
            EditorGUILayout.LabelField(requiredText, EditorCommon.RequiredTextFieldGUIStyle);

            EditorGUILayout.EndHorizontal();
        }

        internal static void CreateNumberInput(Action<float> setter, float defaultValue, string fieldLabel, bool required = false, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.FloatField(defaultValue);
            setter?.Invoke(newValue);

            string requiredText = "";
            if (required)
            {
                requiredText = "Required";
            }
            EditorGUILayout.LabelField(requiredText, EditorCommon.RequiredTextFieldGUIStyle);

            EditorGUILayout.EndHorizontal();
        }

        internal static void CreateNumberInput(Action<int> setter, int defaultValue, string fieldLabel, bool required = false, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.IntField(defaultValue);
            setter?.Invoke(newValue);

            string requiredText = "";
            if (required)
            {
                requiredText = "Required";
            }
            EditorGUILayout.LabelField(requiredText, EditorCommon.RequiredTextFieldGUIStyle);

            EditorGUILayout.EndHorizontal();
        }

        internal static void CreateTextInput(Action<string> setter, string defaultValue, string fieldLabel, bool required = false, bool @readonly = false, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            EditorGUILayout.LabelField(fieldLabel);

            if (!@readonly)
            {
                var newValue = EditorGUILayout.TextField(defaultValue);
                setter?.Invoke(newValue);

                string requiredText = "";
                if (required && string.IsNullOrEmpty(newValue))
                {
                    requiredText = "Required";
                }
                EditorGUILayout.LabelField(requiredText, EditorCommon.RequiredTextFieldGUIStyle);
            }
            else
            {
                EditorGUILayout.LabelField(defaultValue);
                EditorGUILayout.LabelField(string.Empty, EditorCommon.RequiredTextFieldGUIStyle);
            }

            EditorGUILayout.EndHorizontal();
        }

        internal static void CreateToggleInput(Action<bool> setter, bool defaultValue, string fieldLabel, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            EditorGUILayout.LabelField(fieldLabel);
            var newValue = EditorGUILayout.Toggle(defaultValue);
            setter?.Invoke(newValue);

            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();
        }

        internal static bool CreateFoldout(string label, Dictionary<string, bool> foldoutStatus, int indentLevel = 0)
        {
            bool previousStatus = false;
            if (foldoutStatus.ContainsKey(label))
            {
                previousStatus = foldoutStatus[label];
            }

            EditorGUILayout.BeginHorizontal();
            AddIndent(indentLevel);
            foldoutStatus[label] = EditorGUILayout.Foldout(previousStatus, content: label);
            EditorGUILayout.EndHorizontal();

            return foldoutStatus[label];
        }

        internal static string GetPlatformName(string[] platformList, int index)
        {
            string targetPlatform = "";
            if (platformList[index] != "Default")
            {
                targetPlatform = platformList[index];
            }
            return targetPlatform;
        }

        internal static SettingsEnvironment GetEnvironment(string[] environmentList, int index)
        {
            SettingsEnvironment retval = SettingsEnvironment.Default;
            try
            {
                string envString = environmentList[index];
                if(Enum.TryParse(envString, out SettingsEnvironment parsedEnum))
                {
                    retval = parsedEnum;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return retval;
        }

        internal static bool CompareConfig<T>(T firstConfig, T secondConfig) where T : class
        {
            if(firstConfig == null && secondConfig == null)
            {
                return true;
            }
            else if (firstConfig == null && secondConfig != null)
            {
                return false;
            }
            else if (firstConfig != null && secondConfig == null)
            {
                return false;
            }

            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach(FieldInfo field in fields)
            {
                object firstConfigValue = field.GetValue(firstConfig);
                object secondConfigValue = field.GetValue(secondConfig);
                if (firstConfigValue != null && secondConfigValue == null)
                {
                    return false;
                }
                else if(firstConfigValue == null && secondConfigValue != null)
                {
                    return false;
                }
                else if(firstConfigValue != null && secondConfigValue != null)
                {
                    if (!firstConfigValue.Equals(secondConfigValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal static void AddIndent(int depth)
        {
            int targetDepth = 0;
            if(depth > 0)
            {
                targetDepth = depth;
            }
            GUILayout.Space(targetDepth * 15 + 4);
        }
    }
}
