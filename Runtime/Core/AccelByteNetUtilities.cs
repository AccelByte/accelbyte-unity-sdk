﻿// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
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
        private static readonly IHttpClient httpClient;
        private static readonly HttpOperator httpOperator;

        static AccelByteNetUtilities()
        {
            IHttpRequestSender httpSender = AccelByteSDK.Implementation.SdkHttpSenderFactory.CreateHttpRequestSender();
            httpClient = new AccelByteHttpClient(httpSender);
            httpOperator = HttpOperator.CreateDefault(httpClient);
        }

        [Obsolete("ipify supports will be deprecated in future releases)")]
        public static void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            GetPublicIpWithIpify(httpOperator, callback);
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

        /// <summary>
        /// Upload Json / string data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream")
        {
            UploadTo(url, data, new UploadToOptionalParameters()
            {
                ContentType = contentType
            }, callback);
        }
        
        internal static void UploadTo(string url, byte[] data, UploadToOptionalParameters optionalParameters, ResultCallback callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, url cannot be null!"));
                return;
            }
            if (data == null)
            {
                callback?.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, data cannot be null!"));
                return;
            }

            if (optionalParameters == null)
            {
                optionalParameters = new UploadToOptionalParameters();
            }

            UploadFileOptionalParameters uploadFileOptionalParameters = new UploadFileOptionalParameters()
            {
                Logger = optionalParameters.Logger,
                OverrideHttpOperator = optionalParameters.OverrideHttpOperator
            };

            UploadFile(url, data, false, optionalParameters.ContentType, uploadFileOptionalParameters, callback);
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
            UploadBinaryTo(url, data, new UploadBinaryOptionalParameters(), callback);
        }
        
        /// <summary>
        /// Upload binary data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="optionalParameters">Optional parameters to upload binary data.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadBinaryTo(string url
            , byte[] data
            , UploadBinaryOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, url cannot be null!"));
                return;
            }
            if (data == null)
            {
                callback?.TryError(new Error(ErrorCode.InvalidArgument, "Failed to upload, data cannot be null!"));
                return;
            }

            if (optionalParameters == null)
            {
                optionalParameters = new UploadBinaryOptionalParameters();
            }
            
            string contentType = "application/octet-stream";
            if (!string.IsNullOrEmpty(optionalParameters.ContentType))
            {
                contentType = optionalParameters.ContentType;
            }
            
            UploadFileOptionalParameters uploadFileOptionalParameters = new UploadFileOptionalParameters()
            {
                Logger = optionalParameters.Logger,
                OverrideHttpOperator = optionalParameters.OverrideHttpOperator
            };
            
            UploadFile(url, data, isBinary: true, contentType: contentType, uploadFileOptionalParameters, callback: callback);
        }

        private static void UploadFile(string url
            , byte[] data
            , bool isBinary
            , string contentType
            , UploadFileOptionalParameters optionalParameters
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

            var targetHttpOperator = httpOperator;
            if (optionalParameters != null && optionalParameters.OverrideHttpOperator != null)
            {
                targetHttpOperator = optionalParameters.OverrideHttpOperator;
            }
            
            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            targetHttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        /// <summary>
        /// Download Json / string data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadFrom(string url, ResultCallback<byte[]> callback)
        {
            DownloadFrom(url, new DownloadFromOptionalParameters(), callback);
        }
        
        internal static void DownloadFrom(string url, DownloadFromOptionalParameters optionalOptionalParameters, ResultCallback<byte[]> callback)
        {
            var downloadOptionalParameters = new DownloadFileOptionalParameters()
            {
                Logger = optionalOptionalParameters?.Logger,
                IsBinary = false,
                OverrideHttpOperator = optionalOptionalParameters?.OverrideHttpOperator
            };
            DownloadFile(url, downloadOptionalParameters, callback);
        }

        /// <summary>
        /// Download binary data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadBinaryFrom(string url, ResultCallback<byte[]> callback)
        {
            DownloadBinaryFrom(url, new DownloadBinaryFromOptionalParameters(), callback);
        }
        
        internal static void DownloadBinaryFrom(string url, DownloadBinaryFromOptionalParameters optionalOptionalParameters, ResultCallback<byte[]> callback)
        {
            var downloadOptionalParameters = new DownloadFileOptionalParameters()
            {
                Logger = optionalOptionalParameters?.Logger,
                IsBinary = true,
                OverrideHttpOperator = optionalOptionalParameters?.OverrideHttpOperator
            };
            DownloadFile(url, downloadOptionalParameters, callback);
        }

        private static void DownloadFile(string url, DownloadFileOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
        {
            var uploadRequest = HttpRequestBuilder.CreateGet(url)
                .WithContentType(MediaType.ApplicationOctetStream)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var targetHttpOperator = httpOperator;
            if (optionalParameters != null && optionalParameters.OverrideHttpOperator != null)
            {
                targetHttpOperator = optionalParameters.OverrideHttpOperator;
            }
            
            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            targetHttpOperator.SendRequest(additionalHttpParameters, uploadRequest, response =>
            {
                Result<byte[]> result;

                if (response == null)
                {
                    result = Result<byte[]>.CreateError(ErrorCode.InvalidResponse);
                    callback?.Try(result);
                    return;
                }

                bool isBinary = false;
                if (optionalParameters != null)
                {
                    isBinary = optionalParameters.IsBinary;
                }

                switch ((HttpStatusCode)response.Code)
                {
                    case HttpStatusCode.OK:
                        var value = response.BodyBytes;
                        if (!isBinary && value != null)
                        {
                            string resultString = System.Text.Encoding.UTF8.GetString(value);
                            value = Convert.FromBase64String(resultString);
                        }
                        result = Result<byte[]>.CreateOk(value);
                        break;
                    default:
                        result = Result<byte[]>.CreateError((ErrorCode)response.Code);
                        break;
                }

                callback?.Try(result);
            });
        }
    }
}
