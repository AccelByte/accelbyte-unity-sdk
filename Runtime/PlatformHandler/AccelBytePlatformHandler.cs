// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Core
{
    public class AccelBytePlatformHandler
    {
        private IPlatformWrapper currentPlatformWrapper;
        private IDebugger logger;
        
        public AccelBytePlatformHandler(IDebugger logger = null)
        {
            this.logger = logger;
        }

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
                logger?.LogWarning("The current platform wrapper is null. Please set it using SetPlatformWrapper function");
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