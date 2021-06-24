// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Text;
using AccelByte.Api;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    public static class UnityWebRequestExtension
    {
        public static UnityWebRequest GetUnityWebRequest(this IHttpRequest request)
        {
            Uri uri = new Uri(request.Url);
#if NET_2_0_SUBSET || NET_2_0 || !UNITY_2017_1_OR_NEWER
            UriHelper.ForceCanonicalPathAndQuery(uri);
#endif

            var unityWebRequest = new UnityWebRequest(uri, request.Method);

            foreach (var headerPair in request.Headers)
            {
                unityWebRequest.SetRequestHeader(headerPair.Key, headerPair.Value);
            }

            if (request.BodyBytes != null)
            {
                unityWebRequest.uploadHandler = new UploadHandlerRaw(request.BodyBytes);
            }

            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            UnityWebRequest.ClearCookieCache(new Uri(AccelBytePlugin.Config.BaseUrl));

            return unityWebRequest;
        }

        public static IHttpResponse GetHttpResponse(this UnityWebRequest request)
        {
            return new UnityHttpResponseAdapter
            {
                Url = request.url, Code = request.responseCode, BodyBytes = request.downloadHandler.data
            };
        }

        private class UnityHttpResponseAdapter : IHttpResponse
        {
            public string Url { get; set; }
            public long Code { get; set; }
            public Stream BodyStream { get; set; }
            public byte[] BodyBytes { get; set; }

            public string Body
            {
                get { return this.BodyBytes == null ? null : Encoding.UTF8.GetString(this.BodyBytes); }
            }
        }
    }

#if NET_2_0_SUBSET || NET_2_0 || !UNITY_2017_1_OR_NEWER
    public static class UriHelper
    {
        private static readonly Type uriType = typeof(Uri);
        private static readonly FieldInfo sourceField;
        private static readonly FieldInfo queryField;
        private static readonly FieldInfo pathField;
        private static readonly FieldInfo cachedToStringField;
        private static readonly FieldInfo cachedAbsoluteUriField;

        static UriHelper()
        {
            UriHelper.sourceField = UriHelper.uriType.GetField(
                "source",
                BindingFlags.NonPublic | BindingFlags.Instance);

            UriHelper.queryField = UriHelper.uriType.GetField("query", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.pathField = UriHelper.uriType.GetField("path", BindingFlags.NonPublic | BindingFlags.Instance);
            UriHelper.cachedToStringField = UriHelper.uriType.GetField(
                "cachedToString",
                BindingFlags.NonPublic | BindingFlags.Instance);

            UriHelper.cachedAbsoluteUriField = UriHelper.uriType.GetField(
                "cachedAbsoluteUri",
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void ForceCanonicalPathAndQuery(Uri uri)
        {
            var source = (string) UriHelper.sourceField.GetValue(uri);
            UriHelper.cachedToStringField.SetValue(uri, source);
            UriHelper.cachedAbsoluteUriField.SetValue(uri, source);
            var fragPos = source.IndexOf("#");
            var queryPos = source.IndexOf("?");
            var start = source.IndexOf(uri.Host) + uri.Host.Length;
            var pathEnd = queryPos == -1 ? fragPos : queryPos;

            if (pathEnd == -1)
            {
                pathEnd = source.Length + 1;
            }

            var path = queryPos > -1 ? source.Substring(start, pathEnd - start) : source.Substring(start);
            UriHelper.pathField.SetValue(uri, path);

            if (queryPos > -1)
            {
                UriHelper.queryField.SetValue(
                    uri,
                    fragPos > -1 ? source.Substring(queryPos, fragPos - queryPos) : source.Substring(queryPos));
            }
        }
    }
#endif
}