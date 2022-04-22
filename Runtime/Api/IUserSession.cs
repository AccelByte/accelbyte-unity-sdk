// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public interface IUserSession : ISession
    {
        event Action<string> RefreshTokenCallback;

        IEnumerator LoginWithUsername(string username, string password, ResultCallback callback,
            bool rememberMe = false);

        IEnumerator LoginWithUsernameV3(string username, string password, ResultCallback callback,
            bool rememberMe = false);

        IEnumerator LoginWithDeviceId(ResultCallback callback);

        IEnumerator LoginWithOtherPlatform(PlatformType platformType, string platformToken,
            ResultCallback callback);

        IEnumerator LoginWithAuthorizationCode(string code, ResultCallback callback);
        IEnumerator LoginWithLatestRefreshToken(string refreshToken, ResultCallback callback);
        
        IEnumerator Logout(ResultCallback callback);
        IEnumerator RefreshSession(ResultCallback callback);
    }
}