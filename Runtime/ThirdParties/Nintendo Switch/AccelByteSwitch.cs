// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.NintendoSwitch
{
    public class AccelByteSwitch
    {
        internal static System.Func<ISwitchWrapper> SwitchWrapperGetter;

        public static ISwitchWrapper Wrapper
        {
            get
            {
                ISwitchWrapper retval = null;
                if (SwitchWrapperGetter == null)
                {
                    if (defaultWrapper == null)
                    {
                        defaultWrapper = new NullSwitchWrapper();
                    }
                    retval = defaultWrapper;
                }
                else
                {
                    retval = SwitchWrapperGetter.Invoke();
                }
                return retval;
            }
        }

        private static ISwitchWrapper defaultWrapper;
    }
}