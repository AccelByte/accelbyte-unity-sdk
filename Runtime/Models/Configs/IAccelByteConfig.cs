// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Models
{
    internal interface IAccelByteConfig
    {
        public void SanitizeBaseUrl();
        public void Expand();
    }

    internal interface IAccelByteMultiConfigs
    {
        public void Expand();

        internal IAccelByteConfig GetConfigFromEnvironment(SettingsEnvironment targetEnvironment);
    }
}
