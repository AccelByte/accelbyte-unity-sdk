// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal interface IHeartBeatState
    {
        public void Run();

        public void Stop();

        public bool IsHeartBeatEnabled();
        
        public bool IsHeartBeatJobRunning();
        
        public bool IsHeartBeatPaused();
    }
}