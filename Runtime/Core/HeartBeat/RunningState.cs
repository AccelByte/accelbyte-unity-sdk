// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal class RunningState : IHeartBeatState
    {
        private int delayTimeInMs;
        private bool isRunning;
        private Action triggerAction;

        public RunningState(int delayTimeInMs, Action triggerAction)
        {
            this.triggerAction = triggerAction;
            isRunning = true;
            this.delayTimeInMs = delayTimeInMs;
        }

        public async void Run()
        {
            while (isRunning)
            {
                triggerAction?.Invoke();
                await System.Threading.Tasks.Task.Delay(delayTimeInMs);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }

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
            return false;
        }
    }
}