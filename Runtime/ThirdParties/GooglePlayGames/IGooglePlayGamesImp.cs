// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.GooglePlayGames
{
    public interface IGooglePlayGamesImp
    {
        Models.AccelByteResult<SignInGooglePlayGamesResult, Core.Error> GetGooglePlayGamesSignInToken();
    }
}