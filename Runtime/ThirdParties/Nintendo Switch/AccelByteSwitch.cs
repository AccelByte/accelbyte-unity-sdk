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
        /// Get Switch user access token
        /// </summary>
        public static Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetAccelBytePlatformLoginToken()
        {
            return Wrapper.GetNsaToken();
        }

        /// <summary>
        /// Initialize AccelByte Switch SDK
        /// </summary>
        /// <param name="mountName">Mount name</param>
        public static Models.AccelByteResult<Core.Error> Initialize(string mountName)
        {
            return Initialize(mountName, handleOnExit: true);
        }

        /// <summary>
        /// Initialize AccelByte Switch SDK
        /// </summary>
        /// <param name="mountName">Mount name</param>
        /// <param name="handleOnExit">Give permission to AccelByte SDK to handle Switch exit notification</param>
        public static Models.AccelByteResult<Core.Error> Initialize(string mountName, bool handleOnExit)
        {
            return Wrapper.Initialize(mountName, handleOnExit);
        }

        /// <summary>
        /// Mount switch storage
        /// </summary>
        /// <param name="mountName">Mount Name</param>
        public static Models.AccelByteResult<Core.Error> MountStorage(string mountName)
        {
            return Wrapper.MountStorage(mountName);
        }

        /// <summary>
        /// Unmount switch storage
        /// </summary>
        /// <param name="mountName">Mount Name</param>
        public static Models.AccelByteResult<Core.Error> UnmountStorage(string mountName)
        {
            return Wrapper.UnmountStorage(mountName);
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