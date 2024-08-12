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


        Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken();
        Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken(string serviceIdentity);
        Models.AccelByteResult<Core.Error> AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity);
    }
}