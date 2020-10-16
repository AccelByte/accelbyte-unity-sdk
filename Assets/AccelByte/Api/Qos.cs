// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Qos
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly QosManagerApi qosManager;

        internal Qos(QosManagerApi qosManager, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(qosManager, nameof(qosManager) + " is null.");
            Assert.IsNotNull(coroutineRunner, nameof(coroutineRunner) + " is null.");
            this.qosManager = qosManager;
            this.coroutineRunner = coroutineRunner;
        }

        public void GetServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            this.coroutineRunner.Run(GetServerLatenciesAsync(callback));
        }

        private IEnumerator GetServerLatenciesAsync(ResultCallback<Dictionary<string, int>> callback)
        {
            Result<QosServerList> getQosServersResult = null;

            yield return this.qosManager.GetQosServers(result => getQosServersResult = result);

            if (getQosServersResult.IsError)
            {
                callback.TryError(getQosServersResult.Error.Code);

                yield break;
            }

            var stopwatch = new Stopwatch();
            var latencies = new Dictionary<string, int>();

            foreach (QosServer server in getQosServersResult.Value.servers)
            {
                using (var udpClient = new UdpClient(server.port))
                {
                    udpClient.Connect(new IPEndPoint(IPAddress.Parse(server.ip), server.port));
                    byte[] sendBytes = Encoding.ASCII.GetBytes("PING");
                    stopwatch.Restart();
                    IAsyncResult asyncResult = udpClient.BeginSend(sendBytes, sendBytes.Length, null, null);

                    yield return new WaitUntil(() => asyncResult.IsCompleted);

                    udpClient.EndSend(asyncResult);
                    var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    asyncResult = udpClient.BeginReceive(null, null);

                    yield return new WaitUntil(() => asyncResult.IsCompleted);

                    udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                    latencies[server.region] = stopwatch.Elapsed.Milliseconds;
                }
            }

            callback.TryOk(latencies);
        }
    }
}