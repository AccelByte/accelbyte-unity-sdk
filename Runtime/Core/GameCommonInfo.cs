// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine.Device;

namespace AccelByte.Core
{
    public static class GameCommonInfo
    {
        public static string ProductName = Application.productName;
        public static string PlatformName = Application.platform.ToString();
        public static string DeviceType = SystemInfo.deviceType.ToString();
#if !UNITY_SWITCH || UNITY_EDITOR
        public static string PersistenPath = $"{Application.persistentDataPath}";
#endif
    }
}