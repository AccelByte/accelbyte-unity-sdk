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
            return new DeviceProvider(
                "device",
                "unity:" + SystemInfo.deviceType + ":" + SystemInfo.deviceUniqueIdentifier);
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