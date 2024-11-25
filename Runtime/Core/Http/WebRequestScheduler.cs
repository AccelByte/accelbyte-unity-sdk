// Copyright (c) 2021 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class WebRequestScheduler
    {
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

            using (UnityEngine.Networking.UnityWebRequest webRequest = task.CreateWebRequest())
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
                    Report.GetHttpRequest(task.HttpRequest, webRequest, logger);
                    var asyncOp = webRequest.SendWebRequest();
                    while (!asyncOp.isDone && !isTimeout)
                    {
                        await System.Threading.Tasks.Task.Yield();
                    }
                    Report.GetHttpResponse(webRequest, logger);
                    webRequestCancelTokenSource.Cancel();
                    task.SetComplete(webRequest);
                }
            }
        }
    }
}