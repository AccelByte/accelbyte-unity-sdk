// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class PresenceBroadcastEvent : WrapperBase, IAccelByteAnalyticsWrapper
    {
        private readonly PresenceBroadcastEventApi api = null;
        private readonly ISession session = null;

        [UnityEngine.Scripting.Preserve]
        public PresenceBroadcastEvent(PresenceBroadcastEventApi inApi,
            ISession inSession,
            CoroutineRunner runner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            api = inApi;
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

        public void SendData(List<TelemetryBody> data, ResultCallback callback)
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

        public ISession GetSession()
        {
            return session;
        }
    }
}
