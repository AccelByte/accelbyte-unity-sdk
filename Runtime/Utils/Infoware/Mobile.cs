// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    internal class Android : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            string uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(uniqueIdentifier))
            {
                uniqueIdentifier = Guid.NewGuid().ToString();
            }

            return uniqueIdentifier;
        }

        internal override string GetMacAddress()
        {
            return DeviceProvider.GetDeviceMacAddress();
        }
    }

    internal class IOS : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            string uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(uniqueIdentifier))
            {
                uniqueIdentifier = Guid.NewGuid().ToString();
            }

            return uniqueIdentifier;
        }

        internal override string GetMacAddress()
        {
            return DeviceProvider.GetDeviceMacAddress();
        }
    }
}