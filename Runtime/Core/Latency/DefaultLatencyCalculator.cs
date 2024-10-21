// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using AccelByte.Models;
using System.Collections;
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
        
        private CoreHeartBeat coreHeartBeat;
        private float timeOutSeconds = 60;

        private static Dictionary<int, List<CalculateLatencyTask>> portQueue;
        
        public DefaultLatencyCalculator(CoreHeartBeat coreHeartBeat, int timeOutSeconds = 60)
        {
            UnityEngine.Assertions.Assert.IsNotNull(coreHeartBeat);
            if (portQueue == null)
            {
                portQueue = new Dictionary<int, List<CalculateLatencyTask>>();
            }

            this.coreHeartBeat = coreHeartBeat;
            this.timeOutSeconds = timeOutSeconds;
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
                await CalculateLatency(nextTask.Url, nextTask.Port, nextTask.TaskResult);

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
        
        private async System.Threading.Tasks.Task CalculateLatency(string url, int port, AccelByteResult<int, Error> result)
        {
            var stopwatch = new Stopwatch();
            try
            {
                using (var udpClient = new UdpClient(port))
                {
                    udpClient.Connect(url, port);
                    byte[] sendBytes = Encoding.ASCII.GetBytes("PING");
                    stopwatch.Start();

                    using (var cts = new System.Threading.CancellationTokenSource())
                    {
                        System.Threading.Tasks.Task<int> sendPingTask = null;
                        coreHeartBeat.Wait(new WaitTimeCommand(timeOutSeconds, cancellationToken: cts.Token, onDone: () =>
                        {
                            udpClient.Close();
                        }));

                        IAsyncResult sendResult = udpClient.BeginSend(
                            sendBytes,
                            sendBytes.Length,
                            null,
                            null);

                        sendPingTask = System.Threading.Tasks.Task.Factory.FromAsync(sendResult, udpClient.EndSend);
                        await sendPingTask;
                    
                        var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                        IAsyncResult receiveResult = udpClient.BeginReceive(null, null);

                        Func<IAsyncResult, byte[]> receiveEndMethod = (asyncResult) =>
                        {
                            return udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                        };
                        await System.Threading.Tasks.Task.Factory.FromAsync(receiveResult, receiveEndMethod);

                        cts.Cancel();
                    }
                    
                    udpClient.Close();
                    result.Resolve(stopwatch.Elapsed.Milliseconds);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Encountered issue on calculating latency to \"{url}:{port}\".\n{ex.Message}";
                result.Reject(new Error(ErrorCode.InternalServerError, errorMessage));
            }
            stopwatch.Stop();
        }
    }
}

