// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public abstract class AnalyticsEventScheduler : IDisposable
    {
        public const int MiniumAllowedIntervalInlMs = 5 * 1000;

        protected IAccelByteAnalyticsWrapper analyticsWrapper = null;
        protected AccelByteHeartBeat maintainer;

        protected int eventIntervalInlMs;
        protected bool keepValidating;

        protected abstract int defaultEventIntervalInMs
        {
            get;
        }

        protected virtual int defaultMinimumEventIntervalInMs
        {
            get
            {
                return MiniumAllowedIntervalInlMs;
            }
        }

        protected ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>> jobQueue =
            new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();

        internal Action<TelemetryBody> OnTelemetryEventAdded;

        public bool KeepValidating
        {
            get
            {
                return keepValidating;
            }
        }

        public bool IsEventJobEnabled
        {
            get
            {
                if (maintainer == null)
                {
                    return false;
                }

                return maintainer.IsHeartBeatEnabled;
            }
        }

        public int EventIntervalInlMs
        {
            get
            {
                return eventIntervalInlMs;
            }
        }

        public AnalyticsEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper)
        {
            eventIntervalInlMs = defaultEventIntervalInMs;
            this.analyticsWrapper = analyticsWrapper;

            keepValidating = true;
            RunValidator();
        }

        /// <summary>
        /// Add event to scheduler's cache
        /// </summary>
        /// <param name="telemetryEvent">New event</param>
        /// <param name="callback">Event on sent</param>
        public abstract void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback);
        
        /// <summary>
        /// Execute validator. For example checking session, or execute login before starting the heartbeat.
        /// </summary>
        protected abstract void RunValidator();
        
        /// <summary>
        /// Send cached events with the API
        /// </summary>
        protected abstract void TriggerSend();

        #region Disposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            StopEventScheduler();
            keepValidating = false;
        }
        #endregion

        internal void SetAnalyticsApiWrapper(IAccelByteAnalyticsWrapper newAnalyticsWrapper)
        {
            analyticsWrapper = newAnalyticsWrapper;
        }

        internal void ClearTasks()
        {
            lock (jobQueue)
            {
                jobQueue = new ConcurrentQueue<Tuple<TelemetryBody, ResultCallback>>();
            }
        }

        internal void SetInterval(int eventIntervalInlMs)
        {
            if (eventIntervalInlMs < defaultMinimumEventIntervalInMs)
            {
                AccelByteDebug.LogWarning($"The interval is lower than the allowed. The interval will be changed into {defaultMinimumEventIntervalInMs} ms");
                this.eventIntervalInlMs = defaultMinimumEventIntervalInMs;
            }
            else
            {
                this.eventIntervalInlMs = eventIntervalInlMs;
            }
        }

        public void SetInterval(float newIntervalInS)
        {
            SetInterval(UnityEngine.Mathf.RoundToInt(Utils.TimeUtils.SecondsToMilliseconds(newIntervalInS)));
        }

        internal void SetEventEnabled(bool enabled, int intervalInMs = 0)
        {
            bool enablePredefined = FilterEventActivation(enabled);

            if (intervalInMs == 0)
            {
                intervalInMs = eventIntervalInlMs;
            }

            if (enablePredefined)
            {
                StartEventScheduler(intervalInMs);
            }
            else
            {
                StopEventScheduler();
            }
        }

        /// <summary>
        /// The function to adjust the scheduler activation. For example the scheduler is always off on server platform.
        /// </summary>
        /// <param name="setSchedulerActive">The targeted active state</param>
        /// <returns>The final active state</returns>
        protected virtual bool FilterEventActivation(bool setSchedulerActive)
        {
            return setSchedulerActive;
        }

        private void StartEventScheduler(int intervalMs)
        {
            Report.GetFunctionLog(GetType().Name);

            if (maintainer != null)
            {
                maintainer.Stop();
            }

            int heartbeatInterval = intervalMs;
            if(heartbeatInterval < defaultMinimumEventIntervalInMs)
            {
                heartbeatInterval = defaultEventIntervalInMs;
                AccelByteDebug.LogWarning($"The interval is lower than the allowed. The interval will be changed into {defaultMinimumEventIntervalInMs} ms");
            }
            maintainer = new AccelByteHeartBeat(heartbeatInterval);
            maintainer.OnHeartbeatTrigger += () =>
            {
                if (analyticsWrapper != null && analyticsWrapper.GetSession().IsValid())
                {
                    TriggerSend();
                }
            };
        }

        private void StopEventScheduler()
        {
            if (maintainer != null)
            {
                maintainer.Stop();
                maintainer = null;
            }
        }

        #region Common Function
        protected void CommonTriggerSend(bool overrideEventNamespaceFromSessionData)
        {
            lock (jobQueue)
            {
                while (!jobQueue.IsEmpty)
                {
                    if (jobQueue.TryDequeue(out var dequeueResult))
                    {
                        ISession session = analyticsWrapper != null ? analyticsWrapper.GetSession() : null;
                        TelemetryBody telemetryBody = dequeueResult.Item1;
                        if(overrideEventNamespaceFromSessionData && session != null && session.IsValid())
                        {
                            telemetryBody.EventNamespace = session.Namespace;
                        }
                        ResultCallback cb = dequeueResult.Item2;
                        analyticsWrapper.SendData(telemetryBody, cb);
                    }
                }
            }
        }

        protected void SendCommonEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            TelemetryBody eventBody = new TelemetryBody(telemetryEvent);

            lock (jobQueue)
            {
                jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            }
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        protected async void RunCommonValidator()
        {
            bool currentSessionValid;
            ISession session = null;

            while (keepValidating)
            {
                if (maintainer != null)
                {
                    session = analyticsWrapper != null ? analyticsWrapper.GetSession() : null;

                    currentSessionValid = session != null ? session.IsValid() : false;

                    if (currentSessionValid)
                    {
                        maintainer.Start();
                    }
                    else
                    {
                        maintainer.Pause();
                    }
                }

                await System.Threading.Tasks.Task.Delay(AccelByteHttpHelper.HttpDelayOneFrameTimeMs);
            }
        }
        #endregion
    }
}