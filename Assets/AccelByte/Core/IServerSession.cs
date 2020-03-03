// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public interface IServerSession
    {
        string AuthorizationToken { get; }
    }
    
    public static class ServerSessionExtension
    {
        public static bool IsValid(this IServerSession session)
        {
            return !string.IsNullOrEmpty(session.AuthorizationToken);
        }

        public static void AssertValid(this IServerSession session)
        {
            Assert.IsNotNull(session);
            Assert.IsFalse(string.IsNullOrEmpty(session.AuthorizationToken));
        }
    }
}