// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class AccelByteDictionaryCacheImplementation<ItemT> : IAccelByteCacheImplementation<ItemT>
    {
        readonly Dictionary<string, ItemT> cache;

        public AccelByteDictionaryCacheImplementation()
        {
            cache = new Dictionary<string, ItemT>();
        }

        public bool Contains(string key)
        {
            var retval = cache.ContainsKey(key);
            return retval;
        }

        public bool Emplace(string key, ItemT item)
        {
            if (Contains(key))
            {
                cache.Remove(key);
            }
            cache.Add(key, item);
            return true;
        }

        public bool Update(string key, ItemT item)
        {
            return Emplace(key, item);
        }

        public void Empty()
        {
            cache.Clear();
        }

        public ItemT Retrieve(string key)
        {
            if (!Contains(key))
            {
                return default;
            }
            var retval = cache[key];
            return retval;
        }

        public ItemT Peek(string key)
        {
            return Retrieve(key);
        }

        public bool Remove(string key)
        {
            var retval = cache.Remove(key); ;
            return retval;
        }
    }
}
