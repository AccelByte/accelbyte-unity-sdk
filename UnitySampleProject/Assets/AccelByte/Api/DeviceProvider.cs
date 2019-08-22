// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class DeviceProvider
    {
        public static DeviceProvider GetFromSystemInfo()
        {
            string identifier = "unity:" + SystemInfo.deviceType + ":" + Application.platform;
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!PlayerPrefs.HasKey("AccelByteDeviceUniqueId")){
                PlayerPrefs.SetString("AccelByteDeviceUniqueId", System.Guid.NewGuid().ToString());
            }
            return new DeviceProvider(
                "device",
                identifier + ":" + PlayerPrefs.GetString("AccelByteDeviceUniqueId")); 
#else
            return new DeviceProvider(
                "device",
                identifier + ":" + SystemInfo.deviceUniqueIdentifier);
#endif
        }

        public readonly string DeviceId;
        public readonly string DeviceType;

        private DeviceProvider(string deviceType, string deviceId)
        {
            Assert.IsNotNull(deviceType, "DeviceType is null!");
            Assert.IsNotNull(deviceId, "DeviceId is null!");
            this.DeviceType = deviceType;
            this.DeviceId = deviceId;
        }
    }
}