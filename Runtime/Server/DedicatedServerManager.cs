// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

//TODO: Do authentication using server creadentials

namespace AccelByte.Server
{
    public class DedicatedServerManager
    {
        //Readonly members
        private readonly CoroutineRunner coroutineRunner;
        private readonly IServerSession serverSession;
        private readonly string @namespace;
        private readonly DedicatedServerManagerApi api;

        private string name = "";
        private string matchSessionId = "";
        private bool isHeartBeatAutomatic;
        private float heartbeatTimeoutSeconds;
        private uint heartbeatErrorRetry;
        private Coroutine heartBeatCoroutine;
        private uint heartbeatRetryCount = 0;

        internal DedicatedServerManager(DedicatedServerManagerApi api, IServerSession serverSession, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null.");
            Assert.IsNotNull(serverSession, "session parameter can not be null");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction failed");

            this.coroutineRunner = coroutineRunner;
            this.serverSession = serverSession;
            this.api = api;

            AccelByteDebug.Log("Server init sessionAdapter: " + this.serverSession.AuthorizationToken);
            AccelByteDebug.Log("Server init podName: " + this.name);
        }

        /// <summary>
        /// Register server to DSM and mark this machine as ready
        /// </summary>
        /// <param name="port">Exposed port number to connect to</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RegisterServer(int portNumber, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.serverSession.IsValid())
            {
                AccelByteDebug.Log("Server RegisterServer session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.name = Environment.GetEnvironmentVariable("POD_NAME");
            var request = new RegisterServerRequest {pod_name = this.name, port = portNumber};
            this.coroutineRunner.Run(this.api.RegisterServer(request, this.serverSession.AuthorizationToken, callback));
        }

        /// <summary>
        /// Register server ready to DSM
        /// </summary>
        /// <param name="killMe">Signaling DSM to forcefully shutdown this machine</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ShutdownServer(bool killMe, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (this.heartBeatCoroutine != null)
            {
                this.coroutineRunner.Stop(this.heartBeatCoroutine);
                this.heartBeatCoroutine = null;
            }
            
            if (!this.serverSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            var request = new ShutdownServerRequest
            {
                kill_me = killMe, pod_name = this.name, session_id = this.matchSessionId
            };

            this.coroutineRunner.Run(this.api.ShutdownServer(request, this.serverSession.AuthorizationToken, callback));
        }

        /// <summary>
        /// Register local server to DSM and mark this machine as ready
        /// </summary>
        /// <param name="ip">Local IP Address</param>
        /// <param name="port">Port number</param>
        /// <param name="name">Name to uniquely identify this local server</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void RegisterLocalServer(string ip, uint port, string name, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            Assert.IsNotNull(name, "Can't Register server; podName is null!");

            if (!this.serverSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.name = name;
            var request = new RegisterLocalServerRequest {ip = ip, port = port, name = name};
            string authToken = this.serverSession.AuthorizationToken;
            this.coroutineRunner.Run(this.api.RegisterLocalServer(request, authToken, callback));
        }

        [Obsolete("ipify supports will be deprecated in future releases, please use RegisterLocalServer(string ip, uint port, string name, ResultCallback callback)")]
        public void RegisterLocalServer(uint port, string name, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            string accessToken = this.serverSession.AuthorizationToken;
            this.coroutineRunner.Run(this.api.RegisterLocalServer(port, name, this.serverSession.AuthorizationToken, callback));
        }

        /// <summary>
        /// Deregister local server from DSM
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeregisterLocalServer(ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (this.heartBeatCoroutine != null)
            {
                this.coroutineRunner.Stop(this.heartBeatCoroutine);
                this.heartBeatCoroutine = null;
            }

            if (!this.serverSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }
            
            string authToken = this.serverSession.AuthorizationToken;
            this.coroutineRunner.Run(this.api.DeregisterLocalServer(this.name, authToken, callback));
        }

        /// <summary>
        /// Get Session ID of a claimed DS. will return empty session_id if DS is not claimed yet.
        /// DS is claimed when a party / player first joined a DS.
        /// </summary>
        /// <param name="callback">Returns a session ID via callback when completed</param>
        public void GetSessionId(ResultCallback<ServerSessionResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if(!this.serverSession.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            string authToken = this.serverSession.AuthorizationToken;
            this.coroutineRunner.Run(this.api.GetSessionId(authToken, callback));
        }
    }
}