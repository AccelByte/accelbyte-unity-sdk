// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal class RunningState : IHeartBeatState
    {
        private Action triggerAction;
        private bool isRunning;
        Utils.AccelByteTimer timer;

        public RunningState(int delayTimeInMs, Action triggerAction)
        {
            float delayTimeInS = Utils.TimeUtils.MillisecondsToSeconds(delayTimeInMs);
            this.triggerAction = triggerAction;
            Action onTimerDone = () =>
            {
                if (isRunning)
                {
                    this.triggerAction?.Invoke();
                    timer.Start();
                }
            };
            timer = new Utils.AccelByteTimer(delayTimeInS, onTimerDone);
        }

        public void Run()
        {
            triggerAction?.Invoke();
            timer.Start();
            isRunning = true;
        }

        public void Stop()
        {
            timer.Stop();
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