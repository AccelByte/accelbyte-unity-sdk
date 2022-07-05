// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
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
        public static UnityWebRequest GetUnityWebRequest(this IHttpRequest request)
        {
            var uri = new Uri(request.Url);
            var unityWebRequest = new UnityWebRequest(uri, request.Method);

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
            }

            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            return unityWebRequest;
        }

        public static IHttpResponse GetHttpResponse(this UnityWebRequest request)
        {
            return new UnityHttpResponseAdapter
            {
                Url = request.url, Code = request.responseCode, Headers = request.GetResponseHeaders(), BodyBytes = request.downloadHandler.data
            };
        }

        private class UnityHttpResponseAdapter : IHttpResponse
        {
            public string Url { get; set; }
            public long Code { get; set; }
            
            public IDictionary<string, string> Headers { get; set;  }
            public byte[] BodyBytes { get; set; }
        }
    }
}