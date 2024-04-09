// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Models
{
    internal interface IAccelByteConfig
    {
        public void SanitizeBaseUrl();
        public void Expand(bool forceExpandServiceUrl);
    }

    internal interface IAccelByteMultiConfigs
    {
        public void Expand(bool forceExpandServiceUrl);
        void InitializeNullEnv();
        internal IAccelByteConfig GetConfigFromEnvironment(SettingsEnvironment targetEnvironment);
        internal void SetConfigToEnv(IAccelByteConfig newConfig, SettingsEnvironment targetEnvironment);
    }
}
