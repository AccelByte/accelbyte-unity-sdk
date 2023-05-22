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
            do
            {
                if (requestTask != null && requestTask.Count > 0)
                {
                    if (requestTask[0].DelayMs > 0)
                    {
                        yield return new WaitForSeconds(Utils.TimeUtils.SecondsToMilliseconds(requestTask[0].DelayMs));
                    }
                    Report.GetHttpRequest(requestTask[0].HttpRequest, requestTask[0].WebRequest);
                    yield return requestTask[0].WebRequest.SendWebRequest();
                    Report.GetHttpResponse(requestTask[0].WebRequest);
                    requestTask[0].SetComplete();
                    requestTask[0].Dispose();
                    requestTask.RemoveAt(0);
                }
                yield return null;
            }
            while (isRunning);
        }
    }
}