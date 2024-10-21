// Copyright (c) 2021 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using UnityEngine;

namespace AccelByte.Core
{
    internal class WebRequestSchedulerCoroutine : WebRequestScheduler
    {
        CoroutineRunner coroutineRunner;
        internal bool IsRunning
        {
            get;
            private set;
        }

        public WebRequestSchedulerCoroutine(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        internal override void StartScheduler()
        {
            if (!IsRunning && coroutineRunner != null)
            {
                coroutineRunner.Run(Update());
            }
        }

        internal override void StopScheduler()
        {
            IsRunning = false;
        }

        private IEnumerator Update()
        {
            IsRunning = true;
            int requestCount;
            WebRequestTask executedTask;
            do
            {
                executedTask = FetchTask();

                if (executedTask != null)
                {
                    if (executedTask.DelayMs > 0)
                    {
                        yield return new WaitForSeconds(Utils.TimeUtils.MillisecondsToSeconds(executedTask.DelayMs));
                    }

                    using (UnityEngine.Networking.UnityWebRequest webRequest = executedTask.CreateWebRequest())
                    {
                        Report.GetHttpRequest(executedTask.HttpRequest, webRequest, logger);
                        yield return ExecuteWebRequest(webRequest);
                        Report.GetHttpResponse(webRequest, logger);
                        executedTask.SetComplete(webRequest);
                    }
                }
                yield return null;

                requestCount = 0;
                if (requestTask != null)
                {
                    lock (requestTask)
                    {
                        requestCount = requestTask.Count;
                    }
                }
            }
            while (IsRunning && requestCount > 0);
            IsRunning = false;
        }

        internal virtual IEnumerator ExecuteWebRequest(UnityEngine.Networking.UnityWebRequest webRequest)
        {
            yield return webRequest.SendWebRequest();;
        }
    }
}