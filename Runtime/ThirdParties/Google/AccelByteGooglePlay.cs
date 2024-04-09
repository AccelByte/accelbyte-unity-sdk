// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.Google
{
    public class AccelByteGooglePlay
    {
        internal static System.Func<IGoogleImp> ImpGetter;

        internal static IGoogleImp Implementation
        {
            get
            {
                IGoogleImp retval = null;
                if (ImpGetter == null)
                {
                    if (defaultImp == null)
                    {
                        defaultImp = new NullGoogleImp();
                    }
                    retval = defaultImp;
                }
                else
                {
                    retval = ImpGetter.Invoke();
                }
                return retval;
            }
        }

        private static IGoogleImp defaultImp;

        /// <summary>
        /// Sign in to googlay play and retrieve google play user id token.
        /// </summary>
        public static Models.AccelByteResult<GetGooglePlayTokenResult, Core.Error> GetAccelBytePlatformLoginToken()
        {
            return Implementation.GetAndroidSignInToken();
        }
    }
}