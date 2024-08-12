// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class AnalyticsService : WrapperBase, IAccelByteAnalyticsWrapper
    {
        private readonly ClientGameTelemetryApi api = null;
        private readonly ISession session = null;

        [UnityEngine.Scripting.Preserve]
        public AnalyticsService(ClientGameTelemetryApi inApi,
            ISession inSession,
            CoroutineRunner runner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            this.api = inApi;
            session = inSession;
        }

        public void SendData(List<TelemetryBody> data, ResultCallback callback)
        {
            AccelByteTimeManager timeManager = SharedMemory?.TimeManager;
            
            if (timeManager != null)
            {
                foreach (var telemetryBody in data)
                {
                    if (!telemetryBody.IsTimeStampSet && telemetryBody.CreatedElapsedTime == null)
                    {
                        telemetryBody.SetTimeReference(timeManager.TimelapseSinceSessionStart.Elapsed);
                    }
                }
            }

            if (timeManager != null)
            {
                System.Action sendTelemetryAfterFetchingServerTime = () =>
                {
                    foreach (var telemetryBody in data)
                    {
                        if (!telemetryBody.IsTimeStampSet)
                        {
                            telemetryBody.ClientTimestamp = telemetryBody.CalculateClientTimestampFromServerTime(timeManager.GetCachedServerTime());
                        }
                    }
                    ApiSend(data, callback);
                };
                
                if (timeManager.GetCachedServerTime() == null)
                {
                    Action<bool> onFetchingServerTimeDone = null;
                    onFetchingServerTimeDone = success =>
                    {
                        if (success)
                        {
                            timeManager.OnFetchServerTimeComplete -= onFetchingServerTimeDone;
                            sendTelemetryAfterFetchingServerTime?.Invoke();
                        }
                        else
                        {
                            ApiSend(data, callback);
                        }
                    };
                    timeManager.OnFetchServerTimeComplete += onFetchingServerTimeDone;

                    api.FetchServerTime(ref timeManager);
                }
                else
                {
                    sendTelemetryAfterFetchingServerTime?.Invoke();
                }
            }
            else
            {
                ApiSend(data, callback);
            }
        }

        public void SendData(TelemetryBody data, ResultCallback callback)
        {
            List<TelemetryBody> dataList = new List<TelemetryBody>() { data };
            SendData(dataList, callback);
        }

        private void ApiSend(List<TelemetryBody> data, ResultCallback callback)
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