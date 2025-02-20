// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

#if UNITY_2021_3_OR_NEWER
using UnityEngine.Device;
#else
using UnityEngine;
#endif

namespace AccelByte.Core
{
    public static class GameCommonInfo
    {
        public static string ProductName = Application.productName;
        public static string PlatformName = Application.platform.ToString();
        public static string DeviceType = SystemInfo.deviceType.ToString();
#if !UNITY_SWITCH || UNITY_EDITOR
        private static string unityPersistentPath = Application.persistentDataPath;

        public static string PersistentPath
        {
            get
            {
                string persistentPath = unityPersistentPath;
                if(string.IsNullOrEmpty(persistentPath))
                {
                    persistentPath = ".";
                }

                return persistentPath;
            }
        }
#endif
    }
}