// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
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

        /// <summary>
        /// Get all server latencies available
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetAllServerLatencies( ResultCallback<Dictionary<string, int>> callback )
        {
            qosManager.RequestGetAllQosServers(result =>
            {
                Result<QosServerList> getQosServersResult = result;
                if (getQosServersResult.IsError)
                {
                    callback?.TryError(getQosServersResult.Error.Code);
                    return;
                }
                CalculateServerLatencies(getQosServersResult.Value.servers, callback);
            });
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetServerLatencies( ResultCallback<Dictionary<string, int>> callback )
        {
            qosManager.RequestGetQosServers(result =>
            {
                Result<QosServerList> getQosServersResult = result;
                if (getQosServersResult.IsError)
                {
                    callback?.TryError(getQosServersResult.Error.Code);
                    return;
                }
                CalculateServerLatencies(getQosServersResult.Value.servers, callback);
            });
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
        
        private void CalculateServerLatencies(QosServer[] servers, ResultCallback<Dictionary<string, int>> callback)
        {
            var stopwatch = new Stopwatch();
            var latencies = new Dictionary<string, int>();

            foreach (QosServer server in servers)
            {
                try
                {
                    using (var udpClient = new UdpClient(server.port))
                    {
                        udpClient.Connect(server.ip, server.port);
                        byte[] sendBytes = Encoding.ASCII.GetBytes("PING");
                        stopwatch.Restart();
                    
                        IAsyncResult asyncResult = udpClient.BeginSend(
                            sendBytes, 
                            sendBytes.Length, 
                            null, 
                            null);
                        
                        udpClient.EndSend(asyncResult);
                        var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                       
                        asyncResult = udpClient.BeginReceive(null, null);
                        udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                        latencies[server.region] = stopwatch.Elapsed.Milliseconds;
                    }
                }
                catch (Exception ex)
                {
                    AccelByteDebug.LogWarning($"Encounter issue on calculating serevr latency. {ex.Message}");
                }
            }

            callback?.TryOk(latencies);
        }
    }
}