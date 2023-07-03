// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal class IdleState : IHeartBeatState
    {
        public bool IsHeartBeatEnabled()
        {
            return false;
        }

        public bool IsHeartBeatJobRunning()
        {
            return false;
        }

        public bool IsHeartBeatPaused()
        {
            return false;
        }

        public void Run()
        {
        }
       
        public void Stop()
        {
        }
    }
}