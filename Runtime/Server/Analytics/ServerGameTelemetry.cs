// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    /// <summary>
    /// Send telemetry data securely and the server should be logged in.
    /// </summary>
    public class ServerGameTelemetry : WrapperBase
    {
        private readonly ServerAnalyticsService serverAnalyticsService;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        private TimeSpan telemetryInterval = TimeSpan.FromMinutes(1);
        private HashSet<string> immediateEvents = new HashSet<string>();
        private ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
        private System.Threading.CancellationTokenSource cancelationTokenSource = null;
        private static readonly TimeSpan minimumTelemetryInterval = new TimeSpan(0, 0, 5);

        internal TimeSpan TelemetryInterval
        {
            get
            {
                return telemetryInterval;
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal ServerGameTelemetry( ServerGameTelemetryApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null");

            serverAnalyticsService = new ServerAnalyticsService(inApi, inSession, inCoroutineRunner);
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Set the interval of sending telemetry event to the backend.
        /// By default it sends the queued events once a minute.
        /// Should not be less than 5 seconds.
        /// </summary>
        /// <param name="interval">The interval between telemetry event.</param>
        public void SetBatchFrequency( TimeSpan interval )
        {
            if (interval >= minimumTelemetryInterval)
            {
                telemetryInterval = interval;
            }
            else
            {
                SharedMemory?.Logger?.LogWarning($"Telemetry schedule interval is too small! Set to {minimumTelemetryInterval.TotalSeconds} seconds.");
                telemetryInterval = minimumTelemetryInterval;
            }
            
            cancelationTokenSource?.Cancel();
            cancelationTokenSource?.Dispse();
            InitializeTelemetryLoop();
        }

        /// <summary>
        /// Set the interval of sending telemetry event to the backend.
        /// By default it sends the queued events once a minute.
        /// Should not be less than 5 seconds.
        /// </summary>
        /// <param name="intervalSeconds">The seconds interval between telemetry event.</param>
        public void SetBatchFrequency(int intervalSeconds)
        {
            SetBatchFrequency(TimeSpan.FromSeconds(intervalSeconds));
        }

        /// <summary>
        /// Set list of event that need to be sent immediately without the needs to jobQueue it.
        /// </summary>
        /// <param name="eventList">String list of payload EventName.</param>
        public void SetImmediateEventList( List<string> eventList )
        {
            immediateEvents = new HashSet<string>(eventList);
        }

        /// <summary>
        /// Send/enqueue a single authorized telemetry data. 
        /// Server should be logged in. See DedicatedServer::LoginWithClientCredentials()
        /// </summary>
        /// <param name="telemetryBody">Telemetry request with arbitrary payload. </param>
        /// <param name="callback">Returns boolean status via callback when completed. </param>
        public void Send( TelemetryBody telemetryBody
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (SharedMemory != null && SharedMemory.TimeManager != null)
            {
                AccelByteGameTelemetryApi.TryAssignTelemetryBodyClientTimestamps(ref telemetryBody, ref SharedMemory.TimeManager);
            }
            if (immediateEvents.Contains(telemetryBody.EventName))
            {
                SendTelemetryRequest(new List<TelemetryBody> { telemetryBody }, callback);
            }
            else
            {
                jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(telemetryBody, callback));
                if (cancelationTokenSource == null)
                {
                    RunPeriodicTelemetry();
                }
            }
        }

        /// <summary>
        /// Send telemetry data in the batch
        /// </summary>
        /// <param name="callback">Callback after sending telemetry data is complete</param>
        public void SendTelemetryBatch(ResultCallback callback)
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
                jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
            }

            var queueBackup = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>(queue);

            List<TelemetryBody> telemetryBodies = new List<TelemetryBody>();
            List<ResultCallback> telemetryCallbacks = new List<ResultCallback>();
            while (!queue.IsEmpty)
            {
                if (queue.TryDequeue(out var dequeueResult))
                {
                    telemetryBodies.Add(dequeueResult.Item1);
                    telemetryCallbacks.Add(dequeueResult.Item2);
                }
            }

            SendTelemetryRequest(telemetryBodies, result =>
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

        internal void ClearQueuedTelemetry()
        {
            lock (jobQueue)
            {
                jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
            }
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            serverAnalyticsService.SetSharedMemory(newSharedMemory);
        }

        private void RunPeriodicTelemetry()
        {
            if(cancelationTokenSource != null)
            {
                return;
            }

            SendTelemetryBatch(callback: null);
            InitializeTelemetryLoop();
        }

        private void InitializeTelemetryLoop()
        {
            cancelationTokenSource = new System.Threading.CancellationTokenSource();
            IndefiniteLoopCommand telemetryLoopCommand = new IndefiniteLoopCommand(interval: telemetryInterval.TotalSeconds
                , cancellationToken: cancelationTokenSource.Token
                , onUpdate: TelemetryLoop);
            SharedMemory?.CoreHeartBeat?.Wait(telemetryLoopCommand);
        }
        
        private void TelemetryLoop()
        {
            if (cancelationTokenSource == null || cancelationTokenSource.IsCancellationRequested)
            {
                return;
            }
            
            if (session == null || !session.IsValid())
            {
                return;
            }

            lock (jobQueue)
            {
                if (jobQueue == null || jobQueue.IsEmpty)
                {
                    cancelationTokenSource.Cancel();
                    cancelationTokenSource.Dispose();
                    cancelationTokenSource = null;
                    return;
                }   
            }

            SendTelemetryBatch(callback: null);
        }

        private void SendTelemetryRequest(List<TelemetryBody> telemetryBodies, ResultCallback callback)
        {
            if (telemetryBodies != null && telemetryBodies.Count > 0)
            {
                serverAnalyticsService.SendData(telemetryBodies, callback);
            }
            else
            {
                callback?.Invoke(Result.CreateOk());
            }
        }
    }
}