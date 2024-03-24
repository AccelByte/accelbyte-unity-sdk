// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.Steam
{
    public class AccelByteSteam
    {
        internal static System.Func<ISteamImp> SteamImpGetter;

        public static ISteamWrapper Wrapper
        {
            get
            {
                ISteamImp retval = null;
                if (SteamImpGetter == null)
                {
                    if (defaultImp == null)
                    {
                        defaultImp = new NullSteamImp();
                    }
                    retval = defaultImp;
                }
                else
                {
                    retval = SteamImpGetter.Invoke();
                }
                return retval;
            }
        }

        private static ISteamImp defaultImp;

        /// <summary>
        /// Generate AccelByte Platform Token from steam service 
        /// </summary>
        public static Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken()
        {
            return Wrapper.GetAccelByteSteamToken();
        }

        /// <summary>
        /// Generate AccelByte Platform Token from steam service with service identity
        /// </summary>
        /// <param name="serviceIdentity">Steam Identity</param>
        public static Models.AccelByteResult<GetSteamTokenResult, Core.Error> GetAccelByteSteamToken(string serviceIdentity)
        {
            return Wrapper.GetAccelByteSteamToken(serviceIdentity);
        }

        /// <summary>
        /// Send cancel on all active steam ticket. Suggested to call when the game closing.
        /// </summary>
        public static void CancelAllSteamTicket()
        {
            Wrapper.CancelAllSteamTicket();
        }

        /// <summary>
        /// Authenticate a steam ticket is valid.
        /// </summary>
        public static Models.AccelByteResult<Core.Error> AuthenticateSteamTicket(string appid, string steamTicket, string webkey, string identity)
        {
            return Wrapper.AuthenticateSteamTicket(appid, steamTicket, webkey, identity);
        }
    }
}