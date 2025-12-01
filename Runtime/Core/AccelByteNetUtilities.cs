// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Models;

namespace AccelByte.Core
{
    /// <summary>
    /// Static utility class for network operations (backward compatibility facade)
    /// For new code, use INetUtilities from AccelByteSdk.GetClientRegistry().NetUtilities
    /// </summary>
    public static class AccelByteNetUtilities
    {
        private static Lazy<INetUtilities> instance = new Lazy<INetUtilities>(CreateInstance);

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            instance = new Lazy<INetUtilities>(CreateInstance);
        }

        private static INetUtilities CreateInstance()
        {
            IHttpRequestSender httpSender = AccelByteSDK.Implementation.SdkHttpSenderFactory.CreateHttpRequestSender();
            IHttpClient httpClient = new AccelByteHttpClient(httpSender);
            return new AccelByteNetUtilitiesService(httpClient);
        }

        private static INetUtilities Instance => instance.Value;

        [Obsolete("ipify supports will be deprecated in future releases)")]
        public static void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            Instance.GetPublicIp(callback);
        }

        internal static void GetPublicIpWithIpify(HttpOperator httpOperator, ResultCallback<PublicIp> callback)
        {
            // For backward compatibility with DedicatedServerManagerApi
            var service = new AccelByteNetUtilitiesService(httpOperator);
            service.GetPublicIp(callback);
        }

        /// <summary>
        /// Upload Json / string data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream")
        {
            Instance.UploadTo(url, data, callback, contentType);
        }

        internal static void UploadTo(string url, byte[] data, UploadToOptionalParameters optionalParameters, ResultCallback callback)
        {
            Instance.UploadTo(url, data, optionalParameters, callback);
        }

        /// <summary>
        /// Upload binary data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadBinaryTo(string url, byte[] data, ResultCallback callback)
        {
            Instance.UploadBinaryTo(url, data, callback);
        }

        /// <summary>
        /// Upload binary data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="optionalParameters">Optional parameters to upload binary data.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        public static void UploadBinaryTo(string url, byte[] data, UploadBinaryOptionalParameters optionalParameters, ResultCallback callback)
        {
            Instance.UploadBinaryTo(url, data, optionalParameters, callback);
        }

        /// <summary>
        /// Download Json / string data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadFrom(string url, ResultCallback<byte[]> callback)
        {
            Instance.DownloadFrom(url, callback);
        }

        internal static void DownloadFrom(string url, DownloadFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
        {
            Instance.DownloadFrom(url, optionalParameters, callback);
        }

        /// <summary>
        /// Download binary data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        public static void DownloadBinaryFrom(string url, ResultCallback<byte[]> callback)
        {
            Instance.DownloadBinaryFrom(url, callback);
        }

        internal static void DownloadBinaryFrom(string url, DownloadBinaryFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback)
        {
            Instance.DownloadBinaryFrom(url, optionalParameters, callback);
        }
    }
}
