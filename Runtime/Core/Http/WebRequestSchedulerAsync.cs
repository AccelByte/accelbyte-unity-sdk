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
            do
            {
                if (requestTask != null && requestTask.Count > 0)
                {
                    if(requestTask[0].DelayMs > 0)
                    {
                        await Task.Delay((int)requestTask[0].DelayMs);
                    }
                    Report.GetHttpRequest(requestTask[0].HttpRequest, requestTask[0].WebRequest);
                    await requestTask[0].WebRequest.SendWebRequest();
                    Report.GetHttpResponse(requestTask[0].WebRequest);
                    requestTask[0].SetComplete();
                    requestTask.RemoveAt(0);
                }
                await AccelByteHttpHelper.HttpDelayOneFrame;
            }
            while (isRunning);
        }
    }
}