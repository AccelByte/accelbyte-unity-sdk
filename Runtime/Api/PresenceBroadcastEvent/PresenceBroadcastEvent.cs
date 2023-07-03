// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class PresenceBroadcastEvent : WrapperBase
    {
        private PresenceBroadcastEventApi api = null;
        private ISession session = null;

        public PresenceBroadcastEvent(PresenceBroadcastEventApi inApi,
            ISession inSession,
            CoroutineRunner runner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            this.api = inApi;
            session = inSession;
        }

        public void SendData(TelemetryBody data, ResultCallback callback)
        {
            if (session.IsValid())
            {
                api.SendPresenceBroadcastEvent(data, callback);
            }
            else
            {
                callback.TryError(ErrorCode.InvalidRequest, "User is not logged in.");
                return;
            }
        }

        internal Config GetConfig()
        {
            return api.Config;
        }

        internal ISession GetSession()
        {
            return this.session;
        }
    }
}
