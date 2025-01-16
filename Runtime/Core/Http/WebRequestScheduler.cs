// Copyright (c) 2021 - 2024 AccelByte Inc. All Rights Reserved.
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

        private WebRequestTaskOrderComparer orderComparer = new WebRequestTaskOrderComparer();

        protected IDebugger logger;
        protected CoreHeartBeat heartBeat;
        private bool stopRequested;
        
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

            using (AccelByteWebRequest webRequest = task.CreateWebRequest())
            {
                using (var webRequestCancelTokenSource = new System.Threading.CancellationTokenSource())
                {
                    double timeoutSeconds = task.TimeoutMs / 1000;
                    bool isTimeout = false;
                    heartBeat.Wait(new WaitTimeCommand(waitTime: timeoutSeconds, cancellationToken: webRequestCancelTokenSource.Token, onDone: () =>
                    {
                        logger?.LogWarning($"{webRequest.method} {webRequest.uri} reached timeout");
                        webRequest?.Abort();
                        isTimeout = true;
                    }));
                    
                    webRequest.SentTimestamp = DateTime.UtcNow;
                    PreHttpRequest?.Invoke(webRequest, task.HttpRequest.Headers, task.HttpRequest.BodyBytes, logger);
                    
                    var asyncOp = webRequest.SendWebRequest();
                    while (!asyncOp.isDone && !isTimeout)
                    {
                        await System.Threading.Tasks.Task.Yield();
                    }
                    webRequest.ResponseTimestamp = DateTime.UtcNow;
                    PostHttpRequest?.Invoke(webRequest, logger);
                    
                    webRequestCancelTokenSource.Cancel();
                    task.SetComplete(webRequest);
                }
            }
        }
    }
}