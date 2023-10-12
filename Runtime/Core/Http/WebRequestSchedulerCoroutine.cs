// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using UnityEngine;

namespace AccelByte.Core
{
    internal class WebRequestSchedulerCoroutine : WebRequestScheduler
    {
        CoroutineRunner coroutineRunner;
        bool isRunning;

        public WebRequestSchedulerCoroutine(CoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
            StartScheduler();
        }

        ~WebRequestSchedulerCoroutine()
        {
            StopScheduler();
        }

        internal override void StartScheduler()
        {
            isRunning = true;
            coroutineRunner.Run(Update());
        }

        internal override void StopScheduler()
        {
            isRunning = false;
        }

        private IEnumerator Update()
        {
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
                        Report.GetHttpRequest(executedTask.HttpRequest, webRequest);
                        yield return webRequest.SendWebRequest();
                        Report.GetHttpResponse(webRequest);
                        executedTask.SetComplete(webRequest);
                    }
                }
                yield return null;
            }
            while (isRunning);
        }
    }
}