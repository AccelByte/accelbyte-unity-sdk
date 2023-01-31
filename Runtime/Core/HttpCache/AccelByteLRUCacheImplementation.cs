// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Linq;

namespace AccelByte.Core
{
    internal class AccelByteLRUMemoryCacheImplementation<ItemT> : IAccelByteCacheImplementation<ItemT>
    {
        readonly int maxSize;
        readonly Dictionary<string, (LinkedListNode<string> node, ItemT value)> slots;
        readonly LinkedList<string> referenceHistory;

        internal string[] ReferenceHistory
        {
            get
            {
                return referenceHistory.ToArray();
            }
        }

        public AccelByteLRUMemoryCacheImplementation(int maxSize)
        {
            this.maxSize = maxSize;
            slots = new Dictionary<string, (LinkedListNode<string> node, ItemT value)>(maxSize);
            referenceHistory = new LinkedList<string>();
        }

        public bool Contains(string key)
        {
            var retval = slots.ContainsKey(key);
            return retval;
        }

        public bool Emplace(string key, ItemT item)
        {
            if (Contains(key))
            {
                MoveReferenceToHead(key);
                slots[key] = (slots[key].node, item);
                return true;
            }

            if(referenceHistory.Count >= maxSize)
            {
                var removedKey = referenceHistory.Last!.Value;
                if (string.IsNullOrEmpty(removedKey))
                {
                    throw new System.InvalidOperationException("Failed removing least used item");
                }
                slots.Remove(removedKey);
                referenceHistory.RemoveLast();
            }

            var newNode = referenceHistory.AddFirst(key);
            slots.Add(key, (newNode, item));

            return true;
        }

        public void Empty()
        {
            slots.Clear();
            referenceHistory.Clear();
        }

        public ItemT Retrieve(string key)
        {
            if (!Contains(key))
            {
                return default;
            }
            MoveReferenceToHead(key);

            var retval = slots[key];
            return retval.value;
        }

        public ItemT Peek(string key)
        {
            if (!Contains(key))
            {
                return default;
            }

            var retval = slots[key];
            return retval.value;
        }

        public bool Remove(string key)
        {
            var slotRemoveResult = slots.Remove(key);
            var referenceHistoryRemoveResult = referenceHistory.Remove(key);
            return slotRemoveResult && referenceHistoryRemoveResult;
        }

        public bool Update(string key, ItemT item)
        {
            if (!Contains(key))
            {
                return false;
            }
            slots[key] = (slots[key].node, item);

            return true;
        }

        private void MoveReferenceToHead(string key)
        {
            try
            {
                var cacheSlotReference = slots[key];
                referenceHistory.Remove(cacheSlotReference.node);
                referenceHistory.AddFirst(cacheSlotReference.node);
            }
            catch (System.Exception e)
            {
                throw new System.InvalidOperationException($"Failed moving cache head\n Error: {e.Message}");
            }
        }
    }
}
