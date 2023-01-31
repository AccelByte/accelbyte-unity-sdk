// Copyright (c) 2020-2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using System.Net;
using System;

namespace AccelByte.Core
{
    public static class AccelByteNetUtilities
    {
        private static readonly CoroutineRunner coroutineRunner = new CoroutineRunner();
        private static readonly IHttpClient HttpClient = new AccelByteHttpClient();

        [Obsolete("ipify supports will be deprecated in future releases)")]
        public static void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            coroutineRunner.Run(GetPublicIpAsync(callback));
        }

        private static IEnumerator GetPublicIpAsync(ResultCallback<PublicIp> callback)
        {
            var getPubIpRequest = HttpRequestBuilder.CreateGet("https://api.ipify.org?format=json")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return AccelByteNetUtilities.HttpClient.SendRequest(getPubIpRequest, rsp => response = rsp);

            var result = response.TryParseJson<PublicIp>();
            callback.Try(result);
        }

        public static void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream")
        {
            coroutineRunner.Run(UploadToAsync(url, data, callback, contentType));
        }

        private static IEnumerator UploadToAsync(string url, byte[] data, ResultCallback callback, string contentType)
        {
            var uploadRequest = HttpRequestBuilder.CreatePut(url)
                .WithContentType(contentType)
                .WithBody(System.Convert.ToBase64String(data))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return AccelByteNetUtilities.HttpClient.SendRequest(uploadRequest, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public static void DownloadFrom(string url, ResultCallback<byte[]> callback)
        {
            coroutineRunner.Run(DownloadFromAsync(url, callback));
        }

        private static IEnumerator DownloadFromAsync(string url, ResultCallback<byte[]> callback)
        {
            var uploadRequest = HttpRequestBuilder.CreateGet(url)
                .WithContentType(MediaType.ApplicationOctetStream)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return AccelByteNetUtilities.HttpClient.SendRequest(uploadRequest, rsp => response = rsp);

            Result<byte[]> result;

            switch ((HttpStatusCode)response.Code)
            {
                case HttpStatusCode.OK:
                    result = Result<byte[]>.CreateOk(response.BodyBytes);

                    break;

                default:
                    result = Result<byte[]>.CreateError((ErrorCode)response.Code);

                    break;
            }

            callback.Try(result);
        }
    }
}
