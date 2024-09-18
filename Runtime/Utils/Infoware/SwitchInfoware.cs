// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    public class SwitchInfoware : InfowareUtils
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
            logger?.LogWarning("Incomplete implementation for Switch platform");
            return "";
        }

        public virtual void GetAccelByteToken(System.Action<string> onGetPlatformTokenFinished)
        {
            throw new NotSupportedException("AccelByte Switch Extension is not installed. Please contact AccelByte support.");
        }
    }
}