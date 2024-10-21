// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
    public class CoroutineRunner
    {
        public readonly MonoBehaviour MonoBehaviour;
        private readonly Queue<Action> callbacks = new Queue<Action>();
        private readonly object syncToken = new object();

        private bool isRunning = true;

        public CoroutineRunner()
        {
            GameObject sdkGameObject = Utils.AccelByteGameObject.GetOrCreateGameObject();

            this.MonoBehaviour = sdkGameObject.GetComponent<DummyBehaviour>();
            if(this.MonoBehaviour == null)
            {
                this.MonoBehaviour = sdkGameObject.AddComponent<DummyBehaviour>();
            }
			            
            this.MonoBehaviour.StartCoroutine(this.RunCallbacks());
        }

        ~CoroutineRunner() { 
            this.isRunning = false; 
        }

        public Coroutine Run(IEnumerator coroutine)
        { 
            return this.MonoBehaviour != null ? this.MonoBehaviour.StartCoroutine(coroutine) : null;
        }
        
        public void Stop(Coroutine coroutine) { this.MonoBehaviour.StopCoroutine(coroutine); }

        public void Run(Action callback)
        {
            lock (this.syncToken)
            {
                this.callbacks.Enqueue(callback);
            }
        }
        
        private IEnumerator RunCallbacks()
        {
            while (this.isRunning)
            {
                yield return new WaitUntil(() => this.callbacks.Count > 0);

                Action callback;

                lock (this.syncToken)
                {
                    callback = this.callbacks.Dequeue();
                }

                callback();
            }
        }
    }
}