// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    internal abstract class InfowareUtils
    {
        internal abstract string GetDeviceID();
        internal abstract string GetMacAddress();
    }

    class OtherOs : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            string uniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(uniqueIdentifier))
            {
                uniqueIdentifier = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(uniqueIdentifier))
                {
                    uniqueIdentifier = "OtherOS";
                }
            }

            return uniqueIdentifier;
        }

        internal override string GetMacAddress()
        {
            return DeviceProvider.GetDeviceMacAddress();
        }
    }
}
