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
        
        ~WebRequestScheduler()
        {
            StopScheduler();
            CleanTask();
        }

        internal void AddTask(WebRequestTask newTask)
        {
            if(requestTask == null)
            {
                requestTask = new List<WebRequestTask>();
            }

            lock (requestTask)
            {
                requestTask.Add(newTask);
                requestTask.Sort(orderComparer);
            }

            StartScheduler();
        }

        internal WebRequestTask FetchTask()
        {
            if(requestTask == null)
            {
                return null;
            }

            WebRequestTask retval = null;
            lock (requestTask)
            {
                while(requestTask.Count > 0)
                {
                    if(requestTask[0].State == WebRequestState.Waiting)
                    {
                        break;
                    }
                    if (requestTask[0].State == WebRequestState.OnProcess)
                    {
                        // If a is task already running, skip fetching
                        return null;
                    }
                    else if(requestTask[0].State == WebRequestState.Complete)
                    {
                        requestTask.RemoveAt(0);
                    }
                }

                if (requestTask.Count > 0)
                {
                    retval = requestTask[0];
                    requestTask.RemoveAt(0);
                    retval.SetState(WebRequestState.OnProcess);
                }
            }

            return retval;
        }

        internal void CleanTask()
        {
            if (requestTask != null)
            {
                lock (requestTask)
                {
                    requestTask.Clear();
                }
            }
        }
    }
}