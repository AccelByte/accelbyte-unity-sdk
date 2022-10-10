// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Send telemetry data securely and the game user should be logged in.
    /// </summary>
    public class GameTelemetry : WrapperBase
    {
        private readonly GameTelemetryApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        private TimeSpan telemetryInterval = TimeSpan.FromMinutes(1);
        private HashSet<string> immediateEvents = new HashSet<string>();
        private ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> jobQueue = 
            new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
        private List<TelemetryBody> eventList = new List<TelemetryBody>();
        
        private Coroutine telemetryCoroutine = null;
        private bool isTelemetryJobStarted = false;
        private static readonly TimeSpan MINIMUM_INTERVAL_TELEMETRY = 
            new TimeSpan(0, 0, 5);
        
        internal GameTelemetry( GameTelemetryApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
            if (session.IsValid())
            {
                LoadEventsFromCache();
            }
            else
            {
                session.RefreshTokenCallback += LoadEventsFromCache;
            }
        }

        /// <summary>
        /// Set the interval of sending telemetry event to the backend.
        /// By default it sends the queued events once a minute.
        /// Should not be less than 5 seconds.
        /// </summary>
        /// <param name="interval">The interval between telemetry event.</param>
        public void SetBatchFrequency( TimeSpan interval )
        {
            if (interval >= MINIMUM_INTERVAL_TELEMETRY)
            {
                telemetryInterval = interval;
            }
            else
            {
                AccelByteDebug.LogWarning($"Telemetry schedule interval is too small! " +
                    $"Set to {MINIMUM_INTERVAL_TELEMETRY.TotalSeconds} seconds.");
                telemetryInterval = MINIMUM_INTERVAL_TELEMETRY;
            }
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
                        new List<TelemetryBody> { telemetryBody }, 
                        callback));
            }
            else
            {
                if (isTelemetryJobStarted == false)
                {
                    StartTelemetryScheduler();
                }
                jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(telemetryBody, callback));
                AppendEventToCache(telemetryBody);
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

                    eventList.Clear();
                    yield return api.SendProtectedEvents(telemetryBodies, result =>
                    {
                        RemoveEventsFromCache();
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

        private string GetTelemetryKey()
        {
            return "TELEMETRY_" + session.UserId;
        }

        private void LoadEventsFromCache( string accessToken = "" )
        {
            if (!session.IsValid() || !session.usePlayerPrefs)
            {
                return;
            }
            string telemetryKey = GetTelemetryKey();
            var events = PlayerPrefs.GetString(telemetryKey);
            if (!string.IsNullOrEmpty(events))
            {
                List<TelemetryBody> telemetryBodies = new List<TelemetryBody>();
                var eventListDict = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(events);
                foreach(var item in eventListDict)
                {
                    TelemetryBody telemetryBody = new TelemetryBody();
                    telemetryBody.EventName = item["EventName"].ToString();
                    telemetryBody.EventNamespace = item["EventNamespace"].ToString();
                    telemetryBody.Payload = item["Payload"];
                    telemetryBodies.Add(telemetryBody);
                }
                coroutineRunner.Run(
                    api.SendProtectedEvents(telemetryBodies, result =>
                    {
                        RemoveEventsFromCache();
                    })
                );
            }
            if (!string.IsNullOrEmpty(accessToken))
            {
                session.RefreshTokenCallback -= LoadEventsFromCache;
            }
        }

        private void RemoveEventsFromCache()
        {
            if (!session.IsValid() || !session.usePlayerPrefs)
            {
                return;
            }
            string telemetryKey = GetTelemetryKey();
            PlayerPrefs.DeleteKey(telemetryKey);
            PlayerPrefs.Save();
        }

        private void AppendEventToCache( TelemetryBody telemetryBody )
        {
            if (!session.IsValid() || !session.usePlayerPrefs)
            {
                return;
            }
            string telemetryKey = GetTelemetryKey();
            eventList.Add(telemetryBody);
            var telemetryEventsJson = System.Text.Encoding.UTF8.GetString(eventList.ToUtf8Json());
            PlayerPrefs.SetString(telemetryKey, telemetryEventsJson);
            PlayerPrefs.Save();
        }
    }
}