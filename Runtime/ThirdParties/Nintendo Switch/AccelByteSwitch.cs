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

        /// <summary>
        /// Initialize AccelByte Switch SDK
        /// </summary>
        /// <param name="mountName">Mount name</param>
        public static Models.AccelByteResult<Core.Error> Initialize(string mountName)
        {
            return Wrapper.Initialize(mountName);
        }

        /// <summary>
        /// Get Switch user access token
        /// </summary>
        public static Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetAccelBytePlatformLoginToken(string userHandle)
        {
            return Wrapper.GetNsaToken(userHandle);
        }

        /// <summary>
        /// Trigger saving SDK caches
        /// </summary>
        public static Models.AccelByteResult<Core.Error> SaveCacheFiles()
        {
            return Wrapper.SaveCacheFiles();
        }

        /// <summary>
        /// Trigger SDK Switch on game exit handler manually.
        /// </summary>
        public static Models.AccelByteResult<Core.Error> HandleGameExit()
        {
            return Wrapper.HandleGameExit();
        }
    }
}