// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
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
        private readonly ServerGameTelemetryApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        private TimeSpan telemetryInterval = TimeSpan.FromMinutes(1);
        private HashSet<string> immediateEvents = new HashSet<string>();
        private ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
        private Coroutine telemetryCoroutine = null;
        private bool isTelemetryJobStarted = false;
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

            api = inApi;
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
                AccelByteDebug.LogWarning($"Telemetry schedule interval is too small! Set to {minimumTelemetryInterval.TotalSeconds} seconds.");
                telemetryInterval = minimumTelemetryInterval;
            }
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

            if (immediateEvents.Contains(telemetryBody.EventName))
            {
                coroutineRunner.Run(
                    api.SendProtectedEvents(
                        new List<TelemetryBody> {telemetryBody}, 
                        session.AuthorizationToken, 
                        callback));
            }
            else
            {
                jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(telemetryBody, callback));
                if (isTelemetryJobStarted == false)
                {
                    StartTelemetryScheduler();
                }
            }
        }

        private IEnumerator RunPeriodicTelemetry()
        {
            while (session.IsValid())
            {
                if (!jobQueue.IsEmpty)
                {
                    Report.GetFunctionLog(GetType().Name);

                    List<TelemetryBody> telemetryBodies = new List<TelemetryBody>(); 
                    List<ResultCallback> telemetryCallbacks = new List<ResultCallback>();
                    while (!jobQueue.IsEmpty)
                    {
                        if (jobQueue.TryDequeue(out var dequeueResult))
                        {
                            telemetryBodies.Add(dequeueResult.Item1);
                            telemetryCallbacks.Add(dequeueResult.Item2);
                        }
                    }

                    yield return api.SendProtectedEvents(telemetryBodies, session.AuthorizationToken, result =>
                    {
                        foreach (var callback in telemetryCallbacks)
                        {
                            callback.Invoke(result);
                        }
                    });
                }
                
                yield return new WaitForSeconds((float)telemetryInterval.TotalSeconds);
            }

            ResetTelemetryScheduler();
        }

        private void StartTelemetryScheduler()
        {
            telemetryCoroutine = coroutineRunner.Run(RunPeriodicTelemetry());
            isTelemetryJobStarted = true;
        }

        private void ResetTelemetryScheduler()
        {
            isTelemetryJobStarted = false;
            telemetryCoroutine = null;
        }
    }
}