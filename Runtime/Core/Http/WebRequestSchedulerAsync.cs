// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Threading.Tasks;

namespace AccelByte.Core
{
    internal class WebRequestSchedulerAsync : WebRequestScheduler
    {
        bool isRunning;

        public WebRequestSchedulerAsync()
        {
            StartScheduler();
        }

        internal override void StartScheduler()
        {
            isRunning = true;
            Update();
        }

        internal override void StopScheduler()
        {
            isRunning = false;
        }

        private async void Update()
        {
            WebRequestTask executedTask;
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
                        Report.GetHttpRequest(executedTask.HttpRequest, webRequest);
                        await webRequest.SendWebRequest();
                        Report.GetHttpResponse(webRequest);
                        executedTask.SetComplete(webRequest);
                    }
                }
                await AccelByteHttpHelper.HttpDelayOneFrame;
            }
            while (isRunning);
        }
    }
}