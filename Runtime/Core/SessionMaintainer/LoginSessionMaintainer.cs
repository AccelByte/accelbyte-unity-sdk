// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Threading;
using AccelByte.Models;

namespace AccelByte.Core
{
    internal class LoginSessionMaintainer
    {
        public Func<AccelByteResult<TokenData, OAuthError>> RefreshSession {  get; set; }

        private CancellationTokenSource cts;

        private CoreHeartBeat sdkHeartbeat;
        private int maxRetries;
        private float initialBackoffDelay;
        private float maxBackoffDelay;
        private IDebugger logger;

        ~LoginSessionMaintainer()
        {
            Stop();
        }

        public void Start(
            CoreHeartBeat sdkHeartbeat
            , IDebugger logger
            , float refreshTimeSeconds
            , float intialBackOffDelaySeconds = 1f
            , float maxBackoffTimeSeconds = 3600f
            , int maxRetries = 10)
        {
            UnityEngine.Assertions.Assert.IsNotNull(sdkHeartbeat, "Expecting heartbeat not null");
            Stop();

            this.logger = logger;
            this.sdkHeartbeat = sdkHeartbeat;
            float initialDelay = refreshTimeSeconds;
            initialBackoffDelay = intialBackOffDelaySeconds;
            
            cts = new CancellationTokenSource();
            this.maxBackoffDelay = maxBackoffTimeSeconds;
            this.maxRetries = maxRetries;
            
            Action onWaitDone = () =>
            {
                RefreshSession()
                    .OnFailed((error) =>
                    {
                        logger?.LogWarning("Failed refreshing token, begin retry");
                        RunMaintainer(initialBackoffDelay, retry:0);
                    });
            };
            WaitTimeCommand cmd = new WaitTimeCommand(initialDelay, onWaitDone, cts.Token);
            sdkHeartbeat.Wait(cmd);
        }

        public void Stop()
        {
            try
            {
                cts?.Cancel();
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        private void RunMaintainer(float delay, int retry)
        {
            if (retry == maxRetries)
            {
                logger?.LogWarning($"Failed refreshing token, user timeout");
                return;
            }
            
            Action onWaitDone = () =>
            {
                RefreshSession()
                    .OnFailed((error) =>
                    {
                        retry++;
                        logger?.LogWarning($"Failed refreshing token, retry ({retry}/{maxRetries})");
                        float nextDelay = delay * 2f;
                        nextDelay = Math.Min(nextDelay, maxBackoffDelay);
                        RunMaintainer(nextDelay, retry);
                    });
            };
            WaitTimeCommand cmd = new WaitTimeCommand(delay, onWaitDone, cts.Token);
            sdkHeartbeat.Wait(cmd);
        }

    }
}