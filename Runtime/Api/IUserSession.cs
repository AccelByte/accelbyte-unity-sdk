// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public interface IUserSession : ISession
    {
        string UserId { get; }
        bool IsComply { get; }
        
        event Action<string> RefreshTokenCallback;

        IEnumerator LoginWithUsername(string username, string password, ResultCallback<TokenData, OAuthError> callback,
            bool rememberMe = false);

        IEnumerator LoginWithUsernameV3(string username, string password, ResultCallback<TokenData, OAuthError> callback,
            bool rememberMe = false);

        IEnumerator LoginWithDeviceId(ResultCallback<TokenData, OAuthError> callback);

        IEnumerator LoginWithOtherPlatform(PlatformType platformType, string platformToken,
            ResultCallback<TokenData, OAuthError> callback, bool createHeadless);

        IEnumerator LoginWithAuthorizationCode(string code, ResultCallback<TokenData, OAuthError> callback);
        IEnumerator LoginWithLatestRefreshToken(string refreshToken, ResultCallback<TokenData, OAuthError> callback);
        
        IEnumerator Logout(ResultCallback callback);
        IEnumerator RefreshSession(ResultCallback<TokenData, OAuthError> callback);
    }
    public static class UserSessionExtension
    {
        public static bool IsValid( this IUserSession session )
        {
            return session != null && !string.IsNullOrEmpty( session.AuthorizationToken ) && !string.IsNullOrEmpty( session.UserId );
        }

        public static void AssertValid( this IUserSession session )
        {
            Assert.IsNotNull( session );
            Assert.IsFalse( string.IsNullOrEmpty( session.AuthorizationToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( session.UserId ) );
        }
    }
}