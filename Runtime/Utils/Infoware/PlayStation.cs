// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    internal class PlayStation4 : InfowareUtils
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
            AccelByteDebug.LogWarning("Incomplete implementation for PlayStation 4 platform");
            return "";
        }
    }

    internal class PlayStation5 : InfowareUtils
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
            AccelByteDebug.LogWarning("Incomplete implementation for PlayStation 5 platform");
            return "";
        }
    }
}