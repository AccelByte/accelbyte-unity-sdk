// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    internal class UnityHttpRequestSender : IHttpRequestSender
    {
        public IEnumerator Send(IHttpRequest request, Action<IHttpResponse, Error> callback, int timeoutMs)
        {
            using (UnityWebRequest unityWebRequest = request.GetUnityWebRequest())
            {
                unityWebRequest.timeout = timeoutMs / 1000;

                Report.GetHttpRequest(request, unityWebRequest);

                yield return unityWebRequest.SendWebRequest();

                Report.GetHttpResponse(unityWebRequest);

#if UNITY_2020_3_OR_NEWER
                switch (unityWebRequest.result)
                {
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    callback?.Invoke(unityWebRequest.GetHttpResponse(), null);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    callback?.Invoke(null, new Error(ErrorCode.NetworkError));
                    break;
                }
#else
                if (unityWebRequest.isNetworkError)
                {
                    callback?.Invoke(null, new Error(ErrorCode.NetworkError));
                }
                else
                {
                    callback?.Invoke(unityWebRequest.GetHttpResponse(), null);
                }
#endif
            }
        }

        public void ClearCookies(Uri uri)
        {
            UnityWebRequest.ClearCookieCache(uri);
        }
    }
}