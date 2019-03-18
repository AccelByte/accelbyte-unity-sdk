// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace AccelByte.Core
{
    public class DelayTask : ITask
    {
        private readonly Timer timer;
        private readonly ManualResetEvent awaiter;
        private bool isStarted;

        internal DelayTask(uint milliseconds)
        {
            this.awaiter = new ManualResetEvent(false);
            this.timer = new Timer(milliseconds);
            this.timer.AutoReset = false;

            this.timer.Elapsed += (obj, evt) =>
            {
                this.awaiter.Set();
            };
        }

        public event Action Completed;

        public WaitHandle GetAwaiter() { return this.awaiter; }

        public bool Execute()
        {
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.timer.Start();
            }

            return this.timer.Enabled;
        }
    }
}