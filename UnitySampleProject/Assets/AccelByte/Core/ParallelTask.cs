// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AccelByte.Core
{
    public class ParallelTask : ITask
    {
        private readonly IList<ITask> tasks;
        private readonly ManualResetEvent awaiter;
        private int numTasksCompleted;
        private readonly Action incrementNumTasksCompleted;

        internal ParallelTask(IList<ITask> tasks)
        {
            this.tasks = tasks;
            this.awaiter = new ManualResetEvent(false);
            this.incrementNumTasksCompleted = () =>
            {
                this.numTasksCompleted++;

                if (this.numTasksCompleted == tasks.Count)
                {
                    this.awaiter.Set();
                }
            };
            
            foreach (var task in tasks)
            {
                task.Completed += this.incrementNumTasksCompleted;
            }
        }

        public event Action Completed;

        public WaitHandle GetAwaiter() { return this.awaiter; }

        public bool Execute()
        {
            return this.tasks.All(task =>
            {
                if (task.Execute()) return true;

                task.Completed -= this.incrementNumTasksCompleted;
                return false;
            });
        }
    }
}





















