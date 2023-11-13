// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerAnalyticsService : WrapperBase, IAccelByteAnalyticsWrapper
    {
        private readonly ServerAnalyticsApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerAnalyticsService(ServerAnalyticsApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, nameof(inApi) + " is null.");

            api = inApi;
            session = inSession;
        }

        public void SendData(TelemetryBody data, ResultCallback callback)
        {
            if (session.IsValid())
            {
                List<TelemetryBody> dataList = new List<TelemetryBody>() { data };
                api.SendData(dataList, callback);
            }
            else
            {
                callback.TryError(ErrorCode.InvalidRequest, "User is not logged in.");
            }
        }

        public void SendData(List<TelemetryBody> data, ResultCallback callback)
        {
            if (session.IsValid())
            {
                api.SendData(data, callback);
            }
            else
            {
                callback.TryError(ErrorCode.InvalidRequest, "User is not logged in.");
            }
        }

        public ISession GetSession()
        {
            return this.session;
        }

    }
}