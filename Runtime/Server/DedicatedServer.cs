// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Server
{
    public class DedicatedServer
    {
        private readonly ServerOauthLoginSession session;
        private readonly CoroutineRunner coroutineRunner;

        public ISession Session { get { return session; } }

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
    }
}