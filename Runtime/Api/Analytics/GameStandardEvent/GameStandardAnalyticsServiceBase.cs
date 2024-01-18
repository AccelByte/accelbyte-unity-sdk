﻿// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;

namespace AccelByte.Api
{
    public class GameStandardAnalyticsServiceBase<TAnalyticsWrapper> : WrapperBase where TAnalyticsWrapper : IAccelByteAnalyticsWrapper
    {
        internal static readonly string DefaultCacheDirectory = $"{Application.persistentDataPath}/AccelByte/{Application.productName}/";

        internal GameStandardEventScheduler<TAnalyticsWrapper> Scheduler
        {
            get;
            private set;
        }

        public GameStandardAnalyticsServiceBase(TAnalyticsWrapper inApi)
        {
            Initialize(inApi);
        }

        internal void Initialize(TAnalyticsWrapper inApi)
        {
            Scheduler = new GameStandardEventScheduler<TAnalyticsWrapper>(inApi);
        }

        internal void SetAnalyticsWrapper(TAnalyticsWrapper inApi)
        {
            Scheduler.SetAnalyticsApiWrapper(inApi);
        }

        internal void CacheEvents(AccelByteGameStandardEventCacheImpTemplate cacheImp, string environment)
        {
            UnityEngine.Assertions.Assert.IsNotNull(cacheImp);

            var eventQueue = Scheduler.GetEventQueue();
            System.Tuple<TelemetryBody, ResultCallback>[] queueArray = eventQueue.ToArray();
            var inProcessJobArray = Scheduler.GetInProcessEventQueue();

            var telemetryCollection = new System.Collections.Generic.List<TelemetryBody>();
            foreach (var telemetryTuple in queueArray)
            {
                telemetryCollection.Add(telemetryTuple.Item1);
            }
            foreach (var telemetryTuple in inProcessJobArray)
            {
                telemetryCollection.Add(telemetryTuple.Item1);
            }

            cacheImp.SaveToCache(environment, telemetryCollection, null);
        }

        internal void LoadCachedEvent(AccelByteGameStandardEventCacheImpTemplate cacheImp, string environment)
        {
            UnityEngine.Assertions.Assert.IsNotNull(cacheImp);

            cacheImp.LoadCache(environment, (telemetryCollection) =>
            {
                if (telemetryCollection != null)
                {
                    SendEvent(telemetryCollection.ToArray());
                }
            });
        }

        internal void StopScheduler()
        {
            Scheduler.Dispose();
        }

        internal virtual void SendEvent(GameStandardEventPayload payload)
        {
            var telemetryEvent = new AccelByteTelemetryEvent(payload);
            ResultCallback callback = null;
            Scheduler.SendEvent(telemetryEvent, callback);

            if (!Scheduler.IsEventJobEnabled)
            {
                Scheduler.SetEventEnabled(true);
            }
        }

        private void SendEvent(TelemetryBody[] newTelemetryQueue)
        {
            if (newTelemetryQueue != null && newTelemetryQueue.Length > 0)
            {
                for (int index = 0; index < newTelemetryQueue.Length; index++)
                {
                    ResultCallback callback = null;
                    Scheduler.SendEvent(newTelemetryQueue[index], callback);
                }

                if (!Scheduler.IsEventJobEnabled)
                {
                    Scheduler.SetEventEnabled(true);
                }
            }
        }
    }
}
