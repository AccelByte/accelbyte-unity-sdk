// Copyright (c) 2022-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Send heart beat event.
    /// </summary>
    public class HeartBeat : WrapperBase
    {
        internal const string PublisherNamespaceKey = "pn";
        internal const string CustomerNameKey = "customer";
        internal const string EnvironmentKey = "environment";
        internal const string GameNameKey = "game";

        private const int defaultHeartbeatIntervalSeconds = 60;

        private readonly HeartBeatApi api;

        private ResultCallback onHeartBeatResponse = null;

        private Dictionary<string, object> heartBeatData = new Dictionary<string, object>();

        private HeartBeatMaintainer maintainer;
        private int heartBeatIntervalMs = Utils.TimeUtils.SecondsToMilliseconds(defaultHeartbeatIntervalSeconds);

        public bool IsHeartBeatJobRunning
        {
            get
            {
                if(maintainer == null)
                {
                    return false;
                }

                return maintainer.IsHeartBeatJobRunning;
            }
        }

        public bool IsHeartBeatJobEnabled
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

        public ResultCallback OnHeartBeatResponse
        {
            get
            {
                return onHeartBeatResponse;
            }
        }

        public Dictionary<string, object> HeartBeatData 
        {
            get
            {
                return heartBeatData;
            }
        }

        public HeartBeatApi Api
        {
            get
            {
                return api;
            }
        }

        public int HeartBeatIntervalMs
        {
            get
            {
                return heartBeatIntervalMs;
            }
        }

        internal HeartBeat(HeartBeatApi inApi)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");

            api = inApi;
        }

        public void SetHeartbeatInterval(int newIntervalMs)
        {
            if(maintainer != null)
            {
                AccelByteDebug.LogWarning("Heartbeat is still running, please stop heartbeat before changing the interval");
                return;
            }
            heartBeatIntervalMs = newIntervalMs;
        }

        /// <summary>
        /// Add heartbeat send information
        /// </summary>
        /// <param name="key">Data key</param>
        /// <param name="data">Data value</param>
        public void AddSendData(string key, string data)
        {
            if(heartBeatData.ContainsKey(key))
            {
                heartBeatData[key] = data;
            }
            else
            {
                heartBeatData.Add(key, data);
            }
        }

        /// <summary>
        /// Remove heartbeat send information
        /// </summary>
        /// <param name="key">Data key</param>
        /// <returns>true: Successfully removed</returns>
        /// <returns>false: Key not found removed</returns>
        public bool RemoveSendData(string key)
        {
            var successfullyRemoved = heartBeatData.Remove(key);
            return successfullyRemoved;
        }

        /// <summary>
        /// Enables or disables heart beat
        /// </summary>
        public void SetHeartBeatEnabled(bool enable)
        {
            bool enableHeartbeat = false;
#if !UNITY_SERVER
            enableHeartbeat = enable;
#endif
            if(enableHeartbeat)
            {
                StartHeartBeatScheduler(heartBeatIntervalMs);
            }
            else
            {
                StopHeartBeatScheduler();
            }
        }

        /// <summary>
        /// set heart beat response callback
        /// </summary>
        public void SetHeartBeatCallback(ResultCallback callback)
        {
            onHeartBeatResponse = callback;
        }

        private void StartHeartBeatScheduler(int intervalMs)
        {
            if(maintainer != null)
            {
                maintainer.Stop();
            }
            maintainer = new HeartBeatMaintainer(this, intervalMs);
            maintainer.Start();
        }

        private void StopHeartBeatScheduler()
        {
            if(maintainer != null)
            {
                maintainer.Stop();
                maintainer = null;
            }
        }

        private class HeartBeatMaintainer
        {
            private readonly HeartBeat controller;

            private readonly int heartBeatIntervalMs;

            public bool IsHeartBeatEnabled
            {
                get;
                private set;
            }

            public bool IsHeartBeatJobRunning
            {
                get;
                private set;
            }

            public HeartBeatMaintainer(HeartBeat controller, int heartBeatIntervalMs)
            {
                this.controller = controller;
                this.heartBeatIntervalMs = heartBeatIntervalMs;
            }

            ~HeartBeatMaintainer()
            {
                Stop();
            }

            public void Start()
            {
                RunPeriodicHeartBeat();
            }

            public void Stop()
            {
                IsHeartBeatEnabled = false;
            }

            private async void RunPeriodicHeartBeat()
            {
                IsHeartBeatEnabled = true;
                IsHeartBeatJobRunning = true;

                while (IsHeartBeatEnabled)
                {
                    controller.api.SendHeartBeatEvent(controller.HeartBeatData, controller.OnHeartBeatResponse);
                    await System.Threading.Tasks.Task.Delay(this.heartBeatIntervalMs);
                }

                IsHeartBeatJobRunning = false;
            }
        }
    }
}