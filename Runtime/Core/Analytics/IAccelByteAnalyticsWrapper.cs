// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public interface IAccelByteAnalyticsWrapper
    {
        public void SendData(List<TelemetryBody> data, ResultCallback callback);
        public void SendData(TelemetryBody data, ResultCallback callback);

        public ISession GetSession();
    }
}