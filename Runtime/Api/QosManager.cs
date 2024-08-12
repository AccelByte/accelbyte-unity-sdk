// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
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

        private AccelByteMessagingSystem messagingSystem;

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

        /// <summary>
        /// Get all server latencies available
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetAllServerLatencies( ResultCallback<Dictionary<string, int>> callback )
        {
            Result<QosServerList> getQosServersResult = null;

            api.RequestGetAllQosServers(result =>
            {
                getQosServersResult = result;
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
        public void GetServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            GetServerLatenciesAsync(callback);
        }

        internal virtual void GetServerLatenciesAsync(ResultCallback<Dictionary<string, int>> callback)
        {
            Result<QosServerList> getQosServersResult = null;

            api.RequestGetQosServers(result =>
            {
                getQosServersResult = result;
                if (getQosServersResult.IsError)
                {
                    callback?.TryError(getQosServersResult.Error.Code);
                    return;
                }
                CalculateServerLatencies(getQosServersResult.Value.servers, callback);
            });
        }

        internal override void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            messagingSystem?.UnsubscribeToTopic(AccelByteMessagingTopic.LobbyConnected, OnLobbyConnectedHandle);
            base.SetSharedMemory(newSharedMemory);

            messagingSystem = newSharedMemory.MessagingSystem;
            messagingSystem?.SubscribeToTopic(AccelByteMessagingTopic.LobbyConnected, OnLobbyConnectedHandle);
        }

        private void OnLobbyConnectedHandle(string payload)
        {
            GetServerLatencies(result =>
            {
                if (result.IsError)
                {
                    return;
                }

                string closestRegion = GetClosestRegion(result.Value);
                messagingSystem?.SendMessage(AccelByteMessagingTopic.QosRegionLatenciesUpdated, closestRegion);
            });
        }

        private string GetClosestRegion(Dictionary<string, int> listRegion)
        {
            if (listRegion.Count == 0)
            {
                AccelByteDebug.LogWarning(
                    $"No region servers found.");
                return string.Empty;
            }

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