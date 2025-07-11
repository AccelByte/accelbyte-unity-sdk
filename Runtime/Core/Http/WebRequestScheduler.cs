﻿// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class WebRequestScheduler
    {
        public System.Action<AccelByteWebRequest, IDictionary<string ,string>, byte[], IDebugger> PreHttpRequest;
        public System.Action<AccelByteWebRequest, IDebugger> PostHttpRequest;
        protected List<WebRequestTask> requestTask;

        internal static Utils.IApiTracker GlobalApiTracker;

        protected IDebugger logger;
        protected CoreHeartBeat heartBeat;
        private bool stopRequested;

        private WebRequestTaskOrderComparer orderComparer = new WebRequestTaskOrderComparer();
        
        public WebRequestScheduler()
        {
            stopRequested = false;
        }

        public void Stop()
        {
            stopRequested = true;
        }

        public void SetLogger(IDebugger newLogger)
        {
            this.logger = newLogger;
        }

        public void SetHeartBeat(CoreHeartBeat heartBeat)
        {
            UnityEngine.Assertions.Assert.IsNotNull(heartBeat);
            this.heartBeat = heartBeat;
        }

        internal async void ExecuteWebTask(WebRequestTask task)
        {
            if (stopRequested)
            {
                return;
            }
            
            if (task.DelayMs > 0)
            {
                bool waitDone = false;
                heartBeat.Wait(new WaitTimeCommand(task.DelayMs / 1000, 
                    cancellationToken: new System.Threading.CancellationTokenSource().Token,
                    onDone: () =>
                {
                    waitDone = true;
                }));
                while (!waitDone)
                {
                    await System.Threading.Tasks.Task.Yield();
                }
            }

            IDebugger targetLogger = null;
            if (task.AdditionalHttpParameters.Logger != null)
            {
                targetLogger = task.AdditionalHttpParameters.Logger;
            }
            
            if (targetLogger == null)
            {
                targetLogger = this.logger;
            }

            using (AccelByteWebRequest webRequest = task.CreateWebRequest())
            {
                using (var webRequestCancelTokenSource = new System.Threading.CancellationTokenSource())
                {
                    double timeoutSeconds = task.TimeoutMs / 1000;
                    bool isTimeout = false;
                    heartBeat.Wait(new WaitTimeCommand(waitTime: timeoutSeconds, cancellationToken: webRequestCancelTokenSource.Token, onDone: () =>
                    {
                        targetLogger?.LogWarning($"{webRequest.method} {webRequest.uri} reached timeout");
                        webRequest?.Abort();
                        isTimeout = true;
                    }));

                    task.HttpRequest.Timestamp = DateTime.UtcNow;
                    webRequest.SentTimestamp = DateTime.UtcNow;

                    if (GlobalApiTracker != null)
                    {
                        GlobalApiTracker.NewHttpRequestSent(task.HttpRequest);
                    }
                    
                    if (task.AdditionalHttpParameters.ApiTracker != null)
                    {
                        task.AdditionalHttpParameters.ApiTracker.NewHttpRequestSent(task.HttpRequest);
                    }
                    
                    PreHttpRequest?.Invoke(webRequest, task.HttpRequest.Headers, task.HttpRequest.BodyBytes, targetLogger);
                    
                    var asyncOp = webRequest.SendWebRequest();
                    while (!asyncOp.isDone && !isTimeout)
                    {
                        await System.Threading.Tasks.Task.Yield();
                    }
                    webRequest.ResponseTimestamp = DateTime.UtcNow;
                    PostHttpRequest?.Invoke(webRequest, targetLogger);
                    
                    webRequestCancelTokenSource.Cancel();
                    task.SetComplete(webRequest);
                }
            }
        }
    }
}