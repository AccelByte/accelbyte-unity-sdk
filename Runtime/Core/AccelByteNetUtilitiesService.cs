// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using AccelByte.Models;

namespace AccelByte.Core
{
    /// <summary>
    /// Implementation of network utility operations
    /// </summary>
    public class AccelByteNetUtilitiesService : INetUtilities
    {
        private readonly HttpOperator httpOperator;

        /// <summary>
        /// Create a new instance of AccelByteNetUtilitiesService
        /// </summary>
        /// <param name="httpClient">HTTP client for network operations</param>
        public AccelByteNetUtilitiesService(IHttpClient httpClient)
        {
            this.httpOperator = HttpOperator.CreateDefault(httpClient);
        }

        /// <summary>
        /// Create a new instance with custom HttpOperator
        /// </summary>
        /// <param name="httpClient">HTTP client for network operations</param>
        /// <param name="httpOperator">Custom HTTP operator</param>
        public AccelByteNetUtilitiesService(HttpOperator httpOperator)
        {
            this.httpOperator = httpOperator ?? throw new ArgumentNullException(nameof(httpOperator));
        }

        public void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            GetPublicIpWithIpify(httpOperator, callback);
        }

        internal void GetPublicIpWithIpify(HttpOperator httpOperator, ResultCallback<PublicIp> callback)
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

        public void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream")
        {
            UploadTo(url, data, new UploadToOptionalParameters()
            {
                ContentType = contentType
            }, callback);
        }

        public void UploadTo(string url, byte[] data, UploadToOptionalParameters optionalParameters, ResultCallback callback)
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

            var uploadFileOptionalParameters = new UploadFileOptionalParameters()
            {
                Logger = optionalParameters.Logger,
                ApiTracker = optionalParameters.ApiTracker,
                OverrideHttpOperator = optionalParameters.OverrideHttpOperator
            };

            UploadFile(url, data, false, optionalParameters.ContentType, uploadFileOptionalParameters, callback);
        }

        public void UploadBinaryTo(string url, byte[] data, ResultCallback callback)
        {
            UploadBinaryTo(url, data, new UploadBinaryOptionalParameters(), callback);
        }

        public void UploadBinaryTo(string url, byte[] data, UploadBinaryOptionalParameters optionalParameters, ResultCallback callback)
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

            var uploadFileOptionalParameters = new UploadFileOptionalParameters()
            {
                Logger = optionalParameters.Logger,
                ApiTracker = optionalParameters.ApiTracker,
                OverrideHttpOperator = optionalParameters.OverrideHttpOperator
            };

            UploadFile(url, data, isBinary: true, contentType: contentType, uploadFileOptionalParameters, callback: callback);
        }

        private void UploadFile(string url, byte[] data, bool isBinary, string contentType, UploadFileOptionalParameters optionalParameters, ResultCallback callback)
        {
            var requestBuilder = HttpRequestBuilder.CreatePut(url)
                .WithContentType(contentType)
                .Accepts(MediaType.ApplicationJson);

            if (!isBinary)
            {
                requestBuilder.WithBody(Convert.ToBase64String(data));
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

        public void DownloadFrom(string url, ResultCallback<byte[]> callback)
        {
            DownloadFrom(url, new DownloadFromOptionalParameters(), callback);
        }

        public void DownloadFrom(string url, DownloadFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
        {
            var downloadOptionalParameters = new DownloadFileOptionalParameters()
            {
                Logger = optionalParameters?.Logger,
                IsBinary = false,
                OverrideHttpOperator = optionalParameters?.OverrideHttpOperator
            };
            DownloadFile(url, downloadOptionalParameters, callback);
        }

        public void DownloadBinaryFrom(string url, ResultCallback<byte[]> callback)
        {
            DownloadBinaryFrom(url, new DownloadBinaryFromOptionalParameters(), callback);
        }

        public void DownloadBinaryFrom(string url, DownloadBinaryFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
        {
            var downloadOptionalParameters = new DownloadFileOptionalParameters()
            {
                Logger = optionalParameters?.Logger,
                IsBinary = true,
                OverrideHttpOperator = optionalParameters?.OverrideHttpOperator
            };
            DownloadFile(url, downloadOptionalParameters, callback);
        }

        private void DownloadFile(string url, DownloadFileOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
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
