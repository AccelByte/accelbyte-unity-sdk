// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// QoS == Quality of Service (Latencies, Pings, Regions, etc)
    /// </summary>
    public class QosManager : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly QosManagerApi api;

        [UnityEngine.Scripting.Preserve]
        internal QosManager( QosManagerApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, nameof(inApi) + " is null.");
            Assert.IsNotNull(inCoroutineRunner, nameof(inCoroutineRunner) + " is null.");
            
            api = inApi;
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
                    
                    IAsyncResult asyncResult = udpClient.BeginSend(
                        sendBytes, 
                        sendBytes.Length, 
                        null, 
                        null);

                    yield return new WaitUntil(() => asyncResult.IsCompleted);

                    udpClient.EndSend(asyncResult);
                    var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    asyncResult = udpClient.BeginReceive(null, null);

                    yield return new WaitUntil(() => asyncResult.IsCompleted);
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

        internal virtual IEnumerator GetAllServerLatenciesAsync( ResultCallback<Dictionary<string, int>> callback )
        {
            Result<QosServerList> getQosServersResult = null;

            yield return api.GetAllQosServers(result => getQosServersResult = result);

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
        public void GetServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            coroutineRunner.Run(GetServerLatenciesAsync(callback));
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            base.SetSharedMemory(newSharedMemory);
            SharedMemory.OnLobbyConnected += HandleOnLobbyConnected;
        }
        

        private IEnumerator GetServerLatenciesAsync(ResultCallback<Dictionary<string, int>> callback)
        {
            Result<QosServerList> getQosServersResult = null;

            yield return api.GetQosServers(result => getQosServersResult = result);

            if (getQosServersResult.IsError)
            {
                callback.TryError(getQosServersResult.Error.Code);

                yield break;
            }

            yield return CalculateServerLatencies(getQosServersResult.Value.servers, callback);
        }

        private void HandleOnLobbyConnected()
        {
            GetAllServerLatencies(result =>
            {
                if(result.IsError)
                {
                    return;
                }

                SharedMemory.ClosestRegion = GetClosestRegion(result.Value);
            });
        }

        private string GetClosestRegion(Dictionary<string, int> listRegion)
        {
            var closestRegion = listRegion.First();
            foreach (var serverLatency in listRegion)
            {
                if (serverLatency.Value < closestRegion.Value)
                {
                    closestRegion = serverLatency;
                }
            }
            return closestRegion.Key;
        }
    }
}