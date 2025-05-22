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
        internal ILatencyCalculator DefaultLatencyCalculator;

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
        /// Fetch QoS server list
        /// </summary>
        public void FetchQosServers(ResultCallback<QosServerList> callback)
        {
            FetchQosServers(fetchOptionalParams: null, callback);
        }

        /// <summary>
        /// Fetch QoS server list
        /// </summary>
        public void FetchQosServers(FetchQosServerOptionalParameters fetchOptionalParams, ResultCallback<QosServerList> callback)
        {
            var apiOptionalParameter = new GetQosServerOptionalParameters();
            if (fetchOptionalParams == null)
            {
                fetchOptionalParams = new FetchQosServerOptionalParameters();
                fetchOptionalParams.Status = QosStatus.Active;
            }

            apiOptionalParameter.LatencyCalculator = fetchOptionalParams.LatencyCalculator;
            apiOptionalParameter.Status = fetchOptionalParams.Status;
            apiOptionalParameter.ApiTracker = fetchOptionalParams.ApiTracker;
            apiOptionalParameter.Logger = fetchOptionalParams.Logger;
            
            api.RequestGetQosServers(apiOptionalParameter, getQosServersResult =>
            {
                if (getQosServersResult.IsError)
                {
                    callback?.TryError(getQosServersResult.Error.Code);
                    return;
                }

                if (getQosServersResult.Value != null && getQosServersResult.Value.servers != null && getQosServersResult.Value.servers.Length > 0)
                {
                    if (fetchOptionalParams != null && fetchOptionalParams.LatencyCalculator != null)
                    {
                        foreach (QosServer qosServer in getQosServersResult.Value.servers)
                        {
                            qosServer.LatencyCalculator = fetchOptionalParams.LatencyCalculator;
                        }
                    }
                }
                
                callback?.Try(getQosServersResult);
            });
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
        internal void GetAllServerLatencies(GetAllServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            if (api.Config.EnableAmsServerQos)
            {
                var getServersOptionalParams = new GetQosServerOptionalParameters();
                if (optionalParams != null)
                {
                    getServersOptionalParams.LatencyCalculator = optionalParams.LatencyCalculator;
                    getServersOptionalParams.ApiTracker = optionalParams.ApiTracker;
                    getServersOptionalParams.Logger = optionalParams.Logger;
                }

                GetServerLatencies(getServersOptionalParams, callback);
            }
            else
            {
                api.RequestGetAllQosServers(optionalParams, getQosServersResult =>
                {
                    if (getQosServersResult.IsError)
                    {
                        callback?.TryError(getQosServersResult.Error.Code);
                        return;
                    }

                    if (getQosServersResult.Value != null && getQosServersResult.Value.servers != null && getQosServersResult.Value.servers.Length > 0)
                    {
                        if (optionalParams != null && optionalParams.LatencyCalculator != null)
                        {
                            foreach (QosServer qosServer in getQosServersResult.Value.servers)
                            {
                                qosServer.LatencyCalculator = optionalParams.LatencyCalculator;
                            }
                        }
                        AccelByte.Models.AccelByteResult<Dictionary<string, int>, Error> generateLatencyResult = getQosServersResult.Value.GenerateLatenciesMap(optionalParams?.Logger, useCache: false);
                        generateLatencyResult.OnSuccess(map =>
                        {
                            callback?.TryOk(map);
                        });
                        generateLatencyResult.OnFailed(error =>
                        {
                            callback?.TryError(error);
                        });
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
        [System.Obsolete("Deprecated, please use GetAllActiveServerLatencies instead. This interface will be removed in 2025.4.AGS versions.")]
        public void GetServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            GetServerLatencies(null, callback);
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetAllActiveServerLatencies(ResultCallback<Dictionary<string, int>> callback)
        {
            GetAllActiveServerLatencies(optionalParameters: null, callback);
        }
        
        internal void GetAllActiveServerLatencies(GetAllActiveServerLatenciesOptionalParameters optionalParameters, ResultCallback<Dictionary<string, int>> callback)
        {
            var getQoSServerOptionalParams = new GetQosServerOptionalParameters();
            getQoSServerOptionalParams.Status = QosStatus.Active;
            if (optionalParameters != null)
            {
                getQoSServerOptionalParams.Logger = optionalParameters.Logger;
                getQoSServerOptionalParams.ApiTracker = optionalParameters.ApiTracker;
            }
            
            GetServerLatencies(getQoSServerOptionalParams, callback);
        }

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="optionalParams">Optional parameter to modify the function</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        internal void GetServerLatencies(GetQosServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            FetchQosServerOptionalParameters fetchOptionalParams = null;
            if (optionalParams != null)
            {
                fetchOptionalParams = optionalParams.ConvertToFetchParameters();
            }

            if (fetchOptionalParams == null)
            {
                fetchOptionalParams = new FetchQosServerOptionalParameters();
            }

            FetchQosServers(fetchOptionalParams, fetchQosServerResult =>
            {
                if (fetchQosServerResult.IsError)
                {
                    callback?.TryError(fetchQosServerResult.Error.Code);
                    return;
                }
                
                if (fetchQosServerResult.Value != null && fetchQosServerResult.Value.servers != null && fetchQosServerResult.Value.servers.Length > 0)
                {
                    AccelByte.Models.AccelByteResult<Dictionary<string, int>, Error> generateLatencyResult = fetchQosServerResult.Value.GenerateLatenciesMap(optionalParams?.Logger, useCache: false);
                    generateLatencyResult.OnSuccess(map =>
                    {
                        callback?.TryOk(map);
                    });
                    generateLatencyResult.OnFailed(error =>
                    {
                        callback?.TryError(error);
                    });
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
            GetQosServerOptionalParameters optionalParameters = new GetQosServerOptionalParameters()
            {
                LatencyCalculator = DefaultLatencyCalculator
            };
            GetServerLatencies(optionalParameters, result =>
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
    }
}