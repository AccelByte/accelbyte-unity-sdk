// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    public static class UnityWebRequestExtension
    {
        public static AccelByteWebRequest GetUnityWebRequest(this IHttpRequest request)
        {
            var uri = new Uri(request.Url);
            var unityWebRequest = new AccelByteWebRequest(uri, request.Method);

            unityWebRequest.RequestId = request.Id;
            if (request.Headers.TryGetValue("Authorization", out string value))
            {
                unityWebRequest.SetRequestHeader("Authorization", value);
            }

            foreach (var headerPair in request.Headers.Where(x => x.Key != "Authorization"))
            {
                unityWebRequest.SetRequestHeader(headerPair.Key, headerPair.Value);
            }

            if (request.BodyBytes != null)
            {
                unityWebRequest.uploadHandler = new UploadHandlerRaw(request.BodyBytes);
                unityWebRequest.disposeUploadHandlerOnDispose = true;
            }

            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            unityWebRequest.disposeDownloadHandlerOnDispose = true;

            return unityWebRequest;
        }

        public static IHttpResponse GetHttpResponse(this AccelByteWebRequest request)
        {
            return new UnityHttpResponseAdapter
            {
                Url = request.url,
                Method = request.method,
                Code = request.responseCode,
                Headers = request.GetResponseHeaders(),
                BodyBytes = request.downloadHandler.data,
                Timestamp = request.ResponseTimestamp
            };
        }

        private class UnityHttpResponseAdapter : IHttpResponse
        {
            public string Url { get; set; }
            public string Method { get; set; }
            public long Code { get; set; }
            
            public IDictionary<string, string> Headers { get; set;  }
            public byte[] BodyBytes { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public static System.Runtime.CompilerServices.TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp)
        {
            var taskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<UnityWebRequest.Result>();
            reqOp.completed += (asyncOp) =>
            {
                taskCompletionSource.TrySetResult(reqOp.webRequest.result);
            };

            if (reqOp.isDone)
            {
                taskCompletionSource.TrySetResult(reqOp.webRequest.result);
            }    

            return taskCompletionSource.Task.GetAwaiter();
        }
    }
}