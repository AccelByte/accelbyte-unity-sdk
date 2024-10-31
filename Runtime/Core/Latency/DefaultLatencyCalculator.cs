// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class DefaultLatencyCalculator : ILatencyCalculator
    {
        private struct CalculateLatencyTask
        {
            public int Port;
            public string Url;
            public AccelByteResult<int, Error> TaskResult;
        }
        
        private float timeOutSeconds;
        private uint pingRetry;

        private static Dictionary<int, List<CalculateLatencyTask>> portQueue;
        
        public DefaultLatencyCalculator(int timeOutSeconds = 10, uint pingRetry = 6)
        {
            if (portQueue == null)
            {
                portQueue = new Dictionary<int, List<CalculateLatencyTask>>();
            }

            this.timeOutSeconds = timeOutSeconds;
            this.pingRetry = pingRetry;
        }
        
        public AccelByteResult<int, Error> CalculateLatency(string url, int port)
        {
            var result = new AccelByteResult<int, Error>();
            var newTask = new CalculateLatencyTask() { Port = port, Url = url, TaskResult = result };

            lock (portQueue)
            {
                bool newPort = false;
                if (!portQueue.ContainsKey(port))
                {
                    portQueue.Add(port, new List<CalculateLatencyTask>());
                    newPort = true;
                }

                portQueue[port].Add(newTask);
                if (newPort)
                {
                    StartPortQueueController(port);
                }
            }

            return result;
        }

        private async void StartPortQueueController(int port)
        {
            bool hasNextTask = false;
            do
            {
                CalculateLatencyTask nextTask;
                lock (portQueue)
                {
                    nextTask = portQueue[port][0];
                    portQueue[port].RemoveAt(0);   
                }

                var optionalParameters = new Utils.Networking.PingOptionalParameters();
                optionalParameters.InTimeOutInMs = (uint)timeOutSeconds * 1000;
                optionalParameters.MaxRetry = pingRetry;
#if !UNITY_WEBGL || UNITY_EDITOR
                AccelByteResult<int, Error> pingResult = AccelByte.Utils.Networking.UdpPing(nextTask.Url, (uint) nextTask.Port, optionalParameters);
#else
                AccelByteResult<int, Error> pingResult = AccelByte.Utils.Networking.HttpPing(nextTask.Url, optionalParameters);
#endif
                bool pingComplete = false;

                pingResult.OnSuccess(ping =>
                {
                    nextTask.TaskResult.Resolve(ping);
                });
                pingResult.OnFailed(error =>
                {
                    nextTask.TaskResult.Reject(error);
                });
                pingResult.OnComplete(() =>
                {
                    pingComplete = true;
                });
                
                while (!pingComplete)
                {
                    await System.Threading.Tasks.Task.Yield();
                }
                
                lock (portQueue)
                {
                    hasNextTask = portQueue[port].Count > 0;
                    if (!hasNextTask)
                    {
                        portQueue.Remove(port);
                    }
                }
            } 
            while (hasNextTask);
        }
    }
}

