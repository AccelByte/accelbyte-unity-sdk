// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Core
{
    /// <summary>
    /// Interface for network utility operations including file upload/download and IP address retrieval
    /// </summary>
    public interface INetUtilities
    {
        /// <summary>
        /// Get public IP address using ipify service
        /// </summary>
        /// <param name="callback">Returns PublicIp via callback when completed</param>
        void GetPublicIp(ResultCallback<PublicIp> callback);

        /// <summary>
        /// Upload Json / string data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        /// <param name="contentType">Content type of the data (default: "application/octet-stream")</param>
        void UploadTo(string url, byte[] data, ResultCallback callback, string contentType = "application/octet-stream");

        /// <summary>
        /// Upload Json / string data to given URL with optional parameters.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="optionalParameters">Optional parameters for upload operation.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        void UploadTo(string url, byte[] data, UploadToOptionalParameters optionalParameters, ResultCallback callback);

        /// <summary>
        /// Upload binary data to given URL.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        void UploadBinaryTo(string url, byte[] data, ResultCallback callback);

        /// <summary>
        /// Upload binary data to given URL with optional parameters.
        /// </summary>
        /// <param name="url">URL to upload.</param>
        /// <param name="data">Data to upload.</param>
        /// <param name="optionalParameters">Optional parameters for binary upload operation.</param>
        /// <param name="callback">Returns a Result via callback when completed.</param>
        void UploadBinaryTo(string url, byte[] data, UploadBinaryOptionalParameters optionalParameters, ResultCallback callback);

        /// <summary>
        /// Download Json / string data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        void DownloadFrom(string url, ResultCallback<byte[]> callback);

        /// <summary>
        /// Download Json / string data from given URL with optional parameters.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="optionalParameters">Optional parameters for download operation.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        void DownloadFrom(string url, DownloadFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback);

        /// <summary>
        /// Download binary data from given URL.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        void DownloadBinaryFrom(string url, ResultCallback<byte[]> callback);

        /// <summary>
        /// Download binary data from given URL with optional parameters.
        /// </summary>
        /// <param name="url">URL to download.</param>
        /// <param name="optionalParameters">Optional parameters for binary download operation.</param>
        /// <param name="callback">Returns a Result that contains byte[] data via callback when completed.</param>
        void DownloadBinaryFrom(string url, DownloadBinaryFromOptionalParameters optionalParameters, ResultCallback<byte[]> callback);
    }
}
