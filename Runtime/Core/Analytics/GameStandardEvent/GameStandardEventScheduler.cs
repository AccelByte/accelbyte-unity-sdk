// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Concurrent;

namespace AccelByte.Core
{
    internal class GameStandardEventScheduler<T> : AnalyticsEventScheduler where T : IAccelByteAnalyticsWrapper
    {
        System.Collections.Generic.List<System.Tuple<TelemetryBody, ResultCallback>> inProcessJob;

        protected override int defaultEventIntervalInMs
        {
            get
            {
                const int defaultIntervalMs = 1 * 60 * 1000;
                return defaultIntervalMs;
            }
        }

        public GameStandardEventScheduler(T analyticsWrapper) : base(analyticsWrapper)
        {
            inProcessJob = new System.Collections.Generic.List<System.Tuple<TelemetryBody, ResultCallback>>();
        }

        internal ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> GetEventQueue()
        {
            return jobQueue;
        }

        internal System.Collections.Generic.List<System.Tuple<TelemetryBody, ResultCallback>> GetInProcessEventQueue()
        {
            return inProcessJob;
        }

        public override void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            TelemetryBody eventBody = new TelemetryBody(telemetryEvent);

            lock (jobQueue)
            {
                jobQueue.Enqueue(new System.Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            }
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        public void SendEvent(TelemetryBody eventBody, ResultCallback callback)
        {
            lock (jobQueue)
            {
                jobQueue.Enqueue(new System.Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            }
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        protected override void TriggerSend()
        {
            lock (jobQueue)
            {
                while (!jobQueue.IsEmpty)
                {
                    if (jobQueue.TryDequeue(out var dequeueResult))
                    {
                        lock (inProcessJob)
                        {
                            inProcessJob.Add(dequeueResult);
                        }
                        TelemetryBody telemetryBody = dequeueResult.Item1;
                        ResultCallback cb = dequeueResult.Item2;
                        
                        telemetryBody.EventNamespace = analyticsWrapper.GetSession().Namespace;

                        analyticsWrapper.SendData(telemetryBody, (callback) =>
                        {
                            lock (inProcessJob)
                            {
                                inProcessJob.Remove(dequeueResult);
                            }
                            if (callback.IsError)
                            {
                                lock (jobQueue)
                                {
                                    jobQueue.Enqueue(dequeueResult);
                                }
                            }
                            else
                            {
                                cb?.Try(callback);
                            }
                        });
                    }
                }
            }
        }

        protected override void RunValidator()
        {
            RunCommonValidator();
        }
    }
}