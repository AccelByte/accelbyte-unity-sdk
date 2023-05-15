// Copyright (c) 2021-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    internal class UnityHttpRequestSender : IHttpRequestSender
    {
        WebRequestScheduler httpTaskScheduler;
        public UnityHttpRequestSender(WebRequestScheduler httpTaskScheduler)
        {
            this.httpTaskScheduler = httpTaskScheduler;
        }

        public void AddTask(IHttpRequest request, Action<HttpSendResult> callback, int timeoutMs, uint delayTimeMs = 0)
        {
            System.Action<float> onGameUpdate = null;

            onGameUpdate = (deltaTime) =>
            {
                AccelByteSDKMain.OnGameUpdate -= onGameUpdate;

                UnityWebRequest unityWebRequest = request.GetUnityWebRequest();
                unityWebRequest.timeout = timeoutMs / 1000;

                WebRequestTask newTask = new WebRequestTask(request, unityWebRequest, delayTimeMs)
                {
                    OnComplete = (sentWebRequest) =>
                    {
                        HttpSendResult responseResult = ParseWebRequestResult(sentWebRequest);
                        callback?.Invoke(responseResult);
                        // See https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Dispose.html
                        unityWebRequest.Dispose();
                    }
                };
                httpTaskScheduler.AddTask(newTask);
            };

            AccelByteSDKMain.OnGameUpdate += onGameUpdate;
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