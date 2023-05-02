// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Threading.Tasks;

namespace AccelByte.Core
{
    internal static class AccelByteHttpHelper
    {
        internal const int HttpRequestDefaultPriority = 2;
        internal const int HttpDelayOneFrameTimeMs = 25;

        internal static Task HttpDelayOneFrame
        {
            get
            {
                return Task.Delay(HttpDelayOneFrameTimeMs);
            }
        }
    }
}