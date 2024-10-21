// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.ThirdParties.Steam
{
    public class NullSteamImp : ISteamImp
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

        public AccelByteResult<Error> AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity)
        {
            var result = new AccelByteResult<Error>();
            result.Reject(new Error(ErrorCode.NotImplemented, "Steamworks not installed/enabled"));
            return result;
        }

        public AccelByteResult<GetSteamTokenResult, Error> GetAccelByteSteamToken()
        {
            return GetAccelByteSteamToken(serviceIdentity: SteamWrapperInfo.AccelByteServiceIdentity);
        }

        public AccelByteResult<GetSteamTokenResult, Error> GetAccelByteSteamToken(string serviceIdentity)
        {
            AccelByteResult<GetSteamTokenResult, Error> result = new AccelByteResult<GetSteamTokenResult, Error>();
            result.Reject(new Error(ErrorCode.NotImplemented, "AccelByte Steam Extension is not installed. Please contact AccelByte support."));
            return result;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}