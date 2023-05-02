// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Core
{
    internal abstract class WebRequestScheduler
    {
        protected List<WebRequestTask> requestTask;

        internal abstract void StartScheduler();
        internal abstract void StopScheduler();

        private WebRequestTaskOrderComparer orderComparer = new WebRequestTaskOrderComparer();

        internal void AddTask(WebRequestTask newTask)
        {
            if(requestTask == null)
            {
                requestTask = new List<WebRequestTask>();
            }

            requestTask.Add(newTask);
            requestTask.Sort(orderComparer);
        }
    }
}