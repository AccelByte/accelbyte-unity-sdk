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

        private AccelByteHeartBeat maintainer;
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

        public Dictionary<string, object> HeartBeatData 
        {
            get
            {
                return heartBeatData;
            }
        }

        public int HeartBeatIntervalMs
        {
            get
            {
                return heartBeatIntervalMs;
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal HeartBeat(
            HeartBeatApi inApi
            , ISession inLoginSession
            , CoroutineRunner inCoroutineRunner)
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
            maintainer = new AccelByteHeartBeat(intervalMs);
            maintainer.OnHeartbeatTrigger += () =>
            {
                api.SendHeartBeatEvent(HeartBeatData, onHeartBeatResponse);
            };
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
    }
}