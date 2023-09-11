// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System;
using System.IO;
using UnityEditor;

namespace AccelByte.Core
{
    public static class UpdateSDKVersionEditorBootstrap
    {
        public static Action<Exception> OnVersionCopyState;

        public static void Execute(bool isForced = false)
        {
            if (isForced)
            {
#if UNITY_EDITOR
                SessionState.SetBool("IsAccelByteVersionUpdated", false);
#endif
            }
            UpdateVersionFile();
        }

        internal static void UpdateVersionFile()
        {
#if UNITY_EDITOR
            try
            {
                if (SessionState.GetBool("IsAccelByteVersionUpdated", false))
                {
                    OnVersionCopyState?.Invoke(null);
                    return;
                }

                var accelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettingsV2).Assembly);
                var versionJsonAbs = Path.Combine(accelBytePackageInfo.assetPath, "version.json");

                var sdkVersionPath = AccelByteSettingsV2.SDKVersionFullPath();

                UpdateVersionFile(versionJsonAbs, sdkVersionPath);
                AssetDatabase.Refresh();

                SessionState.SetBool("IsAccelByteVersionUpdated", true);

                OnVersionCopyState?.Invoke(null);
            }
            catch (Exception exception)
            {
                AccelByteDebug.LogException(exception);
                OnVersionCopyState?.Invoke(exception);
            }
#else
            OnVersionCopyState?.Invoke(null);
#endif
        }

        internal static void UpdateVersionFile(string sourcePath, string destinationPath)
        {
            string fileData;

            using (var reader = new StreamReader(sourcePath))
            {
                fileData = reader.ReadToEnd();
            }

            using (var writer = new StreamWriter(destinationPath))
            {
                writer.Write(fileData);
            }
        }
    }
}

