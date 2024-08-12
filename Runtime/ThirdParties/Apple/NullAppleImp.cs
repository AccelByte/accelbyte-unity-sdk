// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.ThirdParties.Apple
{
    public class NullAppleImp : IAppleImp
    {
        public Models.AccelByteResult<GetAppleTokenResult, Core.Error> GetAppleIdToken()
        {
            var result = new Models.AccelByteResult<GetAppleTokenResult, Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Apple Extension is not installed. Please contact AccelByte support."));
            return result;
        }
    }
}