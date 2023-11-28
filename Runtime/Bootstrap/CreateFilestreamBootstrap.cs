// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System;
using System.IO;
using UnityEditor;

namespace AccelByte.Core
{
    public static class CreateFilestreamBootstrap
    {
        public static void Execute()
        {
            AccelByteSDK.FileStream = new AccelByteFileStream();
            const bool createSignallerObjectOnNotExist = false;
            AccelByteSDKMain.AddGameUpdateListener(OnGameUpdate, createSignallerObjectOnNotExist);
        }

        public static void Stop()
        {
            AccelByteSDKMain.RemoveGameUpdateListener(OnGameUpdate);
            AccelByteSDK.FileStream = null;
        }

        private static void OnGameUpdate(float dt)
        {
            if (AccelByteSDK.FileStream != null)
            {
                AccelByteSDK.FileStream.Pop();
            }
        }
    }
}

