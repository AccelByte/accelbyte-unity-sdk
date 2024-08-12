// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System.IO;
using System;
using UnityEngine;

namespace AccelByte.Core
{
    internal static class EnvironmentBootstrap
    {
        internal static void Execute()
        {
            AccelByteSDK.Environment = new AccelByteEnvironment();
            AccelByteSDK.Environment.OnEnvironmentChanged += OnEnvironmentChanged;
        }

        public static void Stop()
        {
            AccelByteSDK.Environment.OnEnvironmentChanged -= OnEnvironmentChanged;
        }

        private static void OnEnvironmentChanged(Models.SettingsEnvironment newEnvironment)
        {
            AccelByteSDK.ChangeInterfaceEnvironment(newEnvironment);
        }
    }
}
