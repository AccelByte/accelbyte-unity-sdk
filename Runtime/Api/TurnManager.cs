// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// This endpoint lists all TURN server available in all regions.
    /// </summary>
    public class TurnManager : WrapperBase
    {
        private readonly TurnManagerApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal TurnManager(TurnManagerApi inApi, UserSession inSession, CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner parameter can not be null");

            this.api = inApi;
            this.session = inSession;
            this.coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Get List of TURN Server(s).
        /// </summary>
        /// <param name="callback">Return list of turn server</param>
        public void GetTurnServers(ResultCallback<TurnServerList> callback)
        {
            GetTurnServers(null, callback);
        }

        /// <summary>
        /// Get List of TURN Server(s).
        /// </summary>
        /// <param name="optionalParam">Optional parameter to modify the function</param>
        /// <param name="callback">Return list of turn server</param>
        public void GetTurnServers(GetTurnServerOptionalParameters optionalParam, 
            ResultCallback<TurnServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RequestGetTurnServers(getResult =>
            {
                if (getResult.IsError)
                {
                    callback?.TryError(getResult.Error.Code);
                    return;
                }

                bool autoCalculateLatency = true;
                if (optionalParam != null)
                {
                    autoCalculateLatency = optionalParam.AutoCalculateLatency;
                }
                
                if (autoCalculateLatency && getResult.Value != null && getResult.Value.servers != null && getResult.Value.servers.Length > 0)
                {
                    int currentLatencyTaskCalculationDoneCount = 0;
                    int expectedLength = getResult.Value.servers.Length;
                    
                    Action onCalculateLatencyDone = () =>
                    {
                        currentLatencyTaskCalculationDoneCount++;
                        if (currentLatencyTaskCalculationDoneCount == expectedLength)
                        {
                            callback?.Try(getResult);
                        }
                    };
                    
                    foreach (var server in getResult.Value.servers)
                    {
                        ILatencyCalculator calculator =
                            optionalParam?.LatencyCalculator == null ? LatencyCalculatorFactory.CreateDefaultCalculator() : optionalParam?.LatencyCalculator;
                        server.LatencyCalculator = calculator;
                        server.GetLatency(useCache: false).OnComplete(onCalculateLatencyDone);
                    }
                }
                else
                {
                    callback?.Try(getResult);
                }
            });
        }

        /// <summary>
        /// Get the closest TURN server
        /// </summary>
        /// <param name="callback">callback to trigger with operation result</param>
        [Obsolete("This method is deprecated and will be removed on 3.81 release. Please use " +
            "GetTurnServers(ResultCallback<TurnServerList> callback) " + 
            "and call GetClosestTurnServer() from callback's model.")]
        public void GetClosestTurnServer(ResultCallback<TurnServer> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            GetTurnServers((getTurnServersResult) =>
            {
                if (getTurnServersResult.IsError)
                {
                    callback?.TryError(getTurnServersResult.Error.Code);
                    return;
                }

                AccelByteResult<TurnServer, Error> closestTurnServerTask = getTurnServersResult.Value.GetClosestTurnServer();
                closestTurnServerTask.OnSuccess(closestTurnServer =>
                {
                    callback?.TryOk(closestTurnServer); 
                });
                closestTurnServerTask.OnFailed(error =>
                {
                    callback?.TryError(error);
                });
            });
        }

        /// <summary>
        /// Get the turn server credential
        /// </summary>
        /// <param name="region">region of selected turn server</param>
        /// <param name="ip">ip of selected turn server</param>
        /// <param name="port">port of selected turn server</param>
        /// <param name="callback">Return turn server credential</param>
        public void GetTurnServerCredential(string region
            , string ip
            , int port
            , ResultCallback<TurnServerCredential> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RequestGetTurnServerCredential(region, ip, port, callback);
        }

        /// <summary>
        /// Send connected metric
        /// </summary>
        /// <param name="turnServerRegion">Region of the selected turn server</param>
        /// <param name="connectionType">P2P connection type</param>
        /// <param name="callback">callback to trigger with operation result</param>
        public void SendMetric(string turnServerRegion, P2PConnectionType connectionType, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RequestSendMetric(turnServerRegion, connectionType, callback);
        }

        /// <summary>
        /// Send disconnected metric
        /// </summary>
        /// <param name="callback">callback to trigger with operation result</param>
        public void SendDisconnectedMetric(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.RequestDisconnect(callback);
        }
    }
}
