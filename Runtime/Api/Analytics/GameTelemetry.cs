// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Send telemetry data securely and the game user should be logged in.
    /// </summary>
    public class GameTelemetry : WrapperBase
    {
        private readonly AnalyticsService analyticsWrapper;
        private readonly UserSession session;

        private TimeSpan telemetryInterval = TimeSpan.FromMinutes(1);
        private HashSet<string> immediateEvents = new HashSet<string>();
        private ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> jobQueue =
            new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();

        private List<TelemetryBody> eventList = new List<TelemetryBody>();
        
        private static readonly TimeSpan minimumTelemetryInterval = 
            new TimeSpan(0, 0, 5);
        private CancellationTokenSource cancelationTokenSource = null;

        IAccelByteDataStorage cacheStorage;
        string cacheTableName;
        private bool useCache;
        private WaitTimeCommand telemetryLoopCommand;

        internal TimeSpan TelemetryInterval
        {
            get
            {
                return telemetryInterval;
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal GameTelemetry( ClientGameTelemetryApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            analyticsWrapper = new AnalyticsService(inApi, inSession, inCoroutineRunner);
            
            session = inSession;

            useCache = true;
            cacheStorage = session.DataStorage;
            cacheTableName = $"GameTelemetryCache/{AccelByteSDK.Environment.Current}.cache";

            if (session.IsValid())
            {
                SendCachedTelemetry();
            }
            else
            {
                Action<string> onGetRefreshToken = null;
                onGetRefreshToken = (newAccessToken) =>
                {
                    session.RefreshTokenCallback -= onGetRefreshToken;
                    SendCachedTelemetry();
                };
                session.RefreshTokenCallback += onGetRefreshToken;
            }
        }

        ~GameTelemetry()
        {
            if (cancelationTokenSource != null)
            {
                cancelationTokenSource.Cancel();
                cancelationTokenSource.Dispose();
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
            if (interval >= minimumTelemetryInterval)
            {
                telemetryInterval = interval;
            }
            else
            {
                SharedMemory?.Logger?.LogWarning($"Telemetry schedule interval is too small! " +
                    $"Set to {minimumTelemetryInterval.TotalSeconds} seconds.");
                telemetryInterval = minimumTelemetryInterval;
            }

            if (cancelationTokenSource != null)
            {
                cancelationTokenSource.Cancel();
                cancelationTokenSource.Dispose();
                InitializeTelemetryLoop();
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
        /// </summary>
        public void SetUseCache(bool shouldUseCache)
        {
            useCache = shouldUseCache;

            if (!useCache)
            {
                DeleteCache();
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
        /// Server should be logged in. See DedicatedServer.LoginWithClientCredentials()
        /// </summary>
        /// <param name="telemetryBody">Telemetry request with arbitrary payload. </param>
        /// <param name="callback">Returns boolean status via callback when completed. </param>
        public void Send( TelemetryBody telemetryBody
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            if (SharedMemory != null && SharedMemory.TimeManager != null)
            {
                telemetryBody.SetTimeReference(SharedMemory.TimeManager.TimelapseSinceSessionStart.Elapsed);
            }
            
            if (immediateEvents.Contains(telemetryBody.EventName))
            {
                SendTelemetryRequest(
                    new List<TelemetryBody> { telemetryBody }
                    , callback
                );
            }
            else
            {
                jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(telemetryBody, callback));

                if (useCache)
                {
                    AppendEventToCache(telemetryBody);
                }

                if (cancelationTokenSource == null)
                {
                    RunPeriodicTelemetry();
                }
            }
        }

        /// <summary>
        /// Send telemetry data in the btach
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

            eventList.Clear();
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
                else
                {
                    if (useCache)
                    {
                        RemoveEventsFromCache();
                    }
                }

                foreach (var telemetryCallback in telemetryCallbacks)
                {
                    telemetryCallback?.Invoke(result);
                }
                callback?.Invoke(result);
            });
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            analyticsWrapper.SetSharedMemory(newSharedMemory);
        }

        private void SendCachedTelemetry()
        {
            if (!useCache)
            {
                return;
            }
            
            LoadEventsFromCache(telemetryBodies =>
            {
                SendTelemetryRequest(telemetryBodies, cb =>
                {
                    if (!cb.IsError)
                    {
                        RemoveEventsFromCache();
                    }
                });
            });
        }

        internal void ClearQueuedTelemetry()
        {
            lock(jobQueue)
            {
                jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
            }
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
                cancelationTokenSource.Dispose();
                cancelationTokenSource = null;
                return;
            }

            lock (jobQueue)
            {
                if (jobQueue == null || jobQueue.IsEmpty)
                {
                    cancelationTokenSource.Dispose();
                    cancelationTokenSource = null;
                    return;
                }   
            }

            SendTelemetryBatch(callback: null);

            telemetryLoopCommand.ResetTime();
            SharedMemory?.CoreHeartBeat?.Wait(telemetryLoopCommand);
        }

        internal void LoadEventsFromCache(Action<List<TelemetryBody>> onLoadCacheDone)
        {
            if (!session.IsValid())
            {
                onLoadCacheDone?.Invoke(null);
                return;
            }

            LoadEventsFromCache(session.UserId, onLoadCacheDone);
        }

        internal void LoadEventsFromCache(string key, Action<List<TelemetryBody>> onLoadCacheDone)
        {
            cacheStorage.GetItem(
                key
                , tableName: cacheTableName
                , onDone: (bool isSuccess, string loadedTelemetry) =>
                {
                    if (isSuccess && !string.IsNullOrEmpty(loadedTelemetry))
                    {
                        List<TelemetryBody> telemetryBodies = JsonConvert.DeserializeObject<List<TelemetryBody>>(loadedTelemetry);
                        if (telemetryBodies != null)
                        {
                            foreach (var telemetryBody in telemetryBodies)
                            {
                                telemetryBody.IsTimeStampSet = true;
                            }
                        }
                        onLoadCacheDone?.Invoke(telemetryBodies);
                    }
                    else
                    {
                        onLoadCacheDone?.Invoke(null);
                    }
                }
            );
        }

        internal void AppendEventToCache(TelemetryBody telemetryBody, Action<bool> onSaveCacheDone = null)
        {
            if (!session.IsValid())
            {
                onSaveCacheDone?.Invoke(false);
                return;
            }

            AppendEventToCache(session.UserId, telemetryBody, onSaveCacheDone);
        }

        internal void AppendEventToCache(string key, TelemetryBody telemetryBody, Action<bool> onSaveCacheDone = null)
        {
            eventList.Add(telemetryBody);
            var telemetryEventsJson = System.Text.Encoding.UTF8.GetString(eventList.ToUtf8Json());
            cacheStorage.SaveItem(key, telemetryEventsJson, onSaveCacheDone, tableName: cacheTableName);
        }

        internal void DeleteCache(Action<bool> onDone = null)
        {
            cacheStorage.Reset(result: onDone, tableName: cacheTableName);
        }

        private void RemoveEventsFromCache()
        {
            if (session != null && !session.IsValid())
            {
                return;
            }

            cacheStorage.DeleteItem(session.UserId, onDone: null, tableName:cacheTableName);
        }

        private void SendTelemetryRequest(List<TelemetryBody> telemetryBodies, ResultCallback callback)
        {
            if (telemetryBodies != null && telemetryBodies.Count > 0)
            {
                analyticsWrapper.SendData(telemetryBodies, callback);
            }
            else
            {
                callback?.Invoke(Result.CreateOk());
            }
        }
    }
}