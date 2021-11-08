// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public class CoroutineRunner
    {
        private readonly GameObject gameObject;
        private readonly MonoBehaviour monoBehaviour;
        private readonly Queue<Action> callbacks = new Queue<Action>();
        private readonly object syncToken = new object();

        private bool isRunning = true;

        public CoroutineRunner()
        {
            this.gameObject = GameObject.Find("AccelByteDummyGameObject");

            if (this.gameObject == null)
            {
                this.gameObject = new GameObject("AccelByteDummyGameObject");
            }

            this.monoBehaviour = this.gameObject.AddComponent<DummyBehaviour>();
            this.monoBehaviour.StartCoroutine(this.RunCallbacks());
            Object.DontDestroyOnLoad(this.gameObject);
        }

        ~CoroutineRunner() { this.isRunning = false; }

        public Coroutine Run(IEnumerator coroutine) { return this.monoBehaviour.StartCoroutine(coroutine); }
        
        public void Stop(Coroutine coroutine) { this.monoBehaviour.StopCoroutine(coroutine); }

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

        private class DummyBehaviour : MonoBehaviour { }
    }
}