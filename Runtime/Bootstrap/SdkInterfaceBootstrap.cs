// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System.IO;
using System;
using UnityEngine;

namespace AccelByte.Core
{
    internal static class SdkInterfaceBootstrap
    {
        internal static void Execute()
        {
        }

        public static void Stop()
        {
            AccelByteSDK.Implementation.Reset();
        }
    }
}
