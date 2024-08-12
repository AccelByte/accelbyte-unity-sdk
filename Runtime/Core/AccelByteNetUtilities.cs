// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
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
        private static readonly IHttpClient httpClient;

        static AccelByteNetUtilities()
        {
            IHttpRequestSender httpSender = AccelByteSDK.SdkHttpSenderFactory.CreateHttpRequestSender();
            httpClient = new AccelByteHttpClient(httpSender);
        }

        [Obsolete("ipify supports will be deprecated in future releases)")]
        public static void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            coroutineRunner.Run(GetPublicIpAsync(callback));
        }

        internal static void GetPublicIpWithIpify(HttpOperator httpOperator, ResultCallback<PublicIp> callback)
        {
            var getPubIpRequest = HttpRequestBuilder.CreateGet("https://api.ipify.org?format=json")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            httpOperator.SendRequest(getPubIpRequest, response =>
            {
                var result = response.TryParseJson<PublicIp>();
                callback?.Try(result);
            });
        }

        private static IEnumerator GetPublicIpAsync(ResultCallback<PublicIp> callback)
        {
            var getPubIpRequest = HttpRequestBuilder.CreateGet("https://api.ipify.org?format=json")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return AccelByteSDK.GetClientRegistry().GetApi().httpClient.SendRequest(getPubIpRequest, rsp => response = rsp);

            var result = response.TryParseJson<PublicIp>();
            callback.Try(result);
        }

        /// <summary>
        /// Upload Json / string data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream")
        {
            if (string.IsNullOrEmpty(url))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, url cannot be null!"));
                return;
            }
            if (data == null)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, data cannot be null!"));
                return;
            }

            coroutineRunner.Run(UploadToAsync(url, data, false, contentType, callback));
        }

        /// <summary>
        /// Upload binary data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadBinaryTo(string url
            , byte[] data
            , ResultCallback callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, url cannot be null!"));
                return;
            }
            if (data == null)
            {
                callback.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, data cannot be null!"));
                return;
            }

            coroutineRunner.Run(UploadToAsync(url, data, true, "application/octet-stream", callback));
        }

        private static IEnumerator UploadToAsync(string url
            , byte[] data
            , bool isBinary
            , string contentType
            , ResultCallback callback)
        {
            var requestBuilder = HttpRequestBuilder.CreatePut(url)
                .WithContentType(contentType)
                .Accepts(MediaType.ApplicationJson);

            if (!isBinary)
            {
                requestBuilder.WithBody(System.Convert.ToBase64String(data));
            }
            else
            {
                requestBuilder.WithBody(data);
            }

            var request = requestBuilder.GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        /// <summary>
        /// Download Json / string data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadFrom(string url, ResultCallback<byte[]> callback)
        {
            coroutineRunner.Run(DownloadFromAsync(url, false, callback));
        }

        /// <summary>
        /// Download binary data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadBinaryFrom(string url, ResultCallback<byte[]> callback)
        {
            coroutineRunner.Run(DownloadFromAsync(url, true, callback));
        }

        private static IEnumerator DownloadFromAsync(string url, bool isBinary, ResultCallback<byte[]> callback)
        {
            var uploadRequest = HttpRequestBuilder.CreateGet(url)
                .WithContentType(MediaType.ApplicationOctetStream)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(uploadRequest, rsp => response = rsp);

            Result<byte[]> result;

            if (response == null)
            {
                result = Result<byte[]>.CreateError(ErrorCode.InvalidResponse);
                callback.Try(result);
                yield break;
            }

            switch ((HttpStatusCode)response.Code)
            {
                case HttpStatusCode.OK:
                    var value = response.BodyBytes;
                    if (!isBinary && value != null)
                    {
                        value = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(value));
                    }
                    result = Result<byte[]>.CreateOk(value);

                    break;

                default:
                    result = Result<byte[]>.CreateError((ErrorCode)response.Code);

                    break;
            }

            callback.Try(result);
        }
    }
}
