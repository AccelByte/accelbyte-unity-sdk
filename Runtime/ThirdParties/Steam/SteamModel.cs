// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.ThirdParties.Steam
{
    public class GetSteamTokenResult
    {
        public string SteamTicket;
        public Core.Error Error;
        private readonly string serviceId;

        public string AccelByteSteamToken
        {
            get
            {
                return $"{serviceId}:{SteamTicket}";
            }
        }

        public GetSteamTokenResult(string serviceId)
        {
            this.serviceId = serviceId;
        }
    }
}
