// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine.Assertions;

namespace AccelByte.Core {
    public interface ISession
    {
        string AuthorizationToken { get; }
        string UserId { get; }
    }

    public static class SessionExtension
    {
        public static bool IsValid(this ISession session)
        {
            return session != null && !string.IsNullOrEmpty(session.AuthorizationToken) && !string.IsNullOrEmpty(session.UserId);
        }

        public static void AssertValid(this ISession session)
        {
            Assert.IsNotNull(session);
            Assert.IsFalse(string.IsNullOrEmpty(session.AuthorizationToken));
            Assert.IsFalse(string.IsNullOrEmpty(session.UserId));
        }
    }
}