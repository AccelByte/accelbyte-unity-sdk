// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
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
        private CancellationTokenSource cancellationTokenSource = null;

        internal bool IsUseCache;
        internal bool IsAutoSendOnStartup;
        private IAccelByteDataStorage cacheStorage;
        private string cacheTableName;
        private WaitTimeCommand telemetryLoopCommand;

        internal TimeSpan TelemetryInterval
        {
            get
            {
                return telemetryInterval;
            }
        }

        internal Action OnTelemetryLoopUpdate;

        [UnityEngine.Scripting.Preserve]
        internal GameTelemetry( ClientGameTelemetryApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            analyticsWrapper = new AnalyticsService(inApi, inSession, inCoroutineRunner);
            
            session = inSession;

            cacheStorage = session.DataStorage;
            cacheTableName = $"GameTelemetryCache/{AccelByteSDK.Environment.Current}.cache";

            SetUseCache(inApi.Config.GameTelemetryCacheEnabled);

            IsAutoSendOnStartup = inApi.Config.EnableGameTelemetryStartupAutoSend;
            if (!IsAutoSendOnStartup)
            {
                return;
            }

            if (session.IsValid())
            {
                SendCachedTelemetry(callback: null);
                return;
            }
                
            Action<string> onGetRefreshToken = null;
            onGetRefreshToken = (newAccessToken) =>
            {
                session.RefreshTokenCallback -= onGetRefreshToken;
                SendCachedTelemetry(callback: null);
            };
            session.RefreshTokenCallback += onGetRefreshToken;
        }

        ~GameTelemetry()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }

        /// <summary>
        /// Enable or disable write operations to the file storage cache.
        /// The cache stores all events in JSON format as a file to be sent on 
        /// service initialization (can be disabled via client config editor) 
        /// or SendCachedTelemetry(callback) in case of crash, errors, or unclean game/app termination.
        /// The cache is enabled by default and can also be configured via client config editor.
        /// </summary>
        /// <param name="isUseCache">Enables or disables cache write operations.</param>
        /// <param name="clearCurrentCache">Clears currently built up cache if true.</param>
        public void SetUseCache(bool isUseCache, bool clearCurrentCache = false)
        {
            IsUseCache = isUseCache;

            if (clearCurrentCache)
            {
                eventList.Clear();
                DeleteCache();
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

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            InitializeTelemetryHeartbeat();
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
                
                if (IsUseCache)
                {
                    AppendEventToCache(telemetryBody);
                }

                if (cancellationTokenSource == null)
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
                    if (IsUseCache)
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

        /// <summary>
        /// Send cached telemetry events
        /// </summary>
        /// <param name="callback">Returns a result via callback when operation is done</param>
        public void SendCachedTelemetry(ResultCallback callback)
        {
            LoadEventsFromCache(telemetryBodies =>
            {
                if (telemetryBodies == null || telemetryBodies.Count == 0)
                {
                    SharedMemory?.Logger?.LogVerbose("No events loaded from cache, send operation is cancelled.");
                    callback?.TryError(ErrorCode.InvalidRequest);
                    return;
                }

                SendTelemetryRequest(telemetryBodies, result =>
                {
                    if (!result.IsError)
                    {
                        RemoveEventsFromCache();
                    }

                    callback?.Try(result);
                });
            });
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            analyticsWrapper.SetSharedMemory(newSharedMemory);
        }

        internal void ClearQueuedTelemetry()
        {
            lock(jobQueue)
            {
                jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
            }
        }

        private void InitializeTelemetryHeartbeat()
        {
            cancellationTokenSource = new CancellationTokenSource();
            telemetryLoopCommand = new WaitTimeCommand(waitTime: telemetryInterval.TotalSeconds
                , cancellationToken: cancellationTokenSource.Token
                , onDone: TelemetryLoop);
            SharedMemory?.CoreHeartBeat?.Wait(telemetryLoopCommand);
        }

        private void RunPeriodicTelemetry()
        {
            if(cancellationTokenSource != null)
            {
                return;
            }

            SendTelemetryBatch(callback: null);

            InitializeTelemetryHeartbeat();
        }

        private void TelemetryLoop()
        {
            OnTelemetryLoopUpdate?.Invoke();

            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
            
            if (session == null || !session.IsValid())
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                return;
            }

            lock (jobQueue)
            {
                if (jobQueue == null || jobQueue.IsEmpty)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
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

            if (!IsUseCache)
            {
                SharedMemory?.Logger?.LogVerbose("Caching is disabled, write event operation is cancelled.");
                onSaveCacheDone?.Invoke(false);
                return;
            }

            AppendEventToCache(session.UserId, telemetryBody, onSaveCacheDone);
        }

        internal void AppendEventToCache(string key, TelemetryBody telemetryBody, Action<bool> onSaveCacheDone = null)
        {
            if (!IsUseCache)
            {
                SharedMemory?.Logger?.LogVerbose("Caching is disabled, write event operation is cancelled.");
                onSaveCacheDone?.Invoke(false);
                return;
            }

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