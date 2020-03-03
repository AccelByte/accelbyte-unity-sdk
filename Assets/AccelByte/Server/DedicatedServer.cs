// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Server
{
    public class DedicatedServer
    {
        private readonly ServerOauthLoginSession session;
        private readonly CoroutineRunner coroutineRunner;

        public DedicatedServer(ServerOauthLoginSession session, CoroutineRunner coroutineRunner)
        {
            this.session = session;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Login as an application (client) with client credentials
        /// </summary>
        public void LoginWithClientCredentials(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            this.coroutineRunner.Run(this.session.LoginWithClientCredentials(callback));
        }
    }
}