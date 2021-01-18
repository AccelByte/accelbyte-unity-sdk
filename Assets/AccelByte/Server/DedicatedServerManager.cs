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
        public delegate void MatchRequestCallback(MatchRequest request);
        public delegate void HeartBeatErrorCallback(ServiceError error);

        /// <summary>
        /// This event will be raised if there is match request coming from heartbeat
        /// </summary>
        public event MatchRequestCallback OnMatchRequest;

        /// <summary>
        /// This event will be raised if heartbeat get error in sequence for several times.
        /// </summary>
        public event HeartBeatErrorCallback OnHeartBeatError;

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
        /// Register server ready to DSM
        /// </summary>
        /// <param name="isAutomatic">Heartbeat will automatically called periodically</param>
        /// <param name="timeoutSeconds">Timeout period the next heartbeat will be triggered</param>
        /// <param name="errorRetry"> Max consecutive heartbeat error, will stop send heartbeat when reach the number</param>
        public void ConfigureHeartBeat(bool isAutomatic = true, uint timeoutSeconds = 5, uint errorRetry = 5)
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.isHeartBeatAutomatic = isAutomatic;
            this.heartbeatTimeoutSeconds = timeoutSeconds;
            this.heartbeatErrorRetry = errorRetry;

            if (this.heartBeatCoroutine != null && !this.isHeartBeatAutomatic)
            {
                this.coroutineRunner.Stop(this.heartBeatCoroutine);
                this.heartBeatCoroutine = null;
            }
        }

        /// <summary>
        /// Call Heartbeat manually
        /// </summary>
        public void PollHeartBeat()
        {
            Report.GetFunctionLog(this.GetType().Name);

            this.coroutineRunner.Run(
                this.api.SendHeartBeat(
                    this.name,
                    this.serverSession.AuthorizationToken,
                    result =>
                    {
                        if (!result.IsError && result.Value != null)
                        {
                            this.OnMatchRequest?.Invoke(result.Value);
                        }
                    }));
        }

        private IEnumerator RunPeriodicHeartBeat()
        {
            while (this.isHeartBeatAutomatic)
            {
                yield return this.api.SendHeartBeat(
                    this.name,
                    this.serverSession.AuthorizationToken,
                    result =>
                    {
                        if (!result.IsError && result.Value != null)
                        {
                            this.OnMatchRequest?.Invoke(result.Value);
                        }else if(result.IsError)
                        {
                            if (((int)result.Error.Code >= 400 && (int)result.Error.Code < 500) || result.Error.Code == ErrorCode.DedicatedServerNotFound)
                            {
                                if (heartbeatRetryCount <= heartbeatErrorRetry)
                                {
                                    heartbeatRetryCount++;
                                }
                                else
                                {
                                    if (this.heartBeatCoroutine != null)
                                    {
                                        this.isHeartBeatAutomatic = false;
                                        this.coroutineRunner.Stop(this.heartBeatCoroutine);
                                        this.heartBeatCoroutine = null;
                                    }
                                    ServiceError error = new ServiceError { errorCode = (long)result.Error.Code, errorMessage = result.Error.Message };
                                    this.OnHeartBeatError?.Invoke(error);
                                }
                            }
                        }
                        else
                        {
                            heartbeatRetryCount = 0;
                        }
                    });

                yield return new WaitForSeconds(this.heartbeatTimeoutSeconds);
            }
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
            this.coroutineRunner.Run(this.api.RegisterServer(request, this.serverSession.AuthorizationToken, (Result result) =>
            {
                if (!result.IsError)
                {
                    if (this.isHeartBeatAutomatic && this.heartBeatCoroutine == null)
                    {
                        this.heartBeatCoroutine = this.coroutineRunner.Run(RunPeriodicHeartBeat());
                    }
                }

                callback.Try(result);
            }));
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
            this.coroutineRunner.Run(this.api.RegisterLocalServer(request, authToken, (Result result) =>
            {
                if (!result.IsError)
                {
                    if (this.isHeartBeatAutomatic && this.heartBeatCoroutine == null)
                    {
                        this.heartBeatCoroutine = this.coroutineRunner.Run(RunPeriodicHeartBeat());
                    }
                }

                callback.Try(result);
            }));
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
    }
}