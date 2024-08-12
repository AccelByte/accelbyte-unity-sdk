// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager

namespace AccelByte.ThirdParties.Apple
{
    public class AccelByteApple
    {
        internal static System.Func<IAppleImp> ImpGetter;

        internal static IAppleImp Implementation
        {
            get
            {
                IAppleImp retval = null;
                if (ImpGetter == null)
                {
                    if (defaultImp == null)
                    {
                        defaultImp = new NullAppleImp();
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

        private static IAppleImp defaultImp;

        /// <summary>
        /// Sign in with Apple and retrieve Apple user id token.
        /// </summary>
        public static Models.AccelByteResult<GetAppleTokenResult, Core.Error> GetAppleSignInToken()
        {
            return Implementation.GetAppleIdToken();
        }
    }
}