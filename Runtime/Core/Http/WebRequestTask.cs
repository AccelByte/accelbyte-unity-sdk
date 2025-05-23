﻿// Copyright (c) 2021-2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class WebRequestTask
    {
        public readonly int TimeoutMs;
        public IHttpRequest HttpRequest
        {
            get;
            private set;
        }

        public System.Action<AccelByteWebRequest> OnComplete;

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

        public WebRequestState State
        {
            get;
            private set;
        }
        
        public readonly AdditionalHttpParameters AdditionalHttpParameters;

        public WebRequestTask(IHttpRequest httpRequest, int timeoutMs, uint delayMs) : this(httpRequest, timeoutMs, delayMs, new AdditionalHttpParameters())
        {
        }

        public WebRequestTask(IHttpRequest httpRequest, int timeoutMs, uint delayMs, AdditionalHttpParameters additionalParameters)
        {
            HttpRequest = httpRequest;
            DelayMs = delayMs;
            this.TimeoutMs = timeoutMs;
            CreatedTimeStamp = DateTime.UtcNow;
            AdditionalHttpParameters = additionalParameters;
            SetState(WebRequestState.Waiting);
        }

        public void SetComplete(AccelByteWebRequest webRequestResult)
        {
            SetState(WebRequestState.Complete);
            OnComplete?.Invoke(webRequestResult);
        }

        public void SetState(WebRequestState newState)
        {
            State = newState;
        }

        public AccelByteWebRequest CreateWebRequest()
        {
            AccelByteWebRequest unityWebRequest = HttpRequest.GetUnityWebRequest();
            return unityWebRequest;
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

    internal enum WebRequestState
    {
        Waiting,
        OnProcess,
        Complete
    }
}