// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.NintendoSwitch
{
    public class AccelByteSwitch
    {
        internal static System.Func<ISwitchImp> SwitchImpGetter;

        internal static ISwitchImp Wrapper
        {
            get
            {
                ISwitchImp retval = null;
                if (SwitchImpGetter == null)
                {
                    if (defaultWrapper == null)
                    {
                        defaultWrapper = new NullSwitchImp();
                    }
                    retval = defaultWrapper;
                }
                else
                {
                    retval = SwitchImpGetter.Invoke();
                }
                return retval;
            }
        }

        private static ISwitchImp defaultWrapper;

        public static Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetAccelBytePlatformLoginToken()
        {
            return Wrapper.GetNsaToken();
        }

        public static Models.AccelByteResult<Core.Error> Initialize(string mountName)
        {
            return Wrapper.Initialize(mountName);
        }

        public static Models.AccelByteResult<Core.Error> MountStorage(string mountName)
        {
            return Wrapper.MountStorage(mountName);
        }

        public static Models.AccelByteResult<Core.Error> UnmountStorage(string mountName)
        {
            return Wrapper.UnmountStorage(mountName);
        }
    }
}