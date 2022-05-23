// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public interface ISession
    {
        string AuthorizationToken { get; set; }
    }
    
    public static class SessionExtension
    {
        public static bool IsValid(this ISession session )
        {
            return !string.IsNullOrEmpty(session.AuthorizationToken);
        }

        public static void AssertValid(this ISession session )
        {
            Assert.IsNotNull(session);
            Assert.IsFalse(string.IsNullOrEmpty(session.AuthorizationToken));
        }
    }
}