// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Core
{
    public class AccelBytePlatformHandler
    {
        private IPlatformWrapper currentPlatformWrapper;

        public IPlatformWrapper GetPlatformWrapper()
        {
            return currentPlatformWrapper;
        }

        public void SetPlatformWrapper(IPlatformWrapper wrapper)
        {
            currentPlatformWrapper = wrapper;
        }

        public void FetchPlatformToken(Action<string> callback)
        {
            if (currentPlatformWrapper == null)
            {
                AccelByteDebug.LogWarning("The current platform wrapper is null. Please set it using SetPlatformWrapper function");
                string errorMessage = Utils.PlatformHandlerUtils.SerializeErrorMessage(
                    ((int)ErrorCode.InvalidResponse).ToString(),
                    "null platformWrapper");
                callback?.Invoke(errorMessage);
            }
            else
            {
                currentPlatformWrapper.FetchPlatformToken(callback);
            }
        }
    }
}