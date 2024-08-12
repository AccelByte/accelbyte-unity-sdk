// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.ThirdParties.GooglePlayGames
{
    public class NullGooglePlayGamesImp : IGooglePlayGamesImp
    {
        public Models.AccelByteResult<SignInGooglePlayGamesResult, Core.Error> GetGooglePlayGamesSignInToken()
        {
            var result = new Models.AccelByteResult<SignInGooglePlayGamesResult, Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Google Play Games Extension is not installed. Please contact AccelByte support."));
            return result;
        }
    }
}