// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal class AccelByteCacheItem<T>
    {
        private System.DateTime expireTime;
        readonly T item;

        public AccelByteCacheItem(System.DateTime expireTime, T item)
        {
            this.expireTime = expireTime;
            this.item = item;
        }

        public System.DateTime ExpireTime
        {
            get
            {
                return expireTime;
            }
            set
            {
                expireTime = value;
            }
        }

        public T Item 
        {
            get
            {
                return item;
            }
        }
    }
}
