// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    /// <summary>
    /// Tracks multiple session instances for class children inheritted from ApiBase (or ServerApiBase)
    /// <para>
    /// Eg, for Xbox with multiple users signing in for a local session of 2+ players
    ///     (both signed into their own, individual accounts)
    /// </para>
    /// </summary>
    public static class MultiRegistry
    {
        static MultiRegistry()
        {
            const string deprecatedWarning = "MultiRegistry is deprecated and will be sunset on 3.79 release. Please migrate to AccelByteSDK.GetClientRegistry and AccelByteSDK.GetServerRegistry.\nGuideline how to migrate: https://docs.accelbyte.io/gaming-services/developers/sdk-tools/sdk-guides/multiple-registries/migrate-to-multiple-registry-unity/";
            UnityEngine.Debug.LogWarning(deprecatedWarning);
        }
        #region Client

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ApiClient GetApiClient(string key = "default")
        {
            return AccelByteSDK.GetClientRegistry().GetApi(key);
        }

        /// <summary>
        /// Creates (and returns) new ApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ApiClient CreateNewApiClient(string key = "default")
        {
            return AccelByteSDK.GetClientRegistry().CreateNewApi();
        }

        /// <summary>
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="key">Leave empty for "default"; duplicate key names will replace the previous entry</param>
        /// <returns>isSuccess</returns>
        public static bool RegisterApiClient(string key, ApiClient apiClient)
        {
            bool retval = AccelByteSDK.GetClientRegistry().RegisterApi(key, apiClient);
            return retval;
        }

        /// <summary>
        /// Remove an existing ApiClient.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>isSuccess</returns>
        internal static bool DeleteApiClient(string key)
        {
            bool retval = AccelByteSDK.GetClientRegistry().RemoveApi(key);
            return retval;
        }
        #endregion /Client

        #region Server
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ServerApiClient GetServerApiClient(string key = "default")
        {
            return AccelByteSDK.GetServerRegistry().GetApi(key);
        }

        /// <summary>
        /// Creates (and returns) new ServerApiClient.
        /// <para>If key == "default", the new client's httpClient credentials will be set from Settings.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ServerApiClient CreateNewServerApiClient(string key = "default")
        {
            return AccelByteSDK.GetServerRegistry().CreateNewApi();
        }

        /// <summary>
        /// Overrides existing session with same key name
        /// </summary>
        /// <param name="serverApiClient"></param>
        /// <param name="key">Leave empty for "default"</param>
        /// <returns>isSuccess</returns>
        public static bool RegisterServerApiClient(string key, ServerApiClient serverApiClient)
        {
            return AccelByteSDK.GetServerRegistry().RegisterApi(key, serverApiClient);
        }
        
        internal static bool DeleteServerApiClient(string key)
        {
            bool retval = AccelByteSDK.GetServerRegistry().RemoveApi(key);
            return retval;
        }
        #endregion /Server
    }
}
