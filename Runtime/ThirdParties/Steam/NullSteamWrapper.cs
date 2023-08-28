// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AccelByte.ThirdParties.Steam
{
    public class NullSteamWrapper : ISteamWrapper
    {
        public bool Initialized
        {
            get
            {
                return false;
            }
        }
#pragma warning disable IDE0060 // Remove unused parameter
        public void CancelAllSteamTicket()
        {

        }

        public void GetAccelByteSteamToken(System.Action<GetSteamTokenResult> onGetPlatformTokenFinished)
        {
            GetAccelByteSteamToken(null, onGetPlatformTokenFinished);
        }

        public void GetAccelByteSteamToken(string serviceIdentity, System.Action<GetSteamTokenResult> onGetPlatformTokenFinished)
        {
            throw new System.NotSupportedException("AccelByte Steam Extension is not installed. Please contact AccelByte support.");
        }

        public void AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity, System.Action onSuccess = null, System.Action<string> onFailed = null)
        {
            onFailed?.Invoke("Steamworks not installed/enabled");
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}