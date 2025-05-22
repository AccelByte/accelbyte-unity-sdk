// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public class DedicatedServer
    {
        internal const string CommandLineDsId = "-dsid";

        private readonly ServerOauthLoginSession session;
        private readonly CoroutineRunner coroutineRunner;

        public ISession Session { get { return session; } }

        internal ServerOauthLoginSession InternalSession
        {
            get
            {
                return session;
            }
        }

        public DedicatedServer( ServerOauthLoginSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Login as an application (client) with client credentials
        /// </summary>
        public void LoginWithClientCredentials( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(session.LoginWithClientCredentials(callback));
        }

        /// <summary>
        /// Retrieves the JSON Web Key Set (JWKS) asynchronously.
        /// </summary>
        /// <param name="callback">The callback to receive the result containing the JWKS.</param>
        public void GetJwks(ResultCallback<JwkSet> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            coroutineRunner.Run(session.GetJwks(callback));
        }

        /// <summary>
        /// Logout from current session.
        /// </summary>
        public void Logout(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (session == null || (session != null && !session.IsValid()))
            {
                callback?.TryError(new Error(ErrorCode.IsNotLoggedIn));
                return;
            }
            
            session.LogoutAsync(callback);
        }
    }
}
