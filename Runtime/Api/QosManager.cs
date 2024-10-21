// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
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
        internal QosManager(QosManagerApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
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
        public void GetAllServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            GetAllServerLatencies(null, callback);
        }

        /// <summary>
        /// Get all server latencies available
        /// </summary>
        /// <param name="optionalParams">Optional parameter to modify the function</param> 
        /// <param name="callback">Returns a result via callback when completed</param>
        internal void GetAllServerLatencies(GetQosServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (api.Config.EnableAmsServerQos)
            {
                GetQosServerOptionalParameters requestedOptionalParams = 
                    optionalParams == null ? new GetQosServerOptionalParameters() : optionalParams;

                GetServerLatenciesAsync(requestedOptionalParams, callback);
            }
            else
            {
                api.RequestGetAllQosServers(getQosServersResult =>
                {
                    if (getQosServersResult.IsError)
                    {
                        callback?.TryError(getQosServersResult.Error.Code);
                        return;
                    }

                    if (getQosServersResult.Value != null && getQosServersResult.Value.servers != null && getQosServersResult.Value.servers.Length > 0)
                    {
                        CalculateServerLatencies(getQosServersResult.Value.servers,
                            optionalParams?.LatencyCalculator,
                            callback);
                    }
                    else
                    {
                        callback?.TryOk(new Dictionary<string, int>());
                    }
                });
            }
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            GetServerLatencies(null, callback);
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        internal void GetServerLatencies(GetQosServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            GetServerLatenciesAsync(optionalParams, callback);
        }

        internal virtual void GetServerLatenciesAsync(GetQosServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            GetQosServerOptionalParameters requestedOptionalParams = optionalParams;

            if (requestedOptionalParams == null)
            {
                requestedOptionalParams = new GetQosServerOptionalParameters();
                requestedOptionalParams.Status = QosStatus.Active;
            }

            api.RequestGetQosServers(requestedOptionalParams, getQosServersResult =>
            {
                if (getQosServersResult.IsError)
                {
                    callback?.TryError(getQosServersResult.Error.Code);
                    return;
                }

                if (getQosServersResult.Value != null && getQosServersResult.Value.servers != null && getQosServersResult.Value.servers.Length > 0)
                {
                    CalculateServerLatencies(getQosServersResult.Value.servers,
                        optionalParams?.LatencyCalculator,
                        callback);
                }
                else
                {
                    callback?.TryOk(new Dictionary<string, int>());
                }
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
                SharedMemory?.Logger?.LogWarning($"No region servers found.");
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

        private void CalculateServerLatencies(QosServer[] servers,
            ILatencyCalculator latencyCalculator,
            ResultCallback<Dictionary<string, int>> callback)
        {
            int currentLatencyTaskCalculationDoneCount = 0;
            int expectedLength = servers.Length;

            Dictionary<string, int> retval = new Dictionary<string, int>();
            ILatencyCalculator calculator =
                latencyCalculator == null ? LatencyCalculatorFactory.CreateDefaultCalculator() : latencyCalculator;
            
            foreach (var server in servers)
            {
                calculator.CalculateLatency(server.ip, server.port)
                    .OnSuccess(latency =>
                    {
                        retval.Add(server.region, latency);
                    })
                    .OnComplete(() =>
                    {
                        currentLatencyTaskCalculationDoneCount++;
                        if (currentLatencyTaskCalculationDoneCount == expectedLength)
                        {
                            callback?.TryOk(retval);
                        }
                    });
            }
        }
    }
}