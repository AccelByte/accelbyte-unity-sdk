﻿// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System.IO;
using System;
using UnityEngine;

namespace AccelByte.Core
{
    internal static class EnvrionmentBootstrap
    {

        internal static void Execute()
        {
            AccelByteSDK.Environment = new AccelByteEnvironment();
        }

        public static void Stop()
        {

        }
    }
}
