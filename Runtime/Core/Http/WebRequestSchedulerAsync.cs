// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Threading.Tasks;
using UnityEngine;

namespace AccelByte.Core
{
    internal class WebRequestSchedulerAsync : WebRequestScheduler
    {
        internal bool IsRunning
        {
            get;
            private set;
        }

        internal override void StartScheduler()
        {
            if (!IsRunning)
            {
                Update();
            }
        }

        internal override void StopScheduler()
        {
            IsRunning = false;
        }

        private async void Update()
        {
            IsRunning = true;
            WebRequestTask executedTask;
            int requestCount;
            do
            {
                executedTask = FetchTask();

                if (executedTask != null)
                {
                    if (executedTask.DelayMs > 0)
                    {
                        await Task.Delay((int)executedTask.DelayMs);
                    }

                    using (UnityEngine.Networking.UnityWebRequest webRequest = executedTask.CreateWebRequest())
                    {
                        Report.GetHttpRequest(executedTask.HttpRequest, webRequest, logger);
                        await ExecuteWebRequest(webRequest);
                        Report.GetHttpResponse(webRequest, logger);
                        executedTask.SetComplete(webRequest);
                    }
                }
                await AccelByteHttpHelper.HttpDelayOneFrame;

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

        internal virtual async Task ExecuteWebRequest(UnityEngine.Networking.UnityWebRequest webRequest)
        {
            await webRequest.SendWebRequest();
        }
    }
}