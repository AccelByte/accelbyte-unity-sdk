// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal enum AccelByteCacheFreshness
    {
        Stale = 0,
        Fresh = 1,
        WaitingRefresh = 2
    }
}
