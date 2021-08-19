// Copyright (c) 2020-2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using System.Net;

namespace AccelByte.Core
{
    static class AccelByteNetUtilities
    {
        private static readonly CoroutineRunner coroutineRunner = new CoroutineRunner();
        private static readonly IHttpWorker httpWorker = new UnityHttpWorker();

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

            yield return httpWorker.SendRequest(getPubIpRequest, rsp => response = rsp);

            var result = response.TryParseJson<PublicIp>();
            callback.Try(result);
        }

        public static void UploadTo(string url, byte[] data, ResultCallback callback)
        {
            coroutineRunner.Run(UploadToAsync(url, data, callback));
        }

        private static IEnumerator UploadToAsync(string url, byte[] data, ResultCallback callback)
        {
            var uploadRequest = HttpRequestBuilder.CreatePut(url)
                .WithContentType(MediaType.OctedStream)
                .WithBody(System.Convert.ToBase64String(data))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(uploadRequest, rsp => response = rsp);

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
                .WithContentType(MediaType.OctedStream)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(uploadRequest, rsp => response = rsp);

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
