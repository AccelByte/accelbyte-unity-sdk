// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    internal struct OverrideConfigs
    {
        public MultiSDKConfigsArgs SDKConfigOverride;
        public MultiOAuthConfigs OAuthConfigOverride;
    }
}
