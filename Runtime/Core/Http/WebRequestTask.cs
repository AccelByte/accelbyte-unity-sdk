// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    internal class WebRequestTask : IDisposable
    {
        public IHttpRequest HttpRequest
        {
            get;
            private set;
        }
        
        public UnityWebRequest WebRequest
        {
            get;
            private set;
        }

        public System.Action<UnityWebRequest> OnComplete;

        public uint DelayMs
        {
            get;
            private set;
        }

        public int Priority
        {
            get
            {
                return HttpRequest.Priority;
            }
        }

        public DateTime CreatedTimeStamp
        {
            get;
            private set;
        }

        public WebRequestTask(IHttpRequest httpRequest, UnityWebRequest webRequest, uint delayMs)
        {
            HttpRequest = httpRequest;
            WebRequest = webRequest;
            CreatedTimeStamp = DateTime.UtcNow;
        }

        public void SetComplete()
        {
            OnComplete?.Invoke(WebRequest);
        }

        public void Dispose()
        {
            WebRequest.Dispose();
        }
    }

    internal class WebRequestTaskOrderComparer : IComparer<WebRequestTask>
    {
        public int Compare(WebRequestTask task1, WebRequestTask task2)
        {
            if (task1.Priority < task2.Priority)
            {
                return -1;
            }
            else if (task1.Priority > task2.Priority)
            {
                return 1;
            }

            if (task1.CreatedTimeStamp < task2.CreatedTimeStamp)
            {
                return -1;
            }
            else if (task1.CreatedTimeStamp > task2.CreatedTimeStamp)
            {
                return 1;
            }

            return 0;
        }
    }
}