// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Utils
{
    internal struct SingularCallEvent
    {
        private System.Action action;
        private bool actionIsTriggered;

        public void AddListener(System.Action newActionHook)
        {
            if (!actionIsTriggered)
            {
                action += newActionHook;
            }
            else
            {
                newActionHook?.Invoke();
            }
        }

        public void RemoveListener(System.Action removedActionHook)
        {
            action -= removedActionHook;
        }
        
        public void Invoke()
        {
            if (!actionIsTriggered)
            {
                actionIsTriggered = true;
                action?.Invoke();
            }
        }

        public static SingularCallEvent operator +(SingularCallEvent callEvent, System.Action hook)
        {
            callEvent.AddListener(hook);
            return callEvent;
        }
        
        public static SingularCallEvent operator -(SingularCallEvent callEvent, System.Action hook)
        {
            callEvent.RemoveListener(hook);
            return callEvent;
        }
    }
    
    internal struct SingularCallEvent<T>
    {
        private System.Action<T> action;
        private bool actionIsTriggered;
        private T cachedTriggerParam;

        public void AddListener(System.Action<T> newActionHook)
        {
            if (!actionIsTriggered)
            {
                action += newActionHook;
            }
            else
            {
                newActionHook?.Invoke(cachedTriggerParam);
            }
        }

        public void RemoveListener(System.Action<T> removedActionHook)
        {
            action -= removedActionHook;
        }
        
        public void Invoke(T value)
        {
            if (!actionIsTriggered)
            {
                actionIsTriggered = true;
                cachedTriggerParam = value;
                action?.Invoke(value);
            }
        }

        public static SingularCallEvent<T> operator +(SingularCallEvent<T> callEvent, System.Action<T> hook)
        {
            callEvent.AddListener(hook);
            return callEvent;
        }

        public static SingularCallEvent<T> operator -(SingularCallEvent<T> callEvent, System.Action<T> hook)
        {
            callEvent.RemoveListener(hook);
            return callEvent;
        }
    }
}