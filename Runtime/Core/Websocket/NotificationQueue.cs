// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;

namespace AccelByte.Core
{
    public class NotificationQueue
    {
        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
        }

        private bool isLocked;
        private Queue<string> notifications;

        public bool IsEmpty()
        {
            var retVal = notifications.Count <= 0;
            return retVal;
        }

        public bool Dequeue(out string notification)
        {
            notification = notifications.Dequeue();

            if (notification != null)
            {
                return true;
            }
            return false;
        }

        public void Enqueue(string notification)
        {
            notifications.Enqueue(notification);
        }

        internal void LockQueue()
        {
            isLocked = true;
        }

        internal void UnlockQueue()
        {
            isLocked = false;
        }

        internal void RefreshQueue()
        {
            notifications = new Queue<string>();
            isLocked = false;
        }
    }
}