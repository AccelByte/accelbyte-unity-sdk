// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerQosManager : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerQosManagerApi qosManager;

        [UnityEngine.Scripting.Preserve]
        internal ServerQosManager( ServerQosManagerApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull( inApi, nameof( inApi ) + " is null.");
            Assert.IsNotNull( inCoroutineRunner, nameof( inCoroutineRunner ) + " is null.");
            
            qosManager = inApi;
            coroutineRunner = inCoroutineRunner;
        }

        private IEnumerator CalculateServerLatencies(QosServer[] servers, ResultCallback<Dictionary<string, int>> callback)
        {
            var stopwatch = new Stopwatch();
            var latencies = new Dictionary<string, int>();

            foreach (QosServer server in servers)
            {
                using (var udpClient = new UdpClient(server.port))
                {
                    udpClient.Connect(new IPEndPoint(IPAddress.Parse(server.ip), server.port));
                    byte[] sendBytes = Encoding.ASCII.GetBytes("PING");
                    stopwatch.Restart();
                    IAsyncResult asyncResult = udpClient.BeginSend(sendBytes, sendBytes.Length, null, null);

                    yield return WaitUntil(() => asyncResult.IsCompleted, 15 * 1000);

                    udpClient.EndSend(asyncResult);
                    var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    asyncResult = udpClient.BeginReceive(null, null);

                    yield return WaitUntil(() => asyncResult.IsCompleted, 15 * 1000);
                    if (!asyncResult.IsCompleted)
                    {
                        AccelByteDebug.Log($"[QOS] timeout to PING {server.ip}");
                    }

                    udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                    latencies[server.region] = stopwatch.Elapsed.Milliseconds;
                }
            }

            callback.TryOk(latencies);
        }

        /// <summary>
        /// Get all server latencies available
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetAllServerLatencies( ResultCallback<Dictionary<string, int>> callback )
        {
            coroutineRunner.Run(GetAllServerLatenciesAsync(callback));
        }

        private IEnumerator GetAllServerLatenciesAsync( ResultCallback<Dictionary<string, int>> callback )
        {
            Result<QosServerList> getQosServersResult = null;

            yield return qosManager.GetAllQosServers(result => getQosServersResult = result);

            if (getQosServersResult.IsError)
            {
                callback.TryError(getQosServersResult.Error.Code);

                yield break;
            }

            yield return CalculateServerLatencies(getQosServersResult.Value.servers, callback);
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetServerLatencies( ResultCallback<Dictionary<string, int>> callback )
        {
            coroutineRunner.Run(GetServerLatenciesAsync(callback));
        }

        private IEnumerator GetServerLatenciesAsync( ResultCallback<Dictionary<string, int>> callback )
        {
            Result<QosServerList> getQosServersResult = null;

            yield return qosManager.GetQosServers(result => getQosServersResult = result);

            if (getQosServersResult.IsError)
            {
                callback.TryError(getQosServersResult.Error.Code);

                yield break;
            }

            yield return CalculateServerLatencies(getQosServersResult.Value.servers, callback);
        }
        
        private static IEnumerator WaitUntil( Func<bool> condition
            , int timeoutMs )
        {
            while (!condition.Invoke() && timeoutMs > 0)
            {
                Thread.Sleep(100);
                timeoutMs -= 100;
                yield return null;
            }
        }
    }
}