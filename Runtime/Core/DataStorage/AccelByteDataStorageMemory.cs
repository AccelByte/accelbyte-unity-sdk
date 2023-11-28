// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class AccelByteDataStorageMemory : IAccelByteDataStorage
    {
        Dictionary<string, Dictionary<string, object>> cacheTable;

        public AccelByteDataStorageMemory()
        {
            cacheTable = new Dictionary<string, Dictionary<string, object>>();
        }

        public void DeleteItem(string key, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            if (!cacheTable.ContainsKey(tableName))
            {
                onDone?.Invoke(false);
                return;
            }

            Dictionary<string, object> cache = cacheTable[tableName];
            bool removeSuccess = cache.Remove(key);
            if (!removeSuccess)
            {
                onDone?.Invoke(false);
                return;
            }

            cacheTable[tableName] = cache;
            onDone?.Invoke(true);
        }

        public void GetItem<T>(string key, Action<bool, T> onDone, string tableName = "DefaultKeyValueTable")
        {
            if (!cacheTable.ContainsKey(tableName))
            {
                onDone?.Invoke(false, default(T));
                return;
            }

            Dictionary<string, object> cache = cacheTable[tableName];
            bool getSuccess = cache.TryGetValue(key, out object value);
            if (!getSuccess)
            {
                onDone?.Invoke(false, default(T));
                return;
            }

            if (value is T)
            {
                onDone?.Invoke(true, (T)value);
            }
            else
            {
                onDone?.Invoke(false, default(T));
            }
        }

        public void Reset(Action<bool> result, string tableName = "DefaultKeyValueTable")
        {
            if (!cacheTable.ContainsKey(tableName))
            {
                result?.Invoke(false);
                return;
            }

            bool deleteTableSuccess = cacheTable.Remove(tableName);
            result?.Invoke(deleteTableSuccess);
        }

        public void SaveItem(string key, object item, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            List<Tuple<string, object>> keyItems = new List<Tuple<string, object>>();
            keyItems.Add(new Tuple<string, object>(key, item));
            SaveItems(keyItems, onDone, tableName);
        }

        public void SaveItems(List<Tuple<string, object>> keyItemPairs, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            Dictionary<string, object> cache = null;
            if (cacheTable.ContainsKey(tableName))
            {
                cache = cacheTable[tableName];
            }
            else
            {
                cache = new Dictionary<string, object>();
            }

            foreach (Tuple<string, object> keyItemPair in keyItemPairs)
            {
                cache[keyItemPair.Item1] = keyItemPair.Item2;
            }

            cacheTable[tableName] = cache;
            onDone?.Invoke(true);
        }
    }
}