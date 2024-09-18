// Copyright (c) 2021 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    internal class UnityHttpRequestSender : IHttpRequestSender
    {
        WebRequestScheduler httpTaskScheduler;

        private IDebugger logger;
        private CoreHeartBeat heartBeat;
        
        public UnityHttpRequestSender(WebRequestScheduler httpTaskScheduler)
        {
            this.httpTaskScheduler = httpTaskScheduler;
        }

        public void SetLogger(IDebugger logger)
        {
            this.logger = logger;
            httpTaskScheduler?.SetLogger(logger);
        }

        internal void SetHeartBeat(CoreHeartBeat coreHeartBeat)
        {
            heartBeat = coreHeartBeat;
        }

        public void AddTask(IHttpRequest request, Action<HttpSendResult> callback, int timeoutMs, uint delayTimeMs = 0)
        {
            if (heartBeat != null)
            {
                Action onNextFrame = () =>
                {
                    WebRequestTask newTask = new WebRequestTask(request, timeoutMs, delayTimeMs)
                    {
                        OnComplete = (sentWebRequest) =>
                        {
                            HttpSendResult responseResult = ParseWebRequestResult(sentWebRequest);
                            callback?.Invoke(responseResult);
                        }
                    };
                    httpTaskScheduler.AddTask(newTask);
                };
                var cancelTokenSource = new System.Threading.CancellationTokenSource();
                WaitAFrameCommand waitCommand = new WaitAFrameCommand(onNextFrame, cancelTokenSource.Token);
                heartBeat.Wait(waitCommand);
            }
        }

        public void ClearTasks()
        {
            this.httpTaskScheduler.CleanTask();
        }

        public void ClearCookies(Uri uri)
        {
            UnityWebRequest.ClearCookieCache(uri);
        }

        private HttpSendResult ParseWebRequestResult(UnityWebRequest unityWebRequest)
        {
            IHttpResponse callBackResponse = null;
            Error callBackError = null;
#if UNITY_2020_3_OR_NEWER
            switch (unityWebRequest.result)
            {
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    callBackResponse = unityWebRequest.GetHttpResponse();
                    callBackError = null;
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    callBackResponse = null;
                    callBackError = new Error(ErrorCode.NetworkError);
                    break;
            }
#else
            if (unityWebRequest.isNetworkError)
            {
                callBackResponse = null;
                callBackError = new Error(ErrorCode.NetworkError);
            }
            else
            {
                callBackResponse = unityWebRequest.GetHttpResponse();
                callBackError = null;
            }
#endif
            return new HttpSendResult(callBackResponse, callBackError);
        }

    }
}