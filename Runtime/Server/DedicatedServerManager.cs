﻿// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class DedicatedServerManager : WrapperBase
    {
        //Readonly members
        private readonly CoroutineRunner coroutineRunner;
        private readonly ISession session;
        private readonly DedicatedServerManagerApi api;

        private string serverName = "";
        private string matchSessionId = "";
        private Coroutine heartBeatCoroutine;
        private ResultCallback heartBeatCallback = null;
        private HeartBeatMaintainer maintainer;
        private int heartBeatIntervalMs;

        [UnityEngine.Scripting.Preserve]
        internal DedicatedServerManager( DedicatedServerManagerApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            coroutineRunner = inCoroutineRunner;
            session = inSession;
            api = inApi;

            AccelByteDebug.Log("Server init sessionAdapter: " + session.AuthorizationToken);
            AccelByteDebug.Log("Server init podName: " + serverName);
        }

        /// <summary>
        /// Get the registered server name.
        /// will be filled after server is registered.
        /// </summary>
        public string ServerName
        {
            get { return serverName; }
        }

        /// <summary>
        /// Register server to DSM and mark this machine as ready
        /// </summary>
        /// <param name="port">Exposed port number to connect to</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="customAttribute">A string value that will be sent to game client via DSNotif</param>
        public void RegisterServer( int portNumber
            , ResultCallback callback
            , string customAttribute = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                AccelByteDebug.Log("Server RegisterServer session is not valid");
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            serverName = Environment.GetEnvironmentVariable("POD_NAME");
            var request = new RegisterServerRequest 
            {
                pod_name = serverName, 
                port = portNumber, 
                custom_attribute = customAttribute,
            };
            
            coroutineRunner.Run(api.RegisterServer(request, callback));
        }

        /// <summary>
        /// Shutdown cloud server
        /// </summary>
        /// <param name="killMe">Signaling DSM to forcefully shutdown this machine</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void ShutdownServer( bool killMe
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (heartBeatCoroutine != null)
            {
                coroutineRunner.Stop(heartBeatCoroutine);
                heartBeatCoroutine = null;
            }
            
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var request = new ShutdownServerRequest
            {
                kill_me = killMe, pod_name = serverName, session_id = matchSessionId,
            };

            coroutineRunner.Run(api.ShutdownServer(request, callback));
        }

        /// <summary>
        /// Register local server to DSM and mark this machine as ready
        /// </summary>
        /// <param name="ip">Local IP Address</param>
        /// <param name="port">Port number</param>
        /// <param name="inName">Name to uniquely identify this local server</param>
        /// <param name="callback">Returns a Result via callback when completed</param>
        /// <param name="customAttribute">A string value that will be sent to game client via DSNotif</param>
        public void RegisterLocalServer( string ip
            , uint port
            , string inName
            , ResultCallback callback
            , string customAttribute = "" )
        {
            Report.GetFunctionLog(GetType().Name);

            Assert.IsNotNull(inName, "Can't Register server; podName is null!");

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            serverName = inName;
            var request = new RegisterLocalServerRequest
            {
                ip = ip, 
                port = port, 
                name = inName, 
                custom_attribute = customAttribute,
            };
            
            coroutineRunner.Run(api.RegisterLocalServer(request, callback));
        }

        [Obsolete("ipify supports will be deprecated in future releases, please use " +
            "RegisterLocalServer(string ip, uint port, string inName, ResultCallback callback)")]
        public void RegisterLocalServer( uint port
            , string inName
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            serverName = inName;

            coroutineRunner.Run(api.RegisterLocalServer(port, inName, callback));
        }

        /// <summary>
        /// Deregister local server from DSM
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed</param>
        public void DeregisterLocalServer( ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (heartBeatCoroutine != null)
            {
                coroutineRunner.Stop(heartBeatCoroutine);
                heartBeatCoroutine = null;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            coroutineRunner.Run(api.DeregisterLocalServer(serverName, callback));
        }

        /// <summary>
        /// Get Session ID of a claimed DS. will return empty session_id if DS is not claimed yet.
        /// DS is claimed when a party / player first joined a DS.
        /// </summary>
        /// <param name="callback">Returns a session ID via callback when completed</param>
        public void GetSessionId( ResultCallback<ServerSessionResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if(!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetSessionId(callback));
        }

        /// <summary>
        /// Get the session timeout that will be used for the DS.
        /// </summary>
        /// <param name="callback">Returns a session timeout via callback when completed</param>
        public void GetSessionTimeout( ResultCallback<ServerSessionTimeoutResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if(!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetSessionTimeout(callback));
        }

        /// <summary>
        /// Server heart beat
        /// </summary>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public void ServerHeartBeat(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ServerHeartBeat(result =>
            {
                callback?.Invoke(result);
            });
        }

        /// <summary>
        /// Set heart beat response callback
        /// </summary>
        public void SetHeartBeatEventCallback(ResultCallback callback)
        {
            heartBeatCallback = callback;
        }

        /// <summary>
        /// Set heart beat enabled
        /// </summary>
        public void SetHeartBeatEnabled(bool enabled, int intervalInMs = 0)
        {
            if (intervalInMs == 0)
            {
                intervalInMs = heartBeatIntervalMs;
            }
            if (enabled)
            {
                StartHeartBeatScheduler(intervalInMs);
            }
            else
            {
                StopHeartBeatScheduler();
            }
        }

        private void StartHeartBeatScheduler(int intervalMs)
        {
            if (maintainer != null)
            {
                maintainer.Stop();
            }
            maintainer = new HeartBeatMaintainer(intervalMs);
            maintainer.OnHeartBeatTrigger += () => api.ServerHeartBeat(heartBeatCallback);
            maintainer.Start();
        }

        private void StopHeartBeatScheduler()
        {
            if (maintainer != null)
            {
                maintainer.Stop();
                maintainer = null;
            }
        }

        private class HeartBeatMaintainer
        {
            private int intervalInMs = 0;
            internal Action OnHeartBeatTrigger;

            public bool IsHeartBeatEnabled
            {
                get;
                private set;
            }

            public bool IsHeartBeatJobRunning
            {
                get;
                private set;
            }

            public HeartBeatMaintainer(int heartBeatIntervalMs, ServerSession session = null)
            {
                this.intervalInMs = heartBeatIntervalMs;
            }

            public void Start()
            {
                RunPeriodicHeartBeat();
            }

            public void Stop()
            {
                IsHeartBeatEnabled = false;
            }

            private async void RunPeriodicHeartBeat()
            {
                IsHeartBeatEnabled = true;
                IsHeartBeatJobRunning = true;
                while (IsHeartBeatEnabled)
                {
                    OnHeartBeatTrigger.Invoke();
                    await System.Threading.Tasks.Task.Delay(this.intervalInMs);
                }
                IsHeartBeatJobRunning = false;
            }
        }
    }
}
