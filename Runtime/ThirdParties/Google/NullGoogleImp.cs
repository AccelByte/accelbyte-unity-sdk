// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.ThirdParties.Google
{
    public class NullGoogleImp : IGoogleImp
    {
        public Models.AccelByteResult<GetGooglePlayTokenResult, Core.Error> GetAndroidSignInToken()
        {
            var result = new Models.AccelByteResult<GetGooglePlayTokenResult, Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Android Extension is not installed. Please contact AccelByte support."));
            return result;
        }
    }
}