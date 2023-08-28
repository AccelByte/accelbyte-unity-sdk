// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.Steam
{
    public class AccelByteSteam
    {
        internal static System.Func<ISteamWrapper> SteamWrapperGetter;

        public static ISteamWrapper Wrapper
        {
            get
            {
                ISteamWrapper retval = null;
                if (SteamWrapperGetter == null)
                {
                    if (defaultWrapper == null)
                    {
                        defaultWrapper = new NullSteamWrapper();
                    }
                    retval = defaultWrapper;
                }
                else
                {
                    retval = SteamWrapperGetter.Invoke();
                }
                return retval;
            }
        }

        private static ISteamWrapper defaultWrapper;
    }
}