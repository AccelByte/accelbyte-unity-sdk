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

        /// <summary>
        /// Enqueue a single telemetry data. 
        /// Client/Server should be logged in.
        /// </summary>
        /// <param name="eventBody">Telemetry request with arbitrary payload. </param>
        /// <param name="callback">Returns boolean status via callback when completed. </param>
        public void SendEvent(TelemetryBody eventBody, ResultCallback callback)
        {
            lock (jobQueue)
            {
                jobQueue.Enqueue(new System.Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            }
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        /// <summary>
        /// Send telemetry data in queue
        /// </summary>
        /// <param name="callback">Callback after sending telemetry data is complete</param>
        public void SendQueuedEvent(ResultCallback callback)
        {
            ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> queue = null;
            lock (jobQueue)
            {
                if (jobQueue == null || jobQueue.IsEmpty)
                {
                    callback?.Invoke(Result.CreateOk());
                    return;
                }

                queue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>(jobQueue);
                jobQueue.Clear();
            }

            var queueBackup = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>(queue);

            var telemetryBodies = new System.Collections.Generic.List<TelemetryBody>();
            var telemetryCallbacks = new System.Collections.Generic.List<ResultCallback>();
            while (!queue.IsEmpty)
            {
                if (queue.TryDequeue(out var dequeueResult))
                {
                    TelemetryBody telemetryBody = dequeueResult.Item1;
                    ResultCallback cb = dequeueResult.Item2;
                    telemetryBody.EventNamespace = analyticsWrapper.GetSession().Namespace;

                    telemetryBodies.Add(telemetryBody);
                    telemetryCallbacks.Add(cb);
                }
            }

            analyticsWrapper.SendData(telemetryBodies, (result) =>
            {
                if (result.IsError)
                {
                    lock (jobQueue)
                    {
                        foreach (var unsentQueue in queueBackup)
                        {
                            jobQueue.Enqueue(unsentQueue);
                        }
                    }
                }

                foreach (var telemetryCallback in telemetryCallbacks)
                {
                    telemetryCallback?.Invoke(result);
                }

                callback?.Invoke(result);
            });
        }

        protected override void TriggerSend()
        {
            SendQueuedEvent(callback: null);
        }

        protected override void RunValidator()
        {
            RunCommonValidator();
        }
    }
}