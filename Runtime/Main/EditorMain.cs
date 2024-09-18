﻿// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.Core
{
	internal class EditorMain : IPlatformMain
    {
        public void Run()
        {
            var argsConfigParser = new Utils.CustomConfigParser();
            Models.MultiSDKConfigsArgs configArgs = argsConfigParser.ParseSDKConfigFromArgs();
            if(configArgs != null)
            {
                AccelByteSDK.Implementation.OverrideConfigs.SDKConfigOverride = configArgs;
            }
            Models.MultiOAuthConfigs oAuthArgs = argsConfigParser.ParseOAuthConfigFromArgs();
            if (oAuthArgs != null)
            {
                AccelByteSDK.Implementation.OverrideConfigs.OAuthConfigOverride = oAuthArgs;
            }
        }

        public void Stop()
        {
        }
    }
}