// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class AnalyticsService : WrapperBase, IAccelByteAnalyticsWrapper
    {
        private readonly AnalyticsApi api = null;
        private readonly ISession session = null;

        public AnalyticsService(AnalyticsApi inApi,
            ISession inSession,
            CoroutineRunner runner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            this.api = inApi;
            session = inSession;
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

        internal Config GetConfig()
        {
            return api.Config;
        }

        public ISession GetSession()
        {
            return this.session;
        }
    }
}