// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

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
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetTurnServers(callback));
        }

        /// <summary>
        /// Get the closest TURN server
        /// </summary>
        /// <param name="callback">callback to trigger with operation result</param>
        public void GetClosestTurnServer(ResultCallback<TurnServer> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(GetClosestTurnServerAsync(callback));
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
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetTurnServerCredential(region, ip, port, callback));
        }

        /// <summary>
        /// Send connected metric
        /// </summary>
        /// <param name="turnServerRegion">Region of the selected turn server</param>
        /// <param name="connectionType">P2P connection type</param>
        /// /// <param name="callback">callback to trigger with operation result</param>
        public void SendMetric(string turnServerRegion, P2PConnectionType connectionType, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.SendMetric(turnServerRegion, connectionType, callback));
        }

        private IEnumerator GetClosestTurnServerAsync(ResultCallback<TurnServer> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            Result<TurnServerList> getResult = null;
            yield return api.GetTurnServers(result => getResult = result);

            if (getResult.IsError)
            {
                AccelByte.Core.AccelByteDebug.LogWarning("TURN Manager error on getting closest TURN server");
                callback.TryError(getResult.Error.Code);
                yield break;
            }

            var stopwatch = new Stopwatch();
            TurnServer currentClosestServer = null;
            int fastestPingMiliseconds = 1000;
            
            foreach (TurnServer server in getResult.Value.servers)
            {
                using (var udpClient = new UdpClient())
                {
                    var turnServerEndpoint = new IPEndPoint(IPAddress.Parse(server.ip), server.qos_port);
                    udpClient.Connect(turnServerEndpoint);
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

                    udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                    if (fastestPingMiliseconds > stopwatch.Elapsed.Milliseconds)
                    {
                        fastestPingMiliseconds = stopwatch.Elapsed.Milliseconds;
                        currentClosestServer = server;
                    }
                }
            }

            if (currentClosestServer == null)
            {
                callback.TryError(ErrorCode.InternalServerError);
            }
            else
            {
                callback.TryOk(currentClosestServer);
            }
        }
    }
}
