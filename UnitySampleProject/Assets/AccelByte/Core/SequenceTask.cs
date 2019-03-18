// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Threading;

namespace AccelByte.Core
{
    public class SequenceTask : ITask
    {
        private readonly IEnumerator<ITask> tasks;
        private readonly ManualResetEvent awaiter;
        private readonly Action signalAwaiter;
        private bool isStarted;

        internal SequenceTask(IEnumerator<ITask> tasks)
        {
            this.awaiter = new ManualResetEvent(true);
            this.tasks = tasks;
            this.signalAwaiter = () => this.awaiter.Set();
        }

        public event Action Completed;

        public WaitHandle GetAwaiter() { return this.awaiter; }

        public bool Execute()
        {
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.tasks.MoveNext();
            }

            if (this.tasks.Current == null) return false;

            this.awaiter.Reset();
            this.tasks.Current.Completed += this.signalAwaiter;

            if (this.tasks.Current.Execute()) return true;

            this.tasks.Current.Completed -= this.signalAwaiter;

            return this.tasks.MoveNext();
        }
    }
}