// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerQosManager : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerQosManagerApi api;

        [UnityEngine.Scripting.Preserve]
        internal ServerQosManager( ServerQosManagerApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull( inApi, nameof( inApi ) + " is null.");
            Assert.IsNotNull( inCoroutineRunner, nameof( inCoroutineRunner ) + " is null.");
            
            api = inApi;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get all server latencies available
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetAllServerLatencies( ResultCallback<Dictionary<string, int>> callback )
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

        /// <summary>
        /// Get server latencies for active QoS server in the namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetServerLatencies( ResultCallback<Dictionary<string, int>> callback )
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

        private void GetServerLatenciesAsync(GetQosServerOptionalParameters optionalParams, ResultCallback<Dictionary<string, int>> callback)
        {
            GetQosServerOptionalParameters requestedOptionalParams = optionalParams;

            if (requestedOptionalParams == null)
            {
                requestedOptionalParams = new GetQosServerOptionalParameters();
                requestedOptionalParams.Status = QosStatus.Active;
            }

            api.RequestGetPublicQosServers(requestedOptionalParams, getQosServersResult =>
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
                string url = server.ip;
#if UNITY_WEBGL && !UNITY_EDITOR
                url = LatencyCalculatorFactory.GetAwsPingEndpoint(server.region);
#endif
                
                calculator.CalculateLatency(url, server.port)
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