// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
using System;

namespace AccelByte.ThirdParties.GooglePlayGames
{
    [Obsolete("This interface is deprecated and will be removed on AGS 2025.5. Please use package AccelByte Unity SDK Google for future development.")]
    public interface IGooglePlayGamesImp
    {
        Models.AccelByteResult<SignInGooglePlayGamesResult, Core.Error> GetGooglePlayGamesSignInToken();
    }
}