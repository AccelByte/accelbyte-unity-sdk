// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal class VersionLogger
    {
        public string PrintedVersion
        {
            get;
            private set;
        }

        public VersionLogger()
        {
            PrintedVersion = null;
        }

        public void PrintVersion()
        {
            if (PrintedVersion == null)
            {
                string sdkVersionResourcePath = Api.AccelByteSettingsV2.SDKVersionResourcePath();
                var versionText = UnityEngine.Resources.Load<UnityEngine.TextAsset>(sdkVersionResourcePath);
                if (versionText != null && !string.IsNullOrEmpty(versionText.text))
                {
                    PrintedVersion = versionText.text;
                    UnityEngine.Debug.Log($"Installed AccelByte SDK versions:\n{versionText.text}");
                }
            }
        }
    }
}