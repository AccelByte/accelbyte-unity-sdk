// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Timers; 
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Timer = System.Timers.Timer; 

namespace Tests.AUnitTests
{
    class ExampleTask : ITask
    {
        private bool isStarted;
        private readonly Timer timer;
        private readonly ResultCallback callback;
        private readonly ManualResetEvent awaiter;

        internal ExampleTask(uint milliseconds, ResultCallback callback)
        {
            this.awaiter = new ManualResetEvent(false);
            this.timer = new Timer(milliseconds);
            this.timer.AutoReset = false;
            this.timer.Elapsed += OnTimeElapsed;
            this.callback = callback;
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

        private void OnTimeElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.awaiter.Set();
            this.callback.Try(Result.CreateOk());
        }
    }
    
    [TestFixture]
    public class AsyncTaskDispatcherTest
    {
        [UnityTest]
        public IEnumerator Start_MultipleAsynchronousTask_Success()
        {
            var queue = new Queue<int>();
            var expectedQueue = new Queue<int>();
            expectedQueue.Enqueue(200);
            expectedQueue.Enqueue(300);
            expectedQueue.Enqueue(400);
            expectedQueue.Enqueue(500);
            expectedQueue.Enqueue(700);
            AsyncTaskDispatcher asyncTaskDispatcher = new AsyncTaskDispatcher();
            ExampleTask exampleTask = new ExampleTask(300, result => { queue.Enqueue(300); });
            ExampleTask example2Task = new ExampleTask(200, result => { queue.Enqueue(200); });
            ExampleTask example3Task = new ExampleTask(700, result => { queue.Enqueue(700); });
            ExampleTask example4Task = new ExampleTask(500, result => { queue.Enqueue(500); });
            ExampleTask example5Task = new ExampleTask(400, result => { queue.Enqueue(400); });
            asyncTaskDispatcher.Start(exampleTask);
            asyncTaskDispatcher.Start(example2Task);
            asyncTaskDispatcher.Start(example3Task);
            asyncTaskDispatcher.Start(example4Task);
            asyncTaskDispatcher.Start(example5Task);
            while (queue.Count < 5) { yield return new WaitForSeconds(0.1f); }
            
            TestHelper.Assert(() => 
                Assert.AreEqual(queue, expectedQueue));
        }
    }
}