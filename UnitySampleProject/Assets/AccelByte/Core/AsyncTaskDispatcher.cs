// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AccelByte.Core
{
    internal class AsyncTaskDispatcher
    {
        private const int NoTaskDelay = 100;
        private const int RunTasksJoinTimeout = 5000;

        private readonly List<ITask> pendingTasks = new List<ITask>();
        private readonly ManualResetEvent tasksAwaiter = new ManualResetEvent(false);
        private readonly List<ITask> runningTasks = new List<ITask>();
        private readonly object syncToken = new object();
        private readonly Thread runTasksThread;

        private bool isRunning = true;

        public AsyncTaskDispatcher()
        {
            this.runTasksThread = new Thread(RunTasks);
            this.runTasksThread.IsBackground = true;
            this.runTasksThread.Start();
        }

        ~AsyncTaskDispatcher()
        {
            this.isRunning = false;
            this.runTasksThread.Join(AsyncTaskDispatcher.RunTasksJoinTimeout);
            this.runTasksThread.Abort();
        }

        public void Start(IEnumerator<ITask> tasks) { Start(new SequenceTask(tasks)); }

        public void Start(ITask task)
        {
            lock (this.syncToken)
            {
                this.pendingTasks.Add(task);
                this.tasksAwaiter.Set();
            }
        }

        private void RunTasks()
        {
            while (this.isRunning)
            {
                if (this.pendingTasks.Count == 0 && !this.tasksAwaiter.WaitOne(AsyncTaskDispatcher.NoTaskDelay))
                {
                    continue;
                }

                lock (this.syncToken)
                {
                    this.runningTasks.AddRange(this.pendingTasks);
                    this.pendingTasks.Clear();
                }

                WaitHandle.WaitAny(
                    this.runningTasks.Select(task => task.GetAwaiter()).ToArray(),
                    AsyncTaskDispatcher.NoTaskDelay);
                this.runningTasks.RemoveAll(task => !task.Execute());

                lock (this.syncToken)
                {
                    if (this.runningTasks.Count == 0)
                    {
                        this.tasksAwaiter.Reset();
                    }
                }
            }
        }
    }
}