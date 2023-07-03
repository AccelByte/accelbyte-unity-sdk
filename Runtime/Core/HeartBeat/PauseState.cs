// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal class PauseState : IHeartBeatState
    {
        public bool IsHeartBeatEnabled()
        {
            return true;
        }

        public bool IsHeartBeatJobRunning()
        {
            return true;
        }

        public bool IsHeartBeatPaused()
        {
            return true;
        }

        public PauseState()
        {
        }

        public void Run()
        {
        }

        public void Stop()
        {
        }
    }
}