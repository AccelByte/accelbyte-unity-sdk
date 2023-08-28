// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using UnityEngine;

namespace AccelByte.Utils.Infoware
{
    public class PlayStation5Infoware : InfowareUtils
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

        public virtual void GetAccelBytePS5Token(string ps5ClientId, System.Action<string> onGetPlatformTokenFinished)
        {
#if !UNITY_EDITOR && UNITY_PS5
            throw new NotSupportedException("AccelByte PS5 Extension is not installed. Please contact AccelByte support.");
#else
            throw new NotSupportedException("PS5 token generator only available in PS5 package.");
#endif
        }
    }
}