// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    public class PlayStation4Infoware : InfowareUtils
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
            logger?.LogWarning("Incomplete implementation for PlayStation 4 platform");
            return "";
        }

        public virtual void GetAccelBytePS4Token(string ps4ClientId, System.Action<string> onGetPlatformTokenFinished)
        {
#if !UNITY_EDITOR && UNITY_PS4
            throw new NotSupportedException("AccelByte PS4 Extension is not installed. Please contact AccelByte support.");
#else
            throw new NotSupportedException("PS4 token generator only available in PS4 package.");
#endif
        }
    }
}