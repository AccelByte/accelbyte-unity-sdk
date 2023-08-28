// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.Steam
{
    public interface ISteamWrapper
    {
        public bool Initialized
        {
            get;
        }

        void CancelAllSteamTicket();

        public void GetAccelByteSteamToken(System.Action<GetSteamTokenResult> onGetPlatformTokenFinished);

        public void GetAccelByteSteamToken(string serviceIdentity, System.Action<GetSteamTokenResult> onGetPlatformTokenFinished);

        public void AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity, System.Action onSuccess = null, System.Action<string> onFailed = null);
    }
}