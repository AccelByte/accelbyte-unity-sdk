// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
using System;

namespace AccelByte.ThirdParties.Steam
{
    public interface ISteamWrapper
    {
        bool Initialized
        {
            get;
        }

        void CancelAllSteamTicket();

        [Obsolete("This API will be removed on July release, please use GetAccelByteSteamToken() instead")]
        void GetAccelByteSteamToken(System.Action<GetSteamTokenResult> onGetPlatformTokenFinished);

        Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken();

        [Obsolete("This API will be removed on July release, please use GetAccelByteSteamToken(serviceIdentity) instead")]
        void GetAccelByteSteamToken(string serviceIdentity, System.Action<GetSteamTokenResult> onGetPlatformTokenFinished);

        Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken(string serviceIdentity);

        [Obsolete("This API will be removed on July release, please use AuthenticateSteamTicket(appid, steamTicket, webkey, identity) instead")]
        void AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity, System.Action onSuccess = null, System.Action<string> onFailed = null);

        Models.AccelByteResult<Core.Error> AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity);
    }
}